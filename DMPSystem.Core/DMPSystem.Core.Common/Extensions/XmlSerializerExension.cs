using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DMPSystem.Core.Common.Extensions
{
    public static class XmlSerializerExension
    {
        public static string SerializeToXml<T>(this XmlSerializer serializer, T o, string modelName = null)
        {
            var result = new StringBuilder();
            var settings = new XmlWriterSettings {OmitXmlDeclaration = true, Encoding = Encoding.UTF8};
            using (var stream = new MemoryStream())
            {
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    var ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    if (serializer != null) serializer.Serialize(writer, o, ns);
                }
                result.Append(Encoding.UTF8.GetString(stream.ToArray()));
            }
            if (!string.IsNullOrEmpty(modelName))
                result.Replace(typeof (T).Name, modelName);
            return result.ToString();
            //return Encoding.GetEncoding("ISO8859-1").GetString(Encoding.UTF8.GetBytes( result.ToString()));
           
          
        }

        public static T Deserialize<T>(this XmlSerializer serializer, string o) where T : class
        {
            T result;
            var modelName = typeof (T).Name;
            var reg = new Regex("xml+?>");
            if (reg.IsMatch(o))
            {
                o = reg.Replace(o, string.Format("{0}>", modelName));
            }
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(o)))
            {
                result = serializer.Deserialize(stream) as T;
            }
            return result;
        }
    }
}
