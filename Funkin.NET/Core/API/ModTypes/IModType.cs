namespace Funkin.NET.Core.API.ModTypes
{
    public interface IModType : ILoadable
    {
        IMod Mod { get; set; }
    }
}