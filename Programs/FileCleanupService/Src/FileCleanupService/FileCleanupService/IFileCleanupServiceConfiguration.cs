using Neis.FileCleanup.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Neis.FileCleanup.Service
{
    /// <summary>
    /// Interface for the File Cleanup Service configuration methods
    /// </summary>
    [ServiceContract]
    [ServiceKnownType(typeof(FileCleanupConfiguration))]
    [ServiceKnownType(typeof(DirectoryConfiguration))]
    [ServiceKnownType(typeof(CleanupAction))]
    public interface IFileCleanupServiceConfiguration
    {
        [WebGet(ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        FileCleanupConfiguration GetConfiguration();
    }
}