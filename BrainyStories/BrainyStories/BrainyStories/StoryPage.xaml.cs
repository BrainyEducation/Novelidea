using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Plugin.SimpleAudioPlayer;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using BrainyStories.Objects;

namespace BrainyStories
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    // Class for the page that displays the selected story
    public partial class StoryPage : ContentPage
    {
        private ISimpleAudioPlayer player;
        private int quizNum = -1;

        private bool fullScreen = false;

        private View oldContent = null;

        private PinchGestureRecognizer pinchGesture = new PinchGestureRecognizer();
        private static TimeSpan timeStamp = new TimeSpan(0, 0, 0);

        public StoryPage(Story story)
        {
            InitializeComponent();
            ImageButton button = new ImageButton()
            {
                Source = "pause.png",
                HeightRequest = 40,
                BorderColor = Color.Transparent,
                BackgroundColor = Color.Transparent,
                Margin = 20
            };
            ImageButton button2 = new ImageButton()
            {
                Source = "play.png",
                IsVisible = false,
                HeightRequest = 40,
                BorderColor = Color.Transparent,
                BackgroundColor = Color.Transparent,
                Margin = 20
            };
            ImageButton Expand = new ImageButton()
            {
                Source = "expand.png",
                HorizontalOptions = LayoutOptions.EndAndExpand,
                HeightRequest = 40,
                BorderColor = Color.Transparent,
                BackgroundColor = Color.Transparent,
                Margin = 20
            };
            ImageButton QuizButton = new ImageButton()
            {
                Source = "Quizzes.png",
                BackgroundColor = Color.Green,
                IsVisible = false
            };

            Label displayLabel = new Label
            {
                Text = "0:00",
                FontFamily = Device.RuntimePlatform == Device.Android ? "Comic.ttf#Comic" : "Comic",
                Margin = 20
            };
            Slider slider = new Slider
            {
                Maximum = story.Duration.Seconds + (story.Duration.Minutes * 60),
                Minimum = 0,
                Value = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 50 // Controls size of area that can grab the slider
            };
            Image storyImage = new Image() { Source = story.PictureCues[new TimeSpan(0, 0, 0)], HeightRequest = 150 };
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                if (oldContent != null)
                {
                    Content = oldContent;
                    storyImage.HeightRequest = 150;
                    fullScreen = false;
                }
            };
            storyImage.GestureRecognizers.Add(tapGestureRecognizer);

            Expand.Clicked += (sender, args) =>
            {
                if (!fullScreen)
                {
                    Content = storyImage;
                    fullScreen = true;
                }
            };
            player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Load(story.AudioClip);
            bool audioFromTimer = false;
            bool playAudio = true;
            player.Play();
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
              {
                  if (playAudio)
                  {
                      audioFromTimer = true;
                      slider.Value += 1;
                  }
                  if (slider.Value == story.Duration.Seconds + (story.Duration.Minutes * 60))
                  {
                      player.Stop();
                      if (story.QuizNum > 0)
                      {
                          ChangePage(story);
                      }
                      else
                      {
                          GoBack();
                      }

                      return false;
                  }

                  return true;
              });
            button.Clicked += (sender, args) =>
            {
                player.Pause();
                playAudio = false;
                button.IsVisible = false;
                button2.IsVisible = true;
            };
            button2.Clicked += (sender, args) =>
            {
                player.Seek(timeStamp.TotalSeconds);
                player.Play();

                playAudio = true;
                button.IsVisible = true;
                button2.IsVisible = false;
            };
            QuizButton.Clicked += (sender, args) =>
            {
                Navigation.PushAsync(new QuizPage(story.Quizzes[quizNum], story.AudioClip));
                QuizButton.IsVisible = false;
                button.IsVisible = false;
                button2.IsVisible = true;
            };
            slider.ValueChanged += (sender, args) =>
            {
                QuizButton.IsVisible = false;
                int minutes = (int)args.NewValue / 60;
                int seconds = (int)args.NewValue - (minutes * 60);
                Console.WriteLine(args.NewValue);
                Console.WriteLine(player.CurrentPosition);
                Console.WriteLine(args.NewValue);
                if (!audioFromTimer)
                {
                    player.Seek(args.NewValue);
                }
                String second = seconds.ToString();
                if (seconds < 10)
                {
                    second = '0' + seconds.ToString();
                }
                displayLabel.Text = String.Format("{0}:{1}", minutes, second);
                timeStamp = new TimeSpan(0, minutes, seconds);
                var savedTime = new TimeSpan(0, 0, 0);
                foreach (TimeSpan key in story.PictureCues.Keys)
                {
                    if (key.TotalSeconds < args.NewValue)
                    {
                        savedTime = key;
                    }
                    else
                    {
                        break;
                    }
                }
                storyImage.Source = story.PictureCues[savedTime];
                quizNum = -1;
                for (int i = 0; i < story.QuizNum; i++)
                {
                    if (timeStamp.CompareTo(story.Quizzes[i].PlayTime) >= 0)
                    {
                        quizNum++;
                    }
                }
                for (int i = 0; i < story.QuizNum; i++)
                {
                    if (timeStamp.Equals(story.Quizzes[i].PlayTime))
                    {
                        player.Pause();
                        QuizButton.IsVisible = true;
                        playAudio = false;
                        button.IsVisible = false;
                        button2.IsVisible = true;
                        Content = oldContent;
                        storyImage.HeightRequest = 150;
                        fullScreen = false;
                    }
                }
                audioFromTimer = false;
            };

            StackLayout audio = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    button,
                    button2,
                    displayLabel,
                    QuizButton,
                    Expand
                }
            };

            TopStack.Children.Add(storyImage);
            TopStack.Children.Add(slider);
            TopStack.Children.Add(audio);
            oldContent = Content;
        }

        // Goes to the end of story page
        protected void ChangePage(Story story)
        {
            Navigation.PushAsync(new EndOfStory(story));
        }

        // Returns to the previous page
        private async void GoBack()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }

        // Returns to the previous page
        protected override bool OnBackButtonPressed()
        {
            player.Stop();
            return base.OnBackButtonPressed();
        }

        // Navbar methods
        // Returns to the previous page
        private async void BackClicked(object sender, EventArgs e)
        {
            player.Stop();
            await App.Current.MainPage.Navigation.PopAsync();
        }

        // Returns to the Home page
        private async void HomeClicked(object sender, EventArgs e)
        {
            player.Stop();
            await App.Current.MainPage.Navigation.PopToRootAsync();
        }
    }
}