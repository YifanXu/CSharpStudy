using System;
using System.Collections.Generic;

namespace DataTree
{
	public class Node
	{
		public int data;
		public Node[] children;

		public Node(){
		}

		public Node (int data) : this()
		{
			this.data = data;
		}

		public Node (int data, Node[] children) : this (data) {
			this.children = children;
		}

		public Node this[int id] {
			get{
				return children [id];
			}
		}
	}
}

