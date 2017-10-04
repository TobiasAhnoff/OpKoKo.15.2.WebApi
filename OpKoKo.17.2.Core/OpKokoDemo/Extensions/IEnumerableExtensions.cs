using System;
using System.Collections.Generic;

namespace OpKokoDemo.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> value, Action<T> action)
        {
            foreach (T obj in value)
                action(obj);
        }
    }
}
