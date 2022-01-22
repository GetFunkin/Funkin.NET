using System;

namespace Funkin.Game.API.Exceptions
{
    public class RegistryIdentifierAlreadyPresentException : Exception
    {
        public RegistryIdentifierAlreadyPresentException(Identifier identifier, Exception? innerException = null)
            : base(getMessage(identifier), innerException)
        {
        }

        private static string getMessage(Identifier identifier) => $"Attempted to register entry with an identifier (\"{identifier}\") despite a previous entry already using that identifier!";
    }
}
