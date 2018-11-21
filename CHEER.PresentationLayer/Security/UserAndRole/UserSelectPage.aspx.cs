using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.Common.Schema;
using CHEER.CommonLayer.ePersonnel.Schema;
using CHEER.CommonLayer.eSecurity.Schema;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;
using CHEER.Platform.DAL;
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
    public partial class UserSelectPage : CHEERBasePage
    {
        private const string RoleUserFunID = "010020020040";//"010020010050";//"010020020040";
        private string ManageMenuid = "'900110030','050202001005','050202001004','020060030040','030070020','030070030','900010010001','900010020'";
        private string QuerySQLStr
        {
            get { return (string)ViewState["QuerySQLStr"]; }
            set { ViewState["QuerySQLStr"] = value; }
        }
        private string RoleID
        {
            get { return (string)ViewState["RoleID"]; }
            set { ViewState["RoleID"] = value; }
        }

        private void get_transdata()
        {
            if (Request.QueryString["ROLEID"] != null)
                this.RoleID = Request.QueryString["ROLEID"].Trim();
            else
                this.RoleID = "";
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

            (this.StdBranchLoader as StdBranchLoader).IsHaveManageUnit = true;
            (this.StdBranchLoader as StdBranchLoader).LoadBranchDtsByID(base.GetSecurityChecker(), RoleUserFunID);
        }

        protected void Page_Init()
        {
            CommonMethod.AddField(UlUserGrid, "", SECUserSchema.USERID, 0, true);
            CommonMethod.AddField(UlUserGrid, "登录名", SECUserSchema.LOGINNAME, 100, false);
            //CommonMethod.AddField(UlUserGrid, "组织代码", ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE, 100, false);
            //CommonMethod.AddField(UlUserGrid, "登陆名", ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME, 100, false);
            //CommonMethod.AddField(UlUserGrid, "工号", PSNAccountSchema.PSNACCOUNT_EMPLOYEEID, 100, false);
            CommonMethod.AddField(UlUserGrid, "姓名", PSNAccountSchema.PSNACCOUNT_TRUENAME, 100, false);
            CommonMethod.AddField(UlUserGrid, "在职状态", SECUserSchema.STARTDATE, 100, false);
            CommonMethod.AddField(UlUserGrid, "是否停用", "ISUSERLOCK", 100, false);
            CommonMethod.AddField(UlUserGrid, "在职状态", "USERACCSTATE", 100, false);
            CommonMethod.AddField(UlUserGrid, "生效日期", SECUserSchema.STARTDATE, 100, false);
            CommonMethod.AddField(UlUserGrid, "失效日期", SECUserSchema.ENDDATE, 100, false);
            CommonMethod.AddField(UlUserGrid, "", PSNAccountSchema.PSNACCOUNT_BRANCHID, 0, true);
            CommonMethod.AddField(UlUserGrid, "", SECUserSchema.PERSONID, 0, true);
            CommonMethod.AddField(UlUserGrid, "", PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE, 0, true);
            CommonMethod.AddField(UlUserGrid, "", SECUserSchema.ISLOCK, 0, true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Init_Lau();
                this.get_transdata();
                this.init_dropdownlist();
                this.btnClose.OnClientClick = CheerUI.ActiveWindow.GetHidePostBackReference();
            }
        }

        void Init_Lau()
        {
            this.txtLoginName.Label = "登录名";
            this.txtEmployeeid.Label = "工号";
            this.txtName.Label = "姓名";
            this.txtRoleName.Label = "角色名称";
            this.drpAccState.Label = "在职状态";
            this.drpLock.Label = "是否停用";
            (this.StdBranchLoader as StdBranchLoader).Label = "归属组织";
            this.btnQuery.Text = "查询";
            this.btnSave.Text = "确定";
            this.btnClose.Text = "取消";
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string selectwherestr = "1=1";
            string loginname = this.txtLoginName.Text.Trim().DBReplace();
            string name = this.txtName.Text.Trim().DBReplace();
            string employeeid = this.txtEmployeeid.Text.Trim().DBReplace();
            string rolename = this.txtRoleName.Text.Trim().DBReplace();
            string accstate = this.drpAccState.SelectedValue.Trim();
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
            if (this.RoleID.Trim() != "")
                selectwherestr += " and " + SECUserSchema.SECUSER_TABLE + "." + SECUserSchema.USERID + " not in ( select " +
                    SECRoleUserMapSchema.USERID + " from " + SECRoleUserMapSchema.SECROLEUSERMAP_TABLE +
                    " where " + SECRoleUserMapSchema.ROLEID + "='" + this.RoleID.Trim() + "')";
            this.QuerySQLStr = selectwherestr;
            this.load_data(this.UlUserGrid.PageIndex, true);
        }

        private void load_data(int pageindex, bool isAlert)
        {
            if (this.QuerySQLStr != null && this.QuerySQLStr.Trim() != "")
            {
                User usermanager = (User)base.GetPalauObject(typeof(User), RoleUserFunID, true, "", true, base.getBusinessUnitID());
                DataSet userds = usermanager.GetAllUserInforWithSecurity(this.QuerySQLStr.Trim(), false);
                if (userds.Tables.Count > 0)
                {
                    if (userds.Tables[0].Rows.Count > 0)
                    {
                        DataSet newuserds = this.replacedscolumndata(userds);
                        newuserds.Tables[0].DefaultView.Sort = SECUserSchema.LOGINNAME + " ASC";
                        this.UlUserGrid.DataSource = newuserds.Tables[0].DefaultView;
                        this.UlUserGrid.DataBind();
                        return;
                    }
                }
                else {
                    this.UlUserGrid.DataSource = new DataTable();
                    this.UlUserGrid.DataBind();
                }
            }
            if (this.UlUserGrid.Rows.Count == 0 && isAlert)
            {
                base.ShowAlert("没有查询到符合条件的数据！");
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

        private bool swAddManage(int addnum, out string MSG)
        {
            //Dim 增加判断是否新增的人员属于主管
            MSG = "";
            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ArrayList addidlist = new ArrayList();
            var selectedIndexs = UlUserGrid.SelectedRowIndexArray;
            foreach (var index in selectedIndexs)
            {
                var row = UlUserGrid.Rows[index];
                string userid = row.Cells.FromKey(SECUserSchema.USERID).ToString().Trim();
                addidlist.Add(userid);
            }
            if (addidlist.Count > 0)
            {
                string MSG = "";
                if (swAddManage(addidlist.Count, out MSG))
                {
                    UserRoleMapManager mapmanager = (UserRoleMapManager)eHRPageServer.GetPalauObject(typeof(UserRoleMapManager));
                    mapmanager.InsertUserRoleRelation(addidlist, this.RoleID.Trim(), 0);
                    this.load_data(this.UlUserGrid.PageIndex, false);
                    base.ShowAlert("保存成功!");
                }
                else
                {
                    base.ShowAlert(MSG);
                }
            }
        }

        protected void UlUserGrid_PageIndexChange(object sender, CheerUI.GridPageEventArgs e)
        {
            this.UlUserGrid.PageIndex = e.NewPageIndex;
            load_data(this.UlUserGrid.PageIndex, false);
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UlUserGrid.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            load_data(this.UlUserGrid.PageIndex, false);
        }
    }
}