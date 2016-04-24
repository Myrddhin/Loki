using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

using Loki.Core.Resources;

namespace Loki.Common
{
    internal class ErrorService : IErrorComponent
    {
        private readonly ILog log;

        public ErrorService(ILoggerComponent logManager)
        {
            log = logManager.GetLog(nameof(ErrorService));
        }

        private readonly ConcurrentDictionary<Type, Func<string, Exception, Exception>> fullTypeBuilders = new ConcurrentDictionary<Type, Func<string, Exception, Exception>>();
        private readonly ConcurrentDictionary<Type, Func<string, Exception>> stringTypeBuilders = new ConcurrentDictionary<Type, Func<string, Exception>>();

        public T BuildError<T>(string message, Exception innerException) where T : Exception
        {
            // log exception
            log.Error(message, innerException);

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

        public T BuildError<T>(string message) where T : Exception
        {
            return BuildError<T>(message, null);
        }

        public T BuildErrorFormat<T>(Exception innerException, string message, params object[] parameters) where T : Exception
        {
            // log exception
            log.Error(string.Format(CultureInfo.InvariantCulture, message, parameters), innerException);

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

        public T BuildErrorFormat<T>(string message, params object[] parameters) where T : Exception
        {
            return BuildErrorFormat<T>(null, message, parameters);
        }

        public void LogError(string message, Exception innerException, params object[] parameters)
        {
            log.Error(message, innerException);
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
                    throw BuildErrorFormat<ArgumentException>(Errors.Error_InvalidExceptionType, log, exceptionType);
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
                    throw BuildErrorFormat<ArgumentException>(Errors.Error_InvalidExceptionType, log, exceptionType);
                }
            }
        }
    }
}