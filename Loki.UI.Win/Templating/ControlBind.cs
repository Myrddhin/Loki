using System;
using System.Windows.Forms;
using Loki.Common;

namespace Loki.UI.Win
{
    public class ControlBind<T> : ComponentBind<T> where T : Control
    {
        public ControlBind(T view, object viewModel)
            : base(view, viewModel)
        {
            view.Tag = viewModel;

            var initializable = viewModel as IInitializable;
            if (initializable != null)
            {
                Component.BindingContextChanged += View_Initialize;
            }

            var loadable = viewModel as ILoadable;
            if (loadable != null)
            {
                Component.ExecuteOnFirstLoad(loadable.Load);
            }

            // Display name
            BindName(ExpressionHelper.GetProperty<Control, string>(x => x.Text));
        }

        private void View_Initialize(object sender, EventArgs e)
        {
            var initializable = ViewModel as IInitializable;
            if (initializable != null)
            {
                initializable.Initialize();
            }
        }
    }
}