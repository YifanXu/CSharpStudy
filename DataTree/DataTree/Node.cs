using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTree
{
    class Node
    {
        public int data;
        private List<Node> childNode;

        public Node(int data)
        {
            this.data = data;
            childNode = new List<Node>();
        }

        public Node(int data, params Node[] childNodes) : this(data)
        {
            this.childNode = new List<Node>();
            this.childNode.AddRange(childNodes);
        }

        public int Count
        {
            get
            {
                return childNode.Count;
            }
        }

        Node this[int id] {
            get
            {
                return childNode[id];
            }
            set
            {
                childNode[id] = value;
            }
        }

    }
}
