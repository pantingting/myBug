using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CheerUI;
using CHEER.PresentationLayer;
using CHEER.Common;

namespace CHEER.PresentationLayer.Security.UserAndRole
{
    public partial class UserTabPage : CHEERBasePage
    {
        private const string UserRoleFunID = "010020010040";
        private string IsAdd
        {
            get { return (string)ViewState["IsAdd"]; }
            set { ViewState["IsAdd"] = value; }
        }
        private string UserID
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
            if (Request.QueryString["USERID"] != null)
                this.UserID = Request.QueryString["USERID"].Trim();
            else
                this.UserID = "";
            this.txtUserID.Text = this.UserID;
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
                _Tab.ID = "UserBase";
                _Tab.Title = "基本资料";
                _Tab.IFrameUrl = "UserAddPage.aspx";
                _Tab.EnableIFrame = true;
                mainTabStrip.Tabs.Add(_Tab);
            }
            else
            {
                CheerUI.Tab _Tab = new CheerUI.Tab();
                _Tab.Title = "基本资料";
                _Tab.IFrameUrl = "UserEditPage.aspx?USERID=" + this.UserID.Trim();
                _Tab.EnableIFrame = true;
                mainTabStrip.Tabs.Add(_Tab);
            }
            if (this.CheckFunAccess(UserRoleFunID))
            {
                CheerUI.Tab _Tab = new CheerUI.Tab();
                _Tab.ID = "UserRole";
                _Tab.Title = "包含角色";
                _Tab.IFrameUrl = "UserRolePage.aspx?USERID=" + this.txtUserID.Text.Trim().DBReplace();
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

        protected string StrCheck
        {
            get
            {
                return "请先维护用户基本资料!";
            }
        }

        protected void mainTabStrip_TabIndexChanged(object sender, EventArgs e)
        {
            if (this.UserID.Trim() == "" && mainTabStrip.ActiveTabIndex == 1)
            {
                mainTabStrip.ActiveTabIndex = 0;
                ShowAlert("请先维护用户基本资料！");
            }
        }
    }
}