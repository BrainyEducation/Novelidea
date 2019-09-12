using System;
using System.Linq;
using System.Threading.Tasks;
using BrainyStories.Objects;
using FFImageLoading.Forms;
using Plugin.SimpleAudioPlayer;
using Realms;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainyStories
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    // Class for the ThinkAndDo popups
    public partial class ThinkAndDoPopup : PopupPage
    {
        private ISimpleAudioPlayer player;

        //track which star was clicked last
        private static int lastClickedStarNumber;

        private static string lastClickedThinkAndDoName;

        private CachedImage playButton;

        //force them to use the parameterized constructor
        private ThinkAndDoPopup()
        {
        }

        /// <summary>
        /// This constructor creates a think and do popup for either the first or second star number
        /// </summary>
        /// <param name="thinkAndDo">Desired think and do</param>
        /// <param name="starNumber">Enter 1 for Star Number 1 or 2 for Star Number 2</param>
        public ThinkAndDoPopup(ThinkAndDo thinkAndDo, int starNumber)
        {
            InitializeComponent();
            ThinkAndDoTitle.Text = starNumber == 1 ? thinkAndDo.Text1 : thinkAndDo.Text2;
            lastClickedStarNumber = starNumber;
            lastClickedThinkAndDoName = thinkAndDo.ThinkAndDoName;

            //June 2019: moved the player init to the top of the file to be able to calculate duration later on
            ObjCRuntime.Class.ThrowOnInitFailure = false;

            player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();

            string audioFile = starNumber == 1 ? thinkAndDo.ThinkAndDoAudioClip1 : thinkAndDo.ThinkAndDoAudioClip2;

            player.Load(audioFile);

            playButton = new CachedImage()
            {
                Source = "pause.png",
            };

            Label displayLabel = new Label
            {
                Text = "0:00",
                TextColor = Color.White
            };

            Slider slider = new Slider
            {
                Maximum = player.Duration,
                Minimum = 0,
                Value = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 50 // Controls size of area that can grab the slider
            };

            CachedImage close = new CachedImage()
            {
                Source = "CloseButton",
                BackgroundColor = Color.Transparent
            };
            var closeGestureRecognizer = new TapGestureRecognizer();
            closeGestureRecognizer.Tapped += OnCloseButtonTapped;

            close.GestureRecognizers.Add(closeGestureRecognizer);

            //here is where we should add logic for what happens when playback ends
            player.PlaybackEnded += MarkAsPlayed;
            bool audioFromTimer = false;
            bool playAudio = true;
            player.Play();

            var playGestureRecognizer = new TapGestureRecognizer();
            playGestureRecognizer.Tapped += PlayButtonTapped;
            playButton.GestureRecognizers.Add(playGestureRecognizer);

            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                if (playAudio)
                {
                    audioFromTimer = true;
                    slider.Value += 1;
                }
                return true;
            });
            slider.ValueChanged += (sender, args) =>
            {
                if (player != null)
                {
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
                    var timeStamp = new TimeSpan(0, minutes, seconds);
                    audioFromTimer = false;
                }
            };
            StackLayout audio = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    playButton,
                    displayLabel,
                    close
                }
            };

            TopStack.Children.Add(slider);
            TopStack.Children.Add(audio);
        }

        private void PlayButtonTapped(object sender, EventArgs e)
        {
            //toggle play/pause
            if (player.IsPlaying)
            {
                player.Pause();
                playButton.Source = StoryPage.PLAY_ICON;
            }
            else
            {
                player.Play();
                playButton.Source = StoryPage.PAUSE_ICON;
            }
        }

        //marks a think and do as completed
        private void MarkAsPlayed(object sender, EventArgs e)
        {
            //write to the database that the think and do is completed
            using (var realmConnection = Realm.GetInstance(RealmConfiguration.DefaultConfiguration))
            {
                using (var writer = realmConnection.BeginWrite())
                {
                    ////update the prompt completion status
                    var playedTAD = realmConnection.All<ThinkAndDo>().Where(x => x.ThinkAndDoName.Equals(lastClickedThinkAndDoName)).FirstOrDefault();
                    if (lastClickedStarNumber == 1)
                    {
                        playedTAD.CompletedPrompt1 = true;
                    }
                    else
                    {
                        playedTAD.CompletedPrompt2 = true;
                    }
                    writer.Commit();
                }
            }
            CloseAllPopup();
        }

        // Returns to previous page when back button is selected
        protected override bool OnBackButtonPressed()
        {
            CloseAllPopup();
            return false;
        }

        // Returns to the previous page when an area outside the popup is clicked
        private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            CloseAllPopup();
        }

        // Returns to the previous page when an area outside the popup is clicked
        protected override bool OnBackgroundClicked()
        {
            CloseAllPopup();
            return false;
        }

        // Returns to the previous page when an area outside the popup is clicked
        private async void CloseAllPopup()
        {
            player.Stop();
            await PopupNavigation.Instance.PopAsync();
            player.Dispose();
            player = null;
        }
    }
}