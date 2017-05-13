using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflector_Calculator
{
    class NicknameAttribute : Attribute
    {
        public string[] names;
        public NicknameAttribute(params string[] names)
        {
            this.names = names;
        }
    }

    class Calculator
    {

        [Nickname("+","plus")]
        public int add (int num1, int num2)
        {
            return num1 + num2;
        }

        [Nickname("-","minus")]
        public int subtract (int num1, int num2)
        {
            return num1 - num2;
        }

        [Nickname("*","x","times")]
        public int multiply (int num1, int num2)
        {
            return num1 * num2;
        }

        [Nickname("/","dividedby")]
        public int divide (int num1, int num2)
        {
            if(num2 == 0)
            {
                Console.WriteLine("SYSTEM: -Cannot divide by 0");
                return 0;
            }
            return num1 / num2;
        }

        [Nickname("^", "")]
        public int power (int basee, int power)
        {
            if(basee == 0)
            {
                return 0;
            }
            if(power == 0)
            {
                return 1;
            }
            int num = basee;
            int pow = power;
            if (power < 0)
            {
                pow = -1 * power;
            }
            for(int i = 0; i < pow; i++)
            {
                num = num * basee;
            }
            return num;
        }

    }
}
