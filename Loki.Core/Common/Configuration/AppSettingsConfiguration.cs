using System.ComponentModel;
using System.Configuration;

namespace Loki.Common.Configuration
{
    public class AppSettingsConfiguration : IConfiguration
    {
        public T GetValue<T>(string key)
        {
            var type = typeof(T);
            var converter = TypeDescriptor.GetConverter(type);
            return (T)converter.ConvertFromString(GetValue(key));
        }

        public string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}