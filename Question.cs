using System;
using System.Collections.Generic;
using System.Linq;

namespace ExamConsole
{
    public class Question
    {
        public int Index { get; set; }
        public string Text { get; set; }
        public List<Answer> Answers { get; set; }

        public void Show(bool showAnswers)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"\n{Index}. ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(Text + "\n");
            Console.ResetColor();

            foreach (var a in Answers)
            {
                if (showAnswers)
                {
                    Console.ForegroundColor = a.Correct ? ConsoleColor.Green : ConsoleColor.White;
                }

                Console.WriteLine($"{a.Index}) {a.Text}");
                Console.ResetColor();
            }
        }

        public List<string> CorrectAnswers()
        {
            var correctAnswers = Answers.Where(a => a.Correct).Select(a => a.Index).ToList();
            return correctAnswers;
        }
    }
}