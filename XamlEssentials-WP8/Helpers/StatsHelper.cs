using System;
using System.Diagnostics;
using System.Threading.Tasks;
using XamlEssentials.Storage;
#if WINDOWS_PHONE
using System.Windows;
#elif WINRT
using Windows.UI.Xaml;
#endif

namespace XamlEssentials.Helpers
{

    /// <summary>
    /// This tool helps gather information about the lifecycle of an application. For example, how many times the current version has crashed, or when it was first installed.
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
        private static readonly StoredItem<DateTime> _lastCleanShutdownDate = new StoredItem<DateTime>("lastCleanShutdownDate", DateTime.Now);
        private static bool _markExceptionsAsHandled = false;
        private static bool _handleAsyncExceptions = false;
        internal static bool IsInitialized = false;
        internal static bool IsInitializing = false;

        #endregion

        #region Properties

        #region CurrentVersion

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

        #endregion

        #region CurrentVersionExceptionCount

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

        #endregion

        #region CurrentVersionInstallDate

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

        #endregion

        #region CurrentVersionRunCount

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

        #endregion

        #region InitialVersion

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

        #endregion

        #region InitialVersionInstallDate

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

        #endregion

        #region LastCleanShutdownDate

        /// <summary>
        /// The date the app was last started, run, and shutdown without error.
        /// </summary>
        public static DateTime LastCleanShutdownDate
        {
            get
            {
                CheckIsInitialized();
                return _lastCleanShutdownDate.Value;
            }
            set { _lastCleanShutdownDate.Value = value; }
        }

        #endregion

        #region LastRunDate

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

        #endregion

        #region TotalExceptionCount

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

        #endregion

        #region TotalRunCount

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

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts tracking application lifecycle statistics.
        /// </summary>
        /// <param name="handleAsyncExceptions">
        /// This specifies whether the default SynchronizationContext should be replaced with one that can handle exceptions in "async void" methods.
        /// The default is True.
        /// <remarks>Ignored on Windows 8.1, as the built-in SynchronizationContext already handles "async void" exceptions.</remarks>
        /// </param>
        /// <param name="markExceptionsAsHandled">
        /// Specifies whether exception processing should stop after the counters record their information. 
        /// Default is False, should usually stay that way.
        /// </param>
        /// <remarks>Should be called whenever the app is first run, only once and as early as possible.</remarks>
        public static void Initialize(bool handleAsyncExceptions = true, bool markExceptionsAsHandled = false)
        {
            //RWM: Check to make sure some other implementing framework (like AppSupport)
            //     has not already initialized this helper.
            if (IsInitialized || IsInitializing) return;
            IsInitializing = true;
            _markExceptionsAsHandled = markExceptionsAsHandled;
            _handleAsyncExceptions = handleAsyncExceptions;

#if false
            if (handleAsyncExceptions)
            {
                AsynchronizationContext.Register();
                AsynchronizationContext.AsyncException += AsyncException;
            }
#endif
            TaskScheduler.UnobservedTaskException += UnobservedTaskException;
            Application.Current.UnhandledException += RecordUnhandledException;


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
            IsInitializing = false;
            IsInitialized = true;
        }

        /// <summary>
        /// Should be called whenever the app is shutting down, as early as possible.
        /// </summary>
        public static void TearDown()
        {
#if false
            if (_handleAsyncExceptions)
            {
                AsynchronizationContext.AsyncException -= AsyncException;
            }
#endif
            TaskScheduler.UnobservedTaskException -= UnobservedTaskException;
            Application.Current.UnhandledException -= RecordUnhandledException;
            LastCleanShutdownDate = DateTime.UtcNow;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Checks to see if the StatsHelper has been fully-initialized.
        /// </summary>
        /// <remarks>Short-circuits if the StatsHelper is currently in the process of initializing, so as to not throw exceptions.</remarks>
        internal static void CheckIsInitialized()
        {
            if (IsInitializing) return;
            if (!IsInitialized)
            {
                throw new InvalidOperationException("The XamlEssentials StatsHelper is not initialized. Please call the Initialize() method in the constructor for App.cs or App.vb.");
            }
        }

        /// <summary>
        /// Increments the internal exception counters.
        /// </summary>
        private static void IncrementExceptionCounters()
        {
            if (Debugger.IsAttached) return;
            CurrentVersionExceptionCount++;
            TotalExceptionCount++;
        }

        #endregion

        #region Event Handlers

#if WINDOWS_PHONE
        private static void RecordUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
#elif WINRT
        private static void RecordUnhandledException(object sender, UnhandledExceptionEventArgs e)
#endif
        {
            e.Handled = _markExceptionsAsHandled;
            IncrementExceptionCounters();
        }

        private static void UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            if (_markExceptionsAsHandled) e.SetObserved();
            IncrementExceptionCounters();
        }

#if false
        private static void AsyncException(object sender, AsyncExceptionEventArgs e)
        {
            e.Handled = _markExceptionsAsHandled;
            IncrementExceptionCounters();
            //if (!_markExceptionsAsHandled) throw e.Exception;
        }
#endif

        #endregion

    }
}
