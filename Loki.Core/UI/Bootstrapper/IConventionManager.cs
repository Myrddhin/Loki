using System;
using System.Collections.Generic;
using System.Reflection;

namespace Loki.UI
{
    public interface IConventionManager
    {
        IDictionary<string, Type> ViewViewModel(params Assembly[] assemblies);
    }
}