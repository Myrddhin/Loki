using System;
using System.Collections.Generic;
using System.Reflection;

using Loki.Common.Diagnostics;

namespace Loki.UI
{
    public interface IConventionManager
    {
        IDictionary<string, Func<object>> ViewViewModel(params Assembly[] assemblies);
    }
}