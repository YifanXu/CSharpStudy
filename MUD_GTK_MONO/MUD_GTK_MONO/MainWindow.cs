using System;
using Gtk;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using MUD_GTK_MONO;

public partial class MainWindow: Gtk.Window
{
	//private Act action = new Act ();
	private bool submitted = false;
	private bool initialized = false;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		Stage.Text = "New Or Load?";
	}

	private void init(){
		Stage.Text = "New Or Load?";
		//string input = Console.ReadLine();
//		while (string.IsNullOrEmpty(input) || (input != "new" && input != "load")) {
//			write(ConsoleColor.Red, "Invalid Input");
//			input = Console.ReadLine();     
//		}
//		if(input == "new")
//		{
//			player = new Player();
//		}else
//		{
//			XmlSerializer seralizer = new XmlSerializer(typeof(Player));
//			using (Stream s = new FileStream(path.playerSave, FileMode.Open, FileAccess.Read))
//			{
//				player = (Player) seralizer.Deserialize(s);
//			}
//		}
//		IRoom entrance = ReadFile(path.defaultMap,player.position);
//		player.current = entrance;
//		Act action = new Act(entrance);
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	private bool checkInput(out string text){
		if (submitted) {
			submitted = false;
			text = input.Text;
			input.Text = string.Empty;
			return true;
		}
		text = "";
		return false;
	}

	private bool equal (string a, string b){
		return string.Equals (a, b, StringComparison.OrdinalIgnoreCase);
	}

	protected void OnSubmitButtonClicked (object sender, EventArgs e)
	{
		if (!initialized) {
			if(equal(input.Text,"new") && equal(input.Text,"load")){
				return;
			}
			if (equal (input.Text, "new")) {
			
			}
		}
	}
}
