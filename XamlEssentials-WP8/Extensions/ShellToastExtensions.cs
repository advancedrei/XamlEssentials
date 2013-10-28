using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Shell;
using XamlEssentials;
using XamlEssentials.Helpers;

namespace XMicrosoft.Phone.Shell
{

    /// <summary>
    /// 
    /// </summary>
    public static class ShellToastExtensions
    {

        #region Public Methods

        /// <summary>
        /// Shows a Toast Notification with sound on GDR3. Otherwise just shows the toast notification.
        /// </summary>
        /// <param name="toast">The ShellToast instance this function will execute against.</param>
        /// <param name="soundFileUri">
        /// The *local* sound file to play. This can be a .wav file, a .mp3 file, or an empty string. If you pass in an empty string, the toast will be silent.
        /// </param>
        /// <remarks>
        /// This is a modified version of Microsoft's official sample from http://msdn.microsoft.com/en-us/library/windowsphone/develop/jj662938(v=vs.105).aspx#BKMK_gdr3
        /// </remarks>
        public static void ShowWithSound(this ShellToast toast, string soundFileUri)
        {
            if (PhoneVersionHelper.HasGdr3)
            {
                SetProperty(toast, "Sound", new Uri(soundFileUri, UriKind.RelativeOrAbsolute));
            }
            toast.Show();
        }

        #endregion

        #region Private Methods

        private static void SetProperty(object instance, string name, object value)
        {
            var setMethod = instance.GetType().GetProperty(name).GetSetMethod();
            setMethod.Invoke(instance, new object[] { value });
        }

        #endregion

    }
}
