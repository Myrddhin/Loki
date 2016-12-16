﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Loki.Common.Messages
{
    /// <summary>
    /// Enables loosely-coupled publication of and subscription to events.
    /// </summary>
    public class MessageBus : IMessageBus
    {
        private readonly List<Handler> handlers = new List<Handler>();

        /// <summary>
        /// Searches the subscribed handlers to check if we have a handler for
        /// the message type supplied.
        /// </summary>
        /// <param name="messageType">
        /// The message type to check with.
        /// </param>
        /// <returns>
        /// True if any handler is found, false if not.
        /// </returns>
        public bool HandlerExistsFor(Type messageType)
        {
            lock (handlers)
            {
                return this.handlers.Any(handler => handler.Handles(messageType) & !handler.IsDead);
            }
        }

        /// <summary>
        /// Subscribes an instance to all events declared through implementations of <see cref="IHandle{T}"/>
        /// </summary>
        /// <param name="subscriber">
        /// The instance to subscribe for event publication.
        /// </param>
        public virtual void Subscribe(object subscriber)
        {
            if (subscriber == null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }

            lock (handlers)
            {
                if (handlers.Any(x => x.Matches(subscriber)))
                {
                    return;
                }

                handlers.Add(new Handler(subscriber));
            }
        }

        /// <summary>
        /// Unsubscribes the instance from all events.
        /// </summary>
        /// <param name="subscriber">
        /// The instance to unsubscribe.
        /// </param>
        public virtual void Unsubscribe(object subscriber)
        {
            if (subscriber == null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }

            lock (handlers)
            {
                var found = handlers.FirstOrDefault(x => x.Matches(subscriber));

                if (found != null)
                {
                    handlers.Remove(found);
                }
            }
        }

        /// <summary>
        /// Publishes a message.
        /// </summary>
        /// <param name="message">
        /// The message instance.
        /// </param>
        /// <param name="marshal">
        /// Allows the publisher to provide a custom thread marshaller for the message publication.
        /// </param>
        public virtual void Publish(object message, Action<Action> marshal)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (marshal == null)
            {
                throw new ArgumentNullException(nameof(marshal));
            }

            Handler[] toNotify;
            lock (handlers)
            {
                toNotify = handlers.ToArray();
            }

            marshal(() =>
            {
                var messageType = message.GetType();

                var dead = toNotify
                    .Where(handler => !handler.Handle(messageType, message))
                    .ToList();

                if (dead.Any())
                {
                    lock (handlers)
                    {
                        dead.Apply(x => handlers.Remove(x));
                    }
                }
            });
        }

        private class Handler
        {
            private readonly WeakReference reference;
            private readonly Dictionary<Type, MethodInfo> supportedHandlers = new Dictionary<Type, MethodInfo>();

            public bool IsDead => this.reference.Target == null;

            public Handler(object handler)
            {
                reference = new WeakReference(handler);

                var interfaces = handler.GetType().GetInterfaces()
                    .Where(x => typeof(IHandle).IsAssignableFrom(x) && x.GetTypeInfo().IsGenericType);

                foreach (var @interface in interfaces)
                {
                    var type = @interface.GetGenericArguments()[0];
                    var method = @interface.GetMethod("Handle");
                    supportedHandlers[type] = method;
                }
            }

            public bool Matches(object instance)
            {
                return reference.Target == instance;
            }

            public bool Handle(Type messageType, object message)
            {
                var target = reference.Target;
                if (target == null)
                {
                    return false;
                }

                foreach (var pair in supportedHandlers)
                {
                    if (pair.Key.IsAssignableFrom(messageType))
                    {
                        pair.Value.Invoke(target, new[] { message });
                    }
                }

                return true;
            }

            public bool Handles(Type messageType)
            {
                return supportedHandlers.Any(pair => pair.Key.IsAssignableFrom(messageType));
            }
        }
    }
}