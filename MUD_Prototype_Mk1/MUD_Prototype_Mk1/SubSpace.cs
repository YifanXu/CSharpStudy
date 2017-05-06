using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUD_Prototype_Mk1
{
    class SubSpace : IRoom
    {
        public string Name { get; }
        public int ID { get; }
        public List<NPC> NPCs { get; }
        public List<NPC> AngryNPCs { get; }
        public Dictionary<Direction,Room> ConnectingRooms { get; }
        public List<Item> Objects { get; }

        
        private const string descrip = "You feel the energy flow around you as you move through the fourth dimension. The details here are too complicated for you to comprehend.";

        public string description
        {
            get
            {
                return descrip;
            }
        }

        Room Move(Direction direction)
        {

        }

    }
}
