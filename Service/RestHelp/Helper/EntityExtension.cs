using TKS.DTO.Core;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.Service.Helper
{
    /// <summary>
    /// 实体扩展类
    /// </summary>
    public static class EntityExtension
    {
        /// <summary>
        /// 将json数据转换为单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="DB"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T GetEntity<T>(string data) where T : BaseModel, new()
        {

            List<T> results = GetEntities<T>(data);
            if (results.Count == 0)
            {
                throw new Exception("指定的json字符串未能转换为实体类" + typeof(T).Name);
            }

            return results[0];
        }

        /// <summary>
        /// 将json数据转换为实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="DB"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<T> GetEntities<T>( string data) where T : BaseModel, new()
        {
            Type tt = typeof(T);
            if (!tt.IsSubclassOf(typeof(BaseModel)))
            {
                throw new Exception("传入的类型必须继承自BaseModel");
            }
            JsonDeserializer json = new JsonDeserializer();
            List<Dictionary<string, object>> res = new List<Dictionary<string, object>>();
            res = json.Deserialize<List<Dictionary<string, object>>>(new RestResponse { Content = data });

            List<T> models = new List<T>();

            for (int i = 0; i < res.Count; i++)
            {
                T temp = new T();
                ((BaseModel)temp).SetDic(res[i]);
                models.Add(temp);
            }


            return models;
        }


    }
}
