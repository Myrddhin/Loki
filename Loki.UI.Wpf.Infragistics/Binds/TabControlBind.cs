using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

using Infragistics.Windows.Controls;
using Infragistics.Windows.Controls.Events;

using Loki.Common;
using Loki.UI.Wpf.Infragistics.Controls;

namespace Loki.UI.Wpf.Binds.Infragistics
{
    internal class TabControlBind : FrameworkElementBind<XamTabControl>
    {
        public class TabWrapper : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                var handler = this.PropertyChanged;
                if (handler != null)
                {
                    handler(this, e);
                }
            }

            public BindableCollection<TabItemEx> Tabs { get; private set; }

            public TabItemEx ActiveItem
            {
                get
                {
                    return Tabs.FirstOrDefault(x => x.DataContext == source.ActiveItem);
                }

                set
                {
                    if (value.DataContext != null)
                    {
                        source.ActiveItem = value.DataContext;
                    }
                }
            }

            private readonly IConductActiveItem source;

            public TabWrapper(ICoreServices services, IThreadingContext threading, IConductActiveItem source)
            {
                //services.Events.WatchCollectionChange(this, source.Children, v => v.SourceChanged);
                //services.Events.WatchPropertyChanged(this, source, x => x.ActiveItem, v => v.ActiveItem_Changed);
                Tabs = new BindableCollection<TabItemEx>(services, threading);
                this.source = source;
            }

            private void ActiveItem_Changed(object sender, PropertyChangedEventArgs e)
            {
                this.OnPropertyChanged(e);
            }

            private static TabItemEx CreateTab(object model)
            {
                var tab = new TabItemEx();
                var tabNameBinding = new Binding(ExpressionHelper.GetProperty<IHaveDisplayName, string>(target => target.DisplayName).Name);
                tabNameBinding.Mode = BindingMode.OneWay;
                tab.SetBinding(HeaderedContentControl.HeaderProperty, tabNameBinding);

                tab.Content = DefaultTemplates.DefaultTabItemContent;

                tab.DataContext = model;
                return tab;
            }

            private void SourceChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        if (e.NewItems == null) return;
                        foreach (var item in e.NewItems)
                        {
                            Tabs.Add(CreateTab(item));
                        }

                        break;

                    case NotifyCollectionChangedAction.Move:
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        if (e.OldItems == null) return;
                        foreach (var item in e.OldItems.OfType<TabItemEx>())
                        {
                            Tabs.Remove(item);
                        }

                        break;

                    case NotifyCollectionChangedAction.Replace:
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        break;
                }
            }
        }

        public TabControlBind(ICoreServices services, IThreadingContext threading, XamTabControl component, object viewModel)
            : base(services.Diagnostics, component, viewModel)
        {
            component.AddHandler(TabItemEx.ClosingEvent, new RoutedEventHandler(TabItem_Closing));
            component.AddHandler(TabItemEx.ClosedEvent, new RoutedEventHandler(TabItem_Closed));
            component.AllowTabClosing = true;
            component.VerticalContentAlignment = VerticalAlignment.Stretch;
            component.TabItemCloseButtonVisibility = TabItemCloseButtonVisibility.WhenSelectedOrHotTracked;
            if (ViewModel == null)
            {
                return;
            }

            wrapper = new TabWrapper(services, threading, ViewModel);
            Component.DataContext = wrapper;

            var itemSourceBinding = new Binding(ExpressionHelper.GetProperty<TabWrapper, IObservableEnumerable>(target => target.Tabs).Name);
            itemSourceBinding.Mode = BindingMode.OneWay;
            component.SetBinding(ItemsControl.ItemsSourceProperty, itemSourceBinding);

            var selectedItemBinding = new Binding(ExpressionHelper.GetProperty<TabWrapper, object>(target => target.ActiveItem).Name);
            selectedItemBinding.Mode = BindingMode.TwoWay;
            component.SetBinding(Selector.SelectedItemProperty, selectedItemBinding);

            // if (component.ItemContainerStyle == null)
            // {
            // component.ItemContainerStyle = DefaultTemplates.DefaultTabItemStyle;
            // }
        }

        private readonly TabWrapper wrapper;

        public new IConductActiveItem ViewModel
        {
            get { return base.ViewModel as IConductActiveItem; }
        }

        private void TabItem_Closing(object sender, RoutedEventArgs e)
        {
            var convertedEvent = e as TabClosingEventArgs;
            var item = e.OriginalSource as TabItemEx;
            if (item == null || convertedEvent == null)
            {
                return;
            }

            var guardClose = item.DataContext as IGuardClose;
            if (guardClose == null)
            {
                return;
            }

            guardClose.CanClose(r => convertedEvent.Cancel = !r);
            e.Handled = true;
        }

        private void TabItem_Closed(object sender, RoutedEventArgs e)
        {
            var item = e.OriginalSource as TabItemEx;
            var parent = this.ViewModel;
            if (item == null || parent == null)
            {
                return;
            }

            parent.DeactivateItem(item.DataContext, true);
            e.Handled = true;
        }

        protected override void DoCleanup()
        {
            Component.RemoveHandler(TabItemEx.ClosingEvent, new RoutedEventHandler(TabItem_Closing));
            Component.RemoveHandler(TabItemEx.ClosedEvent, new RoutedEventHandler(TabItem_Closed));

            base.DoCleanup();
        }
    }
}