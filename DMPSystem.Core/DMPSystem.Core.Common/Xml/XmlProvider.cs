using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

namespace OMCSystem.Core.Common.Xml
{
    public class XmlProvider
    {
        [Obsolete("性能有问题，弃用，请使用XmlSerializer")]
        public static T XmlMapToModel<T>(string xml, bool isJsonProperty = true) where T : class, new()
        {
            var properties = typeof (T).GetProperties();
            var model = new T();
            Dictionary<string, string> dis;
            if (isJsonProperty)
            {
                dis = (from prop in properties
                       orderby prop.Name
                       where prop.GetCustomAttributes<JsonPropertyAttribute>().Any()
                       select prop).ToDictionary(
                           p => p.GetCustomAttribute<JsonPropertyAttribute>().PropertyName, m => m.Name);
            }
            else
            {
                dis = (from prop in properties
                       orderby prop.Name
                       select prop).ToDictionary(
                           p => p.Name, m => m.Name);

            }

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNode xmlNode = xmlDoc.FirstChild; //获取到根节点<xml>
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                var xe = (XmlElement) xn;
                if (dis.ContainsKey(xn.Name))
                {
                    var popertyName = dis[xn.Name];
                    typeof (T).GetProperty(popertyName).SetValue(model, xe.InnerText);
                }
            }
            return model;
        }
    }
}
