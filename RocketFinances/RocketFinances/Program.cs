using System;
using System.Collections.Generic;

namespace RocketFinances
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			
		}

		public static int GetTotalCost (Task rootTask){
			const int cost = 10000;

			List<Task> allTasks = GetAllTasks (rootTask);
			//Count up the total
			int totalCost = 0;
			foreach (Task task in allTasks) {
				totalCost += task.time*cost;
			}
			return totalCost;
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
					newLayerMember.AddRange (member.dependentTasks);
					countedTasks.Add (member);
				}
			}

			return countedTasks;
		}

		public static int GetShortestTime(Task rootTask){
			
		}
	}
}
