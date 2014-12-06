using System.Globalization;

namespace Loki.UI
{
    public interface IWindowManager
    {
        CultureInfo Culture { get; }

        bool DesignMode { get; }

        // string GetOpenFileName(FileDialogInformations informations);

        //  string GetSaveFileName(FileDialogInformations informations);

        bool? ShowAsPopup(object viewModel);
    }
}