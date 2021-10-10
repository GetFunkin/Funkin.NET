using System;

namespace Funkin.NET.Core.API
{
    /// <summary>
    ///     String lexer identity.
    /// </summary>
    public readonly struct Identity : IEquatable<Identity>
    {
        public string Namespace { get; }

        public string Name { get; }

        public Identity(string @namespace, string name)
        {
            Namespace = @namespace;
            Name = name;
        }

        public bool IsDefault() => Namespace is null && Name is null;

        public bool Equals(Identity other) => Namespace == other.Namespace && Name == other.Name;

        public bool Equals(string other) => other == ToString();

        public override bool Equals(object obj) => obj switch
        {
            Identity identity => Equals(identity),
            string identity => Equals(identity),
            _ => false
        };

        public override int GetHashCode() => HashCode.Combine(Namespace, Name);

        public override string ToString() => Namespace + ':' + Name;

        public static bool TryParse(string value, out Identity identity)
        {
            identity = new Identity();

            string[] contents = value.Split(':', 2);

            if (contents.Length != 2)
                return false;

            identity = new Identity(contents[0], contents[1]);
            return true;
        }

        public static Identity Parse(string value)
        {
            if (TryParse(value, out Identity identity))
                return identity;

            throw new Exception("Failed to parse identity from string: " + value);
        }

        public static bool operator ==(Identity left, Identity right) => left.Equals(right);

        public static bool operator !=(Identity left, Identity right) => !(left == right);
    }
}