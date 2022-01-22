using System;

namespace Funkin.Game.API.DependencyInjection
{
    /// <summary>
    ///     Provides context for dispatched injection requests.
    /// </summary>
    public readonly struct DependencyInjectionContext
    {
        /// <summary>
        ///     The base expected type.
        /// </summary>
        public readonly Type BaseType;

        /// <summary>
        ///     The unique contextual identifier.
        /// </summary>
        public readonly Identifier Identifier;

        public DependencyInjectionContext(Type baseType, Identifier identifier)
        {
            BaseType = baseType;
            Identifier = identifier;
        }
    }
}
