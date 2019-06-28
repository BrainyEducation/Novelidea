using Realms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BrainyStories.RealmObjects
{
    // Question class used by the Quiz Class
    public class Question : RealmObject
    {
        [PrimaryKey]
        public String QuestionId { get; private set; } = Guid.NewGuid().ToString();

        [Indexed]
        public String QuizId { get; set; }

        public int QuestionOrder { get; set; }

        // String text of the question
        public string QuestionText { get; set; }

        //number of times the user attempted to answer question
        public int NumberOfAttempts { get; set; }

        // String of the question audio file
        public String Audio { get; set; }
    }
}