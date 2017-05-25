using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUD_GTK_MONO
{
    public class SubSpace : IRoom
    {
        public string Name { get; }
        public int ID { get; set; }
        public List<NPC> NPCs { get; set; }
        public List<NPC> angryNPCs { get; }
        public Dictionary<Direction,IRoom> ConnectingRooms { get; }
        public List<Item> Objects { get; set; }
        public int reactivation { get; set; }
        public IRoom desto { get; set; }
        public NPC guardian { get; }

        public SubSpace()
        {
            this.Name = "Subspace";
            this.ID = 0;
            this.NPCs = new List<NPC>();
            this.angryNPCs = new List<NPC>();
            this.ConnectingRooms = new Dictionary<Direction, IRoom>();
            this.Objects = new List<Item>();

        }
        
        public bool Locked { get; set; }

        
        private const string descrip = "You feel the energy flow around you as you move through the fourth dimension. The details here are too complicated for you to comprehend.";

        public string description
        {
            get
            {
                return descrip;
            }
        }

        

        public string[] getNPCNames()
        {
            return null;
        }

        public IRoom Move(Direction direction)
        {
            Program.write(ConsoleColor.Red, "You cannot move while in subspace.");
            return null;
        }

        public string examine(string parameter)
        {
            return "You cannot look at anything here.";
        }

        public Item obtain (string item)
        {
            return null;
        }

        public NPC getNPC (string keyword)
        {
            Program.write(ConsoleColor.Red, "Such entity does not exist.");
            return null;
        }

        public string getNPCNames (string keyword)
        {
            return null;
        }

        public string getNPCDialogue (string npc)
        {
            Program.write(ConsoleColor.Red, "No such person is in the room");
            return null;
        }

        public void ShowTravelInfo()
        {
            Program.write(ConsoleColor.Cyan, "You are already going through a portal.");
        }
        public void Pay(int offer, out string message)
        {
            message = "There is nobody to pay.";
        }
    }
}
