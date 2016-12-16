using System;
using System.Windows;

using Xunit;

namespace Loki.UI
{
    [Trait("Category", "UI Helpers")]
    public class ViewTest
    {
        private class MockWindow : Window
        {
            public void RaiseContentRendered()
            {
                OnContentRendered(EventArgs.Empty);
            }
        }

        #region DesignMode

        [Fact(DisplayName = "Design mode disabled when testing")]
        public void DesignModeDisabled()
        {
            Assert.False(View.DesignMode);
        }

        [Fact(DisplayName = "Only one check for design mode")]
        public void Singleton()
        {
            var check = View.DesignMode;

            var otherCheck = View.DesignMode;

            Assert.Equal(check, otherCheck);
        }

        #endregion DesignMode

        #region ExecuteOnLoad

        [WpfFact(DisplayName = "Execute immediately when loaded")]
        [DomainNeedsDispatcherCleanup]
        public void AfterLoadExecute()
        {
            bool raised = false;
            var element = new Window { Height = 0, Width = 0, Visibility = Visibility.Collapsed };

            element.Show();
            EventHandler handler = (sender, args) => raised = true;

            View.ExecuteOnLoad(element, handler);
            element.Close();
            Assert.True(raised);
        }

        [WpfFact(DisplayName = "Use load event for standard elements")]
        public void LoadForStandardElement()
        {
            bool raised = false;
            var element = new FrameworkElement();

            EventHandler handler = (sender, args) => raised = true;

            View.ExecuteOnLoad(element, handler);

            element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));

            Assert.True(raised);
        }

        [WpfFact(DisplayName = "Use content rendered event for windows")]
        public void ContentRenderedForElement()
        {
            bool raised = false;
            var element = new MockWindow();

            EventHandler handler = (sender, args) => raised = true;

            View.ExecuteOnLoad(element, handler);
            element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
            Assert.False(raised);
            element.RaiseContentRendered();
            Assert.True(raised);
        }

        [WpfFact(DisplayName = "Only one call per event raised")]
        public void OnlyOneCall()
        {
            int count = 0;
            var element = new FrameworkElement();

            EventHandler handler = (sender, args) => count++;

            View.ExecuteOnLoad(element, handler);

            element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
            element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
            element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));

            Assert.Equal(1, count);
        }

        #endregion ExecuteOnLoad
    }
}