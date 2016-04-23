using System;
using System.ComponentModel;
using System.Windows.Forms;

using Loki.Common;

namespace Loki.UI.Win
{
    internal class FormBind : ControlBind<Form>
    {
        private bool desactivateFromViewModel;
        private bool desativatingFromView;

        private bool activatingFromViewModel;
        private bool activatingFromView;

        private bool closingFromViewModel;
        private bool closingFromView;

        private bool actuallyClosing;

        public FormBind(ICoreServices services, IThreadingContext ctx, Form view, object viewModel)
            : base(services, ctx, view, viewModel)
        {
            var activatable = viewModel as IActivable;
            if (activatable != null)
            {
                activatable.Activated += ViewModel_Activate;
                Component.Activated += View_Activate;
            }

            var deactivatable = viewModel as IDesactivable;
            var closable = viewModel as ICloseable;
            if (deactivatable != null)
            {
                Component.Deactivate += View_Deactivate;
                deactivatable.Desactivated += ViewModel_Deactivated;
            }

            if (closable != null)
            {
                Component.Closed += View_Closed;
                closable.Closed += ViewModel_Closed;
            }

            var guard = viewModel as IGuardClose;
            if (guard != null)
            {
                Component.Closing += Closing;
            }
        }

        private void View_Activate(object sender, EventArgs e)
        {
            if (activatingFromViewModel)
            {
                return;
            }

            var activatable = (IActivable)ViewModel;
            activatingFromView = true;
            activatable.Activate();
            activatingFromView = false;
        }

        private void ViewModel_Activate(object sender, EventArgs e)
        {
            if (activatingFromView)
            {
                return;
            }

            activatingFromViewModel = true;
            if (Component.Visible)
            {
                Component.Activate();
            }
            else
            {
                Component.Show();

                Application.DoEvents();
            }

            activatingFromViewModel = false;
        }

        private void View_Deactivate(object sender, EventArgs e)
        {
            if (desactivateFromViewModel)
            {
                return;
            }

            var desactivatable = (IDesactivable)ViewModel;
            desativatingFromView = true;
            desactivatable.Desactivate(false);
            desativatingFromView = false;
        }

        private void Closed(object sender, EventArgs e)
        {
            Component.Closed -= Closed;
            Component.Closing -= Closing;

            if (desactivateFromViewModel)
            {
                return;
            }

            var deactivatable = (ICloseable)ViewModel;

            desativatingFromView = true;
            deactivatable.TryClose(true);
            desativatingFromView = false;
        }

        private void ViewModel_Deactivated(object sender, DesactivationEventArgs e)
        {
            if (e.WasClosed)
            {
                return;
            }

            if (desativatingFromView)
            {
                return;
            }

            desactivateFromViewModel = true;
            Component.WindowState = FormWindowState.Minimized;
            desactivateFromViewModel = false;
        }

        private void ViewModel_Closed(object sender, EventArgs e)
        {
            if (closingFromView)
            {
                return;
            }

            closingFromViewModel = true;
            actuallyClosing = true;
            Component.ProtectedCall(Component.Close);
            actuallyClosing = false;
            closingFromViewModel = false;
        }

        private void View_Closed(object sender, EventArgs e)
        {
            Component.Closed -= View_Closed;
            Component.Closing -= Closing;

            if (closingFromViewModel)
            {
                return;
            }

            var deactivatable = (ICloseable)ViewModel;

            closingFromView = true;
            deactivatable.TryClose(true);
            closingFromView = false;
        }

        private void Closing(object sender, CancelEventArgs e)
        {
            if (e.Cancel)
            {
                return;
            }

            var guard = (IGuardClose)ViewModel;

            if (actuallyClosing)
            {
                actuallyClosing = false;
                return;
            }

            bool runningAsync = false, shouldEnd = false;

            guard.CanClose(canClose =>
            {
                Context.OnUIThread(
                    () =>
                    {
                        if (runningAsync && canClose)
                        {
                            actuallyClosing = true;
                            this.Component.ProtectedCall(Component.Close);
                        }
                        else
                        {
                            e.Cancel = !canClose;
                        }

                        shouldEnd = true;
                    });
            });

            if (shouldEnd)
            {
                return;
            }

            runningAsync = e.Cancel = true;
        }
    }
}