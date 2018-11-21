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
    public class investorController : ApiController
    {
        #region 公司资料
        [Route("getCompanyInfo")]
        [HttpGet]
        public Dictionary<string, object> getCompanyInfo()
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                List<parent> parent = new List<parent>();
             
                var sql = $"SELECT id,name,en_name from zd_parent_name where isDeleted=0 and type=0 and (parentId='' or parentId is null)";
                var dt = _broker.ExecuteSQLForDst(sql).Tables[0];
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    parent pa = new parent();
                    pa.name = dt.Rows[i]["name"]?.ToString();
                    pa.en_name = dt.Rows[i]["en_name"]?.ToString();
                    var childDt = _broker.ExecuteSQLForDst($"SELECT name,en_name from zd_parent_name where parentId={dt.Rows[i]["id"]}").Tables[0];
                    List<child> childL = new List<child>();
                    for (int j=0;j< childDt.Rows.Count; j++)
                    {
                        child c = new child();
                        c.name = childDt.Rows[j]["name"]?.ToString();
                        c.en_name = childDt.Rows[j]["en_name"]?.ToString();
                        childL.Add(c);
                    }
                    pa.childList = childL;
                    parent.Add(pa);
                }
                response["data"] = parent;
                response["success"] = true;
            }, ex => {
                response["message"] = ex.ToString();
            });
            return response;
        }
        #endregion

        #region 企业管制
        [Route("getEnterpriseControl")]
        [HttpGet]
        public Dictionary<string, object> getEnterpriseControl()
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = $"SELECT `names`,en_names,file from zd_enterprise_control where type=";
                var dt1 = _broker.ExecuteSQLForDst(sql+"0").Tables[0];
                var dt2 = _broker.ExecuteSQLForDst(sql + "1").Tables[0];
                var dt3 = _broker.ExecuteSQLForDst(sql + "2").Tables[0];
                response["data"] = new {
                    审核委员会=dt1,
                    薪酬委员会 = dt2,
                    提名委员会 = dt3
                };
                response["success"] = true;
            }, ex => {
                response["message"] = ex.ToString();
            });
            return response;
        }
        #endregion
        #region 新企业管制
        [Route("getNewEnterpriseControl")]
        [HttpGet]
        public Dictionary<string, object> getNewEnterpriseControl(string type, string pageIndex)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var limit = $" limit {int.Parse(pageIndex) * 20}, 20";
                var sql = $"SELECT file,`names`,en_names from zd_enterprise_control where type={type}";
                var dt = _broker.ExecuteSQLForDst(sql+ limit).Tables[0];
                response["data"] = dt;
                response["success"] = true;
            }, ex => {
                response["message"] = ex.ToString();
            });
            return response;
        }
        #endregion

        #region 高级管理层
        [Route("getSeniorManagement")]
        [HttpGet]
        public Dictionary<string, object> getSeniorManagement(string isEnglish,string type, string pageIndex)
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
                    sql = $"SELECT id,name,description,createTime from zd_senior_management where is_deleted=0 and type={type}";
                }
                else
                {
                    sql = $"SELECT id,en_name,en_description,createTime from zd_senior_management where is_deleted=0 and type={type}";
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

        #region 报告年份
        [Route("getReportYear")]
        [HttpGet]
        public Dictionary<string, object> getReportYear()
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "SELECT `year` from zd_presentation where is_deleted=0 group by year ORDER BY `year` ";
                var dt = _broker.ExecuteSQLForDst(sql).Tables[0];
                response["data"] = dt;
                response["success"] = true;
            }, ex => {
                response["message"] = ex.ToString();
            });
            return response;
        }
        #endregion

        #region 报告列表
        [Route("getReport")]
        [HttpGet]
        public Dictionary<string, object> getReport(string isEnglish, string year)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "";
                if (isEnglish == "0")
                {
                    sql = $"SELECT year,imageUrl,name,file from zd_presentation where is_deleted=0 and year='{year}' ";
                }
                else
                {
                    sql = $"SELECT year,imageUrl,en_name,file from zd_presentation where is_deleted=0 and year='{year}'";
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

        #region 公告
        [Route("getNotice")]
        [HttpGet]
        public Dictionary<string, object> getNotice(string isEnglish, string type, string pageIndex)
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
                    sql = $"SELECT id,name,file,createTime from zd_notice where is_deleted=0 and type={type}";
                }
                else
                {
                    sql = $"SELECT id,en_name,file,createTime from zd_notice where is_deleted=0 and type={type}";
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

        #region 通函
        [Route("getLetter")]
        [HttpGet]
        public Dictionary<string, object> getLetter(string isEnglish, string type, string pageIndex)
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
                    sql = $"SELECT id,name,file,createTime from zd_letter where is_deleted=0 and type={type}";
                }
                else
                {
                    sql = $"SELECT id,en_name,file,createTime from zd_letter where is_deleted=0 and type={type}";
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

        #region 新闻稿
        [Route("getNews")]
        [HttpGet]
        public Dictionary<string, object> getNews(string isEnglish, string type, string pageIndex)
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
                    sql = $"SELECT id,name,file,createTime from zd_news where is_deleted=0 and type={type}";
                }
                else
                {
                    sql = $"SELECT id,en_name,file,createTime from zd_news where is_deleted=0 and type={type}";
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

        #region 股份资料
        [Route("getSharesInfo")]
        [HttpGet]
        public Dictionary<string, object> getSharesInfo()
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                List<parent> parent = new List<parent>();

                var sql = $"SELECT id,name,en_name from zd_parent_name where isDeleted=0 and type=1 and (parentId='' or parentId is null)";
                var dt = _broker.ExecuteSQLForDst(sql).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    parent pa = new parent();
                    pa.name = dt.Rows[i]["name"]?.ToString();
                    pa.en_name = dt.Rows[i]["en_name"]?.ToString();
                    var childDt = _broker.ExecuteSQLForDst($"SELECT name,en_name from zd_parent_name where parentId={dt.Rows[i]["id"]}").Tables[0];
                    List<child> childL = new List<child>();
                    for (int j = 0; j < childDt.Rows.Count; j++)
                    {
                        child c = new child();
                        c.name = childDt.Rows[j]["name"]?.ToString();
                        c.en_name = childDt.Rows[j]["en_name"]?.ToString();
                        childL.Add(c);
                    }
                    pa.childList = childL;
                    parent.Add(pa);
                }
                response["data"] = parent;
                response["success"] = true;
            }, ex => {
                response["message"] = ex.ToString();
            });
            return response;
        }
        #endregion

        #region 联系部
        [Route("getContactPart")]
        [HttpGet]
        public Dictionary<string, object> getContactPart()
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var dt = _broker.ExecuteSQLForDst($"select * from zd_invertor_relation").Tables[0];
                response["data"] = dt;
                response["success"] = true;
            }, ex => {
                response["message"] = ex.ToString();
            });
            return response;
        }
        #endregion


        public class parent{
            public string name { get; set; }

            public string en_name { get; set; }

            public List<child> childList = new List<child>();
        }
        public class child
        {
            public string name { get; set; }
            public string en_name { get; set; }
        }
    }
}