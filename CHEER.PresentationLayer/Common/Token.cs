using CHEER.Platform.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CHEER.PresentationLayer.Common
{
    public class Token
    {
        public static string UserId
        {
            get
            {
                string id = string.Empty;
                Delegate.ExecuteStatement(() => id = Decrypt(HttpContext.Current.Request.Headers["AuthToken"]).Split('|')[1]);
                return id;
            }
        }

        public static bool CheckTokenExpired(string token)
        {
            var tokenExpired = true;
            Delegate.ExecuteRedis(redis =>
            {
                if (redis.Exists(token))
                {
                    tokenExpired = false;
                    redis.SetEX(token, new TimeSpan(24, 0, 0), "");
                }
            });
            return tokenExpired;
        }

        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GenerateToken(string userId)
        {
            var token = Encrypt(Guid.NewGuid().ToString()
                            + "ZYXYZ"
                            + "|"
                            + userId
                            + "|"
                            + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Delegate.ExecuteRedis(redis =>
            {
                //目的是删除已经失去意义的Token
                var oldToken = redis.Get(Encrypt(userId)) ?? "";
                if (redis.Exists(oldToken))
                {
                    redis.Del(oldToken);
                }
                redis.SetEX(token, new TimeSpan(24, 0, 0), "");
                redis.Set(Encrypt(userId), token);
                Log.Instance.Error(typeof(Token), "Encrypt(userId):"+ Encrypt(userId));
            });
            return token;
        }

        private static string Encrypt(string strToEncrypt)
        {
            return Convert.ToBase64String(Encoding.Default.GetBytes(CommonFunction.DESEnCrypt(strToEncrypt)));
        }

        public static string Decrypt(string strToDecrypt)
        {
            return CommonFunction.DESDeCrypt(Encoding.Default.GetString(Convert.FromBase64String(strToDecrypt)));
        }
    }
}