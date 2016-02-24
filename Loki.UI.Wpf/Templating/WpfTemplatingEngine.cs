using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

using Loki.Common;
using Loki.IoC;
using Loki.IoC.Registration;
using Loki.UI.Wpf.Binds;

namespace Loki.UI.Wpf
{
    public class WpfTemplatingEngine : ITemplatingEngine
    {
        private readonly Dictionary<string, Type> associations;
        private readonly IObjectContext internalContext;
        private readonly Dictionary<Type, Type> typeAssociations;

        protected ICoreServices Services { get; private set; }

        protected IThreadingContext ThreadingContext { get; private set; }

        public WpfTemplatingEngine(IObjectContext context, ICoreServices coreServices, IThreadingContext threading)
        {
            this.Services = coreServices;
            this.ThreadingContext = threading;
            associations = new Dictionary<string, Type>();
            typeAssociations = new Dictionary<Type, Type>();
            internalContext = context;
        }

        public object GetTemplate(object model)
        {
            if (model == null)
            {
                return null;
            }

            var type = model.GetType();
            Type viewType = null;
            if (this.typeAssociations.ContainsKey(type))
            {
                viewType = this.typeAssociations[type];
            }
            else if (this.associations.ContainsKey(type.Name))
            {
                viewType = this.associations[type.Name];
            }

            if (viewType != null)
            {
                var view = this.internalContext.Get(viewType);

                this.CreateBind(view, model);

                return view;
            }

            return new TextBlock { Text = string.Format("Cannot find view for {0}.", type) };
        }

        public void LoadByConvention(IConventionManager conventionManager, params Assembly[] assemblies)
        {
            foreach (var match in conventionManager.ViewViewModel(assemblies))
            {
                associations.Add(match.Key, match.Value);
                internalContext.Register(Element.For(match.Value).Lifestyle.Transient);
            }
        }

        public void RegisterAssociation(Type modelType, Type viewType)
        {
            typeAssociations[modelType] = viewType;
            internalContext.Register(Element.For(viewType).Lifestyle.Transient);
        }

        public void RegisterAssociation<TModel, TView>()
        {
            RegisterAssociation(typeof(TModel), typeof(TView));
        }

        public object CreateBind(object view, object viewModel)
        {
            var args = new BindingEventArgs { Bind = null, View = view, ViewModel = viewModel };
            OnBindingRequired(args);
            if (args.Bind != null)
            {
                return args.Bind;
            }

            object buffer = null;
            ThreadingContext.OnUIThread(() => buffer = InternalCreateBind(view, viewModel));
            return buffer;
        }

        protected virtual object InternalCreateBind(object view, object viewModel)
        {
            var control = view as Window;
            if (control != null)
            {
                return new WindowBind(Services, ThreadingContext, control, viewModel);
            }

            var fe = view as FrameworkElement;
            if (fe != null)
            {
                return new FrameworkElementBind<FrameworkElement>(Services, ThreadingContext, fe, viewModel);
            }

            return null;
        }

        #region BindingRequired

        public event EventHandler<BindingEventArgs> BindingRequired;

        protected virtual void OnBindingRequired(BindingEventArgs e)
        {
            EventHandler<BindingEventArgs> handler = BindingRequired;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion BindingRequired
    }
}