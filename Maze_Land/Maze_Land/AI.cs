using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Land
{
    class AI
    {
        public int x;
        public int y;
        public Stack<MapNode> route;
        private int newX;
        private int newY;
        private readonly List<MapTile> ValidTile = new List<MapTile>()
        {
            MapTile.Passage,
            MapTile.Entrance,
            MapTile.Exit
        };
        private readonly Dictionary<Direction, Direction> OppositeDirections = new Dictionary<Direction, Direction>()
        {
            {Direction.Left, Direction.Right },
            {Direction.Right, Direction.Left },
            {Direction.Up, Direction.Down },
            {Direction.Down, Direction.Up }
        };
        private readonly Dictionary<Direction, Direction> NextDirection = new Dictionary<Direction, Direction>()
        {
            {Direction.Left, Direction.Up },
            {Direction.Up, Direction.Right },
            {Direction.Right, Direction.Down },
            {Direction.Down, Direction.None }
        };

        public AI()
        {
            this.route = new Stack<MapNode>();
        }

        public AI(int x, int y)
        {
            this.x = x;
            this.newX = x;
            this.y = y;
            this.newY = y;
            this.route = new Stack<MapNode>();
        }

        public bool Operate(Map map)
        {
            this.newX = x;
            this.newY = y;
            if (!FindExit(map))
            {
                if(route.Count == 1)
                {
                    return false;
                }
                route.Pop();
                this.x = route.Peek().x;
                this.y = route.Peek().y;
                route.Peek().checkNext = NextDirection[route.Peek().checkNext];
            }
            return true;
        }

        //Attempts to find exits from current block. Returns true if succeeds and automatically move there.
        private bool FindExit(Map map)
        {
            //while(the block of direction is not walkable or that direction is where the AI came from)
            while (!CheckBlock(map, route.Peek().checkNext) || IfInLoop(newX, newY)){
                if (route.Peek().checkNext == Direction.None)
                {
                    return false;
                }
                route.Peek().checkNext = NextDirection[route.Peek().checkNext];
                if (route.Peek().checkNext == Direction.None)
                {
                    return false;
                }
            }
            this.x = newX;
            this.y = newY;
            route.Push(new MapNode(x, y));

            return true;
        }

        //Check if a block of a certain direction from the current block. Returns true if that block is movable.
        private bool CheckBlock(Map map, Direction direction)
        {
            MapTile targetTile;
            newX = x;
            newY = y;
            switch (direction)
            {
                case Direction.Left:
                    newX = x - 1;
                    break;
                case Direction.Up:
                    newY = y - 1;
                    break;
                case Direction.Right:
                    newX = x + 1;
                    break;
                case Direction.Down:
                    newY = y + 1;
                    break;
            }
            targetTile = map[newY, newX];
            foreach(MapTile type in ValidTile)
            {
                if(targetTile == type)
                {
                    return true;
                }
            }
            return false;
        }

        //Check if the AI has stepped into a block it has previously stepped into (aka LOOP)
<<<<<<< HEAD
        private bool IfInLoop(out int routeSequence)
		{
			for (int i = 0; i < route.Count; i++) {
				if (x == route [i].x && y == route [i].y) {
					routeSequence = i;
					return true;
				}
			}
			routeSequence = 0;
			return false;
		}
=======
        private bool IfInLoop(int newx, int newy)
        {
            MapNode[] pastRoute = route.ToArray();
            for (int i = 0; i < route.Count - 1; i++)
            {
                if (newx == pastRoute[i].x && newy == pastRoute[i].y)
                {
                    return true;
                }
            }
            return false;
        }
>>>>>>> 6722f27e0f12ad49ca8865d0949d7a037d406ddc
    }
}