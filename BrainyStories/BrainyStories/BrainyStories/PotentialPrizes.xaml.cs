using BrainyStories.RealmObjects;
using FFImageLoading.Forms;
using Plugin.SimpleAudioPlayer;
using Realms;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainyStories
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PotentialPrizes : ContentPage
    {
        private string StoryId = String.Empty;
        private StorySet StorySet;
        private Realm RealmFile;
        private ISimpleAudioPlayer player;

        //constructor for use when the user wants to view the screen without selecting a prize
        public PotentialPrizes()
        {
            InitializeComponent();
            //lookup all prizes
            RealmFile = Realm.GetInstance(RealmConfiguration.DefaultConfiguration);

            //if no prizes are in the db yet, populate the db
            var prizes = RealmFile.All<Prize>();
            if (prizes.Count() == 0)
            {
                PopulatePrizes();
                prizes = RealmFile.All<Prize>();
            }

            //populate prize screen
            foreach (var prize in prizes)
            {
                //add a content view, then insert the image and click listener inside it
                var contentView = new ContentView();
                contentView.SetValue(Grid.RowProperty, prize.Row);
                contentView.SetValue(Grid.ColumnProperty, prize.Column);

                var prizeImage = new CachedImage()
                {
                    Source = prize.Name
                };

                var gesture = new TapGestureRecognizer();
                gesture.Tapped += PrizeSelected;

                contentView.Content = prizeImage;
                contentView.GestureRecognizers.Add(gesture);

                PrizeGrid.Children.Add(contentView);
            }
        }

        public PotentialPrizes(string storyId, StorySet storySet) : this()
        {
            StoryId = storyId;
            StorySet = storySet;
        }

        //method to manage what happens when a prize is selected
        public async void PrizeSelected(object sender, EventArgs e)
        {
            //this is the prize that has been selected
            var selectedPrize = (CachedImage)((ContentView)sender).Content;

            //find the associated audio
            var prize = RealmFile.All<Prize>()
                .Where(x => x.Name == ((FileImageSource)selectedPrize.Source).File.ToString()).FirstOrDefault();

            CrossSimpleAudioPlayer.Current.Dispose();
            //play audio to describe the prize
            player = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            player.Load(prize.Audio);
            player.Volume = 1;
            player.Play();
            player.PlaybackEnded += EndAudio;

            //only react if the user is able to select a prize
            if (StoryId != String.Empty)
            {
                //confirm that the user wants this prize
                if (await ShowConfirmationDialog())
                {
                    var realmFile = Realm.GetInstance(RealmConfiguration.DefaultConfiguration);
                    //lookup the story in the database and add the selected prize
                    var story = realmFile.All<Story>().Where(x => x.StoryId.Equals(StoryId)).FirstOrDefault();
                    using (var realmTransaction = realmFile.BeginWrite())
                    {
                        var imageSourcePath = ((FileImageSource)selectedPrize.Source).File.ToString();
                        //figure out how many prizes have already been selected and add to the next prize
                        switch (story.PrizesSelected)
                        {
                            case (0):
                                story.Prize1 = imageSourcePath;
                                break;

                            case (1):
                                story.Prize2 = imageSourcePath;
                                break;

                            case (2):
                                story.Prize3 = imageSourcePath;
                                break;

                            case (3):
                                story.Prize4 = imageSourcePath;
                                break;

                            case (4):
                                story.Prize5 = imageSourcePath;
                                break;

                            default:
                                break;
                        }
                        realmTransaction.Commit();
                    }
                    realmFile.Dispose();
                    realmFile = null;
                    //return to the story/imagines
                    await Navigation.PushAsync(new Imagines(StorySet, false));
                }
            }
        }

        private void EndAudio(object sender, EventArgs e)
        {
            StopPlayer();
        }

        private void StopPlayer()
        {
            if (player != null)
            {
                player.Stop();
                player.PlaybackEnded -= EndAudio;
                player = null;
            }
        }

        /// <summary>
        /// This method allows the user to confirm that they really want the prize they selected
        /// </summary>
        private async Task<bool> ShowConfirmationDialog()
        {
            return await DisplayAlert("Prize Selected", "Are you sure this is the prize you want?", "Yes", "No");
        }

        // Navbar methods
        private async void BackClicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }

        private async void HomeClicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PopToRootAsync();
        }

        protected override void OnDisappearing()
        {
            StopPlayer();
            RealmFile.Dispose();
        }

        private void PopulatePrizes()
        {
            using (var transaction = RealmFile.BeginWrite())
            {
                var prize1 = new Prize()
                {
                    Audio = "PrizeAudio/backhoe.mp3",
                    Name = GetPrizeFilepath("Bulldozer.png"),
                    Column = 0,
                    Row = 0
                };

                RealmFile.Add(prize1);

                var prize2 = new Prize()
                {
                    Audio = "PrizeAudio/bigfoot.mp3",
                    Name = GetPrizeFilepath("Bigfoot.png"),
                    Column = 1,
                    Row = 0
                };

                RealmFile.Add(prize2);

                var prize3 = new Prize()
                {
                    Audio = "PrizeAudio/butterfly.mp3",
                    Name = GetPrizeFilepath("Butterfly.png"),
                    Column = 2,
                    Row = 0
                };

                RealmFile.Add(prize3);

                var prize4 = new Prize()
                {
                    Audio = "PrizeAudio/canoe.mp3",
                    Name = GetPrizeFilepath("Canoe.png"),
                    Column = 3,
                    Row = 0
                };

                RealmFile.Add(prize4);

                var prize5 = new Prize()
                {
                    Audio = "PrizeAudio/coins.mp3",
                    Name = GetPrizeFilepath("Gold.png"),
                    Column = 4,
                    Row = 0
                };

                RealmFile.Add(prize5);

                var prize6 = new Prize()
                {
                    Audio = "PrizeAudio/cookies.mp3",
                    Name = GetPrizeFilepath("Cookies.png"),
                    Column = 0,
                    Row = 1
                };

                RealmFile.Add(prize6);

                var prize7 = new Prize()
                {
                    Audio = "PrizeAudio/dollhouse.mp3",
                    Name = GetPrizeFilepath("Dollhouse.png"),
                    Column = 1,
                    Row = 1
                };

                RealmFile.Add(prize7);

                var prize8 = new Prize()
                {
                    Audio = "PrizeAudio/dolphin.mp3",
                    Name = GetPrizeFilepath("Dolphin.png"),
                    Column = 2,
                    Row = 1
                };

                RealmFile.Add(prize8);

                var prize9 = new Prize()
                {
                    Audio = "PrizeAudio/dragon.mp3",
                    Name = GetPrizeFilepath("Dragon.png"),
                    Column = 3,
                    Row = 1
                };

                RealmFile.Add(prize9);

                var prize10 = new Prize()
                {
                    Audio = "PrizeAudio/elephant.mp3",
                    Name = GetPrizeFilepath("Elephant.png"),
                    Column = 4,
                    Row = 1
                };

                RealmFile.Add(prize10);

                var prize11 = new Prize()
                {
                    Audio = "PrizeAudio/firetruck.mp3",
                    Name = GetPrizeFilepath("Firetruck.png"),
                    Column = 0,
                    Row = 2
                };

                RealmFile.Add(prize11);

                var prize12 = new Prize()
                {
                    Audio = "PrizeAudio/icecream.mp3",
                    Name = GetPrizeFilepath("IceCream.png"),
                    Column = 1,
                    Row = 2
                };

                RealmFile.Add(prize12);

                var prize13 = new Prize()
                {
                    Audio = "PrizeAudio/kitten.mp3",
                    Name = GetPrizeFilepath("Cat.png"),
                    Column = 2,
                    Row = 2
                };

                RealmFile.Add(prize13);

                var prize14 = new Prize()
                {
                    Audio = "PrizeAudio/mermaid.mp3",
                    Name = GetPrizeFilepath("Mermaid.png"),
                    Column = 3,
                    Row = 2
                };

                RealmFile.Add(prize14);

                var prize15 = new Prize()
                {
                    Audio = "PrizeAudio/parrot.mp3",
                    Name = GetPrizeFilepath("Parrot.png"),
                    Column = 4,
                    Row = 2
                };

                RealmFile.Add(prize15);

                var prize16 = new Prize()
                {
                    Audio = "PrizeAudio/puppy.mp3",
                    Name = GetPrizeFilepath("Dog.png"),
                    Column = 0,
                    Row = 3
                };

                RealmFile.Add(prize16);

                var prize17 = new Prize()
                {
                    Audio = "PrizeAudio/robot.mp3",
                    Name = GetPrizeFilepath("Robot.png"),
                    Column = 1,
                    Row = 3
                };

                RealmFile.Add(prize17);

                var prize18 = new Prize()
                {
                    Audio = "PrizeAudio/skateboard.mp3",
                    Name = GetPrizeFilepath("Skateboard.png"),
                    Column = 2,
                    Row = 3
                };

                RealmFile.Add(prize18);

                var prize19 = new Prize()
                {
                    Audio = "PrizeAudio/swings.mp3",
                    Name = GetPrizeFilepath("Swing.png"),
                    Column = 3,
                    Row = 3
                };

                RealmFile.Add(prize19);

                var prize20 = new Prize()
                {
                    Audio = "PrizeAudio/tent.mp3",
                    Name = GetPrizeFilepath("Tent.png"),
                    Column = 4,
                    Row = 3
                };

                RealmFile.Add(prize20);

                var prize21 = new Prize()
                {
                    Audio = "PrizeAudio/tiara.mp3",
                    Name = GetPrizeFilepath("Tiara.png"),
                    Column = 0,
                    Row = 4
                };

                RealmFile.Add(prize21);

                var prize22 = new Prize()
                {
                    Audio = "PrizeAudio/tiger.mp3",
                    Name = GetPrizeFilepath("Tiger.png"),
                    Column = 1,
                    Row = 4
                };

                RealmFile.Add(prize22);

                var prize23 = new Prize()
                {
                    Audio = "PrizeAudio/trex.mp3",
                    Name = GetPrizeFilepath("Dino.png"),
                    Column = 2,
                    Row = 4
                };

                RealmFile.Add(prize23);

                var prize24 = new Prize()
                {
                    Audio = "PrizeAudio/tricycle.mp3",
                    Name = GetPrizeFilepath("Trike.png"),
                    Column = 3,
                    Row = 4
                };

                RealmFile.Add(prize24);

                var prize25 = new Prize()
                {
                    Audio = "PrizeAudio/unicorn.mp3",
                    Name = GetPrizeFilepath("Unicorn.png"),
                    Column = 4,
                    Row = 4
                };

                RealmFile.Add(prize25);

                transaction.Commit();
            }
        }

        //on Android, I have to keep the prize images inside the Resources folder without any subfolders - on iOS, I don't
        public static string GetPrizeFilepath(string filepath)
        {
            var returnPath = $"Prizes/{filepath}";
            if (Device.RuntimePlatform.Equals(Device.Android))
            {
                return filepath;
            }
            return returnPath;
        }
    }
}