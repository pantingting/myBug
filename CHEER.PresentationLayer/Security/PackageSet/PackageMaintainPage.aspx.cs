using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.Common.AppConstants;
using CHEER.CommonLayer.eSecurity.Data;
using CHEER.PresentationLayer;

namespace CHEER.PresentationLayer.Security.PackageSet
{
    public partial class PackageMaintainPage : CHEERBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.CheckPageAccess();
            if (!IsPostBack)
            {
                this.GetTransData();
                this.InitFace();
                this.LoadPackageData();
                this.InitTab();
            }
        }

        private string PackageID
        {
            get { return (string)ViewState["PackageID"]; }
            set { ViewState["PackageID"] = value; }
        }
        private string IsAdd
        {
            get { return (string)ViewState["IsAdd"]; }
            set { ViewState["IsAdd"] = value; }
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
        private const string PackageFunID = "0090000500020005";
        private void InitFace()
        {
            this.cmdSave.Text = "保存";
            this.cmdConfirm.Text = "功能权限维护";
            this.txtPackageName.EmptyText = "范围包名称";
            this.txtDescription.EmptyText = "范围包描述";
            this.txtDescription.MaxLengthMessage = "(不得超过100字)";
            this.txtChanger.Text = "最后修改人";
            this.txtChangeTime.Text = "最后修改时间";
            this.lblrule.Text = "主纬度关系：｛固定员工纬度｝∪｛人员相关纬度 { ([组织纬度]∪[抽象纬度]) ∩ [人员相关扩展纬度] }｝";
            if (this.lbPackageID.Text.Trim() == "")
                this.cmdConfirm.Enabled = false;
            this.cmdReturn.Text = "返回";
        }

        private void CheckPageAccess()
        {
            if (!GetSecurityChecker().IsAllow(PackageFunID))
                ShowAlert("您没有此功能的权限！");
            this.cmdConfirm.Enabled = CheckFunAccess(PackageFunID);
        }
        private bool CheckFunAccess(string funid)
        {
            return base.GetSecurityChecker().IsAllow(funid);
        }

        private void GetTransData()
        {
            if (Request.QueryString["PACKAGEID"] != null)
                this.PackageID = Request.QueryString["PACKAGEID"].Trim();
            else
                this.PackageID = "";
            this.lbPackageID.Text = this.PackageID.Trim();
            this.lbPackageID.Text = this.PackageID.Trim();
            if (Request.QueryString["ISADD"] != null)
                this.IsAdd = Request.QueryString["ISADD"].Trim();
            else
                this.IsAdd = "NO";
            if (Request.QueryString["MANID"] != null)
                this.ManID = Request.QueryString["MANID"].Trim();
            else
                this.ManID = "";
            if (Request.QueryString["MANTYPE"] != null)
                this.ManType = Request.QueryString["MANTYPE"].Trim();
            else
                this.ManType = "";
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
                if (packagedata.ROLEID.Trim() != "")
                {
                    this.ManID = packagedata.ROLEID.Trim();
                    this.ManType = "0";
                }
                if (packagedata.USERID.Trim() != "")
                {
                    this.ManID = packagedata.USERID.Trim();
                    this.ManType = "1";
                }
            }
            else
            {
                this.txtChanger.Text = Session[SystemAppConstants.SESSION_USERNAME].ToString().Trim();
                this.txtChangeTime.Text = DataProcessor.DateTimeToShortString(DateTime.Now);
            }
        }
        private void InitTab()
        {
            UlPackageTab.Tabs.Clear();
            CheerUI.Tab psnrelatetab = new CheerUI.Tab();
            psnrelatetab.Title = "人员相关维度";
            psnrelatetab.IFrameUrl = "BranchMaintainpage.aspx?PACKAGEID=" + this.lbPackageID.Text.Trim() + "&RIGHTID=" + PackageFunID;
            psnrelatetab.EnableIFrame = true;
            UlPackageTab.Tabs.Add(psnrelatetab);
            CheerUI.Tab persontab = new CheerUI.Tab();
            persontab.Title = "固定员工维度";
            persontab.IFrameUrl = "PersonMaintainPage.aspx?PACKAGEID=" + this.lbPackageID.Text.Trim() + "&RIGHTID=" + PackageFunID;
            persontab.EnableIFrame = true;
            UlPackageTab.Tabs.Add(persontab);
        }
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            this.cmdConfirm.Enabled = this.CheckFunAccess(PackageFunID);
            int length = this.txtDescription.Text.Trim().Length;
            if (length > 100)
            {
                base.ShowAlert("描述信息长度超出范围！");
                return;
            }
            SECScopePackageData packagedata = new SECScopePackageData();
            SecurityScopePackageLoader packagemanager = (SecurityScopePackageLoader)eHRPageServer.GetPalauObject(typeof(SecurityScopePackageLoader));
            bool isadd = false;
            if (this.lbPackageID.Text.Trim() != "")
                packagedata = packagemanager.GetPackageDataByID(this.PackageID.Trim());
            else
            {
                isadd = true;
                packagedata.PACKAGEID = Guid.NewGuid().ToString().Trim();
                this.lbPackageID.Text = packagedata.PACKAGEID;
                this.PackageID = packagedata.PACKAGEID;
            }
            packagedata.LASTCHANGEDATE = DataProcessor.DateTimeToShortString(DateTime.Now);
            packagedata.LASTCHANGER = Session[SystemAppConstants.SESSION_USERNAME].ToString().Trim();
            packagedata.PACKAGEDESC = this.txtDescription.Text.Trim().DBReplace();
            packagedata.PACKAGENAME = this.txtPackageName.Text.Trim().DBReplace();
            if (this.ManType.Trim() == "0")
                packagedata.ROLEID = this.ManID.Trim();
            if (this.ManType.Trim() == "1")
                packagedata.USERID = this.ManID.Trim();
            if (isadd)
            {
                if (packagemanager.IsPackageNameExist(packagedata.USERID, packagedata.ROLEID, packagedata.PACKAGENAME, ""))
                {
                    base.ShowAlert("范围包名称已经存在，请修改！");
                    this.lbPackageID.Text = "";
                    return;
                }
                packagemanager.Create(packagedata);
            }
            else
            {
                if (packagemanager.IsPackageNameExist(packagedata.USERID, packagedata.ROLEID, packagedata.PACKAGENAME, packagedata.PACKAGEID))
                {
                    base.ShowAlert("范围包名称已经存在，请修改！");
                    return;
                }
                packagemanager.Update(packagedata);
            }
            base.ShowAlert("保存成功!");
            CheerUI.PageContext.RegisterStartupScript("var meTab = C('" + UlPackageTab.ClientID + "').items[0];meTab.setIFrameUrl('" + "BranchMaintainpage.aspx?PACKAGEID=" + this.lbPackageID.Text.Trim() + "&RIGHTID=" + PackageFunID + "');");
            CheerUI.PageContext.RegisterStartupScript("var meTab = C('" + UlPackageTab.ClientID + "').items[1];meTab.setIFrameUrl('" + "PersonMaintainPage.aspx?PACKAGEID=" + this.lbPackageID.Text.Trim() + "&RIGHTID=" + PackageFunID + "');");
        }

        protected void cmdReturn_Click(object sender, EventArgs e)
        {
            CheerUI.PageContext.RegisterStartupScript(CheerUI.ActiveWindow.GetHideRefreshReference());
        }

        protected void cmdConfirm_Click(object sender, EventArgs e)
        {
            string urlstr = "../SecuritySet/SecuritySetTabPage.aspx?PACKAGEID=" + this.lbPackageID.Text.Trim() + "&MANID=" + this.ManID.Trim() + "&MANTYPE=" + this.ManType.Trim() + "&BACK=BACK";
            CheerUI.PageContext.Redirect(urlstr);
        }
    }
}