namespace Loki.UI.Commands
{
    public class ApplicationCommands
    {
        private readonly ICommandComponent commandService;

        public ApplicationCommands(ICommandComponent commandService)
        {
            this.commandService = commandService;
        }

        public static class Names
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

        public ICommand Export
        {
            get
            {
                return commandService.GetCommand(Names.EXPORT);
            }
        }

        public ICommand Open
        {
            get
            {
                return commandService.GetCommand(Names.OPEN);
            }
        }

        public ICommand Refresh
        {
            get
            {
                return commandService.GetCommand(Names.REFRESH);
            }
        }

        public ICommand Save
        {
            get
            {
                return commandService.GetCommand(Names.SAVE);
            }
        }

        public ICommand Search
        {
            get
            {
                return commandService.GetCommand(Names.SEARCH);
            }
        }

        public ICommand UpdateStatus
        {
            get
            {
                return commandService.GetCommand(Names.UPDATESTATUS);
            }
        }
    }
}