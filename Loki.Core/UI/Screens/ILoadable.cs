namespace Loki.UI
{
    public interface ILoadable
    {
        bool IsLoaded { get; }

        void Load();
    }
}