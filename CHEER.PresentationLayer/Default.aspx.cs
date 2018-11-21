using System;
using CHEER.Common;
using CHEER.CommonLayer.eSecurity.Data;
using System.Web;
using CHEER.BusinessLayer.Security;
using CHEER.Common.AppConstants;
using CHEER.BusinessLayer.Organize.ManageUnit;
using System.ServiceProcess;
namespace CHEER.PresentationLayer
{
    public partial class _Default : CHEERBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Expires = -1;
            if (!IsPostBack)
            {
                Session.Clear();
                HttpCookie cuser = Request.Cookies["username"];
                if (cuser != null)
                {
                    txtUserName.Text = cuser.Value;
                }
                HttpCookie clan = Request.Cookies["language"];
                string language = "ZH-CN";
                if (clan != null && clan.Value != "")
                {
                    language = clan.Value;
                }

                CHEER.Common.Business.ExecuteRedis(redis =>
                {
                    redis.GetDatabase().Publish("SYNCAPPLICATION", "");
                });
            }
        }

        protected override bool IsCheckLoginOut
        {
            get
            {
                return false;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            HttpCookie mm = new HttpCookie("username", this.txtUserName.Text);
            mm.Expires = DateTime.Now.AddYears(10);
            Response.Cookies.Add(mm);
            string lau = "ZH-CN";
            mm = new HttpCookie("language", lau);
            mm.Expires = DateTime.Now.AddYears(10);
            Response.Cookies.Add(mm);

            string _userName = txtUserName.Text.Trim().DBReplace();
            string _password = txtPassword.Text.Trim().DBReplace();
            string messageID;
            User user = new User();
            try
            {
                if (!user.CheckLoginUser(_userName, _password, false, out messageID))
                {
                    ShowNotify(GetMessage(messageID));
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowNotify(ex.Message);
                return;
            }

            SECUserData _userData = user.GetUserDataByLoginName(_userName);
            Session[SystemAppConstants.SESSION_USERID] = _userData.USERID;
            Session[SystemAppConstants.SESSION_USERNAME] = _userData.LOGINNAME;
            ManageUnitManager MUMger = new ManageUnitManager();
            Session[SystemAppConstants.SESSION_CURRENTMANAGEUNIT] = MUMger.getUserManageUnit(_userData.USERID);

            if (string.IsNullOrEmpty(Request.QueryString["ISOPENED"]))
            {
                Response.Redirect(ResolveUrl("~/CG1Main.aspx"), true);
            }
            else
            {
                CheerUI.PageContext.RegisterStartupScript(CheerUI.ActiveWindow.GetHideReference());
            }
        }

        private void ShowNotify(string message)
        {
            CheerUI.Notify notify = new CheerUI.Notify();
            notify.ShowHeader = true;
            notify.Title = "抱歉";
            notify.DisplayMilliseconds = 3000;
            notify.EnableClose = true;
            notify.Message = message.Replace("'", "\"");
            notify.PositionX = CheerUI.Position.Center;
            notify.PositionY = CheerUI.Position.Top;
            notify.Show();
        }

        private string GetMessage(string messageID)
        {
            switch (messageID)
            {
                case "010100":
                    return "您输入的用户名不存在";
                case "010101":
                    return "您输入的用户帐号已锁定";
                case "010102":
                    return "帐号还未启用";
                case "010103":
                    return "帐号已过期";
                case "010104":
                    return "密码错误";
                case "010105":
                    return "需要改密码";
                default:
                    return "用户名或密码输入错误";
            }
        }
    }
}


