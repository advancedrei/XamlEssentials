using System;
#if WINRT
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using System.Diagnostics;
#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
#if !(WINDOWS_PHONE || SL4)
using XamlEssentials.MarkupExtensions;
#endif
#endif

namespace XamlEssentials.Converters
{

    public class ListBoxItemToIndexConverter :
#if !(SILVERLIGHT || WINDOWS_PHONE || WINRT)
        ConvertibleMarkupExtension<ListBoxItemToIndexConverter>,
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
            ListBoxItem item = (ListBoxItem)value;
            ListBox lb = (ListBox)ItemsControl.ItemsControlFromItemContainer(item);
            return lb.ItemContainerGenerator.IndexFromContainer(item) + 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
#if WINRT
            string language
#else
            System.Globalization.CultureInfo culture
#endif
)
        {
            return null;
        }
    }
}
