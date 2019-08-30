using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

using System.Linq;
using BrainyStories.RealmObjects;
using Realms;

namespace BrainyStories.Objects
{
    // User class to organize the student's progress
    public class User : RealmObject
    {
        public String UserId { get; set; } = Guid.NewGuid().ToString();
        public double Score { get; private set; } = 0;

        public String UserName { get; set; } = String.Empty;

        //multiply by this number depending on how many stories walter adds or removes
        //the base metrics are based off 240 total gold coins (10 stories/480 points)
        //walter dropped the last 5 stories, so we are back to 90 questions (90 gold coins/ 180points)
        //180/480 = 0.375
        private const double SCALING_FACTOR = 1;// 0.375;

        /*shows how many points each is worth*/
        private const double POINTS_PER_SILVER = 1;
        private const double POINTS_PER_GOLD = POINTS_PER_SILVER * 2;
        private const double POINTS_PER_STACK = POINTS_PER_GOLD * 5;
        private const double POINTS_PER_BAG = POINTS_PER_STACK * 5;
        private const double POINTS_PER_CAR = POINTS_PER_BAG * 4;
        private const double POINTS_PER_BANK = POINTS_PER_CAR * 2;

        public User() //, ObservableCollection<Story> StoriesRead)
        {
            //this.RewardsRecieved = RewardsRecieved;
            //this.Name = Name;
            //this.StoriesRead = StoriesRead;
        }

        public void AddSilver(int silverCoinsAdded)
        {
            Score += (silverCoinsAdded * POINTS_PER_SILVER * SCALING_FACTOR);
        }

        public void AddGold(int goldCoinsAdded)
        {
            Score += (goldCoinsAdded * POINTS_PER_GOLD * SCALING_FACTOR);
        }

        public Payout GetPayout()
        {
            var payout = new Payout();
            double remainingScore = Score;
            payout.BankCount = (int)(remainingScore / (POINTS_PER_BANK * SCALING_FACTOR));
            remainingScore -= payout.BankCount * (POINTS_PER_BANK * SCALING_FACTOR);
            payout.CarCount = (int)(remainingScore / (POINTS_PER_CAR * SCALING_FACTOR));
            remainingScore -= payout.CarCount * (POINTS_PER_CAR * SCALING_FACTOR);
            payout.BagCount = (int)(remainingScore / (POINTS_PER_BAG * SCALING_FACTOR));
            remainingScore -= payout.BagCount * (POINTS_PER_BAG * SCALING_FACTOR);
            payout.StackCount = (int)(remainingScore / (POINTS_PER_STACK * SCALING_FACTOR));
            remainingScore -= payout.StackCount * (POINTS_PER_STACK * SCALING_FACTOR);
            payout.GoldCount = (int)(remainingScore / (POINTS_PER_GOLD * SCALING_FACTOR));
            remainingScore -= payout.GoldCount * (POINTS_PER_GOLD * SCALING_FACTOR);
            payout.SilverCount = (int)(remainingScore / (POINTS_PER_SILVER * SCALING_FACTOR));

            return payout;
        }

        public int GetTotalGoldCoins()
        {
            return (int)(Score / (POINTS_PER_GOLD * SCALING_FACTOR));
        }

        // Observable Collection of stories a user has read to completion
        // public ObservableCollection<Story> StoriesRead { get; set; } = new ObservableCollection<Story>();

        // Observable Collection of quizzes that have had all questions attempted
        //public ObservableCollection<Quiz> QuizzesCompleted { get { return new ObservableCollection<Quiz>(StoriesRead.SelectMany(s => s.Quizzes.Where(q => q.NumAttemptsQuiz > 0))); } }

        //TODO: figure out if this should show think and dos if both or either have been completed
        // Observable Collection of completed ThinkAndDos
        // public ObservableCollection<ThinkAndDo> ThinkAndDosCompleted { get { return new ObservableCollection<ThinkAndDo>(StoriesRead.SelectMany(s => s.ThinkAndDos.Where(t => t.CompletedPrompt1 || t.CompletedPrompt2))); } }

        // Int for the number of stories a user has read
        //public int StoryCount { get { return StoriesRead.Count; } }

        //// Int for the number of quizzes a user has completed
        //public int QuizCount { get { return QuizzesCompleted.Count; } }

        //// Int for the number of ThinkAndDos completed
        //public int ThinkAndDoCount { get { return ThinkAndDosCompleted.Count; } }
    }

    //class to store the user's rewards by denomination
    public class Payout
    {
        public int BankCount { get; set; }
        public int CarCount { get; set; }
        public int BagCount { get; set; }
        public int StackCount { get; set; }
        public int GoldCount { get; set; }
        public int SilverCount { get; set; }
    }
}