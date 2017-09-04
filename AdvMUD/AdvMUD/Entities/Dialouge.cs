using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvMUD.Questing;

namespace AdvMUD.Entities
{
    public class Dialouge
    {
        public string input { get; set; }
        public string[] responses { get; set; }
        public Dialouge[] furtherDialouges { get; set; }
        public Trigger isAllowed { get; set; }
        public bool ifReturn { get; set;}
        public bool initiateFight { get; set;}

        public Dialouge()
        {
            if (responses == null)
            {
                responses = new string[1];
            }
        }

        public List<Dialouge> avaliableDialouges
        {
            get
            {
                List<Dialouge> d = new List<Dialouge>();
                if(furtherDialouges == null || furtherDialouges.Length == 0)
                {
                    return d;
                }
                foreach (Dialouge dialouge in furtherDialouges)
                {
                    if (dialouge.isAllowed == null || dialouge.isAllowed.IsTriggered)
                    {
                        d.Add(dialouge);
                    }
                }
                return d;
            }
        }
        
        public bool Run(Entity owner)
        {
            int choice;
            for (int i = 0; i < responses.Length; i++)
            {
                Game.WriteLine(ConsoleColor.Magenta, responses[i]);
                Console.ReadLine();
            }
            if (initiateFight)
            {
                Entity winner = Game.game.Combat(owner, Player.player);
                if(winner is Player)
                {
                    owner.Die();
                    return false;
                }else
                {
                    Player.player.Die();
                }
            }
            if (furtherDialouges == null || furtherDialouges.Length == 0)
            {
                return ifReturn;
            }
            Dialouge[] dialouges;
            do {
                dialouges = avaliableDialouges.ToArray();
                for (int i = 0; i < dialouges.Length; i++)
                {
                    Game.WriteLine(ConsoleColor.Cyan, String.Format("{0}. {1}", i, dialouges[i].input));
                }
                Game.WriteLine(ConsoleColor.Magenta, "Type in an option above or input nothing to exit conversation");
                string input;
                while (true)
                {
                    input = Console.ReadLine();
                    if (input == "")
                    {
                        return ifReturn;
                    }
                    if (int.TryParse(input, out choice) && choice >= 0 && choice < dialouges.Length)
                    {
                        break;
                    }
                    Game.WriteLine(ConsoleColor.Red, "Invalid Input");
                }
                //Check On Quests
                Player.player.CheckQuestDialouges(owner as NPC, dialouges[choice].input);
            }
            while(dialouges[choice].Run(owner));

            return ifReturn;
        }
    }
}
