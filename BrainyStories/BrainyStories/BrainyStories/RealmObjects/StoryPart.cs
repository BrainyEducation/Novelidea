using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace BrainyStories.RealmObjects
{
    public class StoryPart : RealmObject
    {
        public string StoryPartId { get; private set; } = Guid.NewGuid().ToString();

        public string StoryId { get; set; }

        //the numerical order in which this page of the story should be displayed
        public int Order { get; set; }

        //filepath to the image for this page of the story
        public string Image { get; set; }

        //TODO: in the future, we need to figure out how to pair the audio with the images (separate audio per image possibly)
        //for now, manually track when the story part should end in the audio file
        public int EndTimeInSeconds { get; set; }

        public int StartTimeInSeconds { get; set; }
    }
}