#if WINDOWS_PHONE
using System;
using Microsoft.Phone.Info;

namespace XamlEssentials.Helpers
{

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Based on code from http://www.nickharris.net/2010/09/windows-phone-7-how-to-find-the-device-unique-id-windows-live-anonymous-id-and-manufacturer/,
    /// However, the Manufacturer now comes from DeviceStatus.
    /// </remarks>
    public static class PhoneExtendedPropertiesHelper
    {

        #region Private Members

        private const int AnidLength = 32;
        private const int AnidOffset = 2;

        #endregion

        /// <summary>
        /// Gets the unique ID for the device.
        /// </summary>
        /// <returns>A string containing the unique ID for the device.</returns>
        /// <remarks>
        /// This requires ID_CAP_IDENTITY_DEVICE to be set in the Manifest. This in turn will 
        /// trigger a warning to the user when installing from the Store.
        /// </remarks>
        public static string GetDeviceUniqueId()
        {
            byte[] result = null;
            object uniqueId;
            if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))
                result = (byte[])uniqueId;

            return result != null ? Convert.ToBase64String(result) : "";
        }

        /// <summary>
        /// Gets the anonymous ID for the user's primary Windows Live account.
        /// </summary>
        /// <returns>A string containing the anonymous ID for the user.</returns>
        /// <remarks>
        /// This requires ID_CAP_IDENTITY_USER to be set in the Manifest. This in turn will 
        /// trigger a warning to the user when installing from the Store.
        /// </remarks>

        public static string GetWindowsLiveAnonymousId()
        {
            string result = string.Empty;
            object anid;
#if WP8
            if (UserExtendedProperties.TryGetValue("ANID2", out anid))
#else
            if (UserExtendedProperties.TryGetValue("ANID", out anid))
#endif
            {
                if (anid != null && anid.ToString().Length > (AnidLength + AnidOffset))
                {
                    result = anid.ToString().Substring(AnidOffset, AnidLength);
                }
            }

            return result;
        }

    }

}
#endif