using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Loki.Common.Configuration;

namespace Loki.Common
{
    public class TestConfiguration : IConfiguration
    {
        public string GetValue(string key)
        {
            throw new NotImplementedException();
        }

        public T GetValue<T>(string key)
        {
            throw new NotImplementedException();
        }
    }
}
