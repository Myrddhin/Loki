using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

using Loki.Common.Resources;

namespace Loki.Common.Diagnostics
{
    internal partial class Diagnostics : IDiagnostics
    {
        private readonly Lazy<ILogFactory> factory = new Lazy<ILogFactory>(LogFactory);

        private readonly ConcurrentDictionary<string, ILog> logCache = new ConcurrentDictionary<string, ILog>();

        private ILog internalLog;

        public void Initialize()
        {
            factory.Value.Initialize();
            internalLog = factory.Value.CreateLog(typeof(Diagnostics).Name);
        }

        public Task<string> GetLogDataAsync()
        {
            return factory.Value.GetLogDataAsync();
        }

        public bool Initialized => factory.Value.Initialized;

        public ILog GetLog(string logName)
        {
            if (logCache.ContainsKey(logName))
            {
                return logCache[logName];
            }

            return this.logCache.AddOrUpdate(logName, this.factory.Value.CreateLog(logName), (x, l) => l);
        }

        public IActivityLog GetActivityLog(string logName)
        {
            return factory.Value.CreateActivityLog(logName);
        }

        private readonly ConcurrentDictionary<Type, Func<string, Exception, Exception>> fullTypeBuilders = new ConcurrentDictionary<Type, Func<string, Exception, Exception>>();

        private readonly ConcurrentDictionary<Type, Func<string, Exception>> stringTypeBuilders = new ConcurrentDictionary<Type, Func<string, Exception>>();

        public T BuildError<T>(string message, Exception innerException)
            where T : Exception
        {
            // log exception
            internalLog.Error(message, innerException);

            Type exceptionType = typeof(T);

            // build and launch
            if (innerException == null)
            {
                // check cache
                BuildTypeConstructorWithString(exceptionType);

                return stringTypeBuilders[exceptionType](message) as T;
            }

            // check cache
            BuildTypeConstructorWithStringAndException(exceptionType);

            return fullTypeBuilders[exceptionType](message, innerException) as T;
        }

        public T BuildError<T>(string message)
            where T : Exception
        {
            return BuildError<T>(message, null);
        }

        public T BuildErrorFormat<T>(Exception innerException, string message, params object[] parameters)
            where T : Exception
        {
            // log exception
            internalLog.Error(string.Format(CultureInfo.InvariantCulture, message, parameters), innerException);

            Type exceptionType = typeof(T);

            // build and launch
            if (innerException == null)
            {
                // check cache
                BuildTypeConstructorWithString(exceptionType);

                return stringTypeBuilders[exceptionType](string.Format(CultureInfo.InvariantCulture, message, parameters)) as T;
            }

            // check cache
            BuildTypeConstructorWithStringAndException(exceptionType);

            return fullTypeBuilders[exceptionType](string.Format(CultureInfo.InvariantCulture, message, parameters), innerException) as T;
        }

        public T BuildErrorFormat<T>(string message, params object[] parameters)
            where T : Exception
        {
            return BuildErrorFormat<T>(null, message, parameters);
        }

        private void BuildTypeConstructorWithString(Type exceptionType)
        {
            if (!stringTypeBuilders.ContainsKey(exceptionType))
            {
                ConstructorInfo info = exceptionType.GetConstructor(new[] { typeof(string) });
                if (info != null)
                {
                    ParameterExpression messageExpression = Expression.Parameter(typeof(string));
                    Func<string, Exception> method = Expression.Lambda<Func<string, Exception>>(Expression.New(info, messageExpression), messageExpression).Compile();
                    stringTypeBuilders.TryAdd(exceptionType, method);
                }
                else
                {
                    throw BuildErrorFormat<ArgumentException>(Errors.Error_InvalidExceptionType, internalLog, exceptionType);
                }
            }
        }

        private void BuildTypeConstructorWithStringAndException(Type exceptionType)
        {
            if (!fullTypeBuilders.ContainsKey(exceptionType))
            {
                ConstructorInfo info = exceptionType.GetConstructor(new[] { typeof(string), typeof(Exception) });
                if (info != null)
                {
                    ParameterExpression messageExpression = Expression.Parameter(typeof(string));
                    ParameterExpression exceptionExpression = Expression.Parameter(typeof(Exception));
                    Func<string, Exception, Exception> method = Expression.Lambda<Func<string, Exception, Exception>>(Expression.New(info, messageExpression, exceptionExpression), messageExpression, exceptionExpression).Compile();
                    fullTypeBuilders.TryAdd(exceptionType, method);
                }
                else
                {
                    throw BuildErrorFormat<ArgumentException>(Errors.Error_InvalidExceptionType, internalLog, exceptionType);
                }
            }
        }
    }
}