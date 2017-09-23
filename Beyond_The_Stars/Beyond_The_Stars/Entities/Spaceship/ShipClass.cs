using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyond_The_Stars.Entities.Spaceship
{
    public class ShipClass
    {
        public static ShipClass capsule = new ShipClass("Escape Pod", "A small escape pod, a ship with only the basic propulsion systems.", 50);
        public static ShipClass[] startingClasses;

        public string name { get; private set; }
        public string description { get; private set; }
        public SystemType[] startingSystems;

        //Stats
        public int baseHull { get; set; }

        public ShipClass (string name, string description, int baseHull)
        {
            this.name = name;
            this.description = description;
            this.baseHull = baseHull;
        }
    }
}
