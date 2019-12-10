using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neis.Logging
{
    /// <summary>
    /// Base for all loggers
    /// </summary>
    public abstract class LoggerBase
    {
        /// <summary>
        /// Gets the settings for this logger
        /// </summary>
        public LoggerSettings Settings { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settings">Settings for the logger</param>
        public LoggerBase(LoggerSettings settings)
        {
            Settings = settings;
        }

        /// <summary>
        /// Logs a message
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="type">Type of message being logged</param>
        public void LogMessage(string message, LogMessageType type)
        {
            int logLevel = (int)Settings.LogLevel;
            int thisLevel = (int)type;

            if (thisLevel >= logLevel)
            {
                WriteMessage(message, type);
            }
        }

        /// <summary>
        /// Writes a message
        /// </summary>
        /// <param name="message">Message to write</param>
        protected virtual void WriteMessage(string message)
        {
            WriteMessage(message, LogMessageType.Verbose);
        }
        /// <summary>
        /// Writes a message
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="type">Type of message being logged</param>
        protected abstract void WriteMessage(string message, LogMessageType type);
    }
}