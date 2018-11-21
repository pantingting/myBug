using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using System.Configuration;
using System.Text;
using CHEER.Common;
using System.Web.Http;
//using System.Web.Mvc;
using WxPayAPI;
using System.Collections.Specialized;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Script.Serialization;
using System.Collections;
using System.Net.Http;
using System.Security.Cryptography;

namespace CHEER.PresentationLayer.Controllers
{
    [RoutePrefix("Controllers/user")]
    public class WeiXinController : ApiController

    {
        // GET: WeiXin
        [Route("getWXUserInfoRedirect")]
        [HttpPost]
        public object getWXUserInfoRedirect(string url)
        {
            var response = new Dictionary<string, object>();
            response["success"] = true;
            response["redirect_uri"] = $@"{ "https" }://open.weixin.qq.com/connect/oauth2/authorize?appid={  WeiXin.APPID }&redirect_uri={ url }&response_type=code&scope=snsapi_userinfo#wechat_redirect";
            return response;
        }
        [Route("getWXUserInfo")]
        [HttpPost]
        public object getWXUserInfoWithCode(string code)
        {
            var response = new Dictionary<string, object>();
            response["success"] = false;
            Common.Delegate.ExecuteStatement(() =>
            {
                using (var client = new WebClient())
                {
                    var access_token = "";
                    var responseJSON = GetAccessToken(client);
                    if ((responseJSON["errcode"] ?? "").ToString() != "")
                    {
                        response["message"] = responseJSON["errmsg"].ToString();
                        return;
                    }
                    else
                    {
                        access_token = responseJSON["access_token"].ToString();
                    }
                    var openId = GetWXOpenIdWidthCode(code);
                    var userInfo = GetWxUserInfo(openId, access_token);
                    if (userInfo != null)
                    {
                        BindUser(openId, userInfo);
                        response["data"] = userInfo;
                        response["success"] = true;
                    }
                }
            }, ex =>
            {
                response["message"] = ex.Message;
            });
            return response;
        }
        private Dictionary<string, object> GetAccessToken(WebClient client)
        {
            var response = new Dictionary<string, object>() { ["success"] = false };

            var access_token = "";

            CHEER.Common.Business.ExecuteRedis((redis, prefix) =>
            {
                //从redis取数据
                access_token = redis.StringGet(prefix + "_TOKEN").ToString();
                if ((access_token ?? "").ToString() == "")
                {
                    var responseString = client.DownloadString(
                   $"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={WeiXin.APPID}&secret={WeiXin.APPSECRET}");
                    var values = JsonConvert.DeserializeObject(responseString) as JObject;
                    if ((values["errcode"] ?? "").ToString() == "")
                    {
                        access_token = values["access_token"].ToString();
                        response["success"] = true;
                        response["access_token"] = access_token;
                        //将数据保存到redis
                        redis.StringSet(prefix + "_TOKEN", access_token, new TimeSpan(1, 30, 0));
                    }
                    Log2.Instance.Info(GetType(), "获取GetAccessToken()" + responseString);
                }
                else
                {
                    response["success"] = true;
                    response["access_token"] = access_token;
                }
            }, e =>
             {
                 response["success"] = false;
                 response["message"] = e.Message;
                 response["data"] = null;
             });
            return response;
        }
        public static string GetWXOpenIdWidthCode(string code)
        {
            string response = null;
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                CHEER.Common.Delegate.ExecuteStatement(() =>
                {
                    var accessTokenResult = client.DownloadString(
                        @"https://api.weixin.qq.com/sns/oauth2/access_token?appid=" +
                         WeiXin.APPID + "&secret=" +
                         WeiXin.APPSECRET + "&code=" +
                        code + "&grant_type=authorization_code");

                    var _accessTokenResult = JsonConvert.DeserializeObject(accessTokenResult) as JObject;

                    if (_accessTokenResult["errcode"] != null && _accessTokenResult["errcode"].ToString() != "")
                    {
                        Log.Instance.Error(typeof(WeiXin), _accessTokenResult["errmsg"].ToString());
                    }
                    else
                    {
                        response = _accessTokenResult["openid"].ToString();
                    }

                }, ex =>
                {
                    Log.Instance.Error(typeof(WeiXin), "获取微信用户信息出错", ex);
                });
            }

