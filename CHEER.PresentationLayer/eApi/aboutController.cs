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
    public class aboutController : ApiController
    {
        #region 首页
        [Route("getAboutIndex")]
        [HttpGet]
        public Dictionary<string, object> getAboutIndex(string isEnglish)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker=> {
                var sql = "";
                if (isEnglish == "0")
                {
                    sql = "SELECT imageUrl1,imageUrl2,imageUrl3,imageUrl4 from zd_about";
                }
                else
                {
                    sql = "SELECT en_imageUrl1,en_imageUrl2,en_imageUrl3,en_imageUrl4 from zd_about;";
                }
                var dt = _broker.ExecuteSQLForDst(sql).Tables[0];
                response["data"] = dt;
                response["success"] = true;
            },ex=> {
                response["message"] = ex.ToString();
            });
            return response;
        }
        #endregion

        #region 主席致辞
        [Route("getChairpersonSpeech")]
        [HttpGet]
        public Dictionary<string, object> getChairpersonSpeech(string isEnglish)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "";
                if (isEnglish == "0")
                {
                    sql = "SELECT chairpersonSpeech from zd_about";
                }
                else
                {
                    sql = "SELECT en_chairpersonSpeech from zd_about;";
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

        #region 集团简介
        [Route("getGroupIntroduction")]
        [HttpGet]
        public Dictionary<string, object> getGroupIntroduction(string isEnglish)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "";
                if (isEnglish == "0")
                {
                    sql = "SELECT groupIntroduction from zd_about";
                }
                else
                {
                    sql = "SELECT en_groupIntroduction from zd_about;";
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

        #region 管理团队
        [Route("getGroupPerson")]
        [HttpGet]
        public Dictionary<string, object> getGroupPerson()
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                
               var sql = "SELECT * from zd_group ORDER BY sortNo";
                var dt = _broker.ExecuteSQLForDst(sql).Tables[0];
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    Group group = new Group();
                    group.name = dt.Rows[i]["name"]?.ToString();
                    group.sortNo= dt.Rows[i]["sortNo"]?.ToString();
                    var  personDt = _broker.ExecuteSQLForDst($"SELECT * from zd_group_person where groupId={dt.Rows[i]["id"]} ORDER BY sortNo").Tables[0];
                    for(int j = 0; j < personDt.Rows.Count; j++)
                    {
                        GroupPerson person = new GroupPerson();
                        person.imageUrl = personDt.Rows[j]["imageUrl"]?.ToString();
                        person.en_name = personDt.Rows[j]["en_name"]?.ToString();
                        person.name = personDt.Rows[j]["name"]?.ToString();
                        person.en_position = personDt.Rows[j]["en_position"]?.ToString();
                        person.position = personDt.Rows[j]["position"]?.ToString();
                        person.en_introduction = personDt.Rows[j]["en_introduction"]?.ToString();
                        person.introduction = personDt.Rows[j]["introduction"]?.ToString();
                        person.sortNo = personDt.Rows[j]["sortNo"]?.ToString();
                        group.groupPerson.Add(person);
                    }
                    groupList.Add(group);
                }
                response["data"] = groupList;
                response["success"] = true;
            }, ex => {
                response["message"] = ex.ToString();
            });
            return response;
        }
        public List<Group> groupList = new List<Group>();
        public class GroupPerson
        {
            public string imageUrl { get; set; }
            public string en_name { get; set; }
            public string name { get; set; }
            public string en_position { get; set; }
            public string position { get; set; }
            public string en_introduction { get; set; }
            public string introduction { get; set; }
            public string sortNo { get; set; }
        }
        public class Group
        {
            public string name { get; set; }
            public string sortNo { get; set; }
            public List<GroupPerson> groupPerson = new List<GroupPerson>();
        }
        #endregion

        #region 发展战略
        [Route("getDevelopIdea")]
        [HttpGet]
        public Dictionary<string, object> getDevelopIdea(string isEnglish)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "";
                if (isEnglish == "0")
                {
                    sql = "SELECT title,content from zd_develop_idea where is_deleted=0 order by sortNo";
                }
                else
                {
                    sql = "SELECT en_title,en_content from zd_develop_idea where is_deleted=0 order by sortNo";
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

        #region 战略优势
        [Route("getStrategicAdvantage")]
        [HttpGet]
        public Dictionary<string, object> getStrategicAdvantage(string isEnglish)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "";
                if (isEnglish == "0")
                {
                    sql = "SELECT title,content from zd_strategic_advantage where is_deleted=0 order by sortNo";
                }
                else
                {
                    sql = "SELECT en_title,en_content from zd_strategic_advantage where is_deleted=0 order by sortNo";
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

        #region 企业文化
        [Route("getCorporateCulture")]
        [HttpGet]
        public Dictionary<string, object> getCorporateCulture(string isEnglish)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "";
                if (isEnglish == "0")
                {
                    sql = "SELECT title,content from zd_corporate_culture where is_deleted=0 order by sortNo";
                }
                else
                {
                    sql = "SELECT en_title,en_content from zd_corporate_culture where is_deleted=0 order by sortNo";
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

        #region 集团荣誉
        [Route("getGroupHonor")]
        [HttpGet]
        public Dictionary<string, object> getGroupHonor(string isEnglish)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "";
                if (isEnglish == "0")
                {
                    sql = "SELECT imageUrl,content from zd_group_honor where is_deleted=0 order by sortNo";
                }
                else
                {
                    sql = "SELECT imageUrl,en_content from zd_group_honor where is_deleted=0 order by sortNo";
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

        #region 建党工作
        [Route("getPartyBuildingWork")]
        [HttpGet]
        public Dictionary<string, object> getPartyBuildingWork(string isEnglish)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "";
                if (isEnglish == "0")
                {
                    sql = "SELECT partyBuildingWork from zd_about";
                }
                else
                {
                    sql = "SELECT en_partyBuildingWork from zd_about;";
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
    }
}