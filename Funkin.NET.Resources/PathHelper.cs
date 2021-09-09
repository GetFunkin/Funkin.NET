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
        public static class EmbeddedResource
        {
            public static string SanitizeForEmbeds(string path, Assembly assembly = null)
            {
                path = path.Replace(Path.DirectorySeparatorChar, '.')
                    .Replace('/', '.').Replace('\\', '.');

                assembly ??= Assembly;

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

            public static string IntroTextJson => GetJson(IntroTextPath);

            public static string GetJson(string path)
            {
                using Stream stream = Assembly.GetManifestResourceStream(EmbeddedResource.SanitizeForEmbeds(path));
                using TextReader reader = new StreamReader(stream ?? throw new NullReferenceException("Null stream."));
                return reader.ReadToEnd();
            }

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

            public static string ConfirmEnter => GetSample("Main/ConfirmEnter.ogg");
        }

        public static class Texture
        {
            public static string MenuBackground1 => GetTexture("Backgrounds/menu-background-1.png");

            public static string MenuBackground2 => GetTexture("Backgrounds/menu-background-2.png");

            public static string MenuBackground3 => GetTexture("Backgrounds/menu-background-3.png");

            public static string CursorXml => GetTexture("Cursor/cursorwhatever.xml");

            public static string GfDanceTitleXml => GetTexture("Shared/gfDanceTitle.xml");

            public static string HealthBar => GetTexture("Shared/healthBar.png");

            public static string LogoBumpinXml => GetTexture("Shared/logoBumpin.xml");
        }

        public static class Track
        {
            public static string FreakyMenu => GetTrack("Main/FreakyMenu.ogg");
        }

        public static class Atlas
        {
            public static string FrameAsString(int frame)
            {
                if (frame == 0)
                    return "0000";

                int zeroCount = 4;
                int frameLength = frame.ToString().Length;
                zeroCount -= frameLength;

                StringBuilder builder = new();

                return zeroCount <= 0
                    ? builder.Append(frame).ToString()
                    : builder.Append('0', zeroCount).Append(frame).ToString();
            }
        }

        public static Assembly Assembly => typeof(PathHelper).Assembly;

        public static string SanitizeForResources(string path) =>
            path.Replace('\\', '/').Replace(Path.DirectorySeparatorChar, '/');

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