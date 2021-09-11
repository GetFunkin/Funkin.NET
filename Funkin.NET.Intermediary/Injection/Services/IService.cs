namespace Funkin.NET.Intermediary.Injection.Services
{
    /// <summary>
    ///     Indicates a service should be loaded at runtime by an <see cref="IntermediaryGame"/>. <br />
    ///     Assumes a type should be constructed by the default constructor. <br />
    ///     Define a static <c>CreateProvider</c> method with no parameters and make it <see langword="return"/> your type for custom behavior.
    /// </summary>
    public interface IService
    {
        public const string ProviderMethod = "CreateProvider";
    }
}