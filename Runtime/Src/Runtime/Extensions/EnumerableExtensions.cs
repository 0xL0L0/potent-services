using System;
using System.Collections.Generic;

namespace Potency.Services.Runtime.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool TryFind<T>(this IEnumerable<T> values, Func<T, bool> predicate, out T value)
        {
            foreach (T v in values)
            {
                if (predicate(v))
                {
                    value = v;
                    return true;
                }
            }

            value = default;
            return false;
        }
    }
}