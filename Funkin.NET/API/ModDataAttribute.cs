using System;

namespace Funkin.NET.API
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ModDataAttribute : Attribute
    {
        public Type ModType { get; }

        public string ModId { get; }

        public ModDataAttribute(Type modType, string? modId = null)
        {
            ModType = modType;
            ModId = modId ?? modType.Name;
        }
    }
}