using System;
using Gtk;
using System.Threading;
using System.IO;

public partial class MainWindow: Gtk.Window
{
	
	private Thread TimeUpdater;
	private bool Ended = false;
	Gdk.Pixbuf[] images = new Gdk.Pixbuf[10];
	Gdk.Pixbuf signImage = new Gdk.Pixbuf(File.ReadAllBytes("data/sign.png"));
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		for (int i = 0; i < 10; i++) {
			var buffer = File.ReadAllBytes(String.Format("data/{0}.png",i));
			images [i] = new Gdk.Pixbuf (buffer);
		}
		sign1.Pixbuf = sign2.Pixbuf = signImage;
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
			DateTime now = DateTime.Now;
			h1.Pixbuf = images [now.Hour / 10];
			h2.Pixbuf = images [now.Hour % 10];
			m1.Pixbuf = images [now.Minute / 10];
			m2.Pixbuf = images [now.Minute % 10];
			s1.Pixbuf = images [now.Second / 10];
			s2.Pixbuf = images [now.Second % 10];
			Thread.Sleep (500);
		}
	}
}
