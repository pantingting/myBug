using CHEER.Platform.DAL;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CHEER.Common
{
    public class Business
    {
        public static void ExecuteStatement(Action tryAction, Action<Exception> exAction = null, Action finallyAction = null)
        {
            try
            {
                tryAction?.Invoke();
            }
            catch (Exception ex)
            {
                Log2.Instance.Error(typeof(Action), ex.Message, ex);
                exAction?.Invoke(ex);
            }
            finally
            {
                finallyAction?.Invoke();
            }
        }

        public static string RedisPrefix = "CHOOSITON";

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

        public static void ExecuteRedis(Action<ConnectionMultiplexer> redisClient, Action<Exception> exAction = null)
        {
            ExecuteStatement(() =>
            {
                var db = Redis.Instance.GetRedis();
                redisClient?.Invoke(db);
            }, ex =>
            {
                Log2.Instance.Error(typeof(IRedis), ex.Message, ex);
                exAction?.Invoke(ex);
            });
        }

        public static void ExecuteSQL(Action<PersistBroker> brokerAction, Action<Exception> exAction = null, PersistBroker broker = null)
        {
            PersistBroker _broker = broker ?? PersistBroker.Instance();
            ExecuteStatement(() =>
            {
                brokerAction?.Invoke(_broker);
            }, ex =>
            {
                Log2.Instance.Error(typeof(PersistBroker), ex.Message, ex);
                exAction?.Invoke(ex);
            }, () =>
            {
                if (broker == null)
                {
                    _broker.Close();
                }
            });
        }

        public static string SHA1(string str)
        {
            byte[] byteValue = Encoding.UTF8.GetBytes(str);
            HashAlgorithm sha1 = new SHA1CryptoServiceProvider();
            byteValue = sha1.ComputeHash(byteValue);
            StringBuilder result = new StringBuilder();
            foreach (byte _byte in byteValue)
            {
                result.AppendFormat("{0:x2}", _byte);
            }
            return result.ToString();
        }

        public static string CreateMD5(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        static int rep = 0;

        public static string GenerateCheckCode(int codeCount)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }

        public static string GenerateCheckCodeNum(int codeCount)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                int num = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
            }
            return str;
        }
    }
}
