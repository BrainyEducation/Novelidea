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

    // Class for the list of quizzes page
    public partial class QuizList : ContentPage
    {
        private QuizFactory factory = new QuizFactory();
        private StoryFactory factoryStories = new StoryFactory();

        public ObservableCollection<Quiz> ListOfQuizzes = new ObservableCollection<Quiz>();
        private ObservableCollection<Story> ListOfStories = new ObservableCollection<Story>();

        public QuizList()
        {
            ListOfStories = StoryFactory.Stories;
            for (int i = 0; i < ListOfStories.Count; i++)
            {
                ObservableCollection<Quiz> returnQuizzes = factory.GenerateQuizzes(ListOfStories.ElementAt(i).Name);
                foreach (Quiz quiz in returnQuizzes)
                {
                    ListOfQuizzes.Add(quiz);
                }
            }
            InitializeComponent();
            BindList.ItemsSource = ListOfQuizzes;
        }

        // Launches a quiz page for selected quiz
        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListView view = (ListView)sender;
            if (view.SelectedItem == null)
            {
                return;
            }
            var quiz = (Quiz)view.SelectedItem;
            view.SelectedItem = null;
            await Navigation.PushAsync(new QuizPage(quiz));
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