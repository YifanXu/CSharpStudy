

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linked_Boys
{
	class Item
	{
		public int number;
		public Item next;
	}
	class Program
	{
		static void Main(string[] args)
		{
			while (true)
			{
				Item root = new Item { };
				string input;
				Item current = root;
				int itemCount = 0;

				while(true)
				{
					input = Console.ReadLine();
					if(input == "*")
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("List with {0} items Complete.", itemCount);
						Console.ResetColor();
						break;
					}
					Item newItem = new Item { };
					int num;
					while (!int.TryParse(input, out num))
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Invalid Input. Please Try again.");
						Console.ResetColor();
						input = Console.ReadLine();
					}
					if (itemCount == 0)
					{
						root.number = num;
					}
					else
					{
						newItem = new Item {
							number = num
						};
						current.next = newItem;
						current = current.next;
					}
					itemCount++;
				}

				//Calculation for min and max
				current = root;
				int min = root.number;
				int max = root.number;
				while(current.next != null)
				{
					current = current.next;
					if(current.number < min)
					{
						min = current.number;
					}
					else if(current.number > max)
					{
						max = current.number;
					}
				}
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Max is {0}", max);
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Min is {0}", min);
				Console.ResetColor ();
			}
		}
	}
}


