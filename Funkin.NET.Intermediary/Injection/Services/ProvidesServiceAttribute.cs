using System;
using JetBrains.Annotations;

namespace Funkin.NET.Intermediary.Injection.Services
{
    /// <summary>
    ///     Used along-side <see cref="IService"/>. See <see cref="IService"/>'s summary for its usage.
    /// </summary>
    [MeansImplicitUse]
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ProvidesServiceAttribute : Attribute
    {
    }
}