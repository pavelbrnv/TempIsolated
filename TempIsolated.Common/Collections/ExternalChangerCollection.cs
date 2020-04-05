using System;
using System.Collections;
using System.Collections.Specialized;

namespace TempIsolated.Common.Collections
{
	/// <summary>
	/// Коллекция-обертка, обрабатывающая изменения из базовой коллекции с помощью объекта ICollectionChanger.
	/// Этот коллекция может использоваться, например, для выполнения всех изменений в потоке UI.
	/// </summary>
	public sealed class ExternalChangerCollection<TCollection> : IEnumerable, INotifyCollectionChanged, IDisposable
		where TCollection : ICollection, INotifyCollectionChanged
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

			var originalItems = new ArrayList(baseCollection);
			SetItemsList(originalItems);

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

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Items.GetEnumerator();
		}

		#endregion

		#region Items

		private ArrayList items = new ArrayList();

		private IList Items
		{
			get { return items; }
		}

		private void SetItemsList(ArrayList newItems)
		{
			items = newItems;
		}

		private void AddToItems(NotifyCollectionChangedEventArgs e)
		{
			items.InsertRange(e.NewStartingIndex, e.NewItems);
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
			items.InsertRange(e.NewStartingIndex, e.NewItems);
			NotifyCollectionChanged(e);
		}

		private void ReplaceItems(NotifyCollectionChangedEventArgs e)
		{
			var index = e.NewStartingIndex;
			foreach (var item in e.NewItems)
			{
				items[index++] = item;
			}

			NotifyCollectionChanged(e);
		}

		private void ResetItems(NotifyCollectionChangedEventArgs e, ArrayList newItems)
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
					var itemsAfterReset = new ArrayList(baseCollection);
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
