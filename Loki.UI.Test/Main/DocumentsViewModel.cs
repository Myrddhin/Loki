using System;
using Loki.Commands;

namespace Loki.UI.Test
{
    public class DocumentsViewModel : ContainerOneActive<Screen>, IDocumentContainer
    {
        public IScreenFactory ScreenFactory { get; set; }

        public DocumentsViewModel()
        {
            /*  var document1 = new Screen() { DisplayName = "Document 1 Test" };
              var document2 = new Screen() { DisplayName = "Document 2 Test" };

              Items.Add(document1);
              Items.Add(document2);*/
            this.ClosingProcessed += DocumentsViewModel_ClosingProcessed;
        }

        private void DocumentsViewModel_ClosingProcessed(object sender, ClosingProcessedEventArgs<Screen> e)
        {
            ScreenFactory.Release(e.Item);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Commands.Handle(ApplicationCommands.Search, Search_Execute);
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        protected override void OnDesactivate(bool close)
        {
            base.OnDesactivate(close);
        }

        private void Search_Execute(object sender, EventArgs e)
        {
            var document3 = new Screen() { DisplayName = "Created at " + DateTime.Now.ToString() };

            Items.Add(document3);
            ActivateItem(document3);
        }

        public override void ActivateItem(Screen item)
        {
            if (ActiveItem != item)
            {
                item.DisplayName += "Activated";
            }

            base.ActivateItem(item);
        }

        public void Handle(NavigationMessage<Screen> message)
        {
            var viewModel = ScreenFactory.Create<Screen>() as Screen;
            //var viewModel = Toolkit.IoC.DefaultContext.Get(message.NavigateTo) as Screen;
            if (viewModel != null)
            {
                viewModel.DisplayName = "Created by navigation at " + DateTime.Now.ToString();
                Items.Add(viewModel);
                ActivateItem(viewModel);
            }
        }
    }
}