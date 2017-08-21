using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
namespace Calculation_Test
{
    public class Test
    {
        public int[] questionsNeeded;
        public int totalQuestions = 0;
        public Expression[] questions;
        public int[] points;

        public Test()
        {

        }

        public Test(string levelPath, string path)
        {
            Random r = new Random();
            //Seralize Levels
            XmlSerializer serializer = new XmlSerializer(typeof(Level[]));
            using (Stream s = new FileStream(levelPath, FileMode.Open, FileAccess.Read))
            {
                Expression.Levels = serializer.Deserialize(s) as Level[];
            }

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
            questions = new Expression[totalQuestions];
            int count = 0;
            for(int i = 0; i < questionsNeeded.Length; i++)
            {
                while(questionsNeeded[i] > 0)
                {
                    questions[count] = new Expression(r, i);
                    questionsNeeded[i]--;
                    count++;
                }
            }
        }

        public void Run()
        {
            int correctQuestions = 0;
            points = new int[totalQuestions];
            for(int i = 0; i < points.Length; i++)
            {
                Expression current = questions[i];
                //Console.SetCursorPosition(0, 0);
                Write(ConsoleColor.Cyan,String.Format("Question {0}/{1} (Level {2})",i + 1,totalQuestions, current.level));
                DateTime startTime = DateTime.Now;
                Write(ConsoleColor.White, current.ToString());
                string answer = Console.ReadLine();
                DateTime endTime = DateTime.Now;
                points[i] = (endTime.Hour - startTime.Hour) * 3600 + (endTime.Minute - startTime.Minute) * 60 + endTime.Second - startTime.Second;
                int parsedAnswer;
                int correctAnswer = current.Value;
                if(!int.TryParse(answer, out parsedAnswer) || parsedAnswer != correctAnswer)
                {
                    points[i] = 0;
                    Write(ConsoleColor.Red, String.Format("Incorrect or invalid answer (The answer is {0}).", correctAnswer));
                }
                else
                {
                    correctQuestions++;
                    Write(ConsoleColor.Green, String.Format("Correct! You solved it in {0} seconds.", points[i]));
                }
                Write(ConsoleColor.White, "Type \"Enter\" to continue...");
                Console.ReadLine();
                Console.Clear();
            }

            Write(ConsoleColor.Cyan, String.Format("You have finished with a accuracy score of {0}/{1} ({2}%)", correctQuestions, totalQuestions, correctQuestions*100/totalQuestions));
            Write(ConsoleColor.White, "Type \"Retry\" to try again");
            if(Console.ReadLine().ToLower() == "retry")
            {
                Console.Clear();
                Run();
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