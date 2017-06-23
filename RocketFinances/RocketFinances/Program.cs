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
            Task root = getTaskFromFile("data.xml");

            int time = GetTotalTime(root);
            Console.WriteLine("Total Estimated Cost: ${0}.00 (Total Time is {1})", time*cost, time);
            Console.WriteLine("Shortest Predicted Time: {0}", root.maxRequiredTime);
            Console.WriteLine("Contracts Needed: {0} ({1})", GetMaxSimutaniousOffSprings(root), root.maxSimutaniousOffsprings);

            Console.ReadLine();
        }

        public static Task getTaskFromFile(string path)
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

            //Get Connections
            foreach (TaskConnection connection in package.connections)
            {
                package[connection.baseTask].requirements.Add(package[connection.predecessor]);
            }

            return package.tasks[package.finalTaskPosition];
        }

		public static int GetTotalTime (Task rootTask){

			List<Task> allTasks = GetAllTasks (rootTask);
			//Count up the total
			int totalTime = 0;
			foreach (Task task in allTasks) {
				totalTime += task.time;
			}
			return totalTime;
		}

		public static List<Task> GetAllTasks (Task finalTask){
			List<Task> countedTasks = new List<Task> ();
			List<Task> layerMember = new List<Task> ();
			List<Task> newLayerMember = new List<Task> { finalTask };
			while (newLayerMember.Count != 0) {
				layerMember.Clear ();
				layerMember.AddRange (newLayerMember);
				newLayerMember.Clear ();

				foreach (Task member in layerMember) {
					//Checking if the memeber is already counted
					bool memeberAlreadyExists = false;
					foreach (Task countedTask in countedTasks) {
						if(object.ReferenceEquals(countedTask,member)){
							memeberAlreadyExists = true;
							break;
						}
					}
					if (memeberAlreadyExists) {
						continue;
					}
					newLayerMember.AddRange (member.requirements);
					countedTasks.Add (member);
				}
			}

			return countedTasks;
		}


        public static int GetMaxSimutaniousOffSprings(Task root)
        {
            List<Task> allTask = GetAllTasks(root);
            Dictionary<Task, int> timeTable = new Dictionary<Task, int>();
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
