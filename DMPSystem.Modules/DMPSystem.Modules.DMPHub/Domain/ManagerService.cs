using DMPSystem.Core.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMPSystem.IModuleServices.DMPHub;
using DMPSystem.IModuleServices.DMPHub.Models;
using DMPSystem.Modules.DMPHub.Repositories;

namespace DMPSystem.Modules.DMPHub.Domain
{
    public class ManagerService : ServiceBase, IManagerService
    {
        private readonly ManagerRepository _repository;

        public ManagerService(ManagerRepository repository)
        {
            _repository = repository;
        }

        public Manager GetManagerById(int id)
        {
            return _repository.GetManagerById(id);
        }

        public List<Manager> GetManager()
        {
            return _repository.GetManager();
        }
    }
}
