using System;
using System.Diagnostics;
using System.Windows;

namespace XamlEssentials.Helpers
{

    /// <summary>
    /// Helper functions for Beta applications.
    /// </summary>
    public static class BetaExperienceHelper
    {
        
        /// <summary>
        /// Checks to see if the app fulfills the requirements to be prompted for Beta expiration.
        /// </summary>
        /// <param name="calculateFrom">Specifies whether to check from the time the app was first installed, or when just the latest version was installed.</param>
        /// <param name="daysFromInstall">The number of days from the specified install type to check for.</param>
        /// <param name="showMarketplace">Specifies whether to show the Marketplace to check for an app update when the user clicks OK. Defaults to true.</param>
        /// <remarks>Requires that <see ref="StatsHelper.Initialize" /> be called prior to making this call.</remarks>
        public static void CheckExpiration(CalculateFrom calculateFrom, int daysFromInstall, bool showMarketplace = true)
        {
            if (daysFromInstall <= 0)
            {
                throw new ArgumentOutOfRangeException("daysFromInstall", "The days from installation must be greater than 0.");
            }

#if WINDOWS_PHONE
            if (!Debugger.IsAttached)
            {
                if (!ApplicationInfoHelper.IsBeta) return;
            }
#endif

            StatsHelper.CheckIsInitialized();
            bool isExpired = false;
            switch (calculateFrom)
            {
                case CalculateFrom.FirstVersion:
                    isExpired = StatsHelper.InitialVersionInstallDate.AddDays(daysFromInstall) >= DateTime.Today;
                    break;
                case CalculateFrom.CurrentVersion:
                    isExpired = StatsHelper.CurrentVersionInstallDate.AddDays(daysFromInstall) >= DateTime.Today;
                    break;
            }
            if (isExpired)
            {
                ShowPrompt(showMarketplace);
            }
        }

        /// <summary>
        /// Checks to see if the app fulfills the requirements to be prompted for Beta expiration.
        /// </summary>
        /// <param name="specificDate">The specific date from which to expire the application.</param>
        /// <param name="showMarketplace">Specifies whether to show the Marketplace to check for an app update when the user clicks OK. Defaults to true.</param>
        public static void CheckExpiration(DateTime specificDate, bool showMarketplace = true)
        {
            if (specificDate <= DateTime.Today)
            {
                ShowPrompt(showMarketplace);
            }
        }

        /// <summary>
        /// Displays a prompt for the user notifying them that the app has expired, optionally taking them to the Store.
        /// </summary>
        /// <param name="showMarketplace">Specifies whether to show the Marketplace to check for an app update when the user clicks OK.</param>
        private static void ShowPrompt(bool showMarketplace)
        {
            var message = showMarketplace 
                ? "This beta has expired. Would you like to check for an update in the Store?\nIf you choose to cancel, this app will close." 
                : "This beta has expired and will now close. Thank you for your participation.\nPlease check the Store for the latest public release.";
            var result = MessageBox.Show(message, ApplicationInfoHelper.Title, showMarketplace ? MessageBoxButton.OKCancel : MessageBoxButton.OK);
            
            if (result == MessageBoxResult.OK && showMarketplace)
            {
#if WINDOWS_PHONE
                Windows.System.Launcher.LaunchUriAsync(new Uri("zune:navigate?appid=" + ApplicationInfoHelper.ProductId));
#endif
            }
            Application.Current.Terminate();
        }


    }
}
