using System;
using System.Collections.Generic;
using System.Linq;

namespace CHEER.Common.DBService
{
    public class Insert : BaseDB
    {
        Tuple<List<string>, List<object>> parameters = new Tuple<List<string>, List<object>>(new List<string>(), new List<object>());
        private Dictionary<string, object> columns = new Dictionary<string, object>();
        private string tableName = "";
        string connection = "";
        string dbType = "";

        public Insert(string connection = null, string dbType = null)
        {
            this.connection = connection;
            this.dbType = dbType;
        }

        public Insert Into(string tableName)
        {
            this.tableName = tableName;
            return this;
        }

        public Insert Into<T>()
        {
            tableName = GetTableNameByClass<T>();
            return this;
        }

        public Insert Set(String column, Object value)
        {
            columns.Add(column, value);
            return this;
        }

        public Insert Set(Dictionary<string, object> columns)
        {
            this.columns.Concat(columns);
            return this;
        }

        public Insert Save<T>(T entity, bool isGuidPrimaryKey = true)
        {
            var properties = typeof(T).GetProperties();
            tableName = GetTableNameByClass<T>();

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
                if (((ColumnAttribute)attribute).CanInsert)
                {
                    Set(dbName, pi.GetValue(entity));
                }
                if (((ColumnAttribute)attribute).IsPrimaryKey && isGuidPrimaryKey)
                {
                    Set(dbName, Guid.NewGuid());
                }
            }

            return this;
        }

        private string BuildSQL()
        {
            var columns = new List<string>();
            var columnValues = new List<string>();
            this.columns.AsEnumerable().ToList().ForEach(columnValue =>
            {
                columns.Add(columnValue.Key);
                columnValues.Add("@" + columnValue.Key);
                parameters.Item1.Add("@" + columnValue.Key);
                parameters.Item2.Add(columnValue.Value);
            });
            var sql = $@"INSERT INTO { tableName } ({ string.Join(", ", columns) }) VALUES ({ string.Join(", ", columnValues) })";
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

        public int Exec(bool needInsertId = false)
        {
            int lastInsertId = 0;
            Execute(connection, dbType, _broker =>
            {
                _broker.ExecuteNonQuery(BuildSQL(), System.Data.CommandType.Text, parameters.Item1.ToArray(), parameters.Item2.ToArray());
                if (needInsertId)
                {
                    lastInsertId = int.Parse(_broker.ExecuteSQLForDst("SELECT LAST_INSERT_ID()").Tables[0].Rows[0][0].ToString());
                }
            });
            return lastInsertId;
        }
    }
}
