using System;
using System .Xml;
using System.Collections.Generic;

namespace Decoder
{
	class MainClass
	{
		

		public static void Main (string[] args)
		{
			XmlDocument doc = new XmlDocument ();
			doc.Load ("data");
			string result = FindPassword (doc, "password");
			if (result == "&*NotFound*&") {
				Console.WriteLine ("Password Not Found.");
			} else {
				Console.WriteLine ("Password Is {0}", result);
			}

		}

		public static string FindPassword (XmlDocument doc, string keyword){
			Stack<Node> stack = new Stack<Node>();
			XmlNode current = doc.FirstChild;
			while (true) {
				if (stack.Count == 0) {
					stack.Push (new Node(doc.LastChild));
				}
				else if (stack.Peek ().FirstTry) {
					stack.Peek ().FirstTry = false;
					//Examining Itself
					if (stack.Peek ().node.Name == keyword) {
						return stack.Peek ().node.InnerText;
					}

					//Examining Attributes
					var attributes = stack.Peek ().node.Attributes;
					if (attributes != null) {
						for (int i = 0; i < attributes.Count; i++) {
							if (attributes [i].Name == keyword) {
								return attributes [i].Value;
							}
						}
					}
					//Get Into ChildNodes
					if (stack.Peek ().node.ChildNodes.Count != 0) {
						stack.Push (new Node (stack.Peek ().node.FirstChild));
					}

				}
				//Going Back
				else {
					Node lastNode= stack.Pop ();
					if (lastNode.node.NextSibling != null) {
						stack.Push(new Node (lastNode.node.NextSibling));
					}
					if (stack.Count == 0) {
						return "&*NotFound*&";
					}
				}
			}
		}
	}
}
