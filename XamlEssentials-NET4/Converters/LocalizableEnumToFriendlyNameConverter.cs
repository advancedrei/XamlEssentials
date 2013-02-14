using System;
using System.Globalization;
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
    /// This class simply takes an enum and uses some reflection to obtain
    /// the friendly name for the enum. Where the friendlier name is
    /// obtained using the LocalizableDescriptionAttribute, which hold the localized
    /// value read from the resource file for the enum
    /// </summary>
#if !(SILVERLIGHT || WINDOWS_PHONE || WINRT)
    [ValueConversion(typeof(object), typeof(String))]
#endif
    public class EnumToLocalizableFriendlyNameConverter :
#if !(SILVERLIGHT || WINDOWS_PHONE || WINRT)
        ConvertibleMarkupExtension<EnumToLocalizableFriendlyNameConverter>,
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
            // To get around the stupid wpf designer bug
            if (value != null)
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());

                // To get around the stupid wpf designer bug
                if (fi != null)
                {
                    var attributes =
                        (LocalizableDescriptionAttribute[])fi.GetCustomAttributes(typeof(LocalizableDescriptionAttribute), false);

                    return ((attributes.Length > 0) &&
                            (!String.IsNullOrEmpty(attributes[0].Description)))
                               ?
                                   attributes[0].Description
                               : value.ToString();
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
#if WINRT
            string language
#else
            System.Globalization.CultureInfo culture
#endif
)
        {
            throw new Exception("Cant convert back");
        }

    }

}
