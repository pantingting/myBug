using CHEER.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;


namespace CHEER.PresentationLayer.Common
{
    public class WeiXin
    {
        public class UserInfo
        {
            public string openId { get; set; }
            public string nickName { get; set; }
            public string headImage { get; set; }
            public string address { get; set; }
          
        }

        public class Config
        {
            public string appId { get; set; }
            public string nonceStr { get; set; }
            public string timestamp { get; set; }
            public string signature { get; set; }
        }


        public static string GetAccessToken(string code)
        {
            var access_token = "";

            Delegate.ExecuteRedis(redis =>
            {
                access_token = redis.Get(Logic.Weixin.APPID + "access_token");
            });
            if ((access_token ?? "").ToString() == "")
            {
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    Delegate.ExecuteStatement(() =>
                    {
                        //var responseString = client.DownloadString(
                        //$"{"https"}://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={ APPID }&secret={ APP_SECRET }");
                        var responseString = client.DownloadString(
                        $"{"https"}://api.weixin.qq.com/sns/oauth2/access_token?appid={ Logic.Weixin.APPID}&secret={ Logic.Weixin.APP_SECRET}&code={code}&grant_type=authorization_code");
                        var response = JsonConvert.DeserializeObject(responseString) as JObject;
                        if ((response["errcode"] ?? "").ToString() == "")
                        {
                            access_token = response["access_token"].ToString();
                            Delegate.ExecuteRedis(redis =>
                            {
                                redis.SetEX(Logic.Weixin.APPID + "access_token", TimeSpan.FromSeconds(int.Parse(response["expires_in"].ToString()) - 60), access_token);
                            });
                        }
                        else
                        {
                            Log.Instance.Error(typeof(WeiXin), responseString);
                        }
                    }, ex =>
                    {
                        Log.Instance.Error(typeof(WeiXin), "获取微信access_token出错", ex);
                    });
                }
            }

            return access_token;
        }

        public static string GetOpenId(string code)
        {
            var openId = "";

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                Delegate.ExecuteStatement(() =>
                {
                    var responseString = client.DownloadString(
                        $"{"https"}://api.weixin.qq.com/sns/oauth2/access_token?appid={  Logic.Weixin.APPID }&secret={  Logic.Weixin.APP_SECRET }&code={ code }&grant_type=authorization_code");
                    var response = JsonConvert.DeserializeObject(responseString) as JObject;
                    if ((response["errcode"] ?? "").ToString() == "")
                    {
                        openId = response["openid"].ToString();
                    }
                    else
                    {
                        Log.Instance.Error(typeof(WeiXin), responseString);
                    }
                }, ex =>
                {
                    Log.Instance.Error(typeof(WeiXin), "获取微信openid出错", ex);
                });
            }

