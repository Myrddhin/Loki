using System;
using System.IO;
using System.Text;

namespace Loki.Core.Tests.Common.Diagnostics
{
    public static class ConsoleLogListener
    {
        private static readonly StringBuilder InternalBuffer;

        private static TextWriter stdOut;

        private static TextWriter internalOut;

        static ConsoleLogListener()
        {
            InternalBuffer = new StringBuilder();
        }

        public static void StartCapture()
        {
            internalOut = new StringWriter(InternalBuffer);
            stdOut = Console.Out;

            Console.SetOut(internalOut);
        }

        public static bool Present(string token)
        {
            return InternalBuffer.ToString().Contains(token);
        }

        public static void EndCapture()
        {
            Console.SetOut(stdOut);

            internalOut.Dispose();
        }
    }
}