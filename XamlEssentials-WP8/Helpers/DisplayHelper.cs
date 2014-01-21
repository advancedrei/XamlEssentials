using System;
using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Info;

namespace XamlEssentials.Helpers
{

    /// <summary>
    /// Helper methods adapted from Nokia's official guidance for dealing with devices with larger screen sizes.
    /// </summary>
    /// <remarks>
    /// See this Nokia help topic for more information: http://developer.nokia.com/Resources/Library/Lumia/#!optimising-for-nokia-phablets.html
    /// </remarks>
    public static class DisplayHelper
    {

        #region Private members

        private static double _screenSize = -1.0f;
        private static double _screenDpiX = 0.0f;
        private static double _screenDpiY = 0.0f;
        private static Size _resolution;

        #endregion

        #region Dependency Properties

        /// <summary>
        /// Attached property that lets you disable screenshots from XAML.
        /// </summary>
        /// <remarks>Not intended to be used from your application code.</remarks>
        public static readonly DependencyProperty ScreenCaptureEnabledProperty =
            DependencyProperty.RegisterAttached("ScreenCaptureEnabled", typeof(Boolean), typeof(PhoneApplicationPage), new PropertyMetadata(true));

        #endregion

        #region Public Methods

        /// <summary>
        /// NOT INTENDED TO BE CALLED FROM YOUR APPLICATION CODE.
        /// On GDR2 and later: Sets the "IsScreenCaptureEnabled" property on the given PhoneApplicationPage. 
        /// On GDR1 and earlier: Does nothing.
        /// </summary>
        /// <param name="page">The <see cref="PhoneApplicationPage"/> to set the value for.</param>
        /// <param name="value">The value to set.</param>
        /// <remarks>Adapted from code originally posted by Pedro Lamas. http://www.pedrolamas.com/2014/01/20/disabling-screenshot-functionality-in-a-windows-phone-app/ </remarks>
        public static void SetScreenCaptureEnabled(PhoneApplicationPage page, Boolean value)
        {
            if (!PhoneVersionHelper.HasGdr2) return;
            var propertyInfo = typeof(PhoneApplicationPage).GetProperty("IsScreenCaptureEnabled");
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(page, value);
            }
        }

        /// <summary>
        /// Signifies whether or not the ability to capture the screen contents is enabled. Will not throw an exception if property is not supported.
        /// </summary>
        /// <param name="page">The <see cref="PhoneApplicationPage"/> to get the value from.</param>
        /// <returns>
        /// On GDR2 and later: Returns whether or not screen captures are enabled. 
        /// On GDR1 and earlier: Returns false.</returns>
        /// <remarks>Adapted from code originally posted by Pedro Lamas. http://www.pedrolamas.com/2014/01/20/disabling-screenshot-functionality-in-a-windows-phone-app/ </remarks>
        public static Boolean GetScreenCaptureEnabled(PhoneApplicationPage page)
        {
            var propertyInfo = typeof(PhoneApplicationPage).GetProperty("IsScreenCaptureEnabled");
            if (propertyInfo == null) return true;
            return (bool)propertyInfo.GetValue(page);
        }


        /// <summary>
        /// Detects if the physical display size of the device is larger than 5" diagonal.
        /// </summary>
        /// <remarks>
        /// Adapted from http://developer.nokia.com/Resources/Library/Lumia/#!optimising-for-nokia-phablets/optimising-layout-for-big-screens.html
        /// </remarks>
        public static bool IsPhablet
        {
            get
            {
                // Use 720p emulator to simulate big screen.
                if (Microsoft.Devices.Environment.DeviceType == Microsoft.Devices.DeviceType.Emulator)
                {
                    _screenSize = (Application.Current.Host.Content.ScaleFactor == 150) ? 6.0f : 0.0f;
                }

                if (_screenSize == -1.0f)
                {
                    try
                    {
                        _screenDpiX = (double)DeviceExtendedProperties.GetValue("RawDpiX");
                        _screenDpiY = (double)DeviceExtendedProperties.GetValue("RawDpiY");
                        _resolution = (Size)DeviceExtendedProperties.GetValue("PhysicalScreenResolution");

                        // Calculate screen diagonal in inches.
                        _screenSize =
                            Math.Sqrt(Math.Pow(_resolution.Width / _screenDpiX, 2) +
                                      Math.Pow(_resolution.Height / _screenDpiY, 2));
                    }
                    catch (Exception e)
                    {
                        // We're on older software with lower screen size, carry on.
                        Debug.WriteLine("IsPhablet error: " + e.Message);
                        _screenSize = 0;
                    }
                }

                // Returns true if screen size is bigger than 5 inches - you may edit the value based on your app's needs.
                return (_screenSize > 5.0f);
            }
        }

        /// <summary>
        /// If the device is detected to be a Phablet, the referenced XAML file will be added to the application's MergedDictionaries at runtime.
        /// </summary>
        /// <param name="styleXamlUrl">The absolute Url of the XAML file, in Silverlight Component notation.</param>
        /// <example>
        /// DisplayHelper.AddPhabletStyle("/YourAssemblyName;component/Themes/ItemStyle1080p.xaml");
        /// </example>
        /// <remarks>
        /// Adapted from http://developer.nokia.com/Resources/Library/Lumia/#!optimising-for-nokia-phablets/optimising-layout-for-big-screens.html
        /// </remarks>
        [Obsolete("Use ResourceHelper.AddPhabletStyle() instead.", false)]
        public static void AddPhabletStyle(string styleXamlUrl)
        {
            ResourceHelper.AddPhabletStyle(styleXamlUrl);
        }

        /// <summary>
        /// Retrieves the resolution the system is currently displaying at.
        /// </summary>
        /// <exception cref="InvalidOperationException">Is thrown if the resolution does not match any of the known options.</exception>
        /// <remarks>
        /// Adapted from http://developer.nokia.com/Resources/Library/Lumia/#!optimising-for-nokia-phablets/resolution-specific-considerations.html
        /// </remarks>
        public static DisplayResolutions CurrentResolution
        {
            get
            {
                if (IsWvga) return DisplayResolutions.WVGA;
                if (IsWxga) return DisplayResolutions.WXGA;
                if (Is720p) return DisplayResolutions.HD720p;
                if (Is1080p) return DisplayResolutions.HD1080p;
                throw new InvalidOperationException("Unknown resolution");
            }
        }

        #endregion

        #region Private Methods

        private static bool IsWvga
        {
            get
            {
                return Application.Current.Host.Content.ScaleFactor == 100;
            }
        }

        private static bool IsWxga
        {
            get
            {
                return Application.Current.Host.Content.ScaleFactor == 160;
            }
        }

        private static bool Is720p
        {
            get
            {
                return (Application.Current.Host.Content.ScaleFactor == 150 && !Is1080p);
            }
        }

        private static bool Is1080p
        {
            get
            {
                if (_resolution.Width != 0) return _resolution.Width == 1080;
                try
                {
                    _resolution = (Size)DeviceExtendedProperties.GetValue("PhysicalScreenResolution");
                }
                catch (Exception)
                {
                    _resolution.Width = 0;
                }
                return _resolution.Width == 1080;
            }
        }

        #endregion

    }
}
