using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;


namespace CHEER.PresentationLayer.Common
{
    public class CommonMethod
    {
        /// <summary>
        /// SSDB配置
        /// </summary>
        public static string SSDBConfig
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["Redis"];
            }
        }

        /// <summary>
        /// Guid转成Long类型
        /// </summary>
        public static long GuidToLongID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="sigin"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static bool Verify(string sigin, string timestamp)
        {
            bool isSigin = false;
            var appid = ConfigurationManager.AppSettings["appid"];
            var appsecret = ConfigurationManager.AppSettings["appsecret"];
            var prestr = "appid=" + appid + "&appsecret=" + appsecret + "&timestamp=" + timestamp;
            if (sigin == Sign(prestr))
            {
                isSigin = true;
            }
            Log.Instance.Error(typeof(CommonMethod), Sign(prestr));
            return isSigin;
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="prestr"></param>
        /// <returns></returns>
        public static string Sign(string prestr)
        {
            StringBuilder sb = new StringBuilder(32);

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding("utf-8").GetBytes(prestr));
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }

            return sb.ToString();
        }

        static int rep = 0;

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

        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
        public const int LOG_LEVENL = 3;

        /// <summary>
        /// 日志命名类型
        /// </summary>
        public enum LogNameType
        {
            /// <summary>
            /// 微信js支付相关
            /// </summary>
            WxJsPay,
            /// <summary>
            /// 微信app支付相关
            /// </summary>
            WxAppPay,
            /// <summary>
            /// 支付宝js支付相关
            /// </summary>
            AliJsPay,
            /// <summary>
            /// 用户支付相关
            /// </summary>
            UserPay,
            /// <summary>
            /// 用户订单相关
            /// </summary>
            UserOrder,
            /// <summary>
            /// 用户信息相关
            /// </summary>
            UserInfo,
            /// <summary>
            /// 微信扫码支付相关
            /// </summary>
            NativePay
        }
    }
}