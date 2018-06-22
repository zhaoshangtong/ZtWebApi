using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DbModels.DbContextFactory
{
    public class DbContextFactory
    {
        /// <summary>
        /// 获取当前EF数据上下文
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static DbContext GetCurrentDbContext(string key=null)
        {
            DbContext nContext = CallContext.GetData(key + "DBContext") as DbContext;
            if (nContext == null)
            {
                nContext = GetContextByKey(key);
                CallContext.SetData(key + "DBContext", nContext);
            }
            return nContext;
        }

        internal static DbContext GetContextByKey(string key)
        {
            DbContext db = new EFMssqlCodeFirst.EFCodeFirst();
            if (key == "EFMysql")
            {
                db = new EFMysql.EFMysql();
            }
            return db;
        }
    }
}
