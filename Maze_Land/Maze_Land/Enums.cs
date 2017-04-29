using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Land
{
    public enum MapTile
    {
        Wall,
        Passage,
        Entrance,
        Exit,
        IndestructibleWall
    }

    public enum Direction
    {
        Left,
        Up,
        Right,
        Down,
        None
    }
}
