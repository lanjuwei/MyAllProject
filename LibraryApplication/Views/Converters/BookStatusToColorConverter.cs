
using Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Views.Converters
{
    public class BookStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush forgroundColor=new SolidColorBrush(Colors.Black);
            if (value is BookStatus bookStatus)
            {
                switch (bookStatus)
                {
                    case BookStatus.None:
                        forgroundColor.Color = Colors.Red;
                        break;
                    case BookStatus.Lended:
                        forgroundColor.Color = (Color)ColorConverter.ConvertFromString("#ff7256");
                        break;
                    case BookStatus.Returned:
                        forgroundColor.Color = (Color)ColorConverter.ConvertFromString("#ffaa56");
                        break;
                    case BookStatus.Reserved:
                        forgroundColor.Color = (Color)ColorConverter.ConvertFromString("#2db36c");
                        break;
                    default:
                        forgroundColor.Color = Colors.Red;
                        break;
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