using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Loki.UI
{
    public class DefaultConventionManager : IConventionManager
    {
        private const string ViewPattern = "([^.]+)View$";

        private const string ViewModelFormat = "{0}Model";

        private readonly Regex matcher = new Regex(ViewPattern);

        public IDictionary<string, Func<object>> ViewViewModel(params Assembly[] assemblies)
        {
            var associations = new Dictionary<string, Func<object>>();

            foreach (var item in assemblies.Distinct().SelectMany(x => x.GetExportedTypes()))
            {
                string name = item.FullName;
                if (!matcher.IsMatch(name) || item.IsInterface)
                {
                    continue;
                }

                var key = string.Format(
                    CultureInfo.InvariantCulture,
                    ViewModelFormat,
                    matcher.Match(name).Groups[0].Value);

                var builder = item.GetTypeInfo().GetConstructor(Type.EmptyTypes);
                if (builder == null)
                {
                    continue;
                }

                var value = Expression.Lambda<Func<object>>(Expression.New(builder)).Compile();
                associations.Add(key, value);
            }

            return associations;
        }
    }
}