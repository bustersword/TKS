using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TKS.DTO.Models;
using TKS.Service.Helper;
using Zlib.DbUtilities;

namespace RESTfulS.DAL
{
    public class IC_CAR_APPLY_DAL
    {
        //删除
        public int Delete(string data)
        {
            var obj = EntityExtension.GetEntity<IC_CAR_APPLY>(data);
            DBHelper DB = new DBHelper();
            int res = DB.DeleteNonDefults<IC_CAR_APPLY>(new IC_CAR_APPLY { DOC_NO = obj.DOC_NO });
            return res;
        }

        //更新
        public int Update(string data)
        {
            var obj = EntityExtension.GetEntity<IC_CAR_APPLY>(data);
            DBHelper DB = new DBHelper();
            int res = DB.Update<IC_CAR_APPLY>(obj);
            return res;
        }

        //新增
        public int Insert(string data)
        {
            var obj = EntityExtension.GetEntity<IC_CAR_APPLY>(data);
            DBHelper DB = new DBHelper();
            int res = DB.Insert<IC_CAR_APPLY>(obj);
            return res;
        }
        //查询
        public List<IC_CAR_APPLY> Select(string data)
        {
            var obj = EntityExtension.GetEntity<IC_CAR_APPLY>(data);
            DBHelper DB = new DBHelper();
            DB.ExecuteException= (e) => { throw new Exception(e.ExecuteMethod + ">>>" + e.ExecuteText + ">>>" + e._Exception.Message); };

            string _selectSql = @"select  *
            from IC_CAR_APPLY   where  USE_REASONS like  @USE_REASONS  and PROC_OPINION like  @PROC_OPINION  and
            status_code=@STATUS_CODE  and plan_start_time>to_date(@PLAN_START_TIME,'yyyy/mm/dd hh24:mi:ss')";
         
            DB.GetSqlStringCommond(_selectSql);
            DB.AddInParameter("USE_REASONS","%"+obj.USE_REASONS+"%" );
            DB.AddInParameter("PROC_OPINION", "%" + obj.PROC_OPINION+"%" );
            DB.AddInParameter("STATUS_CODE", obj.STATUS_CODE);
            DB.AddInParameter("PLAN_START_TIME", obj.PLAN_START_TIME.ToString("yyyy/MM/dd HH:mm:ss"));
            var res = DB.ExecuteSelectCommandToList<IC_CAR_APPLY>(true, false);

            return res;
        }
    }
}