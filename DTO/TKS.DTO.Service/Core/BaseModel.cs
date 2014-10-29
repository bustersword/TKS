using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
#if SILVERLIGHT
using RestSharp;
#else
using System.Data;
using System.Collections;
using RestSharp;

#endif

namespace TKS.DTO.Core
{
    public interface IBaseModel
    {
        string GetJson();

    }
    public class BaseModel : IBaseModel, INotifyPropertyChanged 
    {
        protected Dictionary<string, object> BaseData = new Dictionary<string, object>();

        public void SetDic(Dictionary<string, object> dic)
        {
            BaseData = dic;
        }

        /// <summary>
        /// 获取字段对应的值
        /// </summary>
        /// <typeparam name="T">字段类型</typeparam>
        /// <param name="field">字段名</param>
        /// <returns>返回字段值</returns>
        public  T GetDataField<T>(string field)
        {
#if SILVERLIGHT
            if (BaseData.ContainsKey(field))
                try
                {
                    if (typeof(T) == typeof(string))
                        return (T)((object)BaseData[field]);
                    if (typeof(T) == typeof(DateTime))
                        return (T)((object)DateTime.Parse(BaseData[field].ToString()));
                    if (typeof(T) == typeof(Int32))
                        return (T)((object)Int32.Parse(BaseData[field].ToString()));
                    if (typeof(T) == typeof(Double))
                        return (T)((object)Double.Parse(BaseData[field].ToString()));
                    if (typeof(T) == typeof(float))
                        return (T)((object)float.Parse(BaseData[field].ToString()));
                    if (typeof(T) == typeof(Decimal))
                        return (T)((object)Decimal.Parse(BaseData[field].ToString()));
                    if (typeof(T) == typeof(long))
                        return (T)((object)long.Parse(BaseData[field].ToString()));
                    if (typeof(T) == typeof(bool))
                        return (T)((object)bool.Parse(BaseData[field].ToString()));

                    if (typeof(T) == typeof(long?))
                    {
                        if (BaseData[field] == null)
                        {
                            return (T)((object)null);
                        }
                        return (T)((object)long.Parse(BaseData[field].ToString()));
                    }

                    if (typeof(T) == typeof(DateTime?))
                    {
                        if (BaseData[field] == null)
                        {
                            return (T)((object)null);
                        }
                        return (T)((object)DateTime.Parse(BaseData[field].ToString()));
                    }

                    if (typeof(T) == typeof(Int32?))
                    {
                        if (BaseData[field] == null)
                        {
                            return (T)((object)null);
                        }
                        return (T)((object)Int32.Parse(BaseData[field].ToString()));
                    }
                    if (typeof(T) == typeof(Double?))
                    {
                        if (BaseData[field] == null)
                        {
                            return (T)((object)null);
                        }
                        return (T)((object)Double.Parse(BaseData[field].ToString()));
                    }

                    if (typeof(T) == typeof(float?))
                    {
                        if (BaseData[field] == null)
                        {
                            return (T)((object)null);
                        }
                        return (T)((object)float.Parse(BaseData[field].ToString()));
                    }

                    if (typeof(T) == typeof(Decimal?))
                    {
                        if (BaseData[field] == null)
                        {
                            return (T)((object)null);
                        }
                        return (T)((object)Decimal.Parse(BaseData[field].ToString()));
                    }

                    if (typeof(T) == typeof(bool?))
                    {
                        if (BaseData[field] == null)
                        {
                            return (T)((object)null);
                        }
                        return (T)((object)bool.Parse(BaseData[field].ToString()));
                    }

                    return default(T);
                }
                catch (Exception ex)
                {
                    return default(T);
                }
            else
                return default(T);
#else
            if (BaseData.ContainsKey(field))
                try
                {
#if SILVERLIGHT
                    return (T)Convert.ChangeType(BaseData[field], typeof(T),null); 
#else
                    return (T)Convert.ChangeType(BaseData[field], typeof(T));
#endif

                }
                catch (Exception ex)
                {
                    return default(T);
                }
            else
                return default(T);
#endif
        }