            return response;
        }
        public Dictionary<string, object> DeviceAuth(string mac)
        {
            var getaccesstoken = new JObject();
            var result = new Dictionary<string, object> { ["success"] = false };
            Dictionary<string, object> getaccess = new Dictionary<string, object>();
            var access_token = "";
            using (var client = new WebClient())
            {
                #region 获取token
                client.Encoding = Encoding.UTF8;
                var responseToken = client.DownloadString(
                  $"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={WeiXin.APPID}&secret={WeiXin.APPSECRET}");
                getaccesstoken = JsonConvert.DeserializeObject(responseToken) as JObject;
                access_token = getaccesstoken["access_token"].ToString();
                #endregion

                #region 反序列化POST数据
                List<Dictionary<string, object>> lists = new List<Dictionary<string, object>>();
                Dictionary<string, object> list = new Dictionary<string, object>();
                list.Add("id", mac);
                list.Add("mac", mac);
                list.Add("connect_protocol", "3");
                list.Add("auth_key", "");
                list.Add("close_strategy", "1");
                list.Add("conn_strategy", "1");
                list.Add("crypt_method", "0");
                list.Add("auth_ver", "0");
                list.Add("manu_mac_pos", "-1");
                list.Add("ser_mac_pos", "-2");
                list.Add("ble_simple_protocol", "0");
                lists.Add(list);
                Dictionary<string, object> post = new Dictionary<string, object>();
                post.Add("device_num", "1");
                post.Add("device_list", lists);
                post.Add("op_type", "0");
                post.Add("product_id", "47201");
                var values = JsonConvert.SerializeObject(post);
                #endregion

                #region 提交数据
                var responseRes = client.UploadString($"https://api.weixin.qq.com/device/authorize_device?access_token={access_token}", values);
                 ResData response  = JsonConvert.DeserializeObject<ResData>(responseRes);
                #endregion




                #region 数据判断
                if (response.resp[0].errmsg.ToString().Equals("ok"))
                {
                    result["success"] = true;
                    result["message"] = "授权成功";
                    result["data"] = null;
                }
                else
                {
                    if (response.resp[0].errmsg.ToString().Equals("already exist") )
                    {
                        result["success"] = false;
                        result["message"] = "设备已经授权过";
                        result["data"] = null;
                    }
                    else if (response.resp[0].errmsg.ToString().Equals("mac invalid"))
                    {
                        result["success"] = false;
                        result["message"] = "设备MAC不存在";
                        result["data"] = null;
                    }
                    else
                    {
                        result["success"] = false;
                        result["message"] = response.resp[0].errmsg.ToString();
                        result["data"] = null;
                    }
                }
                #endregion
            }

            return result;
        }

        /**获取jsapi_ticket
         * **/
        public Dictionary<string, object> Getjsapi_ticket()
        {
            var result = new Dictionary<string, object> { ["success"] = false };
            var ticket = "";
            CHEER.Common.Business.ExecuteRedis((redis,prefix)=>
            {
                ticket = redis.StringGet(prefix + "ticket").ToString();
                if ((ticket ?? "").ToString() == "")
                {
                    using (var client = new WebClient())
                    {
                        var access_token = "";
                        var getaccess = GetAccessToken(client);

                        if (bool.Parse(getaccess["success"].ToString()) == true)
                        {
                            access_token = getaccess["access_token"].ToString();
                        }
                        var responseString = client.DownloadString($"https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={access_token}&type=jsapi");
                        var values = JsonConvert.DeserializeObject(responseString) as JObject;
                        if (values["errcode"].ToString() == "0")
                        {
                            redis.StringSet(prefix + "ticket", values["ticket"].ToString(), new TimeSpan(1, 30, 0));
                            result["ticket"] = values["ticket"].ToString();
                            result["success"] = true;
                        }
                        Log2.Instance.Info(GetType(), "获取Getjsapi_ticket()" + responseString);
                    }
                }
                else
                {
                    result["success"] = true;
                    result["ticket"] = ticket;
                }
            },ex=>
            {
                result["success"] = false;
                result["message"] = ex.Message;
            });
            return result;
        }

        /**生成加密的signature
         * **/
         [HttpPost]
         [Route("getSignature")]
        public Dictionary<string, object> GetSignature([FromBody] Dictionary<string, object> dictionary)
        {
            var result = new Dictionary<string, object> { ["success"] = false };
            var nonceStr = getStr();
            var jsapi_ticket = "";
            var timestamp = GetTimeStamp();
            var url = dictionary["url"];
            var respones = Getjsapi_ticket();
            if (bool.Parse(respones["success"].ToString()) == true)
            {
                jsapi_ticket = respones["ticket"].ToString();
            }
            else
            {
                result["success"] = false;
                result["message"] = "ticket获取失败";
                return result;
            }
            var stringurl = "jsapi_ticket=" + jsapi_ticket + "&noncestr=" + nonceStr + "&timestamp=" + timestamp + "&url=" + url;
            var signature = SHA1(stringurl);
            result["success"] = true;
            result["appId"] = Logic.Weixin.APPID;
            result["nonceStr"] = nonceStr;
            result["timestamp"] = timestamp;
            result["signature"] = signature;
            return result;
        }

