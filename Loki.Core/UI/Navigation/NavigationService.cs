using System;
using System.Collections.Concurrent;

using Loki.Common;

namespace Loki.UI
{
    internal class NavigationService : BaseObject, INavigationService
    {
        private readonly ICoreServices services;

        private readonly IThreadingContext threading;

        private readonly ConcurrentDictionary<string, Func<INavigationMessage>> routes;

        public NavigationService(ICoreServices services, IThreadingContext threading)
            : base(services.Logger, services.Error)
        {
            this.services = services;
            this.threading = threading;

            routes = new ConcurrentDictionary<string, Func<INavigationMessage>>();
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
            services.Messages.PublishOnUIThread(threading, message);
        }

        public void AddRoute(string route, Func<INavigationMessage> message)
        {
            routes.AddOrUpdate(route, message, (r, m) => message);
        }
    }
}