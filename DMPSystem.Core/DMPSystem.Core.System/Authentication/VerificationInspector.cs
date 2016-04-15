using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using DMPSystem.Core.Common.ServicesException;

namespace DMPSystem.Core.System.Authentication
{
    public class VerificationInspector : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            try
            {
                var attributes =
                    operationContext.Host.Description.ServiceType.GetCustomAttributes(typeof (NoVerificationAttribute),
                        false);
                if (attributes.Length > 0) return true;
                var sign = GetHeaderValue("Sign");
                var serviceCode = GetHeaderValue("Code");
                return true;
            }
            catch (Exception e)
            {
                throw new ServiceException(e.Message);
            }
        }

        private string GetHeaderValue(string name)
        {
            if (WebOperationContext.Current == null) return null;
            var headers = WebOperationContext.Current.IncomingRequest;
            return headers.Headers.GetValues(name).FirstOrDefault();
        }
    }
}