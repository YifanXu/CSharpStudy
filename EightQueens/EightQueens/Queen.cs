using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightQueens
{
    public class Queen
    {
        public int x;
        public int y;
        public Queen nextQueen;

        public Queen()
        {

        }
        public Queen(Queen nextQueen)
        {
            this.nextQueen = nextQueen;
        }

        public void reset()
        {
            this.x = 0;
            this.y = 0;
        }

        public bool tryPosition (Map map)
        {
            for (x = 0; x < map.mapSize; x++)
            {
                for (y = 0; y < map.mapSize; y++)
                {
                    if (map[x, y] == MapTile.Empty)
                    {
                        if (nextQueen == null)
                        {
                            map.MarkSpot(x, y);
                            map.display(Program.colors);
                            return true;
                        }
                        Map newMap = new Map(map.mapSize, map.value, map.queens);
                        newMap.MarkSpot(x, y);
                        if (nextQueen.tryPosition(newMap))
                        {
                            map.value = newMap.value;
                            map.display(Program.colors);
                            return true;
                        }
                    }
                }
            }
            this.reset();
            return false;
        }
    }
}
