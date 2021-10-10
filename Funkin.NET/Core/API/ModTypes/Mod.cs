using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Funkin.NET.Core.API.ModTypes
{
    public abstract class Mod : IMod
    {
        public Assembly Assembly { get; set; }

        public Dictionary<Identity, IIdentifiable> Content { get; } = new();

        protected Mod()
        {
            Content.Add(((IIdentifiable) this).Identity, this);
        }

        public void AddContent(IIdentifiable content)
        {
            if (Content.ContainsKey(content.Identity))
                throw new Exception($"Content with identity {content.Identity} already added!");

            if (content is IModType modContent)
                modContent.Mod = this;

            Content[content.Identity] = content;
        }

        public T GetContent<T>() where T : IIdentifiable =>
            (T) Content.Values.FirstOrDefault(x => x.GetType() == typeof(T));

        public T GetContent<T>(Identity identity) where T : IIdentifiable => (T) GetContent(identity);

        public IIdentifiable GetContent(Identity identity) => Content[identity];
    }
}