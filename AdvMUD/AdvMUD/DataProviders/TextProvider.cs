using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using AdvMUD.Questing;

namespace AdvMUD.DataProviders
{
    public class TextProvider 
    {
        public Room GetRootRoom(string path, out Dictionary<int,Room> rooms)
        {
            rooms = null;
            return null;
        }

        public Player GetPlayer(string path)
        {
            return null;
        }

        public Quest[] GetQuests()
        {
            return null;
        }


        private int GetIndent (string str)
        {
            for(int i = 0; i < str.Length; i++)
            {
                if(str[i] != ' ')
                {
                    return i;
                }
            }
            return str.Length;
        }
    }
}
