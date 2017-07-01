using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fractions
{
    class Fraction
    {
        public int numerator;
        public int denominator;

        public Fraction(int numerator, int denominator)
        {
            this.numerator = numerator;
            this.denominator = denominator;
        }

        public int approxInt
        {
            get
            {
                return numerator / denominator;
            }
        }

        public double approxDecimal
        {
            get
            {
                return (double) numerator / denominator;
            }
        }

        public static Fraction operator +(Fraction a, int b)
        {
            return new Fraction(b * a.denominator + a.numerator, a.denominator);
        }

        public static Fraction operator +(Fraction a, Fraction b)
        {
            
        }

        public new Frac

        private int GetGCD (int a, int b)
        {
            List<int> aFactors = GetAllPrimedFactors(a);
            List<int> bFactors = GetAllPrimedFactors(b);

            
        }

        private List<int> GetAllPrimedFactors(int a)
        {
            List<int> factors = new List<int>();
            int i = 0;
            while (a != 1 || a != -1)
            {
                if(a % i == 0)
                {
                    factors.Add(i);
                    a = a / i;
                }
                else
                {
                    i++;
                }
            }
        }
    }
}
