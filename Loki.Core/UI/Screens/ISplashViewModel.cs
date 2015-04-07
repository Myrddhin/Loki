using System;
using System.Threading.Tasks;

namespace Loki.UI
{
    public interface ISplashViewModel : IScreen
    {
        Task ApplicationInitialize();
    }
}