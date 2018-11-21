using CheerUI;
using CHEER.PresentationLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CHEER.PresentationLayer.ePersonnel.SystemConfig
{
    public partial class ParaSettingTabPage : CHEERBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CHEER.BusinessLayer.Security.SecurityChecker _checker = base.GetSecurityChecker();

                //if (_checker.IsAllow("030100020"))
                //{
                //    PageContext.RegisterStartupScript(this.PageTabs.GetAddTabReference("1", "PersonBaseParameterPage.aspx", base.getString("ZGAIA01461"), false));
                //}
                //if (_checker.IsAllow("030100010"))
                //{
                //    PageContext.RegisterStartupScript(this.PageTabs.GetAddTabReference("2", "CustomColumnConfig.aspx", base.getString("ZGAIA01507"), false));
                //}
                //if (_checker.IsAllow("030100050"))
                //{
                //    PageContext.RegisterStartupScript(this.PageTabs.GetAddTabReference("3", "../MailGroupPage.aspx", base.getString("ZGAIA01144"), false));
                //}
                if (_checker.IsAllow("030100070"))
                {
                    PageContext.RegisterStartupScript( this.PageTabs.GetAddTabReference("4", "PersonnelPublicCodeMaintain.aspx", base.getString("ZGAIA01448"), false));
                }
                //if (_checker.IsAllow("030190"))
                //{
                //    PageContext.RegisterStartupScript(this.PageTabs.GetAddTabReference("5", "../Contract/ContractSettingTabPage.aspx", base.getString("ZGAIA02089"), false));
                //}

                //if (_checker.IsAllow("030100080"))
                //{
                //    PageContext.RegisterStartupScript(this.PageTabs.GetAddTabReference("6", "PersonEditField.aspx", base.getString("ZGAIA05923"), false));
                //}

            }
        }
    }
}