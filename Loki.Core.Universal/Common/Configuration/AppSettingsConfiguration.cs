using System;
using System.Collections.Generic;
using System.Text;

namespace Loki.Common.Configuration
{
    public class AppSettingsConfiguration : IConfiguration
    {
        public T GetValue<T>(string key)
        {
            throw new NotImplementedException();
        }

        public string GetValue(string key)
        {
            return "Microsoft.Extensions.Configuration";
        }
    }
}
