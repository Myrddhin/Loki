using System.IO;

namespace Loki.Common
{
    /// <summary>
    /// Logger manager abtraction.
    /// </summary>
    public interface ILoggerComponent
    {
        void Configure();

        void Configure(string configurationFile);

        void Configure(Stream configurationStream);

        ILog GetLog(string logName);

        string LogFileName { get; }
    }
}