using Loki.Common.Diagnostics;
using Loki.Core.Tests.Common;
using Loki.Core.Tests.Common.Diagnostics;

using Xunit;

namespace Loki.Common.Diagnosticts.Tests
{
    public class DiagnosticsTest : CommonTest
    {
        public IDiagnostics Component { get; set; }

        public DiagnosticsTest()
        {
            Component = Context.Resolve<IDiagnostics>();
            Component.Initialize();
        }

        [Fact]
        public void LogLevelDebug()
        {
            var log = Component.GetLog("TestLog");

            ConsoleLogListener.StartCapture();

            log.Debug("Hello");

            ConsoleLogListener.EndCapture();

            Assert.True(ConsoleLogListener.Present("DEBUG"));
        }

        [Fact]
        public void LogLevelError()
        {
            var log = Component.GetLog("TestLog");

            ConsoleLogListener.StartCapture();

            log.Error("Hello");

            ConsoleLogListener.EndCapture();

            Assert.True(ConsoleLogListener.Present("ERROR"));
        }

        [Fact]
        public void LogLeveInfo()
        {
            var log = Component.GetLog("TestLog");

            ConsoleLogListener.StartCapture();

            log.Info("Hello");

            ConsoleLogListener.EndCapture();

            Assert.True(ConsoleLogListener.Present("INFO"));
        }

        [Fact]
        public void LogLevelWarn()
        {
            var log = Component.GetLog("TestLog");

            ConsoleLogListener.StartCapture();

            log.Warn("Hello");

            ConsoleLogListener.EndCapture();

            Assert.True(ConsoleLogListener.Present("WARN"));
        }

        [Fact]
        public void InitializedAtStart()
        {
            Assert.True(Component.Initialized);
            //var log = Component.GetLog("TestLog");

            //ConsoleLogListener.StartCapture();

            //log.Debug("Hello");

            //ConsoleLogListener.EndCapture();

            //Assert.True(ConsoleLogListener.Present("TestLog"));
            //Assert.True(ConsoleLogListener.Present("Hello"));
            //Assert.True(ConsoleLogListener.Present("DEBUG"));
        }
    }
}