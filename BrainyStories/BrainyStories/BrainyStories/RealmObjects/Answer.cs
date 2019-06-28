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

        public String Text { get; set; }

        //set this to true or false if the answer is correct or incorrect
        public bool IsTrue { get; set; }

        //set to true if the user selects this
        public bool IsSelected { get; set; }

        public string Audio { get; set; }
    }
}