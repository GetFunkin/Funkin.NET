using System.Collections.Generic;

namespace Funkin.Game.API
{
    public interface IRegistry
    {
        /// <summary>
        ///     Registers an <paramref name="entry"/> to the registry.
        /// </summary>
        /// <param name="id">The entry's identifier.</param>
        /// <param name="entry">The singleton entry instance.</param>
        /// <returns>The now-registered entry.</returns>
        object Register(Identifier id, object entry);

        /// <summary>
        ///     Retrieves a registered entry based on its <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The entry's identifier.</param>
        /// <returns>The singleton instance registered to the registry.</returns>
        object Get(Identifier id);

        /// <summary>
        ///     Retrieves an identifier belonging to an <see cref="entry"/>.
        /// </summary>
        /// <param name="entry">The singleton instance registered to the registry.</param>
        /// <returns>The identifier of the singleton instance.</returns>
        Identifier GetId(object entry);

        /// <summary>
        ///     Returns an enumerable collection of entries.
        /// </summary>
        /// <returns>An enumerable collection of entries for any immutable use.</returns>
        IEnumerable<object> GetEntries();
    }

    // Discuss whether unregistering entries should be allowed.
    // ...probably not?
    /// <summary>
    ///     Core registry interface.
    /// </summary>
    /// <typeparam name="T">The type of object to handle registering.</typeparam>
    public interface IRegistry<T> : IRegistry
    {
        /// <summary>
        ///     Registers an <paramref name="entry"/> to the registry.
        /// </summary>
        /// <param name="id">The entry's identifier.</param>
        /// <param name="entry">The singleton entry instance.</param>
        /// <returns>The now-registered entry.</returns>
        T Register(Identifier id, T entry);

        /// <summary>
        ///     Retrieves a registered entry based on its <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The entry's identifier.</param>
        /// <returns>The singleton instance registered to the registry.</returns>
        new T Get(Identifier id);

        /// <summary>
        ///     Retrieves an identifier belonging to an <see cref="entry"/>.
        /// </summary>
        /// <param name="entry">The singleton instance registered to the registry.</param>
        /// <returns>The identifier of the singleton instance.</returns>
        Identifier GetId(T entry);

        /// <summary>
        ///     Returns an enumerable collection of entries.
        /// </summary>
        /// <returns>An enumerable collection of entries for any immutable use.</returns>
        new IEnumerable<T> GetEntries();
    }
}
