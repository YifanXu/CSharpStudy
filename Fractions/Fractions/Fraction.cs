using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fractions
{
    public class Fraction
    {
        public int numerator;
        public int denominator;

        public Fraction(int numerator, int denominator)
        {
            this.numerator = numerator;
            this.denominator = denominator;
        }

        public Fraction(int value) : this(value, 1)
        {

        }

        public bool IsNegative
        {
            get
            {
                return (numerator / denominator < 0);
            }
        }

        public int ApproxInt
        {
            get
            {
                return numerator / denominator;
            }
        }

        public double ApproxDecimal
        {
            get
            {
                return (double) numerator / denominator;
            }
        }

        public Fraction Inverse
        {
            get
            {
                return new Fraction(denominator, numerator);
            }
        }
        
        public void ScaleBy(int factor)
        {
            this.denominator = denominator * factor;
            this.numerator = numerator * factor;
        }

        public void Simplify()
        {

            int factor = GetGCD(numerator, denominator);
            numerator = numerator / factor;
            denominator = denominator / factor;

            if(denominator < 0)
            {
                denominator = denominator * -1;
                numerator = numerator * -1;
            }
        }

        public void InvertSign()
        {
            numerator = numerator * -1;
            this.Simplify();
        }

        public static void RaiseToSameDenominator (Fraction a, Fraction b)
        {
            a.Simplify();
            b.Simplify();

            int commonDenominator = GetLCM(a.denominator, b.denominator);
            a.ScaleBy(commonDenominator / a.denominator);
            b.ScaleBy(commonDenominator / b.denominator);
        }

        //TOSTRING
        public override string ToString()
        {
            return String.Format("{0}/{1}",this.numerator, this.denominator);
        }

        //EQUALITY
        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Fraction))
            {
                Fraction a = (Fraction)obj;
                Fraction b = this;
                a.Simplify();
                b.Simplify();
                return (a.denominator == b.denominator && b.numerator == a.numerator);
            }
            if (obj.GetType() == typeof(int))
            {
                int a = (int)obj;
                Fraction b = this;
                b.Simplify();
                return (a == b.numerator && b.denominator == 1);
            }
            if (obj.GetType() == typeof(double))
            {
                double a = (double)obj;
                return (a == this.ApproxDecimal);
            }
            return base.Equals(obj);
        }
        public static bool operator =(Fraction a, object b)
        {
            return a.Equals(b);
        }

        //ADDITIONS
        public static Fraction operator +(Fraction a, int b)
        {
            return new Fraction(b * a.denominator + a.numerator, a.denominator);
        }

        public static Fraction operator +(int a, Fraction b)
        {
            return new Fraction(a * b.denominator + b.numerator, b.denominator);
        }

        public static Fraction operator +(Fraction a, Fraction b)
        {
            RaiseToSameDenominator(a, b);
            Fraction result = new Fraction(a.numerator + b.numerator, a.denominator);
            return result;
        }

        //SUBTRACTIONS
        public static Fraction operator -(Fraction a, int b)
        {
            return new Fraction(a.numerator - b * a.numerator, a.denominator);
        }

        public static Fraction operator -(int a, Fraction b)
        {
            return new Fraction(a * b.denominator - b.numerator, b.denominator);
        }

        public static Fraction operator -(Fraction a, Fraction b)
        {
            RaiseToSameDenominator(a, b);
            Fraction result = new Fraction(a.numerator - b.numerator, a.denominator);
            return result;
        }

        //MULITPLICATIONS
        public static Fraction operator *(Fraction a, int b)
        {
            return new Fraction(b * a.numerator, a.denominator);
        }

        public static Fraction operator *(int a, Fraction b)
        {
            return new Fraction(a * b.denominator, b.denominator);
        }

        public static Fraction operator *(Fraction a, Fraction b)
        {
            Fraction result = new Fraction(a.numerator * b.numerator, a.denominator * b.denominator);
            result.Simplify();
            return result;
        }

        //DIVISIONS
        public static Fraction operator /(Fraction a, int b)
        {
            Fraction result = new Fraction(a.numerator, a.denominator * b);
            result.Simplify();
            return result;
        }

        public static Fraction operator /(int a, Fraction b)
        {
            Fraction result = new Fraction(a* b.denominator, b.numerator);
            result.Simplify();
            return result;
        }

        public static Fraction operator /(Fraction a, Fraction b)
        {
            Fraction result = new Fraction(a.numerator * b.denominator, a.denominator * b.numerator);
            result.Simplify();
            return result;
        }

        //GREATERTHAN
        public static bool operator >(Fraction a, int b)
        {
            return a.numerator > a.denominator * b;
        }

        public static bool operator >(int a, Fraction b)
        {
            return b.numerator > b.denominator * a;
        }

        public static bool operator >(Fraction a, Fraction b)
        {
            return a.ApproxDecimal > b.ApproxDecimal;
        }

        //SMALLERTHAN
        public static bool operator <(Fraction a, int b)
        {
            return a.numerator < a.denominator * b;
        }

        public static bool operator <(int a, Fraction b)
        {
            return b.numerator < b.denominator * a;
        }

        public static bool operator <(Fraction a, Fraction b)
        {
            return a.ApproxDecimal < b.ApproxDecimal;
        }

        //Greatest Common Divider
        public static int GetGCD (int a, int b)
        {
            int newA = Math.Abs(a);
            int newB = Math.Abs(b);
            int range = Math.Min(newA, newB);

            int GCD = 0;

            for(int i = 1; i <= range; i++)
            {
                if(newA % i ==0 && newB % i == 0)
                {
                    GCD = i;
                }
            }

            if(a < 0 && b < 0)
            {
                GCD = GCD * -1;
            }

            return GCD;
        }

        //Least Common Mulitplier
        public static int GetLCM (int a, int b)
        {
            return a / GetGCD(a, b) * b;
        }
    }
}
