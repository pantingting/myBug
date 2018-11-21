using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CHEER.Common.DBService
{
    public class Update : BaseDB
    {
        Tuple<List<string>, List<object>> parameters = new Tuple<List<string>, List<object>>(new List<string>(), new List<object>());
        private Dictionary<string, object> columns = new Dictionary<string, object>();
        List<string> wheres = new List<string>();
        private string tableName = "";
        string connection = "";
        string dbType = "";

        public Update(string connection = null, string dbType = null)
        {
            this.connection = connection;
            this.dbType = dbType;
        }

        public Update Table(string tableName)
        {
            this.tableName = tableName;
            return this;
        }

        public Update Table<T>()
        {
            tableName = GetTableNameByClass<T>();
            return this;
        }

        public Update Set(String column, Object value)
        {
            columns.Add(column, value);
            return this;
        }

        public Update Set(Dictionary<string, object> columns)
        {
            this.columns.Concat(columns);
            return this;
        }

        public Update Save<T>(T entity, bool needPrimaryKey = true)
        {
            tableName = GetTableNameByClass<T>();
            var properties = typeof(T).GetProperties();

            foreach (var pi in properties)
            {
                var attribute = pi.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault();
                if (attribute == null)
                {
                    attribute = new ColumnAttribute();
                }
                var dbName = ((ColumnAttribute)attribute).ColumnName;
                if (dbName == null)
                {
                    dbName = pi.Name;
                }
                if (((ColumnAttribute)attribute).CanUpdate)
                {
                    Set(dbName, pi.GetValue(entity));
                }
                if (((ColumnAttribute)attribute).IsPrimaryKey && needPrimaryKey)
                {
                    Where(dbName + " = ?", pi.GetValue(entity));
                }
            }

            return this;
        }

        public Update Where(string where, params object[] conditions)
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

        private string BuildSQL()
        {
            var columns = new List<string>();
            var columnValues = new List<string>();
            var wheres = "";
            if (this.wheres.Count > 0)
            {
                wheres = " WHERE " + string.Join(" AND ", this.wheres);
            }
            this.columns.AsEnumerable().ToList().ForEach(columnValue =>
            {
                if(columnValue.Value != null)
                {
                    columns.Add(columnValue.Key + " = " + "@" + columnValue.Key);
                    parameters.Item1.Add("@" + columnValue.Key);
                    parameters.Item2.Add(columnValue.Value);
                }
            });

            var sql = $@"UPDATE { tableName } SET { string.Join(", ", columns) }{ wheres }";
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
