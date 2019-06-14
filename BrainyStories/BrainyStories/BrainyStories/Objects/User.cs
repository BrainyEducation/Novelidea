using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.IO;
using Newtonsoft.Json;

using System.Collections.ObjectModel;

using System.Linq;

namespace BrainyStories.Objects
{
    // User class used for persistence of progress throughout the application
    internal class User
    {
        [JsonIgnore]
        private static User INSTANCE = null;

        private User()
        { }

        //This is used when loading the User from JSON (not yet implemented)
        [JsonConstructor]
        public User(Dictionary<String, int> RewardsRecieved, string Name, ObservableCollection<Story> StoriesRead)
        {
            this.RewardsRecieved = RewardsRecieved;
            this.Name = Name;
            this.StoriesRead = StoriesRead;
        }

        // Get the singleton instance of the User class
        public static User Instance
        {
            get
            {
                if (INSTANCE == null)
                    INSTANCE = new User();
                return INSTANCE;
            }
        }

        // Dictionary for the users recieved rewards
        public Dictionary<String, int> RewardsRecieved { get; } = new Dictionary<String, int>() { { "Gold", 50 }, { "Silver", 20 } };

        // String for the name of the user Can be customized for the user
        public string Name { get; } = "Your child";

        // Observable Collection of stories a user has read to completion
        public ObservableCollection<Story> StoriesRead { get; set; } = new ObservableCollection<Story>();

        // Observable Collection of quizzes that have had all questions attempted
        public ObservableCollection<Quiz> QuizzesCompleted { get { return new ObservableCollection<Quiz>(StoriesRead.SelectMany(s => s.Quizzes.Where(q => q.NumAttemptsQuiz > 0))); } }

        //TODO: figure out if this should show think and dos if both or either have been completed
        // Observable Collection of completed ThinkAndDos
        public ObservableCollection<ThinkAndDo> ThinkAndDosCompleted { get { return new ObservableCollection<ThinkAndDo>(StoriesRead.SelectMany(s => s.ThinkAndDos.Where(t => t.CompletedPrompt1 || t.CompletedPrompt2))); } }

        // Int for the number of stories a user has read
        public int StoryCount { get { return StoriesRead.Count; } }

        // Int for the number of quizzes a user has completed
        public int QuizCount { get { return QuizzesCompleted.Count; } }

        // Int for the number of ThinkAndDos completed
        public int ThinkAndDoCount { get { return ThinkAndDosCompleted.Count; } }
    }
}