/* 
    Copyright (c) 2012 - 2013 Microsoft Corporation.  All rights reserved.
    Use of this sample source code is subject to the terms of the Microsoft license 
    agreement under which you licensed this sample source code and is provided AS-IS.
    If you did not accept the terms of the license agreement, you are not authorized 
    to use this sample source code.  For the terms of the license, please see the 
    license agreement between you and Microsoft.
  
    To see all Code Samples for Windows Phone, visit http://code.msdn.microsoft.com/wpapps
    
    Modified by Robert McLaws.
*/

#if !WP71

using System.Diagnostics;
using Windows.ApplicationModel.Store;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace XamlEssentials.Helpers
{

    /// <summary>
    /// The TrialExperienceHelper class can serve as a convenient building-block in your app's trial experience implementation. It queries
    /// the license as infrequently as possible, and caches the results, for maximum performance. When built in debug configuration, the class simulates the purchase
    /// experience when the Buy method is called. To customize what happens in debug when Buy is called, initialize the simulatedLicMode and/or
    /// simulatedLicModeOnPurchase fields to different values. A release build of this class will have access to a license only when the app is
    /// published to the Windows Phone Store. For a release build not published to the Store, the value of the LicenseMode property will be
    /// LicenseModes.MissingOrRevoked.
    /// </summary>
    public static class TrialExperienceHelper
    {
        #region Enums

        /// <summary>
        /// The LicenseModes enumeration describes the mode of a license.
        /// </summary>
        public enum LicenseModes
        {
            Full,
            MissingOrRevoked,
            Trial
        }

        #endregion enums

        #region Fields

        // Determines how a debug build behaves on launch. This field is set to LicenseModes.Full after simulating a purchase.
        // Calling the Buy method (or navigating away from the app and back) will simulate a purchase.
        internal static LicenseModes SimulatedLicenseMode = LicenseModes.Trial;

        private static bool _isActiveCache;
        private static bool _isTrialCache;

        #endregion fields

        #region Constructors

        // The static constructor effectively initializes the cache of the state of the license when the app is launched. It also attaches
        // a handler so that we can refresh the cache whenever the license has (potentially) changed.
        static TrialExperienceHelper()
        {
            RefreshCache();
            PhoneApplicationService.Current.Activated += (sender, e) => 
                {
                    if (Debugger.IsAttached)
                    {
                        // In debug configuration, when the user returns to the application we will simulate a purchase.
                        OnSimulatedPurchase();
                    }
                    else
                    {
                        RefreshCache();
                    }
                };
        }

        #endregion constructors

        #region Properties

        /// <summary>
        /// The LicenseMode property combines the active and trial states of the license into a single
        /// enumerated value. In debug configuration, the simulated value is returned. In release configuration,
        /// if the license is active then it is either trial or full. If the license is not active then
        /// it is either missing or revoked.
        /// </summary>
        public static LicenseModes LicenseMode
        {
            get
            {
                if (Debugger.IsAttached)
                {
                    return SimulatedLicenseMode;
                }

                if (_isActiveCache)
                {
                    return _isTrialCache ? LicenseModes.Trial : LicenseModes.Full;
                }

                return LicenseModes.MissingOrRevoked;
            }
        }

        /// <summary>
        /// The IsFull property provides a convenient way of checking whether the license is full or not.
        /// </summary>
        public static bool IsFull
        {
            get { return (LicenseMode == LicenseModes.Full); }
        }

        #endregion properties

        #region Methods

        /// <summary>
        /// The Buy method can be called when the license state is trial. the user is given the opportunity
        /// to buy the app after which, in all configurations, the Activated event is raised, which we handle.
        /// </summary>
        public static void Buy()
        {
            var marketplaceDetailTask = new MarketplaceDetailTask {ContentType = MarketplaceContentType.Applications};
            marketplaceDetailTask.Show();
        }

        /// <summary>
        /// This method can be called at any time to refresh the values stored in the cache. We re-query the application object
        /// for the current state of the license and cache the fresh values. We also raise the LicenseChanged event.
        /// </summary>
        public static void RefreshCache()
        {
            _isActiveCache = CurrentApp.LicenseInformation.IsActive;
            _isTrialCache = CurrentApp.LicenseInformation.IsTrial;
            RaiseLicenseChanged();
        }

        private static void RaiseLicenseChanged()
        {
            if (LicenseChanged != null)
            {
                LicenseChanged();
            }
        }


        private static void OnSimulatedPurchase()
        {
            SimulatedLicenseMode = LicenseModes.Full;
            RaiseLicenseChanged();
        }

        #endregion methods

        #region Events

        /// <summary>
        /// The static LicenseChanged event is raised whenever the value of the LicenseMode property has (potentially) changed.
        /// </summary>
        public static event LicenseChangedEventHandler LicenseChanged;

        #endregion events

    }
}
#endif