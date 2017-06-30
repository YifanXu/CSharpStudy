using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _42TrappingRainWater
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] testCase = new int[] { 0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1 };
            Console.WriteLine(Trap(testCase));

            Console.ReadLine();
        }

        public static int Trap (int[] height)
        {
            if(height.Length < 3)
            {
                return 0;
            }
            int lastHeight = 0;
            int water = 0;
            int waterTemp = 0;
            for(int i = 1; i < height.Length; i++)
            {
                if(height[i] > height[lastHeight])
                {
                    lastHeight = height[i];
                    water += waterTemp;
                    waterTemp = 0;
                }else
                {
                    waterTemp += height[lastHeight] - height[i];
                }
                if(i == height.Length- 1 && lastHeight != height.Length - 2)
                {
                    i = lastHeight + 1;
                    lastHeight = i;
                    waterTemp = 0;
                }
            }
            return water;
        }
    }
}
