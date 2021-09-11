using System.Collections.Generic;

namespace Funkin.NET.Intermediary.Utilities
{
    /// <summary>
    ///     Null-safe dictionary with easy casting on retrieval.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class CastDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public new TValue this[TKey key]
        {
            get => ContainsKey(key) ? base[key] : default;

            set => base[key] = value;
        }

        public T As<T>(TKey key) where T : TValue => (T) this[key];
    }
}