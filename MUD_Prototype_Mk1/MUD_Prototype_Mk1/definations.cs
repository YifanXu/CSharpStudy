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
    }
    public class Player : Creature
    {
        public int position;
        //[XmlArray("PersonenArray")]
        public List<Item> Inventory;

        public Player() : this("Player") { }

        public Player(string name) : base (name,1000,100,0,0)
        {
            Inventory = new List<Item>();
            position = 0;
        }

        public void DropItems(Room current)
        {
            Random r = new Random();
            foreach (Item item in this.Inventory)
            {
                if(r.Next(2) == 1)
                {
                    current.objects.Add(item);
                }
            }
            this.Inventory.RemoveRange(0, this.Inventory.Count);
        }

    }

    public class NPC : Creature
    {
        public string dialouge;

        public NPC() : this("Person","")
        {

        }

        public NPC(string name, string dialouge) : base(name,1000,10,0.1,2)
        {
            this.dialouge = dialouge;
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
    public class Room
    {
        public string name;
        public int ID;
        private string originalDescription;
        public Room n;
        public Room s;
        public Room w;
        public Room e;
        public List<Item> objects;
        public List<NPC> NPCs;

        public string description
        {
            get
            {
                string actualDescription = this.originalDescription;
                foreach (Item item in this.objects)
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
            this.name = name;
            this.originalDescription = description;
            this.objects = new List<Item>();
            this.NPCs = new List<NPC>();    
        }

        public Room move(Actions direction)
        {
            switch (direction)
            {
                case Actions.North:
                    return this.n;
                case Actions.South:
                    return this.s;
                case Actions.West:
                    return this.w;
                case Actions.East:
                    return this.e;
                default:
                    return null;
            }
        }

        public string examine(string item)
        {
            foreach (Item thing in objects)
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
            foreach (Item thing in objects)
            {
                if (thing.name.Equals(item, StringComparison.OrdinalIgnoreCase))
                {
                    if (thing.obtainable)
                    {
                        Program.write(ConsoleColor.Green, thing.AttemptMessage);
                        this.objects.Remove(thing);
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
            string dialouge = this.getNPC(npc).dialouge;
            Program.write(ConsoleColor.Red, "No such person is in the room");
            return null;
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
    }

    public class SpawnRoom : Room
    {

        public SpawnRoom (string name, string description) : base(name, description)
        {

        }
        public SpawnRoom () : base()
        {

        }

        public virtual NPC getNPC(string npc)
        {
            Program.write(ConsoleColor.Red, "You cannot attack anyone in your spawn room.");
            return null;
        }
    }
}
