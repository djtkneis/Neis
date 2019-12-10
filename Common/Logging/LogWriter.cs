using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Neis.Logging
{
    /// <summary>
    /// Class used for writing logs.  Can be used to write more than one.
    /// </summary>
    public class LogWriter
    {
        private List<LoggerBase> _loggers;
        private string _eventLogSource;
        private string _eventLogName;

        /// <summary>
        /// Constructor for the <see cref="LogWriter"/> class
        /// </summary>
        public LogWriter()
            : this(System.Reflection.Assembly.GetCallingAssembly().GetName().Name)
        {
            
        }
        /// <summary>
        /// Constructor for the <see cref="LogWriter"/> class
        /// </summary>
        /// <param name="eventLogSource">Windows event log source name</param>
        public LogWriter(string eventLogSource)
            : this(eventLogSource, "Application")
        {
        }
        /// <summary>
        /// Constructor for the <see cref="LogWriter"/> class
        /// </summary>
        /// <param name="eventLogSource">Windows event log source name</param>
        /// <param name="eventLogName">Name of the event log that the sources log entries are written to. (Application, System, or a custom name)</param>
        public LogWriter(string eventLogSource, string eventLogName)
        {
            _loggers = new List<LoggerBase>();
            _eventLogSource = eventLogSource;
            _eventLogName = eventLogName;
            if (!EventLog.SourceExists(_eventLogSource))
            {
                EventLog.CreateEventSource(_eventLogSource, _eventLogName);
            }
        }

        /// <summary>
        /// Displays an error message to the console
        /// </summary>
        /// <param name="message">Message to display</param>
        public void ShowError(string message)
        {
            WriteMessage(message, LogMessageType.Error);
        }
        /// <summary>
        /// Displays an information message to the console
        /// </summary>
        /// <param name="message">Message to display</param>
        public void ShowInformation(string message)
        {
            WriteMessage(message, LogMessageType.Information);
        }
        /// <summary>
        /// Displays a warning message to the console
        /// </summary>
        /// <param name="message">Message to display</param>
        public void ShowWarning(string message)
        {
            WriteMessage(message, LogMessageType.Warning);
        }
        /// <summary>
        /// Displays raw text
        /// </summary>
        /// <param name="message">Message to display</param>
        public void ShowRawText(string message)
        {
            WriteMessage(message, LogMessageType.None);
        }

        /// <summary>
        /// Adds a logger to the list
        /// </summary>
        /// <param name="logger">Logger to add</param>
        public void AddLogger(LoggerBase logger)
        {
            lock (_loggers)
            {
                _loggers.Add(logger);
            }
        }
        /// <summary>
        /// Removes a logger from the list
        /// </summary>
        /// <param name="logger">Logger to remove</param>
        public void RemoveLogger(LoggerBase logger)
        {
            lock (_loggers)
            {
                _loggers.Remove(logger);
            }
        }
        /// <summary>
        /// Removes a logger at a given index
        /// </summary>
        /// <param name="index">Index of logger to remove</param>
        public void RemoveLogger(int index)
        {
            lock (_loggers)
            {
                if (index > 0 && index < _loggers.Count)
                {
                    _loggers.RemoveAt(index);
                }
            }
        }
        /// <summary>
        /// Clears all loggers
        /// </summary>
        public void ClearLoggers()
        {
            lock (_loggers)
            {
                _loggers.Clear();
            }
        }

        /// <summary>
        /// Writes a message using all loggers
        /// </summary>
        /// <param name="message">Message to write</param>
        /// <param name="type">Type of message</param>
        private void WriteMessage(string message, LogMessageType type)
        {
            EventLogEntryType eventLogType = EventLogEntryType.Information;

            switch (type)
            {
                case LogMessageType.Warning:
                    eventLogType = EventLogEntryType.Warning;
                    break;

                case LogMessageType.Error:
                    eventLogType = EventLogEntryType.Error;
                    break;

                default:
                    eventLogType = EventLogEntryType.Information;
                    break;
            }

            EventLog.WriteEntry(_eventLogSource, message, eventLogType);

            lock (_loggers)
            {
                _loggers.ForEach(l =>
                    {
                        try
                        {
                            l.LogMessage(message, type);
                        }
                        catch (Exception ex)
                        {
                            EventLog.WriteEntry(_eventLogSource, "Exception while logging a message:" + ex.ToString(), EventLogEntryType.Error);
                        }
                    });
            }
        }
    }
}