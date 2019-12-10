using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Neis.FileCleanupTool.ValueConverters
{
    /// <summary>
    /// <see cref="IValueConverter"/> for converting time strings
    /// </summary>
    public class TimeStringValueConverter : IValueConverter
    {
        /// <summary>
        /// Convert a value into a time string
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="targetType">Target type to convert to</param>
        /// <param name="parameter">Parameter to use while converting</param>
        /// <param name="culture">Culture to use while converting</param>
        /// <returns>Converted value</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime retVal = new DateTime();
            string sVal = value as string;
            int iVal = -1;
            if (!string.IsNullOrWhiteSpace(sVal) && int.TryParse(sVal, out iVal) && iVal > -1)
            {
                int hour = iVal / 100;
                int minute = iVal - (hour * 100);

                if (hour > -1 && hour < 24 && minute > -1 && minute < 60)
                {
                    retVal = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, hour, minute, 0);
                }
            }

            return retVal;
        }

        /// <summary>
        /// Convert a value from a time string
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="targetType">Target type to convert to</param>
        /// <param name="parameter">Parameter to use while converting</param>
        /// <param name="culture">Culture to use while converting</param>
        /// <returns>Converted value</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime dateVal = new DateTime();
            string sVal = value != null ? value.ToString() : string.Empty;
            if (string.IsNullOrWhiteSpace(sVal) || !DateTime.TryParse(sVal, out dateVal))
            {
                dateVal = DateTime.Today;
            }
            string retVal = dateVal.ToString("HHmm");
            return retVal;
        }
    }
}
