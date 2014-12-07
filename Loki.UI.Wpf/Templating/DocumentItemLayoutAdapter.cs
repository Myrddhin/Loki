using DevExpress.Xpf.Docking;

namespace Loki.UI.Wpf
{
    public class DocumentItemLayoutAdapter : ILayoutAdapter
    {
        public string HostingGroupName { get; set; }

        public string Resolve(DockLayoutManager owner, object item)
        {
            return HostingGroupName;
        }
    }
}