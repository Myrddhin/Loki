using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loki.UI
{
    public class ConsoleSignalManager : ISignalManager
    {
        public void ApplicationExit(int returnCode)
        {
            Environment.Exit(returnCode);
        }

        public void Error(Exception exception, bool imperative)
        {
            Console.Error.WriteLine(exception);
            if (imperative)
            {
                ApplicationExit(-1);
            }
        }

        public void Message(string message)
        {
            Console.WriteLine(message);
        }

        public void Warning(string warning)
        {
            Console.WriteLine(warning);
        }
    }
}