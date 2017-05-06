using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUD_Prototype_Mk1
{
    interface IRoom
    {
        string description { get; }
        string Name { get; }
        int ID { get; }
        List<Item> Objects { get; set; }
        List<NPC> NPCs { get; set; }
        Dictionary<Direction,Room> ConnectingRooms { get; }
        List<NPC> angryNPCs { get; }

        Room Move(Direction direction);
        string examine(string item);
        Item obtain(string item);
        string[] getNPCNames();
        string getNPCDialogue(string npc);
        NPC getNPC(string npc);
        void ShowTravelInfo();
    }
}
