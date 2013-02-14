using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Microsoft.Phone.UserData
{
    public static class ContactExtensions
    {

        /// <summary>
        /// An extension method that gets the ImageSource for this Contact's Picture.
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public static ImageSource GetImageSource(this Contact contact)
        {
            System.IO.Stream imageStream = contact.GetPicture();
            if (null != imageStream)
            {
                return PictureDecoder.DecodeJpeg(imageStream);
            }
            return null;
        }


    }
}
