using System;

namespace XamlEssentials
{
    /// <summary>
    /// Provides data for the event that is raised when there is an exception that happens during an "async void" method call.
    /// </summary>
    public class AsyncExceptionEventArgs : EventArgs
    {

        /// <summary>
        /// The Exception thrown by the AsynchronizationContext
        /// </summary>
        public Exception Exception { get; internal set; }

        /// <summary>
        /// Whether or not the Exception should be marked as handled.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="isHandled"></param>
        public AsyncExceptionEventArgs(Exception exception, bool isHandled)
        {
            Exception = exception;
            Handled = isHandled;
        }

    }
}
