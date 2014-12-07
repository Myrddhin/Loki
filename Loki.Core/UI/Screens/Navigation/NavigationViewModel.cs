using System.ComponentModel;
using Loki.Common;

namespace Loki.UI
{
    public class NavigationElement : DisplayElement, INavigationElement
    {
        #region DisplayName

        private static PropertyChangedEventArgs argsDisplayNameChanged = ObservableHelper.CreateChangedArgs<NavigationElement>(x => x.DisplayName);

        private static PropertyChangingEventArgs argsDisplayNameChanging = ObservableHelper.CreateChangingArgs<NavigationElement>(x => x.DisplayName);

        private string displayName;

        public string DisplayName
        {
            get
            {
                return displayName;
            }

            set
            {
                if (value != displayName)
                {
                    NotifyChanging(argsDisplayNameChanging);
                    displayName = value;
                    NotifyChanged(argsDisplayNameChanged);
                }
            }
        }

        #endregion DisplayName

        #region Glyph

        private static PropertyChangedEventArgs argsGlyphChanged = ObservableHelper.CreateChangedArgs<NavigationElement>(x => x.Glyph);

        private static PropertyChangingEventArgs argsGlyphChanging = ObservableHelper.CreateChangingArgs<NavigationElement>(x => x.Glyph);

        private string glyph;

        public string Glyph
        {
            get
            {
                return glyph;
            }

            set
            {
                if (value != glyph)
                {
                    NotifyChanging(argsGlyphChanging);
                    glyph = value;
                    NotifyChanged(argsGlyphChanged);
                }
            }
        }

        #endregion Glyph
    }
}