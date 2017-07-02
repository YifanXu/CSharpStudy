using System;
using System.Xml;

namespace RocketFinances
{
	public class ExecutionTime{
		[XmlAttribute]
		public int value {get; set;}
	}

	public class NewTask
	{
		public string name;
		public ExecutionTime executionTime;
		public int[] dependencyPositions;

		public NewTask ()
		{
			
		}
			
	}
}

