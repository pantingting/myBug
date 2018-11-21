using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CHEER.PresentationLayer.CommonUse;

namespace CHEER.PresentationLayer.eApi
{
    [RoutePrefix("eApi")]
    public class NewsCenterController : ApiController
    {
        #region 首页
        [Route("getNewsCenterIndex")]
        [HttpGet]
        public Dictionary<string, object> getNewsCenterIndex(string isEnglish)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker =>
            {
                List<DataTable> data = new List<DataTable>();
                for(int i = 0; i < 3; i++)
                {
                    var sql = "";
                    int limit = 6;
                    if (i == 0)
                        limit = 14;
                    if (isEnglish == "0")
                    {
                        sql = $"SELECT id,title,createTime from zd_news_center where is_deleted=0  and type={i} order by createTime desc limit {limit}";
                    }
                    else
                    {
                        sql = $"SELECT id,en_title,createTime from zd_news_center where is_deleted=0  and type={i} order by createTime desc limit  {limit}";
                    }
                    var dt = _broker.ExecuteSQLForDst(sql).Tables[0];
                    data.Add(dt);
                }
                List<DataTable> a = new List<DataTable>();
                response["data"] = data;
                response["success"] = true;
            }, ex =>
            {
                response["message"] = ex.ToString();
            });
            return response;
        }
        #endregion

        #region 新闻列表
        [Route("getNewsList")]
        [HttpGet]
        public Dictionary<string, object> getNewsList(string isEnglish, string type, string pageIndex)
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
                    sql = $"SELECT id,title,createTime from zd_news_center where is_deleted=0 and type={type} ORDER BY createTime desc"
                    ;
                }
                else
                {
                    sql = $"SELECT id,en_title,createTime from zd_news_center where is_deleted=0 and type={type} ORDER BY createTime desc"
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

        #region 新闻内容
        [Route("getNewsContent")]
        [HttpGet]
        public Dictionary<string, object> getNewsContent(string isEnglish,string newId)
        {
            var response = new Dictionary<string, object>
            {
                ["success"] = false
            };
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "";
                if (isEnglish == "0")
                {
                    sql = $"SELECT id,title,content,createTime from zd_news_center where id={newId}";
                }
                else
                {
                    sql = $"SELECT id,en_title,en_content,createTime from zd_news_center where id={newId}";
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