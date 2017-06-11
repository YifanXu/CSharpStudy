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
			//Find the Connection Line
			int endLine = 0;
			while (input [endLine] != "END") {
				if (endLine == input.Length - 1) {
					endLine = -1;
					break;
				}
				endLine++;
			}
			//Find Portal Line
			int portalLine = endLine;
			if (portalLine != -1) {
				while (!input [portalLine].StartsWith ("P")) {
					if (portalLine == input.Length - 1) {
						portalLine = -1;
						break;
					}
					portalLine++;
				}
			}

			//Actual Reading Proccess
			int line = 0;
			while(line < input.Length && input[line] != "END")
			{
				//Add Room
				string[] parameters = input[line].Split('|');
				nodeType type = nodeType.Room;
				if (parameters[0] == "P")
				{
					type = nodeType.Portal;
				}
				else
				{
					type = nodeType.Room;
				}
				rootLevel.AppendChild(doc.CreateElement(type.ToString()));
				//Setting Up Connections
				rootLevel.LastChild.AppendChild (doc.CreateElement ("Connections"));
				string[] roomConnections = FindConnections (input, parameters [1], endLine);
				AddAttributes(doc,rootLevel.LastChild.LastChild,nodeType.Connections, roomConnections);
				//Setting Up Portal Connection
				if (type == nodeType.Portal) {
					rootLevel.LastChild.AppendChild (doc.CreateElement ("PortalDesination"));
					rootLevel.LastChild.LastChild.InnerText = FindPortalConnections (input, parameters [1], portalLine);
				}
				currentLevel = rootLevel.LastChild;
				//Attributes
				AddAttributes(doc,currentLevel,type,parameters);
				line++;
				parameters = input[line].Split('|');
				if(type == nodeType.Portal & parameters[0] == "G"){
					//Add Guardian
					currentLevel.AppendChild (doc.CreateElement ("Guardian"));
					currentLevel = currentLevel.LastChild;
					AddAttributes (doc, currentLevel, nodeType.Guardian, parameters);
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
					AddAttributes (doc, currentLevel, nodeType.Item, parameters);

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
						type = nodeType.MovingNPC;
					}
					else
					{
						if (parameters.Length == 7)
						{
							type = nodeType.CustomNPC;
						}
						else
						{
							type = nodeType.NormalNPC;
						}
					}
					currentLevel.AppendChild (doc.CreateElement (type.ToString ()));
					currentLevel = currentLevel.LastChild;
					AddAttributes (doc, currentLevel, type, parameters);
					//REturn
					currentLevel = currentLevel.ParentNode;
					line++;
					parameters = input [line].Split ('|');
				}
				currentLevel = currentLevel.ParentNode;
			}
			return doc;
		}

		private static void AddAttributes(XmlDocument doc, XmlNode node, nodeType type, string[] parameters){
			Dictionary<nodeType,string[]> nodeAttributes = new Dictionary<nodeType, string[]> {
				{ nodeType.Room,new string[]{ "ID", "Name", "Description" } },
				{ nodeType.Portal,new string[]{ "ID", "Name", "Description", "Fee" } },
				{ nodeType.Guardian, new string[]{ "ID", "Name", "Dialouge", "Health", "Damage" } },
				{ nodeType.Item, new string[]{ "ID", "Name", "Description", "Obtainable", "obtainMessage", "roomMessage" } },
				{ nodeType.NormalNPC, new string[]{ "ID", "Name", "PositiveDialouge", "NegativeDialouge" } },
				{ nodeType.CustomNPC, new string[]{ "ID", "Name", "PositiveDialouge", "NegativeDialouge", "Health", "Damage" }},
				{ nodeType.MovingNPC, new string[]{ "ID", "Name", "PositiveDialouge", "NegativeDialouge", "Stamina" } },
				{ nodeType.Connections, new string[]{"North","West","East","South"}}
			};

			for (int i = 1; i < nodeAttributes [type].Length; i++) {
				var attribute = doc.CreateElement(nodeAttributes[type][i - 1]);
				node.AppendChild (attribute);
				attribute.InnerText = parameters [i];
			}
		}
		/// <summary>
		/// Finds the connections of a room.
		/// </summary>
		/// <returns>The connections.</returns>
		/// <param name="input">inputText.</param>
		/// <param name="id">Identifier.</param>
		/// <param name="connectionLine">The line where the connection description start.</param>
		private static string[] FindConnections (string[] input, string id, int endLine){
			Dictionary<string,int> directions = new Dictionary<string, int> () {
				{ "N",0 },
				{ "W",1 },
				{ "E",2 },
				{ "S",3 }
			};
			string[] connections = new string[4];
			if (endLine != -1) {
				for (int i = endLine + 1; i < input.Length; i++) {
					string[] parameters = input[i].Split ('|');
					if (parameters [0] == id) {
						connections [directions [parameters [1]]] = parameters [2];
					}
				}
			}
			return connections;
		}

		private static string FindPortalConnections (string[] input, string id, int portalLine){
			string connection = "";
			if (portalLine != -1) {
				for (int i = portalLine + 1; i < input.Length; i++) {
					string[] parameters = input[i].Split ('|');
					if (parameters [1] == id) {
						connection = parameters[2];
						return connection;
					}
				}
			}
			return "";
		}
	}
}
