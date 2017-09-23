using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyond_The_Stars.Entities.Spaceship.Modules
{
    public class WeaponMod : IShipModule
    {
        public string name { get; set; }
        public string description { get; set; }
        public SystemType type { get; }

        //Combat
        public int recharge { get; set; }
        public int currentRecharge { get; set; }
        public int hitpoints { get; set; }
        public bool damaged { get; set; }
    }
}
