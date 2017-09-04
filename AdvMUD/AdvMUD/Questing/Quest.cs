using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvMUD.Questing
{
    public class Quest
    {
        public int id;
        public string name;
        public int stage = 0;
        public Trigger[] stageTriggers;
        public string[] messages;

        public bool checkQuest()
        {
            if (stage != stageTriggers.Length && stageTriggers[stage].IsTriggered)
            {
                stage++;
                return true;
            }
            return false;
        }
        
        public QuestStage status
        {
            get
            {
                if(stage == 0)
                {
                    return QuestStage.Avaliable;
                }
                if(stage == stageTriggers.Length)
                {
                    return QuestStage.Completed;
                }
                return QuestStage.InProgress;
            }
        }

        public string message
        {
            get
            {
                if(messages == null || messages.Length < stage || stage == 0)
                {
                    return String.Empty;
                }
                return messages[stage - 1];
            }
        }

        public string conditon
        {
            get
            {
                if(stage == stageTriggers.Length)
                {
                    return "Quest Completed";
                }
                if(stageTriggers == null || stageTriggers[stage] == null)
                {
                    return "";
                }
                Trigger current = stageTriggers[stage];
                switch (current.type)
                {
                    case TriggerType.Dialouge:
                        return String.Format("Speak to {0}",current.npcName);
                    case TriggerType.Location:
                        return String.Format("Go to {0}",Game.game.allRooms[current.roomID].name);
                    case TriggerType.Item:
                        StringBuilder str = new StringBuilder();
                        str.AppendLine("Obtain the following items...");
                        for(int i = 0; i < current.itemsNeeded.Length; i++)
                        {
                            str.AppendLine(String.Format("{0} x{1}",current.itemsNeeded[i],current.quantityNeeded[i]));
                        }
                        return str.ToString();
                    case TriggerType.Quest:
                        Quest quest = Player.player.GetQuest(current.QuestID);
                        if(current.QuestStage == quest.stageTriggers.Length)
                        {
                            return String.Format("Complete Quest \"{0}\"",quest.name);
;                        }
                        return String.Format("Advance Quest \"{0}\" to Stage {1}",quest.name,current.QuestStage);
                }

                return String.Empty;
            }
        }
    }
}
