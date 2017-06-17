using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace DataTree
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			/*Node root = new Node (1, new Node[]{ new Node (2), new Node (3) });
			XmlSerializer seralizer = new XmlSerializer (typeof(Node));
			using (Stream s = new FileStream ("data", FileMode.Create,FileAccess.Write)) {
				seralizer.Serialize (s, root);
			}*/
			Node root = ReadIn ("data");
			int depth = getDepth (root);
			Console.WriteLine ("Depth: {0}", depth);
			List<int> numbers = getAllData (root);
			Console.WriteLine ("Data: ");
			foreach (int num in numbers) {
				Console.Write ("{0}, ", num);
			}
		}

		static Node ReadIn (string path){
			Node root;
			if (File.Exists (path)) {
				
				XmlSerializer seralizer = new XmlSerializer (typeof(Node));
				using (Stream s = new FileStream (path, FileMode.Open, FileAccess.Read)) {
					root = (Node)seralizer.Deserialize (s);
				}
			} else {
				root = new Node (0);
			}
			return root;
		}

		static int getDepth (Node root){
			if (root == null) {
				return 0;
			}
			int depth = 0;
			List<Node> layerMembers = new List<Node> ();
			List<Node> newList = new List<Node>{ root };
			while (newList.Count != 0) {
				depth++;
				layerMembers.Clear ();
				layerMembers.AddRange (newList);
				newList.Clear ();
				foreach (Node member in layerMembers) {
					newList.AddRange (member.children);
				}
			}
			return depth;
		}

		static List<int> getAllData (Node root){
			List<int> data  = new List<int> ();
			if (root != null) {
				Stack<DecodingNode> archive = new Stack<DecodingNode>();
				archive.Push (new DecodingNode (root));
				while (archive.Count != 0) {
					DecodingNode current = archive.Peek ();
					if (current.check == false) {
						current.check = true;
						data.Add (current.data.data);
						if (current.data.children.Length != 0) {
							archive.Push (new DecodingNode (current.data.children [0], current.data));
						}
					} else {
						archive.Pop ();
						if (current.NextSibling != null) {
							archive.Push (new DecodingNode(current.NextSibling,current.parent));
						}
					}
				}
			}
			return data;
		}
	}
}
