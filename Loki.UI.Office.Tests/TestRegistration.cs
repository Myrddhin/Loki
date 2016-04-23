using Loki.IoC;
using Loki.IoC.Registration;

namespace Loki.UI.Office.Tests
{
    public class TestRegistration : LokiContextInstaller
    {
        public override void Install(IObjectContext context)
        {
            context.Register(Element.For<TestViewModel>());
        }

        private static readonly TestRegistration winform = new TestRegistration();

        public static TestRegistration Test
        {
            get
            {
                return winform;
            }
        }
    }
}