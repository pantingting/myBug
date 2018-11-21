using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.PresentationLayer;

namespace CHEER.PresentationLayer.Security.UserAndRole
{
    public partial class SecurityTabPage : CHEERBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CHEER.BusinessLayer.Security.SecurityChecker _checker = base.GetSecurityChecker();
                CheerUI.Tab _Tab = new CheerUI.Tab();

                if (_checker.IsAllow("009000040001"))
                {
                    _Tab = new CheerUI.Tab();
                    _Tab.Title = "用户维护";
                    _Tab.IFrameUrl = "UserMaintainPage.aspx";
                    _Tab.EnableIFrame = true;
                    mainTabStrip.Tabs.Add(_Tab);
                }
                if (_checker.IsAllow("009000040002"))
                {
                    _Tab = new CheerUI.Tab();
                    _Tab.Title = "角色维护";
                    _Tab.IFrameUrl = "RoleMaintainPage.aspx";
                    _Tab.EnableIFrame = true;
                    mainTabStrip.Tabs.Add(_Tab);
                }
            }
        }
    }
}