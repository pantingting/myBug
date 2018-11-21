using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.PresentationLayer;

namespace CHEER.PresentationLayer.Security.SuperWork
{
    public partial class RoleCopyTabPage : CHEERBasePage
    {
        private string RoleID
        {
            get { return (string)ViewState["RoleID"]; }
            set { ViewState["RoleID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.cmdReturn.Text = "返回";
                RoleID = Request.QueryString["RoleID"];
                this.init_tab();
            }
        }
        private void init_tab()
        {
            UlRoleTab.Tabs.Clear();
            CheerUI.Tab userbaseinfortab = new CheerUI.Tab();
            userbaseinfortab.Title = "角色至组织";
            userbaseinfortab.IFrameUrl = "RoleCopyPage.aspx?RoleID=" + this.RoleID;
            userbaseinfortab.EnableIFrame = true;
            UlRoleTab.Tabs.Add(userbaseinfortab);
            CheerUI.Tab userbaseinfortab2 = new CheerUI.Tab();
            userbaseinfortab2.Title = "角色至角色";
            userbaseinfortab2.IFrameUrl = "RoleCopyToRolePage.aspx?RoleID=" + this.RoleID;
            userbaseinfortab2.EnableIFrame = true;
            UlRoleTab.Tabs.Add(userbaseinfortab2);
        }
    }
}