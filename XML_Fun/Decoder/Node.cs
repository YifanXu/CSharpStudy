using System;
using System.Xml;

namespace Decoder
{
	public class Node
	{
		public XmlNode node;
		public bool FirstTry;
		public Node (XmlNode node)
		{
			this.node = node;
			FirstTry = true;
		}
	}
}

