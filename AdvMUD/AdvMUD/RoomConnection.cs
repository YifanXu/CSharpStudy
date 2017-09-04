using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvMUD.Questing;

namespace AdvMUD
{
    public class RoomConnection
    {
        public string direction { get; set; }
        public int targetID { get; set; }
        public Room targetRoom { get; set; }
        public Trigger trigger { get; set; }

        public RoomConnection()
        {

        }

        public RoomConnection (string direction, int targetID)
        {
            this.direction = direction;
            this.targetID = targetID;
        }

        public bool ValidateConnection (Dictionary<int,Room> rooms)
        {
            Room target;
            if (rooms.TryGetValue(targetID, out target))
            {
                targetRoom = target;
                return true;
            }
            return false;
        }

        public bool IsValid
        {
            get
            {
                if (trigger == null || trigger.IsTriggered) {
                    return true;
                }
                return false;
            }
        }
    }
}
