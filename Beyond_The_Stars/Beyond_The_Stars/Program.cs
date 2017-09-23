using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyond_The_Stars
{
    class Program
    {
        static void Main(string[] args)
        {
            while (Game.ShowBaseMenu())
            {
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}
