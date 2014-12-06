using System;
using System.Threading.Tasks;

namespace Loki.UI
{
    public interface IThreadingContext
    {
        void BeginOnUIThread(Action action);

        Task OnUIThreadAsync(Action action);

        void OnUIThread(Action action);
    }
}