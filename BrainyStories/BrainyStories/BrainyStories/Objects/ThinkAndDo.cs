using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace BrainyStories.Objects
{
    // Class for the ThinkAndDo activities
    public class ThinkAndDo
    {
        // String of the icon used for ThinkAndDos in the table of contents
        public static String Icon { get; } = "ThinkAndDoIcon.png";

        // String for Name
        public String ThinkAndDoName { get; set; }

        // String for Associated Story
        public String AssociatedImage { get; set; }

        // String for audio file
        public String ThinkAndDoAudioClip1 { get; set; }

        public String ThinkAndDoAudioClip2 { get; set; }

        // Boolean of if the ThinkAndDo was completed or not
        public bool CompletedPrompt1 { get; set; } = false;

        public bool CompletedPrompt2 { get; set; } = false;

        public String Text1 { get; set; }
        public String Text2 { get; set; }

        public TimeSpan GetAudioLength1()
        {
            return GetAudioLength(1);
        }

        public TimeSpan GetAudioLenth2()
        {
            return GetAudioLength(2);
        }

        private TimeSpan GetAudioLength(int audioClipNumber)
        {
            string clipName = ThinkAndDoAudioClip2;
            if (audioClipNumber == 1)
            {
                clipName = ThinkAndDoAudioClip1;
            }

            TimeSpan totalTime = new TimeSpan(0);
            try
            {
                Mp3FileReader mp3FileReader = new Mp3FileReader(clipName);
                totalTime = mp3FileReader.TotalTime;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return totalTime;
        }
    }
}