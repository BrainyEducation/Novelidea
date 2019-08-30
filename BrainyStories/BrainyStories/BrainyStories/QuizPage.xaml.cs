using BrainyStories.Objects;
using BrainyStories.RealmObjects;
using Plugin.SimpleAudioPlayer;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainyStories
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    // Class for the quiz page
    public partial class QuizPage : ContentPage
    {
        private int QuestionNum = 0;
        private int QuestionsCorrect = 0;
        private bool[] NumButtonPressed = { false, false, false, false };
        private bool[] NumButtonDoublePressed = { false, false, false, false };
        private Button PreviousAnswerSelected = new Button() { Text = "" };
        private Quiz quiz;
        private int[] scoreCalculation;
        private ISimpleAudioPlayer player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
        private int tapCount = 0;
        private User user;
        private string audio = null;

        public QuizPage(Quiz temp)
        {
            InitializeComponent();

            quiz = temp;
            temp.NumAttemptsQuiz++;
            scoreCalculation = new int[quiz.NumQuestions];
            for (int i = 0; i < quiz.NumQuestions; i++)
            {
                scoreCalculation[i] = 4;
            }
            QuizTitle.Text = quiz.QuizName;
            NumCorrect.Text = "Questions Correct: " + QuestionsCorrect;
            QuestionTitle.Text = quiz.Questions[QuestionNum].QuestionText;
            One.Text = quiz.Questions[QuestionNum].AnswerArray[0];
            Two.Text = quiz.Questions[QuestionNum].AnswerArray[1];
            Three.Text = quiz.Questions[QuestionNum].AnswerArray[2];
            Four.Text = quiz.Questions[QuestionNum].AnswerArray[3];
            PreviousButton.IsVisible = false;
            try
            {
                player.Load(quiz.Questions[QuestionNum].Audio);
                player.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine(quiz.Questions[QuestionNum].Audio + " does not exist!");
            }
            user = User.Instance;
        }

        public QuizPage(Quiz temp, string audioClip) : this(temp)
        {
            audio = audioClip;
        }

        // Handles if the previous button is clicked
        private void PreviousQuestion(object sender, EventArgs e)
        {
            if (QuestionNum > 0)
            {
                QuestionNum--;
                try
                {
                    player.Load(quiz.Questions[QuestionNum].Audio);
                    player.Play();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(quiz.Questions[QuestionNum].Audio + " does not exist!");
                }
                QuestionTitle.Text = quiz.Questions[QuestionNum].QuestionText;
                One.Text = quiz.Questions[QuestionNum].AnswerArray[0];
                Two.Text = quiz.Questions[QuestionNum].AnswerArray[1];
                Three.Text = quiz.Questions[QuestionNum].AnswerArray[2];
                Four.Text = quiz.Questions[QuestionNum].AnswerArray[3];
                ColorChange();
            }
            if (QuestionNum > 0)
            {
                PreviousButton.IsVisible = true;
            }
            else
            {
                PreviousButton.IsVisible = false;
            }
            if (QuestionNum == quiz.NumQuestions - 1)
            {
                NextButton.IsVisible = false;
                BackToStory.IsVisible = true;
            }
            else
            {
                NextButton.IsVisible = true;
                BackToStory.IsVisible = false;
            }
        }

        // Handles if the next button is clicked
        private void NextQuestion(object sender, EventArgs e)
        {
            if (QuestionNum < quiz.NumQuestions - 1)
            {
                QuestionNum++;
                try
                {
                    player.Load(quiz.Questions[QuestionNum].Audio);
                    player.Play();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(quiz.Questions[QuestionNum].Audio + " does not exist!");
                }
                QuestionTitle.Text = quiz.Questions[QuestionNum].QuestionText;
                One.Text = quiz.Questions[QuestionNum].AnswerArray[0];
                Two.Text = quiz.Questions[QuestionNum].AnswerArray[1];
                Three.Text = quiz.Questions[QuestionNum].AnswerArray[2];
                Four.Text = quiz.Questions[QuestionNum].AnswerArray[3];
                ColorChange();
            }
            if (QuestionNum > 0)
            {
                PreviousButton.IsVisible = true;
            }
            else
            {
                PreviousButton.IsVisible = false;
            }
            if (QuestionNum == quiz.NumQuestions - 1)
            {
                NextButton.IsVisible = false;
                BackToStory.IsVisible = true;
            }
            else
            {
                NextButton.IsVisible = true;
                BackToStory.IsVisible = false;
            }
        }

        // Plays the audio of the question clicked
        private void PlayAudio(object sender, EventArgs e)
        {
            Button clicked = (Button)sender;
            String answer = clicked.Text.ToLower();
            String audioFile = clicked.Text + ".mp3";
            try
            {
                player.Load(audioFile);
                player.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine(audioFile + " does not exist!");
            }
        }

        // Determines if the button has been clicked most previously
        private void ClickOrPlay(object Sender, EventArgs e)
        {
            if (!PreviousAnswerSelected.Text.Equals((Sender as Button).Text))
            {
                if (!(PreviousAnswerSelected.BackgroundColor == Color.Red || PreviousAnswerSelected.BackgroundColor == Color.Green))
                {
                    PreviousAnswerSelected.BackgroundColor = Color.FromHex("#6F2DBD");
                }
            }
            PreviousAnswerSelected = (Sender as Button);
            PlayAudio(Sender, e);
            Submit.IsVisible = true;
            (Sender as Button).BackgroundColor = Color.FromHex("#965ed9");
        }

        // checks the sumbitted answer
        private void CheckAnswer(object sender, EventArgs e)
        {
            quiz.NumAttempts[QuestionNum]++;
            if (PreviousAnswerSelected.Text.Equals(quiz.Questions[QuestionNum].CorrectAnswer))
            {
                PreviousAnswerSelected.BackgroundColor = Color.Green;
                QuestionsCorrect++;
                quiz.Questions[QuestionNum].AnswerSelected[PreviousAnswerSelected.Text] = true;
                scoreCalculation[QuestionNum] = quiz.NumAttempts[QuestionNum];
                if (quiz.NumAttempts[QuestionNum] == 1)
                {
                    user.RewardsRecieved["Gold"]++;
                }
                else if (quiz.NumAttempts[QuestionNum] == 2)
                {
                    user.RewardsRecieved["Silver"]++;
                }
            }
            else
            {
                PreviousAnswerSelected.BackgroundColor = Color.Red;
                quiz.Questions[QuestionNum].AnswerSelected[PreviousAnswerSelected.Text] = true;
            }
            NumCorrect.Text = "Questions Correct: " + QuestionsCorrect;
            bool quizCompleted = true;
            for (int i = 0; i < quiz.NumQuestions; i++)
            {
                if (quiz.NumAttempts[i] < 1)
                {
                    quizCompleted = false;
                }
            }
            CalculateScore();
        }

        // Calculates the current score of the quiz
        private void CalculateScore()
        {
            quiz.Score = 0;
            for (int i = 0; i < quiz.NumQuestions; i++)
            {
                int numAttempts = scoreCalculation[i];
                switch (numAttempts)
                {
                    case 1:
                        quiz.Score += 100 / quiz.NumQuestions;
                        break;

                    case 2:
                        quiz.Score += (100 / quiz.NumQuestions) / 2;
                        break;

                    case 3:
                        quiz.Score += (100 / quiz.NumQuestions) / 3;
                        break;

                    default:
                        break;
                }
            }
        }

        // Handles the color change for correct or incorrect answers
        private void ColorChange()
        {
            Button[] buttonArray = { One, Two, Three, Four };
            for (int i = 0; i < buttonArray.Length; i++)
            {
                if (quiz.Questions[QuestionNum].AnswerSelected[buttonArray[i].Text] == true)
                {
                    if (buttonArray[i].Text.Equals(quiz.Questions[QuestionNum].CorrectAnswer))
                    {
                        buttonArray[i].BackgroundColor = Color.Green;
                    }
                    else
                    {
                        buttonArray[i].BackgroundColor = Color.Red;
                    }
                }
                else
                {
                    buttonArray[i].BackgroundColor = Color.FromHex("#6F2DBD");
                }
            }
        }

        // Navbar methods
        // Returns to the previous page
        private async void BackClicked(object sender, EventArgs e)
        {
            player.Stop();
            if (audio != null)
            {
                player.Load(audio);
            }
            await App.Current.MainPage.Navigation.PopAsync();
        }

        // Returns to the Home page
        private async void HomeClicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PopToRootAsync();
        }
    }
}