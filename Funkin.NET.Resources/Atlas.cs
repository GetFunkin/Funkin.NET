using System.Text;

namespace Funkin.NET.Resources
{
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
}