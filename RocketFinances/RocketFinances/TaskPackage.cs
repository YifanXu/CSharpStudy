using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RocketFinances
{

    public class TaskPackage
    {
		
        public int finalTaskPosition;
        public NewTask[] tasks;
		private Dictionary<string,NewTask> newtaskIDs;

        public TaskPackage()
        {
			
        }

        public NewTask this[int id]
        {
            get
            {
                return tasks[id];
            }
        }

		public Task Assemble (){
			//TAsks
			Task[] assembledTasks = new Task[tasks.Length];
			for (int i = 0; i < assembledTasks.Length; i++) {
				assembledTasks [i] = new Task (tasks [i].executionTime.value, new Task[tasks [i].dependencyPositions.Length]);
			}
			//Connections
			for (int i = 0; i < assembledTasks.Length; i++) {
				for (int d = 0; d < tasks [i].dependencyPositions.Length; d++) {
					assembledTasks[i].requirements[d] = assembledTasks [tasks [i].dependencyPositions [d]];
				}
			}

			return assembledTasks[finalTaskPosition];
		}
    }
}
