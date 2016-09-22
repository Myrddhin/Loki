using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Loki.Common.Diagnostics;

namespace Loki.Common
{
    public class TestDiagnostics : IDiagnostics
    {
        public bool Initialized
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public T BuildError<T>(string message) where T : Exception
        {
            throw new NotImplementedException();
        }

        public T BuildError<T>(string message, Exception innerException) where T : Exception
        {
            throw new NotImplementedException();
        }

        public T BuildErrorFormat<T>(string message, params object[] parameters) where T : Exception
        {
            throw new NotImplementedException();
        }

        public T BuildErrorFormat<T>(Exception innerException, string message, params object[] parameters) where T : Exception
        {
            throw new NotImplementedException();
        }

        public IActivityLog GetActivityLog(string logName)
        {
            throw new NotImplementedException();
        }

        public ILog GetLog(string logName)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetLogDataAsync()
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
