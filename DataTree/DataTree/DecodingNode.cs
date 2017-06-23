using System;

namespace DataTree
{
	public class DecodingNode
	{
		public readonly Node data;
		public bool check;
		public readonly Node parent;

		public DecodingNode (Node data)
		{
			this.data = data;
			this.check = false;
		}

		public DecodingNode (Node data, Node parent) : this(data){
			this.parent = parent;
		}

		public Node NextSibling {
			get {
				if(parent == null){
					return null;
				}
				for (int i = 0; i < parent.children.Length - 1; i++) {
					if (this.data == parent.children[i]) {
						return parent.children [i + 1];
					}
				}
				return null;
			}
		}
	}
}

