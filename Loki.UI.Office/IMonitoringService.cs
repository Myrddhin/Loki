using System;
using System.Security.Principal;

namespace Loki.UI.Office
{
    public interface IMonitoringService
    {
        void Initialize(Type mainType);

        string ApplicationName { get; }

        string ApplicationVersion { get; }

        string ApplicationFullVersion { get; }

        string Copyright { get; }

        string LogFileName { get; }

        void NotifySupport(Exception error);

        void NotifySupport(string message);

        IIdentity User { get; }
    }
}