using BrainyStories.Objects;
using BrainyStories.RealmObjects;
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

        /// <summary>
        /// This class is shared between the stories and the imagines. Stories have one number set, and imagines have another
        /// </summary>
        /// <param name="storySet">Pass in the number corresponding to the imagine or story set</param>
        public Imagines(StorySet storySet)
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
        }

        private void CatClicked(object sender, EventArgs e)
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
            var senderAsImageButton = ((ImageButton)sender);
            Story callingStory = senderAsImageButton.BindingContext as Story;
            ThinkAndDo think = callingStory.ThinkAndDo;
            var starNumber = Int32.Parse(senderAsImageButton.CommandParameter.ToString());

            ThinkAndDoPopup pop = new ThinkAndDoPopup(think, starNumber);
            pop.Disappearing += PopUpClosed;

            await PopupNavigation.Instance.PushAsync(pop);
        }

        private void PopUpClosed(object sender, EventArgs e)
        {
            ListOfImagines.ItemsSource = new StoryFactory().FetchStoriesOrImagines(StorySet);
        }

        private async void ImagineClicked(object sender, ItemTappedEventArgs e)
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
    }
}