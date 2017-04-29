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
        public List<MapNode> route;
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
            this.route = new List<MapNode>();
        }

        public AI(int x, int y)
        {
            this.x = x;
            this.newX = x;
            this.y = y;
            this.newY = y;
            this.route = new List<MapNode>();
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
                route.RemoveAt(route.Count - 1);
                this.x = route[route.Count - 1].x;
                this.y = route[route.Count - 1].y;
                route[route.Count - 1].checkNext = NextDirection[route[route.Count - 1].checkNext];
            }
            return true;
        }

        //Attempts to find exits from current block. Returns true if succeeds and automatically move there.
        private bool FindExit(Map map)
        {
            Direction checkNext = route[route.Count - 1].checkNext;
            //while(the block of direction is not walkable or that direction is where the AI came from)
            while (!CheckBlock(map, checkNext) || (route.Count > 1 && OppositeDirections[checkNext] == route[route.Count - 2].checkNext)){
                route[route.Count - 1].checkNext = NextDirection[checkNext];
                checkNext = route[route.Count - 1].checkNext;
                if (route[route.Count - 1].checkNext == Direction.None)
                {
                    return false;
                }
            }
            this.x = newX;
            this.y = newY;
            route.Add(new MapNode(x, y));

            //Anti-Loop Mechanism
            int routeLoopStart;
            if (IfInLoop(out routeLoopStart))
            {

            }
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
        private bool IfInLoop(out int routeSequence)
        {
            for (int i = 0; i < route.Count; i++)
            {
                if (x == route[i].x && y == route[i].y)
                {
                    routeSequence = i;
                    return true;
                }
            }
            routeSequence = 0;
            return false;
    }
}
