using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using BrainyStories.Objects;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BrainyStories.RealmObjects;
using Realms;

namespace BrainyStories
{
    // Story class used for each story
    // We can't have child classes for the Story and Imagines from a common base class- this isn't supported by Ralm
    public class Story : RealmObject
    {
        public string StoryId { get; set; } = Guid.NewGuid().ToString();

        // String of the name of story
        public String Name { get; set; }

        //TODO: should this be ImageSource?
        // string link to the image
        public string Icon { get; set; }

        // String of a short description of the story
        public String Description { get; set; }

        // Appeal type for colored dots
        //this has to be an int to be added to Realm since enum's aren't supported
        public int Appeal { get; set; }

        //String for audio file
        public String AudioClip { get; set; }

        // Timespan for the duration of the story
        public double DurationInSeconds { get; set; }

        //this is necessary because Realm doesn't support timespan
        [Ignored]
        public TimeSpan DurationInTimeSpan
        {
            get
            {
                return new TimeSpan(hours: 0, minutes: 0, seconds: (int)DurationInSeconds);
            }
        }

        public int WordCount { get; set; }

        /// <summary>
        /// This allows us to easily separate out the imagines, stories, and alternate story sets. Realm requires
        /// us to save this as an int
        /// </summary>
        public int StorySet { get; private set; }

        [Ignored]
        public StorySet StorySetAsEnum
        {
            get
            {
                return (RealmObjects.StorySet)StorySet;
            }
            set
            {
                StorySet = (int)value;
            }
        }

        [Ignored]
        public int HeightRequest
        {
            get
            {
                //TODO: standardize the aspect ratio of all story /imagine icons. Until then, use this
                return this.StorySetAsEnum == RealmObjects.StorySet.StorySet1 ? 200 : 130;
            }
        }

        public ThinkAndDo ThinkAndDo { get; set; }

        //store each prize icon (up to five prizes)
        public string Prize1 { get; set; } = String.Empty;

        public string Prize2 { get; set; } = String.Empty;
        public string Prize3 { get; set; } = String.Empty;
        public string Prize4 { get; set; } = String.Empty;
        public string Prize5 { get; set; } = String.Empty;

        //helper method to figure out how many prizes have been won for this story
        [Ignored]
        public int PrizesSelected
        {
            get
            {
                int count = 0;
                count += Convert.ToInt32(Prize1 != String.Empty);
                count += Convert.ToInt32(Prize2 != String.Empty);
                count += Convert.ToInt32(Prize3 != String.Empty);
                count += Convert.ToInt32(Prize4 != String.Empty);
                count += Convert.ToInt32(Prize5 != String.Empty);
                return count;
            }
        }

        //this toggle allows the user to switch between viewing prizes and viewing story details - default to false
        [Ignored]
        public bool AreRewardsVisible { get; set; } = false;

        [Ignored]
        public bool IsStoryVisible
        {
            get
            {
                return !AreRewardsVisible;
            }
        }

        // TODO: give the quiz object a FK reference to a story

        //public int QuizNum { get; set; } = 0;

        // Dictionary of cues for quizzes to quizzes
        //public Dictionary<TimeSpan, Quiz> QuizCues { get; set; }

        //TODO: replace with a FK reference to the Quiz object
        // Observable Collection of associated Quizzes
        //public ObservableCollection<Quiz> Quizzes { get; set; } = new ObservableCollection<Quiz>();

        // Int for the nNumber of quizzes that have been completed
        //public int NumCompletedQuizzes { get { return Quizzes.Count(q => q.NumAttemptsQuiz > 0); } }

        ////List of Icon Images
        //public ICollection<String> ListOfIcons { get; set; }
    }
}