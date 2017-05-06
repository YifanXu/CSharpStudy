using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MUD_Prototype_Mk1
{

    class Program
    {
        public static Room current;
        public static List<RunningNPC> Runningboys = new List<RunningNPC>();
        public string savePath = "data" + Path.DirectorySeparatorChar + "saveFile";
        public const double counterAttackFactor = 0.7;
        static void Main(string[] args)
        {
            Thread runNPC = new Thread(MoveNPC);
            var path = new Paths();

            //Load Files?
            Player player;
            write(ConsoleColor.Green, "new or load?");
            string input = Console.ReadLine();
            while (string.IsNullOrEmpty(input) || (input != "new" && input != "load")) {
                write(ConsoleColor.Red, "Invalid Input");
                input = Console.ReadLine();     
            }
            if(input == "new")
            {
                player = new Player();
            }else
            {
                XmlSerializer seralizer = new XmlSerializer(typeof(Player));
                using (Stream s = new FileStream(path.playerSave, FileMode.Open, FileAccess.Read))
                {
                    player = (Player) seralizer.Deserialize(s);
                }
            }
            Room entrance = ReadFile(path.defaultMap,player.position, out current);
            Act action = new Act(entrance);
            runNPC.Start();

            write(ConsoleColor.Green, current.Name);
            Console.WriteLine(current.description);
            
            write(ConsoleColor.Yellow, "Type in your action");

            
            
            while (true)
            {
                input = Console.ReadLine();
                string command = input.Split(' ')[0];
                string parameter = string.Empty;
                if (command.Length != input.Length)
                {
                    parameter = input.Substring(command.Length + 1);
                }
                action.execute(current, player, command, parameter);
                
                foreach(NPC angryNPC in current.angryNPCs)
                {
                    Damage(angryNPC, player);
                    if (angryNPC.Health <= 0)
                    {
                        current.NPCs.Remove(angryNPC);
                        continue;
                    }
                    else if (player.Health <= 0)
                    {
                        player.DropItems(current);
                        write(ConsoleColor.Cyan, "You have awaken at the spawnpoint.");
                        current = entrance;
                    }
                }
            }
        }

        public static void write (ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void MoveNPC()
        {
            Random r = new Random();
            while (true)
            {
                foreach(RunningNPC entity in Runningboys)
                {
                    lock (entity)
                    {
                        if (r.Next(100 / entity.stamina) == 0)
                        {
                            for (int i = 0; i < 50; i++)
                            {

                                Direction movingDirection = (Direction)r.Next(4);
                                if (entity.currentRoom.ConnectingRooms[movingDirection] != null)
                                {
                                    Room destoRoom = entity.currentRoom.ConnectingRooms[movingDirection];
                                    if (current == entity.currentRoom)
                                    {
                                        write(ConsoleColor.Yellow, String.Format("{0} has entered the area.", entity.name));
                                    }

                                    destoRoom.NPCs.Add(entity);
                                    entity.currentRoom.NPCs.Remove(entity);
                                    entity.currentRoom = destoRoom;


                                    if (current == entity.currentRoom)
                                    {
                                        write(ConsoleColor.Yellow, String.Format("{0} has left the area.", entity.name));
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                Thread.Sleep(2000);
            }
        }

        public static Room ReadFile(string path, int position, out Room current)
        {
            current = new Room();
            if (!File.Exists(path))
            {
                return null;
            }
            List<Room> rooms = new List<Room>();
            int line = 0;
            string[] input = File.ReadAllLines(path);
            while(line < input.Length && input[line] != "END")
            {
                //Add Room
                string[] parameters = input[line].Split('|');
                rooms.Add(new Room(parameters[2], parameters[3]));
                line++;
                //Add Items
                while(input[line][0] == 'I')
                {
                    parameters = input[line].Split('|');
                    rooms[rooms.Count - 1].Objects.Add(new Item(parameters[2], parameters[3], parameters[5]));
                    Item currentItem = rooms[rooms.Count - 1].Objects[Int32.Parse(parameters[1])];
                    if (parameters[4] == "Y")
                    {
                        currentItem.obtainable = true;
                        currentItem.roomDescription = parameters[6];
                    }
                    else
                    {
                        currentItem.obtainable = false;
                    }
                    line++;
                }
                while(input[line][0] == 'N')
                {
                    parameters = input[line].Split('|');
                    if (input[line].StartsWith("NM"))
                    {
                        RunningNPC runningboi = new RunningNPC(parameters[2], parameters[3], parameters[4], int.Parse(parameters[5]), rooms[rooms.Count - 1]);
                        rooms[rooms.Count - 1].NPCs.Add(runningboi);
                        Runningboys.Add(runningboi);
                    }
                    else
                    {
                        rooms[rooms.Count - 1].NPCs.Add(new NPC(parameters[2], parameters[3], parameters[4]));
                    }
                    line++;
                }
            }
            line++;
            while (line < input.Length)
            {
                string[] parameters = input[line].Split('|');
                int originRoom = int.Parse(parameters[0]);
                int destoRoom = int.Parse(parameters[2]);
                switch (parameters[1])
                {
                    case "E":
                        rooms[originRoom].ConnectingRooms[Direction.East] = rooms[destoRoom];
                        break;
                    case "W":
                        rooms[originRoom].ConnectingRooms[Direction.West] = rooms[destoRoom];
                        break;
                    case "N":
                        rooms[originRoom].ConnectingRooms[Direction.North] = rooms[destoRoom];
                        break;
                    case "S":
                        rooms[originRoom].ConnectingRooms[Direction.South] = rooms[destoRoom];
                        break;
                }
                line++;
            }
            current = rooms[0];
            for(int i = 0; i < rooms.Count; i++)
            {
                if(i == position)
                {
                    current = rooms[i];
                }
                rooms[i].ID = i;
            }
            return rooms[0];
        }

        public static void Damage(Player player, NPC npc)
        {
            int damage = (int)((double)player.damage * (1 - npc.resistence));
            npc.Health -= damage;
            write(ConsoleColor.Green, String.Format("You dealt {0} damage to {1}", damage, npc.name));
            if(npc.Health > 0)
            {
                damage = (int)((double)npc.damage * (1 - player.resistence) * counterAttackFactor);
                player.Health -= damage;
                write(ConsoleColor.Red, string.Format("{1} counterattacked and dealt {0} damage to you.", damage, npc.name));
            }else
            {
                write(ConsoleColor.Green, string.Format("You successfully defeated {0}", npc.name));
            }
        }
        public static void Damage(NPC npc, Player player)
        {
            int damage = (int)((double)npc.damage * (1 - player.resistence));
            player.Health -= damage;
            write(ConsoleColor.Red, string.Format("Seeing you as an enemy, {1} suddenly attacked you for {0} damage.", damage, npc.name));
            if (player.Health > 0)
            {
                damage = (int)((double)player.damage * (1 - npc.resistence));
                write(ConsoleColor.Green,string.Format("You attempted an counterattck on {1} and dealt {0} damage.", damage, npc.name));
                npc.Health -= damage;
            }
            else
            {
                write(ConsoleColor.Cyan,string.Format("You died to {0}",npc.name));
            }
        }


    }
}
