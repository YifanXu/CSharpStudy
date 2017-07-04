using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Atron
{
    class Program
    {
        static void Main(string[] args)
        {
            Calculation c = new Calculation("data.txt");
            c.Display();            

            Console.ReadLine();
        }

        
    }
}
