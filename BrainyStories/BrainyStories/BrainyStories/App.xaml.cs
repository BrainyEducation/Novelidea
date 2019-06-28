using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace BrainyStories
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            //StoryFactory.GenerateStoriesThread = new Thread(new ThreadStart(StoryFactory.GenerateAll));
            //StoryFactory.GenerateStoriesThread.IsBackground = true;
            //StoryFactory.GenerateStoriesThread.Start();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}