using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository.Hierarchy;

namespace Loki.Common
{
    /// <summary>
    /// Log4Net logger implementation.
    /// </summary>
    internal class Log4NetLogger : ILoggerComponent
    {
        public string LogFileName
        {
            get
            {
                var rootAppender = ((Hierarchy)LogManager.GetRepository())
                                         .Root.Appenders.OfType<FileAppender>()
                                         .FirstOrDefault();

                return rootAppender != null ? rootAppender.File : string.Empty;
            }
        }

        private bool configured = false;

        private ConcurrentDictionary<string, ILog> store = new ConcurrentDictionary<string, ILog>();

        public void Configure()
        {
            XmlConfigurator.Configure();
            configured = true;
        }

        public void Configure(string configurationFile)
        {
            XmlConfigurator.Configure(new FileInfo(configurationFile));
            configured = true;
        }

        public void Configure(Stream configurationStream)
        {
            XmlConfigurator.Configure(configurationStream);
            configured = true;
        }

        public ILog GetLog(string logName)
        {
            if (!configured)
            {
                Configure();
            }

            if (!store.ContainsKey(logName))
            {
                return store.AddOrUpdate(logName, new Log4NetLog(logName), (x, l) => l);
            }
            else
            {
                return store[logName];
            }
        }
    }
}