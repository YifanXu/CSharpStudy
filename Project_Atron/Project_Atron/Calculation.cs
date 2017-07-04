using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace Project_Atron
{
    public class Calculation
    {
        public int[] questionsNeeded;
        public int totalQuestions = 0;
        public Question[] questions;
        public int[] points;

        public Calculation(string path)
        {
            //ReadIn Question Amounts
            string[] input = File.ReadAllLines("data.txt");
            questionsNeeded = new int[5];
            
            for(int i = 0; i < questionsNeeded.Length || i < input.Length; i++)
            {
                int questionNeededOnLevel = int.Parse(input[i]);
                totalQuestions += questionNeededOnLevel;
                questionsNeeded[i] = questionNeededOnLevel;
            }

            //Generate Questions
            questions = new Question[totalQuestions];
            int count = 0;
            for(int i = 0; i < questionsNeeded.Length; i++)
            {
                while(questionsNeeded[i] > 0)
                {
                    questions[count] = new Question(i+1);
                    questionsNeeded[i]--;
                    count++;
                }
            }
        }

        public void Display()
        {
            points = new int[totalQuestions];
            for(int i = 0; i < points.Length; i++)
            {
                Question current = questions[i];
                //Console.SetCursorPosition(0, 0);
                Write(ConsoleColor.Cyan,String.Format("Question {0}/{1} (Level {2})",i + 1,totalQuestions, current.level));
                DateTime startTime = DateTime.Now;
                Write(ConsoleColor.White, current.displayer);
                string answer = Console.ReadLine();
                DateTime endTime = DateTime.Now;
                points[i] = (endTime.Hour - startTime.Hour) * 3600 + (endTime.Minute - startTime.Minute) * 60 + endTime.Second - startTime.Second;
                int parsedAnswer;
                int correctAnswer = current.Answer;
                if(!int.TryParse(answer, out parsedAnswer) || parsedAnswer != correctAnswer)
                {
                    points[i] = 0;
                    Write(ConsoleColor.Red, String.Format("Incorrect or invalid answer (The answer is {0}).", correctAnswer));
                }
                else
                {
                    Write(ConsoleColor.Green, String.Format("Correct! You solved it in {0} seconds.", points[i]));
                }
            }
        }

        private void Write(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}