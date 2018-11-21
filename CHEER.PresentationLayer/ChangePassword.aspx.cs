using CHEER.Platform.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CHEER.PresentationLayer
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnClose.OnClientClick = "document.location.replace('Default.aspx');return false;";
            }
        }

        protected void btnModify_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text.Trim().Replace("'", "''");
            string oldPw = tbxOldPwd.Text.Trim().Replace("'", "''");
            string newPw = tbxNewPwd.Text.Trim().Replace("'", "''");
            string ConfirmPwd = tbxConfirmPwd.Text.Trim().Replace("'", "''");
            if (PwIsTrue(userName, oldPw))
            {
                if (!ifNotSame(ConfirmPwd, newPw))
                {
                    if (updatePw(userName, newPw))
                    {
                        CheerUI.Alert.ShowInTop("修改成功");
                    }
                    else
                    {
                        CheerUI.Alert.ShowInTop("修改失败");
                    }
                }
                else
                {
                    CheerUI.Alert.ShowInTop("两次密码不一致");
                }
            }
            else
            {
                tbxOldPwd.Reset();
                tbxNewPwd.Reset();
                tbxConfirmPwd.Reset();
                CheerUI.Alert.ShowInTop("帐号密码错误");
            }

        }

        /// <summary>
        /// 老登录用户和密码是否正确
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="oldPw"></param>
        /// <returns></returns>
        public bool PwIsTrue(string userName, string oldPw)
        {
            PersistBroker _broker = PersistBroker.Instance();
            string sql = "select 1 from secuser where loginname='" + userName + "' and (PASSWORD='" + oldPw + "' or PASSWORD=md5('" + oldPw + "'))";
            try
            {
                DataTable dt = _broker.ExecuteSQLForDst(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _broker.Close();
            }
        }


        /// <summary>
        /// 判断是否不同如果两次输入不一致则返回true
        /// </summary>
        /// <param name="oldPw"></param>
        /// <param name="newPw"></param>
        /// <returns></returns>
        private bool ifNotSame(string ConfirmPwd, string newPw)
        {
            if (ConfirmPwd != newPw)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="newPw"></param>
        /// <returns></returns>
        private bool updatePw(string userName, string newPw)
        {
            string sql = "update secuser set PASSWORD=md5('" + newPw + "') where loginname='" + userName + "'";
            if (newPw == "")
            {
                sql = "update secuser set PASSWORD='" + newPw + "' where loginname='" + userName + "'";
            }

            PersistBroker _broker = PersistBroker.Instance();
            try
            {
                _broker.ExecuteNonQuery(sql);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                _broker.Close();
            }
        }
    }
}