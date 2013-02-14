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
    /// <summary>
    /// Converts an Integer Value into an Enumeration. 
    /// </summary>
    public class StringToBooleanConverter :
#if !(SILVERLIGHT || WINDOWS_PHONE || WINRT)
        ConvertibleMarkupExtension<CollectionCountToVisibilityConverter>,
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
            bool result = false;
            if (value != null && value is string)
                Boolean.TryParse(value as string, out result);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
#if WINRT
            string language
#else
            System.Globalization.CultureInfo culture
#endif
)
        {
            return ((bool)value).ToString();
        }

    }

}