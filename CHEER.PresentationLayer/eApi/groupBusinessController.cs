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
    public class groupBusinessController : ApiController
    {
        #region 集团业务分类
        [Route("getGroupBusinessCategory")]
        [HttpGet]
        public Dictionary<string, object> getGroupBusinessCategory(string isEnglish)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "";
                if (isEnglish == "0")
                {
                    sql = $"SELECT id,`name`,imageUrl,description,createTime from zd_group_business where is_deleted=0 ORDER BY sortNo";
                }
                else
                {
                    sql = $"SELECT id,imageUrl,en_name,en_description,createTime from zd_group_business where is_deleted=0 ORDER BY sortNo";
                }
                var dt = _broker.ExecuteSQLForDst(sql).Tables[0];
                response["data"] = dt;
                response["success"] = true;
            }, ex => {
                response["message"] = ex.ToString();
            });
            return response;
        }
        #endregion

        #region 集团业务
        [Route("getGroupBusiness")]
        [HttpGet]
        public Dictionary<string, object> getGroupBusiness(string isEnglish,string categoryId,string pageIndex)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "";
                var limit= $" limit {int.Parse(pageIndex) * 20}, 20";
                if (isEnglish == "0")
                {
                    sql = $"SELECT title,imageUrl,description,content,createTime from zd_group_business_company where group_business_id={categoryId} and is_deleted=0"
                    ;
                }
                else
                {
                    sql = $"SELECT en_title,imageUrl,en_description,en_content,createTime from zd_group_business_company where group_business_id={categoryId} and is_deleted=0"
                    ;
                }
                var dt = _broker.ExecuteSQLForDst(sql).Tables[0];
                response["rowCount"] = dt.Rows.Count;
                response["data"] = _broker.ExecuteSQLForDst(sql+ limit).Tables[0];
                response["success"] = true;
            }, ex => {
                response["message"] = ex.ToString();
            });
            return response;
        }
        #endregion
    }
}