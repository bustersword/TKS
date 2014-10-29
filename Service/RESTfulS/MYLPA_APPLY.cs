using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TKS.DTO.Models;
using Zlib.DbUtilities;
using TKS.Service.Helper;
namespace RESTfulS
{
    public class MYLPA_APPLY
    {
        public List<ExampleData> Test(string data_a, string data_b, string data_c)
        {
            DBHelper DB = new DBHelper();
            var a = EntityExtension.GetEntities<ExampleData>(data_a);
            var b = EntityExtension.GetEntity<ExampleData>(data_b);
            var c = data_c;

            return a;
        }

        public ExampleData Test(string data)
        {
            DBHelper DB = new DBHelper();
            var result = EntityExtension.GetEntity<ExampleData>(data);
            return result;
        }

        public void Test()
        {
            //return "I don't recognize u";
        }

        public string FuncA(string a,string b ,string c)
        {
            return a + b + c;
        }

        public string FuncB()
        {
            return "this is function B";
        }

        public List<IC_CAR_APPLY> FuncC()
        {
            DBHelper DB = new DBHelper();
           var result= DB.Select<IC_CAR_APPLY>(new IC_CAR_APPLY ());
            return result ;
        }

     
    }
}