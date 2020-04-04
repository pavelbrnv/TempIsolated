using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TempIsolated.Common.Gui.Converters
{
	public class BoolToVisibilityConverter : IValueConverter
	{
		public static BoolToVisibilityConverter Instance { get; } = new BoolToVisibilityConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is bool))
				return Visibility.Collapsed;

			var bValue = (bool)value;

			var sPar = parameter as string;
			if (sPar != null)
			{
				sPar = sPar.ToLower();
				if (sPar == "reverse" || sPar == "inverse")
					bValue = !bValue;
			}


			if (bValue)
				return Visibility.Visible;

			return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is Visibility))
				return false;

			var bValue = true;
			var sPar = parameter as string;
			if (sPar != null)
			{
				if (sPar == "Reverse" || sPar == "Inverse")
					bValue = false;
			}

			switch ((Visibility)value)
			{
				case Visibility.Visible:
					return bValue;
				case Visibility.Collapsed:
					return !bValue;
				default:
					return bValue;
			}
		}
	}
}