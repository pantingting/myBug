using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.Controls;

namespace CHEER.PresentationLayer.Security.SuperWork
{
    public partial class UserCopyToUserPage : CHEERBasePage
    {
        private const string AllotFunID = "0090000500010005";
        protected string UserID
        {
            get { return ViewState["UserID"].ToString(); }
            set { ViewState["UserID"] = value; }
        }
        string LoginName
        {
            get { return ViewState["LoginName"].ToString(); }
            set { ViewState["LoginName"] = value; }
        }
        string DeptID
        {
            get { return ViewState["DeptID"].ToString(); }
            set { ViewState["DeptID"] = value; }
        }
        string DeptName
        {
            get { return ViewState["DeptName"].ToString(); }
            set { ViewState["DeptName"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckPageAccess();
                UserID = Request.QueryString["UserID"];
                if (!GetData(UserID))
                {
                    base.ShowAlert("获取用户数据出错，原因：该用户所属组织不存在！");
                    return;
                }
                InitFace();
                CheerUI.PageContext.RegisterStartupScript("C('" + img.ClientID + "').el.on('click',function(){" + detailWindow.GetSaveStateReference(txtRoleName.ClientID, testname.ClientID)
                    + detailWindow.GetShowReference(base.getBaseUrl() + "Security/SuperWork/UserSelectPage.aspx?UserID=" + UserID, "用户选取", Unit.Pixel(800), Unit.Pixel(500)) + "});");
                CheerUI.PageContext.RegisterStartupScript("C('" + txtRoleName.ClientID + "').on('focus',function(){C('" + img.ClientID + "').el.click();});");
            }
        }
        private void InitFace()
        {
            this.txtName.Label = "源用户登陆名";
            this.txtDeptName.Label = "源用户组织";
            this.txtEmpID.Label = "源用户工号";
            txtRoleName.Label = "目的用户";
            this.ckPack.Label = "是否仅复制功能权限";
            this.ckPack.Text = "是";
            this.ckSec.Label = "是否覆盖原有权限";
            this.ckSec.Text = "是";
            this.cmdAdd.Text = "复制";
            this.cmdReturn.Text = "返回";
            this.ckPack.Checked = false;
            this.ckSec.Checked = false;
        }
        private void CheckPageAccess()
        {
            if (!GetSecurityChecker().IsAllow(AllotFunID))
                ShowAlert("您没有此功能的权限！");
        }
        bool GetData(string userID)
        {
            UserCopy loader = (UserCopy)eHRPageServer.GetPalauObject(typeof(UserCopy));
            DataTable dt = loader.LoadUser(userID);
            if (dt.Rows.Count > 0)
            {
                this.LoginName = dt.Rows[0]["LOGINNAME"].ToString();
                this.DeptID = dt.Rows[0]["UNITID"].ToString();
                this.DeptName = dt.Rows[0]["UNITNAME"].ToString();
                this.txtName.Text = this.LoginName;
                this.txtDeptName.Text = this.DeptName;
                this.txtEmpID.Text = dt.Rows[0]["EMPLOYEEID"].ToString();
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            //modify by Lawliet 20130306
            string touserID = testname.Text;
            //string touserID = this.txtSysID.Text.Trim().DBReplace();
            if (touserID == "")
            {
                base.ShowAlert("目的用户不能为空！");
                return;
            }
            UserCopy loader = (UserCopy)eHRPageServer.GetPalauObject(typeof(UserCopy));
            bool iscopysec = this.ckPack.Checked ? false : true;
            if (!loader.UserCopyExecute(this.UserID, touserID, Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERNAME].ToString(), true, iscopysec, this.ckSec.Checked))
            {
                base.ShowAlert(loader.BLError);
            }
            else
            {
                base.ShowAlert("复制成功！");
                this.lblmsg.Text = "复制用户：[" + this.LoginName + "]权限信息到用户：[" + txtRoleName.Text + "]下成功!";

            }
        }

        protected void cmdReturn_Click(object sender, EventArgs e)
        {
            CheerUI.PageContext.Redirect("../UserAndRole/UserMaintainPage.aspx?BACK=Back");
        }


        private void checkchange(CheerUI.CheckBox checkedbox, CheerUI.CheckBox nocheckbox)
        {
            if (nocheckbox.Checked)
                nocheckbox.Checked = false;
        }

        protected void ckPack_CheckedChanged(object sender, CheerUI.CheckedEventArgs e)
        {
            checkchange(ckPack, ckSec);
        }

        protected void ckSec_CheckedChanged(object sender, CheerUI.CheckedEventArgs e)
        {
            checkchange(ckSec, ckPack);
        }
    }
}