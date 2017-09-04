using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvMUD
{
    public class RoomLoader
    {
        public Room[] rooms;

        public RoomLoader()
        {

        }

        public RoomLoader(Room[] rooms)
        {
            this.rooms = rooms;
        }
    }
}
