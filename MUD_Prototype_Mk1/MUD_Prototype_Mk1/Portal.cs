using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUD_Prototype_Mk1
{
    class Portal : Room
    {
        private readonly Room destination;
        public bool locked;
        public readonly int fee;
        public NPC guardian;
        public int reactivation = 0;

        public Room desto
        {
            get
            {
                return destination;
            }
        }

        public Portal() : base()
        {
            
        }

        public Portal(string name, string description) : base(name, description)
        {
            this.locked = false;
        }

        public Portal(string name, string description, int fee, NPC guardian) : base(name, description)
        {
            this.locked = true;
            this.fee = fee;
            this.guardian = guardian;
        }

        public override void ShowTravelInfo()
        {
            Program.write(ConsoleColor.Magenta, String.Format("This room also contains a portal that leads to {0}", destination.Name));
            if (locked)
            {
                Program.write(ConsoleColor.Red, "Your access to the portal is currently denied.");
            }
            else if (reactivation > 0)
            {
                Program.write(ConsoleColor.Cyan, "The portal is currently unstable, therefore you are unable to travel through it.");
                Program.write(ConsoleColor.Cyan, String.Format("Reactivation Timer Left: {0}", reactivation));
            }
        }

        public bool pay (int offer, out string message)
        {
            if(offer < fee)
            {
                message = "The guardian refused your offer, and the portal remains closed.";
                return false;
            }
            if(guardian.standing < -2)
            {
                message = "The guardian is hostile to you. He will reject all the offers you make.";
                return false;
            }
            if(reactivation > 0)
            {
                message = "The portal is unstable as you have traveled through it recently. You decided not to walk through it.";
                return false;
            }
            if (!locked)
            {
                message = "The portal is already unlocked.";
                return false;
            }
            message = "You have successfully accessed the portal. Congrats!";
            return false;
        }

    }
}
