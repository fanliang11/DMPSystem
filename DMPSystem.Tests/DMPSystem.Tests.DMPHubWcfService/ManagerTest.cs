using System;
using DMPSystem.Core.WebModule.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace DMPSystem.Tests.DMPHubWcfService
{
    [TestClass]
    public class ManagerTest
    {
        [TestMethod]
        public void TestGetManageById()
        {
            var t = DateTime.Now;
            for (var i = 0; i < 1000; i++)
            {
                var result = HttpClientProvider.GetResponse("http://localhost:59572/ManagerService/GetManagerById/2");
            }
            var t1 = (DateTime.Now - t).TotalMilliseconds;
        }
    }
}
