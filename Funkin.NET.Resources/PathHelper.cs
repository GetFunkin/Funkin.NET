using System;
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
                assembly ??= Assembly;

                // if the assembly name doesn't exist, cry about it through an exception
                // if the path doesn't start with the assembly name, add it to the beginning
                if (!path.StartsWith(assembly.GetName().Name ?? throw new NullReferenceException("Assembly name was null.")))
                    path = $"{assembly.GetName().Name}.{path}";

                return path;
            }
        }

        public static class Font
        {
            public static string Vcr => GetFont("VCR/VCR");

            public static string Funkin => GetFont("Funkin/Funkin");

            public static string TorusRegular => GetFont("Torus/Torus-Regular");

            public static string TorusLight => GetFont("Torus/Torus-Light");

            public static string TorusSemiBold => GetFont("Torus/Torus-SemiBold");

            public static string TorusBold => GetFont("Torus/Torus-Bold");
        }

        public static class Json
        {
            public static string IntroTextPath => GetPath("Json/IntroText");

            public static string IntroTextJson => GetEmbeddedJson(IntroTextPath);

            /// <summary>
            ///     Retrieves an embedded JSON by opening a stream (sanitized with <see cref="EmbeddedResource.SanitizeForEmbeds"/>) and initializing a <see cref="StreamReader"/>.
            /// </summary>
            /// <param name="path">The embedded path.</param>
            /// <param name="assembly">The assembly to retrieve from. Defaults to <see cref="Assembly"/>.</param>
            /// <returns>The contents of the Json at the specified path.</returns>
            public static string GetEmbeddedJson(string path, Assembly assembly = null)
            {
                using Stream stream = Assembly.GetManifestResourceStream(EmbeddedResource.SanitizeForEmbeds(path, assembly));
                using TextReader reader = new StreamReader(stream ?? throw new NullReferenceException("Null stream."));
                return reader.ReadToEnd();
            }

            /// <summary>
            ///     Sanitizes and streamlines Json paths.
            /// </summary>
            /// <remarks>
            ///     <c>Json/</c> will be added to the beginning if it was not already.
            /// </remarks>
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

        public static class Sample
        {
            public static string KeyCaps => GetSample("Keyboard/key-caps.mp3");

            public static string KeyConfirm => GetSample("Keyboard/key-confirm.mp3");

            public static string KeyDelete => GetSample("Keyboard/key-delete.mp3");

            public static string KeyMovement => GetSample("Keyboard/key-movement.mp3");

            public static string KeyPress1 => GetSample("Keyboard/key-press-1.mp3");

            public static string KeyPress2 => GetSample("Keyboard/key-press-2.mp3");

            public static string KeyPress3 => GetSample("Keyboard/key-press-3.mp3");

            public static string KeyPress4 => GetSample("Keyboard/key-press-4.mp3");

            public static string ConfirmEnter => GetSample("Main/ConfirmEnter.ogg", includeSamples: false);
        }

        public static class Texture
        {
            public static string MenuBackground1 => GetTexture("Backgrounds/menu-background-1.png", includeTextures: false);

            public static string MenuBackground2 => GetTexture("Backgrounds/menu-background-2.png", includeTextures: false);

            public static string MenuBackground3 => GetTexture("Backgrounds/menu-background-3.png", includeTextures: false);

            public static string CursorXml => GetTexture("Cursor/cursorwhatever.xml");

            public static string GfDanceTitleXml => GetTexture("Shared/gfDanceTitle.xml");

            public static string HealthBar => GetTexture("Shared/healthBar.png", includeTextures: false);

            public static string LogoBumpinXml => GetTexture("Shared/logoBumpin.xml");

            public static string StoryModeButton => GetTexture("Shared/story mode white.png", includeTextures: false);

            public static string FreeplayButton => GetTexture("Shared/freeplay white.png", includeTextures: false);

            public static string OptionsButton => GetTexture("Shared/options white.png", includeTextures: false);
        }

        public static class Track
        {
            public static string FreakyMenu => GetTrack("Main/FreakyMenu.ogg");
        }

        /// <summary>
        ///     Sparrow atlas utilities.
        /// </summary>
        public static class Atlas
        {
            /// <summary>
            ///     Converts a frame to a 4-digit string.
            /// </summary>
            public static string FrameAsString(int frame)
            {
                // if it's 0, waste no time
                // and just return "0000"
                if (frame == 0)
                    return "0000";

                int zeroCount = 4;
                int frameLength = frame.ToString().Length;

                // subtract the amount of expected zeroes
                zeroCount -= frameLength;

                StringBuilder builder = new();

                // if there are no zeroes, return the frame,
                // otherwise append zeroes to the beginning
                // of the string, then add the frame count
                return zeroCount <= 0
                    ? builder.Append(frame).ToString()
                    : builder.Append('0', zeroCount).Append(frame).ToString();
            }
        }

        public static Assembly Assembly => typeof(PathHelper).Assembly;

        // replaces '\' and the directory separator character with '/'
        public static string SanitizeForResources(string path) =>
            path.Replace('\\', '/').Replace(Path.DirectorySeparatorChar, '/');

        // replaces '\' and '/' with the directory separator character
        public static string SanitizeForComputer(string path) =>
            path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);

        public static string GetTrack(string path, bool sanitize = true, bool includeTrack = true) =>
            GetPath(path, sanitize, includeTrack, "Track/");

        public static string GetTexture(string path, bool sanitize = true, bool includeTextures = true) =>
            GetPath(path, sanitize, includeTextures, "Textures/");

        public static string GetSample(string path, bool sanitize = true, bool includeSamples = true) =>
            GetPath(path, sanitize, includeSamples, "Samples/");

        public static string GetFont(string path, bool sanitize = true, bool includeFonts = true) =>
            GetPath(path, sanitize, includeFonts, "Fonts/");

        public static string GetPath(string path, bool sanitize, bool includeStart, string start)
        {
            StringBuilder builder = new();

            if (includeStart)
                builder.Append(start);

            builder.Append(path);

            return sanitize ? SanitizeForResources(builder.ToString()) : builder.ToString();
        }
    }
}