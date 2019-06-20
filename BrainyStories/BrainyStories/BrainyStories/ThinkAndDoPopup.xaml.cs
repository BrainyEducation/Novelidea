using System;
using System.Linq;
using System.Threading.Tasks;
using BrainyStories.Objects;
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
            player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            string audioFile = starNumber == 1 ? thinkAndDo.ThinkAndDoAudioClip1 : thinkAndDo.ThinkAndDoAudioClip2;
            player.Load(audioFile);

            ImageButton button = new ImageButton()
            {
                Source = "pause.png",
                BackgroundColor = Color.Transparent
            };
            ImageButton button2 = new ImageButton()
            {
                Source = "play.png",
                IsVisible = false,
                BackgroundColor = Color.Transparent
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
            ImageButton close = new ImageButton()
            {
                Source = "CloseButton",
                BackgroundColor = Color.Transparent
            };
            close.Clicked += (sender, args) =>
            {
                player.Stop();
                CloseAllPopup();
            };

            //here is where we should add logic for what happens when playback ends
            player.PlaybackEnded += MarkAsPlayed;
            bool audioFromTimer = false;
            bool playAudio = true;
            player.Play();
            button.Clicked += (sender, args) =>
            {
                player.Pause();
                playAudio = false;
                button.IsVisible = false;
                button2.IsVisible = true;
            };
            button2.Clicked += (sender, args) =>
            {
                player.Play();
                playAudio = true;
                button.IsVisible = true;
                button2.IsVisible = false;
            };
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
            };
            StackLayout audio = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    button,
                    button2,
                    displayLabel,
                    close
                }
            };

            TopStack.Children.Add(slider);
            TopStack.Children.Add(audio);
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
        }

        // Returns to previous page when back button is selected
        protected override bool OnBackButtonPressed()
        {
            player.Stop();
            return false;
        }

        // Returns to the previous page when an area outside the popup is clicked
        private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            player.Stop();
            CloseAllPopup();
        }

        // Returns to the previous page when an area outside the popup is clicked
        protected override bool OnBackgroundClicked()
        {
            player.Stop();
            CloseAllPopup();
            return false;
        }

        // Returns to the previous page when an area outside the popup is clicked
        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}