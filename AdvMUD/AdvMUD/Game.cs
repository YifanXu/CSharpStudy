using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvMUD.Entities;
using AdvMUD.DataProviders;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using AdvMUD.Questing;

namespace AdvMUD
{
    public class Game
    {
        public static Game game { get; private set; }
        public static Random r = new System.Random();
        private const int CombatHpBarLength = 20;
        IDataProvider provider { get; set; }
        private static Dictionary<string, string> Directions = new Dictionary<string, string>()
        {
            {"north", "n"},
            {"south", "s"},
            {"west", "w" },
            {"east", "e" },
            {"n", "n" },
            {"w", "w" },
            {"e", "e" },
            {"s", "s" },
            {"portal", "p" },
            {"p", "p" }
        };
        private static Dictionary<string, Func<string,string>> actionDictionary;
        private Room spawn;
        private string lastCommand = String.Empty;
        public Dictionary<int, Room> allRooms;
        
        public Game(IDataProvider provider, string roomPath)
        {
            game = this;
            this.provider = provider;
            actionDictionary = new Dictionary<string, Func<string,string>>()
            {
                {"look", Look },
                {"survey", Look },
                {"move", Move},
                {"travel", Move },
                {"examine", Examine },
                {"get", GetItem },
                {"pickup", GetItem },
                {"obtain", GetItem },
                {"take", GetItem },
                {"drop", DropItem },
                {"help", GetHelp },
                {"exits", ShowExits },
                {"talk", InteractNPC },
                {"talkto", InteractNPC },
                {"interact", InteractNPC },
                {"checkinv", CheckInv },
                {"inv", CheckInv },
                {"inventory", CheckInv },
                {"quests", CheckQuests },
                {"quest", CheckQuests },
                {"fight", Fight },
                {"attack", Fight },
                {"save", SaveGame }
            };
            spawn = provider.GetRootRoom(roomPath,out allRooms);
            Player.player = new Player(spawn,provider.GetQuests(provider.questDirectory));
        }

        public void Run()
        {
            Display(ConsoleColor.White, spawn.desc);
            while (true)
            {
                Func<string, string> action;
                string par;
                lastCommand = Console.ReadLine();
                if (!TryDecipherCommand(lastCommand.Trim(), out action, out par))
                {
                    Console.Clear();
                    Display(ConsoleColor.Red, "Command was not recognized. Type \"help\" to see all commands");
                    continue;
                }
                string result = action(par.Trim());
                Console.Clear();
                Display(ConsoleColor.White, result);
                if (action == null || action == Move || action == GetItem || action == InteractNPC)
                {
                    List<Quest> updatedQuests = Player.player.checkQuest();
                    while (updatedQuests.Count != 0)
                    {
                        foreach (Quest quest in updatedQuests)
                        {
                            if (quest.stage == 1)
                            {
                                Console.WriteLine("QUEST STARTED: {0}", quest.name);
                            }
                            else if (quest.status == QuestStage.Completed)
                            {
                                Console.WriteLine("QUEST COMPLETED: {0}", quest.name);
                            }
                            else
                            {
                                Console.WriteLine("QUEST UPDATED: {0}", quest.name);
                            }
                            Console.WriteLine("- {0}", quest.message);
                            Console.WriteLine();
                        }
                        updatedQuests = Player.player.checkQuest();
                    }
                }
            }
        }

        public void Display(ConsoleColor resultColor, string commandResults)
        {
            Console.WriteLine(Player.player.Location.name);
            WriteLine(ConsoleColor.Cyan, lastCommand);
            WriteLine(resultColor, commandResults);
            Console.WriteLine();
        }

