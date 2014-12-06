using Loki.Common;

namespace Loki.UI
{
    /// <summary>
    /// Denotes an instance which implements <see cref="IHaveDisplayName"/>, <see cref="IActivate"/>,
    /// <see cref="IDeactivate"/>, <see cref="IGuardClose"/> and <see cref="INotifyPropertyChangedEx"/>.
    /// </summary>
    public interface IScreen : IHaveDisplayName, IActivable, IDesactivable, IGuardClose, INotifyPropertyChangedEx, IInitializable, ILoadable
    {
        ICentralizedChangeTracking State { get; }
    }
}