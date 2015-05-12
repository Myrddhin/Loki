using System;
using System.Threading.Tasks;

namespace Loki.UI.Tasks
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITaskConfiguration<TArg, TResult> : ITaskConfiguration
    {
        Func<TArg, Task<TResult>> DoWorkAsync { get; }
    }
}