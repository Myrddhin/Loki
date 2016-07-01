using System;
using System.Globalization;

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

        #region Activity log

        [Fact(DisplayName = "Activity log")]
        public void ActivityLog()

        {
            ConsoleLogListener.StartCapture();
            using (var x = Component.GetActivityLog("Activity"))
            {
                Assert.True(ConsoleLogListener.Present("Start"));
                x.Info("Doing");
            }

            Assert.True(ConsoleLogListener.Present("End"));

            ConsoleLogListener.EndCapture();
        }

        #endregion Activity log

        #region Log Test

        [Fact(DisplayName = "Log Info format")]
        public void LogLevelInfoFormat()
        {
            var log = Component.GetLog("TestLog");

            ConsoleLogListener.StartCapture();

            log.InfoFormat("Hello {0}", 1);

            ConsoleLogListener.EndCapture();

            Assert.True(ConsoleLogListener.Present("INFO"));
            Assert.True(ConsoleLogListener.Present(string.Format("Hello {0}", 1)));
        }

        [Fact(DisplayName = "Log Warn format")]
        public void LogLevelWarnFormat()
        {
            var log = Component.GetLog("TestLog");

            ConsoleLogListener.StartCapture();

            log.WarnFormat("Hello {0}", 1);

            ConsoleLogListener.EndCapture();

            Assert.True(ConsoleLogListener.Present("WARN"));
            Assert.True(ConsoleLogListener.Present(string.Format("Hello {0}", 1)));
        }

        [Fact(DisplayName = "Log Error format")]
        public void LogLevelErrorFormat()
        {
            var log = Component.GetLog("TestLog");

            ConsoleLogListener.StartCapture();

            log.ErrorFormat("Hello {0}", 1);

            ConsoleLogListener.EndCapture();

            Assert.True(ConsoleLogListener.Present("ERROR"));
            Assert.True(ConsoleLogListener.Present(string.Format("Hello {0}", 1)));
        }

        [Fact(DisplayName = "Log Debug format")]
        public void LogLevelDebugFormat()
        {
            var log = Component.GetLog("TestLog");

            ConsoleLogListener.StartCapture();

            log.DebugFormat("Hello {0}", 1);

            ConsoleLogListener.EndCapture();

            Assert.True(ConsoleLogListener.Present("DEBUG"));
            Assert.True(ConsoleLogListener.Present(string.Format("Hello {0}", 1)));
        }

        [Fact(DisplayName = "Log Debug")]
        public void LogLevelDebug()
        {
            var log = Component.GetLog("TestLog");

            ConsoleLogListener.StartCapture();

            log.Debug("Hello");

            ConsoleLogListener.EndCapture();

            Assert.True(ConsoleLogListener.Present("DEBUG"));
        }

        [Fact(DisplayName = "Log Error")]
        public void LogLevelError()
        {
            var log = Component.GetLog("TestLog");

            ConsoleLogListener.StartCapture();

            log.Error("Hello");

            ConsoleLogListener.EndCapture();

            Assert.True(ConsoleLogListener.Present("ERROR"));
        }

        [Fact(DisplayName = "Log Info")]
        public void LogLeveInfo()
        {
            var log = Component.GetLog("TestLog");

            ConsoleLogListener.StartCapture();

            log.Info("Hello");

            ConsoleLogListener.EndCapture();

            Assert.True(ConsoleLogListener.Present("INFO"));
        }

        [Fact(DisplayName = "Log Warn")]
        public void LogLevelWarn()
        {
            var log = Component.GetLog("TestLog");

            ConsoleLogListener.StartCapture();

            log.Warn("Hello");

            ConsoleLogListener.EndCapture();

            Assert.True(ConsoleLogListener.Present("WARN"));
        }

        [Fact(DisplayName = "Log object is a singleton by name")]
        public void LogNameSingleton()
        {
            var log1 = Component.GetLog("TestLog");
            var log2 = Component.GetLog("TestLog");

            Assert.Same(log2, log1);
        }

        #endregion Log Test

        #region Build error test

        [Fact(DisplayName = "Error when the exception type has no constructor with string")]
        public void InvalidExceptionType()
        {
            Assert.Throws<ArgumentException>(() => Component.BuildError<InvalidException>("o"));
        }

        [Fact(DisplayName = "Error when the exception type has no constructor with string and exception")]
        public void InvalidExceptionTypeWithInner()
        {
            Assert.Throws<ArgumentException>(() => Component.BuildError<InvalidException>("o", new ApplicationException()));
        }

        [Fact(DisplayName = "Exception formatted message is logged with Error level")]
        public void BuildExecptionFormat()
        {
            ConsoleLogListener.StartCapture();

            var ex = Component.BuildErrorFormat<ApplicationException>("Hello {0}", 1);

            ConsoleLogListener.EndCapture();

            Assert.True(ConsoleLogListener.Present("ERROR"));
            Assert.True(ConsoleLogListener.Present(string.Format(CultureInfo.InvariantCulture, "Hello {0}", 1)));
        }

        [Fact(DisplayName = "Inner exception is logged with Error level when present (with format)")]
        public void BuildExecptionFormatWithInner()
        {
            ConsoleLogListener.StartCapture();

            var inner = new ApplicationException("Inner message");

            var ex = Component.BuildErrorFormat<ApplicationException>(inner, "Hello {0}", 1);

            ConsoleLogListener.EndCapture();

            Assert.True(ConsoleLogListener.Present(inner.ToString()));
        }

        [Fact(DisplayName = "Exception builded is of the required type")]
        public void BuildExceptionCheckType()
        {
            var ex = Component.BuildError<ApplicationException>("Test");

            Assert.IsType<ApplicationException>(ex);
        }

        [Fact(DisplayName = "Exception message is logged with Error level")]
        public void BuildExceptionLogError()
        {
            ConsoleLogListener.StartCapture();

            var ex = Component.BuildError<ApplicationException>("Test Message");

            ConsoleLogListener.EndCapture();

            Assert.True(ConsoleLogListener.Present("ERROR"));
            Assert.True(ConsoleLogListener.Present(ex.Message));
        }

        [Fact(DisplayName = "Inner exception is logged with Error level when present")]
        public void BuildExceptionLogErrorWithInner()
        {
            ConsoleLogListener.StartCapture();

            var inner = new ApplicationException("Inner message");

            var ex = Component.BuildError<ApplicationException>("Test Message", inner);

            ConsoleLogListener.EndCapture();

            Assert.True(ConsoleLogListener.Present(inner.ToString()));
        }

        #endregion Build error test

        [Fact(DisplayName = "Component is initialized when resolved")]
        public void InitializedAtStart()
        {
            Assert.True(Component.Initialized);
        }
    }
}