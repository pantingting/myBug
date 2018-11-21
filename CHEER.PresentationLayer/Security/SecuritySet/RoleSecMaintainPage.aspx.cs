using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.Organize;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.CommonLayer.eSecurity.Data;
using CHEER.CommonLayer.Organize.Data.STDOrganize;

namespace CHEER.PresentationLayer.Security.SecuritySet
{
    public partial class RoleSecMaintainPage : CHEER.PresentationLayer.CHEERBasePage
    {
        private const string SecuritySetPageID = "0090000500020005";//"010020030030";
        private const string SecurityQueryFunID = "0090000500020005";
        private const string SecurityFieldFunID = "0090000500020005";
        private const string SecurityEditFunID = "0090000500020005";
        private string RoleID
        {
            get { return (string)ViewState["RoleID"]; }
            set { ViewState["RoleID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitFace();
                this.CheckPageAccess();
                this.GetTransData();
                this.LoadData();
                this.InitTab();
            }
        }
        private void InitFace()
        {
            this.cmdReturn.Text = "返回";
            this.txtRoleName.Label = "角色名称";
            this.txtDept.Label = "归属组织";
        }

        private void CheckPageAccess()
        {
            if (!GetSecurityChecker().IsAllow(SecuritySetPageID))
                ShowAlert("您没有此功能的权限！");
        }
        private bool CheckFunAccess(string funid)
        {
            return base.GetSecurityChecker().IsAllow(funid);
        }
        private void GetTransData()
        {
            if (Request.QueryString["ROLEID"] != null)
                this.RoleID = Request.QueryString["ROLEID"].Trim();
            else
                this.RoleID = "";
            this.txtRoleID.Text = this.RoleID;
            if (Request.QueryString["FROMURL"] != null)
                this.txtFromUrl.Text = Request.QueryString["FROMURL"].Trim();
            else
                this.txtFromUrl.Text = "";
        }
        private void LoadData()
        {
            if (this.RoleID.Trim() != "")
            {
                Role rolemanager = (Role)eHRPageServer.GetPalauObject(typeof(Role));
                SECRoleData roledata = rolemanager.GetSelfInfor(this.RoleID.Trim());
                this.txtRoleName.Text = roledata.ROLENAME;
                STDUnitManager unitmanager = (STDUnitManager)eHRPageServer.GetPalauObject(typeof(STDUnitManager));
                UnitData unitdata = unitmanager.GetCurentUnitDataByID(roledata.DEPTID.Trim());
                if (unitdata != null)
                    this.txtDept.Text = unitdata.UnitName;
            }
        }
        private void InitTab()
        {
            string roleid = this.txtRoleID.Text.Trim().DBReplace();
            UlRoleSecTab.Tabs.Clear();
            if (CheckFunAccess("0090000500020005"))
            {
                CheerUI.Tab secsettab = new CheerUI.Tab();
                secsettab.Title = "设置";
                secsettab.IFrameUrl = "SecurityMaintainPage.aspx?MANID=" + roleid + "&MANTYPE=0";
                secsettab.EnableIFrame = true;
                UlRoleSecTab.Tabs.Add(secsettab);
            }
            //if (CheckFunAccess(SecurityQueryFunID))
            //{
            //    CheerUI.Tab secinfortab = new CheerUI.Tab();
            //    secinfortab.Title = base.getAlert("ZGAIA00190");
            //    secinfortab.IFrameUrl = "SecurityInforPage.aspx?MANID=" + roleid + "&MANTYPE=0";
            //    secinfortab.EnableIFrame = true;
            //    UlRoleSecTab.Tabs.Add(secinfortab);
            //}
            //if (CheckFunAccess(SecurityFieldFunID))
            //{
            //    CheerUI.Tab secfieldtab = new CheerUI.Tab();
            //    secfieldtab.Title = "字段维护";
            //    secfieldtab.IFrameUrl = "SecFieldMaintainPage.aspx?MANID=" + roleid + "&MANTYPE=0";
            //    secfieldtab.EnableIFrame = true;
            //    UlRoleSecTab.Tabs.Add(secfieldtab);
            //}
            if (roleid == "")
                this.UlRoleSecTab.Enabled = false;
        }
    }
}