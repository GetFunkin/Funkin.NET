using System.Collections.Generic;

namespace Funkin.NET.Game.Audio
{
    public interface ISampleInfo
    {
        /// <summary>
        ///     Retrieve all possible filenames that can be used as a source, returned in order of preference (highest first).
        /// </summary>
        IEnumerable<string> LookupNames { get; }

        int Volume { get; }
    }
}
