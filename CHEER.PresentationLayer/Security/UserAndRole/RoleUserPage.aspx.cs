using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.Common.Schema;
using CHEER.CommonLayer.ePersonnel.Schema;
using CHEER.CommonLayer.eSecurity.Schema;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;
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
    public partial class RoleUserPage : CHEERBasePage
    {
        private string RoleID
        {
            get { return (string)ViewState["RoleID"]; }
            set { ViewState["RoleID"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.init_face();
            this.get_transdata();
            this.load_data(1);
        }

        protected void Page_Init()
        {
            CommonMethod.AddField(UlRoleGrid, "", SECUserSchema.USERID, 0, true);
            CommonMethod.AddField(UlRoleGrid, "登录名", SECUserSchema.LOGINNAME, 100, false);
            //CommonMethod.AddField(UlRoleGrid, "组织代码", ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE, 100, false);
            //CommonMethod.AddField(UlRoleGrid, "登陆名", ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME, 100, false);
            //CommonMethod.AddField(UlRoleGrid, "工号", PSNAccountSchema.PSNACCOUNT_EMPLOYEEID, 100, false);
            CommonMethod.AddField(UlRoleGrid, "姓名", PSNAccountSchema.PSNACCOUNT_TRUENAME, 100, false);
            CommonMethod.AddField(UlRoleGrid, "在职状态", SECUserSchema.STARTDATE, 100, false);
            CommonMethod.AddField(UlRoleGrid, "是否停用", "ISUSERLOCK", 100, false);
            CommonMethod.AddField(UlRoleGrid, "在职状态", "USERACCSTATE", 100, false);
            CommonMethod.AddField(UlRoleGrid, "生效日期", SECUserSchema.STARTDATE, 100, false);
            CommonMethod.AddField(UlRoleGrid, "失效日期", SECUserSchema.ENDDATE, 100, false);
            CommonMethod.AddField(UlRoleGrid, "", PSNAccountSchema.PSNACCOUNT_BRANCHID, 0, true);
            CommonMethod.AddField(UlRoleGrid, "", SECUserSchema.PERSONID, 0, true);
            CommonMethod.AddField(UlRoleGrid, "", PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE, 0, true);
            CommonMethod.AddField(UlRoleGrid, "", SECUserSchema.ISLOCK, 0, true);
        }

        private void init_face()
        {
            this.btnAdd.Text = "新增";
            this.btnDelete.Text = "删除";
        }
        private void get_transdata()
        {
            if (Request.QueryString["ROLEID"] != null)
                this.RoleID = Request.QueryString["ROLEID"].Trim();
            else
                this.RoleID = "";
            this.txtRoleID.Text = this.RoleID.Trim();
        }

        private void load_data(int pageindex)
        {
            if (this.RoleID.Trim() != "")
            {
                string roleid = this.RoleID.Trim();
                UserRoleMapManager mapmanager = (UserRoleMapManager)eHRPageServer.GetPalauObject(typeof(UserRoleMapManager));
                DataSet mapds = mapmanager.GetUserInforByRoleID(roleid);
                if (mapds.Tables[0].Rows.Count > 0)
                {
                    DataSet newmapds = this.replacedscolumndata(mapds);
                    newmapds.Tables[0].DefaultView.Sort = SECUserSchema.LOGINNAME + " ASC";
                    this.UlRoleGrid.DataSource = newmapds.Tables[0].DefaultView;
                    this.UlRoleGrid.DataBind();
                }
                else {
                    this.UlRoleGrid.DataSource = new DataTable();
                    this.UlRoleGrid.DataBind();
                }
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

        protected void UlRoleGrid_PageIndexChange(object sender, CheerUI.GridPageEventArgs e)
        {
            this.UlRoleGrid.PageIndex = e.NewPageIndex;
            load_data(e.NewPageIndex);
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            load_data(UlRoleGrid.PageIndex);
        }

        private List<int> GetSortedArray(int[] value)
        {
            List<int> list = new List<int>();
            if (value != null)
            {
                list.AddRange(value);
                list.Sort();
            }
            return list;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            
                ArrayList delidlist = new ArrayList();
                var selectedIndexs = UlRoleGrid.SelectedRowIndexArray;
                if (selectedIndexs.Length == 0)
                {
                    selectedIndexs = CheerUI.StringUtil.GetIntListFromString(this.selectedRowIndexArray.Value, true).ToArray();
                }
                foreach (var index in selectedIndexs)
                {
                    var row = UlRoleGrid.Rows[index];
                    string userid = row.Cells.FromKey(SECUserSchema.USERID).ToString().Trim();
                    delidlist.Add(userid);
                }
                if (delidlist.Count > 0)
                {
                    UserRoleMapManager mapmanager = (UserRoleMapManager)eHRPageServer.GetPalauObject(typeof(UserRoleMapManager));
                    mapmanager.DeleteRelationList(delidlist, this.RoleID.Trim(), 0);
                    this.load_data(UlRoleGrid.PageIndex);
                    base.ShowAlert("删除成功!");
                }
            
            
        }
    }
}