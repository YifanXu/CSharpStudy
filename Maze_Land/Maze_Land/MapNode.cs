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
        public Direction checkNext;

        public MapNode(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.checkNext = Direction.Left;
        }
    }
}
