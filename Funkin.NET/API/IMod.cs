using System.Reflection;

namespace Funkin.NET.API
{
    public interface IMod
    {
        Assembly Assembly { get; }

        ModDataAttribute Data { get; }
    }
}