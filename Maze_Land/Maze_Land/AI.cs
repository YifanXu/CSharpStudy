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
		private List<MapNode> blacklist;
        private readonly List<MapTile> ValidTile = new List<MapTile>()
        {
            MapTile.Passage,
            MapTile.Entrance,
            MapTile.Exit
        };
        private readonly Dictionary<Direction, Direction> NextDirection = new Dictionary<Direction, Direction>()
        {
            {Direction.Left, Direction.Up },
            {Direction.Up, Direction.Right },
            {Direction.Right, Direction.Down },
            {Direction.Down, Direction.None },
			{Direction.None, Direction.None}
        };

        public AI()
        {
            this.route = new Stack<MapNode>();
			this.blacklist = new List<MapNode> ();
        }

		public AI(int x, int y) : this()
        {
            this.x = x;
            this.newX = x;
            this.y = y;
            this.newY = y;
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
                route.Peek().direction = NextDirection[route.Peek().direction];
            }
            return true;
        }

        //Attempts to find exits from current block. Returns true if succeeds and automatically move there.
        private bool FindExit(Map map)
        {
            //while(the block of direction is not walkable or that direction is where the AI came from)
			while (!CheckBlock(map, route.Peek().direction)){
                route.Peek().direction = NextDirection[route.Peek().direction];
                if (route.Peek().direction == Direction.None)
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
			if(IfInBlacklist(this.x, this.y, direction))
			{
				return false;
			}
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
					int routePosition;
					if (IfInLoop (newX, newY, out routePosition)) 
					{
						if (routePosition != route.Count - 1) 
						{
							blacklist.Add (new MapNode (x, y, direction));
						}
						return false;
					}

                    return true;
                }
            }
            return false;
        }

        //Check if the AI has stepped into a block it has previously stepped into (aka LOOP)
		private bool IfInLoop(int newx, int newy, out int routePosition)
        {
            MapNode[] pastRoute = route.ToArray();
            for (int i = 0; i < route.Count - 1; i++)
            {
                if (newx == pastRoute[i].x && newy == pastRoute[i].y)
                {
					routePosition = i;
                    return true;
                }
            }
			routePosition = 0;
            return false;
        }
		private bool IfInBlacklist (int x, int y, Direction direction){
			MapNode[] blackList = this.blacklist.ToArray();
			for (int i = 0; i < blacklist.Count; i++)
			{
				if (x == blackList[i].x && y == blackList[i].y && direction == blacklist[i].direction)
				{
					return true;
				}
			}
			return false;
		}
    }
}