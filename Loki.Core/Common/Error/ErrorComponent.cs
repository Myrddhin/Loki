using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using Loki.Core.Resources;

namespace Loki.Common
{
    internal class ErrorService : LoggableObject, IErrorComponent
    {
        private ConcurrentDictionary<Type, Func<string, Exception, Exception>> fullTypeBuilders = new ConcurrentDictionary<Type, Func<string, Exception, Exception>>();
        private ConcurrentDictionary<Type, Func<string, Exception>> stringTypeBuilders = new ConcurrentDictionary<Type, Func<string, Exception>>();

        public T BuildError<T>(string message, ILog log, Exception innerException) where T : Exception
        {
            // log exception
            if (log != null)
            {
                log.Error(message, innerException);
            }
            else
            {
                Log.Error(message, innerException);
            }

            Type exceptionType = typeof(T);

            // build and launch
            if (innerException == null)
            {
                // check cache
                BuildTypeConstructorWithString(exceptionType);

                return stringTypeBuilders[exceptionType](message) as T;
            }
            else
            {
                // check cache
                BuildTypeConstructorWithStringAndException(exceptionType);

                return fullTypeBuilders[exceptionType](message, innerException) as T;
            }
        }

        public T BuildError<T>(string message, ILog log) where T : Exception
        {
            return BuildError<T>(message, log, null);
        }

        public T BuildErrorFormat<T>(string message, ILog log, Exception innerException, params object[] parameters) where T : Exception
        {
            // log exception
            if (log != null)
            {
                log.ErrorFormat(message, innerException, parameters);
            }
            else
            {
                Log.ErrorFormat(message, innerException, parameters);
            }

            Type exceptionType = typeof(T);

            // build and launch
            if (innerException == null)
            {
                // check cache
                BuildTypeConstructorWithString(exceptionType);

                return stringTypeBuilders[exceptionType](string.Format(CultureInfo.InvariantCulture, message, parameters)) as T;
            }
            else
            {
                // check cache
                BuildTypeConstructorWithStringAndException(exceptionType);

                return fullTypeBuilders[exceptionType](string.Format(CultureInfo.InvariantCulture, message, parameters), innerException) as T;
            }
        }

        public T BuildErrorFormat<T>(string message, ILog log, params object[] parameters) where T : Exception
        {
            return BuildErrorFormat<T>(message, log, null, parameters);
        }

        public void LogError(string message, ILog log, Exception innerException, params object[] parameters)
        {
            if (log != null)
            {
                log.ErrorFormat(message, innerException);
            }
            else
            {
                Log.ErrorFormat(message, innerException);
            }
        }

        private void BuildTypeConstructorWithString(Type exceptionType)
        {
            if (!stringTypeBuilders.ContainsKey(exceptionType))
            {
                ConstructorInfo info = exceptionType.GetConstructor(new Type[] { typeof(string) });
                if (info != null)
                {
                    ParameterExpression messageExpression = Expression.Parameter(typeof(string));
                    Func<string, Exception> method = Expression.Lambda<Func<string, Exception>>(Expression.New(info, messageExpression), messageExpression).Compile();
                    stringTypeBuilders.TryAdd(exceptionType, method);
                }
                else
                {
                    throw BuildErrorFormat<ArgumentException>(Errors.Error_InvalidExceptionType, Log, exceptionType);
                }
            }
        }

        private void BuildTypeConstructorWithStringAndException(Type exceptionType)
        {
            if (!fullTypeBuilders.ContainsKey(exceptionType))
            {
                ConstructorInfo info = exceptionType.GetConstructor(new Type[] { typeof(string), typeof(Exception) });
                if (info != null)
                {
                    ParameterExpression messageExpression = Expression.Parameter(typeof(string));
                    ParameterExpression exceptionExpression = Expression.Parameter(typeof(Exception));
                    Func<string, Exception, Exception> method = Expression.Lambda<Func<string, Exception, Exception>>(Expression.New(info, messageExpression, exceptionExpression), messageExpression, exceptionExpression).Compile();
                    fullTypeBuilders.TryAdd(exceptionType, method);
                }
                else
                {
                    throw BuildErrorFormat<ArgumentException>(Errors.Error_InvalidExceptionType, Log, exceptionType);
                }
            }
        }
    }
}