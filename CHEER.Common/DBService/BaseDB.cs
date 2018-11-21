using CHEER.Platform.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CHEER.Common.DBService
{
    public class BaseDB
    {
        private Hashtable htPropertyInfo = new Hashtable();

        protected string GetDBName(PropertyInfo pi)
        {
            if (!htPropertyInfo.ContainsKey(pi))
            {
                var attribute = pi.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault();
                var dbName = ((ColumnAttribute)attribute).ColumnName;
                htPropertyInfo.Add(pi, dbName);
                return dbName;
            }
            return htPropertyInfo[pi]?.ToString();
        }

        protected void Execute(string connection, string dbType, Action<PersistBroker> tryAction, Action<Exception> catchAction = null, Action finallyAction = null)
        {
            PersistBroker _broker;
            if (connection != null)
            {
                if (dbType != null)
                {
                    _broker = PersistBroker.Instance(connection, dbType);
                }
                else
                {
                    _broker = PersistBroker.Instance(connection);
                }
            }
            else
            {
                _broker = PersistBroker.Instance();
            }
            try
            {
                tryAction?.Invoke(_broker);
            }
            catch (Exception ex)
            {
                Log2.Instance.Error(GetType(), ex.Message, ex);
                catchAction?.Invoke(ex);
                throw ex;
            }
            finally
            {
                _broker.Close();
                finallyAction?.Invoke();
            }
        }

        protected string GetTableNameByClass<T>()
        {
            var classAttribute = (TableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(TableAttribute));
            if (classAttribute != null && !string.IsNullOrEmpty(classAttribute.TableName))
            {
                return classAttribute.TableName;
            }
            return typeof(T).Name;
        }

        protected void Log(string sql, Tuple<List<string>, List<object>> parameters)
        {
            Log2.Instance.Debug(GetType(), sql);

            if (parameters.Item1.Count > 0)
            {
                Log2.Instance.Debug(GetType(), "--------->>>>>>>>>---------paramters");

                for (var i = 0; i < parameters.Item1.Count; i++)
                {
                    Log2.Instance.Debug(GetType(), $"key: { parameters.Item1[i] }, value: { parameters.Item2[i] }");
                }

                Log2.Instance.Debug(GetType(), "--------->>>>>>>>>---------paramters");
            }
        }
    }
}
