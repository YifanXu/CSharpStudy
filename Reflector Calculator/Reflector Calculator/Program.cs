using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Reflector_Calculator
{
    class Program
    {
        private static Calculator calculator = new Calculator();

        static void Main(string[] args)
        {
            while (true)
            {
                string input = Console.ReadLine();
                execute(input);
            }
        }

        public static void execute (string input)
        {
            string[] parameters = input.Split(' ');
            if(parameters.Length != 3)
            {
                Console.WriteLine("SYSTEM: - Incorrect Input");
                return;
            }
            var methods = calculator.GetType().GetMethods();
            foreach(var method in methods)
            {
                if(MatchMethodName(parameters[1],method))
                {
                    int num1 = 0;
                    int num2 = 0;
                    if(!int.TryParse(parameters[0], out num1) || !int.TryParse(parameters[2],out num2))
                    {
                        Console.WriteLine("SYSTEM: - Invalid Numbers");
						return;
                    }
                    Console.WriteLine("SYSTEM: -Result is {0}", method.Invoke(calculator, new object[] { num1, num2 }));
                    return;
                }
            }
            Console.WriteLine("SYSTEM: - Invalid Operation");
        }

        public static bool MatchMethodName(string methodName, System.Reflection.MethodInfo method)
        {
            if (String.Equals(methodName, method.Name, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            NicknameAttribute attibute = (NicknameAttribute)method.GetCustomAttribute(typeof(NicknameAttribute));

			if (attibute == null) {
				return false;
			}

			foreach (string nickname in attibute.names)
            {
                if (String.Equals(methodName,nickname,StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
