using System.Collections.Generic;
using System.Collections;
using System.Linq;

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
            return (from object item in source let typedItem = item as T where typedItem != null select item as T).ToList();
        }
    }
}
