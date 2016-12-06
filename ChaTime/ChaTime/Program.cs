using System;
using System.Text;
using Lesson002_02_DataTransfer;

namespace ChaTime
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			byte[] bite;
			var dataTransfer = Encoding.ASCII;
			byte[] SendingDataLength = new byte[1];
			string host;
			byte[] RecievingDataLength = new byte[1];
			byte[] RecievingMassage;
			string decodedMassage;
			bool coversationFinished = false;
			if (args [0] == "server") {
				host = null;
			} else {
				host = args [0];
			}
			var ch = new Chat (host, Int32.Parse(args[1]));
			while (true) {
				DateTime rightnow = DateTime.Now;
				while (ch.Available > 0) {
					ch.Receive (RecievingDataLength);
					RecievingMassage = new byte[(int)RecievingDataLength [0]];
					ch.Receive (RecievingMassage);
					decodedMassage = dataTransfer.GetString (RecievingMassage);
					rightnow = DateTime.Now;
					if (decodedMassage == "exit") {
						Console.WriteLine ("The Other User Has Left.");
						coversationFinished = true;
						break;
					}
					Console.WriteLine ("[{0:t}]: {1}", rightnow, decodedMassage);
				}

				if (coversationFinished) {
					break;
				}
				string massage = Console.ReadLine ();
				if (!string.IsNullOrEmpty (massage)) {
					SendingDataLength [0] = (byte)massage.Length;
					bite = dataTransfer.GetBytes (massage);
					ch.Send (SendingDataLength);
					ch.Send (bite);
					if (massage == "exit") {
						Console.WriteLine ("You hav exited the conversation");
						break;
					}
				}
			}
			ch.Dispose ();
		}
	}
}
