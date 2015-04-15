using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loki.UI.Tasks
{
    public class TaskCompletedEventArgs : EventArgs
    {
        public TaskRun Item { get; set; }

        public TaskCompletedEventArgs(TaskRun item)
        {
            Item = item;
        }
    }
}