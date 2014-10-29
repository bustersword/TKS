using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace TKS.Common
{
    public static  class ClientService<T>
    {
        /// <summary>
        /// 获取wcf服务
        /// </summary>
        /// <returns></returns>
        public static  T GetService()
        {
            string name=typeof(T).FullName ;
            List<Endpoint> points=AppStorage.EndPoints.Where((p)=>{return p.Contract ==name ;}).ToList();
            Binding bind=null ;
            Endpoint point=null ;
            if(points.Count >0)
            {
                point =points[0];
                bind =AppStorage.Bindings[point.BindingConfiguration];
            }
            else 
            {
                throw new Exception ("endpoint节点配置错误，未找到指定类型的节点");
            }
            return  GetServiceChannel(bind, point.EndPointAddress);
        }

        public static T GetNetTcpService(InstanceContext context)
        {
            string name = typeof(T).FullName;
            List<Endpoint> points = AppStorage.EndPoints.Where((p) => { return p.Contract == name; }).ToList();
            Binding bind = null;
            Endpoint point = null;
            if (points.Count > 0)
            {
                point = points[0];
                bind = AppStorage.Bindings[point.BindingConfiguration];
            }
            else
            {
                throw new Exception("endpoint节点配置错误，未找到指定类型的节点");
            }

            DuplexChannelFactory<T> factory = new DuplexChannelFactory<T>(context ,bind,point.EndPointAddress );

            T service = factory.CreateChannel();
            return service ;

        }

        public static T GetServiceChannel(System.ServiceModel.Channels.Binding binding, EndpointAddress address)
        {
            //var cf = ((App)Application.Current).serviceConfig;
            if (binding == null || address == null)
            {
                throw new Exception("无法获取配置");
            }
            ChannelFactory<T> factory = new ChannelFactory<T>(binding, address);

            T service = factory.CreateChannel();
            return service;
        }

        public static void Close(T t)
        {
            var val = t as IContextChannel;
            if (val != null)
            {
                val.Close();
            }
        }

    }
}
