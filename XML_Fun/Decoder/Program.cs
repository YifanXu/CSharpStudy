using System;
using System .Xml;
using System.Collections.Generic;

namespace Decoder
{
	class MainClass
	{
		

		public static void Main (string[] args)
		{
			bool itemFound = false;
			XmlDocument doc = new XmlDocument ();
			doc.Load ("data");
			//Ask for keyword
			Console.WriteLine("Enter the keyword!");
			string keyword = Console.ReadLine ();
			if (String.IsNullOrEmpty (keyword)) {
				keyword = "password";
			}
			//Getting The Result
			string result = TryFindKeyword (doc, keyword, out itemFound);
			//Displaying the result
			if (!itemFound) {
				Console.WriteLine ("Keyword \"{0}\" was not Found.", keyword);
			} else {
				Console.WriteLine ("{0} Is {1}", keyword, result);
			}
		}

		public static string TryFindKeyword (XmlDocument doc, string keyword, out bool foundValue){
			//Set Up the stack
			Stack<Node> stack = new Stack<Node>();
			stack.Push (new Node(doc.FirstChild));
			while (true) {
				if (stack.Peek ().FirstTry) {
					stack.Peek ().FirstTry = false;
					//Examining Itself
					if (String.Equals(stack.Peek ().node.Name, keyword, StringComparison.OrdinalIgnoreCase)) {
						foundValue = true;
						return stack.Peek ().node.InnerText;
					}

					//Examining Attributes
					var attributes = stack.Peek ().node.Attributes;
					if (attributes != null) {
						//Looking through all the attributes
						for (int i = 0; i < attributes.Count; i++) {
							if (String.Equals(attributes [i].Name, keyword, StringComparison.OrdinalIgnoreCase)) {
								foundValue = true;
								return attributes [i].Value;
							}
						}
					}
					//Get Into ChildNodes
					if (stack.Peek ().node.ChildNodes.Count != 0) {
						stack.Push (new Node(stack.Peek ().node.FirstChild));
					} 
				}else {
					//Get Into Siblings/Parents
					Node lastNode = stack.Pop ();
					if (lastNode.node.NextSibling != null) {
						//If the node have a sibling, we will examine that sibling next. Else, we return to the parent level.
						stack.Push(new Node(lastNode.node.NextSibling));
					}
					//If we run out of things, too bad lol
					if (stack.Count == 0) {
						foundValue = false;
						return String.Empty;
					}
				}
			}
		}
	}
}
