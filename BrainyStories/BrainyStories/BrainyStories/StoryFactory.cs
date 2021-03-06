using BrainyStories.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using System.Threading;
using System.Diagnostics;
using Realms;
using BrainyStories.RealmObjects;
using System.Linq;

namespace BrainyStories
{
    // Class for generating all story objectds
    public class StoryFactory
    {
        //private QuizFactory quizFactory = new QuizFactory();

        public IEnumerable<Story> FetchStoriesOrImagines(StorySet storySet)
        {
            if (storySet == StorySet.Imagines)
            {
                return GenerateOrGetImagines();
            }
            else if (storySet == StorySet.StorySet1)
            {
                return GenerateOrGetStories();
            }
            else
            {
                //return an empty set if this happens, but it shouldn't ever get here
                return new List<Story>();
            }
        }

        //MANUAL LIST OF STORIES
        public IEnumerable<Story> GenerateOrGetStories()
        {
            var stories = new List<Story>();
            var realmFile = Realm.GetInstance(RealmConfiguration.DefaultConfiguration);

            var queryAllStories = realmFile.All<Story>().Where(x => x.StorySet == (int)(StorySet.StorySet1));
            if (queryAllStories != null && queryAllStories.Count() > 0)
            {
                return queryAllStories;
            }
            else
            {
                //add stories and their pages to the db
                using (var realmTransaction = realmFile.BeginWrite())
                {
                    //STORY 1
                    var story1 = new Story()
                    {
                        Name = "The Lion and the Mouse",
                        Icon = "S1_LATM_1.png",
                        Appeal = (int)AppealType.Animal,
                        Description = "A lion releases a mouse, believing it’s too small and weak ever to return the favor, " +
                        "but when the lion is trapped in a net the mouse gnaws the threads and releases the lion.",
                        AudioClip = "S1_TLATM_Story.mp3",
                        StorySetAsEnum = StorySet.StorySet1,
                        WordCount = 395
                    };

                    stories.Add(story1);
                    realmFile.Add(story1);

                    //these are a stop gap until we can separate the audio files into one file per segment
                    var endTimes = new int[] { 5, 32, 49, 62, 97, 116, 150, 9999 }; //max time can be really high since the end of the story will be reached first

                    //also add the pages of the story
                    GenerateStoryPages(realmFile, story1.StoryId, endTimes, "S1_LATM_1.png", "S1_LATM_2.jpg", "S1_LATM_3.jpg",
                        "S1_LATM_4.jpg", "S1_LATM_5.jpg", "S1_LATM_6.jpg", "S1_LATM_7.jpg", "S1_LATM_8.jpg");

                    //STORY 2
                    var story2 = new Story()
                    {
                        Name = "The Little Red Hen",
                        Icon = "S2_LRH_0.png",
                        Appeal = (int)AppealType.Animal,
                        Description = "Lazy animals refuse to help the hen plant the seed, harvest the grain, or bake the " +
                            "bread, so the hen refuses to share the baked bread with the lazy animals.",
                        AudioClip = "S2_LRH_Story.mp3",
                        StorySetAsEnum = StorySet.StorySet1,
                        WordCount = 477
                    };

                    stories.Add(story2);
                    realmFile.Add(story2);

                    endTimes = new int[] { 24, 54, 63, 96, 127, 160, 9999 };

                    GenerateStoryPages(realmFile, story2.StoryId, endTimes, "S2_LRH_1.jpg", "S2_LRH_2.jpg", "S2_LRH_3.jpg", "S2_LRH_4.jpg",
                        "S2_LRH_5.jpg", "S2_LRH_6.jpg", "S2_LRH_7.jpg");

                    //STORY 3
                    var story3 = new Story()
                    {
                        Name = "The Boy Who Cried Wolf",
                        Icon = "S3_TBWCW_1.png",
                        Appeal = (int)AppealType.Male,
                        Description = "Bored watching over the sheep, a boy causes excitement by lying that a wolf " +
                            "threatens; when a real wolf attacks, the people think the boy’s lying and won’t come to help him.",
                        AudioClip = "S3_BWCW_Story.mp3",
                        StorySetAsEnum = StorySet.StorySet1,
                        WordCount = 722
                    };

                    stories.Add(story3);
                    realmFile.Add(story3);

                    endTimes = new int[] { 2, 34, 77, 105, 141, 179, 209, 223, 9999 };

                    GenerateStoryPages(realmFile, story3.StoryId, endTimes, "S3_TBWCW_1.png", "S3_TBWCW_2.jpg", "S3_TBWCW_3.jpg", "S3_TBWCW_4.jpg",
                        "S3_TBWCW_5.jpg", "S3_TBWCW_6.jpg", "S3_TBWCW_7.jpg", "S3_TBWCW_8.jpg", "S3_TBWCW_9.jpg");

                    //STORY 4
                    var story4 = new Story()
                    {
                        Name = "The Elves and Shoemaker",
                        Icon = "S4_TEATS_1.png",
                        Appeal = (int)AppealType.General,
                        Description = "By secretly making shoes, two elves save a poor shoemaker and his wife; " +
                        "the man and wife make clothes to reward the elves, who leave when their help is no longer needed.",
                        AudioClip = "S4_TEATS_Story.mp3",
                        StorySetAsEnum = StorySet.StorySet1,
                        WordCount = 830
                    };

                    stories.Add(story4);
                    realmFile.Add(story4);

                    endTimes = new int[] { 4, 66, 100, 132, 152, 213, 297, 9999 };

                    GenerateStoryPages(realmFile, story4.StoryId, endTimes, "S4_TEATS_1.png", "S4_TEATS_2.jpg", "S4_TEATS_3.jpg", "S4_TEATS_4.jpg",
                            "S4_TEATS_5.jpg", "S4_TEATS_6.jpg", "S4_TEATS_7.jpg", "S4_TEATS_7.jpg");

                    //STORY 5
                    var story5 = new Story()
                    {
                        Name = "The Three Little Pigs",
                        Icon = "S5_TLP_0.png",
                        Appeal = (int)AppealType.Animal,
                        Description = "Two pigs squander their money and build shabby houses; their smarter brother " +
                             "saves and works hard to build a brick house which protects them all from the big bad wolf.",
                        AudioClip = "S5_TLP_Story.mp3",
                        StorySetAsEnum = StorySet.StorySet1,
                        WordCount = 986
                    };

                    stories.Add(story5);
                    realmFile.Add(story5);

                    endTimes = new int[] { 27, 65, 139, 161, 192, 237, 260, 289, 358, 371, 390, 391, 9999 };

                    GenerateStoryPages(realmFile, story5.StoryId, endTimes, "S5_TLP_1.jpg", "S5_TLP_2.jpg", "S5_TLP_3.jpg", "S5_TLP_4.jpg", "S5_TLP_5.jpg",
                          "S5_TLP_6.jpg", "S5_TLP_7.jpg", "S5_TLP_8.jpg", "S5_TLP_9.jpg", "S5_TLP_10.jpg", "S5_TLP_11.jpg", "S5_TLP_12.jpg", "S5_TLP_13.jpg");

                    realmTransaction.Commit();
                }
            }

            return stories;
        }

