using System;
using System.Collections.Generic;

namespace RocketFinances
{
	public class Task
	{
		public int time;
		public Task[] requirements;

		public Task(){
			
		}


        public Task(int time)
        {
            this.time = time;
        }

		public Task (int time, Task[] dependency) : this(time)
		{
			this.requirements = dependency;
		}

		public void GetAllRequiredTasks (List<Task> tasks){
			foreach (var node in requirements) {
				node.GetAllRequiredTasks (tasks);
			}
			foreach (Task item in tasks) {
				if (object.ReferenceEquals (item, this)) {
					return;
				}
			}
			tasks.Add (this);
		}

		public int maxRequiredTime {
			get{
				int longestDependentTime = 0;
				for (int i = 0; i < requirements.Length; i++) {
					int childMaxTime = requirements [i].maxRequiredTime;
					if (childMaxTime > longestDependentTime) {
						longestDependentTime = childMaxTime;
					}
				}
				return this.time + longestDependentTime;
			}
		}

		public override int GetHashCode ()
		{
			var hash = this.time;
			if (this.requirements != null) {
				hash += this.requirements.GetHashCode ();
			}
			return hash;
		}
        
        /// <summary>
        /// IT IS BREAKABLE PLZ DON'T USE IT
        /// </summary>
        public int maxSimutaniousOffsprings
        {
            get
            {
                if (this.requirements.Length == 0)
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

