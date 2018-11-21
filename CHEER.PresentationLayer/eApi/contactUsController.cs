using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CHEER.Common;
using CHEER.PresentationLayer.CommonUse;

namespace CHEER.PresentationLayer.eApi
{
    [RoutePrefix("eApi")]
    public class contactUsController : ApiController
    {
        #region 廉政举报
        [Route("addReport")]
        [HttpPost]
        public Dictionary<string, object> addReport(Dictionary<string,object> body)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var name = body["name"]?.ToString().DBReplace();
                var phone = body["phone"]?.ToString().DBReplace();
                var email = body["email"]?.ToString().DBReplace();
                var content = body["content"]?.ToString().DBReplace();
                _broker.ExecuteNonQuery($"insert into zd_report (name,phone,email,content,createTime) values('{name}','{phone}','{email}','{content}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')");
                response["success"] = true;
            }, ex => {
                response["message"] = ex.ToString();
            });
            return response;
        }
        #endregion

        #region 投诉建议
        [Route("addAdvice")]
        [HttpPost]
        public Dictionary<string, object> addAdvice(Dictionary<string, object> body)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var name = body["name"]?.ToString().DBReplace();
                var phone = body["phone"]?.ToString().DBReplace();
                var email = body["email"]?.ToString().DBReplace();
                var content = body["content"]?.ToString().DBReplace();
                var place = body["place"]?.ToString().DBReplace();
                _broker.ExecuteNonQuery($"insert into zd_advice (place,name,phone,email,content,createTime) values('{place}','{name}','{phone}','{email}','{content}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')");
                response["success"] = true;
            }, ex => {
                response["message"] = ex.ToString();
            });
            return response;
        }
        #endregion
    }
}