            return openId;

        }

        //public static UserInfo GetUserInfoWithOpenid(string code,string openid)
        //{
        //    UserInfo response = null;

        //    var access_token = GetAccessToken(code);
        //    using (var client = new WebClient())
        //    {
        //        client.Encoding = Encoding.UTF8;

        //        Delegate.ExecuteStatement(() =>
        //        {
        //            //var responseString = client.DownloadString(
        //            //$"{"https"}://api.weixin.qq.com/cgi-bin/user/info?access_token={access_token}&openid={openid}&lang=zh_CN");
        //            var responseString = client.DownloadString(
        //            $"{"https"}://api.weixin.qq.com/sns/userinfo?access_token={access_token}&openid={openid}&lang=zh_CN");
        //            var responseJSON = JsonConvert.DeserializeObject(responseString) as JObject;
        //            if ((responseJSON["errcode"] ?? "").ToString() != "")
        //            {
        //                Log.Instance.Error(typeof(WeiXin), responseString);
        //            }
        //            else
        //            {
        //                response = new UserInfo()
        //                {
        //                    openid = responseJSON["openid"],
        //                    sex = responseJSON["sex"],
        //                    nickname = responseJSON["nickname"],
        //                    headimgurl = responseJSON["headimgurl"],
        //                    subscribe = responseJSON["subscribe"],
        //                    subscribe_time = responseJSON["subscribe_time"],
        //                    unionid = responseJSON["unionid"]
        //                };
        //            }
        //        }, ex =>
        //        {
        //            Log.Instance.Error(typeof(WeiXin), "获取微信openid出错", ex);
        //        });
        //    }

        //    return response;
        //}




        public static UserInfo GetUserInfoWithOpenid(string code)
        {
            UserInfo response = null;
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                Delegate.ExecuteStatement(() =>
                {
                    var accessTokenResult = client.DownloadString(
                        @"https://api.weixin.qq.com/sns/oauth2/access_token?appid=" +
                         Logic.Weixin.APPID + "&secret=" +
                         Logic.Weixin.APP_SECRET + "&code=" +
                        code + "&grant_type=authorization_code");
                    var _accessTokenResult = JsonConvert.DeserializeObject(accessTokenResult) as JObject;

                    if (_accessTokenResult["errcode"] != null && _accessTokenResult["errcode"].ToString() != "")
                    {
                        Log.Instance.Error(typeof(WeiXin), _accessTokenResult["errmsg"].ToString());
                    }
                    else
                    {
                        var access_token = _accessTokenResult["access_token"].ToString();
                        var openId = _accessTokenResult["openid"].ToString();

                        var userInfoResult = client.DownloadString(
                            @"https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + openId + "&lang=zh_CN");

                        var _userInfoResult = JsonConvert.DeserializeObject(userInfoResult) as JObject;

                        if (_userInfoResult["errcode"] != null && _userInfoResult["errcode"].ToString() != "")
                        {
                            Log.Instance.Error(typeof(WeiXin), _userInfoResult["errmsg"].ToString());
                        }
                        else
                        {
                            response = new UserInfo()
                            {
                                openId = _userInfoResult["openid"]?.ToString(),
                                nickName = _userInfoResult["nickname"]?.ToString(),
                                headImage = _userInfoResult["headimgurl"]?.ToString(),
                                address = _userInfoResult["country"]?.ToString() + _userInfoResult["province"]?.ToString() + _userInfoResult["city"]?.ToString(),

                            };
                        }
                    }
                }, ex =>
                {
                    Log.Instance.Error(typeof(WeiXin), "获取微信用户信息出错", ex);
                });
            }

            return response;
        }



        private static string GetSignature(string jsApiTicket, string timestamp, string nonce, string url)
        {
            string[] paramArr = new string[] { "jsapi_ticket=" + jsApiTicket,
                "noncestr=" + nonce, "timestamp=" + timestamp, "url=" + url };
            var tmpStr = string.Join("&", paramArr);
            byte[] StrRes = Encoding.UTF8.GetBytes(tmpStr);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }

        public static void BindUser(string openid, UserInfo userInfo)
        {
            Delegate.ExecuteSQL(_broker =>
            {
                var userTable = _broker.ExecuteDataTable($"SELECT id FROM c_user WHERE openid = '{openid}'");

                if (userTable.Rows.Count > 0)
                {
                    _broker.ExecuteNonQuery(
                        $@"SET NAMES 'utf8mb4';
                                   UPDATE c_user 
                                   SET 
                                        nickName = '{ userInfo.nickName.ToString().DBReplace() }', 
                                        headImage='{userInfo.headImage.ToString()}',
                                        address='{userInfo.address.ToString()}'
                                   WHERE 
                                        id = '{ userTable.Rows[0]["id"].ToString() }'");
                }
                else
                {
                    var sql = $@"
                        INSERT INTO c_user (nickName,headImage,address,openId) 
                        VALUES ('{userInfo.nickName}','{userInfo.headImage}','{userInfo.address}','{userInfo.openId}')";
                    var dt = _broker.ExecuteNonQuery(sql);
                    //var id = dt.Rows[0]["id"].ToString();
                }                
            }, ex =>
            {
                Log.Instance.Error(typeof(WeiXin), "绑定用户信息时出错", ex);
            });
        }
    }
}