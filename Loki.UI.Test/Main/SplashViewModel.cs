using System.ComponentModel;
using System.Threading.Tasks;
using Loki.Common;

namespace Loki.UI.Test
{
    public class SplashViewModel : Screen, ISplashViewModel
    {
        public SplashViewModel(ICoreServices services, IUIServices uiServices)
            : base(services, uiServices)
        {
        }

        private string name;

        private static PropertyChangedEventArgs nameChangedArgs = ObservableHelper.CreateChangedArgs<SplashViewModel>(x => x.Name);

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                if (value != name)
                {
                    name = value;
                    NotifyChanged(nameChangedArgs);
                }
            }
        }

        public async Task ApplicationInitialize()
        {
            Name = "Luna";
            await Task.Delay(1000);
        }
    }
}