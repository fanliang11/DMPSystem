using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.EventBus.Utilities
{
    public sealed class Check
    {
        public static T NotNull<T>(T value, string parameterName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        public static T? NotNull<T>(T? value, string parameterName) where T : struct
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        public static string NotEmpty(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(string.Format(EventResources.ArgumentIsNullOrWhitespace, parameterName));
            }

            return value;
        }

        public static void CheckCondition(Func<bool> condition, string parameterName)
        {
            if (condition.Invoke())
            {
                throw new ArgumentException(string.Format(EventResources.ArgumentIsNullOrWhitespace, parameterName));
            }
        }
    }
}
