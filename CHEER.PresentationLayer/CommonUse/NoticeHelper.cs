using CHEER.Platform.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHEER.PresentationLayer.CommonUse
{
    public class NoticeHelper
    {
        public static void SendMessage(PersistBroker _broker, string id)
        {
            var target = string.Empty;
            var userId = string.Empty;
            var groupId = string.Empty;
            var course_category_id = string.Empty;
            var sql = $@"select target,userId,groupId,course_category_id from zsh_notice where id = '{id}'";
            var dt = _broker.ExecuteSQLForDst(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                target = dt.Rows[0]["target"].ToString();
                userId = dt.Rows[0]["userId"].ToString();
                groupId = dt.Rows[0]["groupId"].ToString();
                course_category_id = dt.Rows[0]["course_category_id"].ToString();

                if (target == "0")
                {
                    //已选课程学员
                    var query = $@"select user_id from zsh_user_course where is_deleted = '0' and course_id = '{course_category_id}' group by user_id";
                    var dtQuery = _broker.ExecuteSQLForDst(query).Tables[0];
                    for (int i = 0; i < dtQuery.Rows.Count; i++)
                    {
                        var user_id = dtQuery.Rows[i]["user_id"].ToString();
                        var insertNotice = $@"insert into zsh_user_notice (user_id,notice_id) values ('{user_id}','{id}')";
                        if (!IsExistsUserNotice(_broker, user_id, id))
                        {
                            _broker.ExecuteNonQuery(insertNotice);
                        }
                    }
                }
                else if (target == "1")
                {
                    //老师
                    var query = $@"select id from zsh_user where isteacher = '1'";
                    var dtQuery = _broker.ExecuteSQLForDst(query).Tables[0];
                    for (int i = 0; i < dtQuery.Rows.Count; i++)
                    {
                        var user_id = dtQuery.Rows[i]["id"].ToString();
                        //老师授权表中有没有课程
                        var queryAuth = $@"select id from zsh_teacher_authorization where is_deleted = '0' and 
                                            type = 'zsh_course_category' and user_id = '{user_id}' and curriculum_id = '{course_category_id}'";
                        var dtAuth = _broker.ExecuteSQLForDst(queryAuth).Tables[0];
                        if(dt.Rows.Count >0)
                        {
                            var insertNotice = $@"insert into zsh_user_notice (user_id,notice_id) values ('{user_id}','{id}')";
                            if (!IsExistsUserNotice(_broker, user_id, id))
                            {
                                _broker.ExecuteNonQuery(insertNotice);
                            }
                        }
                    }
                }
                else if (target == "2")
                {
                    //个人
                    string[] arr = userId.Split(',');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        var insertNotice = $@"insert into zsh_user_notice (user_id,notice_id) values ('{arr[i]}','{id}')";
                        if (!IsExistsUserNotice(_broker, userId, id))
                        {
                            _broker.ExecuteNonQuery(insertNotice);
                        }
                    }
                }
                else if (target == "3")
                {
                    //学员组
                    var query = $@"select user_id from zsh_group_user where group_id = '{groupId}' and is_deleted = '0'";
                    var dtQuery = _broker.ExecuteSQLForDst(query).Tables[0];
                    for (int i = 0; i < dtQuery.Rows.Count; i++)
                    {
                        var user_id = dtQuery.Rows[i]["user_id"].ToString();
                        var insertNotice = $@"insert into zsh_user_notice (user_id,notice_id) values ('{user_id}','{id}')";
                        if (!IsExistsUserNotice(_broker, user_id, id))
                        {
                            _broker.ExecuteNonQuery(insertNotice);
                        }
                    }
                }

            }
        }

        public static bool IsExistsUserNotice(PersistBroker _broker, string user_id, string id)
        {
            var sql = $@"select id from zsh_user_notice where is_deleted = '0' and user_id = '7' and notice_id = '2'";
            var dt = _broker.ExecuteSQLForDst(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}