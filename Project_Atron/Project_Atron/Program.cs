using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculation_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Test c = new Test();
//            try
//            {
                c = new Test("LevelData.xml", "data.txt");
//            }
//            catch(Exception e)
//            {
//                Console.WriteLine("An exception has occured in the serialization of the program.\nDetails:\n{0}/{1}",e,e.Message);
//                Console.ReadLine();
//                return;
//            }

            try
            {
                c.Run();
            }
            catch(Exception e)
            {
                Console.WriteLine("An exception has occured in the running of the program.\nDetails:\n{0}/{1}", e, e.Message);
                Console.ReadLine();
                return;
            }

            //Expression test = new Expression(new Random(), 3);
            //Console.WriteLine("{0} = {1}",test,test.Value);      

            Console.ReadLine();
        }
    }
}
