using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace DBClient.Converters.StyleConverter
{
	class AccessToBoolEnabledConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var accesses = ( (string) parameter ).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
			var listAccess = new List<AccessType>();

			foreach (var ac in accesses)
			{
				listAccess.Add((AccessType) Enum.Parse(typeof(AccessType), ac));
			}

			var cur_ac = (AccessType) value;

			return listAccess.Contains(cur_ac);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	
}
