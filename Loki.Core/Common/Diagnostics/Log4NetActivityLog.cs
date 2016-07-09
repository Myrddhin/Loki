using System;
using System.Diagnostics;

namespace Loki.Common.Diagnostics
{
    internal class Log4NetActivityLog : Log4NetLog, IActivityLog
    {
        private readonly Stopwatch chrono;

        private readonly Guid token;

        private readonly string name;

        public Log4NetActivityLog(string activityName) : base(activityName)
        {
            name = activityName;
            token = Guid.NewGuid();
            chrono = new Stopwatch();
            this.InfoFormat("Start activity {0} ; token {1}", activityName, token);
            chrono.Start();
        }

        public void Dispose()
        {
            chrono.Stop();
            this.InfoFormat("End activity {0} ; token {1} : elapsed {2}", name, token, chrono.Elapsed);
            
        }
    }
}