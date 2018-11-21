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
    public class socialResponsibilityController : ApiController
    {
        #region 首页
        [Route("getSocialIndex")]
        [HttpGet]
        public Dictionary<string, object> getSocialIndex(string isEnglish)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "";
                if (isEnglish == "0")
                {
                    sql = "SELECT imageUrl1,imageUrl2,imageUrl3 from zd_social_responsibility";
                }
                else
                {
                    sql = "SELECT en_imageUrl1,en_imageUrl2,en_imageUrl3 from zd_social_responsibility";
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

        #region 公益理念
        [Route("getWelfareIdea")]
        [HttpGet]
        public Dictionary<string, object> getWelfareIdea(string isEnglish)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "";
                if (isEnglish == "0")
                {
                    sql = "SELECT welfareIdea from zd_social_responsibility";
                }
                else
                {
                    sql = "SELECT en_welfareIdea from zd_social_responsibility;";
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

        #region 脱贫攻坚
        [Route("getOverCard")]
        [HttpGet]
        public Dictionary<string, object> getOverCard(string isEnglish)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "";
                if (isEnglish == "0")
                {
                    sql = "SELECT overCard from zd_social_responsibility";
                }
                else
                {
                    sql = "SELECT en_overCard from zd_social_responsibility;";
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

        #region 脱贫攻坚分类
        [Route("getOverCardCategory")]
        [HttpGet]
        public Dictionary<string, object> getOverCardCategory(string isEnglish)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "";
                if (isEnglish == "0")
                {
                    sql = "SELECT id,title,content,createTime from zd_orver_poor_category where is_deleted=0 order by sortNo";
                }
                else
                {
                    sql = "SELECT id,en_title,en_content,createTime from zd_orver_poor_category where is_deleted=0  order by sortNo";
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

        #region 脱贫攻坚项目内容
        [Route("getOverCardContent")]
        [HttpGet]
        public Dictionary<string, object> getOverCardContent(string isEnglish, string categoryId, string pageIndex)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "";
                var limit = $" limit {int.Parse(pageIndex) * 20}, 20";
                if (isEnglish == "0")
                {
                    sql = $"SELECT imageUrl,content,createTime from zd_poor_content where category_id={categoryId} and is_deleted=0 order by sortNo"
                   ;
                }
                else
                {
                    sql = $"SELECT imageUrl,en_content,createTime from zd_poor_content where category_id={categoryId} and is_deleted=0 order by sortNo"
                    ;
                }
                var dt = _broker.ExecuteSQLForDst(sql).Tables[0];
                response["rowCount"] = dt.Rows.Count;
                response["data"] = _broker.ExecuteSQLForDst(sql + limit).Tables[0];
                response["success"] = true;
            }, ex => {
                response["message"] = ex.ToString();
            });
            return response;
        }
        #endregion

        #region 慈善捐赠内容
        [Route("getCharitableCategory")]
        [HttpGet]
        public Dictionary<string, object> getCharitableCategory(string isEnglish)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "";
                if (isEnglish == "0")
                {
                    sql = "SELECT id,title,content,createTime from zd_charitable_category where is_deleted=0  order by sortNo";
                }
                else
                {
                    sql = "SELECT id,en_title,en_content,createTime from zd_charitable_category where is_deleted=0  order by sortNo";
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

        #region 慈善捐赠项目例子
        [Route("getCharitableProduct")]
        [HttpGet]
        public Dictionary<string, object> getCharitableProduct(string isEnglish, string categoryId, string pageIndex)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "";
                var limit = $" limit {int.Parse(pageIndex) * 20}, 20";
                if (isEnglish == "0")
                {
                    sql = $"SELECT imageUrl,content,createTime from zd_charitable_project where category_id={categoryId} and is_deleted=0"
                    ;
                }
                else
                {
                    sql = $"SELECT imageUrl,en_content,createTime from zd_charitable_project where category_id={categoryId} and is_deleted=0"
                    ;
                }
                var dt = _broker.ExecuteSQLForDst(sql).Tables[0];
                response["rowCount"] = dt.Rows.Count;
                response["data"] = _broker.ExecuteSQLForDst(sql + limit).Tables[0];
                response["success"] = true;
            }, ex => {
                response["message"] = ex.ToString();
            });
            return response;
        }
        #endregion

        #region 企业公民报告
        [Route("getReportFile")]
        [HttpGet]
        public Dictionary<string, object> getReportFile(string isEnglish)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "";
                if (isEnglish == "0")
                {
                    sql = "SELECT title,file,createTime from zd_report_file where is_deleted=0 ORDER BY createTime";
                }
                else
                {
                    sql = "SELECT en_title,file,createTime from zd_report_file where is_deleted=0 ORDER BY createTime";
                }
                var dt = _broker.ExecuteSQLForDst(sql).Tables[0];
                var imageDt = _broker.ExecuteSQLForDst("select report_background from zd_social_responsibility").Tables[0];
                if (imageDt.Rows.Count > 0)
                {
                    response["report_background"] = imageDt.Rows[0]["report_background"]?.ToString();
                }
                response["data"] = dt;
                response["success"] = true;
            }, ex => {
                response["message"] = ex.ToString();
            });
            return response;
        }
        #endregion
    }
}