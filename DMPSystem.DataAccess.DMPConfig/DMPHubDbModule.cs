using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMPSystem.Core.System;
using DMPSystem.Core.System.Module;
using DMPSystem.Core.System.Module.Attributes;

namespace DMPSystem.DataAccess.DMPConfig
{

    [ModuleDescription("12C16D64-693A-4D3E-93EB-B2E1465C24C8", "分布式管理中心数据模块", "分布式管理中心数据模块")]
    public class DMPHubDbModule : SystemModule
    {

        protected override void RegisterBuilder(ContainerBuilderWrapper builder)
        {
            base.RegisterBuilder(builder);
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>));
        }

        public override void Initialize()
        {
            using (var dbcontext = new DMPHubContext())
            {

                var objectContext = ((IObjectContextAdapter)dbcontext).ObjectContext;
                var mappingCollection = (StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
                mappingCollection.GenerateViews(new List<EdmSchemaError>());
                Database.SetInitializer<DMPHubContext>(null);
            }
        }
    }
}
