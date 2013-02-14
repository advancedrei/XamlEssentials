using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
    /// A MultiValueConverter that lets you bind single or multiple values and output them based on the specified FormatString.
    /// </summary>
    /// <remarks>
    /// Turned into a MarkupExtension via: http://blog.wpfwonderland.com/2010/04/15/simplify-your-binding-converter-with-a-custom-markup-extension/
    /// </remarks>
    public class StringFormatConverter :
#if !(SILVERLIGHT || WINDOWS_PHONE || WINRT)
        ConvertibleMarkupExtension<IntEqualToVisibilityConverter>, IMultiValueConverter,
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
            return this.ConvertInternal(new object[] { value }, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
#if WINRT
            string language
#else
            System.Globalization.CultureInfo culture
#endif
)
        {
#if !(SILVERLIGHT || WINRT)
            TypeConverter typeConverter = TypeDescriptor.GetConverter(targetType);
            Object returnValue = null;

            if (typeConverter.CanConvertFrom(value.GetType()))
                returnValue = typeConverter.ConvertFrom(value);

            return returnValue;
#else
            throw new NotImplementedException();

#endif

        }

#if !(SILVERLIGHT || WINDOWS_PHONE || WINRT)

        #region IMultiValueConverter Implementation

        public object Convert(object[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.ConvertInternal(value, targetType, parameter, culture);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

#endif

        private object ConvertInternal(object[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter != null && parameter.GetType() != typeof(string))
                throw new ArgumentException("The parameter specified in the binding must be a string.");

            var test = (from o in value.AsQueryable() select o == DependencyProperty.UnsetValue ? "" : o).ToArray();
            return string.Format(culture, (parameter as string), test);
        }

    }
}