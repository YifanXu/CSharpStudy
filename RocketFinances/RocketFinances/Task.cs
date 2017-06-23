using System;
using System.Collections.Generic;

namespace RocketFinances
{
	public class Task
	{
		public int time;
		public List<Task> requirements;

		public Task(){
            this.requirements = new List<Task>();
		}

        public Task(int time) : this()
        {
            this.time = time;
        }

		public Task (int time, Task[] dependency) : this(time)
		{
			this.requirements.AddRange(dependency);
		}

		public int maxRequiredTime {
			get{
				int longestDependentTime = 0;
				for (int i = 0; i < requirements.Count; i++) {
					if (requirements [i].maxRequiredTime > longestDependentTime) {
						longestDependentTime = requirements [i].maxRequiredTime;
					}
				}
				return this.time + longestDependentTime;
			}
		}
        
        public int maxSimutaniousOffsprings
        {
            get
            {
                if (this.requirements.Count == 0)
                {
                    return 1;
                }
                int layer = 0;

                foreach(Task offspring in this.requirements)
                {
                    layer += offspring.maxSimutaniousOffsprings;
                }

                return layer;
            }
        }
	}
}

