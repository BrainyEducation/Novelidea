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
using Realms;
using Xamarin.Essentials;

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

        public const string PAUSE_ICON = "pause.png";
        public const string PLAY_ICON = "play.png";

        public StoryPage(Story story)
        {
            //pull the corresponding images out of the database
            var realmFile = Realm.GetInstance(RealmConfiguration.DefaultConfiguration);
            var storyPages = realmFile.All<RealmObjects.StoryPart>().Where(x => x.StoryId.Equals(story.StoryId))
                .OrderBy(x => x.Order).ToList();

            InitializeComponent();

            PlayButton = new ImageButton()
            {
                Source = PAUSE_ICON, //set at the pause icon because this page auto-starts
                HeightRequest = 40,
                BorderColor = Color.Transparent,
                BackgroundColor = Color.Transparent,
                Margin = 20
            };

            QuizButton = new ImageButton()
            {
                Source = "Quizzes.png",
                BackgroundColor = Color.Green,
                IsVisible = false
            };

            DurationLabel = new Label
            {
                Text = "0:00",
                FontFamily = Device.RuntimePlatform == Device.Android ? "Comic.ttf#Comic" : "Comic",
                Margin = 20
            };

            //slider init
            StoryPageSlider.Maximum = story.DurationInSeconds;
            StoryPageSlider.Minimum = 0;
            StoryPageSlider.Value = 0;
            StoryPageSlider.HorizontalOptions = LayoutOptions.FillAndExpand;
            StoryPageSlider.MinimumWidthRequest = DeviceDisplay.MainDisplayInfo.Width;
            StoryPageSlider.HeightRequest = 50; // Controls size of area that can grab the slider

            //story content
            StoryImage.Source = storyPages.First().Image;
            StoryImage.MinimumWidthRequest = DeviceDisplay.MainDisplayInfo.Width * .8;
            StoryImage.Aspect = Aspect.AspectFill;

            player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Load(story.AudioClip);

            //register action to be taken once the story ends
            player.PlaybackEnded += EndPlayback;

            //this starts the audio
            player.Play();

            //timer to move the slider
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>

            {
                StoryPageSlider.Value = player.CurrentPosition;
                return true;
            });

            PlayButton.Clicked += (sender, args) =>
            {
                //toggle play/pause
                if (player.IsPlaying)
                {
                    player.Pause();
                    PlayButton.Source = PLAY_ICON;
                }
                else
                {
                    player.Play();
                    PlayButton.Source = PAUSE_ICON;
                }
            };

            QuizButton.Clicked += (sender, args) =>
            {
                //Navigation.PushAsync(new QuizPage(story.Quizzes[quizNum], story.AudioClip));
            };

            //foreach (TimeSpan key in story.PictureCues.Keys)
            //{
            //    if (key.TotalSeconds < args.NewValue)
            //    {
            //        savedTime = key;
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}
            //storyImage.Source = story.PictureCues[savedTime];
            //for (int i = 0; i < story.QuizNum; i++)
            //{
            //    if (timeStamp.CompareTo(story.Quizzes[i].PlayTime) >= 0)
            //    {
            //        quizNum++;
            //    }
            //}
            //for (int i = 0; i < story.QuizNum; i++)
            //{
            //    if (timeStamp.Equals(story.Quizzes[i].PlayTime))
            //    {
            //        player.Pause();
            //        QuizButton.IsVisible = true;
            //        playAudio = false;
            //        button.IsVisible = false;
            //        button2.IsVisible = true;
            //        Content = oldContent;
            //        storyImage.HeightRequest = 150;
            //        fullScreen = false;
            //    }
            //}
        }

        private void EndPlayback(object sender, EventArgs e)
        {
            //TODO: figure out what this ChangePage/End Of Story logic is all about
            player.Stop();
            //if (story.QuizNum > 0)
            //{
            //    ChangePage(story);
            //}
            //else
            //{
            GoBack();
            //}
        }

        // Goes to the end of story page
        protected void ChangePage(Story story)
        {
            //Navigation.PushAsync(new EndOfStory(story));
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