using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Loki.Common;

namespace Loki.UI
{
    public class EditableElement : DisplayElement, ICentralizedChangeTracking, IEditableObject
    {
        private static ConcurrentDictionary<Type, List<PropertyParam>> propertyInfos = new ConcurrentDictionary<Type, List<PropertyParam>>();

        private static void PrepareEditableProperties(Type type)
        {
            if (!propertyInfos.ContainsKey(type))
            {
                IEnumerable<PropertyInfo> properties = type.GetProperties().Where(x => x.CanRead && x.CanWrite);
                List<PropertyParam> propInfos = new List<PropertyParam>(properties.Count());

                foreach (PropertyInfo prop in properties)
                {
                    PropertyParam param = new PropertyParam();
                    param.Name = prop.Name;
                    param.Getter = prop.GetGetMethod();
                    param.Setter = prop.GetSetMethod();

                    if (param.Setter != null && param.Getter != null)
                    {
                        propInfos.Add(param);
                    }
                }

                propertyInfos.TryAdd(type, propInfos);
            }
        }

        private struct PropertyParam
        {
            public string Name { get; set; }

            public MethodInfo Getter { get; set; }

            public MethodInfo Setter { get; set; }
        }

        private Dictionary<string, object> oldValues = new Dictionary<string, object>();

        public void BeginEdit()
        {
            Type type = this.GetType();

            if (!propertyInfos.ContainsKey(type))
            {
                PrepareEditableProperties(type);
            }

            if (!this.IsChanged)
            {
                foreach (PropertyParam propParam in propertyInfos[type])
                {
                    oldValues[propParam.Name] = propParam.Getter.Invoke(this, null);
                }
            }
        }

        public void CancelEdit()
        {
            Type type = this.GetType();

            if (!propertyInfos.ContainsKey(type))
            {
                PrepareEditableProperties(type);
            }

            foreach (PropertyParam propParam in propertyInfos[type])
            {
                if (oldValues.ContainsKey(propParam.Name))
                {
                    propParam.Setter.Invoke(this, new object[] { oldValues[propParam.Name] });
                }
            }
        }

        public void EndEdit()
        {
            if (!this.IsChanged)
            {
                oldValues.Clear();
            }
        }
    }
}