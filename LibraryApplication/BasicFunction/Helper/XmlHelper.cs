
using BasicFunction.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace BasicFunction.Helper
{
    public class XmlHelper
    {
        private static XmlHelper _xmlHelper;
        public static XmlHelper Instance => _xmlHelper ?? (_xmlHelper = new XmlHelper());
        private string _basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs\\");

        /// <summary>
        /// 读取xml文件 并转换成类
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public T DeserializeFromXml<T>(string xmlFilePath)
        {
            object result = null;
            if (!xmlFilePath.Contains(_basePath))
            {
                xmlFilePath = Path.Combine(_basePath, xmlFilePath);
            }
            if (File.Exists(xmlFilePath))
            {
                using (var reader = new StreamReader(xmlFilePath))
                {
                    var xs = new XmlSerializer(typeof(T));
                    result = xs.Deserialize(reader);
                }
            }
            else
            {
                Logger.Info($"Can not find {xmlFilePath} 文件");
            }
            return (T)result;
        }

        /// <summary>
        /// 将类序列化为Xml文件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="xmlFilePath"></param>
        /// <param name="xmlRootName"></param>
        public void SerializeToXml<T>(T model, string xmlFilePath, string xmlRootName = "config") where T : class
        {
            if (!xmlFilePath.Contains(_basePath))
            {
                xmlFilePath = Path.Combine(_basePath, xmlFilePath);
            }
            if (!File.Exists(xmlFilePath))
            {
                using (File.Create(xmlFilePath))
                {

                }
            }
            if (model != null)
            {
                var type = model.GetType();
                using (StreamWriter sw = new StreamWriter(xmlFilePath))
                {
                    XmlSerializer xs = string.IsNullOrEmpty(xmlRootName) ?
                        new XmlSerializer(type) :
                        new XmlSerializer(type, new XmlRootAttribute(xmlRootName));
                    xs.Serialize(sw, model);
                }
            }
        }

        /// <summary>
        /// 修改xml一系列的节点的值
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <param name="nodeList"></param>
        public void SetNodeValueList(string xmlFilePath, Dictionary<string, string> nodeList)
        {
            if (File.Exists(xmlFilePath))
            {
                //Logger.Log("无法找到xml文件路径" + xmlFilePath);
                return;
            }
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);
            foreach (var item in nodeList)
            {
                var element = (XmlElement)xmlDoc.SelectSingleNode(item.Key);//节点的name
                if (element != null)
                {
                    element.InnerText = item.Value;//节点的值
                }
                else
                {
                    //Logger.Log("无法找到节点" + item.Key);
                }
            }
            xmlDoc.Save(xmlFilePath);
        }

        /// <summary>
        /// 对比两个实体类属性值的差异 
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="oldMode">原实体类</param>
        /// <param name="newMode">新实体类</param>
        /// <returns>差异记录</returns>
        public bool IsModelChange<T>(T oldMode, T newMode) where T : class
        {
            var typeDescription = typeof(DescriptionAttribute);
            if (oldMode == null && newMode == null) { return true; }
            if (oldMode != null && newMode == null) { return false; }
            if (oldMode == null) { return false; }
            var mPi = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);//反射获取类 所有的public修饰的信息
            return !(from pi in mPi let oldObj = pi.GetValue(oldMode, null) let newObj = pi.GetValue(newMode, null) let oldValue = oldObj == null ? "" : oldObj.ToString() let newValue = newObj == null ? "" : newObj.ToString() where oldValue != newValue select oldValue).Any();
        }
    }
}
