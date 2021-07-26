using Funkin.NET.Input.Bindings;

namespace Funkin.NET.Songs
{
    public class Note
    {
        /// <summary>
        /// The time when the note has to be pressed (in milliseconds)
        /// </summary>
        public virtual int Offset { get; }

        // TODO: one key -> unlimited key-combos
        /// <summary>
        /// The key that has to be pressed
        /// </summary>
        public virtual KeyAssociatedAction Key { get; }

        /// <summary>
        /// How long the player has to hold the note (in milliseconds)
        /// </summary>
        public virtual int HoldLength { get; }

        public Note(int offset, KeyAssociatedAction key, int holdLength)
        {
            Offset = offset;
            Key = key;
            HoldLength = holdLength;
        }
    }
}