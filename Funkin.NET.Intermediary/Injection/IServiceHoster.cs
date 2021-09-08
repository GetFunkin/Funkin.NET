using System;
using Microsoft.Extensions.DependencyInjection;

namespace Funkin.NET.Intermediary.Injection
{
    public interface IServiceHoster
    {
        IServiceCollection Services { get; }

        IServiceProvider ServiceProvider { get; }
    }
}