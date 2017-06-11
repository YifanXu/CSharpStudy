using System;
using System.Xml;

namespace Decoder
{
	public class Node
	{
		/// <summary>
		/// The node.
		/// </summary>
		public XmlNode node;
		public bool FirstTry;

		/// <summary>
		/// Initializes a new instance of the <see cref="Decoder.Node"/> class.
		/// </summary>
		/// <param name="node">XML element node</param>
		public Node (XmlNode node)
		{
			this.node = node;
			FirstTry = true;
		}
	}
}

