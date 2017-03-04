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
        Get
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
    }
}
