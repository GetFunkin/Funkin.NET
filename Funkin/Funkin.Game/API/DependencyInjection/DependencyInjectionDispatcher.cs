using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Funkin.Game.API.DependencyInjection
{
    /// <summary>
    ///     Simple utility class for dispatching dependency injection events.
    /// </summary>
    public sealed class DependencyInjectionDispatcher
    {
        /// <summary>
        ///     A delegate for injection dispatch listeners.
        /// </summary>
        public delegate object InjectionDispatch(DependencyInjectionContext context, ref object? previousObject);

        private readonly List<InjectionDispatch> listeners = new();

        public ReadOnlyCollection<InjectionDispatch> Listeners => listeners.AsReadOnly();

        public void Listen(InjectionDispatch listener)
        {
            listeners.Add(listener);
        }

        public object Dispatch(DependencyInjectionContext context, object defaultObject)
        {
            object? returnValue = defaultObject;

            foreach (InjectionDispatch listener in listeners)
                listener(context, ref returnValue);

            return returnValue ?? defaultObject;
        }
    }
}
