using System.Reflection;

namespace Loki.IoC.Registration
{
    public interface IProperty<out TService>
    {
        PropertyInfo Key { get; }

        string Name { get; }

        object Value { get; }

        bool Ignore { get; }
    }
}