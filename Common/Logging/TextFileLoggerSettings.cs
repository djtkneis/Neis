using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neis.Logging
{
    /// <summary>
    /// Settings to use for a <see cref="TextFileLogger"/>
    /// </summary>
    public class TextFileLoggerSettings : LoggerSettings
    {
        /// <summary>
        /// Gets or sets the path of the text file to write to
        /// </summary>
        public string FilePath { get; set; }
    }
}