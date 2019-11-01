using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CommonControls.Converter
{
    public class CommonListBoxPropertyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CommonListBox listBox && parameter != null)
            {
                switch (parameter.ToString())
                {
                    case "MouseOverItemBackground":
                        return listBox.MouseOverItemBackground.HasValue ? new SolidColorBrush(listBox.MouseOverItemBackground.Value) :
                            new SolidColorBrush(listBox.NormalItemBackground);
                    case "MouseOverItemBorderBrush":
                        return listBox.MouseOverItemBorderBrush.HasValue ? new SolidColorBrush(listBox.MouseOverItemBorderBrush.Value) :
                            new SolidColorBrush(listBox.NormalItemBorderBrush);
                    case "MouseOverItemThickness":
                        return listBox.MouseOverItemThickness.HasValue ? listBox.MouseOverItemThickness.Value : listBox.NormalItemThickness;
                    case "MouseOverItemOpacity":
                        return listBox.MouseOverItemOpacity.HasValue ? listBox.MouseOverItemOpacity.Value : listBox.NormalItemOpacity;

                    case "PressedItemBackground":
                        return listBox.PressedItemBackground.HasValue ? new SolidColorBrush(listBox.PressedItemBackground.Value) :
                            new SolidColorBrush(listBox.NormalItemBackground);
                    case "PressedItemBorderBrush":
                        return listBox.PressedItemBorderBrush.HasValue ? new SolidColorBrush(listBox.PressedItemBorderBrush.Value) :
                            new SolidColorBrush(listBox.NormalItemBorderBrush);
                    case "PressedItemThickness":
                        return listBox.PressedItemThickness.HasValue ? listBox.PressedItemThickness.Value : listBox.NormalItemThickness;
                    case "PressedItemOpacity":
                        return listBox.PressedItemOpacity.HasValue ? listBox.PressedItemOpacity.Value : listBox.NormalItemOpacity;
                }
            }
            if (value is Color color)
            {
                return new SolidColorBrush(color);
            }
            else if (value is Thickness thickness)
            {
                return thickness;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
