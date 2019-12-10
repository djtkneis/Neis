using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neis.Logging
{
    /// <summary>
    /// Interface for logger settings
    /// </summary>
    public class LoggerSettings
    {
        /// <summary>
        /// Level of logging to output
        /// </summary>
        public LogMessageType LogLevel { get; set; }
        /// <summary>
        /// Indicates whether or not to show the message types as prefixes to the message itself
        /// </summary>
        public bool ShowMessageTypePrefixes { get; set; }
        /// <summary>
        /// Indicates whether or not to automatically include the timestamp for error messages
        /// </summary>
        public bool TimeStampOnError { get; set; }
        /// <summary>
        /// Indicates whether or not to automatically include the timestamp for information messages
        /// </summary>
        public bool TimeStampOnInformation { get; set; }
        /// <summary>
        /// Indicates whether or not to automatically include the timestamp for raw messages
        /// </summary>
        public bool TimeStampOnNone { get; set; }
        /// <summary>
        /// Indicates whether or not to automatically include the timestamp for warning messages
        /// </summary>
        public bool TimeStampOnWarning { get; set; }
    }
}