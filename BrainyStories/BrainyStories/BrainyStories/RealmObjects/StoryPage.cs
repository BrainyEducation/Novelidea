using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace BrainyStories.RealmObjects
{
    public class StoryPage : RealmObject
    {
        public string StoryPageId { get; private set; } = Guid.NewGuid().ToString();

        public string StoryId { get; set; }

        //the numerical order in which this page of the story should be displayed
        public int Order { get; set; }

        //filepath to the image for this page of the story
        public string Image { get; set; }
    }
}