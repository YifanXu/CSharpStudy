using System;
using System.Xml;
using System.Collections.Generic;

namespace Encoder
{
	public class Document
	{
		DateTime created;
		int[] encodedNumbers;
		string password;

		public Document ()
		{
			created = DateTime.Now;
			encodedNumbers = new int[]{ 1, 49, 17, 63, 31, 1 };
			password = "rainbows";
		}
	}
}

