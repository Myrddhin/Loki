using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Loki.Common;
using Loki.IoC;
using Loki.IoC.Registration;

namespace Loki.UI.Win
{
    internal class WinformTemplatingEngine : ITemplatingEngine
    {
        private const string ContextName = "UITemplates";
        private Dictionary<string, Type> associations;
        private IObjectContext internalContext;
        private Dictionary<Type, Type> typeAssociations;

        public WinformTemplatingEngine()
        {
            internalContext = Toolkit.IoC.CreateContext(ContextName);
            associations = new Dictionary<string, Type>();
            typeAssociations = new Dictionary<Type, Type>();
        }

        public object CreateBind(object view, object viewModel)
        {
            var args = new BindingEventArgs() { Bind = null, View = view, ViewModel = viewModel };
            OnBindingRequiered(args);
            if (args.Bind != null)
            {
                return args.Bind;
            }

            object bind = null;

            var control = view as Form;
            if (control != null)
            {
                View.ProtectedCall(control, () => bind = new FormBind(control, viewModel));
            }

            //var documentManager = view as DocumentManager;
            //if (documentManager != null)
            //{
            //    return new DocumentManagerBind(documentManager, viewModel);
            //}

            //var document = view as BaseDocument;
            //if (document != null)
            //{
            //    return new DocumentBind(document, viewModel);
            //}

            return bind;
        }

        #region BindingRequest

        public event EventHandler<BindingEventArgs> BindingRequired;

        protected virtual void OnBindingRequiered(BindingEventArgs e)
        {
            EventHandler<BindingEventArgs> handler = BindingRequired;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion BindingRequest

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
    }
}