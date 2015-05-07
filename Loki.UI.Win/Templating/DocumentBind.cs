using DevExpress.XtraBars.Docking2010.Views;
using Loki.Common;

namespace Loki.UI.Win
{
    public class DocumentBind : ComponentBind<BaseDocument>
    {
        public DocumentBind(BaseDocument view, object viewModel)
            : base(view, viewModel)
        {
            view.Tag = viewModel;

            BindName(ExpressionHelper.GetProperty<BaseDocument, string>(x => x.Caption));
            BindName(ExpressionHelper.GetProperty<BaseDocument, string>(x => x.Header));
        }
    }
}