﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Views.Converters
{
    internal class LengthToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string st&&st== "Length")
            {
                if (value is string t&&t.Length>15)
                {

                    return t.Substring(0, 15) + "...";
                }
                return value;
            }
            else
            {
                if (value is string t)
                {
                    if (t.Length > 15)
                    {
                        return Visibility.Visible;
                    }
                }
                return Visibility.Collapsed;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
