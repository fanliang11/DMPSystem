using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.EventBus
{
   public class EventContainer
    {
        public static T GetInstances<T>(string name) where T : class
        {
            var appConfig = AppConfig.DefaultInstance;
            return appConfig.GetContextInstance<T>(name);
        }

        public static T GetInstances<T>() where T : class
        {
            var appConfig = AppConfig.DefaultInstance;
            return appConfig.GetContextInstance<T>();
        }
    }
}
