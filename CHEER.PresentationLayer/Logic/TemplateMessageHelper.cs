
using CHEER.PresentationLayer.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace CHEER.PresentationLayer.Logic
{
    public class TemplateMessageHelper
    {

    }
    /// <summary>
    /// 设置微信模板id
    /// </summary>
    public class TemplateID
    {
        /// <summary>
        /// 视频课程开课通知
        /// </summary>
        public const string COURSEBEGINNOTICE = "S87kHLwOUJPSLaEKAH74eaLyZqR_4VNRzpxRdh_iZQo";// "S1zEIVKXFKoZU3aWpWbozFbbd9B-aMDBYi54yTBajpg";

        /// <summary>
        /// 采纳最佳答案提醒
        /// </summary>
        public const string ADOPTBESTANSWER = "zA6TsYVfEMOgM0WBuVg9U7eD6xRu8jDnXQVtxrTPX3c";// "mbzLYiuX3-lh0jEKpMZJ1-IBhp-wj1s2nCSR4Wyv-lE";

        /// <summary>
        /// 积分变动提醒
        /// </summary>
        public const string INTEGRALCHANGE = "AWYveTuf9fUi6DxDb70RcWQ2rDVlnGaiE7DAGc0hkXw";// "ziPO1quNtiQag8IljA-T0H-0PbFitpDmZSMqIPOXPYw";

        /// <summary>
        /// 悬赏被采纳模版消息
        /// </summary>
        public const string ADOPTED = "yCqW84nulJ7HSR7vpR5usPKRqE00lNICKYRSvtCift4";

    }






    public class Weixin
    {

        public class Config
        {
            public string appId { get; set; }
            public string nonceStr { get; set; }
            public string timestamp { get; set; }
            public string signature { get; set; }
        }

        public static string APPID
        {
            get
            {
                return ConfigurationManager.AppSettings["Weixin-APPID"];
            }
        }

        public static string APP_SECRET
        {
            get
            {
                return ConfigurationManager.AppSettings["Weixin-APP_SECRET"];
            }
        }

        public static string GetAccessToken(string code)
        {
            var access_token = "";

            Common.Delegate.ExecuteRedis(redis =>
            {
                access_token = redis.Get(APPID + "access_token");
            });
            if ((access_token ?? "").ToString() == "")
            {
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    Common.Delegate.ExecuteStatement(() =>
                    {
                        //var responseString = client.DownloadString(
                        //$"{"https"}://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={ APPID }&secret={ APP_SECRET }");
                        var responseString = client.DownloadString(
                        $"{"https"}://api.weixin.qq.com/sns/oauth2/access_token?appid={APPID}&secret={APP_SECRET}&code={code}&grant_type=authorization_code");
                        var response = JsonConvert.DeserializeObject(responseString) as JObject;
                        if ((response["errcode"] ?? "").ToString() == "")
                        {
                            access_token = response["access_token"].ToString();
                            Common.Delegate.ExecuteRedis(redis =>
                            {
                                redis.SetEX(Logic.Weixin.APPID + "access_token", TimeSpan.FromSeconds(int.Parse(response["expires_in"].ToString()) - 60), access_token);
                            });
                        }
                        else
                        {
                            Log.Info("TemplateMessageHelper", responseString, CommonMethod.LogNameType.WxJsPay.ToString());
                        }
                    }, ex =>
                    {
                        Log.Info("TemplateMessageHelper", "获取微信access_token出错", CommonMethod.LogNameType.WxJsPay.ToString());
                    });
                }
            }

            return access_token;
        }

        public static string GetAccessToken(string APPID, string APP_SECRET)
        {
            var access_token = "";

            Common.Delegate.ExecuteRedis(redis =>
            {
                access_token = redis.Get(APPID + "access_token");
            });
            if ((access_token ?? "").ToString() == "")
            {
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    Common.Delegate.ExecuteStatement(() =>
                    {
                        var responseString = client.DownloadString(
                        $"{"https"}://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={ APPID }&secret={ APP_SECRET }");
                        var response = JsonConvert.DeserializeObject(responseString) as JObject;
                        if ((response["errcode"] ?? "").ToString() == "")
                        {
                            access_token = response["access_token"].ToString();
                            Common.Delegate.ExecuteRedis(redis =>
                            {
                                redis.SetEX(APPID + "access_token", TimeSpan.FromSeconds(int.Parse(response["expires_in"].ToString()) - 60), access_token);
                            });
                        }
                        else
                        {
                            Log.Info("TemplateMessageHelper", responseString, CommonMethod.LogNameType.WxJsPay.ToString());
                        }
                    }, ex =>
                    {
                        Log.Info("TemplateMessageHelper", "获取微信access_token出错", CommonMethod.LogNameType.WxJsPay.ToString());
                    });
                }
            }

            return access_token;
        }


        public static bool SendTemplate(string templateId, string openId, string url, SortedDictionary<string, string> extra)
        {
            bool bl = false;
            var access_token = "";
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                Common.Delegate.ExecuteStatement(() =>
                {
                    var responseString = client.DownloadString(
                    $"{"https"}://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={ APPID }&secret={ APP_SECRET }");
                    var response = JsonConvert.DeserializeObject(responseString) as JObject;
                    if ((response["errcode"] ?? "").ToString() == "")
                    {
                        access_token = response["access_token"].ToString();
                    }
                    else
                    {
                        Log.Info("TemplateMessageHelper", responseString, CommonMethod.LogNameType.WxJsPay.ToString());
                    }
                }, ex =>
                {
                    Log.Info("TemplateMessageHelper", ex.Message, CommonMethod.LogNameType.WxJsPay.ToString());
                });
            }

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;

                client.Headers["Content-Type"] = "application/json";

                var responseString = client.UploadString($@"{ "https" }://api.weixin.qq.com/cgi-bin/message/template/send?access_token={ access_token }", getTemplateById(templateId, openId, url, extra));

                Log.Info("TemplateMessageHelper", $"发送给{ openId }的模版消息, 发送结果{ responseString }", CommonMethod.LogNameType.WxJsPay.ToString());


                if (JsonConvert.DeserializeObject<JObject>(responseString)["errcode"].ToString() == "0")
                {
                    bl = true;
                }
            }
            return bl;
        }


        private static string getTemplateById(string templateId, string openId, string url, SortedDictionary<string, string> extra)
        {
            var data = new SortedDictionary<string, dynamic>();
            extra.AsEnumerable().ToList().ForEach(keyValue =>
            {
                data.Add(keyValue.Key, new
                {
                    value = keyValue.Value,
                    color = "#173177"
                });
            });
            return JsonConvert.SerializeObject(new
            {
                touser = openId,
                template_id = templateId,
                url = url,
                data = data
            });
        }
    }
}