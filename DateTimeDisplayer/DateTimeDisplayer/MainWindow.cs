using System;
using Gtk;
using System.Threading;

public partial class MainWindow: Gtk.Window
{
	
	private Thread TimeUpdater;
	private bool Ended = false;
	Image[] images = new Image[10];
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		for (int i = 0; i < 10; i++) {
			images [i] = new Image(String.Format ("/data/{0}.png", i));
		}
		//sign1 = sign2 = new Image ("data/sign.png");
		//Gdk.Pixmap pix = new Gdk.Pixmap (new IntPtr ());
		///		sign1.SetFromImage (new Gdk.Image(, pix);
		//sign1.File = sign2.File = "/data/0.png";
		//sign1.SetFromStock("GTK_STOCK_NO",IconSize.Button);
		sign1 = sign2 = new Image("/data/sign.png");
		sign1.Show ();
		sign2.Show ();
		images [0].Show ();
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
			//time.Text = DateTime.Now.ToString ();
			Thread.Sleep (500);
		}
	}
}
