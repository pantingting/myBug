using Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using System.Web;

namespace CHEER.PresentationLayer.Common
{
    public class Delegate
    {
        public static void ExecuteRedis(Action<RedisClient> redisClient)
        {
            var redisConfig = ConfigurationManager.AppSettings["Redis"];
            var client = new RedisClient(redisConfig.Split(':')[0], int.Parse(redisConfig.Split(':')[1]));
            ExecuteStatement(() =>
            {
                redisClient(client);
            }, () =>
            {
                client.Dispose();
            });
        }

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

        public static void ExecuteSQL(Action<PersistBroker> brokerAction, Action<Exception> exAction)
        {
            PersistBroker _broker = PersistBroker.Instance();
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