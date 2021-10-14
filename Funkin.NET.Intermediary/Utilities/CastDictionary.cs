using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Funkin.NET.Intermediary.Utilities
{
    /// <summary>
    ///     Null-safe dictionary with easy casting on retrieval.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class CastDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey : notnull
    {
        public new TValue this[TKey key]
        {
            [CanBeNull] get => PrivateGet(key)!;

            set => base[key] = value;
        }

        public T? As<T>(TKey key, bool forgiveNull = false) where T : TValue =>
            (T?) (PrivateGet(key) ?? ThrowNull(key, forgiveNull));

        private TValue? PrivateGet(TKey key) => ContainsKey(key) ? base[key] : default;

        private static TValue ThrowNull(TKey key, bool throwNull)
        {
            if (throwNull)
                throw new NullReferenceException(key + " was null during dictionary retrieval.");

            return default!;
        }
    }
}