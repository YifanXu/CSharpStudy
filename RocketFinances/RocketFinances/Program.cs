using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;


namespace RocketFinances
{
	class MainClass
	{
        public static void Main(string[] args)
        {
            const int cost = 10000;
			Task root = GetTaskFromFile("data.xml");

			int time = GetAmountOfTime(root);
            Console.WriteLine("Total Estimated Cost: ${0}.00 (Total Time is {1})", time*cost, time);
            Console.WriteLine("Shortest Predicted Time: {0}", root.maxRequiredTime);
            Console.WriteLine("Contracts Needed: {0} ({1})", GetMaxSimutaniousOffSprings(root), root.maxSimutaniousOffsprings);

            Console.ReadLine();
        }

		private class Compare : IEqualityComparer<Task>
		{
			public bool Equals (Task x, Task y){
				return object.ReferenceEquals (x, y);
			}

			public int GetHashCode (Task task)
			{
				return task.GetHashCode ();
			}

		}
		/// <summary>
		/// Don't worry abput it
		/// </summary>
		/// <returns>The final task from file.</returns>
		/// <param name="path">XML Document Path</param>
        public static Task GetTaskFromFile(string path)
        {
            //Get Rooms
            if (!File.Exists(path))
            {
                return null;
            }
            TaskPackage package;
            var serializer = new XmlSerializer(typeof(TaskPackage));
            using (Stream s = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
               package = (TaskPackage) serializer.Deserialize(s);
            }

			return package.Assemble();
        }

		public static int GetAmountOfTime (Task rootTask){

			List<Task> tasks = new List<Task> ();
			rootTask.GetAllRequiredTasks (tasks);
			int totalTime = 0;
			foreach (Task task in tasks) {
				totalTime += task.time;
			}
			return totalTime;
		}


		private static readonly Compare comparer = new Compare ();

        public static int GetMaxSimutaniousOffSprings(Task root)
        {
			List<Task> allTask = new List<Task>();
			root.GetAllRequiredTasks (allTask);
			Dictionary<Task, int> timeTable = new Dictionary<Task, int>(comparer);
            foreach(Task task in allTask)
            {
                timeTable.Add(task, task.time);
            }
            List<Task> currentTasks = new List<Task>() { root };

            int maxSimuTask = 1;

            //Time
            int runTime = root.maxRequiredTime;
            for(int time = 0; time < root.maxRequiredTime; time++)
            {
                if(maxSimuTask < currentTasks.Count)
                {
                    maxSimuTask = currentTasks.Count;
                }

                int immediateTaskCount = currentTasks.Count;
                List<Task> RemovePile = new List<Task>();
                for (int i = 0; i < immediateTaskCount; i++)
                {
                    if (timeTable[currentTasks[i]] == 0)
                    {
                        currentTasks.AddRange(currentTasks[i].requirements);
                        RemovePile.Add(currentTasks[i]);
                    }
                    timeTable[currentTasks[i]]--;
                }
                foreach(Task task in RemovePile)
                {
                    currentTasks.Remove(task);
                }
            }

            return maxSimuTask;
        }
	}
}
