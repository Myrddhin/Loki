namespace Loki.UI.Test
{
    public interface IScreenFactory
    {
        T Create<T>() where T : Screen;

        void Release(Screen screen);
    }
}