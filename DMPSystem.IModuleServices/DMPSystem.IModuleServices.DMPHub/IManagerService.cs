using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMPSystem.IModuleServices.DMPHub.Models;

namespace DMPSystem.IModuleServices.DMPHub
{
  public  interface IManagerService
  {
      Manager GetManagerById(int id);

      List<Manager> GetManager();
  }
}
