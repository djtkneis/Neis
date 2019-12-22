using Neis.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Neis.ProductKeyManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static LogWriter _logWriter;
        
        /// <summary>
        /// Gets the singleton instance of the log writer
        /// </summary>
        internal static LogWriter LogWriter
        {
            get
            {
                if (_logWriter == null)
                {
                    _logWriter = InitializeLogWriter();
                }
                return _logWriter;
            }
        }

        /// <summary>
        /// Initializes a new instance of the log writer
        /// </summary>
        /// <returns>New instance of the log writer</returns>
        private static LogWriter InitializeLogWriter()
        {
            var logWriter = new LogWriter("ProductKeyManager", "Application");
            logWriter.AddLogger(
                new ConsoleLogger(
                    new ConsoleLoggerSettings()
                    {
                        LogLevel = LogMessageType.Verbose,
                        TimeStampOnError = true,
                        TimeStampOnNone = true,
                        TimeStampOnInformation = true,
                        TimeStampOnWarning = true,
                        ShowMessageTypePrefixes = true
                    }
                )
            );

            var sLogLevel = Neis.ProductKeyManager.Properties.Settings.Default.LogLevel;
            var logLevel = LogMessageType.Error;
            switch (sLogLevel.ToUpper())
            {
                case "VERBOSE":
                    logLevel = LogMessageType.Verbose;
                    break;
                case "INFORMATION":
                    logLevel = LogMessageType.Information;
                    break;
                case "WARNING":
                    logLevel = LogMessageType.Warning;
                    break;
                case "ERROR":
                    logLevel = LogMessageType.Error;
                    break;
                case "NONE":
                    logLevel = LogMessageType.None;
                    break;
                default:
                    logWriter.ShowError(string.Format("Unrecognized config file setting LogLevel = '{0}'", sLogLevel));
                    break;
            }

            logWriter.AddLogger(
                new TextFileLogger(
                    new TextFileLoggerSettings()
                    {
                        FilePath = Neis.ProductKeyManager.Properties.Settings.Default.LogFile,
                        LogLevel = logLevel,
                        TimeStampOnError = true,
                        TimeStampOnNone = true,
                        TimeStampOnInformation = true,
                        TimeStampOnWarning = true,
                        ShowMessageTypePrefixes = true                                
                    }
                )
            );

            return logWriter;
        }
    }
}
