﻿using System;
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
            Soduku test = new Soduku("Soduku4.txt");
            if (test.TrySolve())
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }

            test.Display();

            Console.ReadLine();
        }
    }
}
