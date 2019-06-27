using BrainyStories.Objects;
using Realms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BrainyStories
{
    // Class that generates all associated ThinkAndDos
    public class ThinkAndDoFactory
    {
        private ObservableCollection<ThinkAndDo> ThinkAndDoListTemp;

        //database init
        private Realm RealmFile;

        //this story gets its own constant because it is an exception since it has only one TAD
        public const String RED_HEN_NAME = "The Little Red Hen";

        public ObservableCollection<ThinkAndDo> generateThinkAndDos()
        {
            RealmFile = Realm.GetInstance(RealmConfiguration.DefaultConfiguration);

            var thinkAndDoQuery = RealmFile.All<ThinkAndDo>();

            //separate logic based on whether or not the data has been created yet or not
            if (thinkAndDoQuery.Count() > 0)
            {
                ThinkAndDoListTemp = new ObservableCollection<ThinkAndDo>(thinkAndDoQuery.ToList());
            }
            else
            {
                ThinkAndDoListTemp = new ObservableCollection<ThinkAndDo>();
                //write to db
                using (var realmTransaction = RealmFile.BeginWrite())
                {
                    // The Lion and the Mouse Think and Dos
                    CreateNewThinkAndDo(new ThinkAndDo
                    {
                        ThinkAndDoName = "The Lion and the Mouse",
                        AssociatedImage = "S1_LATM_1.jpg",
                        ThinkAndDoAudioClip1 = "S1_TLATM_TAD1.mp3",
                        ThinkAndDoAudioClip2 = "S1_TLATM_TAD2.mp3",
                        Text1 = "Draw a picture of the lion that’s different from a picture you’ve already seen. Draw a picture of the mouse that’s different from a picture you’re already seen. Write or say what the mouse was afraid might happen to him. Write or say what the lion was afraid might happen to him.",
                        Text2 = "The lion did not believe the mouse would be able to help him. Did the lion make a mistake about that? Even the smallest friends can do big favors. Have you ever felt too small to help someone?  Cut out a piece of paper and draw or write about a favor that you have done for someone else. Or, you can draw or write about a favor someone did for you. Make a “favor chain” by taping or hooking the papers together in a chain. Hang up the “favor chain” and add a paper link each time you do a favor or someone does a favor for you."
                    });

                    // The Little Red Hen Think and Dos
                    CreateNewThinkAndDo(new ThinkAndDo
                    {
                        ThinkAndDoName = RED_HEN_NAME,
                        AssociatedImage = "S2_LRH_0.jpg",
                        ThinkAndDoAudioClip1 = "S2_LRH_TAD1.mp3",
                        Text1 = "When the Little Red Hen found a grain of wheat what did she want to do with it? Put the story in order by what she did. Did she really want to do all the work alone all by herself? Who did she ask for help? Did the animals help the Little Red Hen in any way? After the Little Red Hen finished with the bread, what did she do?"
                    });

                    // The Boy Who Cried Wolf Think and Dos
                    CreateNewThinkAndDo(new ThinkAndDo
                    {
                        ThinkAndDoName = "The Boy Who Cried Wolf",
                        AssociatedImage = "S3_TBWCW_1.jpg",
                        ThinkAndDoAudioClip1 = "S3_BWCW_TAD1.mp3",
                        ThinkAndDoAudioClip2 = "S3_BWCW_TAD2.mp3",
                        Text1 = "Who is the main character in the story? At the beginning of the story what did he think was a problem? How did he try to solve the problem? By the end of the story what did the people in the village think was the problem? How did the people solve the problem?"
                    });

                    // The Elves and the Shoemaker Think and Dos
                    CreateNewThinkAndDo(new ThinkAndDo
                    {
                        ThinkAndDoName = "The Elves and the Shoemaker",
                        AssociatedImage = "S4_TEATS_1.jpg",
                        ThinkAndDoAudioClip1 = "S4_TEATS_TAD1.mp3",
                        ThinkAndDoAudioClip2 = "S4_TEATS_TAD2.mp3",
                        Text1 = "The elves in the story helped the shoemaker without being asked. What did the elves do to show they were nice? In what way did the shoemaker and his wife thank the elves? Has anyone ever helped you in your life? Draw a picture of something you might do for this person. Or, write a thank you letter from the shoemaker to the elves.",
                        Text2 = "Do you think the story is about something that really did happen, or about something that could happen, or about something that could never happen? What in the story makes you think so?"
                    });

                    // The Three Little Pigs Think and Dos
                    CreateNewThinkAndDo(new ThinkAndDo
                    {
                        ThinkAndDoName = "The Three Little Pigs",
                        AssociatedImage = "S5_TLP_0.jpg",
                        ThinkAndDoAudioClip1 = "S5_TLP_TAD1.mp3",
                        ThinkAndDoAudioClip2 = "S5_TLP_TAD2.mp3",
                        Text1 = "Do you think the first pig made a good choice when he decided to spend all his money on candy? Which pig made the best and smartest choice about saving and then spending his money? Do you think the three pigs were good brothers? Tell why you think that. What if the wolf had one or two brothers? If the wolf had brothers in the story, how do you think the wolf brothers might help each other?",
                        Text2 = "Have you heard a different version of the story about the three pigs or seen a picture book version or watched a cartoon version of the story? If so, which version did you like best and why? Did a different version tell about the pig’s mother? Did the pigs have money in another version? Did the pigs buy candy in another version? Did the wolf say: “Little pig, little pig, let me come in.” Did the pig answer: “Not by the hair of my chinny, chin, chin.” In the other version, what happened to the wolf at the very end? Did he become the pigs’ dinner or did he run away?"
                    });

                    //commenting out the last 5 stories to cut down on resource usage
                    //// The Three Billy Goats Gruff Think and Dos
                    //CreateNewThinkAndDo(new ThinkAndDo
                    //{
                    //    ThinkAndDoName = "The Three Billy Goats Gruff Think And Do",
                    //    AssociatedImage = "S6_BGG_1.jpg",
                    //    ThinkAndDoAudioClip1 = "S6_BGG_TAD1.mp3",
                    //    ThinkAndDoAudioClip2 = "S6_BGG_TAD2.mp3",
                    //    Text1 = "Fold a piece of paper in half once, and then fold it in half again to mark four parts on the paper. In one part draw the smallest goat. In another part draw the middle size goat. In another part draw the biggest goat. In the last part draw the troll. Cut out the pictures of the goats and the troll so you can use the pictures to tell the story. For the grass hillsides you might use two pillows or two handkerchiefs or something else. For the bridge you might use a book or box or something else. Listen to the story being read again and use your pictures to show and tell the story. Show and tell the story to a friend or a grownup or at school.",
                    //    Text2 = "Which character in the story is the meanest and cruelest, like a big bad bully? What does the troll want to do with the goats? Draw a different way the goats can get across the watery stream without using the bridge."
                    //});

                    //// The Tale of Peter Rabbit Think and Dos
                    //CreateNewThinkAndDo(new ThinkAndDo
                    //{
                    //    ThinkAndDoName = "The Tales of Peter Rabbit Think And Do",
                    //    AssociatedImage = "S7_PR_0.jpg",
                    //    ThinkAndDoAudioClip1 = "S7_PR_TAD1.mp3",
                    //    ThinkAndDoAudioClip2 = "S7_PR_TAD2.mp3",
                    //    Text1 = "Have you ever planted a garden? What would be nice to plant in a garden? What kinds of things would a rabbit like in a garden? What did Mr. MacGregor do with Peter’s coat and shoes? Why did Mr. MacGregor do that?",
                    //    Text2 = "Name all of Mother Rabbit’s children. Which rabbit child was the naughtiest and got into the most mischief? Peter Rabbit disobeyed his mother and went where? When Peter ran into the tool shed, where did he land? Why did Peter cry? What clothes did Peter lose? What happened to Peter when he got home? What lesson should Peter have learned from his adventure?"
                    //});

                    //// The Gingerbread Man Think and Dos
                    //CreateNewThinkAndDo(new ThinkAndDo
                    //{
                    //    ThinkAndDoName = "The Gingerbread Man Think And Do",
                    //    AssociatedImage = "S8_TGM_1.jpg",
                    //    ThinkAndDoAudioClip1 = "S8_TGM_TAD1.mp3",
                    //    ThinkAndDoAudioClip2 = "S8_TGM_TAD2.mp3",
                    //    Text2 = "That Gingerbread Man was really fast! Why do you think the Gingerbread Man ran and ran? Who did he run away from first? Who was the last character to chase him?  What would you do to catch the Gingerbread man? Imagine a Gingerbread Man trap and, if you can, draw a picture of the trap.",
                    //    Text1 = "Draw a picture of the Gingerbread Man and use glue or tape to put the picture on a stick or pencil. Listen to the story again, and when you hear the words—“Run, Run fast as you can, you can’t catch me I’m the Gingerbread Man!” — say them at the same time and make your Gingerbread Man hop and bounce and move along quickly."
                    //});

                    //// Rumplestiltskin Think and Dos
                    //CreateNewThinkAndDo(new ThinkAndDo
                    //{
                    //    ThinkAndDoName = "Rumplestiltskin Think And Do",
                    //    AssociatedImage = "S9_R_1.jpg",
                    //    ThinkAndDoAudioClip1 = "S9_R_TAD1.mp3",
                    //    ThinkAndDoAudioClip2 = "S9_R_TAD2.mp3",
                    //    Text1 = "The miller’s daughter had a big problem and Rumplestiltskin helped her by using magic to spin straw into gold. If you could do magic what would you do? Would you be greedy like the king or would you do magic to help others? Draw a picture of you using magic to solve a problem.",
                    //    Text2 = "How many characters in the story were bossy and told the miller’s daughter what to do? Which character was the bossiest? Which character was the meanest? Why do you think that character was meanest? Which character did most to help the miller’s daughter? Which character in the story do you think was the smartest? Why?"
                    //});

                    //// Little Red Riding Hood Think and Dos
                    //CreateNewThinkAndDo(new ThinkAndDo
                    //{
                    //    ThinkAndDoName = "Little Red Riding Hood Think And Do",
                    //    AssociatedImage = "S10_LRRH_1.jpg",
                    //    ThinkAndDoAudioClip1 = "S10_LRRH_TAD1.mp3",
                    //    ThinkAndDoAudioClip2 = "S10_LRRH_TAD2.mp3",
                    //    Text1 = "Why did the little girl cut through the forest? Was it a good choice? Who did she meet along the way? Where did the wolf go? Was the little girl frightened? When Little Red Riding Hood realized who he was, what did the wolf do? How was the little girl saved at the end?",
                    //    Text2 = "Why was the little girl called Little Red Riding Hood? Who made the hood for her? Why was she going to visit her grandmother? Did the little girl disobey her mother? How? What did the wolf do? What did the girl notice that helped her know the wolf was only pretending to be her grandmother? Who happened to hear the snoring and come into grandmother’s house? Would the girl have been in trouble if she had not disobeyed her mother?"
                    //});
                    realmTransaction.Commit();
                }
            }

            return ThinkAndDoListTemp;
        }

        //add to list and add to database
        private void CreateNewThinkAndDo(ThinkAndDo thinkAndDo)
        {
            ThinkAndDoListTemp.Add(thinkAndDo);
            RealmFile.Add<ThinkAndDo>(thinkAndDo);
        }

        public ObservableCollection<ThinkAndDo> StoryThinkAndDos(String story)
        {
            var realmInstance = Realm.GetInstance(RealmConfiguration.DefaultConfiguration);

            return new ObservableCollection<ThinkAndDo>(realmInstance.All<ThinkAndDo>()
                .Where(x => x.ThinkAndDoName.Equals(story)));
        }
    }
}