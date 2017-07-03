using System;

namespace Calculator
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Calculator c = new Calculator ();
			string input = Console.ReadLine ();


			try{
//				Console.WriteLine (c.Calculate (input));
				Console.WriteLine (c.calculateWithoutSpace(input));
			}
			catch(ZeroPowerZeroException){
				Console.WriteLine ("An attempt was made to calculate zero to the zeroth power.");
			}
			catch(DivideByZeroException){
				Console.WriteLine ("An attempt was made to divide a number by zero.");
			}
			catch(InvalidExpressionException e){
				Console.WriteLine ("The expression was invalid. \n\n Details: The element \" {0} \" was invalid.", e.invalidElement);
			}
			catch(InvalidOperationOrderException){
				Console.WriteLine ("An operation is placed at the start or end of the expression.");
			}
			catch(OverflowException){
				Console.WriteLine ("A number or a result was so big that it exceeded the int-32 digit limit and caused an overflow.");
			}
			catch(OperationNumberNotMatchException e){
				Console.WriteLine ("The expression is invalid. \n\n Details: {0}", e.Message);
			}
			catch(Exception e){
				Console.WriteLine("An unknown exception had occured and was unhandled. \n\n Details: {0}",e.Message);
				throw;
			}
		}
	}
}
