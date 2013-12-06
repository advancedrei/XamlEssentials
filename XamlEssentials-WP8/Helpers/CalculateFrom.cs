using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlEssentials.Helpers
{

    /// <summary>
    /// Determines the point of reference for calculating Beta Expiration dates.
    /// </summary>
    public enum CalculateFrom
    {
        /// <summary>
        /// Signifies that we want to calculate the Beta Expiration from the date the application 
        /// was first installed.
        /// </summary>
        FirstVersion,

        /// <summary>
        /// Signifies that we want to calculate the Beta Expiration from the date the currently 
        /// executing version of application was first installed.
        /// </summary>
        CurrentVersion
    }
}
