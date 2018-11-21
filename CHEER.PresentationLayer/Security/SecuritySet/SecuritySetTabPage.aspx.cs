using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.CommonLayer.eSecurity.Data;
using CHEER.PresentationLayer;

namespace CHEER.PresentationLayer.Security.SecuritySet
{
    public partial class SecuritySetTabPage : CHEERBasePage
    {
        private string PackageID
        {
            get { return (string)ViewState["PackageID"]; }
            set { ViewState["PackageID"] = value; }
        }
        private string ManID
        {
            get { return (string)ViewState["ManID"]; }
            set { ViewState["ManID"] = value; }
        }
        private string ManType
        {
            get { return (string)ViewState["ManType"]; }
            set { ViewState["ManType"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.InitFace();
                this.GetTransData();
                this.LoadPackageData();
                init_tab();
            }
        }
        private void InitFace()
        {
            this.cmdReturn.Text = "返回";
            this.txtPackageName.Text = "范围包名称";
            this.txtDescription.Text = "范围包描述";
            this.txtChanger.Text = "最后修改人";
            this.txtChangeTime.Text = "最后修改时间";
        }
        private void GetTransData()
        {
            if (Request.QueryString["PACKAGEID"] != null)
                this.PackageID = Request.QueryString["PACKAGEID"].Trim();
            else
                this.PackageID = "";
            this.txtPackageID.Text = this.PackageID.Trim();
            if (Request.QueryString["MANID"] != null)
                this.ManID = Request.QueryString["MANID"].Trim();
            else
                this.ManID = "";
            if (Request.QueryString["MANTYPE"] != null)
                this.ManType = Request.QueryString["MANTYPE"].Trim();
            else
                this.ManType = "";
            if (Request.QueryString["BACK"] != null)
                this.txtIsBack.Text = Request.QueryString["BACK"].Trim();
            else
                this.txtIsBack.Text = "";
        }
        private void init_tab()
        {
            UlRoleTab.Tabs.Clear();
            CheerUI.Tab userbaseinfortab = new CheerUI.Tab();
            userbaseinfortab.Title = "功能权限维护";
            userbaseinfortab.IFrameUrl = "SecuritySetPage.aspx?PACKAGEID=" + PackageID + "&MANID=" + ManID + "&MANTYPE=" + ManType;
            userbaseinfortab.EnableIFrame = true;
            UlRoleTab.Tabs.Add(userbaseinfortab);
        }
        private void LoadPackageData()
        {
            if (this.PackageID.Trim() != "")
            {
                SecurityScopePackageLoader packagemanager = (SecurityScopePackageLoader)eHRPageServer.GetPalauObject(typeof(SecurityScopePackageLoader));
                SECScopePackageData packagedata = packagemanager.GetPackageDataByID(this.PackageID.Trim());
                this.txtPackageName.Text = packagedata.PACKAGENAME;
                this.txtDescription.Text = packagedata.PACKAGEDESC;
                this.txtChanger.Text = packagedata.LASTCHANGER;
                this.txtChangeTime.Text = packagedata.LASTCHANGEDATE;
            }
        }

        protected void cmdReturn_Click(object sender, EventArgs e)
        {
            CheerUI.PageContext.RegisterStartupScript(CheerUI.ActiveWindow.GetHidePostBackReference());
        }
    }
}