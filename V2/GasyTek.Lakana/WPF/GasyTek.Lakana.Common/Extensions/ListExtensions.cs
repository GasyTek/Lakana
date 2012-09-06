using System.Collections.Generic;
using System.Collections;

namespace GasyTek.Lakana.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Transform a list to its corresponding generic instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static IList<T> ToGeneric<T>(this IList source) where T : class
        {
            IList<T> result = new List<T>();
            foreach (var item in source)
            {
                var typedItem = item as T;
                if (typedItem != null) result.Add(item as T);
            }
            return result;
        }
    }
}
