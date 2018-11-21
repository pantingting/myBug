
using CHEER.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CHEER.PresentationLayer
{
    public partial class syncDevUpdownLine : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CHEER.Common.Log2.Instance.Info(GetType(), "设备上下线记录同步开始");
                Common.Delegate.ExecuteSQL(async _broker =>
                {
                    var boundDevDt = _broker.ExecuteDataTable($@"SELECT openId,d.did from c_user u
                                                                 LEFT JOIN c_device d on d.id=u.deviceId 
                                                                 where IFNULL(deviceId,0)<>0 and d.is_deleted=0");
                    if (boundDevDt.Rows.Count > 0)
                    {
                        for (int i = 0; i < boundDevDt.Rows.Count; i++)
                        {
                            var did = boundDevDt.Rows[i]["did"]?.ToString().Trim();
                            var openId = boundDevDt.Rows[i]["openId"]?.ToString().DBReplace();
                            var tokenData = GetUserToken(openId)["data"].ToString();
                            Log2.Instance.Info(GetType(), "" + tokenData);
                            JObject objData = (JObject)JsonConvert.DeserializeObject(tokenData);
                            var token = objData["token"].ToString();
                            using (var httpClient = new HttpClient())
                            {
                                var start_time = "";
                                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                                start_time = (DateTime.Now.AddMinutes(-15) - startTime).TotalSeconds.ToString();
                                var end_time = (DateTime.Now.AddMinutes(-1) - startTime).TotalSeconds.ToString();
                                start_time = start_time.Substring(0, 10);
                                end_time = end_time.Substring(0, 10);
                                string url = $@"https://api.gizwits.com/app/devices/{did}/raw_data?type=online&start_time={start_time}&end_time={end_time}&skip=0&limit=1000&sort=desc";
                                using (var request = new HttpRequestMessage(new HttpMethod("GET"), url))
                                {
                                    request.Headers.TryAddWithoutValidation("Accept", "application/json");
                                    request.Headers.TryAddWithoutValidation("X-Gizwits-Application-Id", "852af2adf0f44bf595ab1085875dc259");
                                    request.Headers.TryAddWithoutValidation("X-Gizwits-User-token", token);
                                    var response = await httpClient.SendAsync(request);
                                    var responseStr = await response.Content.ReadAsStringAsync();
                                    var obj = JsonConvert.DeserializeObject<JObject>(responseStr.Replace("\\", "").TrimStart('"').TrimEnd('"'));
                                    CHEER.Common.Log2.Instance.Info(GetType(), obj.ToString());
                                    for (int j = 0; j < obj["objects"].Count(); j++)
                                    {
                                        var type = obj["objects"][j]["type"].ToString();
                                        var timestamp = obj["objects"][j]["timestamp"].ToString();
                                        _broker.ExecuteNonQuery($@"INSERT into c_onoff_log(did,openid,datetime,description) VALUES('{did}','{openId}','{timestamp}','{type}')");
                                    }
                                }
                            }
                        }
                    }
                    CHEER.Common.Log2.Instance.Info(GetType(), "设备上下线记录同步完成");
                }, ex =>
                {
                    CHEER.Common.Log2.Instance.Error(GetType(), ex.Message);
                });
            }
        }

        public static Dictionary<string, object> GetUserToken(string openId)
        {
            var result = new Dictionary<string, object>
            {
                ["success"] = false
            };
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Accept", "application/json");
                client.Headers.Add("X-Gizwits-Application-Id", "852af2adf0f44bf595ab1085875dc259");

                string url = "https://api.gizwits.com/app/users";
                var body = new
                {
                    phone_id = openId
                };
                var response = client.UploadString(url, JsonConvert.SerializeObject(body));
                var obj = JsonConvert.DeserializeObject<JObject>(response);
                result["success"] = true;
                result["data"] = obj;
            }
            return result;
        }
    }
}