        /// <summary>
        /// 设置字段值
        /// </summary>
        /// <typeparam name="T">字段类型（string)）</typeparam>
        /// <param name="field">字段名称</param>
        /// <param name="value">字段值</param>
        public  void SetDataField<T>(string field, T value)
        {
            if (BaseData.ContainsKey(field))
            {
                BaseData[field] = value;
            }
            else
            {
                BaseData.Add(field, value);
            }
        }

        public string GetJson()
        {
         
            IDictionary<string, object> datas = BaseData as IDictionary<string, object>;
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            IEnumerator<string> ke = datas.Keys.GetEnumerator();
            IEnumerator<object> ve = datas.Values.GetEnumerator();
            bool first = true;
            while (ke.MoveNext() && ve.MoveNext())
            {
                object key = ke.Current;
                object value = ve.Current;
                if (!first)
                    builder.Append(",");
                string stringKey = key as string;
                if (stringKey != null)
                    SerializeString(stringKey, builder);
                builder.Append(":");
                SerializeValue(value, builder);
                first = false;
            }
            builder.Append("}");
            return builder.ToString();
 
        }
        bool SerializeValue(object  value, StringBuilder builder)
        {
            bool flag = IsString(value);
            if(flag)
                builder.Append("\"");
            char[] charArray = value.ToString().ToCharArray();
            for (int i = 0; i < charArray.Length; i++)
            {
                char c = charArray[i];
                if (c == '"')
                    builder.Append("\\\"");
                else if (c == '\\')
                    builder.Append("\\\\");
                else if (c == '\b')
                    builder.Append("\\b");
                else if (c == '\f')
                    builder.Append("\\f");
                else if (c == '\n')
                    builder.Append("\\n");
                else if (c == '\r')
                    builder.Append("\\r");
                else if (c == '\t')
                    builder.Append("\\t");
                else
                    builder.Append(c);
            }
            if(flag )
                builder.Append("\"");
            return true;
        }

        bool SerializeString(string aString, StringBuilder builder)
        {
            builder.Append("\"");
            char[] charArray = aString.ToCharArray();
            for (int i = 0; i < charArray.Length; i++)
            {
                char c = charArray[i];
                if (c == '"')
                    builder.Append("\\\"");
                else if (c == '\\')
                    builder.Append("\\\\");
                else if (c == '\b')
                    builder.Append("\\b");
                else if (c == '\f')
                    builder.Append("\\f");
                else if (c == '\n')
                    builder.Append("\\n");
                else if (c == '\r')
                    builder.Append("\\r");
                else if (c == '\t')
                    builder.Append("\\t");
                else
                    builder.Append(c);
            }
            builder.Append("\"");
            return true;
        }
        bool IsString(object value)
        {
            Type T = value.GetType();
            bool flag = true;
            switch (T.Name)
            {
                case Types._int16:
                case Types._int32:
                case Types._int64:
                case Types._float:
                case Types._double:
                case Types._decimal:
                    flag = false;
                    break;
            }

            return flag;
        }

        public class Types
        {
            public const string _int16 = "Int16";
            public const string _int32 = "Int32";
            public const string _int64 = "Int64";
            public const string _float = "Single";
            public const string _double = "Double";
            public const string _decimal = "Decimal";

        }

        protected string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }
            MemberExpression body = propertyExpression.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException("Invalid argument", "propertyExpression");
            }
            PropertyInfo member = body.Member as PropertyInfo;
            if (member == null)
            {
                throw new ArgumentException("Argument is not a property", "propertyExpression");
            }
            return member.Name;
        }

        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                string propertyName = this.GetPropertyName<T>(propertyExpression);
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

       

        public event PropertyChangedEventHandler PropertyChanged;

        

     
    }
}
