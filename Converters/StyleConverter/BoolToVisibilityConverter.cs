using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DBClient.Converters.StyleConverter
{
	class BoolToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var visibilityParams = ( (string) parameter ).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
			var isTrue = Enum.Parse(typeof(Visibility), (string) visibilityParams[0], true);			
			var isFalse = Enum.Parse(typeof(Visibility), (string) visibilityParams[1], true);
			return (bool) value ? isTrue : isFalse;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
