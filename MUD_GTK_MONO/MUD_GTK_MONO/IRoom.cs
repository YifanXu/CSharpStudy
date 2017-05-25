using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUD_GTK_MONO
{
    public interface IRoom
    {
        string description { get; }
        string Name { get; }
        int ID { get; set; }
        List<Item> Objects { get; set; }
        List<NPC> NPCs { get; set; }
        Dictionary<Direction, IRoom> ConnectingRooms { get; }
        List<NPC> angryNPCs { get; }
        bool Locked { get; set; }
        int reactivation { get; }
        IRoom desto { get; set; }
        NPC guardian { get; }

        IRoom Move(Direction direction);
        string examine(string item);
        Item obtain(string item);
        string getNPCDialogue(string npc);
        string[] getNPCNames();
        NPC getNPC(string npc);
        void ShowTravelInfo();
        void Pay(int offer, out string message);
    }
}
