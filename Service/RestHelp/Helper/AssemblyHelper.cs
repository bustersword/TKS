using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.IO;
namespace TKS.Service.Helper
{

    public class AssemblyHelper
    {
        Assembly _assembly;
        string _assemblyName;
        public AssemblyHelper(string assemblyName)
        {
            _assembly = Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory +
                "\\bin\\" + assemblyName + ".dll");
            _assemblyName = assemblyName;
        }

        public Assembly Instance
        {
            get
            {
                if (_assembly == null)
                    _assembly = Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory +
                "\\bin\\" + _assemblyName + ".dll");

                return _assembly;


            }
        }
    }

    public static class ConfigHelper
    {

        static XDocument xml;
        static ConfigHelper()
        {
            string filename = AppDomain.CurrentDomain.BaseDirectory + @"\\RouteConfig.xml";
            if (File.Exists(filename))
            {
                if (xml == null)
                    xml = XDocument.Load(filename);
            }
            else
            {
                throw new Exception("路由配置文件不存在");
            }
        }

        /// <summary>
        /// 获取本服务节点的消息路由配置
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, RouteItem> GetRouteItems()
        {
            Dictionary<string, RouteItem> result = new Dictionary<string, RouteItem>();
            List<XElement> res = GetNode("Route");
            foreach (var item in res)
            {
                RouteItem tempData = new RouteItem();
                tempData.Type = GetAttributeValue(item, "Type");
                tempData.RouteAddress = GetAttributeValue(item, "RouteAddress");
                if (!result.Keys.Contains(tempData.Type))
                    result.Add(tempData.Type, tempData);
            }

            return result;

        }
        static string GetAttributeValue(XElement element, string attributeName)
        {
            string result;
            XAttribute atr = element.Attribute(attributeName);

            if (atr != null)
            {
                result = atr.Value;
            }
            else
            {
                throw new Exception("节点中不存在该属性");
            }
            return result;
        }

        /// <summary>
        /// 获取指定节点
        /// </summary>
        /// <param name="nodeName">节点：node1/node2/node3</param>
        /// <returns></returns>
        static List<XElement> GetNode(string nodeName)
        {
            List<XElement> result = xml.Root.Elements().ToList();
            string[] elements = nodeName.Split(new char[] { '/' });

            for (int i = 0; i < elements.Length; i++)
            {

                if (result.Count > 0)
                {
                    if (elements[i].Trim() != "")
                    {
                        result = result.Where((p) =>
                        {
                            if (p.Name.ToString() == elements[i]) return true;
                            else return false;
                        }).ToList();
                        List<XElement> res = new List<XElement>();
                        for (int j = 0; j < result.Count; j++)
                        {
                            res.AddRange(result[j].Elements().ToList());
                        }
                        result.Clear();
                        result = res;
                    }
                }
            }

            return result;
        }
    }
}

