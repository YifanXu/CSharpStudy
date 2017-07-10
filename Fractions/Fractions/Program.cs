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
            Fraction a = new Fraction(5, 6);
            Fraction b = new Fraction(3, -4);
            int c = 7;
            Fraction d = new Fraction(8,4);

            Console.WriteLine(a.ApproxInt);
            Console.WriteLine(a + c);
            Console.WriteLine(a / b);
            Console.WriteLine(d.Equals(2));

            Console.ReadLine();

            
        }
    }
}
