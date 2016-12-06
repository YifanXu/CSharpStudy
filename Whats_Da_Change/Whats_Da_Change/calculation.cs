using System;

namespace Whats_Da_Change
{
	public class calculation
	{
		public int[] calc (int money)
		{
            int[] coinValues = new int[]{25, 10, 5, 1};
			int[] changes = new int[coinValues.Length];
            for(int i = 0; i < coinValues.Length; i++)
			{
                changes[i] = money / coinValues[i];
				money = money % coinValues[i];
			}
			return changes;
		}
	}
}

