namespace Funkin.Game.API
{
    /// <summary>
    ///     An outline containing the core data of a mod, the bare minimum for loading a mod.
    /// </summary>
    public interface IMod
    {
        /// <summary>
        ///     Called the very moment the mod is constructed and loaded, prior to any base game features being loaded. <br />
        ///     You should register any injection listeners here.
        /// </summary>
        void Load();

        /// <summary>
        ///     Simple helper method for constructing mod-owned identifiers.
        /// </summary>
        /// <param name="content">The content's name.</param>
        /// <returns>A mod-owned identifier.</returns>
        Identifier MakeIdentifier(string content);
    }
}
