using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyond_The_Stars.Entities;
using Beyond_The_Stars.Entities.Spaceship;

namespace Beyond_The_Stars
{
    public static class Player
    {
        public static string name { get; set; }
        public static ILocation Location { get; set; }
        public static Ship currentShip { get; set; }
        
        
    }
}
