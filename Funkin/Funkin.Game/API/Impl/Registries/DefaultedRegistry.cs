using System;

namespace Funkin.Game.API.Impl.Registries
{
    /// <summary>
    ///     A basic registry that can return a default value if an identifier does not have an associated entry.
    /// </summary>
    /// <typeparam name="T">The type of object to handle registering.</typeparam>
    public abstract class DefaultedRegistry<T> : BasicRegistry<T> where T : notnull
    {
        public readonly Func<T> ResolveDefault;

        protected DefaultedRegistry(Func<T> resolveDefault)
        {
            ResolveDefault = resolveDefault;
        }

        public override T Get(Identifier id) => !IdentifierMap.ContainsKey(id) ? ResolveDefault() : base.Get(id);
    }
}
