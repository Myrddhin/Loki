namespace Loki.UI
{
    public interface IScreenFactory
    {
        T CreateScreen<T>() where T : Screen;

        void Release(object screenToRelease);
    }
}