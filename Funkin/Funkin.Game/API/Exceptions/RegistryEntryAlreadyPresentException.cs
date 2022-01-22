using System;

namespace Funkin.Game.API.Exceptions
{
    public class RegistryEntryAlreadyPresentException : Exception
    {
        public RegistryEntryAlreadyPresentException(Identifier identifier, Exception? innerException = null)
            : base(getMessage(identifier), innerException)
        {
        }

        private static string getMessage(Identifier identifier) => $"Attempted to register entry despite it already existing (\"{identifier}\")!";
    }
}
