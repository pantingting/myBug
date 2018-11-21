using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHEER.Common
{
    public class OraclePersistBroker
    {
        private OracleConnection sqlConnection;
        public int timeout = 100;

        public OraclePersistBroker(string connectionString, int timeout = 100)
        {
            sqlConnection = new OracleConnection(connectionString);
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
        public static OraclePersistBroker Instance(int timeout = 1000)
        {
            //默认取config节点, 此后数据库默认为Oracle
            var connectionString = "";
            Business.ExecuteStatement(() =>
            {
                connectionString = ConfigurationManager.AppSettings["OracleConnectionString"];
            });
            return new OraclePersistBroker(connectionString, timeout);
        }

        /// <summary>
        /// 执行NonQuery
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, params OracleParameter[] parameters)
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                throw new Exception("未能打开OracleConnection, 请确认OracleConnectionString是否正确: " + sqlConnection.ConnectionString);
            }
            using (OracleCommand cmd = sqlConnection.CreateCommand())
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
                throw new Exception("未能打开OracleConnection, 请确认OracleConnectionString是否正确: " + sqlConnection.ConnectionString);
            }
            using (OracleCommand cmd = sqlConnection.CreateCommand())
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
        public object ExecuteScalar(string sql, params OracleParameter[] parameters)
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                throw new Exception("未能打开OracleConnection, 请确认OracleConnectionString是否正确: " + sqlConnection.ConnectionString);
            }
            using (OracleCommand cmd = sqlConnection.CreateCommand())
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
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql)
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                throw new Exception("未能打开OracleConnection, 请确认OracleConnectionString是否正确: " + sqlConnection.ConnectionString);
            }
            DataTable table = new DataTable();
            using (OracleCommand cmd = sqlConnection.CreateCommand())
            {
                cmd.CommandTimeout = timeout;
                cmd.CommandText = sql;
                using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                {
                    adapter.Fill(table);
                }
                return table;
            }
        }

        /// <summary>
        /// 执行SQL语句, 返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql, params OracleParameter[] parameters)
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                throw new Exception("未能打开OracleConnection, 请确认OracleConnectionString是否正确: " + sqlConnection.ConnectionString);
            }
            DataTable table = new DataTable();
            using (OracleCommand cmd = sqlConnection.CreateCommand())
            {
                cmd.CommandTimeout = timeout;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters);
                using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
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
        public DataSet ExecuteDataSet(string sql, params OracleParameter[] parameters)
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                throw new Exception("未能打开OracleConnection, 请确认OracleConnectionString是否正确: " + sqlConnection.ConnectionString);
            }
            DataSet dataSet = new DataSet();
            using (OracleCommand cmd = sqlConnection.CreateCommand())
            {
                cmd.CommandTimeout = timeout;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters);
                using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                {
                    adapter.Fill(dataSet);
                }
                return dataSet;
            }
        }

        public OracleDataReader ExecuteDataReader(string sql)
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                throw new Exception("未能打开MySqlConnection, 请确认OracleConnectionString是否正确: " + sqlConnection.ConnectionString);
            }
            using (OracleCommand cmd = sqlConnection.CreateCommand())
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
