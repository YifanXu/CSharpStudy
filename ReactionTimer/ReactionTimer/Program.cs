using System;
using System.Threading;

namespace ReactionTimer
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			bool keyPress = false;
			bool timerStart = false;
			bool cheated = false;
			var r = new Random ();
			string readLineInput;
			int currentR;
			int time = 0;
			int chance = 0;
			Console.WriteLine ("Hey there! It would seem that you are seeking a challenge here!");
			Console.WriteLine ("The rules are simple... The white blocks will appear randomly, but wait until the red square pop up, and press a key as fast as possible");
			Console.WriteLine ("When you are ready, press a key to initiate the game. Best of Luck, challenger.");
			while (!keyPress) 
			{
				keyPress = Console.KeyAvailable;
			}
			Console.BackgroundColor = ConsoleColor.Gray;
			Console.ForegroundColor = ConsoleColor.Gray;
			while (Console.KeyAvailable) 
			{
				Console.ReadKey (true);
			};
			while (true){
				while (true)
				{
					keyPress = Console.KeyAvailable;
					if (!timerStart) {
						currentR = r.Next (10000);
						if (keyPress) 
						{
							cheated = true;
							break;
						}
						if (currentR > 8000) 
						{
							Console.Write ("X");
							chance+=2;
						}
						if (currentR < chance) 
						{
							timerStart = true;
							Console.BackgroundColor = ConsoleColor.Red;
							Console.ForegroundColor = ConsoleColor.Red;
						} else {
							Thread.Sleep (50);
						}
					}
					else 
					{
						Thread.Sleep (10);
						Console.Write ("x");
						time++;
						if (keyPress) 
						{
							break;
						}
					}
				}
				Console.ResetColor ();
				Console.WriteLine ();
				if (cheated) 
				{
					Console.WriteLine ("Wow, it seems like you pressed a key before the red square showed up.");
					Console.WriteLine ("It would seem that all you want to do is cheat, am I wrong?");
				} 
				else 
				{
					Console.WriteLine ("Took you forever, dummy. I was getting old waiting for you to press your key.");
					Console.WriteLine ("Wow, you spent {0} seconds just figuring out how to move your fingers.",((double)time)/200);
				}
				readLineInput = null;
				Console.WriteLine("If you are so ashamed of your failure and you want to leave immediately, type in anything then hit Enter, and go on to... uh, contributing to society?");
				Console.WriteLine("Or you can simply press enter to keep playing, like all the cool kids do these days.");
				Thread.Sleep (1000);
				while (Console.KeyAvailable) 
				{
					Console.ReadKey (true);
				}
				readLineInput = Console.ReadLine ();
				if(!string.IsNullOrEmpty(readLineInput))
				{
					Console.WriteLine ("Haha, maybe you should leave after all. I mean.... look at your score. Ha, ha, ha.");
					break;
				}
				else{
					Console.WriteLine("It seems you have taken your choice. I don't think you will be getting any better, though.");
					Console.WriteLine();
					keyPress = false;
					timerStart = false;
					cheated = false;
					time = 0;
					chance = 0;
					Console.BackgroundColor = ConsoleColor.Gray;
					Console.ForegroundColor = ConsoleColor.Gray;
				}
			}
		}
	}
}
