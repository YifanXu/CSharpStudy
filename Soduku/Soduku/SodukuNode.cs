﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soduku
{
    class SodukuNode
    {
        public readonly int x;
        public readonly int y;

        public SodukuNode(int x, int y, List<int> possibleValues)
        {
            this.x = x;
            this.y = y;
        }
    }
}
