using System;
using System.Windows.Data;
using DevExpress.Xpf.Docking;
using Loki.Commands;
using Loki.Common;
using Loki.UI.Wpf.Converters;

namespace Loki.UI.Wpf.Binds
{
    internal class DocumentPanelBind : FrameworkElementBind<DocumentPanel>
    {
        public DocumentPanelBind(DocumentPanel view, object viewModel)
            : base(view, viewModel)
        {
            IScreen screen = viewModel as IScreen;
            if (screen != null)
            {
                if (view.CloseCommand == null)
                {
                    Func<bool> canExecute = null;

                    Action execute = null;
                    var parent = ((IChild)screen).Parent as IConductor;
                    if (parent != null)
                    {
                        execute = () => parent.CloseItem(screen);
                        canExecute = () => true;
                    }
                    else
                    {
                        execute = () => screen.Desactivate(true);
                        canExecute = () =>
                        {
                            bool buffer = false;
                            screen.CanClose(b => buffer = b);
                            return buffer;
                        };
                    }

                    view.CloseCommand = new LokiRelayCommand(canExecute, execute);
                }

                MultiBinding binding = new MultiBinding();
                binding.Bindings.Add(new Binding("DisplayName") { Source = viewModel });
                binding.Bindings.Add(new Binding("IsChanged") { Source = screen.State });
                binding.Converter = new DisplayNameWithStateConverter();
                binding.Mode = BindingMode.OneWay;
                binding.NotifyOnSourceUpdated = true;
                binding.NotifyOnTargetUpdated = false;
                view.SetBinding(DocumentPanel.CaptionProperty, binding);
            }
            else
            {
                IHaveDisplayName withDisplay = viewModel as IHaveDisplayName;
                if (withDisplay != null)
                {
                    Binding binding = new Binding(ExpressionHelper.GetProperty<IHaveDisplayName, string>(x => x.DisplayName).Name);

                    binding.Source = viewModel;
                    binding.Mode = BindingMode.OneWay;
                    binding.NotifyOnSourceUpdated = true;
                    binding.NotifyOnTargetUpdated = false;
                    view.SetBinding(DocumentPanel.CaptionProperty, binding);

                    Log.DebugFormat("Binding {0} and {1}", view, withDisplay.DisplayName);
                }
            }

            if (view.ContentTemplate == null)
            {
                view.ContentTemplate = DefaultItemTemplate;
            }
        }
    }
}