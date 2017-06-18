using System;

namespace RocketFinances
{
	public class Task
	{
		public int time;
		public Task[] dependentTasks;

		public Task(){
		}

		public Task (int time, Task[] dependency)
		{
			this.dependentTasks = dependency;
			this.time = time;
		}

		public int maxRequiredTime {
			get{
				int longestDependentTime = 0;
				for (int i = 0; i < dependentTasks.Length; i++) {
					if (dependentTasks [i].maxRequiredTime > longestDependentTime) {
						longestDependentTime = dependentTasks [i].maxRequiredTime;
					}
				}
				return this.time + longestDependentTime;
			}
		}
	}
}

