using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyond_The_Stars.Entities.Celestials
{
    public class Celestial : ILocation
    {
        public string name { get; set; }
        public string description { get; set; }
    }
}
