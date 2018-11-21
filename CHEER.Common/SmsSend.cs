using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHEER.Common
{
    public class SmsSend
    {
        public static string sendOne(string phone, string[] contact, string tempNo)
        {
            CCPRestSDK smsSDK = new CCPRestSDK();
            smsSDK.init("app.cloopen.com", "8883");
            smsSDK.setAccount("8a216da85e7e4bbd015e80437bc90130", "f03de22366ce4328b68bc60c99c2562e");
            smsSDK.setAppId("8a216da85e7e4bbd015e80437c070134");
            Dictionary<string, object> dr = smsSDK.SendTemplateSMS(phone, tempNo, contact);
            return getDictionaryData(dr);
        }

        private static string getDictionaryData(Dictionary<string, object> data)
        {
            string ret = null;
            foreach (KeyValuePair<string, object> item in data)
            {
                if (item.Value != null && item.Value.GetType() == typeof(Dictionary<string, object>))
                {
                    ret += item.Key.ToString() + "={";
                    ret += getDictionaryData((Dictionary<string, object>)item.Value);
                    ret += "};";
                }
                else
                {
                    ret += item.Key.ToString() + "=" + (item.Value == null ? "null" : item.Value.ToString()) + ";";
                }
            }
            return ret;
        }
    }
}
