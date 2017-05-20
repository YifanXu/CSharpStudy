using System;
using Gtk;

public partial class MainWindow: Gtk.Window
{
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnButton20Clicked (object sender, EventArgs e)
	{
		int n1, n2;
		if (!AreNumbersValid (num1.Text, num2.Text, out n1, out n2)) {
			result.Text = "Invalid Input";
		} else {
			result.Text = (n1 + n2).ToString ();
		}
	}

	protected bool AreNumbersValid (string a, string b, out int n1, out int n2)
	{
		if (!string.IsNullOrEmpty(a) && !string.IsNullOrEmpty(b) && int.TryParse (a, out n1) && int.TryParse (b, out n2)) 
		{
			return true;
		}
		n1 = 0;
		n2 = 0;
		return false;
	}

	protected void OnButton21Clicked (object sender, EventArgs e)
	{
		int n1, n2;
		if (!AreNumbersValid (num1.Text, num2.Text, out n1, out n2)) {
			result.Text = "Invalid Input";
		} else {
			result.Text = (n1 - n2).ToString ();
		}
	}


	protected void OnButton23Clicked (object sender, EventArgs e)
	{
		int n1, n2;
		if (!AreNumbersValid (num1.Text, num2.Text, out n1, out n2)) {
			result.Text = "Invalid Input";
		} else {
			result.Text = (n1 * n2).ToString ();
		}
	}

	protected void OnButton24Clicked (object sender, EventArgs e)
	{
		int n1, n2;
		if (!AreNumbersValid (num1.Text, num2.Text, out n1, out n2)) {
			result.Text = "Invalid Input";
		} else if (n2 == 0) {
			result.Text = "Cannot divide by 0";
		} else {
			result.Text = (n1 + n2).ToString ();
		}
	}

	protected void OnButton26Clicked (object sender, EventArgs e)
	{
		num1.Text = num2.Text = result.Text = string.Empty;
	}
}
