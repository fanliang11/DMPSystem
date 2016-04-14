using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DMPSystem.Core.Common;
using DMPSystem.Core.Common.ServicesException;
using DMPSystem.Core.System.Ioc;
using DMPSystem.IModuleServices.DMPHub.Models;
using DMPSystem.Services.DMPHubServiceContract.Basic;


namespace DMPSystem.Services.DMPHubWcfService.Basic
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“ManagerService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 ManagerService.svc 或 ManagerService.svc.cs，然后开始调试。
       [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, UseSynchronizationContext = false,
        ConcurrencyMode = ConcurrencyMode.Multiple)]
    [IocServiceBehavior]
    public class ManagerService : IManagerService
    {
        private readonly IModuleServices.DMPHub.IManagerService _managerService;

        public ManagerService(IModuleServices.DMPHub.IManagerService manager)
        {
            _managerService = manager;
        }

        public ServiceResult<Manager> GetManagerById(int id)
        {
            try
            {
                ServiceResult<Manager> result = null;
                result = ServiceResult<Manager>.Create(true, _managerService.GetManagerById(id));
                return result;
            }
            catch (ServiceException e)
            {
                var result = ServiceResult<Manager>.Create(false, e.Message, null);
                result.ErrorCode = e.Code;
                return result;
            }
        }
    }
}
