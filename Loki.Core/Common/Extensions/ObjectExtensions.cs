namespace System
{
    public static class ObjectExtensions
    {
        public static void SafeDispose(this object potentialDisposable)
        {
            var disposable = potentialDisposable as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        public static string SafeToString(this object potentialString)
        {
            return potentialString == null ? string.Empty : potentialString.ToString();
        }
    }
}