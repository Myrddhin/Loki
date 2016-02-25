using Loki.UI.Commands;

namespace Loki.UI
{
    public interface ICommandElement
    {
        ICommand Command { get; }

        object Parameter { get; }
    }
}