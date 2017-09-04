using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvMUD.Entities;

namespace AdvMUD.Entities
{
    public class NPC : Entity
    {
        public Dialouge dialouges { get; set; }

        public NPC()
        {
            if (dialouges == null)
            {
                dialouges = new Dialouge();
            }
        }

        public override void Die()
        {
            Location.npcs.Remove(this);
            base.Die();
            
        }
    }
}
