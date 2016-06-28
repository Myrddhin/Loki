namespace Loki.Common
{
    public interface IInitializable
    {
        void Initialize();

        bool Initialized { get; }
    }
}