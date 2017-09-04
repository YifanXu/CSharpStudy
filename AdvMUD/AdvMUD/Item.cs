using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvMUD
{
    public class Item
    {
        public string name { get; set; }
        public string desc { get; set; }
        public int quantity { get; set; }
        public int dropChance { get; set; }
        public bool obtainable { get; set; }

        public Item()
        {
            quantity = 1;
            dropChance = 50;
            obtainable = true;
        }
        
        public Item (string name, string desc, int quantity)
        {
            this.name = name;
            this.desc = desc;
            this.quantity = quantity;
            this.dropChance = 50;
        }

        public Item (string name, string desc, int quantity, int dropChance) : this(name,desc,quantity)
        {
            this.dropChance = dropChance;
        }
    }
}
