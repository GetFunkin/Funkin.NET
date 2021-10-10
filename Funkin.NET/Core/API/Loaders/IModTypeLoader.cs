using System;
using Funkin.NET.Core.API.ModTypes;

namespace Funkin.NET.Core.API.Loaders
{
    public interface IModTypeLoader
    {
        bool CanLoadType(Type type);

        void DoLoad(IMod mod, object type);
    }
}