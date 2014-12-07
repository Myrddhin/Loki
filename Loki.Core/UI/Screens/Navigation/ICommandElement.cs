using System.Windows.Input;

namespace Loki.UI
{
    public interface ICommandElement
    {
        ICommand Command { get; }
    }
}