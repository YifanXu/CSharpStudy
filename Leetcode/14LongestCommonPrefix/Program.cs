using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _14LongestCommonPrefix
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] test = new string[]
            {
                "a",
                "b"
            };
            Console.WriteLine(LongestCommonPrefix(test));
            Console.ReadLine();
        }

        public static string LongestCommonPrefix(string[] strs)
        {
            if (strs.Length == 0)
            {
                return string.Empty;
            }
            string oldPrefix = "";
            string prefix = "";
            bool finished = false;
            while (true)
            {
                foreach (string str in strs)
                {
                    if (str == prefix)
                    {
                        finished = true;
                    }
                    else if (!(prefix.Length == 0) && !str.StartsWith(prefix))
                    {
                        return oldPrefix;
                    }
                }
                if (finished)
                {
                    return prefix;
                }
                oldPrefix = prefix;
                prefix = prefix + strs[0][prefix.Length].ToString();
            }
        }
    }
}
