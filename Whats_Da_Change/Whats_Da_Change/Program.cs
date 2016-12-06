using System;

namespace Whats_Da_Change
{
	class MainClass
	{
		//args[0] = Changes need to make
		public static void Main (string[] args)
		{
            int input;
			var c = new calculation();
            if (!Int32.TryParse(args[0], out input))
            {
                Console.WriteLine("Error detected in input");
            }
            else{
                //input = int.Parse(args[0]);
                int[] results = c.calc(input);
                Console.WriteLine("{0} cents eh?");
                Console.WriteLine("I think {0} quarters, {1} dimes, {2} nickels and {3} pennies. I think.",results[0], results[1], results[2], results[3]);
            }
		}
	}
}
