using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHEER.PresentationLayer.Logic
{
    public class SMSCode
    {
        /// <summary>
        /// 设置验证码
        /// </summary>
        public static void setSMSCode(string mobileNo, string smsCode)
        {
            Common.Delegate.ExecuteRedis(redis => {
                //5分钟自动过期
                redis.SetEX(mobileNo, new TimeSpan(0, 5, 0), smsCode);
            });
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        public static string getSMSCode(string mobileNo)
        {
            var smsCode = string.Empty;
            CHEER.Common.Business.ExecuteRedis((redis, prefix) =>
            {
                smsCode = redis.StringGet(mobileNo);
            });
            //Common.Delegate.ExecuteRedis(redis => {
            //    smsCode = redis.Get(mobileNo);
            //});
            return smsCode;
        }
    }
}