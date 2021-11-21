using System;

namespace Funkin.NET.API.Exceptions
{
    public class ModDataMissingException : Exception
    {
        public ModDataMissingException(IMod mod) : base(
            $"Mod with type name \"{mod.GetType().Name}\" is not decorated with a {nameof(ModDataAttribute)}!"
        )
        {
        }
    }
}