using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Funkin.NET.Core.API;
using Funkin.NET.Intermediary.Utilities;

namespace Funkin.NET.Core.ManiaContent
{
    public sealed class ManiaMod : IMod
    {
        public Assembly Assembly
        {
            get => GetType().Assembly;

            set => throw new NotSupportedException("Tried to set assembly to: " + value);
        }

        public Dictionary<Identity, IIdentifiable> Content { get; } = new CastDictionary<Identity, IIdentifiable>();

        private CastDictionary<Identity, IIdentifiable> CastDict => Content as CastDictionary<Identity, IIdentifiable>;

        public ManiaMod()
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

        public T GetContent<T>(Identity identity) where T : IIdentifiable => CastDict.As<T>(identity);

        public IIdentifiable GetContent(Identity identity) => Content[identity];
    }
}