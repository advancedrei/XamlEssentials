#if WINDOWS_PHONE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Phone.Info;

namespace XamlEssentials
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

        //Note: to get a result requires ID_CAP_IDENTITY_DEVICE
        // to be added to the capabilities of the WMAppManifest
        // this will then warn users in marketplace
        public static string GetDeviceUniqueId()
        {
            byte[] result = null;
            object uniqueId;
            if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))
                result = (byte[])uniqueId;

            return result != null ? Convert.ToBase64String(result) : "";
        }

        // NOTE: to get a result requires ID_CAP_IDENTITY_USER
        //  to be added to the capabilities of the WMAppManifest
        // this will then warn users in marketplace
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