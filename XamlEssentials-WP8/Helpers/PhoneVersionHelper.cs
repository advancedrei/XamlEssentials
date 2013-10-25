using System;

namespace XamlEssentials.Helpers
{

    /// <summary>
    /// Assists with discovering functionality only available to specific GDRs of Windows Phone.
    /// </summary>
    public static class PhoneVersionHelper
    {

        #region Private Members

        private static readonly Version Gdr3Version = new Version(8, 0, 10492);
        private static readonly Version Gdr2Version = new Version(8, 0, 10327);
        private static readonly Version Gdr1Version = new Version(8, 0, 10211);
        private static bool _isBatterySaverActive = false;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current GDR version currently installed on a Windows Phone 8 device.
        /// </summary>
        public static WindowsPhone8Versions InstalledGdrVersion
        {
            get
            {
                var currentversion = Environment.OSVersion.Version;
                if (currentversion >= Gdr3Version) return WindowsPhone8Versions.GDR3;
                if (currentversion < Gdr3Version && currentversion >= Gdr2Version) return WindowsPhone8Versions.GDR2;
                if (currentversion < Gdr2Version && currentversion >= Gdr1Version) return WindowsPhone8Versions.GDR1;
                return WindowsPhone8Versions.RTM;
            }
        }

        /// <summary>
        /// Returns whether or not the BatterySaver is presently on. Useful for warning users if they won't be able to receive push notifications.
        /// </summary>
        /// <remarks>
        /// Will only be true if GDR3 is installed, and the Battery Saver is on. Otherwise it will be false.
        /// Adapted from code originally posted by Marco Minerva. (http://marcominerva.wordpress.com/2013/10/21/using-the-new-powersavingmodeenabled-property-in-windows-phone-8-update-3/)
        /// </remarks>
        public static bool IsBatterySaverActive
        {
            get
            {
                if (InstalledGdrVersion != WindowsPhone8Versions.GDR3) return _isBatterySaverActive;

                var powerManagerType = typeof(Windows.Phone.System.Power.PowerManager);
                var powerSavingModeEnabledProperty = powerManagerType.GetProperty("PowerSavingModeEnabled");
                if (powerSavingModeEnabledProperty != null)
                {
                    _isBatterySaverActive = (bool) powerSavingModeEnabledProperty.GetValue(null);
                }
                return _isBatterySaverActive;
            }
        }

        #endregion

    }

}