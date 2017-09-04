using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvMUD.Entities;

namespace AdvMUD
{
    public class Room
    {
        public static Room deathRoom = new Room();
        public int id { get; set; }
        public string name { get; set; }
        public string desc { get; set; }
        public RoomConnection[] connections { get; set; }
        public List<Item> items { get; set; }
        public List<NPC> npcs { get; set; }

        public Room()
        {

        }
        
        public Room (int id, string name, string desc)
        {
            this.id = id;
            this.name = name;
            this.desc = desc;
        }

        public Item FindItemByName (string name)
        {
            foreach(Item item in items)
            {
                if (String.Equals(name, item.name, StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }
            return null;
        }

        public NPC FindNPCByName(string name)
        {
            foreach (NPC npc in npcs)
            {
                if (String.Equals(name, npc.name, StringComparison.OrdinalIgnoreCase))
                {
                    return npc;
                }
            }
            return null;
        }

        public Room findConnection (string direction)
        {
            if (connections != null)
            {
                foreach (RoomConnection connection in connections)
                {
                    if (connection.direction == direction && connection.IsValid)
                    {
                        return connection.targetRoom;
                    }
                }
            }
            return null;
        }
    }
}
