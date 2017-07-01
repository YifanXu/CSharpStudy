using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fractions
{
    class Program
    {
        static void Main(string[] args)
        {
            Fraction test = new Fraction(1, 3);
            Console.WriteLine(test.approxInt.ToString());
            Console.WriteLine(test.approxDecimal.ToString());

            Console.ReadLine();
        }
    }
}
