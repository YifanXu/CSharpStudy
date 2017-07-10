using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Atron
{
    public class Question
    {
        private static Dictionary<char, Func<int,int,int>> calculation = new Dictionary<char,Func<int,int,int>>{
            {'^', Power },
            {'*', Multiply },
            {'/', Divide },
            {'+', Add },
            {'-', Subtract }
        };
        private static Dictionary<int, char[]> allowedOperations = new Dictionary<int, char[]>
        {
            {1, new char[] {'+', '-'} },
            {2, new char[] {'+', '-'} },
            {3, new char[] {'+', '-'} },
            {4, new char[] {'*', '+', '-'} },
            {5, new char[] {'*', '+', '-'} },
        };
        private static Dictionary<int, int> numberLimit = new Dictionary<int, int>
        {
            {1, 10 },
            {2, 50 },
            {3, 100 },
            {4, 100 },
            {5, 10 }
        };
        private static Dictionary<int, int> resultLimit = new Dictionary<int, int>
        {
            {1, 10 },
            {2, 50 },
            {3, 100 },
            {4, 100 },
            {5, 10 }
        };
        private static Dictionary<int, int> memberCountLimit = new Dictionary<int, int>
        {
            {1, 2 },
            {2, 2 },
            {3, 3 },
            {4, 3 },
            {5, 3 }
        };
        public int[] numbers;
        public char[] operations;
        public string displayer;
        public int level;

        public Question (int level, Random r)
        {
            this.level = level;

            do
            {
                int memberCount = memberCountLimit[level];

                //SUMMON THE NUMBERS!
                numbers = new int[memberCount];
                for (int i = 0; i < memberCount; i++)
                {
                    numbers[i] = r.Next(numberLimit[level]);
                }

                //SUMMON THE OPERATIONS!
                operations = new char[memberCount - 1];
                char[] ops = allowedOperations[level];
                for (int i = 0; i < memberCount - 1; i++)
                {
                    operations[i] = ops[r.Next(ops.Length)];
                }
            } while (this.Answer > resultLimit[level] || this.Answer < 0);

            BuildString();
        }

        public int Answer
        {
            get
            {
                List<int> nums = new List<int>(numbers.Length);
                nums.AddRange(numbers);
                List<char> ops = new List<char>(operations.Length);
                ops.AddRange(operations);
                return Calculate(nums, ops);
            }
        }

        private static int Calculate (List<int>nums, List<char> ops)
        {
            foreach (KeyValuePair<char, Func<int, int, int>> pair in calculation)
            {
                bool operationExists = true;
                while (operationExists)
                {
                    operationExists = false;
                    for (int i = 0; i < ops.Count; i++)
                    {
                        if (ops[i] == pair.Key)
                        {
                            operationExists = true;
                            int result = pair.Value(nums[i], nums[i + 1]);
                            nums[i] = result;
                            nums.RemoveAt(i + 1);
                            ops.RemoveAt(i);
                            break;
                        }
                    }
                }

            }
            if (nums.Count > 1)
            {
                throw new Exception("The calculation failed.");
            }
            return nums[0];
        }

        private static int Add (int a, int b)
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
            return (int) Math.Round(Math.Pow(a, b));
        }

        private void BuildString()
        {
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < operations.Length; i++){
                s.Append(numbers[i]);
                s.Append(operations[i]);
            }
            s.Append(numbers[operations.Length]);
            displayer = s.ToString();
        }
    }
}
