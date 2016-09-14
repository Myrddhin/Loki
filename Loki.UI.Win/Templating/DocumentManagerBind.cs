using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Forms;

using DevExpress.Utils;
using DevExpress.XtraBars.Docking2010;
using DevExpress.XtraBars.Docking2010.Views;

using Loki.Common;

namespace Loki.UI.Win
{
    public class DocumentManagerBind : ComponentBind<DocumentManager>
    {
        public DocumentManagerBind(IInfrastructure services, IThreadingContext ctx, DocumentManager view, object viewModel)
            : base(services, ctx, view, viewModel)
        {
            var containerModel = ViewModel as IParent;
            if (containerModel == null)
            {
                return;
            }

            view.ShowThumbnailsInTaskBar = DefaultBoolean.False;

            AddDocument(Component, containerModel.Children);

            containerModel.Children.CollectionChanged += (s, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        AddDocument(Component, e.NewItems);
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        RemoveDocument(Component, e.OldItems);
                        break;

                    case NotifyCollectionChangedAction.Replace:
                        e.NewItems.OfType<IChild>().Apply(x => x.Parent = this);
                        e.OldItems.OfType<IChild>().Apply(x => x.Parent = null);
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        ResetDocuments(Component, containerModel.Children);
                        break;
                }
            };

            Component.View.DocumentProperties.AllowFloat = false;
            Component.View.DocumentAdded += View_DocumentAdded;
            Component.View.DocumentClosed += View_DocumentClosed;
            Component.View.DocumentActivated += View_DocumentActivated;
            Component.View.DocumentDeactivated += View_DocumentDeactivated;
        }

        private void View_DocumentDeactivated(object sender, DocumentEventArgs e)
        {
            var desactivable = e.Document.Tag as IDesactivable;
            if (desactivateFromViewModel || desactivable == null)
            {
                return;
            }

            desactivateFromView = true;
            desactivable.Desactivate(false);
            desactivateFromView = false;
        }

        private void View_DocumentClosed(object sender, DocumentEventArgs e)
        {
            var activable = e.Document.Tag;
            ((IConductor)this.ViewModel).CloseItem(activable);
        }

        private void View_DocumentAdded(object sender, DocumentEventArgs e)
        {
            // throw new NotImplementedException();
        }

        private void AddDocument(DocumentManager manager, IEnumerable newModels)
        {
            Context.OnUIThread(() =>
            {
                foreach (var documentModel in newModels)
                {
                    CreateDocument(manager, documentModel);
                }
            });
        }

        private BaseDocument CreateDocument(DocumentManager manager, object model)
        {
            var template = Bind.GetTemplate(model) as Control;
            if (template == null)
            {
                template = new Control();
            }

            var document = manager.View.AddDocument(template);

            Bind.CreateBind(document, model);

            IActivable activableModel = model as IActivable;
            if (activableModel != null)
            {
                if (activableModel.IsActive)
                {
                    manager.View.ActivateDocument(document.Control);
                }

                activableModel.Activated += ViewModel_Activated;
            }

            IDesactivable desactivableModel = model as IDesactivable;
            if (desactivableModel != null)
            {
                desactivableModel.Desactivated += ViewModel_Desactivated;
            }

            Log.DebugFormat("Creating document for {0} naming {1}", model, GetName(model));

            return document;
        }

        private bool activateFromView;
        private bool activateFromViewModel;

        private bool desactivateFromView;
        private bool desactivateFromViewModel;

        private void View_DocumentActivated(object sender, DocumentEventArgs e)
        {
            var activable = e.Document.Tag as IActivable;
            if (activateFromViewModel || activable == null)
            {
                return;
            }

            activateFromView = true;
            ((IConductActiveItem)ViewModel).ActivateItem(activable);
            activateFromView = false;
        }

        private void ViewModel_Activated(object sender, EventArgs e)
        {
            var document = Component.View.Documents.FindFirst(x => x.Tag == sender);
            if (document == null || activateFromView)
            {
                return;
            }

            activateFromViewModel = true;
            Component.View.ActivateDocument(document.Control);
            activateFromViewModel = false;
        }

        private void ViewModel_Desactivated(object sender, DesactivationEventArgs e)
        {
            var document = Component.View.Documents.FindFirst(x => x.Tag == sender);
            if (document == null || desactivateFromView)
            {
                return;
            }

            desactivateFromViewModel = true;

            // Try to desactivate control ; to be done for floating
            desactivateFromViewModel = false;
        }

        private void RemoveDocument(DocumentManager manager, IEnumerable oldModels)
        {
            foreach (var documentModel in oldModels)
            {
                ViewModelExtenstions.TryDeactivate(documentModel, true);
                var oldDoc = manager.View.Documents.FindFirst(x => x.Control.Tag == documentModel);
                if (oldDoc != null)
                {
                    manager.View.Controller.Close(oldDoc);
                }
            }
        }

        private void ResetDocuments(DocumentManager manager, IEnumerable listModels)
        {
            foreach (var document in manager.View.Documents)
            {
                ViewModelExtenstions.TryDeactivate(document.Control.Tag, true);
            }

            manager.View.Controller.CloseAll();

            AddDocument(manager, listModels);
        }
    }
}