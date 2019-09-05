using BrainyStories.RealmObjects;
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

        //constructor for use when the user wants to view the screen without selecting a prize
        public PotentialPrizes()
        {
            InitializeComponent();
        }

        public PotentialPrizes(string storyId, StorySet storySet)
        {
            StoryId = storyId;
            StorySet = storySet;
            InitializeComponent();
        }

        //method to manage what happens when a prize is selected
        public async void PrizeSelected(object sender, EventArgs e)
        {
            //only react if the user is able to select a prize
            if (StoryId != String.Empty)
            {
                var selectedPrize = (ImageButton)sender;

                //confirm that the user wants this prize
                if (await ShowConfirmationDialog())
                {
                    var realmFile = Realm.GetInstance(RealmConfiguration.DefaultConfiguration);
                    //lookup the story in the database and add the selected prize
                    var story = realmFile.All<Story>().Where(x => x.StoryId.Equals(StoryId)).FirstOrDefault();
                    using (var realmTransaction = realmFile.BeginWrite())
                    {
                        //figure out how many prizes have already been selected and add to the next prize
                        switch (story.PrizesSelected)
                        {
                            case (0):
                                story.Prize1 = ((FileImageSource)selectedPrize.Source).File.ToString();
                                break;

                            case (1):
                                story.Prize2 = new Uri(selectedPrize.Source.ToString()).ToString();
                                break;

                            case (2):
                                story.Prize3 = new Uri(selectedPrize.Source.ToString()).ToString();
                                break;

                            case (3):
                                story.Prize4 = new Uri(selectedPrize.Source.ToString()).ToString();
                                break;

                            case (4):
                                story.Prize5 = new Uri(selectedPrize.Source.ToString()).ToString();
                                break;

                            default:
                                break;
                        }
                        realmTransaction.Commit();
                    }
                    realmFile.Dispose();
                    realmFile = null;
                    //return to the story/imagines
                    await Navigation.PushAsync(new Imagines(StorySet));
                }
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
    }
}