using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace DMPSystem.Core.EventBus
{
   public class EventContainer
    {
        public static T GetInstance<T>(string name) where T : class
        {
            var appConfig = AppConfig.DefaultInstance;
            return appConfig.GetContextInstance<T>(name);
        }

        public static T GetInstance<T>() where T : class
        {
            var appConfig = AppConfig.DefaultInstance;
            return appConfig.GetContextInstance<T>();
        }

        public static IEnumerable<object> GetInstances(Type type)
        {
            var appConfig = AppConfig.DefaultInstance;
            return appConfig.GetContextInstances(type);
        }

       public static void RegisterConsumeModule(params Type[] types)
       {
           var appConfig = AppConfig.DefaultInstance;
           types.ForEach(appConfig.RegisterConsumeModule);
       }
    }
}
