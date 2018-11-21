using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.Common;
using CHEER.Common.Schema;
using CHEER.CommonLayer.ePersonnel.Schema;
using CHEER.CommonLayer.eSecurity.Schema;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;
using CHEER.PresentationLayer.Controls;
using CHEER.BusinessLayer.Security;
using CheerUI;

namespace CHEER.PresentationLayer.Security.SuperWork
{
    public partial class UserSelectPage : CHEERBasePage
    {
        private const string RoleUserFunID = "010020020040";
        private string QuerySQLStr
        {
            get { return (string)ViewState["QuerySQLStr"]; }
            set { ViewState["QuerySQLStr"] = value; }
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
                InitFace();
                this.get_transdata();
                this.init_dropdownlist();
                this.init_grid();
            }
        }

        private void InitFace()
        {
            //this.Label9.Text = "用户选取";
            this.txtLoginName.Label = "登录名";
            this.txtEmployeeid.Label = "工号";
            this.txtName.Label = "姓名";
            this.txtRoleName.Label = "角色名称";
            this.drpAccState.Label = "在职状态";
            this.drpLock.Label = "是否停用";
            (StdBranchLoader as StdBranchLoader).IsShowLabel = true;

            (StdBranchLoader as StdBranchLoader).Label = "归属组织";
            this.cmdSearch.Text = "查询";
        }
        private void get_transdata()
        {
            if (Request.QueryString["UserID"] != null)
                this.UserID = Request.QueryString["UserID"].Trim();
            else
                this.UserID = "";
        }
        private void init_dropdownlist()
        {
            this.drpLock.Items.Clear();
            this.drpLock.Items.Add(new CheerUI.ListItem("", "-1"));
            this.drpLock.Items.Add(new CheerUI.ListItem("是", "1"));
            this.drpLock.Items.Add(new CheerUI.ListItem("否", "0"));
            this.drpAccState.Items.Clear();
            this.drpAccState.Items.Add(new CheerUI.ListItem("", "-1"));
            this.drpAccState.Items.Add(new CheerUI.ListItem("离职", ((int)AccessionStatus.Dimission).ToString().Trim()));
            this.drpAccState.Items.Add(new CheerUI.ListItem("调出", ((int)AccessionStatus.Export).ToString().Trim()));
            this.drpAccState.Items.Add(new CheerUI.ListItem("试用", ((int)AccessionStatus.Probation).ToString().Trim()));
            this.drpAccState.Items.Add(new CheerUI.ListItem("晋级试用", ((int)AccessionStatus.PromotingProbation).ToString().Trim()));
            this.drpAccState.Items.Add(new CheerUI.ListItem("正式", ((int)AccessionStatus.Regular).ToString().Trim()));
            this.drpAccState.Items.Add(new CheerUI.ListItem("退休", ((int)AccessionStatus.Retired).ToString().Trim()));

            (StdBranchLoader as StdBranchLoader).IsHaveManageUnit = false;
            (StdBranchLoader as StdBranchLoader).LoadBranchDtsByID(base.GetSecurityChecker(), RoleUserFunID);
        }
        private void init_grid()
        {
            DataTable dt = new DataTable();
            
            this.grdMain.DataSource = dt;
            this.grdMain.DataBind();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            InitGrid();
        }
        protected void InitGrid()
        {
            CommonMethod.AddField(grdMain, "ID", SECUserSchema.USERID, 0, true);
            CommonMethod.AddFlexRendererField(grdMain, "登录名", SECUserSchema.LOGINNAME, 30, "", false);
            CommonMethod.AddFlexRendererField(grdMain, "组织代码", ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE, 30, "", false);
            CommonMethod.AddFlexRendererField(grdMain, "登陆名", ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME, 15, "", false);
            CommonMethod.AddFlexRendererField(grdMain, "工号",PSNAccountSchema.PSNACCOUNT_EMPLOYEEID, 15, "", false);
            CommonMethod.AddFlexRendererField(grdMain, "姓名", PSNAccountSchema.PSNACCOUNT_TRUENAME, 15, "", false);
            CommonMethod.AddFlexRendererField(grdMain, "在职状态", "USERACCSTATE", 30, "", false);
            CommonMethod.AddFlexRendererField(grdMain, "是否停用", "ISUSERLOCK", 15, "", false);
            CommonMethod.AddFlexRendererField(grdMain, "生效日期", SECUserSchema.STARTDATE, 15, "", true);
            CommonMethod.AddFlexRendererField(grdMain, "失效日期", SECUserSchema.ENDDATE, 15, "", true);
            CommonMethod.AddFlexRendererField(grdMain, "", PSNAccountSchema.PSNACCOUNT_BRANCHID, 30, "", true);
            CommonMethod.AddFlexRendererField(grdMain, "", SECUserSchema.PERSONID, 15, "", true);
            CommonMethod.AddFlexRendererField(grdMain, "", PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE, 15, "", true);
            CommonMethod.AddFlexRendererField(grdMain, "", SECUserSchema.ISLOCK, 15, "", true);
        }
        protected void grdMain_RowDoubleClick(object sender, CheerUI.GridRowClickEventArgs e)
        {
            var selectedIndex = this.grdMain.SelectedRowIndex;
            var row = this.grdMain.Rows[selectedIndex];
            PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(row.Cells.FromKey(SECUserSchema.LOGINNAME).ToString(), row.Cells.FromKey(SECUserSchema.USERID).ToString()) + ActiveWindow.GetHideReference());
        }

        protected void grdMain_PageIndexChange(object sender, CheerUI.GridPageEventArgs e)
        {
            grdMain.PageIndex = e.NewPageIndex;
            this.load_data(e.NewPageIndex, true);
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            string selectwherestr = "1=1";
            string loginname = this.txtLoginName.Text.Trim().DBReplace();
            string name = this.txtName.Text.Trim().DBReplace();
            string employeeid = this.txtEmployeeid.Text.Trim().DBReplace();
            string rolename = this.txtRoleName.Text.Trim().DBReplace();
            string accstate = this.drpAccState.SelectedValue.Trim();
            string islock = this.drpLock.SelectedValue.Trim();
            string deptselectstr = (StdBranchLoader as StdBranchLoader).GetSelectedBranchSQLStr();
            if (loginname != "")
                selectwherestr += " and " + SECUserSchema.SECUSER_TABLE + "." + SECUserSchema.LOGINNAME + " like '%" + CHEER.Common.DataProcessor.FormatString(loginname) + "%'";
            if (name != "")
                selectwherestr += " and " + PSNAccountSchema.PSNACCOUNT_TABLENAME + "." + PSNAccountSchema.PSNACCOUNT_TRUENAME + " like '%" + CHEER.Common.DataProcessor.FormatString(name) + "%'";
            if (employeeid != "")
                selectwherestr += " and " + PSNAccountSchema.PSNACCOUNT_TABLENAME + "." + PSNAccountSchema.PSNACCOUNT_EMPLOYEEID + " like '%" + CHEER.Common.DataProcessor.FormatString(employeeid) + "%'";
            if (rolename != "")
                selectwherestr += " and " + SECRoleSchema.SECROLE_TABLE + "." + SECRoleSchema.ROLENAME + " like '%" + CHEER.Common.DataProcessor.FormatString(rolename) + "%'";
            if (accstate != "-1")
                selectwherestr += " and " + PSNAccountSchema.PSNACCOUNT_TABLENAME + "." + PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE + "=" + accstate;
            if (islock != "-1")
                selectwherestr += " and " + SECUserSchema.SECUSER_TABLE + "." + SECUserSchema.ISLOCK + "=" + islock;
            if (deptselectstr != "")
                selectwherestr += " and " + deptselectstr;
            if (this.UserID.Trim() != "")
                selectwherestr += " and " + SECUserSchema.SECUSER_TABLE + "." + SECUserSchema.USERID + "<> '" +
                    DataProcessor.FormatString(this.UserID) + "'";
            this.QuerySQLStr = selectwherestr;
            this.load_data(0, true);
        }

        private void load_data(int pageindex, bool isAlert)
        {
            if (this.QuerySQLStr != null && this.QuerySQLStr.Trim() != "")
            {
                User usermanager = (User)base.GetPalauObject(typeof(User), RoleUserFunID, true, "", false, "");
                DataSet userds = usermanager.GetAllUserInforWithSecurity(this.QuerySQLStr.Trim(), false);
                if (userds.Tables.Count > 0)
                {
                    if (userds.Tables[0].Rows.Count > 0)
                    {
                        DataSet newuserds = this.replacedscolumndata(userds);

                        newuserds.Tables[0].DefaultView.Sort = SECUserSchema.LOGINNAME + " ASC";
                        DataTable dt = newuserds.Tables[0];
                        grdMain.RecordCount = dt.Rows.Count;
                        LoadData(GetPagedDataTable(pageindex, grdMain.PageSize, dt));
                        return;
                    }
                }
            }
            this.init_grid();
            if (this.grdMain.Rows.Count == 0 && isAlert)
            {
                base.ShowAlert("没有查询到符合条件的数据！");
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
        private DataSet replacedscolumndata(DataSet ds)
        {
            ds.Tables[0].Columns.Add("ISUSERLOCK");
            ds.Tables[0].Columns.Add("USERACCSTATE");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (!row.IsNull(SECUserSchema.ISLOCK) && row[SECUserSchema.ISLOCK].ToString().Trim() != "")
                {
                    string islock = row[SECUserSchema.ISLOCK].ToString().Trim();
                    if (islock == "0")
                        row["ISUSERLOCK"] = "否";
                    else if (islock == "1")
                        row["ISUSERLOCK"] = "是";
                    else
                        row["ISUSERLOCK"] = "";
                }
                else
                    row[SECUserSchema.ISLOCK] = "";
                if (!row.IsNull(PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE) && row[PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE].ToString().Trim() != "")
                {
                    int accstate = Convert.ToInt32(row[PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE].ToString().Trim());
                    row["USERACCSTATE"] = this.getaccessionstate(accstate);
                }
                else
                    row["USERACCSTATE"] = "";
            }
            return ds;
        }

        private string getaccessionstate(int accstate)
        {
            string accstatestr = "";
            if (accstate == (int)AccessionStatus.Dimission)
                accstatestr = "离职";
            if (accstate == (int)AccessionStatus.Export)
                accstatestr = "调出";
            if (accstate == (int)AccessionStatus.Probation)
                accstatestr = "试用";
            if (accstate == (int)AccessionStatus.PromotingProbation)
                accstatestr = "晋级试用";
            if (accstate == (int)AccessionStatus.Regular)
                accstatestr = "正式";
            if (accstate == (int)AccessionStatus.Retired)
                accstatestr = "退休";
            if (accstate == (int)AccessionStatus.Unchecked)
                accstatestr = "未接收";
            return accstatestr;
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.grdMain.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            load_data(this.grdMain.PageIndex, true);
        }
    }
}