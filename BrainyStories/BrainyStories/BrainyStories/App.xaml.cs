using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;
using Realms;
using BrainyStories.Objects;
using System.Linq;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace BrainyStories
{
    public partial class App : Application
    {
        public String UserId { get; set; }

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
            //create or retrieve the user
            var realmFile = Realm.GetInstance(RealmConfiguration.DefaultConfiguration);
            //there should only be one user per instance of the app as of August 2019
            var users = realmFile.All<User>();
            if (users.Count() < 1)
            {
                using (var transaction = realmFile.BeginWrite())
                {
                    realmFile.Add<User>(new User()
                    {
                        UserName = "New Student"
                    });
                    transaction.Commit();
                }
            }
            else
            {
                UserId = users.FirstOrDefault().UserId;
            }
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