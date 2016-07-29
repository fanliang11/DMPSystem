using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCLSystem.Core.Caching;
using DMPSystem.Core.Common;
using DMPSystem.Core.System;
using DMPSystem.Core.System.Intercept;
using DMPSystem.DataAccess.DMPHub;
using Manager = DMPSystem.IModuleServices.DMPHub.Models.Manager;

namespace DMPSystem.Modules.DMPHub.Repositories
{
    public  class ManagerRepository:ServiceBase
    {
        /// <summary>
        /// 通过ID获取管理员信息
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>返回管理员信息</returns>
        //[InterceptMethod(CachingMethod.Get, Time = 120, Mode = CacheTargetType.Redis, CacheSectionType = SectionType.DMPHub)]
        public virtual Manager GetManagerById(int id)
        {
            using (var db = new DMPHubContext())
            {
                var entity = (from q in db.Manager.AsNoTracking() where q.UserID == id select q).SingleOrDefault();
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
                var entities = (from q in db.Manager.AsNoTracking() select q).ToList();
                return entities.MapToList<Manager>();
            }
        }

        public bool Update(Manager manager)
        {
            using (var db = new DMPHubContext())
            {
                var baseRepository = GetService<IRepository<DataAccess.DMPHub.Manager>>();
                return baseRepository.Instance(db).Modify(new DataAccess.DMPHub.Manager()
                {
                    UserID = manager.UserID,
                    UserName = manager.UserName,
                   Email = manager.Email,
                   Phone = manager.Phone,
                    UpdateTime = DateTime.Now
                },  "UpdateTime") > 0;
                 
            }
        }
    }
}
