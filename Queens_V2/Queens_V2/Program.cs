using System;

namespace Queens_V2
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			
			int[] QueensByX = new int[8];
			SolvePositions (QueensByX);
			Display (QueensByX);
		}
		 
		public static bool IsPositionValid (int[] queens, int currentQueen){
			for(int i = 0; i < currentQueen; i++) {
				if (queens[currentQueen] == queens[i] || Math.Abs(i - currentQueen) == Math.Abs(queens[i] - queens[currentQueen])) {
					return false;
				}
			}
			return true;
		}

		public static void SolvePositions(int[] queens){
			for (int i = 0; i < queens.Length; i++) {
				if(!IsPositionValid(queens,i) || queens[i] > 7){
					if (queens [i] >= 7) {
						queens [i] = 0;
						i --;
					}
					queens [i]++;
					i--;
				}
			}
		}

		public static bool SolveWithRecursion(int[] queens, int position){
			if (position == 8) {
				return true;
			}
			for (queens [position] = 0; queens [position] < 8; queens [position]++) {
				if (IsPositionValid (queens, position)) {
					return true;
				}
			}
			return false;
		}

		public static void SolveWithRecursion(int[] queens){
			if(!SolveWithRecursion(queens, 0)){
				throw new Exception ("Not solved!");
			}
		}

		public static void Display (int[] queens){
			for (int y = 0; y < 8; y ++) {
				for (int x = 0; x < 8; x++) {
					if (y == queens [x]) {
						write (ConsoleColor.Black);
					} else {
						write (ConsoleColor.White);
					}
				}
				Console.WriteLine ();
			}
		}

		private static void write(ConsoleColor color){
			Console.ForegroundColor = color;
			Console.BackgroundColor = color;
			Console.Write ("xx");
			Console.ResetColor ();
		}
	}
}
