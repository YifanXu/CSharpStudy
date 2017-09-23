using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyond_The_Stars.Entities.Spaceship.Systems;

namespace Beyond_The_Stars.Entities.Spaceship
{
    public class Ship : Entity
    {
        public static Ship[] allShips;

        public Dictionary<SystemType, ShipSystem> shipSystems;
        public ShipClass shipClass;

        #region stats
        public int Structure
        {
            get
            {
                int hp = shipClass.baseHull;
                foreach(ShipSystem system in shipSystems.Values)
                {
                    hp += system.hitpoints;
                }
                return hp;
            }
        }

        public int Armor
        {
            get
            {
                return 0;
            }
        }

        public int Shield
        {
            get
            {
                return 0;
            }
        }
        #endregion

        public Ship(ShipClass shipClass)
        {
            this.shipClass = shipClass;

        }
    }
}
