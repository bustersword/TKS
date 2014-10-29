using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RESTfulS.DAL;

namespace RESTfulS.BAL
{
    public class IC_CAR_APPLY_U001
    {
        IC_CAR_APPLY_DAL dal = new IC_CAR_APPLY_DAL();
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Execute(string data)
        {
            return dal.Update(data);
        }
    }
}