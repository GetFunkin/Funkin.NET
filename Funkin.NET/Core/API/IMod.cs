using System.Collections.Generic;
using System.Reflection;

namespace Funkin.NET.Core.API
{
    public interface IMod : IIdentifiable
    {
        Assembly Assembly { get; set; }

        Identity IIdentifiable.Identity => new(GetType().Name, "mod");

        Dictionary<Identity, IIdentifiable> Content { get; }

        void AddContent(IIdentifiable content);

        T GetContent<T>() where T : IIdentifiable;

        T GetContent<T>(Identity identity) where T : IIdentifiable;

        IIdentifiable GetContent(Identity identity);
    }
}