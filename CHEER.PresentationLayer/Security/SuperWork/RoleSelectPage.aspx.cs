using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CheerUI;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.Common.Schema;
using CHEER.CommonLayer.eSecurity.Schema;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;
using CHEER.PresentationLayer.Controls;

namespace CHEER.PresentationLayer.Security.SuperWork
{
    public partial class RoleSelectPage : CHEERBasePage
    {
        private const string UserRoleFunID = "010020010040";
        private int PageIndex
        {
            get { return (int)ViewState["PageIndex"]; }
            set { ViewState["PageIndex"] = value; }
        }
        private string RoleID
        {
            get { return (string)ViewState["RoleID"]; }
            set { ViewState["RoleID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InifFace();
                this.get_transdata();
                this.init_dropdownlist();
                this.init_grid();
            }
        }
        private void InifFace()
        {
            this.txtRoleName.Label = "归属组织";
            (StdBranchLoader as StdBranchLoader).IsShowLabel = true;
            (StdBranchLoader as StdBranchLoader).Label = "归属组织";
            this.cmdSearch.Text = "查询";
        }
        private void init_dropdownlist()
        {
            (StdBranchLoader as StdBranchLoader).IsHaveManageUnit = false;
            (StdBranchLoader as StdBranchLoader).LoadBranchDtsByID(base.GetSecurityChecker(), UserRoleFunID);
        }
        private void get_transdata()
        {
            if (Request.QueryString["RoleID"] != null)
                this.RoleID = Request.QueryString["RoleID"].Trim();
            else
                this.RoleID = "";
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            InitGrid();
        }
        protected void InitGrid()
        {
            CommonMethod.AddFlexRendererField(grdMain, "角色名", SECRoleSchema.ROLENAME, 30, "", false);
            CommonMethod.AddFlexRendererField(grdMain, "", SECRoleSchema.DEPTID, 30, "", true);
            CommonMethod.AddFlexRendererField(grdMain, "", SECRoleSchema.ROLEID, 15, "", true);
            CommonMethod.AddFlexRendererField(grdMain, "归属组织", ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME, 15, "", false);
            CommonMethod.AddFlexRendererField(grdMain, "角色描述", SECRoleSchema.ROLEDESC, 15, "", false);
        }
        private void init_grid()
        {
            DataTable dt = new DataTable();

            this.grdMain.DataSource = dt;
            this.grdMain.DataBind();
        }
        private void LoadData(int index)
        {
            string rolename = this.txtRoleName.Text.Trim().DBReplace();
            string depid = (this.StdBranchLoader as StdBranchLoader).GetSelectBranchItem().Value.Trim();
            bool includesub = (this.StdBranchLoader as StdBranchLoader).IsIncludeSubBranch;
            string otherquerystr = "";
            if (this.RoleID.Trim() != "")
                otherquerystr = " and " + SECRoleSchema.SECROLE_TABLE + "." + SECRoleSchema.ROLEID + "<>'" + DataProcessor.FormatString(RoleID) + "'";
            string pathField = SECRoleSchema.SECROLE_TABLE + "." + SECRoleSchema.DEPTID;
            Role rolemanager = (Role)GetPalauObject(typeof(Role), UserRoleFunID, false, pathField, false, "");
            DataSet roleds = rolemanager.GetRoleDatasWithSecurity(rolename, depid, includesub, otherquerystr);
            if (roleds.Tables.Count > 0)
            {
                if (roleds.Tables[0].Rows.Count > 0)
                {
                    roleds.Tables[0].DefaultView.Sort = SECRoleSchema.ROLENAME + " ASC";
                    //this.NavigatorControl.BindData(roleds.Tables[0].DefaultView, UlRoleGrid, pageindex);
                    grdMain.RecordCount=roleds.Tables[0].Rows.Count;
                    LoadData(GetPagedDataTable(index,grdMain.PageSize,roleds.Tables[0]));
                    return;
                }
            }
        }


        protected void LoadData(DataTable dt)
        {
            this.grdMain.DataSource = dt;
            this.grdMain.DataBind();
        }
        private DataTable GetPagedDataTable(int pageIndex, int pageSize, DataTable dtSource)
        {
            DataTable source = dtSource;

            DataTable paged = source.Clone();

            int rowbegin = pageIndex * pageSize;
            int rowend = (pageIndex + 1) * pageSize;
            if (rowend > source.Rows.Count)
            {
                rowend = source.Rows.Count;
            }

            for (int i = rowbegin; i < rowend; i++)
            {
                paged.ImportRow(source.Rows[i]);
            }

            return paged;
        }
        protected void grdMain_RowDoubleClick(object sender, CheerUI.GridRowClickEventArgs e)
        {
            var selectedIndex = this.grdMain.SelectedRowIndex;
            var row = this.grdMain.Rows[selectedIndex];
            PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(row.Cells.FromKey(SECRoleSchema.ROLENAME).ToString(), row.Cells.FromKey(SECRoleSchema.ROLEID).ToString(), row.Cells.FromKey(ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME).ToString()) + ActiveWindow.GetHideReference());
        }

        protected void grdMain_PageIndexChange(object sender, CheerUI.GridPageEventArgs e)
        {
            grdMain.PageIndex = e.NewPageIndex;
            LoadData(e.NewPageIndex);
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            LoadData(0);
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.grdMain.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            LoadData(this.grdMain.PageIndex);
        }
    }
}