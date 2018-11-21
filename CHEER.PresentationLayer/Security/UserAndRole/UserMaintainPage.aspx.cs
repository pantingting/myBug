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
using CHEER.BusinessLayer.Security;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;
using CHEER.PresentationLayer.Controls;
using System.Collections;
using CHEER.Platform.DAL;

namespace CHEER.PresentationLayer.Security.UserAndRole
{
    public partial class UserMaintainPage : CHEERBasePage
    {
        private const string UserPageID = "009000050001";
        private const string UserAddFunID = "0090000500010001";
        private const string UserEditFunID = "0090000500010002";
        private const string UserDeleteFunID = "0090000500010003";
        private const string UserQueryFunID = "0090000500010004";
        private const string UserSecFunID = "0090000500010005";
        [Serializable]
        private class QueryData
        {
            string loginname = "";
            string deptid = "";
            string psnname = "";
            string employeeid = "";
            string rolename = "";
            string statestr = "-1";
            string islock = "-1";
            bool includesub = false;
            private string querysql = "";
            private int pageindex = 1;
            public string LoginName
            {
                get { return loginname; }
                set { loginname = value; }
            }
            public string DeptID
            {
                get { return deptid; }
                set { deptid = value; }
            }
            public string PsnName
            {
                get { return psnname; }
                set { psnname = value; }
            }
            public string EmployeeID
            {
                get { return employeeid; }
                set { employeeid = value; }
            }
            public string RoleName
            {
                get { return rolename; }
                set { rolename = value; }
            }
            public string StateStr
            {
                get { return statestr; }
                set { statestr = value; }
            }
            public string IsLock
            {
                get { return islock; }
                set { islock = value; }
            }
            public bool IncludeSub
            {
                get { return includesub; }
                set { includesub = value; }
            }
            public string QuerySql
            {
                get { return querysql; }
                set { querysql = value; }
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.CheckPageAccess();
                this.init_face();
                this.GetTransData();
                this.init_dropdownlist();
                if (this.IsBack.ToUpper().Trim() != "BACK")
                {
                    Session["QUERYSQLSTR"] = null;
                    this.init_grid();
                }
                else
                    this.getgriddata(true, 0);
            }
        }

        private void CheckPageAccess()
        {
            if (!GetSecurityChecker().IsAllow(UserPageID))
                ShowAlert(getAlert("ZGAIA00809"));
            this.btnAdd.Hidden = !this.CheckFunAccess(UserAddFunID);
            this.btnDelete.Hidden = !this.CheckFunAccess(UserDeleteFunID);
            this.btnSearch.Hidden = !this.CheckFunAccess(UserQueryFunID);
        }
        private bool CheckFunAccess(string funid)
        {
            return base.GetSecurityChecker().IsAllow(funid);
        }

