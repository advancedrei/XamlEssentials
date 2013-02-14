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
    public class StringNotEmptyToVisibilityConverter : 
#if !(SILVERLIGHT || WINDOWS_PHONE || WINRT)
        ConvertibleMarkupExtension<StringNotEmptyToVisibilityConverter>,
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
            return (!string.IsNullOrWhiteSpace(value as string) ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
#if WINRT
            string language
#else
            System.Globalization.CultureInfo culture
#endif
            )
        {
            throw new NotImplementedException();
        }

    }

}
