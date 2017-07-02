using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightQueens
{
    public class Map
    {
        public MapTile[,] value;
        public Queen[] queens;

        public int mapSize;

        public Map(int size, int numberOfQueens)
        {
            this.mapSize = size;
            value = new MapTile[size, size];
            queens = new Queen[numberOfQueens];
            queens[numberOfQueens - 1] = new Queen();
            for(int i = numberOfQueens - 2; i >= 0; i--)
            {
                queens[i] = new Queen(queens[i + 1]);
            }
        }

        public Map(int size, MapTile[,] premadeMap, Queen[] queens)
        {
            mapSize = size;
            value = new MapTile[size, size];
            for(int x = 0; x < mapSize; x++)
            {
                for(int y = 0; y < mapSize; y++)
                {
                    this[x, y] = premadeMap[x, y];
                }
            }
            this.queens = queens;
        }

        public Queen this[int id]
        {
            get
            {
                return queens[id];
            }
        }

        public MapTile this[int x,int y]
        {
            get
            {
                return value[x, y];
            }
            set
            {
                this.value[x, y] = value;
            }
        }

        public void MarkSpot (int x, int y)
        {
            int topleftLength = Math.Min(x, y);
            int topRightLength = Math.Min(mapSize - x - 1, y);
            int downLeftLength = Math.Min(x, mapSize - y - 1);
            int downRightLength = Math.Min(mapSize - x, mapSize - y ) - 1;
            for(int i = 0; i < this.mapSize; i++)
            {
                //Mark Cardinal Directions
                this[x, i] = MapTile.Threatened;
                this[i, y] = MapTile.Threatened;

                //Mark Dignoals
                if (i <= downRightLength)
                {
                    this[x + i, y + i] = MapTile.Threatened;
                }
                if (i <= topRightLength)
                {
                    this[x + i, y - i] = MapTile.Threatened;
                }
                if (i <= downLeftLength)
                {
                    this[x - i, y + i] = MapTile.Threatened;
                }
                if (i <= topleftLength)
                {
                    this[x - i, y - i] = MapTile.Threatened;
                }
            }
            //Mark Self
            this[x, y] = MapTile.Occupied;
        }

        public void display(IDictionary<MapTile,ConsoleColor> colors)
        {
			System.Threading.Thread.Sleep (1000);
            Console.SetCursorPosition(0, 0);

            for(int y= 0; y < mapSize; y++)
            {
                for(int x = 0; x < mapSize; x++)
                {
                    writeBlock(colors[this[x, y]]);
                }
                Console.WriteLine();
            }
        }

        private void writeBlock (ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.BackgroundColor = color;
            Console.Write("xx");
            Console.ResetColor();
        }
    }
}
