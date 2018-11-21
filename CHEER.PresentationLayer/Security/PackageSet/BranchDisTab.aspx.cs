using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CheerUI;
using CHEER.BusinessLayer.Organize;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.Common.Schema;
using CHEER.CommonLayer.eSecurity;
using CHEER.PresentationLayer;

namespace CHEER.PresentationLayer.Security.PackageSet
{
    public partial class BranchDisTab : CHEERBasePage
    {
        private string SecurityID
        {
            get { return (string)ViewState["SecurityID"]; }
            set { ViewState["SecurityID"] = value; }
        }
        private string PackageID
        {
            get { return (string)ViewState["PackageID"]; }
            set { ViewState["PackageID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.lbIsSecurity.Text = "NO";
                this.InitFace();
                this.get_transdata();
                this.init_tab();
            }
        }
        private void InitFace()
        {
            this.cmdConfirm.Text = "确定";
            this.cmdReturn.Text = "关闭";
            this.txtUnitCode.Label = "组织代码";
            this.checkSub.Label = "包含子组织";
            this.CkbIsIncludeStopUnit.Text = "包含停用组织";
        }

        private void get_transdata()
        {
            if (Request.QueryString["RIGHTID"] != null)
                this.SecurityID = Request.QueryString["RIGHTID"].Trim();
            else
                this.SecurityID = "";
            if (Request.QueryString["PACKAGEID"] != null)
                this.PackageID = Request.QueryString["PACKAGEID"].Trim();
            else
                this.PackageID = "";
        }
        private void init_tab()
        {
            UltDepTab.Tabs.Clear();
            bool isIncludeStopUnit = this.CkbIsIncludeStopUnit.Checked;
            this.Title = "组织选择";
            CheerUI.Tab treetab = new CheerUI.Tab();
            treetab.ID = "Tree";
            treetab.Title = "组织树形图";
            treetab.IFrameUrl = "BranchDisTree.aspx?SECURITYID=" + this.SecurityID.Trim() + "&INCLUDESTOPUNIT=" + isIncludeStopUnit;
            treetab.EnableIFrame = true;
            UltDepTab.Tabs.Add(treetab);
            CheerUI.Tab searchtab = new CheerUI.Tab();
            treetab.ID = "Select";
            searchtab.Title = "组织查询";
            searchtab.IFrameUrl = "BranchSearch.aspx?SECURITYID=" + this.SecurityID.Trim() + "&INCLUDESTOPUNIT=" + isIncludeStopUnit;
            searchtab.EnableIFrame = true;
            UltDepTab.Tabs.Add(searchtab);
        }

        protected void cmdConfirm_Click(object sender, EventArgs e)
        {
            string issec = this.lbIsSecurity.Text.Trim().DBReplace();
            if (issec == "NO")
            {
                STDUnitManager unitmanager = (STDUnitManager)eHRPageServer.GetPalauObject(typeof(STDUnitManager));
                DataSet unitds = unitmanager.FindUnit(this.txtUnitCode.Text.Trim(), "", "", "", "", "", "0");
                if (unitds.Tables.Count <= 0 || unitds.Tables[0].Rows.Count <= 0)
                {

                    base.ShowAlert("请输入正确的组织代码");
                    return;
                }
                else
                    this.lbUnitID.Text = unitds.Tables[0].Rows[0][ORGStdStructSchema.ORGSTDSTRUCT_UNITID].ToString().Trim();
                SecurityChecker checker = base.GetSecurityChecker();
                ArrayList branchlist = checker.GetAccessBranches(this.SecurityID.Trim());
                bool secresult = true;
                if (branchlist.Count <= 0)
                    secresult = false;
                Hashtable secbranchhash = new Hashtable();
                for (int i = 0; i < branchlist.Count; i++)
                {
                    string unitid = ((ArrayList)branchlist[i])[0].ToString().Trim();
                    if (!secbranchhash.Contains(unitid))
                        secbranchhash.Add(unitid, unitid);
                }
                if (!secbranchhash.Contains(this.lbUnitID.Text.Trim()))
                    secresult = false;
                if (!secresult)
                {

                    base.ShowAlert("您对该组织无权限");
                    return;
                }
                this.lbIsSecurity.Text = "YES";
            }
            DimensionItemType itemtype;
            if (this.checkSub.Checked)
                itemtype = DimensionItemType.IncludeSubOrg;
            else if (this.checkSubUnderMU.Checked)
                itemtype = DimensionItemType.IncludeSubOrgInManageUnit;
            else
                itemtype = DimensionItemType.Common;
            SecurityDimensionItemLoader itemmanager = (SecurityDimensionItemLoader)eHRPageServer.GetPalauObject(typeof(SecurityDimensionItemLoader));
            if (itemmanager.JudgeIfItemValueExistInPackage(this.lbUnitID.Text.Trim(), itemtype, this.PackageID.Trim()))
            {
                base.ShowAlert("该条记录已添加");
                return;
            }
            ArrayList dimidlist = new ArrayList();
            dimidlist.Add(this.lbUnitID.Text.Trim());
            itemmanager.SaveDimItemList(dimidlist, this.PackageID.Trim(), "BRANCH", itemtype, false);
            base.ShowAlert("保存成功!");
        }

        protected void cmdReturn_Click(object sender, EventArgs e)
        {
            CheerUI.PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected void CkbIsIncludeStopUnit_CheckedChanged(object sender, CheckedEventArgs e)
        {
            init_tab();
        }
    }
}