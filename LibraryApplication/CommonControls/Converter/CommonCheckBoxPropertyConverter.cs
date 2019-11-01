using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CommonControls.Converter
{
    public class CommonCheckBoxPropertyConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CommonCheckBox commonCheckBox && parameter != null)
            {
                switch (parameter.ToString())
                {
                    case "MouseOverBackground":
                        return commonCheckBox.MouseOverBackground.HasValue ? new SolidColorBrush(commonCheckBox.MouseOverBackground.Value) : commonCheckBox.Background;
                    case "MouseOverBorderBrush":
                        return commonCheckBox.MouseOverBorderBrush.HasValue ? new SolidColorBrush(commonCheckBox.MouseOverBorderBrush.Value) : commonCheckBox.BorderBrush;
                    case "MouseOverForeground":
                        return commonCheckBox.MouseOverForeground.HasValue ? new SolidColorBrush(commonCheckBox.MouseOverForeground.Value) : commonCheckBox.Foreground;
                    case "MouseOverThickness":
                        return commonCheckBox.MouseOverThickness.HasValue ? commonCheckBox.MouseOverThickness.Value : commonCheckBox.BorderThickness;

                    case "PressedBackground":
                        return commonCheckBox.PressedBackground.HasValue ? new SolidColorBrush(commonCheckBox.PressedBackground.Value) :
                           commonCheckBox.MouseOverBackground.HasValue ? new SolidColorBrush(commonCheckBox.MouseOverBackground.Value) : commonCheckBox.Background;
                    case "PressedBorderBrush":
                        return commonCheckBox.PressedBorderBrush.HasValue ? new SolidColorBrush(commonCheckBox.PressedBorderBrush.Value) :
                            commonCheckBox.MouseOverBorderBrush.HasValue ? new SolidColorBrush(commonCheckBox.MouseOverBorderBrush.Value) : commonCheckBox.BorderBrush;
                    case "PressedForeground":
                        return commonCheckBox.PressedForeground.HasValue ? new SolidColorBrush(commonCheckBox.PressedForeground.Value) :
                            commonCheckBox.MouseOverForeground.HasValue ? new SolidColorBrush(commonCheckBox.MouseOverForeground.Value) : commonCheckBox.Foreground;
                    case "PressedThickness":
                        return commonCheckBox.PressedThickness.HasValue ? commonCheckBox.PressedThickness.Value :
                            commonCheckBox.MouseOverThickness.HasValue ? commonCheckBox.MouseOverThickness.Value : commonCheckBox.BorderThickness;

                    case "DisabledBackground":
                        return commonCheckBox.DisabledBackground.HasValue ? new SolidColorBrush(commonCheckBox.DisabledBackground.Value) : commonCheckBox.Background;
                    case "DisabledBorderBrush":
                        return commonCheckBox.DisabledBorderBrush.HasValue ? new SolidColorBrush(commonCheckBox.DisabledBorderBrush.Value) : commonCheckBox.BorderBrush;
                    case "DisabledForeground":
                        return commonCheckBox.DisabledForeground.HasValue ? new SolidColorBrush(commonCheckBox.DisabledForeground.Value) : commonCheckBox.Foreground;
                    case "DisabledThickness":
                        return commonCheckBox.DisabledThickness.HasValue ? commonCheckBox.DisabledThickness.Value : commonCheckBox.BorderThickness;

                    case "CheckBackground":
                        return commonCheckBox.CheckBackground.HasValue ? new SolidColorBrush(commonCheckBox.CheckBackground.Value) : commonCheckBox.Background;
                    case "CheckBorderBrush":
                        return commonCheckBox.CheckBorderBrush.HasValue ? new SolidColorBrush(commonCheckBox.CheckBorderBrush.Value) : commonCheckBox.BorderBrush;
                    case "CheckForeground":
                        return commonCheckBox.CheckForeground.HasValue ? new SolidColorBrush(commonCheckBox.CheckForeground.Value) : commonCheckBox.Foreground;
                    case "CheckThickness":
                        return commonCheckBox.CheckThickness.HasValue ? commonCheckBox.CheckThickness.Value : commonCheckBox.BorderThickness;
                }
            }
            return value;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
