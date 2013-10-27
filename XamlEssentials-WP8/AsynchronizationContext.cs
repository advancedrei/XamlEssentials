using System;
using System.Threading;
#if WINDOWS_PHONE
using System.Windows.Controls;
#elif WINRT
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
#endif
using System.Windows;


namespace XamlEssentials
{

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This technique was adapted from MarkerMetro (http://www.markermetro.com/2013/01/technical/handling-unhandled-exceptions-with-asyncawait-on-windows-8-and-windows-phone-8/),
    /// which was in turn adapted from the work of Mark Young. (https://github.com/kiwidev/WinRTExceptions/blob/master/Source/ExceptionHandlingSynchronizationContext.cs)
    /// </remarks>
    public class AsynchronizationContext : SynchronizationContext
    {

        #region Private Members

        private readonly SynchronizationContext _syncContext;

        #endregion

        #region Events

        /// <summary>
        /// Raised when an unhandled exception occurs in the SynchronizationContext.
        /// </summary>
        public static event EventHandler<AsyncExceptionEventArgs> AsyncException; 

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="syncContext"></param>
        public AsynchronizationContext(SynchronizationContext syncContext)
        {
            _syncContext = syncContext;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Wraps the current SynchronizationContext with one that will handle "async void" exceptions, 
        /// and sets the wrapped context as the application's default Context.
        /// </summary>
        /// <returns>The currently-running instance of the AsynchronizationContext.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <remarks>This method can be called more than once. If the context has alread been registered, the function will short-circuit.</remarks>
        public static AsynchronizationContext Register()
        {
            var syncContext = Current;

            if (syncContext == null)
                throw new InvalidOperationException("Ensure a synchronization context exists before calling this method.");

            var customContext = syncContext as AsynchronizationContext;
            if (customContext != null) return customContext;

            customContext = new AsynchronizationContext(syncContext);
            SetSynchronizationContext(customContext);

            var newContext = Current;

            return customContext;
        }

        #endregion

        #region Base Overrides

        public override SynchronizationContext CreateCopy()
        {
            return new AsynchronizationContext(_syncContext.CreateCopy());
        }

        public override void OperationCompleted()
        {
            _syncContext.OperationCompleted();
        }

        public override void OperationStarted()
        {
            _syncContext.OperationStarted();
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            _syncContext.Post(WrapCallback(d), state);
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            _syncContext.Send(d, state);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sendOrPostCallback">Represents a method to be called when a message is to be dispatched to a synchronization context.</param>
        /// <returns></returns>
        private static SendOrPostCallback WrapCallback(SendOrPostCallback sendOrPostCallback)
        {
            return state =>
            {
                Exception exception = null;

                try
                {
                    sendOrPostCallback(state);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                if (exception != null)
                {
                    OnUnhandledException(new AsyncExceptionEventArgs(exception, false));
                }
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected static void OnUnhandledException(AsyncExceptionEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of 
            // a race condition if the last subscriber unsubscribes 
            // immediately after the null check and before the event is raised.
            var handler = AsyncException;

            // Event will be null if there are no subscribers 
            if (handler != null)
            {
                // Use the () operator to raise the event.
                handler(null, e);
            }
        }

        #endregion

        /// <summary>
        /// Links the synchronization context to the specified frame
        /// and ensures that it is still in use after each navigation event
        /// </summary>
        /// <param name="rootFrame"></param>
        /// <returns></returns>
        public static AsynchronizationContext RegisterForFrame(Frame rootFrame)
        {
            if (rootFrame == null)
                throw new ArgumentNullException("rootFrame");

            var synchronizationContext = Register();

            rootFrame.Navigating += (sender, args) => EnsureContext(synchronizationContext);
            rootFrame.Loaded += (sender, args) => EnsureContext(synchronizationContext);

            return synchronizationContext;
        }


        private static void EnsureContext(SynchronizationContext context)
        {
            if (Current != context)
                SetSynchronizationContext(context);
        }


    }

}
