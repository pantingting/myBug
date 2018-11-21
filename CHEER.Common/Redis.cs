using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHEER.Common
{
    public class Redis
    {
        private Redis() { InitRedis(); }
        public static readonly Redis Instance = new Redis();
        ConnectionMultiplexer redis;

        private void InitRedis()
        {
            var redisConfig = ConfigurationManager.AppSettings["Redis"];
            redis = ConnectionMultiplexer.Connect(redisConfig);
        }

        public int RedisDB
        {
            get
            {
                return 3;
            }
        }

        public ConnectionMultiplexer GetRedis()
        {
            return redis;
        }
    }
}
