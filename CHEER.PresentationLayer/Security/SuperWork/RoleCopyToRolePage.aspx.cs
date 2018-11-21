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

namespace CHEER.PresentationLayer.Security.SuperWork
{
    public partial class RoleCopyToRolePage : CHEERBasePage
    {
        protected string RoleID
        {
            get { return ViewState["RoleID"].ToString(); }
            set { ViewState["RoleID"] = value; }
        }
        string RoleName
        {
            get { return ViewState["RoleName"].ToString(); }
            set { ViewState["RoleName"] = value; }
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
                RoleID = Request.QueryString["ROLEID"];
                if (!GetData(RoleID))
                {
                    base.ShowAlert("获取角色数据出错，原因：该角色所属组织不存在！");
                    return;
                }
                InitFace();
                CheerUI.PageContext.RegisterStartupScript("C('" + img.ClientID + "').el.on('click',function(){" + detailWindow.GetSaveStateReference(txtRoleName.ClientID, txtSysID.ClientID,txtAimDeptName.ClientID)
                    + detailWindow.GetShowReference(base.getBaseUrl() + "Security/SuperWork/RoleSelectPage.aspx?RoleID=" + RoleID, "角色选取", Unit.Pixel(800), Unit.Pixel(500)) + "});");
                CheerUI.PageContext.RegisterStartupScript("C('" + txtRoleName.ClientID + "').on('focus',function(){C('" + img.ClientID + "').el.click();});");
            }
        }
        private const string AllotFunID = "0090000500020005";
        private void InitFace()
        {
            this.gp1.Title = "源角色信息";
            this.txtName.Label = "源角色名称";
            this.txtDeptName.Label = "源角色组织";
            this.gp2.Title = "复制目标";
            this.txtRoleName.Label = "目的角色";
            this.txtAimDeptName.Label = "目的角色组织";
            this.ckPack.Label = "是否仅复制功能权限";
            this.ckPack.Text = "是";
            this.ckSec.Label = "是否覆盖原有权限";
            this.ckSec.Text = "是";
            this.cmdAdd.Text = "复制";
        }
        bool GetData(string roleID)
        {
            RoleCopy loader = (RoleCopy)eHRPageServer.GetPalauObject(typeof(RoleCopy));
            DataTable dt = loader.LoadRole(roleID);
            if (dt.Rows.Count > 0)
            {
                this.RoleName = dt.Rows[0]["ROLENAME"].ToString();
                this.DeptID = dt.Rows[0]["UNITID"].ToString();
                this.DeptName = dt.Rows[0]["UNITNAME"].ToString();
                this.txtName.Text = this.RoleName;
                this.txtDeptName.Text = this.DeptName;
                return true;
            }
            else
            {
                return false;
            }
        }
        private void CheckPageAccess()
        {
            if (!GetSecurityChecker().IsAllow(AllotFunID))
                ShowAlert("您没有此功能的权限！");
        }

        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            string roleID = this.txtSysID.Text.Trim().DBReplace();
            if (roleID == "")
            {
                base.ShowAlert("目的角色不能为空！");
                return;
            }
            RoleCopy loader = (RoleCopy)eHRPageServer.GetPalauObject(typeof(RoleCopy));
            bool iscopysec = this.ckPack.Checked ? true : false;
            if (!loader.RoleCopyToRoleExecute(this.RoleID, roleID, Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERNAME].ToString(), true, iscopysec, this.ckSec.Checked))
            {
                base.ShowAlert(loader.BLError);
            }
            else
            {
                base.ShowAlert("复制成功！");
                this.lblmsg.Text = "复制角色：[" + this.RoleName + "]权限信息到角色：[" + this.txtRoleName.Text +
                    "]下成功!";
                this.txtRoleName.Text = "";
                this.txtSysID.Text = "";
            }
        }

        protected void ckSec_CheckedChanged(object sender, CheerUI.CheckedEventArgs e)
        {
            if (ckPack.Checked)
                ckPack.Checked = false;
        }

        protected void ckPack_CheckedChanged(object sender, CheerUI.CheckedEventArgs e)
        {
            if (ckSec.Checked)
                ckSec.Checked = false;
        }
    }
}