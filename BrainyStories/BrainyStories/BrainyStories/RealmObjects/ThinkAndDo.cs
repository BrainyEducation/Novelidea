using System;
using System.Collections.Generic;
using System.Text;
using Realms;

namespace BrainyStories.Objects
{
    // Class for the ThinkAndDo activities
    public class ThinkAndDo : RealmObject
    {
        // String of the icon used for ThinkAndDos in the table of contents
        public static String Icon { get; } = "ThinkAndDoIcon.png";

        // String for Name
        public String ThinkAndDoName { get; set; }

        // String for Associated Image
        public String AssociatedImage { get; set; }

        //the image references should be ignored in the database
        [Ignored]
        public String Star1Image
        {
            get
            {
                return CompletedPrompt1 ? "GoldStar1.png" : "SilverStar1.png";
            }
        }

        [Ignored]
        public String Star2Image
        {
            get
            {
                return CompletedPrompt2 ? "GoldStar2.png" : "SilverStar2.png";
            }
        }

        // String for audio file
        public String ThinkAndDoAudioClip1 { get; set; }

        public String ThinkAndDoAudioClip2 { get; set; }

        // Boolean of if the ThinkAndDo was completed or not
        public bool CompletedPrompt1 { get; set; } = false;

        public bool CompletedPrompt2 { get; set; } = false;

        //this is the text that contains the think and do narrative
        public String Text1 { get; set; }

        public String Text2 { get; set; }

        //foreign key to Stories
        public String StoryId { get; set; } = null; //nullable
    }
}