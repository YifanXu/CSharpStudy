using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyond_The_Stars.Locations;
using Beyond_The_Stars.Entities.Spaceship;

namespace Beyond_The_Stars
{
    public static class Game
    {
        public static string version = "Not-Even-Close";
        public static Random r = new System.Random();

        public static Dictionary<int,SolSystem> allSystems;

        public static bool ShowBaseMenu()
        {
            WriteLine((ConsoleColor)r.Next(1,16), "Beyond The Stars v."+version);
            Console.WriteLine("\n1. Start New Game\n2.Continue Last Game\n3.Credits\n4.Exit");
            int choice = GetChoice(4, false);
            switch (choice)
            {
                case 1:
                    Create();
                    break;
                case 2:
                    Initialize();
                    break;
                case 3:
                    WriteLine(ConsoleColor.Magenta, "All credits goes to Yifan! He made all of this. Yep");
                    return true;
                case 4:
                    return false;
            }
            Game.Run();
            return true;
        }

        public static bool Create()
        {
            bool confirm = false;
            //Name
            do
            {
                Console.WriteLine("Enter a name for your pilot!");
                Player.name = Console.ReadLine();
                if(String.IsNullOrWhiteSpace(Player.name))
                {
                    Console.WriteLine("You can't have nothing for a name!");
                    continue;
                }
                Console.WriteLine("So your name is {0}? (Type \"yes\" to continue)");
                confirm = (Console.ReadLine().Trim().ToLower() == "yes");
            }   
            while (!confirm);
            confirm = false;
            //Choose A Ship
            do
            {
                Console.WriteLine("Choose your starting ship!");
                WriteLine(ConsoleColor.Cyan,"1. Nothing (Escape Pod)");
                for(int i = 0; i < ShipClass.startingClasses.Length; i++)
                {
                    WriteLine(ConsoleColor.Cyan, String.Format("{0}. ShipClass", i+2));
                }
                int choice = GetChoice(ShipClass.startingClasses.Length, false);
                if(choice == 1)
                {
                    
                }
            }
            while (true);
            return true;
        }

        public static bool Initialize()
        {
            return true;
        }

        public static void Run()
        {

        }

        #region supportFunctions

        public static void WriteLine(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        
        public static int GetChoice(int upperBound, bool includeZero)
        {
            Console.WriteLine("Enter Choice 1 - {0} to make a choice", upperBound);
            if (includeZero)
            {
                Console.WriteLine("Alternatively enter 0 to return to last menu.");
            }

            string input;
            int num;
            while (true)
            {
                input = Console.ReadLine();
                if(!String.IsNullOrEmpty(input) && int.TryParse(input.Trim(), out num) && ((includeZero && num == 0) || (num > 0 && num <= upperBound)))
                {
                    return num;
                }
                Console.WriteLine("Invalid input");
            }
        }

        #endregion
    }
}
