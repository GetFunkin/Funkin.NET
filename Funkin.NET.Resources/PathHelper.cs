using System.IO;
using System.Reflection;
using System.Text;

namespace Funkin.NET.Resources
{
    /// <summary>
    ///     Path retrieval utilities.
    /// </summary>
    public static class PathHelper
    {
        public static class Json
        {
            public static string GetPath(string path, bool sanitize = true)
            {
                StringBuilder builder = new();

                if (!path.StartsWith("Json/"))
                    builder.Append("Json/");

                builder.Append(path);

                if (!path.EndsWith(".json"))
                    builder.Append(".json");

                return sanitize ? SanitizeForComputer(builder.ToString()) : builder.ToString();
            }
        }

        public static Assembly Assembly => typeof(PathHelper).Assembly;

        public static string SanitizeForResources(string path) =>
            path.Replace('\\', '/').Replace(Path.DirectorySeparatorChar, '/');

        public static string SanitizeForComputer(string path) =>
            path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);

        public static string GetTrack(string path, bool sanitize = true, bool includeTrack = false) =>
            GetPath(path, sanitize, includeTrack, "Track/");

        public static string GetTexture(string path, bool sanitize = true, bool includeTextures = false) =>
            GetPath(path, sanitize, includeTextures, "Textures/");

        public static string GetSample(string path, bool sanitize = true, bool includeSamples = false) =>
            GetPath(path, sanitize, includeSamples, "Samples/");

        public static string GetFont(string path, bool sanitize = true, bool includeFonts = true) =>
            GetPath(path, sanitize, includeFonts, "Fonts/");

        private static string GetPath(string path, bool sanitize, bool includeStart, string start)
        {
            StringBuilder builder = new();

            if (includeStart)
                builder.Append(start);

            builder.Append(path);

            return sanitize ? SanitizeForResources(builder.ToString()) : builder.ToString();
        }
    }
}