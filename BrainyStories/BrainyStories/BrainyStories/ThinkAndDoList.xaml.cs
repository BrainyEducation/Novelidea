using BrainyStories.Objects;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainyStories
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    // Class for the ThinkAndDo list page
    public partial class ThinkAndDoList : ContentPage
    {
        private ThinkAndDoFactory factory = new ThinkAndDoFactory();

        public ObservableCollection<ThinkAndDo> ListOfThinkAndDos;

        public ThinkAndDoList()
        {
            ListOfThinkAndDos = factory.generateThinkAndDos();
            InitializeComponent();
            BindList.ItemsSource = ListOfThinkAndDos;
        }

        // Lauches a ThinkAndDo popup for the selected ThinkAndDo
        private async void StarTapped(object sender, int starNumber)
        {
            ThinkAndDo think = ((ImageButton)sender).BindingContext as ThinkAndDo;

            ThinkAndDoPopup pop = new ThinkAndDoPopup(think, starNumber);
            pop.Disappearing += PopUpClosed;
            await PopupNavigation.Instance.PushAsync(pop);
        }

        //refreshes the stars after the popup is closed
        private void PopUpClosed(object sender, EventArgs e)
        {
            ListOfThinkAndDos = factory.generateThinkAndDos();
            BindList.ItemsSource = ListOfThinkAndDos;
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

        private void Star1Clicked(object sender, EventArgs e)
        {
            StarTapped(sender, 1);
        }

        private void Star2Clicked(object sender, EventArgs e)
        {
            StarTapped(sender, 2);
        }
    }
}