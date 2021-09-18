using Funkin.NET.Common.Input;

namespace Funkin.NET.Core.Music.Songs
{
    public class Note
    {
        /// <summary>
        /// The time when the note has to be pressed (in milliseconds)
        /// </summary>
        public virtual double Offset { get; set; }

        // TODO: one key -> unlimited key-combos
        /// <summary>
        /// The key that has to be pressed
        /// </summary>
        public virtual KeyAction Key { get; set; }

        /// <summary>
        /// How long the player has to hold the note (in milliseconds)
        /// </summary>
        public virtual int HoldLength { get; set; }
    }
}