        public class UserInfo
        {
            public string id { get; set; }
            public string nickName { get; set; }
            public string loginName { get; set; }
            public string password { get; set; }
            public string address { get; set; }
            public string headImage { get; set; }
            public string registDate { get; set; }
            public string deviceId { get; set; }
        }
        public static UserInfo GetWxUserInfo(string openId, string accessToken)
        {
            UserInfo response = null;
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                CHEER.Common.Delegate.ExecuteStatement(() =>
                {
                    var result = client.DownloadString(
                        @"https://api.weixin.qq.com/cgi-bin/user/info?access_token=" +
                         accessToken + "&openid=" +
                        openId + "&lang=zh_CN");
                    var userInfo = JsonConvert.DeserializeObject(result) as JObject;
                    if (userInfo["errcode"] != null && userInfo["errcode"].ToString() != "")
                    {
                        Log.Instance.Error(typeof(WeiXin), userInfo["errmsg"].ToString());
                    }
                    else
                    {
                        if (userInfo["subscribe"].ToString() == "1")
                        {
                            response = new UserInfo()
                            {
                                nickName = userInfo["nickname"].ToString(),
                                address = userInfo["country"].ToString() + userInfo["province"].ToString() + userInfo["city"].ToString(),
                                headImage = userInfo["headimgurl"].ToString(),

                            };
                        }
                        else
                        {
                        }

                    }

                }, ex =>
                {
                    Log.Instance.Error(typeof(WeiXin), "获取微信用户信息出错", ex);
                });
            }

            return response;
        }
        public static string BindUser(string openid, UserInfo userInfo)
        {
            var result = "";
            CHEER.Common.Delegate.ExecuteSQL(_broker =>
            {
                var userTable = _broker.ExecuteDataTable($"SELECT id FROM c_user WHERE loginName = '{userInfo.loginName}'");

                if (userTable.Rows.Count > 0)
                {
                    _broker.ExecuteNonQuery(
                        $@"SET NAMES 'utf8mb4';
                                   UPDATE c_user 
                                   SET 
                                        nickName = '{ userInfo.nickName.ToString().DBReplace() }', 
                                        headerImg = '{ userInfo.headImage }'
                                   WHERE 
                                        id = '{ userTable.Rows[0]["id"].ToString() }'");
                    result = userTable.Rows[0]["id"].ToString();

                }
                else
                {
                    var dt = _broker.ExecuteDataTable(
                $@"SET NAMES 'utf8mb4';
                        INSERT INTO c_user (nickName,loginName,address,headImage,registDate) 
                        VALUES ('{userInfo.nickName}', '{userInfo.loginName}', '{userInfo.address}', '{userInfo.headImage}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');
                        select last_insert_id() id;");
                    var id = dt.Rows[0]["id"].ToString();
                    result = id;
                }
            }, ex =>
            {
                Log.Instance.Error(typeof(WeiXin), "绑定用户信息时出错", ex);
            });

            return result;
        }

        public class BaseInfo
        {
            public string device_type { get; set; }
            public string  device_id { get; set; }
        }
        public class Resps
        {
            public BaseInfo base_info;
            public int errcode { get; set; }
            public string errmsg { get; set; }

        }

        public class ResData
        {
            public List<Resps> resp { get; set; }
        }


        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="b"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public string getStr()
        {

            string str = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder SB = new StringBuilder();
            Random rd = new Random();
            for (int i = 0; i < 16; i++)
            {
                SB.Append(str.Substring(rd.Next(0, str.Length), 1));
            }
            return SB.ToString();

        }

        /// <summary>  
        /// SHA1 加密，返回大写字符串  
        /// </summary>  
        /// <param name="content">需要加密字符串</param>  
        /// <returns>返回40位UTF8 大写</returns>  
        public static string SHA1(string content)
        {
            return SHA1(content, Encoding.UTF8);
        }
        /// <summary>  
        /// SHA1 加密，返回大写字符串  
        /// </summary>  
        /// <param name="content">需要加密字符串</param>  
        /// <param name="encode">指定加密编码</param>  
        /// <returns>返回40位大写字符串</returns>  
        public static string SHA1(string content, Encoding encode)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] bytes_in = encode.GetBytes(content);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();
                string result = BitConverter.ToString(bytes_out);
                result = result.Replace("-", "");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("SHA1加密出错：" + ex.Message);
            }
        }





    }
}