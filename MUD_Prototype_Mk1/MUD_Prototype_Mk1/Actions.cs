using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace MUD_Prototype_Mk1
{
    public class Act
    {

        private const double counterAttackFactor = 0.7;
        public readonly Room spawn;
        private readonly string playerSave = string.Format("data{0}playerSaveFile.txt", Path.DirectorySeparatorChar);
        private Dictionary<string, Action<Room, Player, string>> commands;

        private Dictionary<string, Direction> moveCommands = new Dictionary<string, Direction>(StringComparer.OrdinalIgnoreCase){
                {"n", Direction.North },
                {"north", Direction.North },
                {"s", Direction.South },
                {"south", Direction.South },
                {"w", Direction.West },
                {"west", Direction.West },
                {"e", Direction.East },
                {"east", Direction.East }
            };

        public Act(Room entrance)
        {
            this.spawn = entrance;
            this.commands = new Dictionary<string, Action<Room, Player, string>>(StringComparer.OrdinalIgnoreCase)
            {
                {"move", Move},
                {"go", Move},
                {"look", Look},
                {"examine", Examine },
                {"ex", Examine },
                {"help", Help },
                {"get", Get },
                {"obtain", Get },
                {"inv", CheckInv },
                {"inventory", CheckInv },
                {"quit", Save },
                {"exit", Save },
                {"save", Save },
                {"talkto", Talk },
                {"interact", Talk },
                {"attack", Attack },
                {"assault", Attack },
                {"RepeatAttack", RepeatAttack },
                {"Bang", RepeatAttack},
                {"Exits", ShowExits}
            };
        }

        public void execute(Room current, Player player, string command, string parameter)
        {
            Action<Room,Player,string> action;
            if(!commands.TryGetValue(command, out action))
            {
                Program.write(ConsoleColor.Red, "Invalid Input");
            }
            else
            {
                action(current, player, parameter);
            }
        }

        public void Move(Room current, Player player, string parameter)
        {
            Direction dir;
            if (!moveCommands.TryGetValue(parameter, out dir))
            {
                Program.write(ConsoleColor.Red, "Invalid Paremeter");
            }
            else
            {
                Room targetRoom = current.Move(dir);
                if (targetRoom != null)
                {
                    current = targetRoom;
                    Program.write(ConsoleColor.Green, current.Name);
                    Console.WriteLine(current.description);
                    Program.write(ConsoleColor.Yellow, "Type in your action");
                }
                else
                {
                    Program.write(ConsoleColor.Red, "You cannot move in that direction");
                }
            }
        }

        public void Examine(Room current, Player player, string parameter)
        {
            Program.write(ConsoleColor.Cyan, current.examine(parameter));
        }

        public void Look(Room current, Player player, string parameter)
        {
            Program.write(ConsoleColor.Green, current.Name);
            Program.write(ConsoleColor.White, current.description);
            string[] names = current.getNPCNames();
            if (names == null)
            {
                Program.write(ConsoleColor.Cyan, "You are the only person here.");
            }
            else
            {
                Program.write(ConsoleColor.Cyan, "People that are with you include: ");
                foreach (string name in names)
                {
                    Program.write(ConsoleColor.Cyan, name);
                }
            }
        }

        public void Talk(Room current, Player player, string parameter)
        {
            string dialouge = current.getNPCDialogue(parameter);
            if (!string.IsNullOrEmpty(dialouge))
            {
                Program.write(ConsoleColor.Cyan, String.Format("The person says '{1}'", parameter, dialouge));
            }
        }

        public void Help(Room current, Player player, string parameter)
        {
            Program.write(ConsoleColor.Cyan, "Avaliable commands include:");
            foreach(string command in commands.Keys)
            {
                Program.write(ConsoleColor.Cyan, command);
            }
        }

        public void Get(Room current, Player player, string parameter)
        {
            Item thing = current.obtain(parameter);
            if (thing != null)
            {
                player.Inventory.Add(thing);
            }
        }

        public void CheckInv(Room current, Player player, string paremeter)
        {
            Program.write(ConsoleColor.Green, string.Format("You currently have {0} hp left", player.Health));
            Program.write(ConsoleColor.Cyan, "You inventory contains:");
            foreach (Item invItem in player.Inventory)
            {
                Program.write(ConsoleColor.Cyan, invItem.name);
            }
            
        }

        public void Save(Room current, Player player, string paremeter)
        {
            player.position = current.ID;
            var seralizer = new XmlSerializer(typeof(Player));
            using (Stream s = new FileStream(playerSave, FileMode.Create, FileAccess.Write))
            {
                seralizer.Serialize(s, player);
            }
            Program.write(ConsoleColor.Green, "Saved.");
            return;
        }

        public void Attack(Room current, Player player, string parameter)
        {
            NPC target = current.getNPC(parameter);
            if (target != null)
            {
                target.standing--;
                Damage(player, target);
                if (target.Health <= 0)
                {
                    current.NPCs.Remove(target);
                }
                else if (player.Health <= 0)
                {
                    player.DropItems(current);
                    Program.write(ConsoleColor.Cyan, "You have awaken at the spawnpoint.");
                    current = spawn;
                }
            }

        }

        public void RepeatAttack(Room current, Player player, string parameter)
        {
            Program.write(ConsoleColor.Green, string.Format("You currently have {0} hp left", player.Health));
            Program.write(ConsoleColor.Cyan, "You inventory contains:");
            foreach (Item invItem in player.Inventory)
            {
                Program.write(ConsoleColor.Cyan, invItem.name);
            }

        }

        public void ShowExits(Room current, Player player, string parameter)
        {
            Program.write(ConsoleColor.Cyan, "Exits avaliable are:");
            foreach (KeyValuePair<Direction, Room> pair in current.ConnectingRooms)
            {
                if (pair.Value != null)
                {
                    Program.write(ConsoleColor.Cyan, String.Format(" - {0} : {1}", pair.Key.ToString(), pair.Value.Name));
                }
            }
            
            if(current is Portal)
            {
                current.ShowTravelInfo();
            }
        }

        public static void Damage(NPC npc, Player player)
        {
            int damage = (int)((double)npc.damage * (1 - player.resistence));
            player.Health -= damage;
            Program.write(ConsoleColor.Red, string.Format("Seeing you as an enemy, {1} suddenly attacked you for {0} damage.", damage, npc.name));
            if (player.Health > 0)
            {
                damage = (int)((double)player.damage * (1 - npc.resistence) * counterAttackFactor);
                Program.write(ConsoleColor.Green, string.Format("You attempted an counterattck on {1} and dealt {0} damage.", damage, npc.name));
                npc.Health -= damage;
            }
            else
            {
                Program.write(ConsoleColor.Cyan, string.Format("You died to {0}", npc.name));
            }
        }

        public static void Damage(Player player, NPC npc)
        {
            int damage = (int)((double)player.damage * (1 - npc.resistence));
            npc.Health -= damage;
            Program.write(ConsoleColor.Green, String.Format("You dealt {0} damage to {1}", damage, npc.name));
            if (npc.Health > 0)
            {
                damage = (int)((double)npc.damage * (1 - player.resistence) * counterAttackFactor);
                player.Health -= damage;
                Program.write(ConsoleColor.Red, string.Format("{1} counterattacked and dealt {0} damage to you.", damage, npc.name));
            }
            else
            {
                Program.write(ConsoleColor.Green, string.Format("You successfully defeated {0}", npc.name));
            }
        }
    }
}
