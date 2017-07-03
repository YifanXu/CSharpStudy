using System;
using System.Linq;
using System.Collections.Generic;

namespace Calculator
{
	public class Calculator
	{
		public static readonly Dictionary<string,Func<int,int,int>> AllowedOperations = new Dictionary<string, Func<int,int,int>> () {
			{ "^", Power },
			{ "*" ,Multiply},
			{ "/" ,Divide},
			{ "+" ,Add},
			{ "-" ,Subtract}
		};

		private static readonly char[] AllowedOps = AllowedOperations.Keys.Select (k => k [0]).ToArray();
			//new char[]{'^','*','/','+','-'};

		public int Calculate(string expression){
			string[] input = expression.Split(' ');
			List<string> elements = new List<string>();
			elements.AddRange(input);

			foreach (string operation in AllowedOperations.Keys) {
				bool containOperator = true;
				while (containOperator) {
					containOperator = false;
					for (int i = 0; i < elements.Count; i++) {
						if (elements [i] == operation) {
							if (i == 0 || i == elements.Count - 1) {
								throw new InvalidOperationOrderException();
							}
							containOperator = true;
							int n1;
							int n2;
							try{
								n1 = int.Parse(elements [i - 1]);
							}
							catch(FormatException){
								throw new InvalidExpressionException(elements[i - 1]);
							}
							try{
								n2 = int.Parse(elements [i + 1]);
							}
							catch(FormatException){
								throw new InvalidExpressionException (elements[i + 1]);
							}
							int result = AllowedOperations [operation] (n1, n2);
							elements.Insert (i + 2, result.ToString ());
							elements.RemoveRange (i - 1, 3);
						}
					}
				}
			}
			if (elements.Count > 1) {
				throw new InvalidExpressionException (elements [1]);
			}

			return int.Parse (elements [0]);
		}


		public int calculateWithoutSpace(string expression){
			//Find All Operators
			List<string> operations = new List<string>();
			for(int i = 0; i < expression.Length; i++){
				string character = expression [i].ToString ();
				if (AllowedOperations.ContainsKey (character)) {
					if (i == 0 || i == expression.Length - 1) {
						throw new InvalidOperationOrderException ();
					}
					operations.Add (character);
				}
			}

			//Get All Numbers
			string[] input = expression.Split (AllowedOps, StringSplitOptions.RemoveEmptyEntries);

			//Make sure the integrity is ok
			if (input.Length != operations.Count + 1) {
				throw new OperationNumberNotMatchException (operations.Count, input.Length);
			}

			List<int> nums = new List<int>();
			foreach (string element in input) {
				int parseResult;
				if(!int.TryParse(element, out parseResult)){
					throw new InvalidExpressionException (element);
				}
				nums.Add (parseResult);
			}



			//Acutal Calculations
			foreach(var pair in AllowedOperations){
				//Carry out Operations One by one
				bool containsOperation = true;
				while(containsOperation){
					containsOperation = false;
					for(int i = operations.Count - 1; i >= 0; i--){
						if (operations [i] == pair.Key) {
							containsOperation = true;
							int result = pair.Value (nums [i], nums [i + 1]);
							nums [i] = result;
							nums.RemoveAt (i + 1);
							operations.RemoveAt (i);
						}
					}
				}

			}

			return nums [0];
		}

		private static int Add (int n1, int n2){
			return checked (n1+n2);
		}

		private static int Subtract (int n1, int n2){
			return checked (n1 - n2);
		}

		private static int Multiply (int n1, int n2){
			return checked (n1 * n2);
		}
		private static int Divide (int n1, int n2){
			if (n2 == 0) {
				throw new DivideByZeroException();
			}
			return checked (n1 / n2);
		}
		private static int Power( int n1, int n2){
			if( n1 == 0 && n2 == 0){
				throw new ZeroPowerZeroException ("An attempt was made to calculate zero to the zeroth power.");
			}
			return checked ((int) Math.Round(Math.Pow ((double)n1, (double)n2)));
		}

		private void CheckValidity (List<string> elements){
			foreach (string element in elements) {
				int outInt;
				Func<int,int,int> uselessOut;
				if(!int.TryParse(element,out outInt) && !AllowedOperations.TryGetValue(element,out uselessOut))
				{
					throw new InvalidExpressionException(element);
				}
			}
		}
	}
}

