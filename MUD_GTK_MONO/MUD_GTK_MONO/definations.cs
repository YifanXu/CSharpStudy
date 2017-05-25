using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MUD_GTK_MONO
{
    public class Creature
    {
        public string name;
        public int Health;
        public int damage;
        public int speed;
        public double resistence;
        public int standing;
        public Creature(string name, int health, int damage, double resistence, int standing)
        {
            this.name = name;
            this.Health = health;
            this.damage = damage;
            this.resistence = resistence;
            this.standing = standing;
        }
    }

    public class Paths
    {
        public string playerSave;
        public string defaultMap;
        public string mapSave;

        public Paths()
        {
            this.playerSave = string.Format("data{0}playerSaveFile.txt", Path.DirectorySeparatorChar);
            this.defaultMap = string.Format("data{0}defaultMap.txt", Path.DirectorySeparatorChar);
            this.mapSave = string.Format("data{0}mapSaveFile.txt", Path.DirectorySeparatorChar);
        }
    }

    public enum Actions
    {
        Move,
        North,
        South,
        West,
        East,
        Look,
        Get,
        Examine,
        Help,
        CheckInv,
        Save,
        Talk,
        Attack,
        RepeatAttack,
        exits
    }

    public enum Direction
    {
        North,
        South,
        East,
        West
    }

    public class Player : Creature
    {
        public IRoom current;
        public int position;
        //[XmlArray("PersonenArray")]
        public List<Item> Inventory;

        public Player() : this("Player") { }

        public Player(string name) : base (name,1000,100,0,0)
        {
            Inventory = new List<Item>();
            position = 0;
        }

        public void DropItems(IRoom current)
        {
            Random r = new Random();
            foreach (Item item in this.Inventory)
            {
                if(r.Next(2) == 1)
                {
                    current.Objects.Add(item);
                }
            }
            this.Inventory.RemoveRange(0, this.Inventory.Count);
        }

    }

    public class NPC : Creature
    {
        private string positiveDialouge;
        private string negativeDialouge;
        public string dialouge
        {
            get
            {
                if (standing > 0)
                {
                    return positiveDialouge;
                }
                return negativeDialouge;
            }
        }

        public NPC() : this("Person", "", "")
        {

        }

        public NPC(string name, string positiveDialouge, string negativeDialogue) : base(name, 1000, 10, 0.1, 2)
        {
            this.positiveDialouge = positiveDialouge;
            this.negativeDialouge = negativeDialogue;
        }


        public NPC(string name, string dialouge, int health, int damage) : base (name, health, damage, 0.1, 0)
        {
            this.positiveDialouge = dialouge;
            this.negativeDialouge = dialouge;
        }

        public NPC(string name, string positiveDialouge, string negativeDialogue, int health, int damage) : base(name, health, damage, 0.1, 2)
        {
            this.positiveDialouge = positiveDialouge;
            this.negativeDialouge = negativeDialogue;
        }
    }

    public class RunningNPC : NPC
    {
        public int stamina;
        public readonly int maxStamina;
        public IRoom currentRoom;
        
        public RunningNPC (string name, string positiveDialouge, string negativeDialogue, int stamina, IRoom room) : base(name, positiveDialouge, negativeDialogue)
        {
            this.stamina = stamina;
            this.maxStamina = stamina;
            this.currentRoom = room;
        }
    }

    public class Item
    {
        public string name;
        public string description;
        public bool obtainable;
        public string AttemptMessage;
        public string roomDescription;

        public Item()
        {

        }

        public Item(string name, string description, string AttemptMessage)
        {
            this.name = name;
            this.description = description;
            this.AttemptMessage = AttemptMessage;
        }
    }
    public class Room : IRoom
    {
        public string Name { get; }
        public int ID { get; set; }
        private string originalDescription;
        public List<Item> Objects { get; set; }
        public List<NPC> NPCs { get; set; }
        public Dictionary<Direction, IRoom> ConnectingRooms { get; private set; }
        public bool Locked { get; set; }
        public int reactivation { get; set; }
        public IRoom desto { get; set; }
        public NPC guardian { get; protected set; }

        public string description
        {
            get
            {
                string actualDescription = this.originalDescription;
                foreach (Item item in this.Objects)
                {
                    if (!String.IsNullOrEmpty(item.roomDescription))
                    {
                        actualDescription += item.roomDescription;
                    }
                }
                return actualDescription;
            }
        }

        public Room()
        {
        }

        public Room (string name, string description)
        {
            this.Name = name;
            this.originalDescription = description;
            this.Objects = new List<Item>();
            this.NPCs = new List<NPC>();
            this.ConnectingRooms = new Dictionary<Direction, IRoom>()
            {
                {Direction.North, null},
                {Direction.South, null},
                {Direction.West, null },
                {Direction.East, null }
            };
        }

        public IRoom Move(Direction direction)
        {
            return this.ConnectingRooms[direction];
        }

        public string examine(string item)
        {
            foreach (Item thing in Objects)
            {
                if(thing.name.Equals(item,StringComparison.OrdinalIgnoreCase))
                {
                    return thing.description;
                }
            }
            return "You cannot look upon such a thing.";
        }

        public Item obtain(string item)
        {
            foreach (Item thing in Objects)
            {
                if (thing.name.Equals(item, StringComparison.OrdinalIgnoreCase))
                {
                    if (thing.obtainable)
                    {
                        Program.write(ConsoleColor.Green, thing.AttemptMessage);
                        this.Objects.Remove(thing);
                        return thing;
                    }else
                    {
                        Program.write(ConsoleColor.Red, thing.AttemptMessage);
                        return null;
                    }
                }
            }
            Program.write(ConsoleColor.Red, "You cannot look upon such a thing.");
            return null;
        }

        public string[] getNPCNames()
        {
            if(NPCs.Count == 0)
            {
                return null;
            }
            string[] names = new string[this.NPCs.Count];
            for(int i = 0; i < names.Length; i++)
            {
                names[i] = NPCs[i].name;
            }
            return names;
        }

        public string getNPCDialogue(string npc)
        {
            NPC person = this.getNPC(npc);
            if (person == null)
            {
                Program.write(ConsoleColor.Red, "No such person is in the room");
                return null;
            }
            else
            {
                return person.dialouge;
            }
        }

        public NPC getNPC(string npc)
        {
            foreach (NPC person in this.NPCs)
            {
                if (string.Equals(person.name, npc, StringComparison.OrdinalIgnoreCase))
                {
                    return person;
                }
            }
            Program.write(ConsoleColor.Red,"Such entity does not exist.");
            return null;
        }

        public List<NPC> angryNPCs
        {
            get
            {
                List<NPC> angryNPCs = new List<NPC>();
                foreach (NPC npc in this.NPCs)
                {
                    if (npc.standing <= -2)
                    {
                        angryNPCs.Add(npc);
                    }
                }
                return angryNPCs;
            }
        }

        public virtual void ShowTravelInfo()
        {
        }

        public virtual void Pay(int offer, out string message)
        {
            message = "There is nobody to pay.";
        }
    }

    public class SpawnRoom : Room
    {

        public SpawnRoom (string name, string description) : base(name, description)
        {

        }
        public SpawnRoom () : base()
        {

        }

        //public override NPC getNPC(string npc)
        //{
        //    Program.write(ConsoleColor.Red, "You cannot attack anyone in your spawn room.");
        //    return null;
        //}
    }
		
}