        //END OF STORIES

        //MANUAL LIST OF IMAGINES
        public IEnumerable<Story> GenerateOrGetImagines()
        {
            var imagines = new List<Story>();
            var realmFile = Realm.GetInstance(RealmConfiguration.DefaultConfiguration);
            //we have to query the db using an int because Realm doesn't deal with enums
            var imaginesQuery = realmFile.All<Story>().Where(x => x.StorySet == ((int)StorySet.Imagines));

            if (imaginesQuery != null && imaginesQuery.Count() > 0)
            {
                imagines = imaginesQuery.ToList();
            }
            else
            {
                using (var realmTransaction = realmFile.BeginWrite())
                {
                    //use a simple audio player to measure duration
                    using (var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.CreateSimpleAudioPlayer())
                    {
                        var imagine1 = new Story()
                        {
                            Name = "If A Shoe Wanted to be Car",
                            Icon = "I1_IASW_1.jpg",
                            Appeal = (int)AppealType.General,
                            Description = "Imagine a shoe wanting to be like a car, and what a child might find " +
                        "in the home to help.",
                            AudioClip = "I1_IAS_IG.mp3",
                            StorySetAsEnum = StorySet.Imagines,
                            //have to add word count manually because the text is embedded in a jpg image
                            WordCount = 212
                        };
                        player.Load(imagine1.AudioClip);
                        imagine1.DurationInSeconds = player.Duration;

                        imagines.Add(imagine1);
                        realmFile.Add<Story>(imagine1);

                        var endTimes = new int[] { 8, 43, 67, 86, 9999 };

                        GenerateStoryPages(realmFile, imagine1.StoryId, endTimes, "I1_IASW_1.jpg", "I1_IASW_2.jpg",
                            "I1_IASW_3.jpg", "I1_IASW_4.jpg", "I1_IASW_5.jpg");

                        //imagines 2
                        var imagine2 = new Story()
                        {
                            Name = "Do you pump your legs when you swing?",
                            Icon = "I2_DYPYL_1.jpg",
                            Appeal = (int)AppealType.Male,
                            Description = "Imagine swinging as high as trees, birds, clouds, or " +
                            "even higher, what it might feel like, what you might see.",
                            AudioClip = "I2_DYPYL_IG.mp3",
                            StorySetAsEnum = StorySet.Imagines,
                            WordCount = 206
                        };
                        //TODO: put all this in a nice helper function to avoid having to copy this so much
                        player.Load(imagine2.AudioClip);
                        imagine2.DurationInSeconds = player.Duration;

                        imagines.Add(imagine2);
                        realmFile.Add<Story>(imagine2);

                        endTimes = new int[] { 4, 31, 55, 66, 71, 98, 9999 };

                        GenerateStoryPages(realmFile, imagine2.StoryId, endTimes, "I2_DYPYL_1.jpg", "I2_DYPYL_2.jpg", "I2_DYPYL_3.jpg",
                            "I2_DYPYL_4.jpg", "I2_DYPYL_5.jpg", "I2_DYPYL_6.jpg", "I2_DYPYL_7.jpg");

                        var imagine3 = new Story()
                        {
                            Name = "Upside Down Windows",
                            Icon = "I3_TUDW_1.jpg",
                            Appeal = (int)AppealType.Female,
                            Description = "Imagine wandering into a world where everything is upside down and backwards.",
                            AudioClip = "I3_UW_IG.mp3",
                            StorySetAsEnum = StorySet.Imagines,
                            WordCount = 248
                        };
                        player.Load(imagine3.AudioClip);
                        imagine3.DurationInSeconds = player.Duration;

                        imagines.Add(imagine3);
                        realmFile.Add<Story>(imagine3);

                        endTimes = new int[] { 5, 34, 59, 86, 108, 9999 };

                        GenerateStoryPages(realmFile, imagine3.StoryId, endTimes, "I3_TUDW_1.jpg", "I3_TUDW_2.jpg", "I3_TUDW_3.jpg",
                            "I3_TUDW_4.jpg", "I3_TUDW_5.jpg", "I3_TUDW_6.jpg");

                        var imagine4 = new Story()
                        {
                            Name = "The Special One-Eye Blink",
                            Icon = "I4_TSOEB_1.jpg",
                            Appeal = (int)AppealType.Female,
                            Description = "Imagine blinking to become very tiny and what you " +
                            "might be able to do if you were very, very small.",
                            AudioClip = "I4_SOEB_IG.mp3",
                            StorySetAsEnum = StorySet.Imagines,
                            WordCount = 304
                        };
                        player.Load(imagine4.AudioClip);
                        imagine4.DurationInSeconds = player.Duration;

                        imagines.Add(imagine4);
                        realmFile.Add<Story>(imagine4);

                        endTimes = new int[] { 3, 45, 64, 97, 132, 9999 };

                        GenerateStoryPages(realmFile, imagine4.StoryId, endTimes, "I4_TSOEB_1.jpg", "I4_TSOEB_2.jpg",
                            "I4_TSOEB_3.jpg", "I4_TSOEB_4.jpg", "I4_TSOEB_5.jpg", "I4_TSOEB_6.jpg");

                        var imagine5 = new Story()
                        {
                            Name = "If a Naughty Angel",
                            Icon = "I5_IANA_1.jpg",
                            Appeal = (int)AppealType.General,
                            Description = "Imagine what you’d say if a little angel asked your " +
                            "advice on how to be a tiny bit mischievous.",
                            AudioClip = "I5_IANA_IG.mp3",
                            StorySetAsEnum = StorySet.Imagines,
                            WordCount = 399
                        };
                        player.Load(imagine5.AudioClip);
                        imagine5.DurationInSeconds = player.Duration;

                        imagines.Add(imagine5);
                        realmFile.Add<Story>(imagine5);

                        endTimes = new int[] { 3, 45, 64, 97, 132, 9999 };

                        GenerateStoryPages(realmFile, imagine5.StoryId, endTimes, "I5_IANA_1.jpg", "I5_IANA_2.jpg",
                            "I5_IANA_3.jpg", "I5_IANA_4.jpg", "I5_IANA_5.jpg", "I5_IANA_6.jpg");
                    }

                    realmTransaction.Commit();
                }
            }
            return imagines;
        }

