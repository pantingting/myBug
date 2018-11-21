using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.Security;
using CHEER.PresentationLayer;

namespace CHEER.PresentationLayer.ePersonnel.PSNQuerySet
{
    public partial class PSNTabPage : CHEERBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnReturn.Text = getString("ZGAIA00252");//返回
                SecurityChecker sc = base.GetSecurityChecker();
                string PERSONID = Request.QueryString["PERSONID"];
                Session["psnPERSONID"] = PERSONID;

                CheerUI.Tab _Tab = new CheerUI.Tab();
                if (sc.IsAllow("030070050100001"))
                {
                    _Tab = new CheerUI.Tab();
                    _Tab.Title = base.getString("ZGAIA01157");//"个人基本信息";
                    _Tab.IFrameUrl = "PSNBasicQuery.aspx?PERSONID=" + PERSONID;
                    _Tab.EnableIFrame = true;
                    PageTabs.Tabs.Add(_Tab);
                }


            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            CheerUI.PageContext.Redirect("PSNGrid.aspx?BACK=BACK");
        }
    }
}