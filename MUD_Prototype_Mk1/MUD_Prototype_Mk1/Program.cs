using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUD_Prototype_Mk1
{
     

    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, Actions> Command = new Dictionary<string, Actions>(StringComparer.OrdinalIgnoreCase)
            {
                {"move", Actions.Move},
                {"go", Actions.Move},
                {"look", Actions.Look},
                {"examine", Actions.Examine },
                {"ex", Actions.Examine },
                {"help", Actions.Help },
                {"get", Actions.Get },
                {"obtain", Actions.Get },
                {"inv", Actions.checkInv },
                {"inventory", Actions.checkInv }
            };
            Dictionary<string, Actions> moveCommands = new Dictionary<string, Actions>(StringComparer.OrdinalIgnoreCase){
                {"n", Actions.North },
                {"north", Actions.North },
                {"s", Actions.South },
                {"south", Actions.South },
                {"w", Actions.West },
                {"west", Actions.West },
                {"e", Actions.East },
                {"east", Actions.East }
            };
            var path = string.Format("data{0}test.txt", Path.DirectorySeparatorChar);
            room entrance = ReadFile(path);
            room current = entrance;
            var player = new Player
            {
                Inventory = new List<item>()
            };
            write(ConsoleColor.Green, current.name);
            Console.WriteLine(current.description);
            write(ConsoleColor.Yellow, "Type in your action");
            while (true)
            {
                string input = Console.ReadLine();
                Actions act;
                while (string.IsNullOrEmpty(input) || !Command.TryGetValue(input.Split(' ')[0],out act))
                {
                    write(ConsoleColor.Red, "Invalid Input.");
                    input = Console.ReadLine();
                }
                string command = input.Split(' ')[0];
                string parameter = string.Empty;
                if (command.Length != input.Length)
                {
                    parameter = input.Substring(command.Length + 1);
                }
                switch (act)
                {
                    case Actions.Move:
                        Actions dir;
                        if (!moveCommands.TryGetValue(parameter, out dir))
                        {
                            write(ConsoleColor.Red, "Invalid Paremeter");
                        }
                        else
                        {
                            room targetRoom = current.move(dir);
                            if (targetRoom != null)
                            {
                                current = targetRoom;
                                write(ConsoleColor.Green, current.name);
                                Console.WriteLine(current.description);
                                write(ConsoleColor.Yellow, "Type in your action");
                            }
                        }

                        break;

                    case Actions.Examine:
                        string text = current.examine(parameter);
                        write(ConsoleColor.Cyan, text);
                        break;

                    case Actions.Look:
                        write(ConsoleColor.Green, current.name);
                        write(ConsoleColor.Cyan, current.description);
                        break;

                    case Actions.Help:
                        help();
                        break;

                    case Actions.Get:
                        item thing = current.obtain(parameter);
                        if(thing != null)
                        {
                            player.Inventory.Add(thing);
                        }
                        break;
                    case Actions.checkInv:
                        write(ConsoleColor.Cyan, "You inventory contains:");
                        foreach(item invItem in player.Inventory)
                        {
                            write(ConsoleColor.Cyan, invItem.name);
                        }
                        break;
                }
            }
        }

        public static room CreateA()
        {
            var entrance = new room
            {
                description = "You enter room A",
                name = "entrance",
                
                e = new room
                {
                    name = "room B",
                    description = "this is the end of the room",
                    
                }
            };
            room current = entrance;
            while(current.e != null)
            {
                current.e.w = current;
                current = current.e;
            }
            return entrance;
        }

        public static void write (ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void help()
        {
            write(ConsoleColor.Green, "'move + direction' to move (ex. move east)\n'examine+object' to get details on an object (ex. examine rock)\n'look'to get the room description");
        }

        public static room ReadFile(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }
            List<room> rooms = new List<room>();
            int line = 0;
            string[] input = File.ReadAllLines(path);
            while(line < input.Length)
            {
                //Add Room
                string[] parameters = input[line].Split('|');
                rooms.Add(new room {
                    name = parameters[2],
                    description = parameters[3],
                    objects = new List<item>()
                });
                line++;
                //Add Items
                while(input[line][0] == 'I')
                {
                    parameters = input[line].Split('|');
                    rooms[rooms.Count - 1].objects.Add(new item {
                        name = parameters[2],
                        description = parameters[3],
                        AttemptMessage = parameters[5]
                    });
                    if(parameters[4] == "Y")
                    {
                        rooms[rooms.Count - 1].objects[Int32.Parse(parameters[1])].obtainable = true;
                    }else
                    {
                        rooms[rooms.Count - 1].objects[Int32.Parse(parameters[1])].obtainable = false;
                    }
                    line++;
                }
                //Check if i need to start making connections
                if(input[line] == "END")
                {
                    line++;
                    break;
                }
            }
            while (line < input.Length)
            {
                string[] parameters = input[line].Split('|');
                int originRoom = int.Parse(parameters[0]);
                int destoRoom = int.Parse(parameters[2]);
                switch (parameters[1])
                {
                    case "E":
                        rooms[originRoom].e = rooms[destoRoom];
                        break;
                    case "W":
                        rooms[originRoom].w = rooms[destoRoom];
                        break;
                    case "N":
                        rooms[originRoom].n = rooms[destoRoom];
                        break;
                    case "S":
                        rooms[originRoom].s = rooms[destoRoom];
                        break;
                }
                line++;
            }
            return rooms[0];
        }
    }
}
