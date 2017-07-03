using System;

namespace Calculator
{

	public class ZeroPowerZeroException : Exception
	{
		public ZeroPowerZeroException (string message): base(message)
		{
			
		}
	}

	public class InvalidOperationOrderException :Exception{
		public InvalidOperationOrderException (): base("An operation is placed at the start or end of the expression.")
		{

		}
	}

	public class InvalidExpressionException : Exception{
		public string invalidElement;
		public InvalidExpressionException(string invalidElement) : base("An element in the expression is invalid.") {
			this.invalidElement = invalidElement;
		}
	}

	public class OperationNumberNotMatchException : ArgumentException{
		public OperationNumberNotMatchException (int opCount, int numCount) : base (string.Format("{0} operators were expected but {1} operators are received.", numCount - 1, opCount)){
		}
	}
}

