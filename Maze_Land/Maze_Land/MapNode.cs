using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Land
{
    class MapNode
    {
        public readonly int x;
        public readonly int y;
        public Direction direction;

		public MapNode(int x, int y) : this(x, y, Direction.Left)
        {
        }
		public MapNode(int x, int y, Direction direction)
		{
			this.x = x;
			this.y = y;
			this.direction = direction;
		}
    }
}
