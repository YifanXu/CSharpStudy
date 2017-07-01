using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Soduku
{
    class Soduku
    {
        public int size = 9;
        private int[,] values;

        public Soduku()
        {
            values = new int[9, 9];
        }

        public Soduku(string path)
        {
            if (!File.Exists(path))
            {
                return;
            }
            string[] input = File.ReadAllLines(path);
            values = new int[9, 9];
            for (int line = 0; line < input.Length; line++)
            {
                for (int i = 0; i < input[line].Length; i++)
                {
                    if (!int.TryParse(input[line][i].ToString(), out values[i, line])) {
                        continue;
                    }
                }
            }
        }

        public void Display()
        {
            Console.SetCursorPosition(0, 0);
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (values[x, y] == 0)
                    {
                        Console.Write("X");
                    }
                    else
                    {
                        Console.Write(values[x, y]);
                    }
                }
                Console.WriteLine();
            }
        }

        public int[,] getSection (int x, int y)
        {
            int xSection = x / 3;
            int ySection = y / 3;
            int[,] section = new int[3, 3];
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    section[i, j] = values[xSection * 3 + i, ySection * 3 + j];
                }
            }

            return section;
        }

        public List<int> GetPossibleValues (int x, int y) {
            List<int> possibleInts = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            for(int i = 0; i < size; i++)
            {
                //Cardinal Directions
                if (possibleInts.Contains(values[x, i]))
                {
                    possibleInts.Remove(values[x, i]);
                }
                if (possibleInts.Contains(values[y, i]))
                {
                    possibleInts.Remove(values[x, i]);
                }
            }
            //Check Section
            foreach(int value in getSection(x, y))
            {
                if (possibleInts.Contains(value))
                {
                    possibleInts.Remove(value);
                }
            }

            return possibleInts;
        }
        
        public int this[int x, int y]
        {
            get
            {
                return values[x, y];
            }
            set
            {
                values[x, y] = value;
            }
        }

        public bool TrySolve()
        {
            bool bruteForceSuccess = true;
            while (bruteForceSuccess)
            {
                bool TrySuccess = true;
                while (TrySuccess)
                {
                    bool solved = true;
                    TrySuccess = false;
                    for (int x = 0; x < size; x++)
                    {
                        for (int y = 0; y < size; y++)
                        {
                            if (values[x, y] == 0)
                            {
                                solved = false;
                                List<int> possibilities = GetPossibleValues(x, y);
                                if (possibilities.Count == 0)
                                {
                                    return false;
                                }
                                if (possibilities.Count == 1)
                                {
                                    this[x, y] = possibilities[0];
                                    TrySuccess = true;
                                }
                            }
                        }
                    }
                    if (solved)
                    {
                        return true;
                    }
                }
                bruteForceSuccess = SolveByBruteForce();
            }
            return false;
        }

        private bool SolveByBruteForce()
        {
            List<SodukuNode> mysteryNodes = new List<SodukuNode>();
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (values[x, y] == 0)
                    {
                        mysteryNodes.Add(new SodukuNode(x, y, GetPossibleValues(x, y)));
                    }
                }
            }
            foreach(SodukuNode node in mysteryNodes)
            {
                for(int i = 0; i < node.possibleValues.Count; i++)
                {
                    this[node.x, node.y] = node.possibleValues[i];
                    if (TrySolve())
                    {
                        this[node.x, node.y] = node.possibleValues[i];
                        return true;
                    }
                    this[node.x, node.y] = 0;
                }
            }
            return false;
        }
    }
}
