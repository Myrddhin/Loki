using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Loki.UI.Wpf.Behaviors
{
    public class ClickToMessageBehavior : Behavior<UIElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            var element = this.AssociatedObject;
            element.MouseLeftButtonUp += Element_MouseLeftButtonUp;
        }

        public object Message { get; set; }

        private void Element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            View.MessageBus.Publish(Message, action => action());
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            var element = this.AssociatedObject;
            element.MouseLeftButtonUp -= Element_MouseLeftButtonUp;
        }
    }
}