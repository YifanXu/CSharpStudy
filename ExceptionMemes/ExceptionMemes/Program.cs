
using System;
using System.IO;

namespace ExceptionMemes
{
	public class MyDankException : Exception
	{
		public MyDankException (string message) : base(message){
			
		}
	}

	class MainClass
	{
		private const string DefaultFilename = "test.txt";
		public static void Main (string[] args)
		{
			string data;
			try{
				data = ReadRoom ("sample.txt");
			}
			catch(MyDankException ex){
				Console.WriteLine ("Could not find {0}. Using {1} instead.", ex.Message, DefaultFilename);
				data = ReadRoom (DefaultFilename);
			}
			Console.WriteLine (data);
		}

		public static string ReadRoom (string path){
			return ReadFile(path);
		}

		public static string ReadFile (string path){
			try{
				return File.ReadAllText (path);
			}
			catch(ArgumentNullException){
				Console.WriteLine ("File Name is required.");
			}
			catch(FileNotFoundException exp){
				//Console.WriteLine ("Could Not load room file. \n Details: {0}", exp.Message);
				throw new MyDankException(exp.FileName);
			}
			catch (Exception exp){
				Console.WriteLine ("Errorrror: {0}", exp.Message);
			}

			return null;
		}
	}
}
