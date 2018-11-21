using System;
using System.Collections;
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

namespace CHEER.PresentationLayer.Security.UserAndRole
{
    public partial class RoleSelectPage : CHEERBasePage
    {
        private const string UserRoleFunID = "010020010040";
        [Serializable]
        private class QueryData
        {
            private string rolename = "";
            private string deptid = "";
            private bool includesubdept = false;
            public string RoleName
            {
                get { return rolename; }
                set { rolename = value; }
            }
            public string DeptID
            {
                get { return deptid; }
                set { deptid = value; }
            }
            public bool IncludeSubDept
            {
                get { return includesubdept; }
                set { includesubdept = value; }
            }
        }
        private int PageIndex
        {
            get { return (int)ViewState["PageIndex"]; }
            set { ViewState["PageIndex"] = value; }
        }
        private string UserID
        {
            get { return (string)ViewState["UserID"]; }
            set { ViewState["UserID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["QUERYDATA"] = null;
                this.PageIndex = 0;
                InitFace();
                this.get_transdata();
                this.init_dropdownlist();
                this.init_grid();
            }
        }

        private void InitFace()
        {
            this.btnSearch.Text = "查询";
            this.btnConfirm.Text = "确定";
            this.btnCancle.Text = "取消";
            txtRoleName.Label = "角色名称";
        }
        private void get_transdata()
        {
            if (Request.QueryString["USERID"] != null)
                this.UserID = Request.QueryString["USERID"].Trim();
            else
                this.UserID = "";
        }
        private void init_dropdownlist()
        {
            (this.StdBranchLoader as StdBranchLoader).IsHaveManageUnit = true;
            (this.StdBranchLoader as StdBranchLoader).LoadBranchDtsByID(base.GetSecurityChecker(), UserRoleFunID);
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            InitGrid();
        }

        protected void InitGrid()
        {
            CommonMethod.AddFlexField(UlRoleGrid, "角色名", SECRoleSchema.ROLENAME, 25, false);
            CommonMethod.AddFlexField(UlRoleGrid, "", SECRoleSchema.DEPTID, 0, true);
            CommonMethod.AddFlexField(UlRoleGrid, "", SECRoleSchema.ROLEID, 0, true);
            CommonMethod.AddFlexField(UlRoleGrid, "组织代码", ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE, 25, false);
            CommonMethod.AddFlexField(UlRoleGrid, "归属组织", ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME, 25, false);
            CommonMethod.AddFlexField(UlRoleGrid, "角色描述", SECRoleSchema.ROLEDESC, 25, false);
        }
        private void init_grid()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {  new DataColumn(SECRoleSchema.ROLEID,typeof(string)),
													 new DataColumn(SECRoleSchema.ROLENAME,typeof(string)),
													 new DataColumn(SECRoleSchema.DEPTID,typeof(string)),
													 new DataColumn(SECRoleSchema.ROLEDESC,typeof(string)),
													 new DataColumn(ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE,typeof(string)),
													 new DataColumn(ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME,typeof(string)),
			});
            this.UlRoleGrid.DataSource = dt;
            this.UlRoleGrid.DataBind();
        }

        private void load_data(int pageindex, bool isAlert)
        {
            if (Session["QUERYDATA"] != null)
            {
                QueryData querydata = (QueryData)Session["QUERYDATA"];
                string rolename = querydata.RoleName.Trim();
                string depid = querydata.DeptID.Trim();
                bool includesub = querydata.IncludeSubDept;
                string otherquerystr = "";
                if (this.UserID.Trim() != "")
                    otherquerystr = " and " + SECRoleSchema.SECROLE_TABLE + "." + SECRoleSchema.ROLEID + " not in ( select " + SECRoleUserMapSchema.ROLEID + " from " +
                        SECRoleUserMapSchema.SECROLEUSERMAP_TABLE + " where " + SECRoleUserMapSchema.USERID + "='" + this.UserID.Trim() + "' )";
                string pathField = SECRoleSchema.SECROLE_TABLE + "." + SECRoleSchema.DEPTID;
                Role rolemanager = (Role)GetPalauObject(typeof(Role), UserRoleFunID, false, pathField, true, base.getBusinessUnitID());
                DataSet roleds = rolemanager.GetRoleDatasWithSecurity(rolename, depid, includesub, otherquerystr);
                if (roleds.Tables.Count > 0)
                {
                    if (roleds.Tables[0].Rows.Count > 0)
                    {
                        roleds.Tables[0].DefaultView.Sort = SECRoleSchema.ROLENAME + " ASC";
                        LoadData(GetPagedDataTable(pageindex, UlRoleGrid.PageSize, roleds.Tables[0]));
                        return;
                    }
                }
            }
            this.init_grid();
            if (this.UlRoleGrid.Rows.Count == 0 && isAlert)
                base.ShowAlert("没有查询到符合条件的数据！");
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

        protected void LoadData(DataTable dt)
        {
            this.UlRoleGrid.DataSource = dt;
            this.UlRoleGrid.DataBind();
        }

        protected void UlRoleGrid_PageIndexChange(object sender, CheerUI.GridPageEventArgs e)
        {
            UlRoleGrid.PageIndex = e.NewPageIndex;
            load_data(UlRoleGrid.PageIndex, true);
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            ArrayList addidlist = new ArrayList();
            int[] rows = this.UlRoleGrid.SelectedRowIndexArray;
            foreach (int row in rows)
            {
                string roleid = UlRoleGrid.Rows[row].Cells.FromKey(SECRoleSchema.ROLEID).ToString().Trim();
                addidlist.Add(roleid);
            }
            if (addidlist.Count > 0)
            {
                UserRoleMapManager mapmanager = (UserRoleMapManager)eHRPageServer.GetPalauObject(typeof(UserRoleMapManager));
                mapmanager.InsertUserRoleRelation(addidlist, this.UserID.Trim(), 1);
                this.load_data(this.PageIndex, false);
            }
        }

        protected void btnCancle_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            get_querydata();
            load_data(0, true);
            this.PageIndex = 0;
        }
        private void get_querydata()
        {
            QueryData querydata = new QueryData();
            if (this.txtRoleName.Text.Trim() != "")
                querydata.RoleName = this.txtRoleName.Text.Trim().DBReplace();
            CheerUI.ListItem deptitem = (this.StdBranchLoader as StdBranchLoader).GetSelectBranchItem();
            string deptid = deptitem.Value.Trim();
            bool includesub = (this.StdBranchLoader as StdBranchLoader).IsIncludeSubBranch;
            querydata.DeptID = deptid;
            querydata.IncludeSubDept = includesub;
            Session["QUERYDATA"] = querydata;
        }
    }
}