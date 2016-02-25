using System;
using System.Globalization;
using System.Reflection;
using System.Security.Principal;

using Loki.Common;

namespace Loki.UI.Office
{
    internal class MonitoringService : LoggableObject, IMonitoringService
    {
        public MonitoringService(ILoggerComponent logger, IMessageComponent messageComponent) : base(logger)
        {
            this.MessageBus = messageComponent;
            this.Logger = logger;
        }

        #region IoC

        public IMessageComponent MessageBus { get; set; }

        public ILoggerComponent Logger { get; set; }

        #endregion IoC

        protected string GetAttributeValue<TAttr>(Assembly assembly, Func<TAttr, string> resolveFunc) where TAttr : Attribute
        {
            object[] attributes = assembly.GetCustomAttributes(typeof(TAttr), false);
            if (attributes.Length > 0)
            {
                return resolveFunc((TAttr)attributes[0]);
            }

            return string.Empty;
        }

        public string Environment { get; set; }

        public string ApplicationName { get; set; }

        public string ApplicationVersion { get; set; }

        public string ApplicationFullVersion { get; set; }

        public string Copyright { get; set; }

        public IIdentity User
        {
            get
            {
                return WindowsIdentity.GetCurrent();
            }
        }

        public void Handle(ActionMessage message)
        {
            if (message != null)
            {
                // Log usage
                Log.InfoFormat("Used fonction : {0}", message.Action);
            }
        }

        public void Handle(INavigationMessage message)
        {
            if (!string.IsNullOrEmpty(message.RouteName))
            {
                // Log usage
                Log.InfoFormat("Used fonction : {0}", message.RouteName);
            }
        }

        public void Initialize(Type mainType)
        {
            var mainAssembly = mainType.Assembly;

            var name = mainAssembly.GetName();

            // get assembly metadata
            ApplicationVersion = string.Format(CultureInfo.InvariantCulture, "Version {0}.{1}", name.Version.Major, name.Version.Minor);
            ApplicationFullVersion = name.Version.ToString();
            Copyright = GetAttributeValue<AssemblyCopyrightAttribute>(mainAssembly, a => a.Copyright);
            ApplicationName = GetAttributeValue<AssemblyDescriptionAttribute>(mainAssembly, a => a.Description);

            MessageBus.Subscribe(this);
        }

        public string SupportMail { get; set; }

        public string SmtpServer { get; set; }

        public string LogFileName
        {
            get
            {
                if (Logger == null)
                {
                    return null;
                }

                return this.Logger.LogFileName;
            }
        }

        private void NotifySupportCore(string messageFormat, string messageBody)
        {
            try
            {
                // UserPrincipal userdata = ADMethodsAccountManagement.GetUser(User);
                // MailMessage message = new System.Net.Mail.MailMessage();
                // message.Subject = string.Format(CultureInfo.InvariantCulture, messageFormat, ApplicationName, ApplicationFullVersion, Environment);
                // message.Body = messageBody;

                //// copy log file to avoid locks
                // var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                // Directory.CreateDirectory(tempDir);
                // var tempFile = Path.Combine(tempDir, Path.GetFileName(Logger.LogFileName));
                // File.Copy(Logger.LogFileName, tempFile, true);
                // message.Attachments.Add(new Attachment(tempFile));

                // if (Environment == "PROD" || string.IsNullOrEmpty(userdata.EmailAddress))
                // {
                // // PROD : use official support list.
                // string[] supportMail = SupportMail.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                // foreach (var address in supportMail)
                // {
                // message.To.Add(address);
                // }
                // }
                // else
                // {
                // // Other : use current user mail.
                // message.To.Add(userdata.EmailAddress);
                // }

                // if (!string.IsNullOrEmpty(userdata.EmailAddress))
                // {
                // message.From = new MailAddress(userdata.EmailAddress);
                // }

                // SmtpClient client = new SmtpClient(SmtpServer);
                // client.DeliveryMethod = SmtpDeliveryMethod.Network;

                // client.Send(message);
            }
            catch (Exception ex)
            {
                Log.Error("Error when sending error mail", ex);
            }
        }

        public void NotifySupport(Exception error)
        {
            NotifySupportCore("Unexpected error in {0} - {1} - {2}", error.ToString());
        }

        public void NotifySupport(string message)
        {
            NotifySupportCore("Notification from {0} - {1} - {2}", message);
        }
    }
}