namespace Loki.UI.Commands
{
    public class ApplicationCommands
    {
        private readonly ICommandComponent commandService;

        protected ICommand GetCommand(string name)
        {
            return commandService.GetCommand(name);
        }

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
            public const string NAVIGATE = "Application.Navigate";
            public const string SAVE = "Application.Save";
            public const string SEARCH = "Application.Search";
            public const string START = "Application.Start";
            public const string UPDATESTATUS = "Application.UpdateStatus";
            public const string WARNING = "Application.Warning";
        }

        public ICommand Navigate
        {
            get
            {
                return GetCommand(Names.NAVIGATE);
            }
        }

        public ICommand Export
        {
            get
            {
                return GetCommand(Names.EXPORT);
            }
        }

        public ICommand Open
        {
            get
            {
                return GetCommand(Names.OPEN);
            }
        }

        public ICommand Refresh
        {
            get
            {
                return GetCommand(Names.REFRESH);
            }
        }

        public ICommand Save
        {
            get
            {
                return GetCommand(Names.SAVE);
            }
        }

        public ICommand Search
        {
            get
            {
                return GetCommand(Names.SEARCH);
            }
        }

        public ICommand UpdateStatus
        {
            get
            {
                return GetCommand(Names.UPDATESTATUS);
            }
        }
    }
}