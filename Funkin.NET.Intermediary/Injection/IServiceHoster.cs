using System;
using Microsoft.Extensions.DependencyInjection;

namespace Funkin.NET.Intermediary.Injection
{
    /// <summary>
    ///     Provides simple properties for hosting services.
    /// </summary>
    public interface IServiceHoster
    {
        /// <summary>
        ///     Service collection.
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        ///     Provider from the service collection (<see cref="Services"/>).
        /// </summary>
        IServiceProvider ServiceProvider { get; }
    }
}