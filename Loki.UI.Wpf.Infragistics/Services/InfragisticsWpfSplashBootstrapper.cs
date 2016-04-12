using System;
using System.Windows;

using Loki.IoC;

namespace Loki.UI.Wpf.Infragistics
{
    public class InfragisticsWpfSplashBootstrapper<TMainModel, TSplashModel> : WpfSplashBootStrapper<TMainModel, TSplashModel>
        where TMainModel : class, IScreen
        where TSplashModel : class, ISplashViewModel
    {
        protected override void InitializeUIEngine(IObjectContext context)
        {
            context.Initialize(UIInstaller.Wpf);

            // Overrides dictionnary
            ResourceDictionary myResourceDictionary = new ResourceDictionary();
            myResourceDictionary.Source = new Uri("pack://application:,,,/Loki.UI.Wpf.Infragistics;component/Themes/Overrides.xaml");
            Application.Current.Resources.MergedDictionaries.Add(myResourceDictionary);
        }

        public InfragisticsWpfSplashBootstrapper(Window splashWindow)
            : base(splashWindow)
        {
        }
    }
}