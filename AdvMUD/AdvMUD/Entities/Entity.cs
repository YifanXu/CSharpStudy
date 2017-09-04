using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvMUD.Entities
{
    public class Entity
    {
        public string name { get; set; }
        public List<Item> inventory { get; set; }

        public int health { get; set; }
        public int mana { get; set; }
        public int Attack { get; set; }
        public int Speed { get; set; }
        public int Defense { get; set; }
        public int Resistance { get; set;}

        public int maxHealth { get; set; }
        public int maxMana { get; set;}
        public int defSpeed { get; set; }
        public int defAttack { get; set; }
        public int defDefense { get; set; }
        public int defResistance { get; set; }

        public Room Location { get; set; }
        public int standings { get; set; }

        public Entity()
        {
            maxHealth = health = 200;
            maxMana = mana = 100;
            defAttack = Attack = 20;
            defSpeed = Speed = 20;
            defDefense = Defense = 10;
            defResistance = Resistance = 10;
        }

        public virtual void Die()
        {
            if (inventory != null)
            {
                foreach (Item item in inventory)
                {
                    int dropQuantity = 0;
                    for (int i = 0; i < item.quantity; i++)
                    {
                        if (Game.r.Next(100) < item.dropChance)
                        {
                            dropQuantity++;
                        }
                    }
                    if(dropQuantity == 0)
                    {
                        continue;
                    }
                    Location.items.Add(new Item(item.name,item.desc,dropQuantity,item.dropChance));
                }
            }
            this.Location = Room.deathRoom;
        }

        public virtual void ResetStats()
        {
            this.Attack = defAttack;
            Defense = defDefense;
            Speed = defSpeed;
            Resistance = defResistance;
            health = maxHealth;
            mana = maxMana;
        }
    }
}
