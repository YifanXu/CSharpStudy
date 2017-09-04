using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvMUD.Questing;
using AdvMUD.Entities;

namespace AdvMUD
{
    public class Player : Entities.Entity
    {
        public static Player player { get; set; }
        public Quest[] quests { get; set; }
        public List<int> avaliableSpells { get; set; }
        public Room spawn { get; set; }

        public Player()
        {
            player = this;
            this.inventory = new List<Item>();
            this.name = "You";
            avaliableSpells = new List<int> { 0, 1 };
        }

        public Player(Room location, Quest[] quests) : this()
        {
            this.spawn = location;
            this.Location = location;
            this.quests = quests;
        }

        public Item FindItemByName (string keyword)
        {
            foreach (Item item in inventory)
            {
                if (String.Equals(keyword, item.name, StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }
            return null;
        }

        public Quest GetQuest (int id)
        {
            foreach(Quest quest in quests)
            {
                if(quest.id == id)
                {
                    return quest;
                }
            }
            return null;
        }

        public override void Die()
        {
            base.Die();
            ResetStats();
            if (spawn != null)
            {
                this.Location = this.spawn;
            }
        }

        public List<Quest> checkQuest()
        {
            List<Quest> updatedQuests = new List<Quest>(quests.Length);

            foreach(Quest quest in quests)
            {
                if (quest.checkQuest())
                {
                    updatedQuests.Add(quest);   
                }
            }

            return updatedQuests;
        }

        public void CheckQuestDialouges(NPC npc, string input)
        {
            foreach(Quest quest in quests)
            {
                if(quest.status != QuestStage.Completed)
                {
                    quest.stageTriggers[quest.stage].CheckDialouge(npc, input);
                }
            }
        }

        public List<Quest> GetQuestsByCondition(QuestStage condition)
        {
            List<Quest> activeQuests = new List<Quest>();
            foreach(Quest quest in quests)
            {
                if(quest.status == condition)
                {
                    activeQuests.Add(quest);
                }
            }
            return activeQuests;
        }
    }
}
