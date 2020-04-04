using System;

namespace TempIsolated.Common.Extensions.ViewModels
{
	public abstract class ObservingVm<TModel> : NotifyPropertyChanged, IDisposable
	{
		#region Properties

		protected TModel Model { get; }

		#endregion

		#region Ctor

		protected ObservingVm(TModel model)
		{
			Model = model;
		}

		protected virtual void Initialize()
		{
			SubscribeModel(true);
		}

		#endregion

		#region Abstract

		protected abstract void SubscribeModel(bool subscribe);

		#endregion

		#region Disposing

		private bool disposed;

		private readonly object disposingSync = new object();

		public void Dispose()
		{
			lock (disposingSync)
			{
				if (disposed)
				{
					return;
				}

				SubscribeModel(false);

				DisposeResources();

				disposed = true;
			}
		}

		protected virtual void DisposeResources()
		{
		}

		#endregion
	}
}
