using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace OMCSystem.Core.Common.Xml
{  /// <summary>
    /// 扩展序列化![CDATA[]]
    /// </summary>
    public class XmlCData : IXmlSerializable
    {
        /// <summary>
        /// 需要转化的值
        /// </summary>
        private string _mValue;

        /// <summary>
        /// 默认实例化构造函数
        /// </summary>
        public XmlCData()
        {
        }

        /// <summary>
        /// 实例化XmlCData
        /// </summary>
        /// <param name="pValue">需要实例化的值</param>
        public XmlCData(string pValue)
        {
            _mValue = pValue;
        }

        /// <summary>
        /// 需要转化的值
        /// </summary>
        public string Value
        {
            get
            {
                return _mValue;
            }
            set { _mValue = value; }
        }

        /// <summary>
        /// 读取XML数据流
        /// </summary>
        /// <param name="reader"> 要用作基础读取器的 System.Xml.XmlReader 对象。</param>
        public void ReadXml(XmlReader reader)
        {
            _mValue = reader.ReadElementContentAsString();
        }

        /// <summary>
        /// 写入生成XML数据流
        /// </summary>
        /// <param name="writer">要用作基础编写器的 System.Xml.XmlWriter 对象。</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteCData(_mValue);
        }

        /// <summary>
        /// 获取 WWW 联合会 (W3C) XML 架构
        /// </summary>
        /// <returns>返回WWW 联合会 (W3C) XML 架构</returns>
        public XmlSchema GetSchema()
        {
            return (null);
        }

        /// <summary>
        /// 转成字符串
        /// </summary>
        /// <returns>返回字符串</returns>
        public override string ToString()
        {
            return _mValue;
        }

        /// <summary>
        /// XmlCData类转成string类型
        /// </summary>
        /// <param name="element">XmlCData实例</param>
        /// <returns>返回string</returns>
        public static implicit operator string(XmlCData element)
        {
            return (element == null) ? null : element._mValue;
        }

        /// <summary>
        /// string类型转成XmlCData类型
        /// </summary>
        /// <param name="text">需要转化的string</param>
        /// <returns>返回XmlCData实例</returns>
        public static implicit operator XmlCData(string text)
        {
            return new XmlCData(text);
        }

    }
}