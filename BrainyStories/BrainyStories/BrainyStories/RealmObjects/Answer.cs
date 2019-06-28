using System;
using System.Collections.Generic;
using System.Text;
using Realms;

namespace BrainyStories.RealmObjects
{
    public class Answer : RealmObject
    {
        [PrimaryKey]
        public String AnswerId { get; private set; } = Guid.NewGuid().ToString();

        [Indexed]
        public String QuestionId { get; set; }

        public String Text { get; set; }

        [Indexed]
        //set this to true or false if the answer is correct or incorrect
        public bool IsTrue { get; set; }

        [Indexed]
        //if the user selects this answer, record the time during which they attempted
        public DateTimeOffset DateTimeSelected { get; set; }

        public string Audio { get; set; }
    }
}