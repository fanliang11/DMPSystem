/*----------------------------------------------------
 * 作者:范  亮
 * 创建时间：2015-11-28
 * ------------------修改记录-------------------
 * 修改人      修改日期        修改目的
 * 范  亮      2015-11-28      创建
 ----------------------------------------------------*/

using DMPSystem.Core.Common.ServicesException;
using DMPSystem.Core.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Xml;

namespace DMPSystem.Core.System.Ioc
{
   /// <summary>
   /// 捕捉拦截WCF错误信息
   /// </summary>
   public class WcfErrorHandler : IErrorHandler
    {
       /// <summary>
        ///  启用错误相关处理并返回一个值，该值指示调度程序在某些情况下是否中止会话和实例上下文。
       /// </summary>
        /// <param name="error">服务操作过程中引发的 System.Exception 对象。</param>
        /// <returns>如果 Windows Communication Foundation (WCF) 不应中止会话（如果有一个）和实例上下文（如果实例上下文不是 System.ServiceModel.InstanceContextMode.Single），则为
        ///     true；否则为 false。 默认值为 false。
        ///     </returns>
        public bool HandleError(Exception error)
        {
            return true;
        }

       /// <summary>
        /// 启用创建从服务方法过程中的异常返回的自定义 System.ServiceModel.FaultException&lt;TDetail&gt;。
       /// </summary>
        /// <param name="error"> 服务操作过程中引发的 System.Exception 对象。</param>
        /// <param name="version">消息的 SOAP 版本。</param>
        /// <param name="fault">双工情况下，返回到客户端或服务的 System.ServiceModel.Channels.Message 对象。</param>
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
       {


           var context = OperationContext.Current;
           if (context == null)
           {
               return;
           }
           ////获取传进的消息属性

           MessageProperties properties = context.IncomingMessageProperties;
           var endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
           var message = context.RequestContext.RequestMessage;
           var action = message.Headers.Action;
           var document = new XmlDocument();
           document.LoadXml(message.ToString());
           var nodes = document.GetElementsByTagName(action.Substring(action.LastIndexOf('/') + 1));
           var dic = new Dictionary<string, string>();
           var dis = IsLogDisable(context, action);
           dic.Add("url", context.Host.BaseAddresses[0].AbsolutePath + action.Substring(action.LastIndexOf('/')));
           dic.Add("host", context.Host.BaseAddresses[0].AbsoluteUri);
           var xmlNode = nodes.Item(0);
           if (xmlNode != null) dic.Add("parameter", xmlNode.InnerXml);
           dic.Add("message", error.Message);
           //int errorCode = error is ServiceException ? ((ServiceException)error).Code : 0;
           //var faultException = error is FaultException ?
           //   (FaultException)error : new FaultException<ErrorInfo>(new ErrorInfo
           //   {
           //       Message = error.Message,
           //       ErrorCode = errorCode
           //   }, error.Message);
           var faultException =
               new FaultException(error.Message);

           MessageFault messageFault = faultException.CreateMessageFault();
           fault = Message.CreateMessage(version, messageFault, action);
           Log.Write(
               string.Format("\r\n请求： {0} + ！\r\n调用信息：{1}\r\n参数：{2}\r\n返回信息：{3}", dic["host"], dic["url"],
                             dic["parameter"], dic["message"]), MessageType.Error, this.GetType(), error);

           //ExceptionHelper.LogException(error, null, dic, null);
       }

       /// <summary>
       /// 判断日志是否禁用
       /// </summary>
       /// <param name="context">提供对服务方法的执行的上下文的访问权限</param>
        /// <param name="action">应如何处理消息的说明。</param>
       /// <returns>返回true或false(true:禁用 false:启用)</returns>
        private bool IsLogDisable(OperationContext context, string action)
        {
            bool isSuccess = false;
            DispatchOperation operation =
                context.EndpointDispatcher.DispatchRuntime.Operations.FirstOrDefault(o => o.Action == action);
            Type hostType = context.Host.Description.ServiceType;
           if (operation != null)
           {
               MethodInfo method = hostType.GetMethod(operation.Name);
               var attribute = method.GetCustomAttributes<NoLoggerAttribute>().FirstOrDefault(p => p.GetType() == typeof(NoLoggerAttribute));
               if (attribute != null)
               {
                   isSuccess = attribute.Disable;
               }
           }
           return isSuccess;
        }
    }
}
