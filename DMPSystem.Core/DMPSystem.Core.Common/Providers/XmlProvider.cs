using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DMPSystem.Core.Common.Providers
{
    public class XmlProvider
    {
        private XmlDocument _XmlDocument;
        private XmlElement _XmlElement;
        private string _XmlString = string.Empty;

        public XmlProvider(string xmlString)
        {
            if (string.IsNullOrEmpty(xmlString))
            {
                throw new ArgumentNullException("xmlString", "input no xml string");
            }
            this._XmlString = xmlString;
            this.CreateXMLElement();
        }

        public void AppendNode(System.Xml.XmlNode xmlNode)
        {
            System.Xml.XmlNode newChild = this._XmlDocument.ImportNode(xmlNode, true);
            this._XmlElement.AppendChild(newChild);
        }

        private static XmlElement CreateRootElement(string xmlString)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlString);
            return document.DocumentElement;
        }

        private void CreateXMLElement()
        {
            this._XmlDocument = new XmlDocument();
            this._XmlDocument.LoadXml(this._XmlString);
            this._XmlElement = this._XmlDocument.DocumentElement;
        }

        public string GetAttributeValue(string xPath, string attributeName)
        {
            return this._XmlElement.SelectSingleNode(xPath).Attributes[attributeName].Value;
        }

        public static string GetAttributeValue(string xmlFilePath, string xPath, string attributeName)
        {
            return CreateRootElement(xmlFilePath).SelectSingleNode(xPath).Attributes[attributeName].Value;
        }

        public System.Xml.XmlNode GetNode(string xPath)
        {
            return this._XmlElement.SelectSingleNode(xPath);
        }

        public string GetValue(string xPath)
        {
            return this._XmlElement.SelectSingleNode(xPath).InnerText;
        }

        public static string GetValue(string xmlFilePath, string xPath)
        {
            return CreateRootElement(xmlFilePath).SelectSingleNode(xPath).InnerText;
        }

        public string GetValue(System.Xml.XmlNode node, string path)
        {
            return node.SelectSingleNode(path).InnerText;
        }

        public void RemoveNode(string xPath)
        {
            System.Xml.XmlNode oldChild = this._XmlDocument.SelectSingleNode(xPath);
            this._XmlElement.RemoveChild(oldChild);
        }

        public XmlNodeList ChildNodes
        {
            get
            {
                return this._XmlElement.ChildNodes;
            }
        }

        public XmlElement RootNode
        {
            get
            {
                return this._XmlElement;
            }
        }
    }
}

