using BrainyStories.Objects;
using Realms;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainyStories
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    // Class for the rewards page
    public partial class RewardsPage : ContentPage
    {
        public ObservableCollection<Stars> StarsToDisplay;
        private Realm RealmFile;

        //this is the number of stars that Walter wants to display on the rewards screen
        private const int STARS_ON_REWARDS_PAGE = 10;

        private const int ROWS_ON_REWARDS_PAGE = 2;

        public RewardsPage()
        {
            InitializeComponent();
            FindHowManyStarsToDisplay();

            //BindList.ItemsSource = StarsToDisplay;

            //User user = User.Instance;
            //int numOfSilverCoins = user.RewardsRecieved["Silver"];
            //int numOfGoldCoins = user.RewardsRecieved["Gold"] + (numOfSilverCoins / 2);
            //int numOfStacks = numOfGoldCoins / 5;
            //int numOfBags = numOfStacks / 5;
            //int numOfArmoredCars = numOfBags / 5;
            //int numOfBanks = numOfArmoredCars / 5;
            //numOfSilverCoins = numOfSilverCoins % 2;
            //numOfGoldCoins = numOfGoldCoins % 5;
            //numOfStacks = numOfStacks % 5;
            //numOfBags = numOfBags % 5;
            //numOfArmoredCars = numOfArmoredCars % 5;
            //for (int i = 0; i < numOfBanks; i++)
            //{
            //    Image bank = new Image() { Source = "Bank.png" };
            //    BankList.Children.Add(bank);
            //}
            //for (int i = 0; i < numOfArmoredCars; i++)
            //{
            //    Image car = new Image() { Source = "ArmoredCar.png" };
            //    CarList.Children.Add(car);
            //}
            //for (int i = 0; i < numOfBags; i++)
            //{
            //    Image bag = new Image() { Source = "MoneyBag.png" };
            //    BagList.Children.Add(bag);
            //}
            //for (int i = 0; i < numOfStacks; i++)
            //{
            //    Image stack = new Image();
            //    stack.Source = "GoldStack.png";
            //    StackList.Children.Add(stack);
            //}
            //for (int i = 0; i < numOfGoldCoins; i++)
            //{
            //    Image goldCoin = new Image() { Source = "GoldCoin.png" };
            //    GoldList.Children.Add(goldCoin);
            //}
            //for (int i = 0; i < numOfSilverCoins; i++)
            //{
            //    Image silverCoin = new Image() { Source = "SilverCoin.png" };
            //    SilverList.Children.Add(silverCoin);
            //}
        }

        private void FindHowManyStarsToDisplay()
        {
            StarsToDisplay = new ObservableCollection<Stars>();

            RealmFile = Realm.GetInstance(RealmConfiguration.DefaultConfiguration);
            var starCount = RealmFile.All<ThinkAndDo>().Count(x => x.CompletedPrompt1);
            starCount += RealmFile.All<ThinkAndDo>().Count(x => x.CompletedPrompt2);
            //this loop places a star for every star but the last one
            for (int i = 1; i < STARS_ON_REWARDS_PAGE - 1; i++)
            {
                var column = i <= (STARS_ON_REWARDS_PAGE / 2) ? i : i - (STARS_ON_REWARDS_PAGE / 2);
                column -= 1; //subtract 1 because we start i at 1
                var row = i <= (STARS_ON_REWARDS_PAGE / 2) ? 0 : 1;
                StarBlock.Children.Add(new Stars(column: column, row: row)
                {
                    Text = "⭐",
                    IsVisible = i <= starCount ? true : false,//display stars based on the number of TADs completed
                    Vibrates = false
                });
            }

            //put the last star in
            //this is the star icon
            var lastStar = new Stars(column: (STARS_ON_REWARDS_PAGE / 2) - 1, row: 1)
            {
                //last star is always visible
                IsVisible = true,
                Text = "⭐",
                Vibrates = true
            };
            lastStar.SetFontSize(Stars.MEDIUM_STAR_SIZE);

            StarBlock.Children.Add(lastStar);
            //this is the number inside the star icon
            StarBlock.Children.Add(new Stars(column: (STARS_ON_REWARDS_PAGE / 2) - 1, row: 1, textSizeMultiplier: 0.5)
            {
                IsVisible = true,
                Text = starCount.ToString(),
                Vibrates = true,
                FontAttributes = FontAttributes.Bold,
            }); ;

            //start a timer to make the last star "vibrate"
            if (starCount == STARS_ON_REWARDS_PAGE - 1)
            {
                StartStarMovement(StarBlock);
            }
        }

        private void StartStarMovement(Grid starBlock)
        {
            var starTimer = new Timer();
            starTimer.Interval = 250;
            //every time the timer elapses, switch the font size
            starTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
               {
                   //loop through the elements and increase them in size in sync
                   foreach (Stars child in starBlock.Children.Where(x => x.GetType() == typeof(Stars) && ((Stars)x).Vibrates))
                   {
                       if (child.CompareFontSize(Stars.SMALL_STAR_SIZE))
                       {
                           child.SetFontSize(Stars.MEDIUM_STAR_SIZE);
                       }
                       else if (child.CompareFontSize(Stars.MEDIUM_STAR_SIZE))
                       {
                           child.SetFontSize(Stars.LARGE_STAR_SIZE);
                       }
                       else if (child.CompareFontSize(Stars.LARGE_STAR_SIZE))
                       {
                           child.SetFontSize(Stars.SMALL_STAR_SIZE);
                       }
                   }
               });
            };
            starTimer.Start();
        }

        // Navbar methods
        // Returns to the previous page
        private async void BackClicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }

        // Returns to the Home page
        private async void HomeClicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PopToRootAsync();
        }

        protected override void OnDisappearing()
        {
            RealmFile.Dispose();
        }
    }
}