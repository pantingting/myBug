using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHEER.Common
{
    public class MyPersistBroker
    {
        private MySqlConnection sqlConnection;
        public int timeout = 100;

        public MyPersistBroker(string connectionString, int timeout = 100)
        {
            sqlConnection = new MySqlConnection(connectionString);
            this.timeout = timeout;
            if (sqlConnection.State != ConnectionState.Open)
            {
                Business.ExecuteStatement(() =>
                {
                    sqlConnection.Open();
                });
            }
        }

        public ConnectionState State
        {
            get
            {
                return sqlConnection.State;
            }
        }

        /// <summary>
        /// 数据库初始化, 目的兼容以前的写法风格
        /// </summary>
        /// <returns></returns>
        public static MyPersistBroker Instance(int timeout = 1000)
        {
            //默认取config节点, 此后数据库默认为MYSQL
            var connectionString = "";
            Business.ExecuteStatement(() =>
            {
                connectionString = new Encryption().DESDeCrypt(ConfigurationManager.AppSettings["ConnectionString"]);
            });
            return new MyPersistBroker(connectionString, timeout);
        }

        /// <summary>
        /// 执行NonQuery
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, params MySqlParameter[] parameters)
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                throw new Exception("未能打开MySqlConnection, 请确认ConnectionString是否正确: " + sqlConnection.ConnectionString);
            }
            using (MySqlCommand cmd = sqlConnection.CreateCommand())
            {
                cmd.CommandTimeout = timeout;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="action"></param>
        public void ExecuteTransaction(Action successAction, Action<Exception> failAction)
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                throw new Exception("未能打开MySqlConnection, 请确认ConnectionString是否正确: " + sqlConnection.ConnectionString);
            }
            using (MySqlCommand cmd = sqlConnection.CreateCommand())
            {
                var transaction = sqlConnection.BeginTransaction();
                cmd.Transaction = transaction;
                Business.ExecuteStatement(() =>
                {
                    successAction?.Invoke();
                    transaction.Commit();
                }, ex =>
                {
                    failAction?.Invoke(ex);
                    transaction.Rollback();
                    throw ex;
                });
            }
        }

        /// <summary>
        /// 执行ExecuteScalar
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, params MySqlParameter[] parameters)
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                throw new Exception("未能打开MySqlConnection, 请确认ConnectionString是否正确: " + sqlConnection.ConnectionString);
            }
            using (MySqlCommand cmd = sqlConnection.CreateCommand())
            {
                cmd.CommandTimeout = timeout;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// 执行SQL语句, 返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql, params MySqlParameter[] parameters)
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                throw new Exception("未能打开MySqlConnection, 请确认ConnectionString是否正确: " + sqlConnection.ConnectionString);
            }
            DataTable table = new DataTable();
            using (MySqlCommand cmd = sqlConnection.CreateCommand())
            {
                cmd.CommandTimeout = timeout;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters);
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(table);
                }
                return table;
            }
        }

        /// <summary>
        /// 执行SQL语句, 返回DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sql, params MySqlParameter[] parameters)
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                throw new Exception("未能打开MySqlConnection, 请确认ConnectionString是否正确: " + sqlConnection.ConnectionString);
            }
            DataSet dataSet = new DataSet();
            using (MySqlCommand cmd = sqlConnection.CreateCommand())
            {
                cmd.CommandTimeout = timeout;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters);
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dataSet);
                }
                return dataSet;
            }
        }

        public MySqlDataReader ExecuteDataReader(string sql)
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                throw new Exception("未能打开MySqlConnection, 请确认ConnectionString是否正确: " + sqlConnection.ConnectionString);
            }
            using (MySqlCommand cmd = sqlConnection.CreateCommand())
            {
                cmd.CommandTimeout = timeout;
                cmd.CommandText = sql;
                return cmd.ExecuteReader();
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            if (sqlConnection != null && sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Dispose();
            }
        }

    }
}