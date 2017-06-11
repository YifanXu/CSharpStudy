using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace Translator
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string[] input = File.ReadAllLines ("data.txt");
			XmlDocument doc = Decode (input);
			doc.Save ("output.xml");

		}
		public static XmlDocument Decode (string[] input){
			

			XmlDocument doc = new XmlDocument ();
			XmlElement root = doc.CreateElement ("root");
			doc.AppendChild (root);
			XmlNode rootLevel = doc.FirstChild;
			XmlNode currentLevel = rootLevel;
			int line = 0;
			while(line < input.Length && input[line] != "END")
			{
				//Add Room
				string[] parameters = input[line].Split('|');
				string Type = "";
				if (parameters[0] == "P")
				{
					rootLevel.AppendChild(doc.CreateElement("Portal"));
					Type = "Portal";
				}
				else
				{
					rootLevel.AppendChild(doc.CreateElement("Room"));
					Type = "Room";
				}
				currentLevel = rootLevel.LastChild;
				//Attributes
				AddAttributes(doc,currentLevel,Type,parameters);
				line++;
				parameters = input[line].Split('|');
				if(Type == "Portal" & parameters[0] == "G"){
					//Add Guardian
					currentLevel.AppendChild (doc.CreateElement ("Guardian"));
					currentLevel = currentLevel.LastChild;
					AddAttributes (doc, currentLevel, "Guardian", parameters);
					//Return to previous level
					currentLevel = currentLevel.ParentNode;
					line++;
					parameters = input[line].Split('|');
				}
				//Add Items
				currentLevel.AppendChild(doc.CreateElement("Items"));
				currentLevel = currentLevel.LastChild;
				while(input[line][0] == 'I')
				{
					currentLevel.AppendChild(doc.CreateElement("Item"));
					currentLevel = currentLevel.LastChild;
					AddAttributes (doc, currentLevel, "Item", parameters);

					//Return
					currentLevel = currentLevel.ParentNode;
					line++;
					parameters = input [line].Split ('|');
				}
				currentLevel = currentLevel.ParentNode;
				//Add NPC
				currentLevel.AppendChild(doc.CreateElement("NPCs"));
				currentLevel = currentLevel.LastChild;	
				while(input[line][0] == 'N' || input[line][0] == 'M')
				{
					parameters = input[line].Split('|');
					if (input[line].StartsWith("M"))
					{
						Type = "MovingNPC";
					}
					else
					{
						if (parameters.Length == 7)
						{
							Type = "CustomNPC";
						}
						else
						{
							Type = "NormalNPC";
						}
					}
					currentLevel.AppendChild(doc.CreateElement(Type));
					currentLevel = currentLevel.LastChild;
					AddAttributes (doc, currentLevel, Type, parameters);
					//REturn
					currentLevel = currentLevel.ParentNode;
					line++;
					parameters = input [line].Split ('|');
				}
				currentLevel = currentLevel.ParentNode;
			}
			return doc;
		}

		private static void AddAttributes(XmlDocument doc, XmlNode node, string type, string[] parameters){
			Dictionary<string,string[]> nodeAttributes = new Dictionary<string, string[]> {
				{ "Room",new string[]{ "ID", "Name", "Description" } },
				{ "Portal",new string[]{ "ID", "Name", "Description", "Fee" } },
				{ "Guardian", new string[]{ "ID", "Name", "Dialouge", "Health", "Damage" } },
				{ "Item", new string[]{ "ID", "Name", "Description", "Obtainable", "obtainMessage", "roomMessage" } },
				{ "NormalNPC", new string[]{ "ID", "Name", "PositiveDialouge", "NegativeDialouge" } },
				{"CustomNPC", new string[]{ "ID", "Name", "PositiveDialouge", "NegativeDialouge", "Health", "Damage" }},
				{ "MovingNPC", new string[]{ "ID", "Name", "PositiveDialouge", "NegativeDialouge", "Stamina" } }
			};

			for (int i = 1; i < nodeAttributes [type].Length; i++) {
				var attribute = doc.CreateElement(nodeAttributes[type][i]);
				node.AppendChild (attribute);
				attribute.InnerText = parameters [i];
			}
		}
	}
}
