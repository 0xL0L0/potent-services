using System;
using System.Collections.Generic;

namespace Potency.Services.Runtime.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrAddNew<TKey, TValue>(
            this IDictionary<TKey, TValue> dict,
            TKey key,
            Action<TValue> initializer = null)
            where TValue : new()
        {
            if (!dict.ContainsKey(key))
            {
                TValue val = new TValue();
                initializer?.Invoke(val);
                dict.Add(key, val);
            }

            return dict[key];
        }

        public static TValue GetOrAddNew<TKey, TValue>(
            this IDictionary<TKey, TValue> dict,
            TKey key,
            TValue defaultValue)
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, defaultValue);
            }

            return dict[key];
        }
    }
}