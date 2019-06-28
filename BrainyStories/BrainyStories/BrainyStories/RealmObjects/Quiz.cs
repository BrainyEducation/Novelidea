using Realms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BrainyStories.RealmObjects
{
    // The Quiz Class
    public class Quiz : RealmObject
    {
        // String of the quiz icon used in the table of contents
        public static string Icon { get; } = "QuizzesIcon.png";

        // String of the name of the quiz
        public string QuizName { get; set; }

        // String for Associated Story
        public String AssociatedImage { get; set; }

        [Ignored]
        // Double of the score for the quiz
        public double Score
        {
            get
            {
                //this might be too time consuming of a query -keep an eye on it
                return Questions.Count(x => x.AnswerArray.Count(y => y.IsSelected && y.IsTrue) > 0);
            }
        }

        public List<Question> Questions { get; set; }

        // Timespan for the quiz should be played
        public TimeSpan PlayTime { get; set; }

        // Int of the number of attempts on the entire quiz
        public int NumberOfAttemptsQuiz { get; set; }
    }
}