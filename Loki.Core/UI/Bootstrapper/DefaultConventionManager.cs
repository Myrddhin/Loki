using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Loki.UI
{
    public class DefaultConventionManager : IConventionManager
    {
        private const string ViewPattern = "([^.]+)View$";

        private const string ViewModelFormat = "{0}Model";

        private readonly Regex matcher = new Regex(ViewPattern);

        public IDictionary<string, Type> ViewViewModel(params Assembly[] assemblies)
        {
            Dictionary<string, Type> associations = new Dictionary<string, Type>();

            foreach (var item in assemblies.Distinct().SelectMany(x => x.GetExportedTypes()))
            {
                string name = item.FullName;
                if (matcher.IsMatch(name) && !item.IsInterface)
                {
                    associations.Add(string.Format(CultureInfo.InvariantCulture, ViewModelFormat, matcher.Match(name).Groups[0].Value), item);
                }
            }

            return associations;
        }
    }
}