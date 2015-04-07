using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loki.UI.Wpf
{
    public class DefaultSplashModel : Screen, ISplashViewModel
    {
        public async Task ApplicationInitialize()
        {
            await Task.Delay(500);
        }
    }
}