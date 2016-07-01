using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Loki.Common.Diagnostics
{
    public interface ILogFactory : IInitializable
    {
        ILog CreateLog(string logName);

        IActivityLog CreateActivityLog(string activityName);

        Task<string> GetLogDataAsync();
    }
}
