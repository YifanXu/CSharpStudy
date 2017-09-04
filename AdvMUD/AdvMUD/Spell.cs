using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvMUD.Entities;

namespace AdvMUD
{
    public static class Spell
    {
        public static string[] spellNames = new string[]
        {
            "Attack",
            "ManaStrike",
            "Mana Neutralizer"
        };

        public static int[] manaCosts = new int[]
        {
            0,
            50,
            0
        };
        public static void Cast(int id, Entity caster, Entity target)
        {
            int damage;
            switch (id)
            {
                case 0:
                    damage = caster.Attack - target.Defense;
                    target.health -= damage;
                    Game.WriteLine(ConsoleColor.Green, String.Format("You did {0} damage to {1}",damage,target.name));
                    return;
                case 1:
                    damage = caster.Attack + caster.mana - target.Defense;
                    target.health -= damage;
                    Game.WriteLine(ConsoleColor.Green, String.Format("You did {0} damage to {1}", damage, target.name));
                    return;
                case 2:
                    int drain = Math.Min(caster.mana, target.mana);
                    caster.mana -= drain;
                    target.mana -= drain;
                    Game.WriteLine(ConsoleColor.Yellow, String.Format("Both you and {1} lost {0} mana.", drain, target.name));
                    return;
            }
        }
    }
}
