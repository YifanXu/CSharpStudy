using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUD_Prototype_Mk1
{
    class Portal : Room
    {
        public readonly int fee;

        public Portal() : base ()      
        {

        }

        public Portal(string name, string description) : base(name, description)
        {
            this.Locked = false;
            this.Objects = new List<Item>();
            this.NPCs = new List<NPC>();
        }

        public Portal(string name, string description, int fee, NPC guardian) : base(name, description)
        {
            this.Locked = true;
            this.fee = fee;
            this.guardian = guardian;
            this.Objects = new List<Item>();
            this.NPCs = new List<NPC>();
            this.NPCs.Add(this.guardian);
        }

        public override void ShowTravelInfo()
        {
            if(desto == null)
            {
                Program.write(ConsoleColor.Magenta, "This room also contains a portal that doesn't work.");
                return;
            }
            Program.write(ConsoleColor.Magenta, String.Format("This room also contains a portal that leads to {0}", desto.Name));
            if (Locked)
            {
                Program.write(ConsoleColor.Red, "Your access to the portal is currently denied.");
            }
            else if (reactivation > 0)
            {
                Program.write(ConsoleColor.Cyan, "The portal is currently unstable, therefore you are unable to travel through it.");
                Program.write(ConsoleColor.Cyan, String.Format("Reactivation Timer Left: {0}", reactivation));
            }
        }

        public override void Pay (int offer, out string message)
        {
            if(offer < fee)
            {
                message = "The guardian refused your offer, and the portal remains closed.";
                return;
            }
            if(guardian.standing < -2)
            {
                message = "The guardian is hostile to you. He will reject all the offers you make.";
                return;
            }
            if(reactivation > 0)
            {
                message = "The portal is unstable as you have traveled through it recently. You decided not to walk through it.";
                return;
            }
            if (!Locked)
            {
                message = "The portal is already unlocked.";
                return;
            }
            message = "You have successfully accessed the portal. Congrats!";
            Locked = false;
        }

    }
}
