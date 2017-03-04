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
            Dictionary<string, Actions> Command = new Dictionary<string, Actions>(StringComparer.CurrentCultureIgnoreCase)
            {
                {"move", Actions.Move},
                {"look", Actions.Look}
            };
            Dictionary<string, Actions> moveCommands = new Dictionary<string, Actions>(StringComparer.CurrentCultureIgnoreCase){
                {"n", Actions.North },
                {"s", Actions.South },
                {"w", Actions.West },
                {"e", Actions.East }
            };
            var path = string.Format("data{0}test.txt", Path.DirectorySeparatorChar);
            room entrance = ReadFile(path);
            room current = entrance;
            while (true)
            {
                write(ConsoleColor.Green, current.name);
                Console.WriteLine(current.description);
                Console.WriteLine("Type in your action");
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
                            }
                        }

                        break;


                    case Actions.Look:
                        Console.WriteLine(current.description);
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
