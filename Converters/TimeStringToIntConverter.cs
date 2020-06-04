using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace employees.Converters
{
    class TimeStringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return ((int)value).ToString("##0:00");
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strval = value as string;
            if (string.IsNullOrEmpty(strval))
            {
                return null;
            }

            if (int.TryParse(strval.Replace(":", ""), out var intval))
            {
                return intval;
            }
            return null;
        }
    }
}
