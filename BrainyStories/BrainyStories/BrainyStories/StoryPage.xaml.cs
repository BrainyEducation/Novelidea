using System;
using System.Collections.Generic;
using System.Collections;
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
using BrainyStories.RealmObjects;

namespace BrainyStories
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    // Class for the page that displays the selected story
    public partial class StoryPage : ContentPage
    {
        private ISimpleAudioPlayer player;

        private StoryPart CurrentStoryPage = null;
        private double PreviousTime = 0;
        private string StoryId;
        public const string PAUSE_ICON = "pause.png";
        public const string PLAY_ICON = "play.png";

        public StoryPage(Story story)
        {
            StoryId = story.StoryId;
            //pull the corresponding images out of the database
            var realmFile = Realm.GetInstance(RealmConfiguration.DefaultConfiguration);
            var storyPages = realmFile.All<RealmObjects.StoryPart>().Where(x => x.StoryId.Equals(story.StoryId))
                   .OrderBy(x => x.Order).ToList();

            InitializeComponent();

            PlayButton.Source = PAUSE_ICON; //set at the pause icon because this page auto-starts
            PlayButton.HeightRequest = 40;
            PlayButton.BorderColor = Color.Transparent;
            PlayButton.BackgroundColor = Color.Transparent;
            PlayButton.Margin = 20;

            QuizButton.Source = "Quizzes.png";
            QuizButton.BackgroundColor = Color.Green;
            QuizButton.IsVisible = false;

            DurationLabel.Text = "0:00";
            DurationLabel.FontFamily = Device.RuntimePlatform == Device.Android ? "Comic.ttf#Comic" : "Comic";
            DurationLabel.Margin = 20;

            CurrentStoryPage = storyPages.First();
            //story content
            StoryImage.Source = CurrentStoryPage.Image;
            StoryImage.MinimumWidthRequest = DeviceDisplay.MainDisplayInfo.Width;
            StoryImage.Aspect = Aspect.AspectFit;

            player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Load(story.AudioClip);

            //find the story duration if we haven't already
            if (story.DurationInSeconds <= 0)
            {
                using (var transaction = realmFile.BeginWrite())
                {
                    story.DurationInSeconds = player.Duration;
                    transaction.Commit();
                }
            }

            //slider init
            StoryPageSlider.Maximum = story.DurationInSeconds;
            StoryPageSlider.Minimum = 0;
            StoryPageSlider.Value = 0;
            StoryPageSlider.HorizontalOptions = LayoutOptions.FillAndExpand;
            StoryPageSlider.MinimumWidthRequest = DeviceDisplay.MainDisplayInfo.Width - (PlayButton.Width * 4);
            StoryPageSlider.HeightRequest = 50; // Controls size of area that can grab the slider
            StoryPageSlider.ValueChanged += SliderValueChanged;

            //register action to be taken once the story ends
            player.PlaybackEnded += EndPlayback;

            //this starts the audio
            player.Play();

            var timerThread = new Thread(new ThreadStart(() =>
            //timer to move the slider
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>

            {
                var audioPosition = player.CurrentPosition;
                var progressionTime = new TimeSpan(0, 0, (int)audioPosition);
                Device.BeginInvokeOnMainThread(() =>
                {
                    StoryPageSlider.Value = audioPosition; //moves the slider along
                    DurationLabel.Text = String.Format("{0}:{1}", progressionTime.Minutes, progressionTime.Seconds.ToString("D2")); //updates the text
                });
                //check if the audio position has moved forward or backward - then see if we need to make sure the image should progress or regress
                if (audioPosition > PreviousTime && audioPosition >= CurrentStoryPage.EndTimeInSeconds)
                {
                    //progress to the next story page
                    CurrentStoryPage = storyPages.Where(x => x.Order == (CurrentStoryPage.Order + 1)).FirstOrDefault();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        StoryImage.Source = CurrentStoryPage.Image;
                    });
                }
                else if (audioPosition < PreviousTime)
                {
                    //if the current position is less than the previous one - the user has moved the slider backwards
                    var pageEndTime = -1;
                    int pageNumber = 1;
                    StoryPart currentPage;
                    //loop through the ordered storypages list until we get the first one where the end time is after our current time
                    do
                    {
                        currentPage = storyPages.Where(x => x.Order == pageNumber).FirstOrDefault();
                        pageEndTime = currentPage.EndTimeInSeconds;
                        pageNumber++;
                    } while (pageEndTime <= audioPosition);
                    CurrentStoryPage = currentPage;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        StoryImage.Source = CurrentStoryPage.Image;
                    });
                }
                //log the previous time
                PreviousTime = audioPosition;

                return true;
            })));

            timerThread.Start();

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

        private void SliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            //when the slider is dragged change the playback
            if (StoryPageSlider.Value < player.Duration)
            {
                player.Seek(StoryPageSlider.Value);
            }
        }

        private void EndPlayback(object sender, EventArgs e)
        {
            //TODO: figure out what this ChangePage/End Of Story logic is all about
            player.Stop();
            player = null;
            //go to the end story screen
            Navigation.PushAsync(new EndOfStory(StoryId));
            //if (story.QuizNum > 0)
            //{
            //    ChangePage(story);
            //}
            //else
            //{
            //GoBack();
            //}
        }

        // Returns to the previous page
        protected override bool OnBackButtonPressed()
        {
            player.Stop();
            player = null;
            return base.OnBackButtonPressed();
        }

        // Navbar methods
        // Returns to the previous page
        private async void BackClicked(object sender, EventArgs e)
        {
            player.Stop();
            player = null;
            await App.Current.MainPage.Navigation.PopAsync();
        }

        // Returns to the Home page
        private async void HomeClicked(object sender, EventArgs e)
        {
            player.Stop();
            player = null;
            await App.Current.MainPage.Navigation.PopToRootAsync();
        }
    }
}