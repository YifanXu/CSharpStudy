using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Land
{
    class Map
    {
        private MapTile[,] value;
        public readonly int size;
        private readonly Dictionary<MapTile, ConsoleColor> colorKey = new Dictionary<MapTile, ConsoleColor>()
            {
                {MapTile.Passage, ConsoleColor.White},
                {MapTile.Wall, ConsoleColor.Black },
                {MapTile.IndestructibleWall, ConsoleColor.Black },
                {MapTile.Entrance, ConsoleColor.Cyan },
                {MapTile.Exit, ConsoleColor.Green }
            };
        private readonly Dictionary<char, MapTile> tileKey = new Dictionary<char, MapTile>()
        {
            {'X', MapTile.IndestructibleWall},
            {'o', MapTile.Wall },
            {'A', MapTile.Entrance },
            {'B', MapTile.Exit },
            {' ', MapTile.Passage }
        };

        public Map()
        {
        }

        public Map(string path, out bool success)
        {
            //Make sure the thing exists
            if (!File.Exists(path))
            {
                success = false;
                return;
            }
            String[] data = File.ReadAllLines(path);
            //Make sure the thing is a square
            this.size = data.Length;
            foreach(string line in data)
            {
                if(line.Length != size)
                {
                    success = false;
                    return;
                }
            }
            //Make an empty maze
            this.value = new MapTile[size, size];
            //Fill in the bits and pieces
            for(int line = 0; line < size; line++)
            {
                for(int i = 0; i < size; i++)
                {
                    if (!tileKey.TryGetValue(data[line][i], out value[line,i]))
                    {
                        value[i, line] = MapTile.IndestructibleWall;
                    }
                }
            }
            success = true;
        }

        public MapTile this[int y, int x]
        {
            get
            {
                return this.value[y, x];
            }
        }

        public void DrawMap(AI solver)
        {

            Console.SetCursorPosition(0, 0);
            for (int y = 0; y < this.size; y++)
            {
                for (int x = 0; x < this.size; x++)
                {
                    if(x == solver.x && y == solver.y)
                    {
                        DrawBlock(ConsoleColor.Yellow);
                    }else
                    {
                        DrawBlock(colorKey[value[y,x]]);
                    }
                }
                Console.WriteLine();
            }
        }

        private void DrawBlock(ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.BackgroundColor = color;
            Console.Write("xx");
            Console.ResetColor();
        }
    }
}
