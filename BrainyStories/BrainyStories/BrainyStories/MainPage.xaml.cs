using BrainyStories.RealmObjects;
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
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // Imagines TOC
        // Argument: True = Imagines, False = Stories
        private async void ImaginesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Imagines(storySet: StorySet.Imagines));
        }

        // Stories Table of Contents Page Button
        // Argument: True = Imagines, False = Stories
        private async void StoriesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Imagines(storySet: StorySet.StorySet1));
        }

        // Think and Do List Page Button
        private async void ThinkAndDoClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ThinkAndDoList());
        }

        // Quiz List Page Button
        private async void QuizzesClicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new QuizList());
        }

        // Progress Page Button
        private async void ProgressClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProgressPage());
        }

        // Rewards Page button
        private async void RewardsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RewardsPage());
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