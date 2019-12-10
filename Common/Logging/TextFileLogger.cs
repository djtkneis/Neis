using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Neis.Logging
{
    /// <summary>
    /// Logger used for writing messages to a text file
    /// </summary>
    public class TextFileLogger : LoggerBase
    {
        /// <summary>
        /// Constructor for the <see cref="TextFileLogger"/> class
        /// </summary>
        /// <param name="settings">Settings for the logger</param>
        public TextFileLogger(TextFileLoggerSettings settings) :
            base(settings) 
        {
        }

        /// <summary>
        /// Writes a message to the text file
        /// </summary>
        /// <param name="message">Message to write</param>
        /// <param name="type">Type of message to write</param>
        protected override void WriteMessage(string message, LogMessageType type)
        {
            TextFileLoggerSettings textFileSettings = Settings as TextFileLoggerSettings;

            string typePrefix = string.Format("[{0}]: ", type.ToString()[0]);
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt: ");

            bool showTimestamp = false;

            switch (type)
            {
                case LogMessageType.Information:
                    showTimestamp = textFileSettings.TimeStampOnInformation;
                    break;

                case LogMessageType.Warning:
                    showTimestamp = textFileSettings.TimeStampOnWarning;
                    break;

                case LogMessageType.Error:
                    showTimestamp = textFileSettings.TimeStampOnError;
                    break;

                default:
                    showTimestamp = textFileSettings.TimeStampOnNone;
                    typePrefix = string.Empty;
                    break;
            }

            StringBuilder sb = new StringBuilder();
            if (showTimestamp)
            {
                sb.Append(timestamp);
            }
            if (textFileSettings.ShowMessageTypePrefixes && type != LogMessageType.None)
            {
                sb.Append(typePrefix);
            }
            sb.Append(message);

            try
            {
                if (!File.Exists(textFileSettings.FilePath))
                {
                    File.Create(textFileSettings.FilePath).Close();
                }
                File.AppendAllLines(textFileSettings.FilePath, new string[] { sb.ToString() });
            }
            catch
            {

            }
        }
    }
}
