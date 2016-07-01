using System;
using System.Threading.Tasks;

namespace Loki.Common.Diagnostics
{
    public class UWPLogFactory : ILogFactory
    {
        public void Initialize()
        {
            Initialized = true;
        }

        public bool Initialized { get; private set; }

        public ILog CreateLog(string logName)
        {
            throw new NotImplementedException();
        }

        public IActivityLog CreateActivityLog(string activityName)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetLogDataAsync()
        {
            throw new NotImplementedException();
        }
    }
}