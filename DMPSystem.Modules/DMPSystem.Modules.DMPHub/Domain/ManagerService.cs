using DMPSystem.Core.EventBus;
using DMPSystem.Core.EventBus.EventModel;
using DMPSystem.Core.EventBus.Publisher;
using DMPSystem.Core.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMPSystem.Events.Models;
using DMPSystem.IModuleServices.DMPHub;
using DMPSystem.IModuleServices.DMPHub.Models;
using DMPSystem.Modules.DMPHub.Repositories;

namespace DMPSystem.Modules.DMPHub.Domain
{
    public class ManagerService : ServiceBase, IManagerService
    {
        private readonly ManagerRepository _repository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventPublisher _pushEventPublisher;

        public ManagerService(ManagerRepository repository)
        {
            _eventPublisher = EventContainer.GetInstance<IEventPublisher>("DMPHubEvent.RabbitMq");
            _pushEventPublisher = EventContainer.GetInstance<IEventPublisher>("PushEvent");
            _repository = repository;

        }

        public Manager GetManagerById(int id)
        {
            var manager = _repository.GetManagerById(id);

            if (manager != null)
            {
                _eventPublisher.Publish(new ManagerEvent() {UserID = manager.UserID});
                _pushEventPublisher.Publish(new EntityUpdated<ChanageStateEvent>(new ChanageStateEvent() { UserID = manager.UserID }));
            }

            return manager;
        }

        public List<Manager> GetManager()
        {
            return _repository.GetManager();
        }

        public bool Update(Manager manager)
        {
            return _repository.Update(manager);
        }
    }
}
