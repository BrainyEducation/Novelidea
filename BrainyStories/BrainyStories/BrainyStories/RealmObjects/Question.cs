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

        // String text of the question
        public string QuestionText { get; set; }

        //foreign key to the Answers for the Question
        public RealmList<Answer> AnswerArray { get; set; }

        //number of times the user attempted to answer question
        public int NumberOfAttempts { get; set; }

        // String of the question audio file
        public String Audio { get; set; }
    }
}