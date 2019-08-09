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
        //private IEnumerable<Story> ListOfImagines;

        public Imagines()
        {
            //NavigationPage.SetHasNavigationBar(this, false);

            var imaginesData = new StoryFactory().GenerateOrGetImagines();

            InitializeComponent();

            ListOfImagines.ItemsSource = imaginesData;
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