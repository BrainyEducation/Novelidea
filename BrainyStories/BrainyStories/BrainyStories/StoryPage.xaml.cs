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
using FFImageLoading.Forms;
using UIKit;

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
        private StorySet StorySet;
        public const string PAUSE_ICON = "pause.png";
        public const string PLAY_ICON = "play.png";

        //this is the percentage of the story that must be completed to be counted as read
        private const double COMPLETION_THRESHOLD = .95;

        //user this variable to track the start and stop time for this story
        private UserStoryReads UserStoryTransaction;

        private IEnumerable<StoryPart> StoryPages;
        private Story Story;

        //once the desired story % is read, mark this boolean true
        private bool StoryMarkedRead = false;

        public StoryPage(Story story)
        {
            Story = story;
            StoryId = story.StoryId;
            StorySet = story.StorySetAsEnum;
            //pull the corresponding images out of the database
            var realmFile = Realm.GetInstance(RealmConfiguration.DefaultConfiguration);
            StoryPages = realmFile.All<RealmObjects.StoryPart>().Where(x => x.StoryId.Equals(story.StoryId))
                   .OrderBy(x => x.Order).ToList();
            //get the current user's ID - if we ever want multiple users per device, we'll have to store the user's id in RAM
            var userId = realmFile.All<User>().FirstOrDefault().UserId;

            //init user story transaction
            using (var writer = realmFile.BeginWrite())
            {
                UserStoryTransaction = new UserStoryReads();
                UserStoryTransaction.StoryId = StoryId;
                UserStoryTransaction.UserId = userId;
                UserStoryTransaction.StartReadTime = DateTime.UtcNow;

                //add to the db
                realmFile.Add<UserStoryReads>(UserStoryTransaction);

                writer.Commit();
            }

            InitializeComponent();

            PlayButton.Source = PAUSE_ICON; //set at the pause icon because this page auto-starts

            //September 2019: quiz button will be useful in future releases of the app
            //QuizButton.Source = "Quizzes.png";
            //QuizButton.BackgroundColor = Color.Green;
            //QuizButton.IsVisible = false;

            if (Device.RuntimePlatform.Equals(Device.Android))
            {
                BackButton.WidthRequest = 50;
                HomeButton.WidthRequest = 50;
            }

            DurationLabel.Text = "0:00";
            DurationLabel.FontFamily = Device.RuntimePlatform == Device.Android ? "Comic.ttf#Comic" : "Comic";
            DurationLabel.Margin = 20;

            CurrentStoryPage = StoryPages.First();
            //story content
            StoryImage.Source = CurrentStoryPage.Image;
            StoryImage.Aspect = Aspect.Fill;

            player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();

            player.Load(story.AudioClip);
            player.Volume = 1;
            //find the story duration if we haven't already
            if (story.DurationInSeconds <= 0)
            {
                using (var transaction = realmFile.BeginWrite())
                {
                    story.DurationInSeconds = player.Duration;
                    transaction.Commit();
                }
            }

            //kill the realm file to help with memory consumption
            realmFile.Dispose();
            realmFile = null;

            //slider init
            StoryPageSlider.Maximum = story.DurationInSeconds;
            StoryPageSlider.Minimum = 0;
            StoryPageSlider.Value = 0;
            StoryPageSlider.HorizontalOptions = LayoutOptions.FillAndExpand;
            StoryPageSlider.HeightRequest = 50; // Controls size of area that can grab the slider
            //use drag completed instead of value changed to avoid "stuttering" audio
            StoryPageSlider.ValueChanged += UserDraggedSlider;

            //register action to be taken once the story ends
            player.PlaybackEnded += EndPlayback;
            player.Loop = false;

            //start the player in a different thread
            var playerThread = new Thread(new ThreadStart(() =>
            {
                //this starts the audio
                player.Play();
            }));
            playerThread.Start();

            RefreshStoryPagesTimer();

            //QuizButton.Clicked += (sender, args) =>
            //{
            //    //Navigation.PushAsync(new QuizPage(story.Quizzes[quizNum], story.AudioClip));
            //};
        }

        private void RefreshStoryPagesTimer()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                //track how much of the story the user has listened to (not just how much has been progressed with the slider)
                var secondsRead = 0;
                //reset the story - start out as not read
                StoryMarkedRead = false;
                //timer to track story progress and swap pages
                Device.StartTimer(new TimeSpan(0, 0, 1), () =>
                {
                    if (player != null)
                    {
                        var audioPosition = player.CurrentPosition;
                        var progressionTime = new TimeSpan(0, 0, (int)audioPosition);

                        //check if the audio position has moved forward or backward - then see if we need to make sure the image should progress or regress
                        if (audioPosition > PreviousTime)
                        {
                            secondsRead++;

                            //after 90% of the story has been read, mark as read if not marked as read already
                            if (!StoryMarkedRead && secondsRead >= (Story.DurationInSeconds * COMPLETION_THRESHOLD))
                            {
                                MarkAsRead();
                                StoryMarkedRead = true;
                            }
                            //if the page has been completed, go to the next one
                            else if (audioPosition >= CurrentStoryPage.EndTimeInSeconds)
                            {
                                //progress to the next story page
                                CurrentStoryPage = StoryPages.Where(x => x.EndTimeInSeconds >= audioPosition
                                    && x.StartTimeInSeconds <= audioPosition).FirstOrDefault();
                                StoryImage.Source = CurrentStoryPage.Image;
                                GC.Collect();
                            }
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
                                currentPage = StoryPages.Where(x => x.Order == pageNumber).FirstOrDefault();
                                pageEndTime = currentPage.EndTimeInSeconds;
                                pageNumber++;
                            } while (pageEndTime <= audioPosition);
                            CurrentStoryPage = currentPage;

                            StoryImage.Source = CurrentStoryPage.Image;
                        }
                        StoryPageSlider.Value = audioPosition;
                        //update the timestamp text
                        DurationLabel.Text = String.Format("{0}:{1}", progressionTime.Minutes,
                            progressionTime.Seconds.ToString("D2"));

                        //log the previous time
                        PreviousTime = audioPosition;
                        return true;
                    }
                    else
                    {
                        //stop the timer when the player object is null
                        return false;
                    }
                });
            });
        }

        //helper function mark the story as read in the UserStory Transaction table
        private void MarkAsRead()
        {
            var realm = Realm.GetInstance(RealmConfiguration.DefaultConfiguration);

            //mark this story as read if it hasn't been already
            using (var transaction = realm.BeginWrite())
            {
                UserStoryTransaction.EndReadTime = DateTime.UtcNow;
                transaction.Commit();
            }
            realm.Dispose();
            realm = null;
        }

        private void UserDraggedSlider(object sender, EventArgs e)
        {
            var newSliderPosition = StoryPageSlider.Value;
            //when the slider is dragged (as determined by moving more than 2 seconds) change the playback
            if (Math.Abs(PreviousTime - newSliderPosition) > 2 && newSliderPosition < player.Duration)
            {
                player.Seek(StoryPageSlider.Value);
            }
        }

        private void EndPlayback(object sender, EventArgs e)
        {
            //if the user has listened to enough of the story to win a prize, show that screen
            if (StoryMarkedRead)
            {
                //go to the prize screen
                Navigation.PushAsync(new PotentialPrizes(StoryId, StorySet));
            }
            else
            {
                //otherwise, return to the imagines screen
                Navigation.PushAsync(new Imagines(StorySet));
            }
        }

        private void PlayButtonClicked(object sender, EventArgs e)
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
        }

        // Returns to the previous page
        protected override bool OnBackButtonPressed()
        {
            EndPlayer();
            return base.OnBackButtonPressed();
        }

        // Navbar methods
        // Returns to the previous page
        private async void BackClicked(object sender, EventArgs e)
        {
            EndPlayer();
            await App.Current.MainPage.Navigation.PopAsync();
        }

        // Returns to the Home page
        private async void HomeClicked(object sender, EventArgs e)
        {
            EndPlayer();
            await App.Current.MainPage.Navigation.PopToRootAsync();
        }

        private void EndPlayer()
        {
            if (player != null)
            {
                player.Stop();
                player.PlaybackEnded -= EndPlayback;
                player.Dispose();
                player = null;
            }
        }

        /// <summary>
        /// gets called when a new screen is loaded
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            EndPlayer();
            CurrentStoryPage = null;
            Story = null;
            StoryPages = null;
            UserStoryTransaction = null;
            System.GC.Collect();
        }
    }
}