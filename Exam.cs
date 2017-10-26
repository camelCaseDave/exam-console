using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExamConsole
{
    public sealed class Exam
    {
        public int Score { get; set; }
        public int QuestionCount { get; set; }

        public bool ShowAnswers { get; set; }

        public Question CurrentQuestion { get; set; }

        public List<Question> Questions { get; set; }
        public List<UserAnswer> UserAnswers { get; set; }

        public void Begin()
        {
            LoadPaper();
            SetQuestions();
            ToggleShowAnswers();
            UserAnswers = UserAnswers ?? new List<UserAnswer>();
            CurrentQuestion = Questions.FirstOrDefault();
            ShowQuestion();
        }

        public void End()
        {
            ShowResults();

            Console.WriteLine("\nThank you for taking this practice exam. Press any key to exit.");
            Console.ReadKey();
        }

        private void ShowResults()
        {
            var percentage = ((decimal)Score / QuestionCount) * 100;
            var pass = percentage >= 70;

            Console.WriteLine((pass ? "\nCongratulations" : "\nOh no") + $", you scored {Score} out of {QuestionCount}.");
            Console.ForegroundColor = pass ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine($"\nThat's a " + (pass ? "pass" : "fail") + $" at {percentage.ToString("0.##")}%");
            Console.ResetColor();

            Console.WriteLine("\nWould you like to see your detailed resuls? (y/n)");
            var showResults = Console.ReadLine().ToLower() == "y";

            if (showResults)
            {
                RenderResults();
            }
        }

        private void RenderResults()
        {
            foreach (var a in UserAnswers)
            {
                a.RenderResult();
                Console.ReadKey();
            }
        }

        private void LoadPaper()
        {            
            var retry = true;

            while (retry)
            {
                try
                {
                    var examName = PromptForExamName();

                    using (StreamReader r = new StreamReader(examName))
                    {
                        retry = false;
                        var json = r.ReadToEnd();
                        Questions = JsonConvert.DeserializeObject<List<Question>>(json);
                    }
                }
                catch (IOException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message + "\n");
                    Console.ResetColor();
                }
            }

            QuestionCount = Questions.Count;
        }

        private void SetQuestions()
        {
            Console.WriteLine($"\nThere are {QuestionCount} questions available. How many would you like to answer?");
            QuestionCount = Convert.ToInt32(Console.ReadLine());
            ScrambleQuestions();
            TruncateQuestions();
            ReindexQuestions();
        }

        private void ToggleShowAnswers()
        {
            Console.WriteLine("\nWould you like answers to be highlighted during the practice exam? (y/n)");
            var result = Console.ReadLine();
            ShowAnswers = result.ToLower() == "y";
        }

        private void ShowQuestion()
        {
            CurrentQuestion.Show(ShowAnswers);
            ListenForAnswer();
        }

        private void ListenForAnswer()
        {
            var requiredAnswers = CurrentQuestion.CorrectAnswers().Count;

            var userAnswer = new UserAnswer(CurrentQuestion);

            do
            {
                userAnswer.AnswerIndexes.Add(Console.ReadLine());
            }
            while (userAnswer.AnswerIndexes.Count < requiredAnswers);

            if (userAnswer.Validate(CurrentQuestion))
            {
                Score++;
            }

            UserAnswers.Add(userAnswer);
            NextQuestion();
        }

        private void NextQuestion()
        {
            CurrentQuestion = Questions.FirstOrDefault(q => q.Index == CurrentQuestion.Index + 1);

            if (CurrentQuestion != null)
            {
                ShowQuestion();
            }
            else
            {
                End();
            }
        }

        private void ReindexQuestions()
        {
            var i = 1;

            foreach (var q in Questions)
            {
                q.Index = i++;
            }
        }

        private void ScrambleQuestions()
        {
            var rnd = new Random();
            Questions = Questions.OrderBy(q => rnd.Next()).ToList();
        }

        private void TruncateQuestions()
        {
            Questions.RemoveRange(QuestionCount, Questions.Count - (QuestionCount));
        }

        private string PromptForExamName()
        {
            Console.WriteLine("Please enter the name of your exam .json file:");
            var examName = Console.ReadLine();

            if (!examName.EndsWith(".json"))
            {
                examName += ".json";
            }

            return examName;
        }
    }
}