using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculation_Test
{
    public class Expression
    {
        private static Dictionary<char, Func<int, int, int>> calculation = new Dictionary<char, Func<int, int, int>>{
            {'^', Power },
            {'*', Multiply },
            {'/', Divide },
            {'+', Add },
            {'-', Subtract }
        };

        public static Level[] Levels;

        //----------BELOW ARE ALL PART OF GENERATOR STARTER PACK---------------
        private static char[][] allowedOperations = new char[][]
        {
            new char[] {'+', '-'},
            new char[] {'+', '-'},
            new char[] {'+', '-'},
            new char[] {'*', '+', '-'},
            new char[] {'*', '+', '-',}
        };
        private static int[] numberLimit = new int[] { 10, 50, 100, 100, 10 };
        private static int[] resultLimit = new int[] { 10, 50, 100, 100, 10 };
        private static int[] memberCountLimit = new int[] { 2, 2, 3, 3, 3 };
        private static bool[] allowParantheses = new bool[] { false, false, false, false, true };
        //---------ABOVE ARE ALL PART OF GENERATOR STARTER PACK---------------

        private static char[] operatorsByOrder = new char[] { '+', '-', '*', '/', '^'};

        public bool isInt;
        public int directValue;

        public int level = -1;

        public Expression firstElement;
        public Expression secondElement;
        public char operation;

        public Expression(int value)
        {
            this.directValue = value;
            this.isInt = true;
        }

        public Expression (Expression a, Expression b, char operation)
        {
            this.isInt = false;
            this.directValue = -1;
            firstElement = a;
            secondElement = b;
            if (calculation.ContainsKey(operation))
            {
                this.operation = operation;
            }
            else
            {
                throw new Exception("Invalid operation detected");
            }
        }

        

        //Random Generation
        public Expression(Random r, int level) : this(r,level,Levels[level].memberCountLimit)
        {

        }

        public Expression (Random r, int level, int memberCount)
        {
            this.level = level;
            if(memberCount == 1)
            {
                this.directValue = r.Next(Levels[level].numberLimit);
                this.isInt = true;
                return;
            }

            while(true)
            {
                int firstElementSplit = r.Next(1, memberCount - 1);
                firstElement = new Expression(r, level, firstElementSplit);
                secondElement = new Expression(r, level, memberCount - firstElementSplit);

                char[] ops = Levels[level].allowedOperations;
                operation = ops[r.Next(ops.Length)];
                //Check check
                int p = this.OperatorPriority;
                int v = this.Value;
                if((Levels[level].allowParantheses || (firstElement.OperatorPriority > p && secondElement.OperatorPriority > p)) && v >= 0 && v <= Levels[level].resultLimit)
                {
                    break;
                }
            }
            
        }

        //The higher this is, the more powerful the operator is
        public int OperatorPriority
        {
            get
            {
                if (isInt)
                {
                    return int.MaxValue;
                }

                for(int i = 0; i < operatorsByOrder.Length; i++)
                {
                    if(operatorsByOrder[i] == this.operation)
                    {
                        return i;
                    }
                }
                return -1;
            }
        }

        public int Value
        {
            get
            {
                if (isInt)
                {
                    return directValue;
                }
                return calculation[operation](firstElement.Value, secondElement.Value);
            }
        }

        public override string ToString()
        {
            if (isInt)
            {
                return directValue.ToString();
            }

            string aString;
            string bString;

            int thisPriority = this.OperatorPriority;

            if (firstElement.isInt || firstElement.OperatorPriority > thisPriority)
            {
                aString = firstElement.ToString();
            }else
            {
                aString = "(" + firstElement.ToString() + ")";
            }

            if (secondElement.isInt || secondElement.OperatorPriority > thisPriority)
            {
                bString = secondElement.ToString();
            }
            else
            {
                bString = "(" + secondElement.ToString() + ")";
            }

            return aString + operation.ToString() + bString;
        }

        private static int Add(int a, int b)
        {
            return a + b;
        }

        private static int Subtract(int a, int b)
        {
            return a - b;
        }

        private static int Multiply(int a, int b)
        {
            return a * b;
        }

        private static int Divide(int a, int b)
        {
            return a / b;
        }

        private static int Power(int a, int b)
        {
            return (int)Math.Round(Math.Pow(a, b));
        }
    }
}
