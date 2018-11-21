using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.PresentationLayer;

namespace CHEER.PresentationLayer.Security
{
    public partial class AllotTabPage : CHEERBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckPageAccess();
                this.init_tab();
            }
        }
        private void CheckPageAccess()
        {
            if (!GetSecurityChecker().IsAllow("010020040"))
                ShowAlert("您没有此功能的权限！");
        }
        private void init_tab()
        {
            UlUserTab.Tabs.Clear();
            CheerUI.Tab userbaseinfortab = new CheerUI.Tab();
            userbaseinfortab.Title = "快捷分配";
            userbaseinfortab.IFrameUrl = "AccountAllotPage.aspx";
            userbaseinfortab.EnableIFrame = true;
            UlUserTab.Tabs.Add(userbaseinfortab);
            CheerUI.Tab userroletab = new CheerUI.Tab();
            userroletab.Title = "高级分配";
            userroletab.IFrameUrl = "AlotSeletlPage.aspx";
            userroletab.EnableIFrame = true;
            UlUserTab.Tabs.Add(userroletab);
        }
    }
}