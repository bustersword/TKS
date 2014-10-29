using System;
using System.Collections.Generic;

using System.ServiceModel;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Globalization;
using System.IO.IsolatedStorage;

namespace TKS.Common
{
    public static class AppStorage
    {
        #region session操作
        static Dictionary<string, object> Session
        {
            get;
            set;
        }
        public static void ClearSession()
        {
            if (Session != null)
                Session.Clear();
        }

        public static void SetSessionValue(string key, object value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                if (!Session.ContainsKey(key))
                {
                    Session.Add(key, value);
                }
                else
                {
                    Session[key] = value;
                }
            }
        }

        public static object GetSessionValue(string key)
        {
            object result = null;
            try
            {
                if (Session.ContainsKey(key))
                    result = Session[key];

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static bool TryGetSessionValue<T>(string key, out T value)
        {
            bool result = false;
            try
            {
                CultureInfo provider = new CultureInfo("fr-FR");
                if (Session.ContainsKey(key))
                {
                    value = (T)Convert.ChangeType(Session[key], typeof(T), provider);
                    result = true;
                }
                else
                {
                    value = default(T);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        public static T GetSessionValue<T>(string key)
        {
            T result;
            try
            {
                CultureInfo provider = new CultureInfo("fr-FR");
                if (Session.ContainsKey(key))
                {
                    result = (T)Convert.ChangeType(Session[key], typeof(T), provider);
                }
                else
                {
                    result = default(T);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        static AppStorage()
        {
            Session = new Dictionary<string, object>();
            Bindings = new Dictionary<string, Binding>();
            EndPoints = new List<Endpoint>();
        }

        #region 配置WCF管理
        /// <summary>
        /// 服务端地址
        /// </summary>
        public static string WcfConfigAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 绑定配置
        /// </summary>
        public static Dictionary<string, Binding> Bindings
        {
            get;
            set;
        }

        /// <summary>
        /// 终结点配置
        /// </summary>
        public static List<Endpoint> EndPoints
        {
            get;
            set;
        }

        private static string _config;

        /// <summary>
        /// 从服务端获取wcf相关配置，关区代码+区外代码+区内代码 为文件首选名，如不存在，则搜寻关区代码+区外代码，次关区代码
        /// </summary>
        /// <param name="filepath">相对路径</param>
        /// <param name="GQcode">关区代码</param>
        /// <param name="outcompcode">区外代码</param>
        /// <param name="incompcode">区内代码</param>
        /// <param name="call"></param>
        public static void SetConfig(string config)
        {
            _config = config;
            List<XElement> lsBindings = GetNode("system.serviceModel/bindings/basicHttpBinding");

            List<XElement> customBindings = GetNode("system.serviceModel/bindings/customBinding");

            for (int i = 0; i < lsBindings.Count; i++)
            {
                string name = GetAttributeValue(lsBindings[i], "name");
                Binding bind = CreateBasicHttpBindingItem(lsBindings[i]);
                if (!Bindings.ContainsKey(name))
                    Bindings.Add(name, bind);
            }

            for (int c = 0; c < customBindings.Count; c++)
            {
                string name = GetAttributeValue(customBindings[c], "name");
                Binding bind = CreateCustomBindingItem(customBindings[c]);
                if (!Bindings.ContainsKey(name))
                {
                    Bindings.Add(name, bind);
                }
            }

            List<XElement> lsEndpoints = GetNode("system.serviceModel/client");
            for (int j = 0; j < lsEndpoints.Count; j++)
            {
                Endpoint endpoint = CreateEndpoint(lsEndpoints[j]);
                //这里添加有重复的时候，取值时候只取第一个
                EndPoints.Add(endpoint);
            }
        }

        /// <summary>
        /// 获取指定节点
        /// </summary>
        /// <param name="nodeName">节点：node1/node2/node3</param>
        /// <returns></returns>
        public static List<XElement> GetNode(string nodeName)
        {

            byte[] b = UTF8Encoding.UTF8.GetBytes(_config);
            XDocument xml = XDocument.Load(new MemoryStream(b));
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

        /// <summary>
        /// 解析appsettings
        /// </summary>
        /// <param name="path">节点路径</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetAppsettings(string path)
        {
            List<XElement> appsettings = GetNode(path);

            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (XElement item in appsettings)
            {
                string key = GetAttributeValue(item, "key");
                string value = GetAttributeValue(item, "value");
                result.Add(key, value);
            }

            return result;

        }
        /// <summary>
        /// 解析Tabs
        /// </summary>
        /// <returns></returns>
        public static List<TabMenuItem> GetTabMenuItems(string name)
        {
            List<XElement> appsettings = GetNode("Tabs");
            List<XElement> steps = new List<XElement>();

            List<TabMenuItem> result = new List<TabMenuItem>();
            foreach (XElement item in appsettings)
            {
                string _name = GetAttributeValue(item, "name");
                if (name == _name)
                {
                    steps = item.Elements().ToList();
                }
            }

            foreach (var step in steps)
            {
                TabMenuItem temp = new TabMenuItem();
                temp.AssemblyName = GetAttributeValue(step, "assemblyname");
                temp.Description = GetAttributeValue(step, "description");
                temp.Ignore = GetAttributeValue(step, "ignore") == "true" ? true : false;
                temp.Order = int.Parse(GetAttributeValue(step, "order"));
                temp.Type = GetAttributeValue(step, "type");

                result.Add(temp);
            }
            return result;
        }


        /// <summary>
        /// 根据节点属性name的值，在集合中获取节点
        /// </summary>
        /// <param name="listElement"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static XElement GetXElementByName(List<XElement> listElement, string name)
        {
            XElement result = null;
            for (int i = 0; i < listElement.Count; i++)
            {
                XElement element = listElement[i];
                XAttribute atr = element.Attribute(name);
                if (atr == null)
                {
                    throw new Exception("节点中不存在该属性");
                }
                else
                {
                    if (atr.Value.Trim() == name.Trim())
                    {

                        break;
                    }
                }
            }

            return result;
        }

        public static string GetAttributeValue(XElement element, string attributeName)
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
        /// 创建终结点
        /// </summary>
        /// <param name="nodeLayer"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Endpoint CreateEndpoint(string nodeLayer, string name)
        {
            List<XElement> endPoints = GetNode(nodeLayer);
            XElement endPoint = GetXElementByName(endPoints, name);
            Endpoint result = CreateEndpoint(endPoint);

            return result;
        }

        private static Endpoint CreateEndpoint(XElement endPoint)
        {
            string name = GetAttributeValue(endPoint, "name");
            string uri = GetAttributeValue(endPoint, "address");
            string contract = GetAttributeValue(endPoint, "contract");
            string bindingConfiguration = GetAttributeValue(endPoint, "bindingConfiguration");
            EndpointAddress endPointAddress = new EndpointAddress(uri);

            return new Endpoint
            {
                EndPointAddress = endPointAddress,
                BindingConfiguration = bindingConfiguration,
                Contract = contract,
                Name = name
            };
        }

        /// <summary>
        /// 创建绑定
        /// </summary>
        /// <param name="nodeLayer">层级</param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static BasicHttpBinding CreateBasicHttpBinding(string nodeLayer, string name)
        {
            List<XElement> bindings = GetNode(nodeLayer);
            XElement binding = GetXElementByName(bindings, name);
            return CreateBasicHttpBindingItem(binding);
        }

        private static BasicHttpBinding CreateBasicHttpBindingItem(XElement binding)
        {
            BasicHttpBinding bhb = new BasicHttpBinding();

            var atts = binding.Attributes();
            #region 配置绑定
            foreach (var attribute in atts)
            {
                /*
                 * <binding name="WSHttpBinding_ISsoService"
                 * closeTimeout="00:01:00"
                 * openTimeout="00:01:00"
                 * receiveTimeout="00:10:00" 
                 * sendTimeout="00:01:00" 
                 * allowCookies="false"
                 * bypassProxyOnLocal="false"
                 * hostNameComparisonMode="StrongWildcard"
                 * maxBufferPoolSize="524288" 
                 * maxReceivedMessageSize="65536000"
                 * messageEncoding="Text" 
                 * textEncoding="utf-8" 
                 * useDefaultWebProxy="true">
                 * 
                 */
                XName reader = attribute.Name;
                if (reader.LocalName == "name")
                {
                    //base["name"] = reader.Value;
                    bhb.Name = attribute.Value;
                }
                else if (reader.LocalName == "maxBufferSize")
                {
                    int num = int.Parse(attribute.Value);
                    if (num < 1)
                    {
                        //throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("maxBufferSize", System.ServiceModel.SR.GetString("ArgumentOutOfRange", new object[] { 1, 0x7fffffff })));
                        return null;
                    }
                    bhb.MaxBufferSize = num;
                }
                else if (reader.LocalName == "maxReceivedMessageSize")
                {
                    long num2 = long.Parse(attribute.Value);
                    if (num2 < 1L)
                    {
                        return null;
                    }
                    bhb.MaxReceivedMessageSize = num2;
                }
                else if (reader.LocalName == "textEncoding")
                {
                    //base["textEncoding"] = Encoding.GetEncoding(reader.Value);
                    bhb.TextEncoding = Encoding.GetEncoding(attribute.Value);
                }
                else if (reader.LocalName == "closeTimeout")
                {
                    TimeSpan span = TimeSpan.Parse(attribute.Value);
                    if (span < TimeSpan.Zero)
                    {
                        return null;
                    }
                    //base["closeTimeout"] = span;
                    bhb.CloseTimeout = span;
                }
                else if (reader.LocalName == "openTimeout")
                {
                    TimeSpan span2 = TimeSpan.Parse(attribute.Value);
                    if (span2 < TimeSpan.Zero)
                    {
                        return null;
                    }
                    bhb.OpenTimeout = span2;
                }
                else if (reader.LocalName == "receiveTimeout")
                {
                    TimeSpan span3 = TimeSpan.Parse(attribute.Value);
                    if (span3 < TimeSpan.Zero)
                    {
                        return null;
                    }
                    bhb.ReceiveTimeout = span3;
                }
                else if (reader.LocalName == "sendTimeout")
                {
                    TimeSpan span4 = TimeSpan.Parse(attribute.Value);
                    if (span4 < TimeSpan.Zero)
                    {
                        return null;
                    }
                    bhb.SendTimeout = span4;
                }
                else if (reader.LocalName == "enableHttpCookieContainer")
                {
                    bool flag = bool.Parse(attribute.Value);
                    bhb.EnableHttpCookieContainer = flag;
                }

                else if (reader.LocalName == "transferMode")
                {
                    if (attribute.Value.ToLower() == "streamedresponse")
                    {
                        bhb.TransferMode = TransferMode.StreamedResponse;
                    }
                    else
                    {
                        bhb.TransferMode = TransferMode.Buffered;
                    }
                }
            }
            #endregion


            //var readerQuotas = binding.Element("readerQuotas");

            //if (readerQuotas != null)
            //{
            //    var rq = readerQuotas.Attributes();

            //    /* 
            //     * maxDepth="32"
            //     * maxStringContentLength="67108864"  
            //     * maxArrayLength="2147483647" 
            //     * maxBytesPerRead="33554432" 
            //     * maxNameTableCharCount="16384
            //     */
            //    bhb.ReaderQuotas   
            //}

            var security = binding.Element("security");

            if (security != null)
            {
                var mode = security.Attribute("mode");

                bhb.Security.Mode = (BasicHttpSecurityMode)Enum.Parse(typeof(BasicHttpSecurityMode), mode.Value, true);
            }

            return bhb;
        }

        /// <summary>
        /// 创建nettcp 类型的绑定
        /// </summary>
        /// <param name="nodeLayer">层级</param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static CustomBinding CreateCustomBinding(string nodeLayer, string name)
        {
            List<XElement> bindings = GetNode(nodeLayer);
            XElement binding = GetXElementByName(bindings, name);
            return CreateCustomBindingItem(binding);
        }

        private static CustomBinding CreateCustomBindingItem(XElement binding)
        {
            CustomBinding cb = new CustomBinding();
            var atts = binding.Attributes();
            #region 配置绑定
            foreach (var attribute in atts)
            {
                /*
                 *  
                 * 
                 */
                XName reader = attribute.Name;
                if (reader.LocalName == "name")
                {
                    cb.Name = attribute.Value;
                }

                else if (reader.LocalName == "closeTimeout")
                {
                    TimeSpan span = TimeSpan.Parse(attribute.Value);
                    if (span < TimeSpan.Zero)
                    {
                        return null;
                    }
                    //base["closeTimeout"] = span;
                    cb.CloseTimeout = span;
                }
                else if (reader.LocalName == "openTimeout")
                {
                    TimeSpan span2 = TimeSpan.Parse(attribute.Value);
                    if (span2 < TimeSpan.Zero)
                    {
                        return null;
                    }
                    cb.OpenTimeout = span2;
                }
                else if (reader.LocalName == "receiveTimeout")
                {
                    TimeSpan span3 = TimeSpan.Parse(attribute.Value);
                    if (span3 < TimeSpan.Zero)
                    {
                        return null;
                    }
                    cb.ReceiveTimeout = span3;
                }
                else if (reader.LocalName == "sendTimeout")
                {
                    TimeSpan span4 = TimeSpan.Parse(attribute.Value);
                    if (span4 < TimeSpan.Zero)
                    {
                        return null;
                    }
                    cb.SendTimeout = span4;
                }


            }
            #endregion
            BinaryMessageEncodingBindingElement bin = new System.ServiceModel.Channels.BinaryMessageEncodingBindingElement();
            cb.Elements.Add(bin);

            var tcpTransport = binding.Element("tcpTransport");
            if (tcpTransport != null)
            {

                TcpTransportBindingElement tras = new System.ServiceModel.Channels.TcpTransportBindingElement();
                var size = tcpTransport.Attribute("maxReceivedMessageSize");
                tras.MaxReceivedMessageSize = long.Parse(size.Value);
                var buff = tcpTransport.Attribute("maxBufferSize");
                tras.MaxBufferSize = int.Parse(buff.Value);
                cb.Elements.Add(tras);
            }


            return cb;
        }
        #endregion

        #region 独立缓存操作

        /// <summary>
        /// 总空间大小与指定值比较
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static bool IsSpaceEnough(long size)
        {
            size = size * 1024 * 1024;
            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
            Int64 curAvail = store.UsedSize + store.AvailableFreeSpace;
            if (size > curAvail)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// 分配独立缓存空间
        /// </summary>
        /// <param name="size">MB</param>
        /// <returns></returns>
        public static bool ApplyStorageSpace(double size)
        {
            Int64 b = Convert.ToInt64(size * 1024 * 1024);
            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
            Int64 curAvail = store.AvailableFreeSpace;
            bool flag = true;
            if (curAvail < b)
            {
                try
                {
                    flag = store.IncreaseQuotaTo(b);
                }
                catch
                {
                    throw;
                }
            }

            return flag;
        }


        /// <summary>
        /// 从独立缓存中读取程序集流,读取后操作完需关闭流
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="assemblyName">程序集名</param>
        /// <returns></returns>
        public static Stream ReadFromIsolatedStorage(string path, string assemblyName)
        {
            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream assembly = null;
            if (!store.DirectoryExists(path))
            {
                store.CreateDirectory(path);
            }
            assemblyName = path + assemblyName;
            try
            {
                if (store.FileExists(assemblyName))
                {
                    assembly = store.OpenFile(assemblyName, System.IO.FileMode.Open);
                }
                return assembly;
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// 将程序集以二进制方式存入独立缓存
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="assemblyName">程序集名</param>
        /// <param name="stream">要写入的流</param>
        /// <param name="call">执行完流写入方法后执行的方法</param>
        public static void SaveIntoIsolatedStorage(string path, string assemblyName, Stream stream, Action call)
        {

            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream assembly = null;
            assemblyName = path + assemblyName;
            try
            {
                if (!store.DirectoryExists(path))
                {
                    store.CreateDirectory(path);
                }
                stream.Seek(0, SeekOrigin.Begin);

                if (store.FileExists(assemblyName))
                {
                    store.DeleteFile(assemblyName);
                }
                assembly = store.CreateFile(assemblyName);



                byte[] res = StreamToBytes(stream);
                assembly.Write(res, 0, res.Length);
                if (call != null)
                {
                    call();
                }

            }
            catch
            {
                throw;
            }
            finally
            {
                assembly.Close();
            }
        }
        static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary>
        /// 根据指定键从独立缓存中获取值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetAppStettingObject(string name)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(name))
            {
                return IsolatedStorageSettings.ApplicationSettings[name];
            }
            return null;
        }

        /// <summary>
        /// 存储独立缓存键值
        /// </summary>
        public static void SaveAppsetting(string name, object value)
        {
            IsolatedStorageSettings appSetting = IsolatedStorageSettings.ApplicationSettings;
            try
            {
                if (!appSetting.Contains(name))
                {
                    appSetting.Add(name, value);
                }
                else
                {
                    appSetting[name] = value;
                }
                try
                {
                    appSetting.Save();
                }
                catch (IsolatedStorageException ise)
                {

                    throw new Exception("独立存储不够" + ise.Message);

                }
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
