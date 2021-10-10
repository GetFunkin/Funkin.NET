using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Funkin.NET.Core.API.ModTypes
{
    public interface IMod : IIdentifiable
    {
        Assembly Assembly { get; set; }

        Identity IIdentifiable.Identity => new(GetType().Name, GetType().Name);

        Dictionary<Identity, IIdentifiable> Content { get; }

        void AddContent(IIdentifiable content);

        T GetContent<T>() where T : IIdentifiable;

        T GetContent<T>(Identity identity) where T : IIdentifiable;

        IIdentifiable GetContent(Identity identity);

        public IEnumerable<Type> GetLoadableContent() => Assembly.GetTypes().Where(x =>
            typeof(ILoadable).IsAssignableFrom(x) &&
            !x.IsAbstract &&
            x.GetConstructor(Array.Empty<Type>()) != null);
    }
}