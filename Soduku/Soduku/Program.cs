using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soduku
{
    class Program
    {
        static void Main(string[] args)
        {
            Soduku test = new Soduku("Soduku1.txt");
			if (test.SolveByBruteForce())
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }

            test.Display();

            Console.ReadLine();
        }
    }
}
