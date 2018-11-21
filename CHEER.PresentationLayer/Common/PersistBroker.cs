using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace CHEER.PresentationLayer.Common
{
    public class PersistBroker
    {
        private MySqlConnection sqlConnection;
        public int timeout = 100;

        public PersistBroker(string connectionString, int timeout = 100)
        {
            sqlConnection = new MySqlConnection(connectionString);
            this.timeout = timeout;
            if (sqlConnection.State != ConnectionState.Open)
            {
                Delegate.ExecuteStatement(() =>
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
        public static PersistBroker Instance(int timeout = 1000)
        {
            //默认取config节点, 此后数据库默认为MYSQL
            var connectionString = "";
            Delegate.ExecuteStatement(() =>
            {
                connectionString = Encryption.DESDeCrypt(ConfigurationManager.AppSettings["ConnectionString"]);
            });
            return new PersistBroker(connectionString, timeout);
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
                throw new System.Exception("未能打开MySqlConnection, 请确认ConnectionString是否正确: " + sqlConnection.ConnectionString);
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
        /// 执行SQL语句, 返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql, params MySqlParameter[] parameters)
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                throw new System.Exception("未能打开MySqlConnection, 请确认ConnectionString是否正确: " + sqlConnection.ConnectionString);
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
                throw new System.Exception("未能打开MySqlConnection, 请确认ConnectionString是否正确: " + sqlConnection.ConnectionString);
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

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            if (sqlConnection != null && sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

    }
}