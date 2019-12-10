using Neis.FileCleanup.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Neis.FileCleanup.Service
{
    /// <summary>
    /// Interface used for configuring the File Cleanup Service
    /// </summary>
    [ServiceContract]
    [ServiceKnownType(typeof(FileCleanupConfiguration))]
    [ServiceKnownType(typeof(DirectoryConfiguration))]
    public interface IFileCleanupConfiguration
    {
        /// <summary>
        /// Get the current status of the service
        /// </summary>
        /// <returns>Current status</returns>
        [WebGet(ResponseFormat = WebMessageFormat.Xml,
                BodyStyle = WebMessageBodyStyle.Bare,
                RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        [Description("Gets the current status of the File Cleanup Service")]
        string GetStatus();

        /// <summary>
        /// Get the current configuration
        /// </summary>
        /// <returns>Current configuration</returns>
        [WebGet(ResponseFormat = WebMessageFormat.Xml,
                BodyStyle = WebMessageBodyStyle.Bare,
                RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        [Description("Gets the current configuration for the File Cleanup Service")]

        FileCleanupConfiguration GetConfiguration();

        /// <summary>
        /// Sets the configuration
        /// </summary>
        /// <param name="config">Configuration to set</param>
        [WebInvoke(ResponseFormat = WebMessageFormat.Xml,
                   Method = "POST",
                   BodyStyle = WebMessageBodyStyle.Bare,
                   RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        [Description("Sets the configuration for the File Cleanup Service")]
        void SetConfiguration(FileCleanupConfiguration config);

        /// <summary>
        /// Adds a directory to the cleanup congfiguration
        /// </summary>
        /// <param name="path">File path for the directory</param>
        /// <param name="isRecursive">True for recursive cleanup.  False for top level only</param>
        /// <param name="action"><see cref="CleanupAction"/> type</param>
        [WebGet(ResponseFormat = WebMessageFormat.Xml,
                BodyStyle = WebMessageBodyStyle.Bare,
                RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        [Description("Adds a directory to the list of directories to clean")]
        void AddDirectory(string path, bool isRecursive, CleanupAction action);

        /// <summary>
        /// Removes  a directory from the cleanup configuration
        /// </summary>
        /// <param name="path">File path of the directory to remove</param>
        [WebGet(ResponseFormat = WebMessageFormat.Xml,
                BodyStyle = WebMessageBodyStyle.Bare,
                RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        [Description("Removes a directory from the list of directories to clean")]
        void RemoveDirectory(string path);

        /// <summary>
        /// Sets the age of the archive allowed
        /// </summary>
        /// <param name="age">Number of days</param>
        [WebGet(ResponseFormat = WebMessageFormat.Xml,
                BodyStyle = WebMessageBodyStyle.Bare,
                RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        [Description("Sets the number of days old a file can be before it will be cleaned")]
        void SetArchiveDays(int age);

        /// <summary>
        /// Sets the location for archived items to be placed
        /// </summary>
        /// <param name="path">Path for the archive</param>
        [WebGet(ResponseFormat = WebMessageFormat.Xml,
                BodyStyle = WebMessageBodyStyle.Bare,
                RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        [Description("Sets the location for all archives to be placed")]
        void SetArchiveLocation(string path);

        /// <summary>
        /// Sets the time of day to execute the cleanup
        /// </summary>
        /// <param name="timeString">Time of day</param>
        [WebGet(ResponseFormat = WebMessageFormat.Xml,
                BodyStyle = WebMessageBodyStyle.Bare,
                RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        [Description("Sets the time of day for the cleanup process to begin. (format is HHMM where HH is the two digit 24-hour and MM is the two digit minute)")]
        void SetCleanupTime(string timeString);
        
        /// <summary>
        /// Sets whether or not to remove empty folders during cleanup
        /// </summary>
        /// <param name="remove">True to remove.  False to leave.</param>
        [WebGet(ResponseFormat = WebMessageFormat.Xml,
                BodyStyle = WebMessageBodyStyle.Bare,
                RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        [Description("Sets whether or not to remove empty folders after files are cleaned.  (True to remove.  False to leave.)")]
        void SetRemoveEmptyFolders(bool remove);
    }
}