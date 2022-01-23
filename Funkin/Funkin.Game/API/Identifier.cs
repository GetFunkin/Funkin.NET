using System;

namespace Funkin.Game.API
{
    /// <summary>
    ///     A string lexer identifier, with <c>snake_case</c> conventions for naming.
    /// </summary>
    public readonly struct Identifier : IEquatable<Identifier>
    {
        /// <summary>
        ///     The namespace, typically a mod name.
        /// </summary>
        public readonly string Namespace;

        /// <summary>
        ///     The content name, a unique identifier not shared by any other content.
        /// </summary>
        public readonly string Content;

        /// <summary>
        ///     A constructor providing access to namespace and content setting.
        /// </summary>
        /// <param name="namespace">The namespace of the content.</param>
        /// <param name="content">The content's name.</param>
        public Identifier(string @namespace, string content)
        {
            Namespace = @namespace;
            Content = content;
        }

        /// <summary>
        ///     A constructor for producing Mania content. The namespace is already set.
        /// </summary>
        /// <param name="content">The content's name.</param>
        public Identifier(string content)
            : this(Constants.MANIA_NAME, content)
        {
        }

        /// <summary>
        ///     Produces a combination of the <see cref="Namespace"/> and <see cref="Content"/>.
        /// </summary>
        /// <returns><c>$"{Namespace}:{Content}"</c></returns>
        public override string ToString() => $"{Namespace}:{Content}";

        public static Identifier Parse(string s)
        {
            if (s is null)
                throw new ArgumentNullException(nameof(s));

            if (!TryParse(s, out Identifier identifier))
                throw new ArgumentException("Failed to parse into identifier.", nameof(s));

            return identifier;
        }

        public static bool TryParse(string s, out Identifier identifier)
        {
            identifier = new Identifier();

            string[] array = s.Split(':', 1);

            if (array.Length != 2)
                return false; // Invalid length (no ':' present).

            if (string.IsNullOrWhiteSpace(array[0]) || string.IsNullOrWhiteSpace(array[1]))
                // Figure out a way to communicate common errors later.
                // throw new ArgumentException("Input contained an empty namespace or content name.", nameof(s));
                return false;

            identifier = new Identifier(array[0], array[1]);
            return true;
        }

        public bool Equals(Identifier other) => Namespace == other.Namespace && Content == other.Content;

        public bool Equals(string other) => Equals(Parse(other));

        public override bool Equals(object? obj)
        {
            return obj switch
            {
                Identifier other => Equals(other),
                string other => Equals(other),
                _ => false
            };
        }

        public override int GetHashCode() => HashCode.Combine(Namespace, Content);
    }
}
