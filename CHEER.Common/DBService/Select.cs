using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace CHEER.Common.DBService
{
    public class Select : BaseDB
    {
        List<string> columns = new List<string>();
        List<string> tables = new List<string>();
        List<string> innerJoins = new List<string>();
        List<string> leftJoins = new List<string>();
        List<string> rightJoins = new List<string>();
        List<string> wheres = new List<string>();
        Tuple<List<string>, List<object>> parameters = new Tuple<List<string>, List<object>>(new List<string>(), new List<object>());
        Dictionary<string, string> replaceSQLs = new Dictionary<string, string>();

        List<string> havings = new List<string>();
        List<string> groupBys = new List<string>();
        List<string> orderBys = new List<string>();
        int pageIndex = -1;
        int pageSize = -1;
        string connection = "";
        string dbType = "";

        public Select(string connection = null, string dbType = null)
        {
            this.connection = connection;
            this.dbType = dbType;
        }

        public Select Columns(params string[] columns)
        {
            this.columns.AddRange(columns);
            return this;
        }

        public Select From(params string[] tables)
        {
            this.tables.AddRange(tables);
            return this;
        }

        public Select From<T>(string asName = null)
        {
            if (!string.IsNullOrEmpty(asName))
            {
                tables.Add(GetTableNameByClass<T>() + " AS " + asName);
            }
            else
            {
                tables.Add(GetTableNameByClass<T>());
            }
            return this;
        }

        public Select InnerJoin(params string[] innerJoins)
        {
            this.innerJoins.AddRange(innerJoins);
            return this;
        }

        public Select InnerJoinT<T>(string onCondition, string asName = null, params object[] conditionValues)
        {
            onCondition = OnConditionWhere(onCondition, conditionValues);
            if (!string.IsNullOrEmpty(asName))
            {
                innerJoins.Add(GetTableNameByClass<T>() + " AS " + asName + " ON " + onCondition);
            }
            else
            {
                innerJoins.Add(GetTableNameByClass<T>() + " ON " + onCondition);
            }
            return this;
        }

        public Select InnerJoin(Select select, string onCondition, string asName = null, params object[] conditionValues)
        {
            onCondition = OnConditionWhere(onCondition, conditionValues);
            if (!string.IsNullOrEmpty(asName))
            {
                innerJoins.Add(select.ToString() + " AS " + asName + " ON " + onCondition);
            }
            else
            {
                innerJoins.Add(select.ToString() + " ON " + onCondition);
            }
            return this;
        }

        public Select LeftJoin(params string[] leftJoins)
        {
            this.leftJoins.AddRange(leftJoins);
            return this;
        }

        public Select LeftJoinT<T>(string onCondition, string asName = null, params object[] conditionValues)
        {
            if (!string.IsNullOrEmpty(asName))
            {
                leftJoins.Add(GetTableNameByClass<T>() + " AS " + asName + " ON " + onCondition);
            }
            else
            {
                leftJoins.Add(GetTableNameByClass<T>() + " ON " + onCondition);
            }
            return this;
        }

        public Select LeftJoin(Select select, string onCondition, string asName = null, params object[] conditionValues)
        {
            onCondition = OnConditionWhere(onCondition, conditionValues);
            if (!string.IsNullOrEmpty(asName))
            {
                leftJoins.Add(select.ToString() + " AS " + asName + " ON " + onCondition);
            }
            else
            {
                leftJoins.Add(select.ToString() + " ON " + onCondition);
            }
            return this;
        }

        public Select RightJoin(params string[] rightJoins)
        {
            this.rightJoins.AddRange(rightJoins);
            return this;
        }

        public Select RightJoinT<T>(string onCondition, string asName = null, params object[] conditionValues)
        {
            onCondition = OnConditionWhere(onCondition, conditionValues);
            if (!string.IsNullOrEmpty(asName))
            {
                rightJoins.Add(GetTableNameByClass<T>() + " AS " + asName + " ON " + onCondition);
            }
            else
            {
                rightJoins.Add(GetTableNameByClass<T>() + " ON " + onCondition);
            }
            return this;
        }

        public Select RightJoin(Select select, string onCondition, string asName = null, params object[] conditionValues)
        {
            onCondition = OnConditionWhere(onCondition, conditionValues);
            if (!string.IsNullOrEmpty(asName))
            {
                rightJoins.Add(select.ToString() + " AS " + asName + " ON " + onCondition);
            }
            else
            {
                rightJoins.Add(select.ToString() + " ON " + onCondition);
            }
            return this;
        }

        public Select ReplaceSQL(string sourceSQL, string replaceSQL)
        {
            this.replaceSQLs.Add(sourceSQL, replaceSQL);
            return this;
        }

        public Select OrderBy(params string[] orderBys)
        {
            this.orderBys.AddRange(orderBys);
            return this;
        }

        public Select GroupBy(params string[] groupBys)
        {
            this.groupBys.AddRange(groupBys);
            return this;
        }

        public Select Having(params string[] havings)
        {
            this.havings.AddRange(havings);
            return this;
        }

        public Select Limit(int pageIndex, int pageSize)
        {
            this.pageIndex = pageIndex;
            this.pageSize = pageSize;
            return this;
        }

        private string OnConditionWhere(string onCondition, params object[] conditions)
        {
            foreach (var condition in conditions)
            {
                string uuidKey = "@WHEREPARAM" + (parameters.Item1.Count + 1);
                onCondition = new Regex("\\?", RegexOptions.IgnoreCase).Replace(onCondition, uuidKey, 1);
                parameters.Item1.Add(uuidKey);
                parameters.Item2.Add(condition);
            }
            return onCondition;
        }

        public Select Where(string where, params object[] conditions)
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

        private string BuildSQL(bool isTotal = false)
        {
            string columns = " * ";
            if (this.columns.Count > 0)
            {
                columns = " " + string.Join(", ", this.columns) + " ";
            }
            string tables = " " + string.Join(", ", this.tables);
            string innerJoins = "";
            if (this.innerJoins.Count > 0)
            {
                innerJoins = " INNER JOIN " + string.Join(" INNER JOIN ", this.innerJoins);
            }
            string leftJoins = "";
            if (this.leftJoins.Count > 0)
            {
                leftJoins = " LEFT JOIN " + string.Join(" LEFT JOIN ", this.leftJoins);
            }
            string rightJoins = "";
            if (this.rightJoins.Count > 0)
            {
                rightJoins = " RIGHT JOIN " + string.Join(" RIGHT JOIN ", this.rightJoins);
            }
            string wheres = "";
            if (this.wheres.Count > 0)
            {
                wheres = " WHERE " + string.Join(" AND ", this.wheres);
            }
            string groupBys = "";
            if (this.groupBys.Count > 0)
            {
                groupBys = " GROUP BY " + string.Join(", ", this.groupBys);
            }
            string havings = "";
            if (this.havings.Count > 0)
            {
                havings = " HAVING " + string.Join(", ", this.havings);
            }
            string orderBys = "";
            if (this.orderBys.Count > 0)
            {
                orderBys = " ORDER BY " + string.Join(", ", this.orderBys);
            }
            string limit = "";
            if (pageIndex != -1)
            {
                limit = $" OFFSET { pageIndex * pageSize } ROWS FETCH NEXT { pageSize } ROWS ONLY";
            }

            var sql = "";
            if (isTotal)
            {
                sql = $@"SELECT COUNT(0) FROM{ tables }{ innerJoins }{ leftJoins }{ rightJoins }{ wheres }{ groupBys }{ havings }{ orderBys }";
            }
            else
            {
                if (orderBys == "")
                {
                    orderBys = " ORDER BY 1";
                }
                sql = $@"SELECT{ columns }FROM{ tables }{ innerJoins }{ leftJoins }{ rightJoins }{ wheres }{ groupBys }{ havings }{ orderBys }{ limit }";
            }

            var ide = replaceSQLs.GetEnumerator();
            while (ide.MoveNext())
            {
                sql = new Regex(ide.Current.Key, RegexOptions.IgnoreCase).Replace(sql, ide.Current.Value, 1);
            }

            Log(sql, parameters);

            return sql;
        }

        public int Count()
        {
            var total = 0;
            Execute(connection, dbType, _broker =>
            {
                var dt = _broker.ExecuteDataset(BuildSQL(true), CommandType.Text, parameters.Item1.ToArray(), parameters.Item2.ToArray()).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    int.TryParse(dt.Rows[0][0].ToString(), out total);
                }
            });
            return total;
        }

        public Dictionary<string, object> QueryForDictionary()
        {
            var dictionary = new Dictionary<string, object>();

            var dt = QueryForTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    dictionary.Add(column.ColumnName, dt.Rows[0][column.ColumnName]);
                }
            }

            return dictionary;
        }

        public List<Dictionary<string, object>> QueryForListDictionary()
        {
            var dictionarys = new List<Dictionary<string, object>>();

            var dt = QueryForTable();
            dt.AsEnumerable().ToList().ForEach(row =>
            {
                var dictionary = new Dictionary<string, object>();
                foreach (DataColumn column in dt.Columns)
                {
                    dictionary.Add(column.ColumnName, row[column.ColumnName]);
                }
                dictionarys.Add(dictionary);
            });

            return dictionarys;
        }

        public object QueryForObject()
        {
            object result = null;
            var dt = QueryForTable();
            if (dt.Rows.Count > 0)
            {
                result = dt.Rows[0][0];
            }
            return result;
        }

        public List<T> QueryForList<T>() where T : new()
        {
            List<T> lists = DataTableToList<T>(QueryForTable());
            return lists;
        }

        private List<T> DataTableToList<T>(DataTable dt) where T : new()
        {
            List<T> lists = new List<T>();
            var properties = typeof(T).GetProperties();

            dt.AsEnumerable().ToList().ForEach(row =>
            {
                T t = new T();

                foreach (var pi in properties)
                {
                    var dbName = pi.Name; ;
                    var attribute = pi.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault();
                    if (attribute != null)
                    {
                        dbName = ((ColumnAttribute)attribute).ColumnName;
                    }
                    if (dbName == null)
                    {
                        dbName = pi.Name;
                    }
                    if (!dt.Columns.Contains(dbName))
                    {
                        continue;
                    }

                    if (pi.PropertyType == typeof(string))
                    {
                        pi.SetValue(t, row[dbName].ToString());
                    }
                    else if (pi.PropertyType == typeof(int))
                    {
                        int value = 0;
                        int.TryParse(row[dbName].ToString(), out value);
                        pi.SetValue(t, value);
                    }
                    else if (pi.PropertyType == typeof(double))
                    {
                        double value = 0.0;
                        double.TryParse(row[dbName].ToString(), out value);
                        pi.SetValue(t, value);
                    }
                    else if (pi.PropertyType == typeof(float))
                    {
                        float value = 0;
                        float.TryParse(row[dbName].ToString(), out value);
                        pi.SetValue(t, value);
                    }
                    else if (pi.PropertyType == typeof(decimal))
                    {
                        decimal value = 0;
                        decimal.TryParse(row[dbName].ToString(), out value);
                        pi.SetValue(t, value);
                    }
                    else if (pi.PropertyType == typeof(bool))
                    {
                        bool value = false;
                        bool.TryParse(row[dbName].ToString(), out value);
                        if (row[dbName].ToString() == "1") { value = true; }
                        pi.SetValue(t, value);
                    }
                }

                lists.Add(t);
            });
            return lists;
        }

        public T QueryForEntity<T>() where T : new()
        {
            var lists = QueryForList<T>();
            if (lists.Count > 0)
            {
                return lists[0];
            }
            return default(T);
        }

        public DataTable QueryForTable()
        {
            var dt = new DataTable();
            Execute(connection, dbType, _broker =>
            {
                dt = _broker.ExecuteDataset(ToString(), CommandType.Text, parameters.Item1.ToArray(), parameters.Item2.ToArray()).Tables[0];
            });
            return dt;
        }

        public PageList<T> Paging<T>() where T : new()
        {
            var pageList = new PageList<T>();
            Execute(connection, dbType, _broker =>
            {
                var dtPage = _broker.ExecuteDataset(BuildSQL(), CommandType.Text, parameters.Item1.ToArray(), parameters.Item2.ToArray()).Tables[0];
                var dtTotal = _broker.ExecuteDataset(BuildSQL(true), CommandType.Text, parameters.Item1.ToArray(), parameters.Item2.ToArray()).Tables[0];
                pageList.Total = int.Parse(dtTotal.Rows[0][0].ToString());
                pageList.Data = DataTableToList<T>(dtPage);
            });
            return pageList;
        }

        public Page Paging()
        {
            var page = new Page();
            Execute(connection, dbType, _broker =>
            {
                var dtPage = _broker.ExecuteDataset(BuildSQL(), CommandType.Text, parameters.Item1.ToArray(), parameters.Item2.ToArray()).Tables[0];
                var dtTotal = _broker.ExecuteDataset(BuildSQL(true), CommandType.Text, parameters.Item1.ToArray(), parameters.Item2.ToArray()).Tables[0];
                page.Total = int.Parse(dtTotal.Rows[0][0].ToString());
                page.Data = dtPage;
            });
            return page;
        }

        public override string ToString()
        {
            return BuildSQL();
        }

        public class PageList<T>
        {
            public int Total { get; set; }
            public List<T> Data { get; set; } = new List<T>();
        }

        public class Page
        {
            public int Total { get; set; }
            public DataTable Data { get; set; } = new DataTable();
        }
    }
}
