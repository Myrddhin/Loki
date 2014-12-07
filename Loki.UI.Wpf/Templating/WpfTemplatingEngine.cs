using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Docking;
using Loki.Common;
using Loki.IoC;
using Loki.IoC.Registration;
using Loki.UI.Wpf.Binds;

namespace Loki.UI.Wpf
{
    public class WpfTemplatingEngine : ITemplatingEngine
    {
        private const string ContextName = "UITemplates";
        private Dictionary<string, Type> associations;
        private IObjectContext internalContext;
        private Dictionary<Type, Type> typeAssociations;

        public WpfTemplatingEngine()
        {
            associations = new Dictionary<string, Type>();
            typeAssociations = new Dictionary<Type, Type>();
            internalContext = Toolkit.IoC.CreateContext(ContextName);
        }

        public object GetTemplate(object model)
        {
            if (model != null)
            {
                var type = model.GetType();
                Type viewType = null;
                if (typeAssociations.ContainsKey(type))
                {
                    viewType = typeAssociations[type];
                }
                else if (associations.ContainsKey(type.Name))
                {
                    viewType = associations[type.Name];
                }

                if (viewType != null)
                {
                    var view = internalContext.Get(viewType);

                    CreateBind(view, model);

                    return view;
                }
                else
                {
                    return new TextBlock { Text = string.Format("Cannot find view for {0}.", type) };
                }
            }

            return null;
        }

        public void LoadByConvention(IConventionManager conventionManager, params System.Reflection.Assembly[] assemblies)
        {
            foreach (var match in conventionManager.ViewViewModel(assemblies))
            {
                associations.Add(match.Key, match.Value);
                internalContext.Register(Element.For(match.Value).Lifestyle.NoTracking);
            }
        }

        public void RegisterAssociation(Type modelType, Type viewType)
        {
            typeAssociations[modelType] = viewType;
            internalContext.Register(Element.For(viewType).Lifestyle.NoTracking);
        }

        public void RegisterAssociation<TModel, TView>()
        {
            RegisterAssociation(typeof(TModel), typeof(TView));
        }

        public object CreateBind(object view, object viewModel)
        {
            object buffer = null;
            Toolkit.UI.Threading.OnUIThread(() => buffer = InternalCreateBind(view, viewModel));
            return buffer;
        }

        private object InternalCreateBind(object view, object viewModel)
        {
            var args = new BindingEventArgs() { Bind = null, View = view, ViewModel = viewModel };
            OnBindingRequired(args);
            if (args.Bind != null)
            {
                return args.Bind;
            }

            var control = view as Window;
            if (control != null)
            {
                return new WindowBind(control, viewModel);
            }

            var documentManager = view as DocumentGroup;
            if (documentManager != null)
            {
                return new DocumentGroupBind(documentManager, viewModel);
            }

            var document = view as DocumentPanel;
            if (document != null)
            {
                return new DocumentPanelBind(document, viewModel);
            }

            var navBarItem = view as DevExpress.Xpf.NavBar.NavBarItem;
            if (navBarItem != null)
            {
                return new NavBarItemBind(navBarItem, viewModel);
            }

            var fe = view as FrameworkElement;
            if (fe != null)
            {
                return new FrameworkElementBind<FrameworkElement>(fe, viewModel);
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