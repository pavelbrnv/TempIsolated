using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace TempIsolated.Common.Collections
{
	/// <summary>
	/// Коллекция-обертка, обрабатывающая изменения из базовой коллекции с помощью объекта ICollectionChanger.
	/// Этот коллекция может использоваться, например, для выполнения всех изменений в потоке UI.
	/// </summary>
	public sealed class ExternalChangerCollection<T, TCollection> : IEnumerable<T>, INotifyCollectionChanged, IDisposable
		where TCollection : ICollection<T>, INotifyCollectionChanged
	{
		private readonly ICollectionChanger changer;
		private readonly TCollection baseCollection;

		/// <summary>
		/// Создание объекта должно выполняться в потоке, который вносит изменения в базовую коллекцию baseCollection.
		/// Если создание происходит в другом потоке, то оно должно происходить после того, как коллекция отправит
		/// все уведомления о совершенных изменениях, и до того, как начнут происходить новые изменения.
		/// Иначе корректная синхронизация коллекций не гарантируется.
		/// </summary>
		public ExternalChangerCollection(ICollectionChanger changer, TCollection baseCollection)
		{
			this.changer = changer;
			this.baseCollection = baseCollection;

			SetItemsList(baseCollection.ToList());

			SubscribeBaseCollection(true);
		}

		#region INotifyCollectionChanged

		public event NotifyCollectionChangedEventHandler CollectionChanged = delegate { };

		private void NotifyCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			CollectionChanged(this, e);
		}

		#endregion

		#region IEnumerable

		public IEnumerator<T> GetEnumerator()
		{
			return Items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region Items

		private List<T> items = new List<T>();

		private IList<T> Items
		{
			get { return items; }
		}

		private void SetItemsList(List<T> newItems)
		{
			items = newItems;
		}

		private void AddToItems(NotifyCollectionChangedEventArgs e)
		{
			items.InsertRange(e.NewStartingIndex, e.NewItems.Cast<T>());
			NotifyCollectionChanged(e);
		}

		private void RemoveFromItems(NotifyCollectionChangedEventArgs e)
		{
			items.RemoveRange(e.OldStartingIndex, e.OldItems.Count);
			NotifyCollectionChanged(e);
		}

		private void MoveItems(NotifyCollectionChangedEventArgs e)
		{
			items.RemoveRange(e.OldStartingIndex, e.NewItems.Count);
			items.InsertRange(e.NewStartingIndex, e.NewItems.Cast<T>());
			NotifyCollectionChanged(e);
		}

		private void ReplaceItems(NotifyCollectionChangedEventArgs e)
		{
			var index = e.NewStartingIndex;
			foreach (var item in e.NewItems)
			{
				items[index++] = (T)item;
			}

			NotifyCollectionChanged(e);
		}

		private void ResetItems(NotifyCollectionChangedEventArgs e, List<T> newItems)
		{
			SetItemsList(newItems);
			NotifyCollectionChanged(e);
		}

		#endregion

		#region Base collection changed

		private void SubscribeBaseCollection(bool subscribe)
		{
			if (subscribe)
			{
				baseCollection.CollectionChanged += OnBaseCollectionChanged;
			}
			else
			{
				baseCollection.CollectionChanged -= OnBaseCollectionChanged;
			}
		}

		private void OnBaseCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			Action action;

			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					action = () => AddToItems(e);
					break;
				case NotifyCollectionChangedAction.Remove:
					action = () => RemoveFromItems(e);
					break;
				case NotifyCollectionChangedAction.Move:
					action = () => MoveItems(e);
					break;
				case NotifyCollectionChangedAction.Reset:
					var itemsAfterReset = baseCollection.ToList();
					action = () => ResetItems(e, itemsAfterReset);
					break;
				case NotifyCollectionChangedAction.Replace:
					action = () => ReplaceItems(e);
					break;
				default:
					throw new InvalidOperationException("Invalid collection changed action");
			}

			changer.Invoke(e.Action, action);
		}

		#endregion

		#region IDisposable

		public void Dispose()
		{
			SubscribeBaseCollection(false);
		}

		#endregion
	}
}