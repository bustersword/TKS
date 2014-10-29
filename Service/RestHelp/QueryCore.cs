using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Zlib.DbUtilities;

namespace TKS.Service
{
    public class QueryCore
    {

        string sql;
        int _pageNum;
        int _pageSize;
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        public QueryCore(string selectSqlString, string param,int pageNum,int pageSize)
        {

            JsonDeserializer json = new JsonDeserializer();
            if (!string.IsNullOrEmpty(param))
                parameters = json.Deserialize<Dictionary<string, object>>(new RestResponse { Content = param });
            sql = selectSqlString;
            _pageNum = pageNum;
            _pageSize=pageSize;
        }

        public string GetData(out int totalNum)
        {
            string currentSql = "";
            string pagesql = MakeSqlToPageSql(sql,_pageNum,_pageSize,1);
            string countSql = MakeSqlToPageSql(sql, _pageNum, _pageSize, 0);
            List<string> param = GetParams(sql);
            DBHelper DB = new DBHelper();
            DB.IsAutoOpenClose = false;
            DB.ExecuteException += (e) => { throw new Exception("元SQL："+sql+"拼接后SQL数据：" + currentSql + ".方法" + e.ExecuteMethod + ">>>" + e.ExecuteText + ">>>" + e._Exception.Message); };
            currentSql = countSql;
            DB.GetSqlStringCommond(countSql);
            for (int i = 0; i < param.Count; i++)
            {
                string key = param[i].Replace("@", "");
                DB.AddInParameter(key, parameters[key]);
            }
            totalNum= int.Parse ( DB.ExecuteScalar().ToString());


            currentSql = pagesql;
            DB.GetSqlStringCommond(pagesql);
            for (int i = 0; i < param.Count; i++)
            {
                string key = param[i].Replace("@", "");
                DB.AddInParameter(key, parameters[key]);
            }

            string result = DB.ExecuteSelectCmdGetJson();

            DB.Close();
            return result;
        }

        List<string> GetParams(string sql)
        {
            Regex re = new Regex(@"@[A-Za-z0-9_]+[^)\s]\b");
            IEnumerator param = re.Matches(sql).GetEnumerator();
            List<string> result = new List<string>();
            while (param.MoveNext())
            {
                result.Add(param.Current.ToString());
            }

            return result.Distinct().ToList();
        }

        string MakeSqlToPageSql(string sql, int PageNumber, int PageSize, int AllCount)
        {
            if (AllCount == 0)
            {
                //生成统计语句　
                return "select count(*) from (" + sql + ") ";
            }
            //分页摸板语句
            string SqlTemplate = @"SELECT * FROM
 (SELECT rownum r_n,temptable.* FROM  
   ( @@SourceSQL ) temptable Where rownum <= @@RecEnd
 ) temptable2 WHERE r_n >= @@RecStart ";

            int iRecStart = (PageNumber - 1) * PageSize + 1;
            int iRecEnd = PageNumber * PageSize;

            //执行参数替换
            string SQL = SqlTemplate.Replace("@@SourceSQL", sql)
                .Replace("@@RecStart", iRecStart.ToString())
                .Replace("@@RecEnd", iRecEnd.ToString());
            return SQL;
        }
    }
}
