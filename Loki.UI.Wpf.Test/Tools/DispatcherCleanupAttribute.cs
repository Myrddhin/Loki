using System;
using System.Reflection;
using System.Windows.Threading;

using Xunit.Sdk;

namespace Loki.UI
{
    /// <summary>Helful if you stumble upon 'object disconnected from RCW' ComException *after* the test suite finishes,
    /// or if you are unable to start the test because the VS test runner tells you 'Unable to start more than one local run'
    /// even if all tests seem to have finished.</summary>
    /// <remarks>Only to be used with xUnit STA worker threads.</remarks>
    [AttributeUsage(AttributeTargets.Method)]
    public class DomainNeedsDispatcherCleanupAttribute : BeforeAfterTestAttribute
    {
        public override void After(MethodInfo methodUnderTest)
        {
            Dispatcher.CurrentDispatcher.InvokeShutdown();

            base.After(methodUnderTest);
        }
    }
}