using System.Collections.Generic;
using System.Linq;

using Xunit.Abstractions;
using Xunit.Sdk;

namespace Loki.UI
{
    public class WpfFactDiscoverer : IXunitTestCaseDiscoverer
    {
        private readonly FactDiscoverer factDiscoverer;

        public WpfFactDiscoverer(IMessageSink diagnosticMessageSink)
        {
            factDiscoverer = new FactDiscoverer(diagnosticMessageSink);
        }

        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            return factDiscoverer.Discover(discoveryOptions, testMethod, factAttribute)
                                 .Select(testCase => new WpfTestCase(testCase));
        }
    }
}