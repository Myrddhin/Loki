namespace Loki.UI.Models
{
    public interface ILoadable
    {
        bool IsLoaded { get; }

        void Load();
    }
}