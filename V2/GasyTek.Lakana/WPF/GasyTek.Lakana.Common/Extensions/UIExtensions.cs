using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace GasyTek.Lakana.Common.Extensions
{
    public static class UIExtensions
    {
        /// <summary>
        /// Adds asynchronously an item to the observable collection using the current UI dispatcher.
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="item">The item.</param>
        public static void AddAsync<TItem>(this ObservableCollection<TItem> source, TItem item)
        {
            Dispatcher.CurrentDispatcher.
                BeginInvoke(new Action<TItem>(source.Add), DispatcherPriority.Background, item);
        }

        /// <summary>
        /// Adds asynchronously a range of values to the observable collection using the current dispatcher.
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="items">The items.</param>
        public static void AddRangeAsync<TItem>(this ObservableCollection<TItem> source, IEnumerable<TItem> items)
        {
            IEnumerator<TItem> enumerator = items.GetEnumerator();
            while (enumerator.MoveNext())
            {
                source.AddAsync(enumerator.Current);
            }
        }
    }
}