using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvMUD.Questing
{
    class DialougeTrigger : Trigger
    {
        bool finished = false;
        string npcName;
        string dialougeInput;

        public override bool IsTriggered
        {
            get
            {
                return finished;
            }
        }
    }
}
