using System.ComponentModel;
using Loki.Common;

namespace Loki.Commands
{
    public interface ICommandAware : INotifyPropertyChanged, INotifyPropertyChanging
    {
        ICentralizedChangeTracking State { get; }
    }
}