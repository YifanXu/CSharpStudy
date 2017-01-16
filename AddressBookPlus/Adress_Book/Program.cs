using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;

namespace AddressBookPlus
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var book = new List<string>();
			string path = "../../../data/" + ConfigurationManager.AppSettings["deafultOpen"] + ".txt";
			if (args.Length == 0) {
				if (!File.Exists (path)) {
					Console.WriteLine ("Default File does not exist");
					return;
				}
			} else if (args [0] == "*manual") {
				Console.WriteLine ("No File Loaded.");
			}else{
				path = "../../../data/" + args [0] + ".txt"; 
				if (File.Exists (path)) {
					book.AddRange(File.ReadAllLines (path));
					book.Sort ();
					Console.WriteLine ("File Loaded with {0} items.",book.Count);
				} else {
					Console.WriteLine ("File path error.");
					return;
				}
			}

			//Actual Interface
			var commandList = new Dictionary<string,Func<List<string>,string, bool>>(StringComparer.OrdinalIgnoreCase){
				{"search",SearchList},
				{"s", SearchList},
				{"startwith", startwith},
				{"sw", startwith},
				{"exact", BinarySearch},
				{"?",BinarySearch},
				{"add",AddToList},
				{"a", AddToList},
				{"+", AddToList},
				{"remove", RemoveFromList},
				{"r", RemoveFromList},
				{"delete", RemoveFromList},
				{"d", RemoveFromList},
				{"all", ListAll},
				{"exit", Exit}
			};
				
			while (true) {
				string input = Console.ReadLine ();
				string command;
				int spacePosition = input.IndexOf (" ");
				if (spacePosition < 0) {
					command = input;
					input = null;
				} else {
					command = input.Substring (0, spacePosition);
					input = input.Substring (spacePosition + 1);
				}
				Func<List<string>,string, bool> func;
				if (commandList.TryGetValue (command, out func)) {
					if (func (book, input)) {
						break;
					}
				} else {
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine ("Invalid command");
					Console.ResetColor ();
				}
			}
		}

		public static bool Exit(List<string> book, string keyword){
			Console.WriteLine ("Program Exited.");
			return true;
		}

		public static bool ListAll(List<string> book, string keyword){
			for (int i = 0; i < book.Count; i++) {
				Console.WriteLine (" {0}. {1}", i + 1, book [i]);
			}
//			foreach (string line in book) {
//				Console.WriteLine (" - " + line);
//			}
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine ("All Elements displayed");
			Console.ForegroundColor = ConsoleColor.White;
			return false;
		}

		public static bool SearchList (List<string> book, string keyword){
			if (keyword == null) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine ("Argument Does Not Exist");
			} else {
				int resultCount = 0;
				for (int i = 0; i < book.Count; i++) {
					if (book [i].IndexOf (keyword, StringComparison.OrdinalIgnoreCase) != -1) {
						Console.WriteLine (" - " + book [i]);
						resultCount++;
					}
				}
				if (resultCount > 0) {
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine ("{0} results found.", resultCount);
				} else {
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine ("No results found");
				}
			}
			Console.ResetColor ();
			return false;
		} 

		public static bool BinarySearch (List<string>book,string keyword){
			if (keyword == null) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine ("Argument Does Not Exist");
			} else if (book.Count == 0) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine ("List is empty, item cannot be found.");
			} else {
				int min = 0;
				int max = book.Count - 1;
				while (min <= max) {
					int middle = min + (max - min) / 2;
					string attemptPhrase = book [middle];
					switch (keyword.CompareTo (attemptPhrase)) {
					case 0:
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine ("Item '{0}' found at Postion {1}", attemptPhrase, middle + 1);
						Console.ResetColor();
						return false;
					case 1:
						min = middle + 1;
						break;
					default:
						max = middle - 1;
						break;
					}
				}
			}
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine ("Item does not exist.");
			Console.ResetColor();
			return false;
		}

		public static bool startwith (List<string> book, string keyword){
			if (keyword == null) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine ("Argument Does Not Exist");
			} else {
				int resultCount = 0;
				for (int i = 0; i < book.Count; i++) {
					if (book [i].StartsWith (keyword, StringComparison.OrdinalIgnoreCase)) {
						Console.WriteLine (" - " + book [i]);
						resultCount++;
					}
				}
				if (resultCount > 0) {
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine ("{0} results found.", resultCount);
				} else {
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine ("No results found");
				}
			}
			Console.ResetColor ();
			return false;
		}

		public static bool AddToList (List<string> book, string element){
			if (element == null) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine ("Argument Does Not Exist");
			} else {
				var index = book.FindIndex (s => element.Equals (s, StringComparison.OrdinalIgnoreCase));
				if (index < 0) {
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine ("'{0}' Added.", element);
					book.Add (element);
					book.Sort ();
				} else {
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine ("Value Already Exists");
				}
			}

			Console.ResetColor ();
			return false;
		}

		public static bool RemoveFromList (List<string> book, string element){
			if (element == null) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine ("Argument Does Not Exist");
			} else {
				var index = book.FindIndex (s => element.Equals (s, StringComparison.OrdinalIgnoreCase));
				if (index >= 0) {
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine ("'{0}' Removed.", element);
					book.RemoveAt (index);
				} else {
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine ("Value Does Not Exist");
				}
			}
			Console.ResetColor ();
			return false;
		}
	}
}
