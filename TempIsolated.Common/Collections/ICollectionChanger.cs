using System;
using System.Collections.Specialized;

namespace TempIsolated.Common.Collections
{
	public interface ICollectionChanger
	{
		void Invoke(NotifyCollectionChangedAction changeType, Action changeCollection);
	}
}
