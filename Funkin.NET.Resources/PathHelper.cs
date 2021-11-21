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
        public static Assembly Assembly => typeof(PathHelper).Assembly;

        // replaces '\' and the directory separator character with '/'
        public static string SanitizeForResources(string path) =>
            path.Replace('\\', '/').Replace(Path.DirectorySeparatorChar, '/');

        // replaces '\' and '/' with the directory separator character
        public static string SanitizeForComputer(string path) =>
            path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);

        public static string GetPath(string path, bool sanitize, bool includeStart, string start)
        {
            StringBuilder builder = new();

            if (includeStart)
                builder.Append(start);

            builder.Append(path);

            return sanitize ? SanitizeForResources(builder.ToString()) : builder.ToString();
        }
    }

    public static class Fonts
    {
        public static string GetTexture(string path, bool sanitize = true, bool includeStart = false) =>
            PathHelper.GetPath(path, sanitize, includeStart, "Fonts");

        public static class Torus
        {
            private static string Get(string path) => GetTexture("Torus/" + path);

            public static string TorusBold => Get("Torus-Bold");

            public static string TorusLight => Get("Torus-Light");

            public static string TorusRegular => Get("Torus-Regular");

            public static string TorusSemiBold => Get("Torus-SemiBold");
        }
    }

    public static class Textures
    {
        public static string GetTexture(string path, bool sanitize = true, bool includeStart = false) =>
            PathHelper.GetPath(path, sanitize, includeStart, "Textures");

        public static class Backgrounds
        {
            public static string DesaturatedMenu => GetTexture("Backgrounds/DesaturatedMenu");
        }

        public static class General
        {
            public static class Cursors
            {
                public static string Cursor => GetTexture("General/Cursors/Cursor");
            }
        }
    }

    public static class Tracks
    {
        public static string GetTrack(string path, bool sanitize = true, bool includeStart = false) =>
            PathHelper.GetPath(path, sanitize, includeStart, "Tracks");

        public static class Menu
        {
            public static string FreakyMenu => GetTrack("Menu/FreakyMenu.ogg");
        }
    }
}