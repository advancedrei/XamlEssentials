using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Microsoft.Phone.UserData;

namespace XamlEssentials.Converters
{

    /// <summary>
    /// Converts a Microsoft.Phone.UserData.Contact into their Contact Picture.
    /// </summary>
    public class ContactPictureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var c = value as Contact;
            return c == null ? null : c.GetImageSource();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
