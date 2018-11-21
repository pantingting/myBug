using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CHEER.PresentationLayer.CommonUse;

namespace CHEER.PresentationLayer.eApi
{
    [RoutePrefix("eApi")]
    public class IndexController : ApiController
    {
        #region 首页
        [Route("getIndex")]
        [HttpGet]
        public Dictionary<string, object> getIndex()
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var dt1 = _broker.ExecuteSQLForDst("SELECT * from zd_index").Tables[0];
                var dt2 = _broker.ExecuteSQLForDst("SELECT * from zd_news_center where is_deleted=0 ORDER BY isShowIndex desc,createTime desc limit 1").Tables[0];
                var dt3 = _broker.ExecuteSQLForDst("SELECT overCardIndexDesc,en_overCardIndexDesc,overCardIndexImage from zd_social_responsibility").Tables[0];
                response["data"] = new {
                    indexCarousel=dt1,
                    groupKeyNews=dt2,
                    overCard=dt3
                };
                response["success"] = true;
            }, ex => {
                response["message"] = ex.ToString();
            });
            return response;
        }
        #endregion

    }
}