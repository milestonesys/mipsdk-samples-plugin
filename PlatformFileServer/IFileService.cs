using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PlatformFileServer
{
	[ServiceContract]
    public interface IFileService 
    {
        [OperationContract]
        string[] GetFolders(String path);

        [OperationContract]
        string[] GetFiles(String path);
    }
}
