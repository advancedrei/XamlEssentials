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
    public class IntGreaterOrEqualToVisibilityConverter :
#if !(SILVERLIGHT || WINDOWS_PHONE || WINRT)
        ConvertibleMarkupExtension<IntGreaterOrEqualToVisibilityConverter>,
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
            if (value == null || parameter == null) return Visibility.Collapsed;
            if (value == DependencyProperty.UnsetValue) return Visibility.Collapsed;

            int valueToCompare = (int)value;
            int referenceValue = int.Parse((string)parameter);

            return valueToCompare >= referenceValue ? Visibility.Visible : Visibility.Collapsed;

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

