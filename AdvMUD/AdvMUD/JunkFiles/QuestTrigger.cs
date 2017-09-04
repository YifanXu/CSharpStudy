using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvMUD.Questing
{
    class QuestTrigger : Trigger
    {
        int QuestID;
        int QuestStage;

        public override bool IsTriggered
        {
            get
            {
                Quest quest = Player.player.GetQuest(QuestID);
                if(quest != null && quest.stage == QuestStage)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
