using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace CHEER.Common
{
    public class Delegate
    {
        public static void ExecuteRedis(Action<IDatabase, string> redisClient, Action<Exception> exAction = null)
        {
            ExecuteStatement(() =>
            {
                var db = Redis.Instance.GetRedis().GetDatabase(Redis.Instance.RedisDB);
                redisClient?.Invoke(db, RedisPrefix);
            }, ex =>
            {
                Log2.Instance.Error(typeof(IRedis), ex.Message, ex);
                exAction?.Invoke(ex);
            });
        }


        public static string RedisPrefix = "MARKET";

        public static async Task ExecuteStatementAsync(Func<Task> actionAsync, Action<Exception> catchDelegate)
        {
            ExceptionDispatchInfo capturedException = null;
            try
            {
                await actionAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                capturedException = ExceptionDispatchInfo.Capture(ex);
            }

            if (capturedException != null)
            {
                catchDelegate?.Invoke(capturedException.SourceException);
            }
        }

        public static void ExecuteStatement(Action tryDelegate, Action<Exception> catchDelegate, Action finallyDelegate = null)
        {
            try
            {
                tryDelegate?.Invoke();
            }
            catch (Exception ex)
            {
                catchDelegate?.Invoke(ex);
            }
            finally
            {
                finallyDelegate?.Invoke();
            }
        }

        /// <summary>
        /// 执行语句Try-Finally
        /// </summary>
        public static void ExecuteStatement(Action tryDelegate, Action catchDelegate = null)
        {
            try
            {
                tryDelegate?.Invoke();
            }
            catch
            {
                catchDelegate?.Invoke();
            }
        }

        public static void ExecuteSQL(Action<MyPersistBroker> brokerAction, Action<Exception> exAction)
        {
            MyPersistBroker _broker = MyPersistBroker.Instance();
            ExecuteStatement(() =>
            {
                brokerAction?.Invoke(_broker);
            }, ex =>
            {
                exAction?.Invoke(ex);
            }, () =>
            {
                _broker.Close();
            });
        }

        public static void ExecuteOracleSQL(Action<OraclePersistBroker> brokerAction, Action<Exception> exAction)
        {
            OraclePersistBroker _broker = OraclePersistBroker.Instance();
            ExecuteStatement(() =>
            {
                brokerAction?.Invoke(_broker);
            }, ex =>
            {
                exAction?.Invoke(ex);
            }, () =>
            {
                _broker.Close();
            });
        }
    }
}
