using System;
using System.Threading;
using System.Threading.Tasks;
using Loki.UI;

namespace System.Threading
{
    public static class ThreadingExtensions
    {
        public static TResult WithWrite<TResult>(this ReaderWriterLockSlim rwlsLock, Func<TResult> functor)
        {
            rwlsLock.EnterWriteLock();
            try
            {
                return functor();
            }
            finally
            {
                rwlsLock.ExitWriteLock();
            }
        }

        public static void WithWrite(this ReaderWriterLockSlim rwlsLock, Action functor)
        {
            rwlsLock.EnterWriteLock();
            try
            {
                functor();
            }
            finally
            {
                rwlsLock.ExitWriteLock();
            }
        }

        public static TResult WithRead<TResult>(this ReaderWriterLockSlim rwlsLock, Func<TResult> functor)
        {
            rwlsLock.EnterReadLock();
            try
            {
                return functor();
            }
            finally
            {
                rwlsLock.ExitReadLock();
            }
        }

        public static void WithRead(this ReaderWriterLockSlim rwlsLock, Action functor)
        {
            rwlsLock.EnterReadLock();
            try
            {
                functor();
            }
            finally
            {
                rwlsLock.ExitReadLock();
            }
        }

        public static void WithReadWrite(this ReaderWriterLockSlim rwlsLock, Func<bool> discriminator, Action readAction, Action writeAction)
        {
            bool needWrite = false;
            rwlsLock.EnterUpgradeableReadLock();
            try
            {
                needWrite = discriminator();
            }
            catch
            {
                rwlsLock.ExitUpgradeableReadLock();
                throw;
            }

            if (needWrite)
            {
                // write
                rwlsLock.EnterWriteLock();
                try
                {
                    writeAction();
                }
                finally
                {
                    rwlsLock.ExitWriteLock();
                    rwlsLock.ExitUpgradeableReadLock();
                }
            }
            else
            {
                // read
                rwlsLock.EnterReadLock();
                rwlsLock.ExitUpgradeableReadLock();
                try
                {
                    readAction();
                }
                finally
                {
                    rwlsLock.ExitReadLock();
                }
            }
        }

        public static TResult WithReadWrite<TResult>(this ReaderWriterLockSlim rwlsLock, Func<bool> discriminator, Func<TResult> write, Func<TResult> read)
        {
            bool needWrite = false;
            rwlsLock.EnterUpgradeableReadLock();
            try
            {
                needWrite = discriminator();
            }
            catch
            {
                rwlsLock.ExitUpgradeableReadLock();
                throw;
            }

            if (needWrite)
            {
                // write
                rwlsLock.EnterWriteLock();
                try
                {
                    return write();
                }
                finally
                {
                    rwlsLock.ExitWriteLock();
                    rwlsLock.ExitUpgradeableReadLock();
                }
            }
            else
            {
                // read
                rwlsLock.EnterReadLock();
                rwlsLock.ExitUpgradeableReadLock();
                try
                {
                    return read();
                }
                finally
                {
                    rwlsLock.ExitReadLock();
                }
            }
        }
    }
}