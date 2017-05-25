using System;
using Gtk;

namespace MUD_GTK_MONO
{
	class Program
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}

		public static void write(ConsoleColor color, string text){
			
		}
	}
}
