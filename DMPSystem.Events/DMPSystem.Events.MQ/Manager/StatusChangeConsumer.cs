using System;
using DMPSystem.Core.EventBus;
using DMPSystem.Core.EventBus.Publisher;
using DMPSystem.Core.EventBus.Subscription;
using DMPSystem.Core.EventBus.Utilities;
using DMPSystem.Core.System;
using DMPSystem.Events.Models;
using DMPSystem.IModuleServices.DMPHub;

namespace DMPSystem.Events.MQ.Manager
{
    [QueueConsumer("test")]
    public class StatusChangeConsumer : ServiceBase, IConsumer<ManagerEvent>
    {
        public void HandleEvent(ManagerEvent eventMessage)
        {
            var entity = eventMessage;
            if (entity != null)
            {
                try
                {
                    GetService<IManagerService>().Update(new IModuleServices.DMPHub.Models.Manager()
                    {
                        UserID = eventMessage.UserID,
                        CreateTime = eventMessage.CreateTime,
                        Email = eventMessage.Email,
                        Phone = eventMessage.Phone,
                        Sex = eventMessage.Sex,
                        UpdateTime = eventMessage.UpdateTime
                    });
                }
                catch (Exception)
                {

                    
                }
            }
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }

   
}
