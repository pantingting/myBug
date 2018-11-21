using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CHEER.Common.DBService
{
    public class Delete: BaseDB
    {
        List<string> wheres = new List<string>();
        Tuple<List<string>, List<object>> parameters = new Tuple<List<string>, List<object>>(new List<string>(), new List<object>());
        private string tableName = "";
        string connection = "";
        string dbType = "";

        public Delete(string connection = null, string dbType = null)
        {
            this.connection = connection;
            this.dbType = dbType;
        }

        public Delete From(string tableName)
        {
            this.tableName = tableName;
            return this;
        }

        public Delete From<T>()
        {
            tableName = GetTableNameByClass<T>();
            return this;
        }

        public Delete Where(string where, params object[] conditions)
        {
            foreach (var condition in conditions)
            {
                string uuidKey = "@PARAM" + (parameters.Item1.Count + 1);
                where = new Regex("\\?", RegexOptions.IgnoreCase).Replace(where, uuidKey, 1);
                parameters.Item1.Add(uuidKey);
                parameters.Item2.Add(condition);
            }
            wheres.Add(where);
            return this;
        }

        private string BuildSQL ()
        {
            var wheres = "";
            if (this.wheres.Count > 0)
            {
                wheres = " WHERE " + string.Join(" AND ", this.wheres);
            }
            var sql = $@"DELETE FROM { tableName }{ wheres }";
            Log(sql, parameters);

            return sql;
        }

        public void Exec()
        {
            Execute(connection, dbType, _broker =>
            {
                _broker.ExecuteNonQuery(BuildSQL(), System.Data.CommandType.Text, parameters.Item1.ToArray(), parameters.Item2.ToArray());
            });
        }
    }
}
