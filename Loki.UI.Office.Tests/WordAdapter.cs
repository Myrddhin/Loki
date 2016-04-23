using Microsoft.Office.Interop.Word;

namespace Loki.UI.Office.Tests
{
    internal class WordAdapter : IWordAdapter
    {
        private Application application;

        public WordAdapter(Application word)
        {
            this.application = word;
        }
    }
}