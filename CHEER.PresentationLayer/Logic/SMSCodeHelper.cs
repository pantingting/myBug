using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHEER.PresentationLayer.Logic
{
    public class SMSCodeHelper
    {
        /// <summary>
        /// 检测验证码
        /// </summary>
        public static void checkSMSCode(string serverSMSCode, string userSMSCode,
            HelperDelegate.EmptyDelegate emptyDelegate,
            HelperDelegate.EmptyDelegate notSameDelegate,
            HelperDelegate.EmptyDelegate sameDelegate)
        {
            if (string.IsNullOrEmpty(serverSMSCode))
            {
                emptyDelegate();
            }
            else
            {
                if (userSMSCode != serverSMSCode)
                {
                    notSameDelegate();
                }
                else
                {
                    sameDelegate();
                }
            }
        }
    }
}