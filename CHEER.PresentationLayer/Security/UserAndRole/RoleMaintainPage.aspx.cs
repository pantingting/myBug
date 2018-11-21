using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.Common.Schema;
using CHEER.CommonLayer.eSecurity.Schema;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;
using CHEER.PresentationLayer.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CHEER.PresentationLayer.Security.UserAndRole
{
    public partial class RoleMaintainPage : CHEERBasePage
    {
        private const string RolePageID = "009000050002";
        private const string RoleAddFunID = "0090000500020001";
        private const string RoleEditFunID = "0090000500020002";
        private const string RoleDeleteFunID = "0090000500020003";
        private const string RoleQueryFunID = "0090000500020004";
        private const string RoleSecFunID = "0090000500020005";

        [Serializable]
        private class QueryData
        {
            private string rolename = "";
            private string deptid = "";
            private bool includesubdept = false;
            private int pageindex = 1;
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
            public int PageIndex
            {
                get { return pageindex; }
                set { pageindex = value; }
            }
        }
        private string IsBack
        {
            get { return (string)ViewState["IsBack"]; }
            set { ViewState["IsBack"] = value; }
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                this.CheckPageAccess();
                this.init_face();
                this.GetTransData();
                this.init_dropdownlist();
                if (this.IsBack.ToUpper().Trim() != "BACK")
                {
                    Session["QUERYDATA"] = null;
                    this.init_grid();
                }
                else
                    this.load_data(true);
            }
        }
        private void CheckPageAccess()
        {
            if (!GetSecurityChecker().IsAllow(RolePageID))
            {
                ShowAlert("您没有此功能的权限！");
            }
                
            this.btnAdd.Hidden = !this.CheckFunAccess(RoleAddFunID);
            this.btnDelete.Hidden = !this.CheckFunAccess(RoleDeleteFunID);
            this.btnQuery.Hidden = !this.CheckFunAccess(RoleQueryFunID);
        }
        private bool CheckFunAccess(string funid)
        {
            return base.GetSecurityChecker().IsAllow(funid);
        }

        private void init_face()
        {
            this.btnQuery.Text = "查询";
            this.btnAdd.Text = "新增";
            this.btnDelete.Text = "删除";
            this.btnAdd.OnClientClick = "return openaddpage();";
            this.btnDelete.ConfirmText = "确认删除？";
            (this.StdBranchLoader as StdBranchLoader).IsShowLabel = true;
            (this.StdBranchLoader as StdBranchLoader).IsShowIncludeSubBranch = false;
            (this.StdBranchLoader as StdBranchLoader).IsIncludeSubBranch = true;
            (this.StdBranchLoader as StdBranchLoader).Label = "归属组织";
            this.detailWindow.Title = "角色维护";
        }

        private void GetTransData()
        {
            if (Request.QueryString["BACK"] != null)
                this.IsBack = Request.QueryString["BACK"].Trim();
            else
                this.IsBack = "";
        }
        private void init_dropdownlist()
        {
            (this.StdBranchLoader as StdBranchLoader).IsHaveManageUnit = true;
            (this.StdBranchLoader as StdBranchLoader).LoadBranchDtsByID(base.GetSecurityChecker(), RoleQueryFunID);
        }
        private void init_grid()
        {
            this.grdMain.DataSource = new DataTable();
            this.grdMain.DataBind();
        }

        protected void Page_Init()
        {
            this.grdMain.Columns.Clear();
            CommonMethod.AddFlexField(grdMain, "角色名", SECRoleSchema.ROLENAME, 25, false);
            CommonMethod.AddFlexField(grdMain, "", SECRoleSchema.DEPTID, 140, true);
            CommonMethod.AddFlexField(grdMain, "", SECRoleSchema.ROLEID, 140, true);
            //CommonMethod.AddFlexField(grdMain, "组织代码", ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE, 25, false);
            CommonMethod.AddFlexField(grdMain, "归属组织", ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME, 25, false);
            CommonMethod.AddFlexField(grdMain, "角色描述", SECRoleSchema.ROLEDESC, 25, false);
            var security = CommonMethod.AddLinkButtonField(grdMain, "授权", "SECURITY", 60, false, false, "", CheerUI.IconFont.Wrench);
            if (this.CheckFunAccess(RoleSecFunID))
                security.Hidden = false;
            else
                security.Hidden = true;
            //var copy = CommonMethod.AddLinkButtonField(grdMain, "复制", "COPY", 60, false, false, "", CheerUI.IconFont.Copy);
            //if (this.CheckFunAccess("0090000500020005"))
            //    copy.Hidden = false;
            //else
            //    copy.Hidden = true;
            var edit = CommonMethod.AddLinkButtonField(grdMain, "详细", "EDIT", 60, false, false, "", CheerUI.IconFont.Edit);
            if (this.CheckFunAccess(RoleEditFunID))
                edit.Hidden = false;
            else
                edit.Hidden = true;
        }

        protected void btnQuery_Click(object sender, System.EventArgs e)
        {
            this.get_querydata();
            this.load_data(true);
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
            querydata.PageIndex = 1;
            Session["QUERYDATA"] = querydata;
        }
        private void load_data(bool isAlert)
        {
            if (Session["QUERYDATA"] != null)
            {
                QueryData querydata = Session["QUERYDATA"] as QueryData;
                string rolename = querydata.RoleName.Trim();
                string depid = querydata.DeptID.Trim();
                bool includesub = querydata.IncludeSubDept;
                int pageindex = querydata.PageIndex;
                this.txtRoleName.Text = rolename;
                (this.StdBranchLoader as StdBranchLoader).SetSelectBranch(depid);
                (this.StdBranchLoader as StdBranchLoader).IsIncludeSubBranch = includesub;
                string pathField = SECRoleSchema.SECROLE_TABLE + "." + SECRoleSchema.DEPTID;
                Role rolemanager = (Role)GetPalauObject(typeof(Role), RoleQueryFunID, false, pathField, true, base.getBusinessUnitID());
                DataSet roleds = rolemanager.GetRoleDatasWithSecurity(rolename, depid, includesub, null);
                if (roleds.Tables.Count > 0)
                {
                    if (roleds.Tables[0].Rows.Count > 0)
                    {
                        roleds.Tables[0].DefaultView.Sort = SECRoleSchema.ROLENAME + " ASC";
                        this.grdMain.DataSource = roleds.Tables[0].DefaultView;
                        this.grdMain.DataBind();
                        return;
                    }
                    else
                    {
                        this.init_grid();
                    }
                }
                if (this.grdMain.Rows.Count == 0 && isAlert)
                {
                    base.ShowAlert("没有查询到符合条件的数据！");
                }
            }
            else
            {
                this.init_grid();

            }
        }
        protected void btnDelete_Click(object sender, System.EventArgs e)
        {
            ArrayList delidlist = new ArrayList();
            var selectedRowIndexs = this.grdMain.SelectedRowIndexArray;
            if (selectedRowIndexs.Length == 0)
            {
                base.ShowAlert("请选择需要删除的数据");
                return;
            }
            foreach (var index in selectedRowIndexs)
            {
                var row = grdMain.Rows[index];
                string roleid = row.Cells.FromKey(SECRoleSchema.ROLEID).ToString().Trim();
                delidlist.Add(roleid);
            }

            if (delidlist.Count > 0)
            {
                Role rolemanager = (Role)eHRPageServer.GetPalauObject(typeof(Role));
                rolemanager.DeleteRoleList(delidlist);
                this.load_data(false);
                base.ShowAlert("删除成功!");
            }
        }

       
        bool haverightonsec(string roleID)
        {
            if (base.GetSecurityChecker().UserID.ToUpper() == "SA")
                return true;
            Role rolemanager = (Role)base.GetPalauObject(typeof(Role), "0090000500020005", false, "", true, base.getBusinessUnitID());
            DataSet roleds = rolemanager.GetRoleDatasWithSecurity(null, null, false, null);
            string filter = SECRoleSchema.ROLEID + "='" + roleID + "'";
            if (roleds.Tables[0].Select(filter).Length > 0)
                return true;
            else
                return false;
        }
        bool haverightoncopy(string roleID)
        {
            if (base.GetSecurityChecker().UserID.ToUpper() == "SA")
                return true;
            Role rolemanager = (Role)base.GetPalauObject(typeof(Role), "0090000500020005", false, "", false, "");
            DataSet roleds = rolemanager.GetRoleDatasWithSecurity(null, null, false, null);
            string filter = SECRoleSchema.ROLEID + "='" + roleID + "'";
            if (roleds.Tables[0].Select(filter).Length > 0)
                return true;
            else
                return false;
        }

        protected void grdMain_RowCommand(object sender, CheerUI.GridCommandEventArgs e)
        {
            var cell = this.grdMain.Columns[e.ColumnIndex];
            string roleid = this.grdMain.Rows[e.RowIndex].Cells.FromKey(SECRoleSchema.ROLEID).ToString().Trim();
            if (cell.ColumnID == "SECURITY")
            {
                if (haverightonsec(roleid))
                {
                    CheerUI.PageContext.Redirect("../SecuritySet/RoleSecMaintainPage.aspx?ROLEID=" + roleid + "&FROMURL=ROLE");
                }
                else
                {
                    base.ShowAlert("您没有该角色的操作权限！");
                }
            }
            else if (cell.ColumnID == "COPY")
            {
                if (haverightonsec(roleid))
                {
                    CheerUI.PageContext.Redirect("../SuperWork/RoleCopyTabPage.aspx?ROLEID=" + roleid + "&FROMURL=ROLE");
                }
                else
                {
                    base.ShowAlert("您没有该角色的操作权限！");
                }
            }
            else if (cell.ColumnID == "EDIT")
            {
                this.detailWindow.IFrameUrl = "RoleTabPage.aspx?ISADD=NO&ROLEID=" + roleid;
                this.detailWindow.Hidden = false;
            }
        }

        protected void detailWindow_Close(object sender, CheerUI.WindowCloseEventArgs e)
        {
            load_data(false);
        }
    }
}