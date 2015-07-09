using System;
using System.ComponentModel;
using System.Windows.Forms;
using Loki.Common;

namespace Loki.UI.Win
{
    public class ControlBind<T> : ComponentBind<T> where T : Control
    {
        private DevExpress.XtraWaitForm.ProgressPanel waitingView = new DevExpress.XtraWaitForm.ProgressPanel();

        protected void ProtectedExecute(Action action)
        {
            if (Component.IsHandleCreated)
            {
                if (Component.InvokeRequired)
                {
                    Component.BeginInvoke(action);
                }
                else
                {
                    action();
                }
            }
            else
            {
                EventHandler methodToInvoke = null;
                methodToInvoke = (s, e) =>
                {
                    Component.HandleCreated -= methodToInvoke;
                    if (Component.InvokeRequired)
                    {
                        Component.BeginInvoke(action);
                    }
                    else
                    {
                        action();
                    }
                };

                Component.HandleCreated += methodToInvoke;
            }
        }

        public ControlBind(T view, object viewModel)
            : base(view, viewModel)
        {
            view.Tag = viewModel;

            var asyncModel = viewModel as AsyncScreen;
            if (asyncModel != null)
            {
                waitingView.Tag = asyncModel;
                waitingView.Visible = false;
                ProtectedExecute(() =>
                {
                    Component.Controls.Add(waitingView);
                    Component.Controls.SetChildIndex(waitingView, 0);
                });

                /*Toolkit.EventManager.PropertyChanged.Register(
                asyncModel,
                ExpressionHelper.GetProperty<AsyncScreen, bool>(x => x.IsBusy).Name,
                this,
                (b, s, e) => b.Async_Changed(s, e));

                ControlBinder.Editors.BindValue<IAsync>(waitingView., x => x.Status);*/

                Component.Disposed += (s, e) => waitingView.Dispose();
            }

            var loadable = viewModel as ILoadable;

            if (loadable != null)
            {
                Component.ExecuteOnFirstLoad(View_Load);
                Component.ExecuteOnFirstLoad(loadable.Load);
            }

            // Display name
            BindName(ExpressionHelper.GetProperty<System.Windows.Forms.Control, string>(x => x.Text));
            BindName(ExpressionHelper.GetProperty<Control, string>(x => x.Text));
        }

        private void View_Load()
        {
            var initializable = ViewModel as IInitializable;
            var loadable = ViewModel as ILoadable;
            if (initializable != null)
            {
                ProtectedExecute(initializable.Initialize);
            }

            ProtectedExecute(loadable.Load);
        }

        private void Async_Changed(object sender, PropertyChangedEventArgs e)
        {
            Action action = () =>
            {
                AsyncScreen model = ViewModel as AsyncScreen;
                if (model == null || !model.IsBusy)
                {
                    waitingView.Hide();
                    foreach (System.Windows.Forms.Control ctrl in Component.Controls)
                    {
                        ctrl.Enabled = ctrl != waitingView;
                    }
                }
                else
                {
                    waitingView.Left = Component.Left + ((Component.Width - waitingView.Width) / 2);
                    waitingView.Top = Component.Top + ((Component.Height - waitingView.Height) / 2);

                    waitingView.Show();
                    foreach (System.Windows.Forms.Control ctrl in Component.Controls)
                    {
                        ctrl.Enabled = ctrl == waitingView;
                    }

                    Component.Resize += new EventHandler(Component_Resize);
                }
            };

            ProtectedExecute(action);
        }

        private void Component_Resize(object sender, EventArgs e)
        {
            Action action = () =>
            {
                if (waitingView.Visible)
                {
                    waitingView.Left = Component.Left + ((Component.Width - waitingView.Width) / 2);
                    waitingView.Top = Component.Top + ((Component.Height - waitingView.Height) / 2);
                }
            };

            ProtectedExecute(action);
        }
    }
}