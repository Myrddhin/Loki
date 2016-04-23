using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Loki.UI.Wpf.Controls
{
    public class SimpleButton : Button
    {
        static SimpleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(SimpleButton),
                new FrameworkPropertyMetadata(typeof(SimpleButton)));
        }

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image",
            typeof(ImageSource), typeof(SimpleButton), new PropertyMetadata(null));

        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register("ImageHeight",
            typeof(double), typeof(SimpleButton), new PropertyMetadata(double.NaN));

        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register("ImageWidth",
            typeof(double), typeof(SimpleButton), new PropertyMetadata(double.NaN));
    }
}