using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Encoder
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Document doc = new Document ();

			var seralizer = new XmlSerializer (typeof(Document));
			using (Stream s = new FileStream ("int6rceptedD0cume2t.txt", FileMode.Create, FileAccess.Write)) {
				seralizer.Serialize (s, doc);
			}
			Console.WriteLine ("Done");
		}
	}
}
