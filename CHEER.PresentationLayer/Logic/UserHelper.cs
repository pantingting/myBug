
using CHEER.BusinessLayer.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CHEER.PresentationLayer.Logic
{
    public static class stringExtend
    {
        /// <summary>
        /// 判断字符串是否为空或空字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }
    }

    public class UserHelper
    {
        public static User GetUser(string openId)
        {
            User user = new User();
            Common.Delegate.ExecuteSQL(_broker =>
            {
                var sql = $@"select id,nickName,head_portrait from lp_user where wxOpenId = '{openId}'";
                var dt = _broker.ExecuteDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                   // user.id = dt.Rows[0]["id"].ToString();
                   // user.nickName = dt.Rows[0]["nickName"].ToString();
                  //  user.head_portrait = dt.Rows[0]["head_portrait"].ToString();
                }
            }, ex =>
            {
            });
            return user;
        }

        /// <summary>
        /// 获取用户可用积分
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int GetAvailableIntegral(string id)
        {
            int available_integral = 0;
            Common.Delegate.ExecuteSQL(_broker =>
            {
                var sql = $@"select available_integral from zsh_user where id = '{id}'";
                var dt = _broker.ExecuteDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    available_integral = Convert.ToInt32(dt.Rows[0]["available_integral"]);
                }
            }, ex =>
            {
            });
            return available_integral;
        }

        public static string GetUserId(string openId)
        {
            string userId = string.Empty;
            Common.Delegate.ExecuteSQL(_broker => 
            {
                if (!string.IsNullOrEmpty(openId))
                {
                    var userInfo = _broker.ExecuteDataTable($"select id from c_user where openId = '{openId}'");
                    userId = userInfo.Rows.Count > 0 ? userInfo.Rows[0][0]?.ToString() : "";
                }
            }, ex => 
            {
            });

            return userId;
        }

        public static string GetDeviceId(string openId)
        {
            string deviceId = string.Empty;
            Common.Delegate.ExecuteSQL(_broker =>
            {
                if (!string.IsNullOrEmpty(openId))
                {
                    var userInfo = _broker.ExecuteDataTable($"select deviceId from c_user where openId = '{openId}'");
                    deviceId = userInfo.Rows.Count > 0 ? userInfo.Rows[0][0]?.ToString() : "";
                }
            }, ex =>
            {
            });

            return deviceId;
        }

        /// <summary>
        /// 获取用户id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //public static string GetUserId(HttpRequestBase request)
        //{
        //    string userid = "";
        //    HttpCookie cok = request.Cookies["lp_token"];
        //    if (cok != null)
        //    {
        //        string token = HttpUtility.UrlDecode(cok.Value);
        //        token = Common.Token.Decrypt(token);
        //        string[] tokenValues = token.Split('|');
        //        if (tokenValues.Length >= 3)
        //        {
        //            userid = tokenValues[1];
        //        }
        //    }
        //    return userid;
        //}
        /// <summary>
        /// 操作url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="ParamText"></param>
        /// <param name="ParamValue"></param>
        /// <param name="isForAppend"></param>
        /// <param name="appendValue"></param>
        /// <returns></returns>
        public static string BuildUrl(string url, string ParamText, string ParamValue, bool isForAppend, string appendValue)
        {
            appendValue = appendValue?.TrimEnd(',');
            string[] arr = new string[] { };
            if (appendValue != null)
            {
                arr = appendValue?.Split(',');
            }
            List<string> list = new List<string>();
            for (int i = 0; i < arr.Length; i++)
            {
                list.Add(arr[i]);
            }
            Regex reg = new Regex(string.Format("{0}=[^&]*", ParamText), RegexOptions.IgnoreCase);
            Regex reg1 = new Regex("[&]{2,}", RegexOptions.IgnoreCase);
            string _url = reg.Replace(url, "");
            //_url = reg1.Replace(_url, "");
            if (_url.IndexOf("?") == -1)
            {
                if (list.Contains(ParamValue))
                {
                    _url += string.Format("?{0}={1}", ParamText, string.IsNullOrEmpty(appendValue) ? ParamValue : appendValue);//?
                }
                else
                {
                    _url += string.Format("?{0}={1}", ParamText, string.IsNullOrEmpty(appendValue) ? ParamValue : appendValue + "," + ParamValue);//?
                }
            }
            else
            {
                if (list.Contains(ParamValue))
                {
                    _url += string.Format("&{0}={1}", ParamText, string.IsNullOrEmpty(appendValue) ? ParamValue : appendValue);//&
                }
                else
                {
                    _url += string.Format("&{0}={1}", ParamText, string.IsNullOrEmpty(appendValue) ? ParamValue : ParamValue);//&
                }
            }
            _url = reg1.Replace(_url, "&");
            _url = _url.Replace("?&", "?");
            return _url;
        }

        // 当前登录人是否被授权登录、查看设备信息
        public static int getChildIsAuthorization(string childOpenId, string mainOpenId)
        {
            // errCode = 0 授权允许登录 errCode = 1 未允许登录 errCode = 2 当前登录人不是此账号的下级用户 errCode = 3 未获取到微信用户信息 errCode = 4 一级用户删除了设备 errCode = 5 主用户不存在
            var errCode = 0;
            if (!string.IsNullOrEmpty(childOpenId) && !string.IsNullOrEmpty(mainOpenId))
            {
                Common.Delegate.ExecuteSQL(_broker =>
                {
                    var childUserId = GetUserId(childOpenId);

                    var mainUserId = GetUserId(mainOpenId);

                    if (string.IsNullOrEmpty(childUserId))
                    {
                        errCode = 3;
                        return;
                    }

                    // 查询一级用户是否删除了设备
                    var isDeleted = _broker.ExecuteDataTable($"select deviceId from c_user where openId = '{mainOpenId}'");

                    if (isDeleted.Rows.Count > 0)
                    {
                        if (string.IsNullOrEmpty(isDeleted.Rows[0][0]?.ToString()))
                        {
                            errCode = 4;
                            return;
                        }
                    } else
                    {
                        errCode = 5;
                        return;
                    }
                        

                    var check = _broker.ExecuteDataTable($"select ifnull(isAuthorization, 0) from c_user_share where shareUserId = {childUserId} and mainUserId = {mainUserId}");

                    if (check.Rows.Count > 0)
                    {
                        if (check.Rows[0][0]?.ToString() == "0") errCode = 1;
                        else errCode = 0;
                    } else
                    {
                        if (childUserId != mainUserId)
                            errCode = 2;
                    }
                }, ex => { });                
            }

            return errCode;
        }

    }
}