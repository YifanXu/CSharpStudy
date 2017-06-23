using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketFinances
{
    public class TaskPackage
    {
        public int rootTaskPosition;
        public int finalTaskPosition;
        public Task[] tasks;
        public TaskConnection[] connections;

        public TaskPackage()
        {

        }

        public Task this[int id]
        {
            get
            {
                return tasks[id];
            }
        }
    }
}
