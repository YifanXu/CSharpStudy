using System;
using Gtk;
using System.Threading;

public partial class MainWindow: Gtk.Window
{
	private Thread TimeUpdater;
	private bool Ended = false;
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		TimeUpdater = new Thread(UpdateTime);
		TimeUpdater.Start ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Ended = true;
		TimeUpdater.Join (1000);
		Application.Quit ();
		a.RetVal = true;
	}

	private void UpdateTime (){
		while (!Ended) { 
			time.Text = DateTime.Now.ToString ();
			Thread.Sleep (500);
		}
	}
}
