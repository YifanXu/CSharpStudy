using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUD_Prototype_Mk1
{
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
        checkInv
    }
    public class Player
    {
        public List<item> Inventory;
    }
    public class item
    {
        public string name;
        public string description;
        public bool obtainable;
        public string AttemptMessage;
    }
    public class room
    {
        public string name;
        public string description;
        public room n;
        public room s;
        public room w;
        public room e;
        public List<item> objects;

        public room move(Actions direction)
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
            foreach (item thing in objects)
            {
                if(thing.name.Equals(item,StringComparison.OrdinalIgnoreCase))
                {
                    return thing.description;
                }
            }
            return "You cannot look upon such a thing.";
        }

        public item obtain(string item)
        {
            foreach (item thing in objects)
            {
                if (thing.name.Equals(item, StringComparison.OrdinalIgnoreCase))
                {
                    if (thing.obtainable)
                    {
                        Program.write(ConsoleColor.Green, thing.AttemptMessage);
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
    }
}
