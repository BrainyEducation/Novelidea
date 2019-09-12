using BrainyStories.Objects;
using BrainyStories.RealmObjects;
using FFImageLoading.Forms;
using Plugin.SimpleAudioPlayer;
using Realms;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainyStories
{
    public class BindableStackLayout : StackLayout
    {
        private readonly Label header;

        public BindableStackLayout()
        {
            header = new Label();
            Children.Add(header);
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(BindableStackLayout),
                                    propertyChanged: (bindable, oldValue, newValue) => ((BindableStackLayout)bindable).PopulateItems());

        public DataTemplate ItemDataTemplate
        {
            get { return (DataTemplate)GetValue(ItemDataTemplateProperty); }
            set { SetValue(ItemDataTemplateProperty, value); }
        }

        public static readonly BindableProperty ItemDataTemplateProperty =
            BindableProperty.Create(nameof(ItemDataTemplate), typeof(DataTemplate), typeof(BindableStackLayout));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(BindableStackLayout),
                                    propertyChanged: (bindable, oldValue, newValue) => ((BindableStackLayout)bindable).PopulateHeader());

        private void PopulateItems()
        {
            if (ItemsSource == null) return;
            foreach (var item in ItemsSource)
            {
                var itemTemplate = ItemDataTemplate.CreateContent() as View;
                itemTemplate.BindingContext = item;
                Children.Add(itemTemplate);
            }
        }

        private void PopulateHeader() => header.Text = Title;
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Imagines : ContentPage
    {
        private StorySet StorySet;

        //used for prize accouncement playback
        private ISimpleAudioPlayer player;

        /// <summary>
        /// This class is shared between the stories and the imagines. Stories have one number set, and imagines have another
        /// </summary>
        /// <param name="storySet">Pass in the number corresponding to the imagine or story set</param>
        /// <param name="displayDescription">This boolean defaults to true. If false, display the rewards instead of the description</param>
        public Imagines(StorySet storySet, bool displayDescription = true)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            StorySet = storySet;

            var imaginesData = new StoryFactory().FetchStoriesOrImagines(StorySet);

            InitializeComponent();

            if (StorySet == StorySet.Imagines)
            {
                StoryMenuLabel.Text = "Imagines";
            }
            else if (StorySet == StorySet.StorySet1)
            {
                StoryMenuLabel.Text = "Stories";
            }

            ListOfImagines.ItemsSource = imaginesData;
            //set the total number of prizes
            TotalPrizeCount.Text = GetTotalPrizeCount();
            if (!displayDescription)
            {
                ToggleRewardsScreen();
            }
        }

        private String GetTotalPrizeCount()
        {
            var count = 0;
            foreach (var story in (IEnumerable<Story>)ListOfImagines.ItemsSource)
            {
                count += story.PrizesSelected;
            }
            return Environment.NewLine + count.ToString();
        }

        private void CatClicked(object sender, EventArgs e)
        {
            ToggleRewardsScreen();
        }

        private void ToggleRewardsScreen()
        {
            var newItemSource = new List<Story>();
            //switch to the prize grid view when the student clicks a button to view their prizes
            foreach (var story in (IEnumerable<Story>)ListOfImagines.ItemsSource)
            {
                story.AreRewardsVisible = !story.AreRewardsVisible;
                newItemSource.Add(story);
            }
            ListOfImagines.ItemsSource = newItemSource;
        }

        //view the screen that displays all potential prizes
        private async void PrizeTapped(object sender, EventArgs e)
        {
            //load screen of potential prizes
            await Navigation.PushAsync(new PotentialPrizes());
        }

        // Lauches a ThinkAndDo popup for the selected ThinkAndDo
        private async void StarTapped(object sender, EventArgs e)
        {
            var contentView = ((ContentView)sender);
            //there should only be one tap gesture recognizer
            var tapGestureRecognizer = (TapGestureRecognizer)contentView.GestureRecognizers.FirstOrDefault();
            var senderAsImageButton = (CachedImage)contentView.Content;
            Story callingStory = senderAsImageButton.BindingContext as Story;
            ThinkAndDo think = callingStory.ThinkAndDo;
            var starNumber = Int32.Parse(tapGestureRecognizer.CommandParameter.ToString());

            ThinkAndDoPopup pop = new ThinkAndDoPopup(think, starNumber);
            pop.Disappearing += PopUpClosed;

            await PopupNavigation.Instance.PushAsync(pop);
        }

        private void PlayPrizeAudio(object sender, EventArgs e)
        {
            var imageButton = (CachedImage)((ContentView)sender).Content;

            var realmFile = Realm.GetInstance(RealmConfiguration.DefaultConfiguration);
            var prize = realmFile.All<Prize>()
                .Where(x => x.Name.Equals(((FileImageSource)imageButton.Source).File.ToString())).FirstOrDefault();
            //play audio to describe the prize
            player = CrossSimpleAudioPlayer.Current;
            player.Load(prize.Audio);
            player.Play();
            player.PlaybackEnded += EndAudio;
        }

        private void EndAudio(object sender, EventArgs e)
        {
            if (player != null)
            {
                player.Stop();
                player.PlaybackEnded -= EndAudio;
                player = null;
            }
        }

        private void PopUpClosed(object sender, EventArgs e)
        {
            ListOfImagines.ItemsSource = new StoryFactory().FetchStoriesOrImagines(StorySet);
        }

        private async void ImagineClicked(object sender, EventArgs e)
        {
            ListView view = (ListView)sender;
            if (view.SelectedItem == null)
            {
                return;
            }
            var story = (Story)view.SelectedItem;
            view.SelectedItem = null;
            await Navigation.PushAsync(new StoryPage(story));
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

        /// <summary>
        /// This is used to stage data for testing - this shouldn't be accessible when this goes to production
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StagePrizes(object sender, EventArgs e)
        {
            var realmFile = Realm.GetInstance(RealmConfiguration.DefaultConfiguration);
            //lookup the story in the database and add the selected prize
            var stories = realmFile.All<Story>();
            using (var realmTransaction = realmFile.BeginWrite())
            {
                foreach (var story in stories)
                {
                    story.Prize1 = story.Prize2 = story.Prize3 = story.Prize4 = story.Prize5 = String.Empty;
                    //random number of prizes between 0 and 5
                    var randomPrizeCount = new Random().Next(0, 6);
                    switch (randomPrizeCount)
                    {
                        case 5:
                            story.Prize5 = PotentialPrizes.GetPrizeFilepath("Bigfoot.png");
                            goto case 4;
                        case 4:
                            story.Prize4 = PotentialPrizes.GetPrizeFilepath("Cat.png");
                            goto case 3;
                        case 3:
                            story.Prize3 = PotentialPrizes.GetPrizeFilepath("Canoe.png");
                            goto case 2;
                        case 2:
                            story.Prize2 = PotentialPrizes.GetPrizeFilepath("Dolphin.png");
                            goto case 1;
                        case 1:
                            story.Prize1 = PotentialPrizes.GetPrizeFilepath("Firetruck.png");
                            break;

                        default:
                            break;
                    }
                }
                realmTransaction.Commit();
            }
        }
    }
}