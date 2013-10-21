using System;
using System.Collections.Generic;
using Castle.Windsor;

namespace CloudDataAnalytics.Shared.DomainEvents
{
    public static class DomainEventDispatcher
    {
        [ThreadStatic]
        private static List<Delegate> _actions;

        private static IWindsorContainer Container { get; set; }    
        public static void SetContainer(IWindsorContainer container) { Container = container; }

        public static void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            if (_actions == null)
            {
                _actions = new List<Delegate>();
            }
            _actions.Add(callback);
        }

        public static void Register<T>(IDomainEventHandler<T> handler) where T : IDomainEvent
        {
            Register<T>(handler.Handle);
        }

        public static void ClearCallbacks()
        {
            _actions = null;
        }

        public static void Raise<T>(T args) where T : IDomainEvent
        {
            if (Container != null)
            {
                foreach (var handler in Container.ResolveAll<IDomainEventHandler<T>>())
                {
                    handler.Handle(args);
                }
            }

            if (_actions == null) return;
            foreach (var action in _actions)
                if (action is Action<T>)
                    ((Action<T>)action)(args);
        }
    }

}
