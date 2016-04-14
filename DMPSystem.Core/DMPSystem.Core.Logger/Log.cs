using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace DMPSystem.Core.Logger
{
   public class Log
    {
        private static readonly bool IsThrowException = (!string.IsNullOrEmpty(ConfigurationSettings.AppSettings["IsThrowException"]) ? Convert.ToBoolean(ConfigurationSettings.AppSettings["IsThrowException"]) : false);

        public static void Assert(bool condition, string message)
        {
            Assert(condition, message, Type.GetType("System.Object"));
        }

        public static void Assert(bool condition, string message, Type type)
        {
            if (!condition)
            {
                Write(message, MessageType.Info, type, null);
            }
        }

        public static void Write(string message, MessageType messageType)
        {
            Write(message, messageType, Type.GetType("System.Object"), null);
        }

        public static void Write(string message, MessageType messageType, Type type)
        {
            Write(message, messageType, type, null);
        }

        public static void Write(string message, MessageType messageType, Type type, Exception ex)
        {
            ILog logger = LogManager.GetLogger(type);
            switch (messageType)
            {
                case MessageType.Debug:
                    if (!logger.IsDebugEnabled)
                    {
                        break;
                    }
                    logger.Debug(message, ex);
                    return;

                case MessageType.Info:
                    if (!logger.IsInfoEnabled)
                    {
                        break;
                    }
                    logger.Info(message, ex);
                    return;

                case MessageType.Warn:
                    if (!logger.IsWarnEnabled)
                    {
                        break;
                    }
                    logger.Warn(message, ex);
                    return;

                case MessageType.Error:
                    if (logger.IsErrorEnabled)
                    {
                        logger.Error(message, ex);
                        if (IsThrowException)
                        {
                            throw ex;
                        }
                    }
                    break;

                case MessageType.Fatal:
                    if (logger.IsFatalEnabled)
                    {
                        logger.Fatal(message, ex);
                        if (IsThrowException)
                        {
                            throw ex;
                        }
                    }
                    break;

                default:
                    return;
            }
        }
    }
}
