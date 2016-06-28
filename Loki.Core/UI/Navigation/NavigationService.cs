using System;
using System.Collections.Concurrent;

using Loki.Common;
using Loki.UI.Commands;

namespace Loki.UI
{
    internal class NavigationService : BaseObject, INavigationService, IDisposable
    {
        private readonly ICoreServices services;

        private readonly IThreadingContext threading;

        private readonly ICommandComponent commands;

        private readonly ConcurrentDictionary<string, Func<INavigationMessage>> routes;

        private readonly ICommandHandler handler;

        private readonly ICommand navigationCommand;

        public NavigationService(ICoreServices services, IThreadingContext threading, ICommandComponent commands)
            : base(services.Diagnostics)
        {
            this.services = services;
            this.threading = threading;
            this.commands = commands;

            routes = new ConcurrentDictionary<string, Func<INavigationMessage>>();

            navigationCommand = commands.GetCommand(ApplicationCommands.Names.NAVIGATE);

            handler = commands.CreateHandler(navigationCommand, Navigate_CanExecute, Navigate_Execute);
        }

        private void Navigate_CanExecute(object sender, CanExecuteCommandEventArgs e)
        {
            var route = e.Parameter.SafeToString();
            e.CanExecute |= routes.ContainsKey(route);
        }

        private void Navigate_Execute(object sender, CommandEventArgs e)
        {
            var route = e.Parameter.SafeToString();
            Navigate(route);
        }

        public void Navigate(string route)
        {
            Func<INavigationMessage> messageCreator;
            if (!routes.TryGetValue(route, out messageCreator))
            {
                Log.WarnFormat("Unknown route : {0}", route);
                return;
            }

            var message = messageCreator();
            Log.DebugFormat("Navigating to {0}", route);
            message.RouteName = route;
            services.Messages.PublishOnUIThread(threading, message);
        }

        public void AddRoute(string route, Func<INavigationMessage> message)
        {
            routes.AddOrUpdate(route, message, (r, m) => message);
        }

        public void Dispose()
        {
            commands.RemoveHandler(navigationCommand, handler);
        }
    }
}