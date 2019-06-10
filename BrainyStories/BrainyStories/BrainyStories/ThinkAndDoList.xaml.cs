using BrainyStories.Objects;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListView view = (ListView)sender;
            if (view.SelectedItem == null)
            {
                return;
            }
            var think = (ThinkAndDo)view.SelectedItem;
            view.SelectedItem = null;
            ThinkAndDoPopup pop = new ThinkAndDoPopup(think);
            await PopupNavigation.Instance.PushAsync(pop);
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