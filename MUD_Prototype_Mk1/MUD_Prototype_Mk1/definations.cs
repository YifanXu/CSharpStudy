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
        public Creature(string name, int health, int damage)
        {
            this.name = name;
            this.Health = health;
            this.damage = damage;
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
    }
    public class Player : Creature
    {
        //[XmlArray("PersonenArray")]
        public List<Item> Inventory;

        public Player() : this("Player") { }

        public Player(string name) : base (name,1000,100)
        {
            Inventory = new List<Item>();
        }
    }

    public class NPC : Creature
    {
        public string dialouge;

        public NPC() : this("Person","")
        {

        }

        public NPC(string name, string dialouge) : base(name,100,10)
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
        public string description;
        public string originaldescription;
        public Room n;
        public Room s;
        public Room w;
        public Room e;
        public List<Item> objects;
        public List<NPC> NPCs;

        public Room()
        {
        }

        public Room (string name, string description)
        {
            this.name = name;
            this.description = description;
            this.originaldescription = description;
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
            foreach(NPC person in this.NPCs)
            {
                if (string.Equals(person.name, npc, StringComparison.OrdinalIgnoreCase))
                {
                    return person.dialouge;
                }
            }
            Program.write(ConsoleColor.Red, "No such person is in the room");
            return null;
        }

        public void updateDescription()
        {
            this.description = this.originaldescription;
            foreach(Item item in this.objects)
            {
                if (!String.IsNullOrEmpty(item.roomDescription))
                {
                    this.description += item.roomDescription;
                }
            }
        }
    }
}
