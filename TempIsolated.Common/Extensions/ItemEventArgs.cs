using System;

namespace TempIsolated.Common.Extensions
{
    public class ItemEventArgs<T> : EventArgs
    {
        public T Item { get; }

        public ItemEventArgs(T item)
        {
            Item = item;
        }
    }
}
