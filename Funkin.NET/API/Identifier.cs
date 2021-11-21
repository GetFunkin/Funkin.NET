using System;

namespace Funkin.NET.API
{
    // String lexer format.
    /// <summary>
    ///     An identifier used for identifying game content.
    /// </summary>
    public readonly struct Identifier : IComparable<Identifier>, IComparable<string>, IEquatable<Identifier>, IEquatable<string>
    {
        /// <summary>
        ///     The namespace (name of the mod) attributed to this identifier.
        /// </summary>
        public string Namespace { get; }

        /// <summary>
        ///     The actual identity of the content, typically in snake_case.
        /// </summary>
        public string Id { get; }

        // Optional parameters to allow a default constructor with non-null properties.
        /// <summary>
        ///     Instantiates a new <see cref="Identifier"/> instance with a specified namespace and id.
        /// </summary>
        /// <param name="namespace">The unique namespace of your content.</param>
        /// <param name="id">The name of your content, which should be unique amongst your mod.</param>
        public Identifier(string @namespace = "'", string id = "")
        {
            Namespace = @namespace;
            Id = id;
        }

        /// <summary>
        ///     Instantiates a new <see cref="Identifier"/> with a pre-set default namespace of <c>mania</c>, the base mod's name.
        /// </summary>
        /// <param name="id">The name of a piece of <c>mania</c> content.</param>
        public Identifier(string id) : this("mania", id)
        {
        }

        public override string ToString() => Namespace + ":" + Id;

        public override int GetHashCode() => HashCode.Combine(Namespace, Id);

        public int CompareTo(Identifier other)
        {
            string first = ToString();
            string second = other.ToString();

            return string.Compare(first, second, StringComparison.Ordinal);
        }

        public int CompareTo(string? other) => string.Compare(ToString(), other, StringComparison.Ordinal);

        public bool Equals(Identifier other) => ToString().Equals(other.ToString());

        public bool Equals(string? other) => ToString().Equals(other);

        public static implicit operator string(Identifier identifier) => identifier.ToString();

        public static implicit operator Identifier(string str)
        {
            string[] contents = str.Split(':', 1);

            if (contents.Length == 0)
                throw new ArgumentException("Provided string could not be split into a valid identifier!", nameof(str));

            return new Identifier(contents[0], contents[1]);
        }

        public override bool Equals(object? obj) =>
            obj switch
            {
                null => false,
                string str => Equals(str),
                Identifier id => Equals(id),
                _ => Equals(obj.ToString())
            };
    }
}