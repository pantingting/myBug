using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHEER.Service.Extension
{
    public static class DBExtension
    {
        /// <summary>
        /// 扩展字段, 将数据库的'变成'',防止SQL注入
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DBReplace(this string str)
        {
            return str.Replace(@"\", @"\\").Replace(@"'", @"''");
        }


    }
}
