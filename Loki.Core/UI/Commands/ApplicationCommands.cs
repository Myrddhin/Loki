using System;
using Loki.Common;
using Loki.IoC;
using Loki.UI;

namespace Loki.Commands
{
    public static class ApplicationCommands
    {
        static ApplicationCommands()
        {
            if (!Toolkit.UI.Windows.DesignMode)
            {
                CreateCommands(Toolkit.IoC.DefaultContext);
            }
            else
            {
                CreateCommands(null);
            }
        }

        private static class Names
        {
            public const string ERROR = "Application.Error";
            public const string EXIT = "Application.Exit";
            public const string EXPORT = "Application.Export";
            public const string MESSAGE = "Application.Message";
            public const string OPEN = "Application.Open";
            public const string REFRESH = "Application.Refresh";
            public const string SAVE = "Application.Save";
            public const string SEARCH = "Application.Search";
            public const string START = "Application.Start";
            public const string UPDATESTATUS = "Application.UpdateStatus";
            public const string WARNING = "Application.Warning";
        }

        public static void CreateCommands(IObjectContext context)
        {
            Func<string, ICommand> builder = s => new LokiRelayCommand(() => { });
            if (context != null)
            {
                var commands = context.Get<ICommandComponent>();
                builder = name => commands.GetCommand(name);
            }

            Export = builder(Names.EXPORT);
            Open = builder(Names.OPEN);
            Refresh = builder(Names.REFRESH);
            Save = builder(Names.SAVE);
            UpdateStatus = builder(Names.UPDATESTATUS);
            Search = builder(Names.SEARCH);
        }

        public static ICommand Export { get; private set; }

        public static ICommand Open { get; private set; }

        public static ICommand Refresh { get; private set; }

        public static ICommand Save { get; private set; }

        public static ICommand Search { get; private set; }

        public static ICommand UpdateStatus { get; private set; }
    }
}