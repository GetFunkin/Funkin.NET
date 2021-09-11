namespace Funkin.NET.Intermediary.Injection.Services
{
    /// <summary>
    ///     Indicates a service should be loaded at runtime by an <see cref="IntermediaryGame"/>. <br />
    ///     Assumes a type should be constructed by the default constructor. <br />
    ///     Mark a static, parameterless method with <see cref="ProvidesServiceAttribute"/> to use as a constructor alternatively.
    /// </summary>
    public interface IService
    {
    }
}