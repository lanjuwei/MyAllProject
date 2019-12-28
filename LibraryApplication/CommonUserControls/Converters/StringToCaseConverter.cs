using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CommonUserControls.Converters
{
    public class StringToCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isCheck;
            if (!bool.TryParse(value?.ToString(), out isCheck) || parameter == null) return "";
            return isCheck ? parameter.ToString().ToUpperInvariant() : parameter.ToString().ToLowerInvariant();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
