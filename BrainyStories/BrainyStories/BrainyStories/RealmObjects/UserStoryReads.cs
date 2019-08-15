using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace BrainyStories.RealmObjects
{
    /*  NOTE: this class represents a transaction "table" in the Realm database that tracks the total number of times that
    *   the user has read individual stories. Realm isn't a relational DB, so this isn't the textbook way to do this. However,
    *   I don't trust the current state of Realm (August 2019) enough to utilize it nonrelationally
    */

    public class UserStoryReads : RealmObject
    {
        public String UserStoryId { get; set; } = Guid.NewGuid().ToString();
        public String UserId { get; set; }

        public String StoryId { get; set; }

        //records when the story was read - to get total times, query this table and count the total user stories
        public DateTimeOffset Timestamp { get; set; }
    }
}