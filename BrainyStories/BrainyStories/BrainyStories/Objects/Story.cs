using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using BrainyStories.Objects;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace BrainyStories
{
    // AppealType class associated with each classic story
    public class AppealType
    {
        // AppealType of the story
        private AppealType(string value) { Value = value; }

        // String of the appealtype value
        public string Value { get; set; }

        // Male, Female, General, and Animal appeal types
        public static AppealType Male { get { return new AppealType("Male.png"); } }

        public static AppealType Female { get { return new AppealType("Female.png"); } }
        public static AppealType General { get { return new AppealType("General.png"); } }
        public static AppealType Animal { get { return new AppealType("Animal.png"); } }
    }

    // Story class used for each story
    public class Story
    {
        // String of the name of story
        public String Name { get; set; }

        // ImageSource clip for icon
        public ImageSource Icon { get; set; }

        // Timespan for the duration of the story
        public TimeSpan Duration { get; set; }

        // Int for the wordcount of the story
        public int WordCount { get; set; }

        // String of a short description of the story
        public String Description { get; set; }

        // Appeal type for colored dots
        public AppealType Appeal { get; set; }

        // Int for the number of quizzes for story/imagine
        public int QuizNum { get; set; } = 0;

        // Int for the number of think and do exercises
        public int ThinkDoNum { get; set; } = 0;

        // Dictionary of cues for quizzes to quizzes
        public Dictionary<TimeSpan, Quiz> QuizCues { get; set; }

        // Dictionary of cues for picture transition to pictures
        public Dictionary<TimeSpan, String> PictureCues { get; set; }

        // Observable Collection of associated ThinkAndDos
        public ObservableCollection<ThinkAndDo> ThinkAndDos { get; set; } = new ObservableCollection<ThinkAndDo>();

        // Int for the number of think and do's that have been completed
        public int NumCompletedThinkAndDos { get { return ThinkAndDos.Count(t => t.CompletedPrompt1) + ThinkAndDos.Count(x => x.CompletedPrompt2); } }

        // Observable Collection of associated Quizzes
        public ObservableCollection<Quiz> Quizzes { get; set; } = new ObservableCollection<Quiz>();

        // Int for the nNumber of quizzes that have been completed
        public int NumCompletedQuizzes { get { return Quizzes.Count(q => q.NumAttemptsQuiz > 0); } }

        // Double for the average score of completed quizzes, or 0 if there are no completed quizzes
        public int QuizAvgScore { get { return NumCompletedQuizzes > 0 ? (int)Math.Round(Quizzes.Where(q => q.NumAttemptsQuiz > 0).Average(q => q.Score), 0) : 0; } }

        //String for audio file
        public String AudioClip { get; set; }

        // Static Observable Collection of stories used for data binding
        public static ObservableCollection<Story> ListOfStories { get; set; }

        //List of Icon Images
        public ICollection<String> ListOfIcons { get; set; }
    }
}