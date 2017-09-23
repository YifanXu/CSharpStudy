using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyond_The_Stars.Entities.Spaceship.Modules;

namespace Beyond_The_Stars.Entities.Spaceship.Systems
{
    public class ShipSystem
    {
        public SystemType type;
        public int slots;

        public IShipModule[] modules;

        public int hitpoints
        {
            get
            {
                int hp = 0;
                foreach (IShipModule module in modules)
                {
                    hp += module.hitpoints;
                }
                return hp;
            }
        }
    }
}
