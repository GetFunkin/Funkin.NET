using System;
using System.Collections.Generic;
using System.Linq;
using Funkin.Game.API.Exceptions;

namespace Funkin.Game.API.Impl.Registries
{
    /// <summary>
    ///     A basic registry implementation.
    /// </summary>
    /// <typeparam name="T">The type of object to handle registering.</typeparam>
    public abstract class BasicRegistry<T> : IRegistry<T> where T : notnull
    {
        protected Dictionary<Identifier, T> IdentifierMap = new();

        public virtual T Register(Identifier id, T entry)
        {
            if (IdentifierMap.ContainsKey(id))
                throw new RegistryIdentifierAlreadyPresentException(id);

            if (IdentifierMap.ContainsValue(entry))
                throw new RegistryEntryAlreadyPresentException(id);

            IdentifierMap.Add(id, entry);

            return entry;
        }

        public virtual T Get(Identifier id)
        {
            if (IdentifierMap.ContainsKey(id))
                return IdentifierMap[id];

            throw new IndexOutOfRangeException($"Identifier \"{id}\" did not have an associated, registered entry.");
        }

        public virtual Identifier GetId(T entry)
        {
            if (IdentifierMap.ContainsValue(entry))
                return IdentifierMap.First(x => x.Value.Equals(entry)).Key;

            throw new IndexOutOfRangeException("Could not resolve identifier for entry.");
        }

        public virtual IEnumerable<T> GetEntries() => IdentifierMap.Values;

        #region Genericless

        object IRegistry.Register(Identifier id, object entry) => Register(id, (T)entry);

        Identifier IRegistry.GetId(object entry) => GetId((T)entry);

        IEnumerable<object> IRegistry.GetEntries() => GetEntries().Cast<object>();

        object IRegistry.Get(Identifier id) => Get(id);

        #endregion
    }
}
