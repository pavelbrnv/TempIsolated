using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using TempIsolated.Common.Collections;

namespace TempIsolated.Common.Gui.Converters
{
	public sealed class DispatcherCollectionConverter : IValueConverter
	{
		public static DispatcherCollectionConverter Instance { get; } = new DispatcherCollectionConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var currentApplication = Application.Current;
			if (currentApplication == null || value == null)
			{
				return null;
			}

			var dispatcherChanger = new DispatcherCollectionChanger(currentApplication.Dispatcher);

			var collectionType = value.GetType();
			var externalChangerCollectionType = typeof(ExternalChangerCollection<>).MakeGenericType(collectionType);

			var externalChangerCollection = Activator.CreateInstance(externalChangerCollectionType, dispatcherChanger, value);

			return externalChangerCollection;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}

		#region Nested

		private sealed class DispatcherCollectionChanger : ICollectionChanger
		{
			private readonly Dispatcher dispatcher;

			public DispatcherCollectionChanger(Dispatcher dispatcher)
			{
				this.dispatcher = dispatcher;
			}

			public void Invoke(NotifyCollectionChangedAction changeType, Action changeCollection)
			{
				dispatcher.BeginInvoke(changeCollection);
			}
		}

		#endregion
	}
}
