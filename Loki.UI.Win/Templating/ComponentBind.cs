﻿using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Loki.Common;

namespace Loki.UI.Win
{
    public class ComponentBind<TComponent> : BaseObject
           where TComponent : Component
    {
        private Binder binder = new Binder();

        protected TComponent Component { get; private set; }

        protected object ViewModel { get; private set; }

        public ComponentBind(TComponent component, object viewModel)
        {
            Component = component;
            ViewModel = viewModel;
        }

        public IValueConverter GlyphConverter { get; set; }

        public IValueConverter NameConverter { get; set; }

        protected void BindName(PropertyInfo destinationProperty)
        {
            binder.BindName(Component, destinationProperty, ViewModel);
        }

        protected void OneWay(
                            object destination,
                            PropertyInfo destinationProperty,
                            INotifyPropertyChanged source,
                            PropertyInfo sourceProperty,
                            IValueConverter converter = null,
                            object converterParameter = null)
        {
            binder.OneWay(destination, destinationProperty, source, sourceProperty, converter, converterParameter);
        }

        protected string GetName(object item)
        {
            return binder.GetName(item);
        }
    }
}