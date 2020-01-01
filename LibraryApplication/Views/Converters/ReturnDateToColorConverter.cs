using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Views.Converters
{
    public class ReturnDateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush forgroundColor = new SolidColorBrush(Colors.Black);
            if (value==null)
            {
                return forgroundColor;
            }
            DateTime t;
            if (DateTime.TryParse(value.ToString(),out t))
            {
                var d = (t - DateTime.Now).Days;
                if (d <= 30)
                {
                    forgroundColor.Color = (Color)ColorConverter.ConvertFromString("#ff7256");
                }
                else if (d>30)
                {
                    forgroundColor.Color = (Color)ColorConverter.ConvertFromString("#ffaa56");
                }             
            }
            return forgroundColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
