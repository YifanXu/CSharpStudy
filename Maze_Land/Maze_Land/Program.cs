using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Land
{
    class Program
    {
        public static Map map;
        public const string path = "Map.txt";
        static void Main(string[] args)
        {
            bool success;
            map = new Map(path, out success);
            if (!success)
            {
                SpaceCheck();
                return;
            }
            AI solver = new AI();
            if(!FindFirstTile(MapTile.Entrance, out solver.x, out solver.y))
            {
                Write(ConsoleColor.Red, "No Entrance Found.");
                SpaceCheck();
                return;
            }

            solver.route.Push(new MapNode(solver.x, solver.y));
            while (true)
            {
                map.DrawMap(solver);
                if (!solver.Operate(map))
                {
                    Write(ConsoleColor.Red, "No Exits detected.");
                    SpaceCheck();
                    break;
                }
                else if(map[solver.y,solver.x] == MapTile.Exit)
                {
                    Console.WriteLine();
                    Write(ConsoleColor.Green, "AI Found the Exit!");
                    SpaceCheck();
                    break;
                }
                System.Threading.Thread.Sleep(200);
            }
        }

        public static bool FindFirstTile (MapTile type, out int x, out int y)
        {
            x = 0;
            y = 0;
            for (y = 0; y < map.size; y++)
            {
                for (x = 0; x < map.size; x++)
                {
                    if(map[y,x] == type)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void Write(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void SpaceCheck()
        {
            ConsoleKeyInfo key = Console.ReadKey();
            while (key.Key != ConsoleKey.Spacebar)
            {
                key = Console.ReadKey();
            }
        }
    }
}
