using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Funkin.NET.Game.Audio
{
    /// <summary>
    ///     Describes a gameplay sample.
    /// </summary>
    public class SampleInfo : ISampleInfo, IEquatable<SampleInfo>
    {
        protected readonly string[] SampleNames;

        public SampleInfo(params string[] sampleNames)
        {
            SampleNames = sampleNames;
            Array.Sort(SampleNames);
        }

        public IEnumerable<string> LookupNames => SampleNames;

        public int Volume => 100;

        public override int GetHashCode() => HashCode.Combine(StructuralComparisons.StructuralEqualityComparer.GetHashCode(SampleNames), Volume);

        public bool Equals(SampleInfo? other) => other is not null && SampleNames.SequenceEqual(other.SampleNames);

        public override bool Equals(object obj) => obj is SampleInfo other && Equals(other);
    }
}
