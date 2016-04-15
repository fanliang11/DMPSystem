using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.System.Authentication
{
    /// <summary>
    /// NoVerificationAttribute自定义特性,不需要授权验证
    /// </summary>
    public class NoVerificationAttribute : Attribute
    {
    }
}