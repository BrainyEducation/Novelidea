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
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainyStories
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    // Class for the rewards page
    public partial class RewardsPage : ContentPage
    {
        public ObservableCollection<Stars> StarsToDisplay;
        private Realm RealmFile;

        //this is the number of stars that Walter wants to display on the rewards screen
        private const int STARS_ON_REWARDS_PAGE = 10;

        //the row # for the row which contains the gold star
        private const int BOTTOM_ROW = 4;

        public RewardsPage()
        {
            RealmFile = Realm.GetInstance(RealmConfiguration.DefaultConfiguration);

            //todo: remove once stories are put into place again
            StageTestData();

            InitializeComponent();
            FindHowManyStarsToDisplay();

            FindQuizScore();

            //BindList.ItemsSource = StarsToDisplay;

            //User user = User.Instance;
            //int numOfSilverCoins = user.RewardsRecieved["Silver"];
            //int numOfGoldCoins = user.RewardsRecieved["Gold"] + (numOfSilverCoins / 2);
            //int numOfStacks = numOfGoldCoins / 5;
            //int numOfBags = numOfStacks / 5;
            //int numOfArmoredCars = numOfBags / 5;
            //int numOfBanks = numOfArmoredCars / 5;
            //numOfSilverCoins = numOfSilverCoins % 2;
            //numOfGoldCoins = numOfGoldCoins % 5;
            //numOfStacks = numOfStacks % 5;
            //numOfBags = numOfBags % 5;
            //numOfArmoredCars = numOfArmoredCars % 5;
            //for (int i = 0; i < numOfBanks; i++)
            //{
            //    Image bank = new Image() { Source = "Bank.png" };
            //    BankList.Children.Add(bank);
            //}
            //for (int i = 0; i < numOfArmoredCars; i++)
            //{
            //    Image car = new Image() { Source = "ArmoredCar.png" };
            //    CarList.Children.Add(car);
            //}
            //for (int i = 0; i < numOfBags; i++)
            //{
            //    Image bag = new Image() { Source = "MoneyBag.png" };
            //    BagList.Children.Add(bag);
            //}
            //for (int i = 0; i < numOfStacks; i++)
            //{
            //    Image stack = new Image();
            //    stack.Source = "GoldStack.png";
            //    StackList.Children.Add(stack);
            //}
            //for (int i = 0; i < numOfGoldCoins; i++)
            //{
            //    Image goldCoin = new Image() { Source = "GoldCoin.png" };
            //    GoldList.Children.Add(goldCoin);
            //}
            //for (int i = 0; i < numOfSilverCoins; i++)
            //{
            //    Image silverCoin = new Image() { Source = "SilverCoin.png" };
            //    SilverList.Children.Add(silverCoin);
            //}
        }

        //stage 1 silver coin (second attempt on a quiz question)
        private void StageTestData()
        {
            var user = new User();
            user.AddSilver(313);
            //user.AddGold(100);

            RealmFile.Write(() =>
            {
                RealmFile.Add(user);
            });

            //I don't think I need the rest of this now that i put score in user
            var quizzes = new List<Quiz>();
            var questions = new List<Question>();
            var answers = new List<Answer>();

            quizzes.Add(new Quiz()
            {
                AssociatedImage = "S1_LATM_1.jpg",
                NumberOfAttempts = 2,
                QuizName = "The Lion and the Mouse Quiz 1",
            });

            questions.Add(new Question()
            {
                NumberOfAttempts = 2,
                QuestionOrder = 1,
                Audio = "SQ_1_1.mp3",
                QuestionText = "Question 1: After a big meal the lion felt drowsy and what?",
                QuizId = quizzes.LastOrDefault<Quiz>().QuizId,
            });

            //selected correctly the second time
            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "lazy",
                IsTrue = true,
                DateTimeSelected = new DateTimeOffset(new DateTime(2019, 06, 24, 12, 12, 12)),
            });
            //stage this as the incorrect answer selected first
            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "angry",
                IsTrue = false,
                DateTimeSelected = new DateTimeOffset(new DateTime(2019, 05, 24, 12, 12, 12)),
            });
            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "forgetful",
                IsTrue = false,
            });
            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "sick",
                IsTrue = false,
            });

            //gold star on this one
            questions.Add(new Question()
            {
                NumberOfAttempts = 1,
                QuestionOrder = 2,
                Audio = "SQ_1_2.mp3",
                QuestionText = "Question 2: What part of the mouse does the lion put his paw on to trap him?",
                QuizId = quizzes.LastOrDefault<Quiz>().QuizId,
            });

            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "tail",
                IsTrue = true,
                DateTimeSelected = new DateTimeOffset(new DateTime(2019, 05, 24, 12, 12, 12)),
            });
            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "ear",
                IsTrue = false,
            });
            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "foot",
                IsTrue = false,
            });
            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "nose",
                IsTrue = false,
            });

            questions.Add(new Question()
            {
                NumberOfAttempts = 0,
                QuestionOrder = 3,
                Audio = "SQ_1_3.mp3",
                QuestionText = "Question 3: The lion said the mouse ruined his what?",
                QuizId = quizzes.LastOrDefault<Quiz>().QuizId,
            });
            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "nap",
                IsTrue = true,
            });
            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "work",
                IsTrue = false,
            });
            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "hunt",
                IsTrue = false,
            });
            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "dance",
                IsTrue = false,
            });

            quizzes.Add(new Quiz()
            {
                QuizName = "The Little Red Hen Quiz 1",
                NumberOfAttempts = 0,
                AssociatedImage = "S2_LRH_0.jpg"
            });
            questions.Add(new Question()
            {
                NumberOfAttempts = 0,
                QuestionOrder = 1,
                Audio = "SQ_2_1.mp3",
                QuestionText = "Question 1: What kind of fat grain did the little" +
                " red hen find when she scratched in the yard?",
                QuizId = quizzes.LastOrDefault<Quiz>().QuizId,
            });
            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "wheat",
                IsTrue = true,
            });
            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "oats",
                IsTrue = false,
            });
            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "wild rice",
                IsTrue = false,
            });
            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "popcorn",
                IsTrue = false,
            });
            questions.Add(new Question()
            {
                NumberOfAttempts = 0,
                QuestionOrder = 2,
                Audio = "SQ_2_2.mp3",
                QuestionText = "Question 2: When the hen found the grain she thought, " +
                "I won’t eat it—I’ll do what to it?",
                QuizId = quizzes.LastOrDefault<Quiz>().QuizId,
            });
            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "plant",
                IsTrue = true,
            });
            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "burn",
                IsTrue = false,
            });
            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "hug",
                IsTrue = false,
            });
            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "throw",
                IsTrue = false,
            });
            questions.Add(new Question()
            {
                NumberOfAttempts = 0,
                QuestionOrder = 3,
                Audio = "SQ_2_2.mp3",
                QuestionText = "Question 3: What animal liked to swim?",
                QuizId = quizzes.LastOrDefault<Quiz>().QuizId,
            });
            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "duck",
                IsTrue = true,
            });
            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "dog",
                IsTrue = false,
            });
            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "hen",
                IsTrue = false,
            });
            answers.Add(new Answer()
            {
                QuestionId = questions.LastOrDefault().QuestionId,
                Text = "cat",
                IsTrue = false,
            });
        }

        private void FindQuizScore()
        {
            //if we ever make this online or allow multiple logins, we may have more than one student
            User user = RealmFile.All<User>().FirstOrDefault();
            var payout = user.GetPayout();
            //count the total gold coins
            ScoreCoinLabel.Text = user.GetTotalGoldCoins().ToString();
            //only one option for silver coin - 1 or 0
            if (payout.SilverCount > 0)
            {
                var silverCoin = new Image();
                silverCoin.Source = "SilverCoin.png";
                Grid.SetRow(silverCoin, BOTTOM_ROW);
                //five columns total - silver and gold coins go on the first column
                Grid.SetColumn(silverCoin, 0);
                PayoutGrid.Children.Add(silverCoin);
            }

            for (int i = 0; i < payout.GoldCount; i++)
            {
                var goldCoin = new Image();
                goldCoin.Source = "GoldCoin.png";
                Grid.SetRow(goldCoin, i);
                Grid.SetColumn(goldCoin, 0);
                PayoutGrid.Children.Add(goldCoin);
            }
            for (int i = 0; i < payout.StackCount; i++)
            {
                var stacks = new Image();
                stacks.Source = "GoldStack.png";
                Grid.SetRow(stacks, i);
                Grid.SetColumn(stacks, 1);
                PayoutGrid.Children.Add(stacks);
            }

            for (int i = 0; i < payout.BagCount; i++)
            {
                var bags = new Image();
                bags.Source = "MoneyBag.png";
                Grid.SetRow(bags, i);
                Grid.SetColumn(bags, 2);
                PayoutGrid.Children.Add(bags);
            }

            for (int i = 0; i < payout.CarCount; i++)
            {
                var cars = new Image();
                cars.Source = "ArmoredCar1.png";
                Grid.SetRow(cars, i);
                Grid.SetColumn(cars, 3);
                PayoutGrid.Children.Add(cars);
            }

            for (int i = 0; i < payout.BankCount; i++)
            {
                var bank = new Image();
                bank.Source = "Bank.png";
                Grid.SetRow(bank, i);
                Grid.SetColumn(bank, 4);
                PayoutGrid.Children.Add(bank);
            }
        }

        private void FindHowManyStarsToDisplay()
        {
            StarsToDisplay = new ObservableCollection<Stars>();

            var starCount = RealmFile.All<ThinkAndDo>().Count(x => x.CompletedPrompt1);
            starCount += RealmFile.All<ThinkAndDo>().Count(x => x.CompletedPrompt2);
            //this loop places a star for every star but the last one
            for (int i = 1; i < STARS_ON_REWARDS_PAGE - 1; i++)
            {
                var column = i <= ((STARS_ON_REWARDS_PAGE - 1) / 2) ? i : i - ((STARS_ON_REWARDS_PAGE - 1) / 2);
                column -= 1; //subtract 1 because we start i at 1
                var row = i <= ((STARS_ON_REWARDS_PAGE - 1) / 2) ? 0 : 1;
                StarBlock.Children.Add(new Stars(column: column, row: row)
                {
                    Text = "⭐",
                    IsVisible = i <= starCount ? true : false,//display stars based on the number of TADs completed
                    Vibrates = false
                });
            }

            //put the last star in
            //this is the star icon
            var lastStar = new Stars(column: (STARS_ON_REWARDS_PAGE / 2) - 1, row: 1)
            {
                //last star is always visible
                IsVisible = true,
                Text = "⭐",
                Vibrates = true,
            };
            lastStar.SetFontSize(Stars.MEDIUM_STAR_SIZE);
            lastStar.VerticalTextAlignment = TextAlignment.Start;

            StarBlock.Children.Add(lastStar);
            //this is the number inside the star icon
            var starLabel = new Stars(column: (STARS_ON_REWARDS_PAGE / 2) - 1, row: 1, textSizeMultiplier: 0.5)
            {
                IsVisible = true,
                Text = starCount.ToString(),
                Vibrates = true,
                FontAttributes = FontAttributes.Bold,
            };
            starLabel.VerticalTextAlignment = TextAlignment.End;
            StarBlock.Children.Add(starLabel);

            //start a timer to make the last star "vibrate"
            if (starCount == STARS_ON_REWARDS_PAGE - 1)
            {
                StartStarMovement(StarBlock);
            }
        }

        private void StartStarMovement(Grid starBlock)
        {
            var starTimer = new Timer();
            starTimer.Interval = 250;
            //every time the timer elapses, switch the font size
            starTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
               {
                   //loop through the elements and increase them in size in sync
                   foreach (Stars child in starBlock.Children.Where(x => x.GetType() == typeof(Stars) && ((Stars)x).Vibrates))
                   {
                       if (child.CompareFontSize(Stars.SMALL_STAR_SIZE))
                       {
                           child.SetFontSize(Stars.MEDIUM_STAR_SIZE);
                       }
                       else if (child.CompareFontSize(Stars.MEDIUM_STAR_SIZE))
                       {
                           child.SetFontSize(Stars.LARGE_STAR_SIZE);
                       }
                       else if (child.CompareFontSize(Stars.LARGE_STAR_SIZE))
                       {
                           child.SetFontSize(Stars.SMALL_STAR_SIZE);
                       }
                   }
               });
            };
            starTimer.Start();
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

        protected override void OnDisappearing()
        {
            RealmFile.Dispose();
        }
    }
}