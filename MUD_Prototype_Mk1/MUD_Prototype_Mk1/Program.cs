using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MUD_Prototype_Mk1
{
     
    class Program
    {
        public string savePath = "data" + Path.DirectorySeparatorChar + "saveFile";
        public const double counterAttackFactor = 0.7;
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
                {"inv", Actions.CheckInv },
                {"inventory", Actions.CheckInv },
                {"quit", Actions.Save },
                {"exit", Actions.Save },
                {"save", Actions.Save },
                {"talkto", Actions.Talk },
                {"interact", Actions.Talk },
                {"attack", Actions.Attack },
                {"assault", Actions.Attack },
                {"RepeatAttack", Actions.RepeatAttack },
                {"Bang", Actions.RepeatAttack}
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
            Room current;
            Room entrance = ReadFile(path.defaultMap,player.position, out current);
            

            write(ConsoleColor.Green, current.name);
            Console.WriteLine(current.description);
            write(ConsoleColor.Yellow, "Type in your action");
            while (true)
            {
                input = Console.ReadLine();
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
                            Room targetRoom = current.move(dir);
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
                        write(ConsoleColor.White, current.description);
                        string[] names = current.getNPCNames();
                        if (names == null)
                        {
                            write(ConsoleColor.Cyan, "You are the only person here.");
                        }
                        else
                        {
                            write(ConsoleColor.Cyan, "People that are with you include: ");
                            foreach (string name in names)
                            {
                                write(ConsoleColor.Cyan, name);
                            }
                        }
                        break;

                    case Actions.Talk:
                        string dialouge = current.getNPCDialogue(parameter);
                        if (!string.IsNullOrEmpty(dialouge))
                        {
                            write(ConsoleColor.Cyan, String.Format("{0} says '{1}'",parameter,dialouge));
                        }
                        break;

                    case Actions.Help:
                        help();
                        break;

                    case Actions.Get:
                        Item thing = current.obtain(parameter);
                        if(thing != null)
                        {
                            player.Inventory.Add(thing);
                        }
                        break;
                    case Actions.CheckInv:
                        write(ConsoleColor.Green, string.Format("You currently have {0} hp left", player.Health));
                        write(ConsoleColor.Cyan, "You inventory contains:");
                        foreach(Item invItem in player.Inventory)
                        {
                            write(ConsoleColor.Cyan, invItem.name);
                        }
                        break;
                    case Actions.Save:
                        player.position = current.ID;
                        var seralizer = new XmlSerializer(typeof (Player));
                        using(Stream s = new FileStream(path.playerSave,FileMode.Create,FileAccess.Write))
                        {
                            seralizer.Serialize(s, player);
                        }
                        write(ConsoleColor.Green, "Saved.");
                        return;
                    case Actions.Attack:
                        NPC target = current.getNPC(parameter);
                        if(target != null)
                        {
                            target.standing--;
                            Damage(player, target);
                            if(target.Health <= 0)
                            {
                                current.NPCs.Remove(target);
                            }
                            else if(player.Health <= 0)
                            {
                                player.DropItems(current);
                                write(ConsoleColor.Cyan, "You have awaken at the spawnpoint.");
                                current = entrance;
                            }
                        }
                        break;
                    case Actions.RepeatAttack:
                        NPC deletedTarget = current.getNPC(parameter);
                        if (deletedTarget != null)
                        {
                            while (deletedTarget.Health > 0 && player.Health > 0)
                            {
                                deletedTarget.standing--;
                                Damage(player, deletedTarget);
                                foreach (NPC angryNPC in current.angryNPCs)
                                {
                                    if (angryNPC.Health >= 0)
                                    {
                                        Damage(angryNPC, player);
                                    }
                                }
                                System.Threading.Thread.Sleep(100);
                            }
                            if (deletedTarget.Health <= 0)
                            {
                                current.NPCs.Remove(deletedTarget);
                            }
                            else if (player.Health <= 0)
                            {
                                player.DropItems(current);
                                write(ConsoleColor.Cyan, "You have awaken at the spawnpoint.");
                                current = entrance;
                            }
                        }
                        break;
                        break;
                }
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

        public static void help()
        {
            write(ConsoleColor.Green, "'move + direction' to move (ex. move east)\n'examine+object' to get details on an object (ex. examine rock)\n'look'to get the room description");
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
                    rooms[rooms.Count - 1].objects.Add(new Item(parameters[2], parameters[3], parameters[5]));
                    Item currentItem = rooms[rooms.Count - 1].objects[Int32.Parse(parameters[1])];
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
                    rooms[rooms.Count - 1].NPCs.Add(new NPC(parameters[2], parameters[3]));
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
                write(ConsoleColor.Green,string.Format("You attempted an counterattck on {1} and dealt {0} damage.", damage, npc.name));
                player.Health -= (int)((double)npc.damage * (1 - player.resistence) * counterAttackFactor);
            }
            else
            {
                write(ConsoleColor.Cyan,string.Format("You died to {0}",npc.name));
            }
        }
    }
}
