using System;

namespace Loki.UI.Office
{
    public class OfficeShell : Screen
    {
        public OfficeShell(IDisplayServices services, IScreenFactory factory) : base(services)
        {
            this.factory = factory;
        }

        private readonly IScreenFactory factory;

        #region IoC

        public IMonitoringService Monitoring { get; set; }

        #endregion IoC

        public string Environment { get; set; }

        // public void Handle(TechnicalInformationsMessage message)
        // {
        // ShowAsDialog(message);
        // }
        protected void ShowAsDialog<T>(NavigationMessage<T> message) where T : Screen
        {
            T screen = null;
            try
            {
                screen = factory.CreateScreen<T>();
                Windows.ShowAsPopup(screen);
            }
            catch (Exception)
            {
                factory.Release(screen);
                throw;
            }
        }

        protected override void OnActivate()
        {
            DisplayName = Monitoring.ApplicationName;

            base.OnActivate();
        }
    }
}