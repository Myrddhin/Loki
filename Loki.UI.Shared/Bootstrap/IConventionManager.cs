using System;
using System.Collections.Generic;
using System.Reflection;

namespace Loki.UI.Bootstrap
{
    public interface IConventionManager
    {
        IDictionary<string, Func<object>> ViewViewModel(params Assembly[] assemblies);
    }
}