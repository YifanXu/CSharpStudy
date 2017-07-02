using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightQueens
{
    class Program
    {
        public static Dictionary<MapTile, ConsoleColor> colors = new Dictionary<MapTile, ConsoleColor>()
            {
                {MapTile.Empty, ConsoleColor.White },
                {MapTile.Occupied, ConsoleColor.Yellow },
                {MapTile.Threatened, ConsoleColor.DarkRed}
            };

        static void Main(string[] args)
        {
            Map map = new Map(8, 8);
            if (map[0].tryPosition(map))
            {
                map.display(colors);
            }
            else
            {
                throw new Exception("We have failed.");
            }
            Console.ReadLine();
        }
    }
}
