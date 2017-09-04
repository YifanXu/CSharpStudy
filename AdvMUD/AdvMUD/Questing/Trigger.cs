using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvMUD.Entities;

namespace AdvMUD.Questing
{
    public class Trigger
    {
        public TriggerType type;

        //Item
        public string[] itemsNeeded;
        public int[] quantityNeeded;
        
        //Quest
        public int QuestID;
        public int QuestStage;

        //Dialouge
        public bool finished = false;
        public string npcName;
        public string dialougeInput;

        //Location
        public int roomID;

        public Trigger()
        {

        }

        public Trigger(string[] itemsNeeded, int[] quantitiesNeeded)
        {
            type = TriggerType.Item;
            this.itemsNeeded = itemsNeeded;
            this.quantityNeeded = quantitiesNeeded;
        }

        public Trigger(int QuestID, int QuestStage)
        {
            type = TriggerType.Quest;
            this.QuestID = QuestID;
            this.QuestStage = QuestStage;
        }

        public Trigger(string npcName, string dialougeInput)
        {
            type = TriggerType.Dialouge;
            this.npcName = npcName;
            this.dialougeInput = dialougeInput;
        }

        public Trigger(int RoomID)
        {
            type = TriggerType.Location;
            this.roomID = RoomID;
        }

        public virtual bool IsTriggered
        {
            get
            {
                switch (type)
                {
                    case TriggerType.Item:
                        for (int i = 0; i < itemsNeeded.Length; i++)
                        {
                            if (i >= quantityNeeded.Length)
                            {
                                return true;
                            }
                            Item item = Player.player.FindItemByName(itemsNeeded[i]);
                            if (item == null || item.quantity < quantityNeeded[i])
                            {
                                return false;
                            }
                        }
                        return true;
                    case TriggerType.Dialouge:
                        return finished;
                    case TriggerType.Quest:
                        Quest quest = Player.player.GetQuest(QuestID);
                        if (quest != null && quest.stage == QuestStage)
                        {
                            return true;
                        }
                        return false;
                    case TriggerType.Location:
                        if(Player.player.Location.id == roomID)
                        {
                            return true;
                        }
                        return false;
                }
                return true;
            }
        }

        public virtual void CheckDialouge (NPC npc, string input)
        {
            if(this.type == TriggerType.Dialouge && npc.name == npcName && input == dialougeInput)
            {
                this.finished = true;
            }
        }
    }
}
