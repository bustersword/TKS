using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RESTfulS.DAL;
using System.Threading;

namespace RESTfulS.BAL
{
    public class IC_CAR_APPLY_D001
    {
        IC_CAR_APPLY_DAL dal = new IC_CAR_APPLY_DAL();

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Execute(string data)
        {
            Thread.Sleep(5000);
            return dal.Delete(data);
        }
        
    }
}