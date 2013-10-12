using System;
using System.Diagnostics;
using System.Windows;
using XamlEssentials.Storage;

namespace XamlEssentials.Helpers
{

    /// <summary>
    /// This tool helps gather 
    /// </summary>
    public static class StatsHelper
    {

        #region Private Members

        private static readonly StoredItem<string> _initialVersion = new StoredItem<string>("initialVersion", ApplicationInfoHelper.Version);
        private static readonly StoredItem<DateTime> _initialVersionInstallDate = new StoredItem<DateTime>("initialVersionInstallDate", DateTime.UtcNow);
        private static readonly StoredItem<string> _currentVersion = new StoredItem<string>("currentVersion", ApplicationInfoHelper.Version);
        private static readonly StoredItem<long> _currentVersionExceptionCount = new StoredItem<long>("currentVersionExceptionCount", 0);
        private static readonly StoredItem<DateTime> _currentVersionInstallDate = new StoredItem<DateTime>("currentVersionInstallDate", DateTime.UtcNow);
        private static readonly StoredItem<long> _currentVersionRunCount = new StoredItem<long>("currentVersionRunCount", 0);
        private static readonly StoredItem<long> _totalExceptionCount = new StoredItem<long>("totalExceptionCount", 0);
        private static readonly StoredItem<long> _totalRunCount = new StoredItem<long>("totalRunCount", 0);
        private static readonly StoredItem<DateTime> _lastRunDate = new StoredItem<DateTime>("lastRunDate", DateTime.Now);
        internal static bool IsInitialized = false;

        #endregion

        #region Properties

        /// <summary>
        /// The currently-executing version of the app, as stored by the <see cref="StatsHelper"/> .
        /// </summary>
        /// <remarks>This is the value stored in IsolatedStoage, and may not match the version returned by <see cref="ApplicationInfoHelper"/>.</remarks>
        public static string CurrentVersion
        {
            get
            {
                CheckIsInitialized(); 
                return _currentVersion.Value;
            }
            set { _currentVersion.Value = value; }
        }

        /// <summary>
        /// The number of exceptions that have been recorded for the <see cref="CurrentVersion"/>.
        /// </summary>
        /// <remarks>Exceptions are not counted when the Debugger is attached.</remarks>
        public static long CurrentVersionExceptionCount
        {
            get
            {
                CheckIsInitialized(); 
                return _currentVersionExceptionCount.Value;
            }
            set { _currentVersionExceptionCount.Value = value; }
        }

        /// <summary>
        /// The date the <see cref="CurrentVersion"/> was installed.
        /// </summary>
        public static DateTime CurrentVersionInstallDate
        {
            get
            {
                CheckIsInitialized(); 
                return _currentVersionInstallDate.Value;
            }
            set { _currentVersionInstallDate.Value = value; }
        }

        /// <summary>
        /// The number of times the <see cref="CurrentVersion"/> has run since installation.
        /// </summary>
        public static long CurrentVersionRunCount
        {
            get
            {
                CheckIsInitialized(); 
                return _currentVersionRunCount.Value;
            }
            set { _currentVersionRunCount.Value = value; }
        }

        /// <summary>
        /// The version of this app that was initially installed on the system.
        /// </summary>
        public static string InitialVersion
        {
            get
            {
                CheckIsInitialized(); 
                return _initialVersion.Value;
            }
            set { _initialVersion.Value = value; }
        }

        /// <summary>
        /// The date this app was initially installed on the system.
        /// </summary>
        public static DateTime InitialVersionInstallDate
        {
            get
            {
                CheckIsInitialized();
                return _initialVersionInstallDate.Value;
            }
            set { _initialVersionInstallDate.Value = value; }
        }

        /// <summary>
        /// The date the app was last run.
        /// </summary>
        public static DateTime LastRunDate
        {
            get
            {
                CheckIsInitialized(); 
                return _lastRunDate.Value;
            }
            set { _lastRunDate.Value = value; }
        }

        /// <summary>
        /// The total number of exceptions that have ever been recorded for this application.
        /// </summary>
        /// <remarks>Exceptions are not counted when the Debugger is attached.</remarks>
        public static long TotalExceptionCount
        {
            get
            {
                CheckIsInitialized(); 
                return _totalExceptionCount.Value;
            }
            set { _totalExceptionCount.Value = value; }
        }


        /// <summary>
        /// The number of times this app has run since <see cref="InitialVersionInstallDate"/>.
        /// </summary>
        public static long TotalRunCount
        {
            get
            {
                CheckIsInitialized(); 
                return _totalRunCount.Value;
            }
            set { _totalRunCount.Value = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Should be called whenever the app is first run, as early as possible.
        /// </summary>
        public static void Initialize()
        {
            if (CurrentVersion == ApplicationInfoHelper.Version)
            {
                CurrentVersionRunCount++;
            }
            else
            {
                CurrentVersionInstallDate = DateTime.UtcNow;
                CurrentVersionRunCount = 1;
                CurrentVersionExceptionCount = 0;
            }
            TotalRunCount++;
            LastRunDate = DateTime.UtcNow;
            IsInitialized = true;
            Application.Current.UnhandledException += RecordUnhandledException;
        }

        /// <summary>
        /// Should be called whenever the app is shutting down, as early as possible.
        /// </summary>
        public static void TearDown()
        {
            Application.Current.UnhandledException -= RecordUnhandledException;
        }

        #endregion

        #region Private Methods

        private static void CheckIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("The XamlEssentials StatsHelper is not initialized. Please call the Initialize() method in the constructor for App.cs or App.vb.");
            }
        }

        #endregion

        #region Event Handlers

        public static void RecordUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            if (Debugger.IsAttached) return;
            CurrentVersionExceptionCount++;
            TotalExceptionCount++;
        }

        #endregion

    }
}
