﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Loki.Common;

namespace Loki.UI.Wpf
{
    [ContentProperty("LoadingChild")]
    public class LoadingDecorator : Decorator
    {
        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading", typeof(bool), typeof(LoadingDecorator), new PropertyMetadata(false, OnIsLoadingChanged));

        public static readonly DependencyProperty SplashScreenTemplateProperty = DependencyProperty.Register("SplashScreenTemplate", typeof(DataTemplate), typeof(LoadingDecorator), new PropertyMetadata(null, (d, e) => ((LoadingDecorator)d).OnSplashScreenTemplateChanged()));

        public static readonly DependencyProperty UseSplashScreenProperty = DependencyProperty.Register("UseSplashScreen", typeof(bool), typeof(LoadingDecorator), new PropertyMetadata(true));

        private bool contentLoaded;

        private FrameworkElement loadingChild = null;

        private Cursor oldCursor = null;
        private DXSplashScreen.SplashScreenContainer splashContainer = null;

        public LoadingDecorator()
        {
            Loaded += OnLoaded;
        }

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public FrameworkElement LoadingChild
        {
            get
            {
                return loadingChild;
            }

            set
            {
                if (loadingChild == value)
                {
                    return;
                }

                loadingChild = value;
                if (Toolkit.UI.Windows.DesignMode)
                {
                    Child = loadingChild;
                    return;
                }

                if (loadingChild != null)
                {
                    loadingChild.Visibility = Visibility.Hidden;
                }

                OnLoadingChildChanged();
            }
        }

        public DataTemplate SplashScreenTemplate
        {
            get { return (DataTemplate)GetValue(SplashScreenTemplateProperty); }
            set { SetValue(SplashScreenTemplateProperty, value); }
        }

        public bool UseSplashScreen
        {
            get { return (bool)GetValue(UseSplashScreenProperty); }
            set { SetValue(UseSplashScreenProperty, value); }
        }

        private DXSplashScreen.SplashScreenContainer SplashContainer
        {
            get
            {
                if (splashContainer == null)
                {
                    splashContainer = new DXSplashScreen.SplashScreenContainer();
                }

                return splashContainer;
            }
        }

        private static object CreateSplashScreen(object parameter)
        {
            object[] parameters = (object[])parameter;
            DataTemplate splashScreenTemplate = (DataTemplate)parameters[0];
            if (splashScreenTemplate == null)
            {
                return new WaitIndicator()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    DeferedVisibility = true,
                    Content = "Chargement..."
                };
            }
            else
            {
                return splashScreenTemplate.LoadContent();
            }
        }

        private static Window CreateSplashScreenWindow(object parameter)
        {
            object[] parameters = (object[])parameter;
            double left = (double)parameters[0];
            double top = (double)parameters[1];
            double width = (double)parameters[2];
            double height = (double)parameters[3];
            string themeName = (string)parameters[4];
            Window w = new Window()
            {
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                ShowInTaskbar = false,
                Background = new SolidColorBrush(Colors.Transparent),
                WindowStartupLocation = WindowStartupLocation.Manual,
                SizeToContent = SizeToContent.Manual,
                Left = left,
                Top = top,
                Width = width,
                Height = height,
                Topmost = true,
                ShowActivated = true,
            };
            WindowFadeAnimationBehavior.SetEnableAnimation(w, true);
            ThemeManager.SetThemeName(w, themeName);
            return w;
        }

        private static Rect GetScreenRect(FrameworkElement el)
        {
            Window w = Window.GetWindow(el);
            var leftTop = el.PointToScreen(new Point());
            var presentationSource = PresentationSource.FromVisual(w);
            if (presentationSource != null)
            {
                double dpiX = 96.0 * presentationSource.CompositionTarget.TransformToDevice.M11;
                double dpiY = 96.0 * presentationSource.CompositionTarget.TransformToDevice.M22;
                leftTop = new Point(leftTop.X * 96.0 / dpiX, leftTop.Y * 96.0 / dpiY);
            }

            return new Rect(leftTop, new Size(el.ActualWidth, el.ActualHeight));
        }

        private static void OnIsLoadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LoadingDecorator)d).OnIsLoadingChanged(e.OldValue, e.NewValue);
        }

        private void CloseSplashScreen()
        {
            if (oldCursor != null)
            {
                Mouse.OverrideCursor = oldCursor;
            }
            else
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }

            if (SplashContainer.IsActive)
            {
                SplashContainer.Close();
            }

            if (loadingChild != null)
            {
                loadingChild.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void OnIsLoadingChanged(object oldValue, object newValue)
        {
            if (object.Equals(newValue, true))
            {
                Dispatcher.BeginInvoke(new System.Action(ShowSplashScreen), DispatcherPriority.Render);
            }
            else if (contentLoaded)
            {
                Dispatcher.BeginInvoke(new System.Action(CloseSplashScreen), DispatcherPriority.Render);
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            if (Toolkit.UI.Windows.DesignMode)
            {
                return;
            }

            Dispatcher.BeginInvoke(new System.Action(OnLoadingChildChanged), DispatcherPriority.Render);
        }

        private void OnLoadingChildChanged()
        {
            Child = null;
            contentLoaded = false;
            if (IsLoaded && LoadingChild != null)
            {
                LoadingChild.Loaded += OnLoadingChildLoaded;
                ShowSplashScreen();
                Dispatcher.BeginInvoke(new System.Action(() => { Child = LoadingChild; }), DispatcherPriority.Render);
            }
        }

        private void OnLoadingChildLoaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement child = (FrameworkElement)sender;
            child.Loaded -= OnLoadingChildLoaded;

            contentLoaded = true;
            if (IsLoading)
            {
                return;
            }

            Dispatcher.BeginInvoke(new System.Action(CloseSplashScreen), DispatcherPriority.Render);
        }

        private void OnSplashScreenTemplateChanged()
        {
            if (SplashScreenTemplate != null)
            {
                SplashScreenTemplate.Seal();
            }
        }

        private void ShowSplashScreen()
        {
            if (UseSplashScreen && !SplashContainer.IsActive)
            {
                var pos = GetScreenRect(this);
                string themeName = ThemeManager.ApplicationThemeName;
                if (string.IsNullOrEmpty(themeName))
                {
                    var themeTreeWalker = ThemeManager.GetTreeWalker(this);
                    if (themeTreeWalker != null)
                    {
                        themeName = themeTreeWalker.ThemeName;
                    }
                }

                oldCursor = Mouse.OverrideCursor;
                Mouse.OverrideCursor = Cursors.Wait;

                SplashContainer.Show(
                    CreateSplashScreenWindow,
                    CreateSplashScreen,
                    new object[] { pos.Left, pos.Top, pos.Width, pos.Height, themeName },
                    new object[] { SplashScreenTemplate });
            }
        }
    }
}