using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Funkin.NET.API.Exceptions;

namespace Funkin.NET.API
{
    /// <summary>
    ///     The core implementation of <see cref="IMod"/>. <br />
    ///     Contains everything you need for simple inheritance.
    /// </summary>
    public abstract class Mod : IMod, ILoadable
    {
        public Assembly Assembly { get; }

        public ModDataAttribute Data { get; }

        IMod ILoadable.Mod
        {
            get => this;
            set { }
        }

        public Identifier Identifier => new(Data.ModId, "mod");

        protected Mod()
        {
            Data = GetType().GetCustomAttribute<ModDataAttribute>() ?? throw new ModDataMissingException(this);
            Assembly = GetType().Assembly;
        }

        public virtual void Load()
        {
        }

        public virtual IEnumerable<ILoadable> ResolveLoadables()
        {
            foreach (Type type in Assembly.GetTypes().Where(
                         x => !x.IsInterface && !x.IsAbstract && x.GetConstructor(Type.EmptyTypes) != null
                     ))
            {
                if (Activator.CreateInstance(type) is not ILoadable loadable)
                    throw new LoadableException(type);

                loadable.Load();

                yield return loadable;
            }
        }
    }
}