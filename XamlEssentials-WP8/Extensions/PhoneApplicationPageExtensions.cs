using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlEssentials.Helpers;

namespace Microsoft.Phone.Controls
{
    public static class PhoneApplicationPageExtensions
    {

        /// <summary>
        /// Disables the ability to capture the contents of the screen for a given PhoneApplicationPage.
        /// </summary>
        /// <param name="page">The instance of the <see cref="PhoneApplicationPage"/> to disable screenshots for.</param>
        public static void DisableScreenshots(this PhoneApplicationPage page)
        {
            DisplayHelper.SetScreenCaptureEnabled(page, false);
        }

        /// <summary>
        /// Enables the ability to capture the contents of the screen for a given PhoneApplicationPage.
        /// </summary>
        /// <param name="page">The instance of the <see cref="PhoneApplicationPage"/> to enable screenshots for.</param>
        public static void EnableScreenshots(this PhoneApplicationPage page)
        {
            DisplayHelper.SetScreenCaptureEnabled(page, true);
        }

    }
}
