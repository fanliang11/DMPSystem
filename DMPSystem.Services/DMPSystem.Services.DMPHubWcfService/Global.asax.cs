using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Activation;
using System.Threading.Tasks;
using System.Web.Routing;
using DMPSystem.Core.System.Authentication;
using DMPSystem.Core.System.Ioc;
using Autofac;
using DMPSystem.Core.System.Module;
using DMPSystem.Core.System.Module.Attributes;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace DMPSystem.Services.DMPHubWcfService
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            var builder = new ContainerBuilder();
            builder.Initialize();
            builder.RegisterServices();
            builder.RegisterRepositories();
            builder.RegisterWcfServices();
            builder.RegisterModules();

            var container = builder.Build();
            EnterpriseLibraryContainer.Current = new AutofacServiceLocator(container);
            builder.InitializeModule();
           
                var first = (from p in builder.GetReferenceAssembly()
                    where p.GetCustomAttribute<AssemblyModuleTypeAttribute>().Type == ModuleType.WcfService
                    select p.GetTypes().Where(t => !t.IsInterface && t.Name.EndsWith("Service"))).FirstOrDefault(
                        p => p.Any());
                if (first == null) return;
                var services = first.ToList();

                foreach (var service in services)
                {

                    RouteTable.Routes.Add(new ServiceRoute(service.Name,
                        new WebServiceHostFactory(), service));
                }
        
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}