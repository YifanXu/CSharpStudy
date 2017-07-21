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


            //Expression test = new Expression(new Random(), 3);
            //Console.WriteLine("{0} = {1}",test,test.Value);      

            Console.ReadLine();
        }
    }
}
