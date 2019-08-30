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
        public String QuizId { get; private set; } = Guid.NewGuid().ToString();

        // String of the quiz icon used in the table of contents
        public static string Icon { get; } = "QuizzesIcon.png";

        // String of the name of the quiz
        public string QuizName { get; set; }

        // String for Associated Story
        public String AssociatedImage { get; set; }

        // Timespan for the quiz should be played
        //public TimeSpan PlayTime { get; set; }

        // Int of the number of attempts on the entire quiz
        public int NumberOfAttempts { get; set; }
    }
}