using System;
using System.Collections.Generic;
using System.Linq;

namespace ExamConsole
{
    public sealed class UserAnswer
    {
        public bool Correct { get; set; }
        public Question Question { get; set; }
        public List<string> AnswerIndexes { get; set; }

        public UserAnswer(Question question)
        {
            Question = question;
            AnswerIndexes = new List<string>();
        }

        public bool Validate(Question question)
        {
            var correctAnswers = question.CorrectAnswers();

            var missingAnswers = AnswerIndexes.Except(correctAnswers).ToList();
            var correct = missingAnswers.Count == 0;
            Correct = correct;
            return correct;
        }

        public void RenderResult()
        {
            if (Correct)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nCorrect! {this.ToString()}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nIncorrect. {this.ToString()}");
            }

            Console.ResetColor();
            Question.Show(true);
        }

        public override string ToString()
        {
            var answers = FormatStringList(AnswerIndexes);
            var correctAnswers = FormatStringList(Question.CorrectAnswers());

            return $"You answered {answers}. The correct " + (correctAnswers.Length > 1 ? "answers were " : "answer was ") + $"{correctAnswers}.";
        }

        private string FormatStringList(List<string> list)
        {
            return string.Join(", ",
                list.Take(AnswerIndexes.Count - 1)) +
                (list.Count <= 1 ? "" : " and ") +
                list.LastOrDefault();
        }
    }
}