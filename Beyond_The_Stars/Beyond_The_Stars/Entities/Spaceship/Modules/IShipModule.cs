using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyond_The_Stars.Entities.Spaceship.Modules
{
    public interface IShipModule
    {
        string name { get; set; }
        string description { get; set; }
        SystemType type { get; }

        //Combat
        int recharge { get; set; }
        int currentRecharge { get; set; }
        int hitpoints { get; set; }
        bool damaged { get; set; }

    }
}
