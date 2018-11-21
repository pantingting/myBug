using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CheerUI;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;

namespace CHEER.PresentationLayer.Security.SecurityView
{
    public partial class SecurityViewTabPage : CHEERBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.init_face();
                this.get_transdata();
                this.init_tab();
            }
        }

        private void init_face()
        {
           
        }

        private void get_transdata()
        {
        }

        private void init_tab()
        {
            myTab.Tabs.Clear();
            Tab userbaseinfortab = new Tab();
           
            userbaseinfortab.Title = "用户权限";
            userbaseinfortab.IFrameUrl = "SecurityViewUserPage.aspx";
            userbaseinfortab.EnableIFrame = true;
            myTab.Tabs.Add(userbaseinfortab);

            userbaseinfortab = new Tab();

            userbaseinfortab.Title = "角色权限";
            userbaseinfortab.IFrameUrl = "SecurityViewRolePage.aspx";
            userbaseinfortab.EnableIFrame = true;
            myTab.Tabs.Add(userbaseinfortab);

        }
    }
}