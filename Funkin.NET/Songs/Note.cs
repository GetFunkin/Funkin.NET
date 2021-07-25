using Funkin.NET.Input.Bindings;

namespace Funkin.NET.Songs
{
    public struct Note
    {
        /// <summary>
        /// The time when the note has to be pressed (in milliseconds)
        /// </summary>
        public double Offset { get; set; }

        // TODO: one key -> unlimited key-combos
        /// <summary>
        /// The key that has to be pressed
        /// </summary>
        public KeyAssociatedAction Key { get; set; }

        /// <summary>
        /// How long the player has to hold the note (in milliseconds)
        /// </summary>
        public int HoldLength { get; }

        public Note(int offset, KeyAssociatedAction key, int holdLength)
        {
            Offset = offset;
            Key = key;
            HoldLength = holdLength;
        }
    }
}