using Funkin.NET.Common.KeyBinds.ArrowKeys;

namespace Funkin.NET.Core.Models
{
    public class Note
    {
        /// <summary>
        /// The time when the note has to be pressed (in milliseconds)
        /// </summary>
        public int Offset { get; }
        /// <summary>
        /// The key that has to be pressed
        /// </summary>
        public ArrowKeyAction Key { get; }
        /// <summary>
        /// How long the player has to hold the note (in milliseconds)
        /// </summary>
        public int HoldLength { get; }

        public Note(int offset, ArrowKeyAction key, int holdLength)
        {
            Offset = offset;
            Key = key;
            HoldLength = holdLength;
        }
    }
}