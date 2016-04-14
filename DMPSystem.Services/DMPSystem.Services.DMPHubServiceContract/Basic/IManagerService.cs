using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DMPSystem.Core.Common;
using DMPSystem.IModuleServices.DMPHub.Models;

namespace DMPSystem.Services.DMPHubServiceContract.Basic
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IManagerService”。
    [ServiceContract]
    public interface IManagerService
    {
        [OperationContract]
        ServiceResult<Manager> GetManagerById(int id);
    }
}
