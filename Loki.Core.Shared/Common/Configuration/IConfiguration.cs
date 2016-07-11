namespace Loki.Common.Configuration
{
    public interface IConfiguration
    {
        T GetValue<T>(string key);

        string GetValue(string key);
    }
}