using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketFinances
{
    public class TaskConnection
    {
        public int baseTask;
        public int predecessor;

        public TaskConnection()
        {
        }

        public TaskConnection(int baseTask, int predecessor)
        {
            this.baseTask = baseTask;
            this.predecessor = predecessor;
        }
    }
}
