using RESTfulS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using TKS.DTO.Models ;
namespace RESTfulS.BAL
{
    public class IC_CAR_APPLY_S001
    {
        IC_CAR_APPLY_DAL dal = new IC_CAR_APPLY_DAL();
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<IC_CAR_APPLY> Execute(string data)
        {
           
            return dal.Select(data);
        }

    }
}