        /// <summary>
        /// helper method to generate the story pages that belong to a parent story
        /// </summary>
        /// <param name="storyId"></param>
        /// <param name="listOfPages"></param>
        /// <returns></returns>
        private IEnumerable<StoryPart> GenerateStoryPages(Realm realmFile, string storyId, int[] endTimes, params string[] listOfPages)
        {
            var storyPages = new List<StoryPart>();

            for (int i = 0; i < listOfPages.Length; i++)
            {
                var page = new StoryPart()
                {
                    Image = listOfPages[i],
                    Order = i + 1,
                    StoryId = storyId,
                    EndTimeInSeconds = endTimes[i],
                    //set the start time to end time of the previous page plus 1 (this helps for faster sorting later)
                    StartTimeInSeconds = i > 0 ? (endTimes[i - 1]) : 0
                };
                realmFile.Add<StoryPart>(page);
                storyPages.Add(page);
            }

            return storyPages;
        }

        //private ICollection<String> CreateStoryActivitiesStack(int quizNum, int thinkAndDoNum, AppealType type)
        //{
        //    ICollection<String> images = new List<String>();
        //    if (type != null)
        //    {
        //        images.Add(type.Value);
        //    }
        //    //for (int i = 0; i < quizNum; i++)
        //    //{
        //    //    images.Add(Quiz.Icon);
        //    //}
        //    for (int i = 0; i < thinkAndDoNum; i++)
        //    {
        //        images.Add(ThinkAndDo.Icon);
        //    }
        //    return images;
        //}
    }
}