using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neis.Logging
{
    /// <summary>
    /// Logger used for writing messages to the console
    /// </summary>
    public class ConsoleLogger : LoggerBase
    {
        /// <summary>
        /// Constructor for the <see cref="ConsoleLogger"/> class
        /// </summary>
        /// <param name="settings">Settings for the logger</param>
        public ConsoleLogger(ConsoleLoggerSettings settings)
            :  base(settings)
        {
        }

        /// <summary>
        /// Writes a message to the console
        /// </summary>
        /// <param name="message">Message to write</param>
        /// <param name="type">Type of message to write</param>
        protected override void WriteMessage(string message, LogMessageType type)
        {
            ConsoleLoggerSettings consoleSettings = Settings as ConsoleLoggerSettings;

            string typePrefix = string.Format("[{0}]: ", type.ToString()[0]);
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt: ");

            bool showTimestamp = false;
            
            ConsoleColor foreground = Console.ForegroundColor;
            ConsoleColor background = Console.BackgroundColor;
            switch (type)
            {
                case LogMessageType.Information:
                    foreground = consoleSettings.InformationForegroundColor;
                    background = consoleSettings.InformationBackgroundColor;
                    showTimestamp = consoleSettings.TimeStampOnInformation;
                    break;

                case LogMessageType.Warning:
                    foreground = consoleSettings.WarningForegroundColor;
                    background = consoleSettings.WarningBackgroundColor;
                    showTimestamp = consoleSettings.TimeStampOnWarning;
                    break;

                case LogMessageType.Error:
                    foreground = consoleSettings.ErrorForegroundColor;
                    background = consoleSettings.ErrorBackgroundColor;
                    showTimestamp = consoleSettings.TimeStampOnError;
                    break;

                default:
                    foreground = consoleSettings.DefaultForegroundColor;
                    background = consoleSettings.DefaultBackgroundColor;
                    showTimestamp = consoleSettings.TimeStampOnNone;
                    typePrefix = string.Empty;
                    break;
            }

            StringBuilder sb = new StringBuilder();
            if (showTimestamp)
            {
                sb.Append(timestamp);
            }
            if (consoleSettings.ShowMessageTypePrefixes && type != LogMessageType.None)
            {
                sb.Append(typePrefix);
            }
            sb.Append(message);

            WriteMessage(sb.ToString(), foreground, background);
        }
        /// <summary>
        /// Writes a message to the console
        /// </summary>
        /// <param name="message">Message to write</param>
        /// <param name="foreground">Foreground color to use</param>
        /// <param name="background">Background color to use</param>
        private void WriteMessage(string message, ConsoleColor foreground, ConsoleColor background)
        {
            ConsoleColor origBackground = Console.BackgroundColor;
            ConsoleColor origForeground = Console.ForegroundColor;

            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.WriteLine(message);

            Console.ForegroundColor = origForeground;
            Console.BackgroundColor = origBackground;
        }
    }
}