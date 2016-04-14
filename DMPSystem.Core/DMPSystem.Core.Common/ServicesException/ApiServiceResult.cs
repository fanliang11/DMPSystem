using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.Common.ServicesException
{
   public interface IApiServiceResult
    {
        string Create(Enum result,string message);
    }
}