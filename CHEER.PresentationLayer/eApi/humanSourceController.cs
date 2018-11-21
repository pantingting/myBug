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
    public class humanSourceController : ApiController
    {
        #region 首页
        [Route("getHumanIndex")]
        [HttpGet]
        public Dictionary<string, object> getHumanIndex(string isEnglish)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "";
                if (isEnglish == "0")
                {
                    sql = "SELECT imageUrl1,imageUrl2,imageUrl3 from zd_human_resources";
                }
                else
                {
                    sql = "SELECT en_imageUrl1,en_imageUrl2,en_imageUrl3 from zd_human_resources";
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

        #region 人才理念
        [Route("getTalentIdea")]
        [HttpGet]
        public Dictionary<string, object> getTalentIdea(string isEnglish)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "";
                if (isEnglish == "0")
                {
                    sql = "SELECT talent_idea from zd_human_resources";
                }
                else
                {
                    sql = "SELECT en_talent_idea from zd_human_resources;";
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

        #region 校园招聘
        [Route("getCampusRecruitment")]
        [HttpGet]
        public Dictionary<string, object> getCampusRecruitment(string isEnglish)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "";
                if (isEnglish == "0")
                {
                    sql = "SELECT campus_recruitment from zd_human_resources";
                }
                else
                {
                    sql = "SELECT en_campus_recruitment from zd_human_resources;";
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

        #region 社会招聘
        [Route("getSocialRecruit")]
        [HttpGet]
        public Dictionary<string, object> getSocialRecruit(string isEnglish, string pageIndex)
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
                    sql = $"SELECT id,career_name,work_place,main_responsibilities,work_qualification,createTime from zd_social_recruit where  is_deleted=0 order by createTime desc"
                    ;
                }
                else
                {
                    sql = $"SELECT id,en_career_name,en_work_place,en_main_responsibilities,en_work_qualification,createTime from zd_social_recruit where  is_deleted=0 order by createTime desc"
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
    }
}