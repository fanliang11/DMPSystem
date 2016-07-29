using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMPSystem.Core.EventBus;
using DMPSystem.Core.EventBus.EventModel;
using DMPSystem.Core.EventBus.Publisher;
using DMPSystem.Core.EventBus.Subscription;
using DMPSystem.Core.System;
using DMPSystem.Events.Models;
using DMPSystem.IModuleServices.DMPHub;

namespace DMPSystem.Events.PushEvent.DMPHub.Manager
{
    public class StatusChangeConsumer : ServiceBase, IConsumer<EntityUpdated<ChanageStateEvent>>
    {
        public void Dispose()
        { 
        }

        public void HandleEvent(EntityUpdated<ChanageStateEvent> eventMessage)
        {
            var entity = eventMessage.Entity;
            if (entity != null)
            {
                try
                {
                    GetService<IManagerService>().Update(new IModuleServices.DMPHub.Models.Manager()
                    {
                        UserID = entity.UserID,
                        UserName = entity.UserName??"",
                        CreateTime = entity.CreateTime??"",
                        Email = entity.Email ?? "",
                        Phone = entity.Phone ?? "",
                        Sex = entity.Sex ?? "",
                        UpdateTime = entity.UpdateTime
                    });
                }
                catch (Exception)
                {
                    EventContainer.GetInstances<IEventPublisher>("PushEvent").Publish(eventMessage);
                }
            }
        }
    }
}
