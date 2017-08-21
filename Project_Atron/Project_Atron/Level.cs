using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculation_Test
{
    public class Level
    {
        public char[] allowedOperations;
        public int numberLimit;
        public int resultLimit;
        public int memberCountLimit;
        public bool allowParantheses;

        public Level()
        {

        }

        public Level(int numberLimit, int resultLimit, int memberCountLimit, bool allowParantheses, params char[] allowedOperations)
        {
            this.numberLimit = numberLimit;
            this.resultLimit = resultLimit;
            this.memberCountLimit = memberCountLimit;
            this.allowParantheses = allowParantheses;
            this.allowedOperations = allowedOperations;
        }
    }
}
