using CHEER.Common.DBService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CHEER.PresentationLayer.CommonUse
{
    public class Common
    {
        public static DateTime UnixTimeStampToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        public static int DateTimeToUnixTimestamp(DateTime dateTime)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(dateTime - startTime).TotalSeconds;
        }

        public static long GuidToLongID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
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
        //验证手机号码
        public static bool IsHandset(string str_handset)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"^(13[0-9]|14[5|7]|15[0|1|2|3|5|6|7|8|9]|18[0-9]|17[0-9])\d{8}$");
        }
        //验证身份证号
        public static bool IsIDcard(string str_idcard)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_idcard, @"(^\d{18}$)|(^\d{15}$)");
        }

        /// <summary>
        ///根据身份证号码获取出生日期
        /// </summary>
        /// <param name="str_idcard"></param>
        /// <returns></returns>
        public static string getBhdayByIDcard(string str_idcard)
        {
            string birthday = "";

            if (str_idcard.Length == 15)
            {
                birthday = "19" + str_idcard.Substring(6, 6);
            }
            else if (str_idcard.Length == 18)
            {
                birthday = str_idcard.Substring(6, 8);
            }

            birthday = birthday.Insert(6, "-").Insert(4,"-");


            return birthday;
        }

        public static int getAgeByIDcard(string str_idcard)
        {
            int age = 0;
            string birthday = getBhdayByIDcard(str_idcard);
            try
            {
                if (birthday != "")
                {
                    age = (DateTime.Now - DateTime.Parse(birthday)).Days / 360;
                }
            }
            catch (Exception)
            {

                age=0;
            }
            
            return age;
        }

    }
}