        private void init_face()
        {
            this.btnSearch.Text = base.getString("ZGAIA00196");
            this.btnAdd.Text = base.getString("ZGAIA00023");
            this.btnDelete.Text = base.getString("ZGAIA00194");
            this.btnExport.Text = base.getString("ZGAIA00548");
            txtLoginName.Label = base.getString("ZGAIA00238");
            txtName.Label = base.getString("ZGAIA00208");
            txtEmployeeid.Label = base.getString("ZGAIA00209");
            txtRoleName.Label = base.getString("ZGAIA00237");
            drpAccStates.Label = base.getString("ZGAIA00241");
            drpLock.Label = base.getString("ZGAIA00239");
            this.btnAdd.OnClientClick = "return openaddpage()";
            btnDelete.ConfirmText = base.getString("ZGAIA01407");
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
            this.drpLock.Items.Clear();
            this.drpLock.Items.Add(new CheerUI.ListItem("", "-1"));
            this.drpLock.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA00254"), "1"));
            this.drpLock.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA00785"), "0"));
            this.drpAccStates.Items.Clear();
            this.drpAccStates.Items.Add(new CheerUI.ListItem("", "-1"));
            this.drpAccStates.Items.Add(new CheerUI.ListItem(base.getAlert("ZGAIA00037"), ((int)AccessionStatus.Dimission).ToString().Trim()));
            this.drpAccStates.Items.Add(new CheerUI.ListItem(base.getAlert("ZGAIA00030"), ((int)AccessionStatus.Export).ToString().Trim()));
            this.drpAccStates.Items.Add(new CheerUI.ListItem(base.getAlert("ZGAIA00029"), ((int)AccessionStatus.Probation).ToString().Trim()));
            this.drpAccStates.Items.Add(new CheerUI.ListItem(base.getAlert("ZGAIA00028"), ((int)AccessionStatus.PromotingProbation).ToString().Trim()));
            this.drpAccStates.Items.Add(new CheerUI.ListItem(base.getAlert("ZGAIA00027"), ((int)AccessionStatus.Regular).ToString().Trim()));
            this.drpAccStates.Items.Add(new CheerUI.ListItem(base.getAlert("ZGAIA00026"), ((int)AccessionStatus.Retired).ToString().Trim()));
            (StdBranchLoader as StdBranchLoader).LoadBranchDtsByID(base.GetSecurityChecker(), UserQueryFunID);
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            InitGrid();
        }
        private void init_grid()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {  new DataColumn(SECUserSchema.USERID,typeof(string)),
                                                     new DataColumn(SECUserSchema.LOGINNAME,typeof(string)),
                                                     new DataColumn(SECUserSchema.ISLOCK,typeof(int)),
                                                     new DataColumn(SECUserSchema.STARTDATE,typeof(string)),
                                                     new DataColumn(SECUserSchema.ENDDATE,typeof(string)),
                                                     new DataColumn(SECUserSchema.PERSONID,typeof(string)),
                                                     new DataColumn(PSNAccountSchema.PSNACCOUNT_TRUENAME,typeof(string)),
                                                     new DataColumn(PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE,typeof(int)),
                                                     new DataColumn(PSNAccountSchema.PSNACCOUNT_BRANCHID,typeof(string)),
                                                     new DataColumn(PSNAccountSchema.PSNACCOUNT_EMPLOYEEID,typeof(string)),
                                                     new DataColumn(ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME,typeof(string)),
                                                     new DataColumn(ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE,typeof(string)),
                                                     new DataColumn("ISUSERLOCK",typeof(string)),
                                                     new DataColumn("USERACCSTATE",typeof(string)),
            });
            this.UlUserGrid.DataSource = dt;
            this.UlUserGrid.DataBind();
        }


        protected void InitGrid()
        {
            CommonMethod.AddFlexField(this.UlUserGrid, "", SECUserSchema.USERID, 0, true);
            CommonMethod.AddFlexField(this.UlUserGrid, base.getAlert("ZGAIA00024"), SECUserSchema.LOGINNAME, 15, false);
            CommonMethod.AddFlexField(this.UlUserGrid, base.getAlert("ZGAIA00010"), ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE, 15, false);
            CommonMethod.AddFlexField(this.UlUserGrid, base.getAlert("ZGAIA00023"), ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME, 15, false);
            CommonMethod.AddFlexField(this.UlUserGrid, base.getAlert("ZGAIA00036"), PSNAccountSchema.PSNACCOUNT_EMPLOYEEID, 15, false);
            CommonMethod.AddFlexField(this.UlUserGrid, base.getAlert("ZGAIA00035"), PSNAccountSchema.PSNACCOUNT_TRUENAME, 15, false);
            CommonMethod.AddFlexField(this.UlUserGrid, base.getAlert("ZGAIA00034"), "USERACCSTATE", 15, false);
            CommonMethod.AddFlexField(this.UlUserGrid, base.getAlert("ZGAIA00033"), "ISUSERLOCK", 10, false);
            CommonMethod.AddFlexField(this.UlUserGrid, base.getAlert("ZGAIA00032"), SECUserSchema.STARTDATE, 15, false);
            CommonMethod.AddFlexField(this.UlUserGrid, base.getAlert("ZGAIA00031"), SECUserSchema.ENDDATE, 15, false);
            CommonMethod.AddFlexField(this.UlUserGrid, "", PSNAccountSchema.PSNACCOUNT_BRANCHID, 0, true);
            CommonMethod.AddFlexField(this.UlUserGrid, "", SECUserSchema.PERSONID, 0, true);
            CommonMethod.AddLinkButtonField(this.UlUserGrid, base.getAlert("ZGAIA00044"), "", 40, !this.CheckFunAccess(UserSecFunID), false, "", CheerUI.IconFont.Wrench).CommandName = "SECURITY";
           // CommonMethod.AddLinkButtonField(this.UlUserGrid, base.getAlert("ZGAIA00017"), "", 40, false, false, "", CheerUI.IconFont.Copy).CommandName = "COPY";
            //CommonMethod.AddWindowField(this.UlUserGrid, base.getAlert("ZGAIA00017"), "COPY", 40, "", "", "", "");
            CommonMethod.AddWindowField(this.UlUserGrid, base.getString("ZGAIA00089"), "EDIT", 50, "UserTabPage", SECUserSchema.USERID, "UserTabPage.aspx?ISADD=NO&USERID={0}", base.getString("ZGAIA00256"));
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string selectwherestr = "1=1";
            string loginname = this.txtLoginName.Text.Trim().DBReplace();
            string name = this.txtName.Text.Trim().DBReplace();
            string employeeid = this.txtEmployeeid.Text.Trim().DBReplace();
            string rolename = this.txtRoleName.Text.Trim().DBReplace();
            string accstate = this.drpAccStates.SelectedValue.Trim();
            string islock = this.drpLock.SelectedValue.Trim();
            string deptselectstr = (this.StdBranchLoader as StdBranchLoader).GetSelectedBranchSQLStr();
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
            QueryData querydata = new QueryData();
            querydata.QuerySql = selectwherestr;
            querydata.PageIndex = 1;
            CheerUI.ListItem deptitem = (this.StdBranchLoader as StdBranchLoader).GetSelectBranchItem();
            querydata.DeptID = deptitem.Value;
            querydata.EmployeeID = employeeid;
            querydata.IsLock = islock;
            querydata.LoginName = loginname;
            querydata.PsnName = name;
            querydata.RoleName = rolename;
            querydata.StateStr = accstate;
            querydata.IncludeSub = (this.StdBranchLoader as StdBranchLoader).IsIncludeSubBranch;
            Session["QUERYSQLSTR"] = querydata;
            this.getgriddata(true, 0);
        }

        private void SetQueryData(QueryData data)
        {
            this.txtEmployeeid.Text = data.EmployeeID;
            this.txtLoginName.Text = data.LoginName;
            this.txtName.Text = data.PsnName;
            this.txtRoleName.Text = data.RoleName;
            this.drpAccStates.SelectedValue = data.StateStr;
            this.drpLock.SelectedValue = data.IsLock;
            (this.StdBranchLoader as StdBranchLoader).SetSelectBranch(data.DeptID);
            (this.StdBranchLoader as StdBranchLoader).IsIncludeSubBranch = data.IncludeSub;
        }

        private void getgriddata(bool isAlert, int index)
        {
            if (Session["QUERYSQLSTR"] != null)
            {
                QueryData querydata = (QueryData)Session["QUERYSQLSTR"];
                this.SetQueryData(querydata);
                string selectstr = querydata.QuerySql;
                int pageindex = querydata.PageIndex;
                if (selectstr.Trim() == "")
                {
                    this.init_grid();
                    return;
                }
                User usermanager = (User)base.GetPalauObject(typeof(User), UserQueryFunID, true, "", true, base.getBusinessUnitID());
                DataSet userds = usermanager.GetAllUserInforWithSecurity(selectstr, false);
                if (userds.Tables.Count > 0)
                {
                    if (userds.Tables[0].Rows.Count > 0)
                    {
                        DataSet newuserds = this.replacedscolumndata(userds);
                        newuserds.Tables[0].DefaultView.Sort = SECUserSchema.LOGINNAME + " ASC";
                        ViewState["newuserds"] = newuserds.Tables[0];
                        UlUserGrid.RecordCount = newuserds.Tables[0].Rows.Count;
                        LoadData(GetPagedDataTable(index, UlUserGrid.PageSize, newuserds.Tables[0]));
                    }
                    else
                    {
                        this.init_grid();
                    }
                }
                if (this.UlUserGrid.Rows.Count == 0 && isAlert)
                {
                    base.ShowAlert(base.getString("ZGAIA03512"));
                }
            }
            else
            {
                this.init_grid();

            }
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
                        row["ISUSERLOCK"] = base.getString("ZGAIA00785");
                    else if (islock == "1")
                        row["ISUSERLOCK"] = base.getString("ZGAIA00254");
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
                accstatestr = base.getAlert("ZGAIA00037");
            if (accstate == (int)AccessionStatus.Export)
                accstatestr = base.getAlert("ZGAIA00030");
            if (accstate == (int)AccessionStatus.Probation)
                accstatestr = base.getAlert("ZGAIA00029");
            if (accstate == (int)AccessionStatus.PromotingProbation)
                accstatestr = base.getAlert("ZGAIA00028");
            if (accstate == (int)AccessionStatus.Regular)
                accstatestr = base.getAlert("ZGAIA00027");
            if (accstate == (int)AccessionStatus.Retired)
                accstatestr = base.getAlert("ZGAIA00026");
            if (accstate == (int)AccessionStatus.Unchecked)
                accstatestr = base.getAlert("ZGAIA00025");
            return accstatestr;
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
            this.UlUserGrid.DataSource = dt;
            this.UlUserGrid.DataBind();
        }

        protected void UlUserGrid_PageIndexChange(object sender, CheerUI.GridPageEventArgs e)
        {
            this.UlUserGrid.PageIndex = e.NewPageIndex;
            getgriddata(true, e.NewPageIndex);
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UlUserGrid.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            getgriddata(true, this.UlUserGrid.PageIndex);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ArrayList delidlist = new ArrayList();
            int[] rows = this.UlUserGrid.SelectedRowIndexArray;
            foreach (int row in rows)
            {
                string userid = this.UlUserGrid.Rows[row].Cells.FromKey(SECUserSchema.USERID).ToString().Trim();
                delidlist.Add(userid);
            }
            if (delidlist.Count > 0)
            {
                User usermanager = (User)eHRPageServer.GetPalauObject(typeof(User));
                usermanager.DeleteUserList(delidlist);
                this.getgriddata(false, 0);
                base.ShowAlert(base.getString("ZGAIA00940"));
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = ViewState["newuserds"] as DataTable;
            if (dt == null)
            {
                base.ShowAlert(base.getString("ZGAIA03512"));
                return;
            }
            if (dt.Columns.Contains("PERSONID"))
            {
                dt.Columns.Remove("PERSONID");
            }
            if (dt.Columns.Contains("BUSINESSUNITID"))
            {
                dt.Columns.Remove("BUSINESSUNITID");
            }
            if (dt.Columns.Contains("ISLOCK"))
            {
                dt.Columns.Remove("ISLOCK");
            }
            if (dt.Columns.Contains("ACCESSIONSTATE"))
            {
                dt.Columns.Remove("ACCESSIONSTATE");
            }
            if (dt.Columns.Contains("BRANCHID"))
            {
                dt.Columns.Remove("BRANCHID");
            }

            dt.Columns.Add("ROLENAME");//在dt里面新增一列数据，列名为：包含角色

            User user = new User();
            PersistBroker broker = PersistBroker.Instance();
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)//根据Usurid循环遍历 用户角色表 取出数据 赋值给 包含角色列
                {
                    string USERID = dt.Rows[i][0].ToString();

                    DataTable dt2 = user.AddContainsTheRoleExprot(USERID.Trim(), broker);
                    if (dt2.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {

                            dt.Rows[i]["ROLENAME"] += dt2.Rows[j][0].ToString();
                            if (j != dt2.Rows.Count - 1)
                            {
                                dt.Rows[i]["ROLENAME"] += ",";//每个角色用逗号隔开
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                base.eHRThrow_Sys(ex);
            }
            finally
            {
                broker.Close();
            }

            if (dt.Columns.Contains("USERID"))
            {
                dt.Columns.Remove("USERID");
            }
            DataTable datatable = new DataTable();
            datatable.Columns.Add(base.getString("ZGAIA00238"));
            datatable.Columns.Add(base.getString("ZGAIA00261"));
            datatable.Columns.Add(base.getString("ZGAIA01617"));
            datatable.Columns.Add(base.getString("ZGAIA00209"));
            datatable.Columns.Add(base.getString("ZGAIA00208"));
            datatable.Columns.Add(base.getString("ZGAIA00241"));
            datatable.Columns.Add(base.getString("ZGAIA00239"));
            datatable.Columns.Add(base.getString("ZGAIA00148"));
            datatable.Columns.Add(base.getString("ZGAIA00581"));
            datatable.Columns.Add(base.getString("ZGAIA06132"));

            dt.Columns["LOGINNAME"].ColumnName = base.getString("ZGAIA00238");
            dt.Columns["UNITNAME"].ColumnName = base.getString("ZGAIA01617");
            dt.Columns["UNITCODE"].ColumnName = base.getString("ZGAIA00261");
            dt.Columns["TRUENAME"].ColumnName = base.getString("ZGAIA00208");
            dt.Columns["EMPLOYEEID"].ColumnName = base.getString("ZGAIA00209");
            dt.Columns["ISUSERLOCK"].ColumnName = base.getString("ZGAIA00239");
            dt.Columns["USERACCSTATE"].ColumnName = base.getString("ZGAIA00241");
            dt.Columns["STARTDATE"].ColumnName = base.getString("ZGAIA00148");
            dt.Columns["ENDDATE"].ColumnName = base.getString("ZGAIA00581");
            dt.Columns["ROLENAME"].ColumnName = base.getString("ZGAIA06132");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                datatable.Rows.Add();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        if (datatable.Columns[j].ColumnName == dt.Columns[k].ColumnName)
                        {

                            if (!(dt.Rows[i][k] == DBNull.Value))
                            {
                                datatable.Rows[i][j] = dt.Rows[i][k].ToString();
                            }
                        }
                    }
                }
            }

            if (dt != null)
            {
                GridDataExportor _exportor = new GridDataExportor(this.Request);
                DataSet ds = new DataSet();
                ds.Tables.Add(datatable);
                string _scriptStr = _exportor.ExportExcelData("USERROLE" + DateTime.Now.ToString("yyyyMMdd"), ds);
                CheerUI.PageContext.RegisterStartupScript(_scriptStr);
            }
        }

        protected void UlUserGrid_RowCommand(object sender, CheerUI.GridCommandEventArgs e)
        {
            string userid = UlUserGrid.Rows[e.RowIndex].Cells.FromKey(SECUserSchema.USERID).ToString().Trim();
            if (e.CommandName == "SECURITY")
            {
                if (haverightonsec(userid))
                {
                    CheerUI.PageContext.Redirect("../SecuritySet/UserSecMaintainPage.aspx?USERID=" + userid + "&FROMURL=USER");
                }
                else
                {
                    base.ShowAlert(base.getAlert("ZGAIA00042"));
                }
            }
            if (e.CommandName == "COPY")
            {
                if (haverightonsec(userid))
                    CheerUI.PageContext.Redirect("../SuperWork/UserCopyToUserPage.aspx?UserID=" + userid + "&FROMURL=USER");
                else
                {
                    base.ShowAlert(base.getAlert("ZGAIA00042"));
                }
            }
        }

        bool haverightonsec(string userID)
        {
            if (base.GetSecurityChecker().UserID.ToUpper() == "SA")
                return true;
            User usermanager = (User)base.GetPalauObject(typeof(User), "0090000500010005", true, "", true, base.getBusinessUnitID());
            DataSet userds = usermanager.GetAllUserInforWithSecurity("", false);
            string filter = SECUserSchema.USERID + "='" + userID + "'";
            if (userds.Tables[0].Select(filter).Length > 0)
                return true;
            else
                return false;
        }
    }
}