using CHEER.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHEER.PresentationLayer.CommonUse
{
    public class InfoSend
    {
        /// <summary>
        /// 密码重置
        /// </summary>
        public static void setPwdChange(string mobileNo, string sellerName, string rePwd)
        {
            var result = SmsSend.sendOne(mobileNo, new string[] { sellerName, rePwd }, "80246");
        }

        /// <summary>
        /// 审核结果
        /// </summary>
        public static void changeStatus(string mobileNo, string status, string remark)
        {
            var result = SmsSend.sendOne(mobileNo, new string[] { status, remark }, "80248");
        }
        /// <summary>
        /// 开除
        /// </summary>
        public static void deletePerson(string mobileNo, string remark)
        {
            var result = SmsSend.sendOne(mobileNo, new string[] { remark }, "80250");
        }
    }
}