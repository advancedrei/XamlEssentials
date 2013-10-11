using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
#if !(WINDOWS_PHONE || SL4)
using XamlEssentials.MarkupExtensions;
#endif


namespace XamlEssentials.Converters
{

    public class BooleanToVisibilityConverter :
#if !(SILVERLIGHT || WINDOWS_PHONE || WINRT)
 ConvertibleMarkupExtension<System.Windows.Controls.BooleanToVisibilityConverter>
#else
        IValueConverter
#endif
    {

#if (SILVERLIGHT || WINDOWS_PHONE)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToBoolean(value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(Visibility.Visible);
        }
#endif

    }

}
