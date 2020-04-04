using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TempIsolated.Common.Gui.Converters
{
	public class CollectionToVisibilityConverter : IValueConverter
	{
		public static CollectionToVisibilityConverter Instance { get; } = new CollectionToVisibilityConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int count;
			if (value is int)
			{
				count = (int)value;
			}
			else
			{
				var collection = value as ICollection;
				if (collection == null)
					return Visibility.Collapsed;
				count = collection.Count;
			}

			var paramCount = 0;
			if (parameter is int)
			{
				paramCount = (int)parameter;
			}

			return count > paramCount ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
