using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMPSystem.Core.Common;
using DMPSystem.Core.System;
using DMPSystem.Core.System.Intercept;
using DMPSystem.DataAccess.DMPConfig;
using DMPSystem.IModuleServices.DMPHub.Models;

namespace DMPSystem.Modules.DMPHub.Repositories
{
    public  class ManagerRepository
    {
        /// <summary>
        /// 通过ID获取管理员信息
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>返回管理员信息</returns>
            [InterceptMethod(CachingMethod.Get, Time = 120)]
        public virtual Manager GetManagerById(int id)
        {
            using (var db = new DMPHubContext())
            {
                var entity = (from q in db.manager.AsNoTracking() where q.UserID == id select q).SingleOrDefault();
                return entity.MapTo<Manager>();
            }
        }

        /// <summary>
        /// 获取所有管理员信息
        /// </summary>
        /// <returns></returns>
        [InterceptMethod(CachingMethod.Get, Time = 120)]
        public virtual List<Manager> GetManager()
        {
            using (var db = new DMPHubContext())
            {
                var entities = (from q in db.manager.AsNoTracking()  select q).ToList();
                return entities.MapToList<Manager>();
            }
        }
    }
}
