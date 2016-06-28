using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository.Hierarchy;

namespace Loki.Common.Diagnostics
{
    public class Log4NetLogFactory : ILogFactory
    {
        public ILog CreateLog(string logName)
        {
            return new Log4NetLog(logName);
        }

        public Task<string> GetLogDataAsync()
        {
            var rootAppender = ((Hierarchy)LogManager.GetRepository())
                                         .Root.Appenders.OfType<FileAppender>()
                                         .FirstOrDefault();

            var fileName= rootAppender != null ? rootAppender.File : string.Empty;

            if (!File.Exists(fileName))
            {
                return Task.FromResult(string.Empty);
            }

            using (var fs = File.OpenText(fileName))
            {
                return fs.ReadToEndAsync();
            }
        }

        public void Initialize()
        {
            XmlConfigurator.Configure();
            Initialized = true;
        }

        public bool Initialized { get; private set; }
    }
}