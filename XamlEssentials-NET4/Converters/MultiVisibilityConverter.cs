using System;
using System.Collections.Generic;
using System.Linq;
#if WINRT
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#else
using System.Windows;
using System.Windows.Data;
using XamlEssentials.MarkupExtensions;
#endif

namespace XamlEssentials.Converters
{

    /// <summary>
    /// Converts an Integer Value into an Enumeration. 
    /// </summary>
#if WINRT
    public class MultiVisibilityConverter : IValueConverter
#else
    public class MultiVisibilityConverter : ConvertibleMarkupExtension<MultiVisibilityConverter>, IValueConverter, IMultiValueConverter
#endif
    {

        #region IValueConverter Implementation

#if WINRT
        public object Convert(object value, string typeName, object parameter, string language)
#else
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
#endif
        {
            if (value == null || parameter == null) return Visibility.Collapsed;
            if (value == DependencyProperty.UnsetValue) return Visibility.Collapsed;

            int valueToCompare = (int)value;
            int referenceValue = int.Parse((string)parameter);

            return valueToCompare == referenceValue ? Visibility.Visible : Visibility.Collapsed;

        }

#if WINRT
        public object ConvertBack(object value, string typeName, object parameter, string language)
#else
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
#endif
        {
            return ((value is Visibility) && (((Visibility)value) == Visibility.Visible));
        }

        #endregion

#if !WINRT

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ////TODO: RWM: This needs to be tested.
            //if (values == null) return Visibility.Collapsed;

            //var results = values.Select(value => (Visibility)this.Convert(value, targetType, parameter, culture)).ToList();
            //var collapsed = results.Select(c => c == Visibility.Collapsed).FirstOrDefault();
            //return collapsed ? Visibility.Collapsed : Visibility.Visible;

            if (values.ToList().Contains(DependencyProperty.UnsetValue)) return Visibility.Collapsed;

            var result = values.Cast<Visibility>().FirstOrDefault(v => v == Visibility.Collapsed);
            return result;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

#endif

    }

}