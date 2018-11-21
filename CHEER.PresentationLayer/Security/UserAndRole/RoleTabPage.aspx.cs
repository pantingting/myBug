using CheerUI;
using CHEER.PresentationLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.Common;

namespace CHEER.PresentationLayer.Security.UserAndRole
{
    public partial class RoleTabPage : CHEERBasePage
    {
        private const string RoleUserFunID = "010020020040";
        private string IsAdd
        {
            get { return (string)ViewState["IsAdd"]; }
            set { ViewState["IsAdd"] = value; }
        }
        private string RoleID
        {
            get { return (string)ViewState["UserID"]; }
            set { ViewState["UserID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.get_transdata();
                this.init_tab();
            }
        }

        private void get_transdata()
        {
            if (Request.QueryString["ISADD"] != null)
                this.IsAdd = Request.QueryString["ISADD"].Trim();
            else
                this.IsAdd = "";
            if (Request.QueryString["ROLEID"] != null)
                this.RoleID = Request.QueryString["ROLEID"].Trim();
            else
                this.RoleID = "";
            this.txtRoleID.Text = this.RoleID;
        }

        private bool CheckFunAccess(string funid)
        {
            return base.GetSecurityChecker().IsAllow(funid);
        }

        private void init_tab()
        {
            mainTabStrip.Tabs.Clear();
            if (this.IsAdd == "YES")
            {
                CheerUI.Tab _Tab = new CheerUI.Tab();
                _Tab.ID = "RoleBase";
                _Tab.Title = "基本资料";
                _Tab.IFrameUrl = "RoleAddPage.aspx";
                _Tab.EnableIFrame = true;
                mainTabStrip.Tabs.Add(_Tab);
            }
            else
            {
                CheerUI.Tab _Tab = new CheerUI.Tab();
                _Tab.ID = "RoleBase";
                _Tab.Title = "基本资料";
                _Tab.IFrameUrl = "RoleEditPage.aspx?ROLEID=" + this.RoleID.Trim();
                _Tab.EnableIFrame = true;
                mainTabStrip.Tabs.Add(_Tab);
            }
            if (this.CheckFunAccess(RoleUserFunID))
            {
                CheerUI.Tab _Tab = new CheerUI.Tab();
                _Tab.ID = "RoleUser";
                _Tab.Title = "包含用户";
                _Tab.IFrameUrl = "RoleUserPage.aspx?ROLEID=" + this.txtRoleID.Text.Trim().DBReplace();
                _Tab.EnableIFrame = true;
                mainTabStrip.Tabs.Add(_Tab);
            }
            Init_Tab(mainTabStrip);
        }

        public void Init_Tab(CheerUI.TabStrip webTab)
        {
            if (webTab.Tabs.Count == 0)
            {
                CheerUI.Tab itemtab = new CheerUI.Tab();
                webTab.Tabs.Add(itemtab);
            }
        }

        protected void mainTabStrip_TabIndexChanged(object sender, EventArgs e)
        {
            if (this.RoleID.Trim() == "" && mainTabStrip.ActiveTabIndex == 1)
            {
                mainTabStrip.ActiveTabIndex = 0;
                ShowAlert("请先维护用户基本资料！");
            }
        }
    }
}