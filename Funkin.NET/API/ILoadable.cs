namespace Funkin.NET.API
{
    /// <summary>
    ///     Represents a singleton class which will be automatically loaded upon the loading of its associated mod. <br />
    ///     Contains an identifier and an <see cref="IMod"/> instance.
    /// </summary>
    public interface ILoadable
    {
        /// <summary>
        ///     The <see cref="IMod"/> instance that loaded this <see cref="ILoadable"/>.
        /// </summary>
        IMod Mod { get; internal set; }

        /// <summary>
        ///     The unique identifier used to identify your piece of content.
        /// </summary>
        Identifier Identifier { get; }

        /// <summary>
        ///     Invoked upon mod load.
        /// </summary>
        void Load();
    }
}