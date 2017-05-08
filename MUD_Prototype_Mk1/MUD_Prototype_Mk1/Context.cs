using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUD_Prototype_Mk1
{
    public class Context
    {
        public IRoom room;

        public Context (IRoom room)
        {
            this.room = room;
        }
    }
}
