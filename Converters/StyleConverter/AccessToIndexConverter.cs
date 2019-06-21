using System;
using System.Globalization;
using System.Windows.Data;

namespace DBClient.Converters.StyleConverter
{
	class AccessToIndexConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (int) value - 1;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (AccessType) ( (int) value + 1 );
		}
	}
}
