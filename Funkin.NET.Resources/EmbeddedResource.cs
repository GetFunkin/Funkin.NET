using System;
using System.IO;
using System.Reflection;

namespace Funkin.NET.Resources
{
    /// <summary>
    ///     Utilities for retrieving embedded resources.
    /// </summary>
    public static class EmbeddedResource
    {
        /// <summary>
        ///     Sanitizes and streamlines embedded resource paths.
        /// </summary>
        /// <param name="path">The path to sanitize.</param>
        /// <param name="assembly">The assembly to use. Defaults to <see cref="Assembly"/></param>.
        /// <returns>A sanitized path with the assembly name added to the beginning if it was not already there.</returns>
        public static string SanitizeForEmbeds(string path, Assembly assembly = null)
        {
            // replace '/' and '\' with '.'
            path = path.Replace(Path.DirectorySeparatorChar, '.')
                .Replace('/', '.').Replace('\\', '.');

            // if no assembly was set, use ours
            assembly ??= PathHelper.Assembly;

            // if the assembly name doesn't exist, cry about it through an exception
            // if the path doesn't start with the assembly name, add it to the beginning
            if (!path.StartsWith(assembly.GetName().Name ?? throw new NullReferenceException("Assembly name was null.")))
                path = $"{assembly.GetName().Name}.{path}";

            return path;
        }
    }
}