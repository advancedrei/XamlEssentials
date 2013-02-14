using System;
#if WINRT
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using System.Diagnostics;
#else
using System.Windows;
using System.Windows.Data;
#if !(WINDOWS_PHONE || SL4)
using XamlEssentials.MarkupExtensions;
#endif
#endif

namespace XamlEssentials.Converters
{


    public class IntegerIsNotZeroConverter :
#if !(SILVERLIGHT || WINDOWS_PHONE || WINRT)
        ConvertibleMarkupExtension<IntegerIsNotZeroConverter>,
#endif
 IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter,
#if WINRT
            string language
#else
            System.Globalization.CultureInfo culture
#endif
)
        {
            if (value == DependencyProperty.UnsetValue) return Visibility.Collapsed;
            if (value.GetType() != typeof(int))
            {
                //Trace.TraceWarning("The value passed to the IntegerIsNotZeroConverter is not an Integer. Returning \"False\".");
                return false;
            }

            bool flag = (int)value > 0;
            return flag;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
#if WINRT
            string language
#else
 System.Globalization.CultureInfo culture
#endif
)
        {
            return ((value is Visibility) && (((Visibility)value) == Visibility.Visible));
        }
    }

}
