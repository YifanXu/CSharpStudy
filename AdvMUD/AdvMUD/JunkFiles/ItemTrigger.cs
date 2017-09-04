using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvMUD.Questing
{
    class ItemTrigger : Trigger
    {
        public string[] itemsNeeded;
        public int[] quantityNeeded;

        public override bool IsTriggered
        {
            get
            {
                for(int i = 0; i < itemsNeeded.Length; i++)
                {
                    if(i >= quantityNeeded.Length)
                    {
                        return true;
                    }
                    Item item = Player.player.FindItemByName(itemsNeeded[i]);
                    if(item == null || item.quantity < quantityNeeded.Length)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
