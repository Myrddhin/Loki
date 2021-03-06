﻿using System;
using System.ComponentModel;
using System.Windows;

using Loki.Common;
using Loki.Common.Diagnostics;
using Loki.UI.Models;

namespace Loki.UI.Wpf.Binds
{
    internal class WindowBind : FrameworkElementBind<Window>
    {
        private bool desactivateFromViewModel;
        private bool desativatingFromView;

        private bool activatingFromViewModel;
        private bool activatingFromView;

        private bool closingFromViewModel;
        private bool closingFromView;

        private bool actuallyClosing;

        public WindowBind(IDiagnostics diagnostics, Window view, object viewModel)
            : base(diagnostics, view, viewModel)
        {
            var withDisplayName = viewModel as IHaveDisplayName;
            if (withDisplayName != null)
            {
                Component.SetBinding(Window.TitleProperty, ExpressionHelper.GetProperty<IHaveDisplayName, string>(x => x.DisplayName).Name);
            }

            var activatable = viewModel as IActivable;
            if (activatable != null)
            {
                activatable.Activated += ViewModel_Activate;
                view.Activated += View_Activate;
            }

            var deactivatable = viewModel as IActivable;
            var closable = viewModel as ICloseable;
            if (deactivatable != null)
            {
                view.Deactivated += View_Deactivate;
                deactivatable.Desactivated += ViewModel_Deactivated;
            }

            if (closable != null)
            {
                view.Closed += View_Closed;
                closable.Closed += ViewModel_Closed;
            }

            var guard = viewModel as IGuardClose;
            if (guard != null)
            {
                view.Closing += Closing;
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
            if (Component.Visibility == Visibility.Visible)
            {
                Component.Activate();
            }
            else
            {
                Component.Show();
            }

            activatingFromViewModel = false;
        }

        private void View_Deactivate(object sender, EventArgs e)
        {
            if (desactivateFromViewModel)
            {
                return;
            }

            var desactivatable = (IActivable)ViewModel;
            desativatingFromView = true;
            desactivatable.Desactivate(false);
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
            Component.WindowState = WindowState.Minimized;
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
            Component.Dispatcher.Invoke(Component.Close);
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
            deactivatable.TryClose(Component.DialogResult);
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
               // Threading.OnUIThread(
               //     () =>
               //     {
                        if (runningAsync && canClose)
                        {
                            actuallyClosing = true;
                            Component.Close();
                        }
                        else
                        {
                            e.Cancel = !canClose;
                        }

                        shouldEnd = true;
                 //   });
            });

            if (shouldEnd)
            {
                return;
            }

            runningAsync = e.Cancel = true;
        }
    }
}