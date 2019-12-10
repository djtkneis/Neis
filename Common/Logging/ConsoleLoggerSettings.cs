using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neis.Logging
{
    /// <summary>
    /// Settings to use for a <see cref="ConsoleLogger"/>
    /// </summary>
    public class ConsoleLoggerSettings : LoggerSettings
    {
        /// <summary>
        /// Color to use for the default foreground
        /// </summary>
        public ConsoleColor DefaultForegroundColor { get; set; }
        /// <summary>
        /// Color to use for the default background
        /// </summary>
        public ConsoleColor DefaultBackgroundColor { get; set; }
        /// <summary>
        /// Color to use for the foreground of an error message
        /// </summary>
        public ConsoleColor ErrorForegroundColor { get; set; }
        /// <summary>
        /// Color to use for the background of an error message
        /// </summary>
        public ConsoleColor ErrorBackgroundColor { get; set; }
        /// <summary>
        /// Color to use for the foreground of an information message
        /// </summary>
        public ConsoleColor InformationForegroundColor { get; set; }
        /// <summary>
        /// Color to use for the background of an information message
        /// </summary>
        public ConsoleColor InformationBackgroundColor { get; set; }
        /// <summary>
        /// Color to use for the foreground of a warning message
        /// </summary>
        public ConsoleColor WarningForegroundColor { get; set; }
        /// <summary>
        /// Color to use for the background of a warning message
        /// </summary>
        public ConsoleColor WarningBackgroundColor { get; set; }
    }
}