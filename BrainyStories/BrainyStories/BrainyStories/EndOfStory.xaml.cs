using BrainyStories.Objects;
using BrainyStories.RealmObjects;
using Realms;
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

    // Class for the page at the end of a completed classic story
    public partial class EndOfStory : ContentPage
    {
        public ObservableCollection<ThinkAndDo> ListOfThinkAndDos;
        private Quiz last;

        public EndOfStory(String storyId)
        {
            var realm = Realm.GetInstance(RealmConfiguration.DefaultConfiguration);
            //get the current user's ID - if we ever want multiple users per device, we'll have to store the user's id in RAM
            var userId = realm.All<User>().FirstOrDefault().UserId;

            //mark this story as read if it hasn't been already
            using (var transaction = realm.BeginWrite())
            {
                realm.Add<UserStoryReads>(new UserStoryReads()
                {
                    StoryId = storyId,
                    Timestamp = DateTime.UtcNow,
                    UserId = userId,
                });
                transaction.Commit();
            }

            InitializeComponent();
            //BindThinkAndDoList.ItemsSource = ListOfThinkAndDos;
            //Label displayLabel = new Label
            //{
            //    Text = last.QuizName,
            //    FontFamily = "ComicSansMS", //Device.RuntimePlatform == Device.Android ? "Comic.ttf#Comic" : "Comic",
            //    VerticalOptions = LayoutOptions.Center,
            //    FontSize = 20
            //};
        }

        // Launches a ThinkAndDo popup for selected activity
        private async void OnTaskTapped(object sender, ItemTappedEventArgs e)
        {
            ListView view = (ListView)sender;
            var think = (ThinkAndDo)view.SelectedItem;
            //todo: fix starNumber
            ThinkAndDoPopup pop = new ThinkAndDoPopup(think, 1);
            await PopupNavigation.Instance.PushAsync(pop);
        }

        // Launches a quiz page for selected quiz
        private async void OnQuizTapped(object sender, EventArgs e)
        {
            //todo: wire this up once quizzes are done
            //await Navigation.PushAsync(new QuizPage(last));
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