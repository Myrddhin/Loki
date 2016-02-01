namespace Loki.UI.Wpf
{
    public class WpfBootStrapper<TMainViewModelType> : WpfSplashBootStrapper<TMainViewModelType, DefaultSplashModel>
        where TMainViewModelType : class, IScreen
    {
        public WpfBootStrapper() : base(new SplashView())
        {
        }
    }
}