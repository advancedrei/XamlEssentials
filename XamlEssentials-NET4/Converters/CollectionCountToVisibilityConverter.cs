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
    /// 
    /// </summary>
    public class CollectionCountToVisibilityConverter :
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
            if (value == DependencyProperty.UnsetValue) return Visibility.Collapsed;
            bool flag = false;
            if (value is int)
                if ((int)value > 0)
                    flag = true;

            return (flag ? Visibility.Visible : Visibility.Collapsed);
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
