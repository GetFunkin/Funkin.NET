namespace Funkin.NET.Core.API
{
    public interface IModType : IIdentifiable
    {
        IMod Mod { get; set; }
    }
}