using System;

namespace Sorted_Linked_Boys
{
	public class Item
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
				Item root;
				string input;

				while(true)
				{
					//Input
					input = Console.ReadLine();
					//Decide if it's a signal to end list
					if(input == "*")
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("List with {0} items Complete.", itemCount);
						Console.ResetColor();
						break;
					}

					//Validate input
					int num = GetNumber(input);
					//Creating root
					//Creating a new item
					var newItem = new Item {
						number = num
					};
					root = AddNewItem (newItem, root);
				}

				//Print out number sequence
				printList(root);
			}
		}

		public static Item AddNewItem (Item newItem, Item root){
			Item current = root;
			Item prev = null;
			while (true) {
				if ((current == null) ||(newItem.number < current.number)) {
					newItem.next = current;
					if (prev == null) {
						root = newItem;
					} else {
						prev.next = newItem;
					}
					break;
				}

				prev = current;
				current = current.next;
			}

			return root;
		}

		public static void printList (Item root){
			Console.ForegroundColor = ConsoleColor.Green;

			Item current = root;
			while (current != null) {
				if (current.next != null) {
					Console.Write ("{0} <= ", current.number);
				} else {
					Console.Write ("{0}", current.number);
				}
				current = current.next;
			}
			Console.WriteLine ();
			Console.ResetColor ();
		}

		public static int GetNumber(string input){
			int num;
			while (!int.TryParse(input, out num))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Invalid Input. Please Try again.");
				Console.ResetColor();
				input = Console.ReadLine();
			}
			return num;
		}
	}
}
