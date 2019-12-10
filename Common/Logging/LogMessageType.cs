using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neis.Logging
{
    /// <summary>
    /// Types of log messages
    /// </summary>
    public enum LogMessageType : int
    {
        /// <summary>
        /// Verbose message
        /// </summary>
        Verbose = 0,
        /// <summary>
        /// Information message
        /// </summary>
        Information = 1,
        /// <summary>
        /// Warning message
        /// </summary>
        Warning = 2,
        /// <summary>
        /// Error message
        /// </summary>
        Error = 3,
        /// <summary>
        /// None
        /// </summary>
        None = 4,
    }
}