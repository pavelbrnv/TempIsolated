using System;
using System.Collections.Generic;

namespace TempIsolated.Common.Extensions
{
	public class CollectionChangedEventArgs<T> : EventArgs
	{
		public IReadOnlyList<T> Added { get; }

		public bool HasAdded => Added.Count > 0;

		public IReadOnlyList<T> Removed { get; }

		public bool HasRemoved => Removed.Count > 0;

		public CollectionChangedEventArgs(IReadOnlyList<T> added, IReadOnlyList<T> removed)
		{
			Added = added ?? new T[0];
			Removed = removed ?? new T[0];
		}
	}
}
