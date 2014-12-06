namespace Loki.UI
{
    public interface IInitializable
    {
        bool IsInitialized { get; }

        void Initialize();
    }
}