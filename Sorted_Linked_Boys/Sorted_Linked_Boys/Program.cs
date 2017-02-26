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
				Item root = new Item { };
				string input;
				int itemCount = 0;

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
					if (itemCount == 0)
					{
						root.number = num;
					}
					else
					{
						//Creating a new item
						var newItem = new Item {
							number = num
						};
						AddNewItem (newItem, ref root);
					}
					itemCount++;
				}

				//Print out number sequence
				printList(root);
			}
		}

		public static void AddNewItem (Item newItem, ref Item root){
			//In case new item is before root
			if (newItem.number < root.number) 
			{
				newItem.next = root;
				root = newItem;
			} 
			else 
			{
				//If there is only one item and new item is bigger than root
				if (root.next == null) 
				{
					root.next = newItem;
				} 
				else 
				{
					Item searchCurrent = root.next;
					//Make sure it's not the end of list
					while (searchCurrent.next != null) 
					{
						//If the item found something bigger than itself...
						if (newItem.number < searchCurrent.next.number) 
						{
							newItem.next = searchCurrent.next;
							break;
						}
						searchCurrent = searchCurrent.next;
					}
					searchCurrent.next = newItem;
				}
			}
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
