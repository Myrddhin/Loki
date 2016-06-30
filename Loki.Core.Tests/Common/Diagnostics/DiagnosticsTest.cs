using System;

using Loki.Common.Tests;

using Xunit;

namespace Loki.Common.Diagnostics.Tests
{
    public class DiagnosticsTest : CommonTest
    {
        public IDiagnostics Component { get; set; }

        public DiagnosticsTest()
        {
            Component = Context.Resolve<IDiagnostics>();
            Component.Initialize();
        }

        #region Log Test

        [Fact]
        public void LogLevelInfoFormat()
        {
            var log = Component.GetLog("TestLog");

            ConsoleLogListener.StartCapture();

            log.InfoFormat("Hello {0}", 1);

            ConsoleLogListener.EndCapture();

            Assert.True(ConsoleLogListener.Present("INFO"));
            Assert.True(ConsoleLogListener.Present(string.Format("Hello {0}", 1)));
        }

        [Fact]
        public void LogLevelWarnFormat()
        {
            var log = Component.GetLog("TestLog");

            ConsoleLogListener.StartCapture();

            log.WarnFormat("Hello {0}", 1);

            ConsoleLogListener.EndCapture();

            Assert.True(ConsoleLogListener.Present("WARN"));
            Assert.True(ConsoleLogListener.Present(string.Format("Hello {0}", 1)));
        }

        [Fact]
        public void LogLevelErrorFormat()
        {
            var log = Component.GetLog("TestLog");

            ConsoleLogListener.StartCapture();

            log.ErrorFormat("Hello {0}", 1);

            ConsoleLogListener.EndCapture();

            Assert.True(ConsoleLogListener.Present("ERROR"));
            Assert.True(ConsoleLogListener.Present(string.Format("Hello {0}", 1)));
        }

        [Fact]
        public void LogLevelDebugFormat()
        {
            var log = Component.GetLog("TestLog");

            ConsoleLogListener.StartCapture();

            log.DebugFormat("Hello {0}", 1);

            ConsoleLogListener.EndCapture();

            Assert.True(ConsoleLogListener.Present("DEBUG"));
            Assert.True(ConsoleLogListener.Present(string.Format("Hello {0}", 1)));
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
        public void LogNameSingleton()
        {
            var log1 = Component.GetLog("TestLog");
            var log2 = Component.GetLog("TestLog");

            Assert.Same(log2, log1);
        }

        #endregion Log Test

        #region Build error test

        [Fact]
        public void BuildExceptionLogError()
        {
            ConsoleLogListener.StartCapture();

            var ex = Component.BuildError<ApplicationException>("Test Message");

            ConsoleLogListener.EndCapture();

            Assert.True(ConsoleLogListener.Present("ERROR"));
            Assert.True(ConsoleLogListener.Present(ex.Message));
        }

        [Fact]
        public void BuildExceptionLogErrorWithInner()
        {
            ConsoleLogListener.StartCapture();

            var inner = new ApplicationException("Inner message");

            var ex = Component.BuildError<ApplicationException>("Test Message", inner);

            ConsoleLogListener.EndCapture();

            Assert.True(ConsoleLogListener.Present(inner.ToString()));
        }

        #endregion Build error test

        [Fact]
        public void InitializedAtStart()
        {
            Assert.True(Component.Initialized);
        }
    }
}