        public static void Write(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public static void WriteLine(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        private static int GetChoice (int numOfChoices, bool allowNull)
        {
            string input = Console.ReadLine();
            if(allowNull && input == String.Empty)
            {
                return -1;
            }
            int choice;
            while (!int.TryParse(input, out choice) || choice < 0 || choice >= numOfChoices)
            {
                Console.WriteLine("Invlaid Input. Enter a number 0-2");
                input = Console.ReadLine();
                if (allowNull && input == "")
                {
                    return -1;
                }
            }
            return choice;
        }

        public static void WriteBlock(ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.BackgroundColor = color;
            Console.Write("xx");
            Console.ResetColor();
        }

        private bool TryDecipherCommand (string input, out Func<string,string> command, out string parameter)
        {
            command = null;
            parameter = "";
            string[] words = input.Split(' ');
            if(words.Length == 0)
            {
                return false;
            }
            
            string prefix = words[0];       
            prefix = prefix.ToLower();
            if(words.Length != 1)
            {
                parameter = input.Substring(prefix.Length+1);
            }
            if(actionDictionary.TryGetValue(prefix, out command))
            {
                return true;
            }
            return false;
        }

        public string Look (string parameter)
        {
            Room playerLocation = Player.player.Location;
            StringBuilder str = new StringBuilder();
            str.AppendLine(playerLocation.desc);
            if (playerLocation.items != null && playerLocation.items.Count != 0)
            {
                str.AppendLine("Objects you can examine include:");
                foreach(Item item in playerLocation.items)
                {
                    str.AppendLine(String.Format(" - {0} x {1}", item.name, item.quantity));
                }
            }
            else
            {
                str.AppendLine("There are no objects in this area that you can examine.");
            }
            if (playerLocation.npcs != null && playerLocation.npcs.Count > 0)
            {
                str.AppendLine("You can see the following npcs:");
                foreach (NPC npc in playerLocation.npcs)
                {
                    str.AppendLine(" - " + npc.name);
                }
            }
            else
            {
                str.AppendLine("There are nobody here but you.");
            }

            return str.ToString();
        }

        public string Move (string parameter)
        {
            string dir;
            if(!Directions.TryGetValue(parameter, out dir))
            {
                return "There is no such direction";
            }
            Room current = Player.player.Location;
            Room destination = current.findConnection(dir);
            if (destination != null)
            {
                Player.player.Location = destination;
                return String.Format("You moved to {0}.\n{1}", destination.name, destination.desc);
            }
            return "You cannot move in that direction";
        }
        
        public string Examine (string parameter)
        {
            Room current = Player.player.Location;
            Item item = current.FindItemByName(parameter);
            if(item == null)
            {
                item = Player.player.FindItemByName(parameter);
            }
            if(item == null)
            {
                return "This item is not found!";
            }
            return item.desc;
        }

        public string GetItem (string parameter)
        {
            Room current = Player.player.Location;
            Item item = current.FindItemByName(parameter);
            if (item == null)
            {
                return "Such Item does not exist";
            }
            if (!item.obtainable)
            {
                return String.Format("You cannot obtain {0}",item.name);
            }
            if(item.quantity > 1)
            {
                WriteLine(ConsoleColor.Cyan, String.Format("There are {0} of such items avaliable. How many do you pick up?",item.quantity));
                string input = Console.ReadLine();
                int pickupQuantity;
                if (!int.TryParse(input, out pickupQuantity))
                {
                    return "Invalid input";
                }
                if(pickupQuantity < 1 || pickupQuantity > item.quantity)
                {
                    return "The quantity of items inputted was invalid.";
                }
                if(pickupQuantity == item.quantity)
                {
                    current.items.Remove(item);

                    Item playerItem = Player.player.FindItemByName(item.name);
                    if(playerItem != null)
                    {
                        playerItem.quantity += pickupQuantity;
                    }else
                    {
                        Player.player.inventory.Add(item);
                    }
                }else
                {
                    Item playerItem = Player.player.FindItemByName(item.name);
                    if (playerItem != null)
                    {
                        playerItem.quantity += pickupQuantity;
                    }
                    else
                    {

                        Player.player.inventory.Add(new Item(item.name, item.desc, pickupQuantity, item.dropChance));
                    }
                    item.quantity -= pickupQuantity;
                }
            }
            else
            {
                current.items.Remove(item);
                Player.player.inventory.Add(item);
            }
            return String.Format("You have successfully collected {0}",item.name);
        }

        public string DropItem (string itemName)
        {
            Item item = Player.player.FindItemByName(itemName);
            Room room = Player.player.Location;
            if(item == null)
            {
                return "There is no such item in your inventory";
            }
            if (item.quantity > 1)
            {
                WriteLine(ConsoleColor.Cyan, String.Format("There are {0} of such items avaliable. How many do you drop?", item.quantity));
                string input = Console.ReadLine();
                int dropQuantity;
                if (!int.TryParse(input, out dropQuantity))
                {
                    return "Invalid input";
                }
                if (dropQuantity < 1 || dropQuantity > item.quantity)
                {
                    return "The quantity of items inputted was invalid.";
                }
                if (dropQuantity == item.quantity)
                {
                    Player.player.inventory.Remove(item);

                    Item roomItem = room.FindItemByName(item.name);
                    if (roomItem != null)
                    {
                        roomItem.quantity += dropQuantity;
                    }
                    else
                    {
                        room.items.Add(item);
                    }
                }
                else
                {
                    Item roomItem = room.FindItemByName(item.name);
                    if (roomItem != null)
                    {
                        roomItem.quantity += dropQuantity;
                    }
                    else
                    {

                        room.items.Add(new Item(item.name, item.desc, dropQuantity, item.dropChance));
                    }
                    item.quantity -= dropQuantity;
                }
            }
            else
            {
                room.items.Add(item);
                Player.player.inventory.Remove(item);
            }
            return String.Format("You dropped {0}", item.name);
        }

        public string GetHelp(string parameter)
        {
            StringBuilder s = new StringBuilder();
            foreach(string command in actionDictionary.Keys)
            {
                s.AppendLine(command);
            }
            return s.ToString();
        }

        public string ShowExits (string parameter)
        {
            if(Player.player.Location.connections == null || Player.player.Location.connections.Length == 0)
            {
                return "There is no exits to this place. You. Are. STUCK.\n(Unless you figure something out)";
            }
            StringBuilder s = new StringBuilder();
            foreach (RoomConnection connection in Player.player.Location.connections)
            {
                if (connection.IsValid)
                {
                    s.AppendLine(String.Format("{0}: {1}", connection.direction, connection.targetRoom.name));
                }
            }
            return s.ToString();
        }

        

        public string InteractNPC(string NPCName)
        {
            NPC n = Player.player.Location.FindNPCByName(NPCName);
            if(n == null)
            {
                return "There is no such NPC in the room with you.";
            }
            n.dialouges.Run(n);
            return String.Format("You finished talking to {0}",n.name);
        }

        public string CheckInv(string parameter)
        {
            List<Item> inventory = Player.player.inventory;
            if(inventory == null || inventory.Count == 0)
            {
                return "There is nothing in your inventory.";
            }
            StringBuilder str = new StringBuilder();
            str.AppendLine(String.Format("There are {0} items in your inventory:",inventory.Count));
            foreach(Item item in inventory)
            {
                str.AppendLine(String.Format(" - {0} x {1}", item.name,item.quantity));
            }

            return str.ToString();
        }

        public string CheckQuests (string parameter)
        {
            List<Quest> quests = Player.player.GetQuestsByCondition(QuestStage.InProgress);
            List<Quest> completed = Player.player.GetQuestsByCondition(QuestStage.Completed);
            if(quests == null || (quests.Count == 0 && completed.Count == 0))
            {
                return "You have no quests active or completed";
            }
            Console.WriteLine("You have {0} active quests and {0} completed quests.", quests.Count, completed.Count);
            int i;
            for(i = 0; i < quests.Count; i++)
            {
                WriteLine(ConsoleColor.Cyan, String.Format("{0}. {1}", i, quests[i].name));
            }
            for(i = quests.Count; i < quests.Count + completed.Count; i++)
            {
                WriteLine(ConsoleColor.DarkCyan, String.Format("{0}. {1}", i, completed[i - quests.Count].name));
            }
            Console.ResetColor();
            while (true)
            {
                Console.WriteLine("Select a quest to inspect it further, or input nothing to return");
                int choice = GetChoice(quests.Count + completed.Count, true);
                if(choice == -1)
                {
                    break;
                }
                Quest chosen;
                if(choice >= quests.Count)
                {
                    chosen = completed[choice - quests.Count];
                }
                else
                {
                    chosen = quests[choice];
                }
                WriteLine(ConsoleColor.Cyan, String.Format("Quest: {0}\nStatus:{1} (Stage {2})\nCondition:{3}\n\n{4}", chosen.name, chosen.status, chosen.stage, chosen.conditon, chosen.message));
            }

            return "You finished inspecting quests";
        }

        public string SaveGame (string parameter)
        {
            Room[] rooms = allRooms.Values.ToArray();
            foreach (Room room in rooms)
            {
                if (room.connections != null)
                {
                    foreach (RoomConnection connection in room.connections)
                    {
                        connection.targetRoom = null;
                    }
                }
                if(room.npcs != null)
                {
                    foreach(NPC npc in room.npcs){
                        npc.Location = null;
                    }
                }
            }
            if (String.IsNullOrEmpty(parameter) || parameter.ToLower() == provider.type)
            {
                provider.SaveRooms(provider.saveRoomDirectory, rooms);
            }
            else
            {
                IDataProvider newProvider;
                switch (parameter)
                {
                    case "xml":
                        newProvider = new XMLProvider();
                        break;
                    case "json":
                        newProvider = new JsonProvider();
                        break;
                    default:
                        return "Failure to validate provider type";

                }
                newProvider.SaveRooms(newProvider.saveRoomDirectory, rooms);
            }
            if (String.IsNullOrEmpty(parameter))
            {
                parameter = provider.type;
            }
            return String.Format("Successfully saved to provider type {0}", parameter);
        }

        public string Fight (string parameter)
        {
            Player player = Player.player;
            NPC target = player.Location.FindNPCByName(parameter);
            if(target == null)
            {
                return "There is no such npc in the room";
            }
            Entity winner = Combat(player, target);
            if(winner == null)
            {
                return "You escaped the fight";
            }
            else if(winner is Player)
            {
                target.Die();
                return "You won the fight!";
            }
            else
            {
                player.Die();
                return "You lost the fight and you died.";
            }
        }

        public Entity Combat (Entity a, Entity b)
        {
            while (true)
            {
                Console.Clear();
                CombatDisplay(a,b);
                Console.WriteLine();
                if(a is Player)
                {
                    if (PlayerMove(b))
                    {
                        return null;
                    }
                    if (b.health <= 0)
                    {
                        return a;
                    }
                    int damage = b.Attack - a.Defense;
                    WriteLine(ConsoleColor.Red, String.Format("You received {0} damage",damage));
                    Player.player.health -= damage;
                    if (a.health <= 0)
                    {
                        return b;
                    }
                }
                else
                {
                    int damage = a.Attack - b.Defense;
                    WriteLine(ConsoleColor.Red, String.Format("You received {0} damage", damage));
                    if (b.health <= 0)
                    {
                        return a;
                    }
                    if (PlayerMove(a))
                    {
                        return null;
                    }
                    if (a.health <= 0)
                    {
                        return b;
                    }
                    Player.player.health -= damage;
                }
                Console.ReadLine();
            }
            
        }


        private static void CombatDisplay(Entity a, Entity b)
        {
            WriteLine(ConsoleColor.Gray, String.Format("IN COMBAT: {0} vs. {1}", a.name, b.name));
            ShowHealth(a);
            ShowHealth(b);
        }

        private static void ShowHealth(Entity a)
        {
            int HpPercent = a.health * CombatHpBarLength / a.maxHealth;
            Console.Write("{0}: \n", a.name);
            for(int i = 0; i < HpPercent; i++)
            {
                WriteBlock(ConsoleColor.Green);
            }
            int lostHpPercent = CombatHpBarLength - HpPercent;
            for (int i = 0; i < lostHpPercent; i++)
            {
                WriteBlock(ConsoleColor.Red);
            }
            Console.WriteLine();
        }

        private bool PlayerMove(Entity opponent)
        {
            Console.WriteLine("Make your move...");
            Console.WriteLine("0. Attack");
            Console.WriteLine("1. Attempt to Escape");
            Console.WriteLine("2. Use Item");
            Console.WriteLine("3. Attempt to Inspect Opponent");
            Console.WriteLine("Enter a number.");

            int choice = GetChoice(4,false);
            switch (choice)
            {
                case 0:
                    List<int> playerSpells = Player.player.avaliableSpells;
                    WriteLine(ConsoleColor.Cyan, String.Format("Make an attack....\nYou have {0}/{1} mana left",Player.player.mana,Player.player.maxMana));
                    for(int i = 0; i < playerSpells.Count; i++)
                    {
                        int spellID = playerSpells[i];
                        ConsoleColor color;
                        if (Player.player.mana >= Spell.manaCosts[spellID])
                        {
                            color = ConsoleColor.Green;
                        }
                        else
                        {
                            color = ConsoleColor.Red;
                        }
                        WriteLine(color, string.Format(" - {0}. {1} (Cost {2} mana)",i,Spell.spellNames[spellID],Spell.manaCosts[spellID]));
                    }
                    Console.WriteLine("Choose an attack by typing in a number.");
                    int spellChoice = playerSpells[GetChoice(playerSpells.Count,false)];
                    while(Player.player.mana < Spell.manaCosts[spellChoice])
                    {
                        Console.WriteLine("You cannot cast that spell because you have insufficent mana.");
                        spellChoice = playerSpells[GetChoice(playerSpells.Count,false)];
                    }
                    Spell.Cast(spellChoice, Player.player, opponent);
                    Player.player.mana -= Spell.manaCosts[spellChoice];
                    return false;
                case 1:
                    int roll = r.Next(Player.player.Speed + opponent.Speed);
                    if (roll < Player.player.Speed)
                    {
                        WriteLine(ConsoleColor.Green, "Success");
                        return true;
                    }
                    else
                    {
                        WriteLine(ConsoleColor.Red, "You failed to escape");
                        return false;
                    }
                case 2:
                    Write(ConsoleColor.Red, "You do not have any activatable items");
                    return false;
                case 3:
                    if (r.Next(Player.player.Speed + opponent.Speed) < Player.player.Speed)
                    {
                        WriteLine(ConsoleColor.Green, "Success");
                        Console.WriteLine("Opponent {0} : Atk: {1}, Spd: {2}, Def: {3}, Res: {4}", opponent.name, opponent.Attack, opponent.Speed, opponent.Defense, opponent.Resistance);
                    }else
                    {
                        WriteLine(ConsoleColor.Red, "Failure");
                    }
                    return false;
            }
            return false;
        }
    }
}
