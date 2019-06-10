using BrainyStories.Objects;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainyStories
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    // Class for the rewards page
    public partial class RewardsPage : ContentPage
    {
        public RewardsPage()
        {
            InitializeComponent();

            User user = User.Instance;
            int numOfSilverCoins = user.RewardsRecieved["Silver"];
            int numOfGoldCoins = user.RewardsRecieved["Gold"] + (numOfSilverCoins / 2);
            int numOfStacks = numOfGoldCoins / 5;
            int numOfBags = numOfStacks / 5;
            int numOfArmoredCars = numOfBags / 5;
            int numOfBanks = numOfArmoredCars / 5;
            numOfSilverCoins = numOfSilverCoins % 2;
            numOfGoldCoins = numOfGoldCoins % 5;
            numOfStacks = numOfStacks % 5;
            numOfBags = numOfBags % 5;
            numOfArmoredCars = numOfArmoredCars % 5;
            for (int i = 0; i < numOfBanks; i++)
            {
                Image bank = new Image() { Source = "Bank.png" };
                BankList.Children.Add(bank);
            }
            for (int i = 0; i < numOfArmoredCars; i++)
            {
                Image car = new Image() { Source = "ArmoredCar.png" };
                CarList.Children.Add(car);
            }
            for (int i = 0; i < numOfBags; i++)
            {
                Image bag = new Image() { Source = "MoneyBag.png" };
                BagList.Children.Add(bag);
            }
            for (int i = 0; i < numOfStacks; i++)
            {
                Image stack = new Image();
                stack.Source = "GoldStack.png";
                StackList.Children.Add(stack);
            }
            for (int i = 0; i < numOfGoldCoins; i++)
            {
                Image goldCoin = new Image() { Source = "GoldCoin.png" };
                GoldList.Children.Add(goldCoin);
            }
            for (int i = 0; i < numOfSilverCoins; i++)
            {
                Image silverCoin = new Image() { Source = "SilverCoin.png" };
                SilverList.Children.Add(silverCoin);
            }
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
    }
}