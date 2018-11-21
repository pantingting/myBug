using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.Common.Schema;
using CHEER.CommonLayer.ePersonnel.Schema;
using CHEER.CommonLayer.eSecurity.Schema;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;

namespace CHEER.PresentationLayer.Security.PackageSet
{
    public partial class PersonMaintainPage : CHEERBasePage
    {
        private string PackageID
        {
            get { return (string)ViewState["PackageID"]; }
            set { ViewState["PackageID"] = value; }
        }
        private int PageIndex
        {
            get { return (int)ViewState["PageIndex"]; }
            set { ViewState["PageIndex"] = value; }
        }
        private string RightID
        {
            get { return (string)ViewState["RightID"]; }
            set { ViewState["RightID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.CheckAccess();
            if (!IsPostBack)
            {
                this.PageIndex = 1;
                this.InitFace();
                this.GetTransData();
                this.LoadData(0);
            }
        }
        private void CheckAccess()
        {
            base.CheckIsLoginOut();
        }
        private void InitFace()
        {
            this.cmdAdd.Text = "新增";
            this.cmdDelete.Text = "删除";
            cmdDelete.ConfirmText = "确认删除？";
        }
        private void GetTransData()
        {
            if (Request.QueryString["PACKAGEID"] != null)
                this.PackageID = Request.QueryString["PACKAGEID"].Trim();
            else
                this.PackageID = "";
            this.txtPackageID.Text = PackageID.Trim();
            if (Request.QueryString["RIGHTID"] != null)
                this.RightID = Request.QueryString["RIGHTID"].Trim();
            else
                this.RightID = "";
            this.txtRightID.Text = this.RightID.Trim();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            init_Grid();
        }
        protected void init_Grid()
        {
            CommonMethod.AddFlexField(this.UlSecPersonGrid, "", SECDimensionItemSchema.DIMITEMID, 0, true);
            CommonMethod.AddFlexField(this.UlSecPersonGrid, "组织代码", ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE, 20, false);
            CommonMethod.AddFlexField(this.UlSecPersonGrid, "组织", ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME, 20, false);
            CommonMethod.AddFlexField(this.UlSecPersonGrid, "工号", PSNAccountSchema.PSNACCOUNT_EMPLOYEEID, 20, false);
            CommonMethod.AddFlexField(this.UlSecPersonGrid, "姓名", PSNAccountSchema.PSNACCOUNT_TRUENAME, 20, false);
            CommonMethod.AddFlexField(this.UlSecPersonGrid, "在职状态", "ACCSTATUS", 20, false);
            CommonMethod.AddFlexField(this.UlSecPersonGrid, "", PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE, 0, true);
        }

        private void InitGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {  new DataColumn(SECDimensionItemSchema.DIMITEMID,typeof(string)),
													 new DataColumn(PSNAccountSchema.PSNACCOUNT_EMPLOYEEID,typeof(string)),
													 new DataColumn(PSNAccountSchema.PSNACCOUNT_TRUENAME,typeof(string)),
													 new DataColumn(PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE,typeof(int)),
													 new DataColumn(ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME,typeof(string)),
													 new DataColumn(ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE,typeof(string)),
													new DataColumn("ACCSTATUS",typeof(int)),
			});
            this.UlSecPersonGrid.DataSource = dt;
            this.UlSecPersonGrid.DataBind();
        }
        private DataSet ReplaceDsData(DataSet ds)
        {
            ds.Tables[0].Columns.Add("ACCSTATUS");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (!row.IsNull(PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE) && row[PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE].ToString().Trim() != "")
                {
                    int accstate = Convert.ToInt32(row[PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE].ToString().Trim());
                    row["ACCSTATUS"] = this.getaccessionstate(accstate);
                }
                else
                    row["ACCSTATUS"] = "";
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

        private void LoadData(int pageindex)
        {
            this.UlSecPersonGrid.RecordCount = 0;
            if (this.PackageID.Trim() != "")
            {
                SecurityDimensionItemLoader itemmanager = (SecurityDimensionItemLoader)eHRPageServer.GetPalauObject(typeof(SecurityDimensionItemLoader));
                DataSet psndimds = itemmanager.GetPersonDimensionInfor(this.PackageID.Trim());
                if (psndimds.Tables.Count > 0)
                {
                    if (psndimds.Tables[0].Rows.Count > 0)
                    {
                        this.UlSecPersonGrid.RecordCount = psndimds.Tables[0].Rows.Count;
                        DataSet repds = this.ReplaceDsData(psndimds);
                        this.LoadData(GetPagedDataTable(pageindex, UlSecPersonGrid.PageSize, repds.Tables[0]));
                        return;
                    }
                }
            }
            this.LoadData(new DataTable());
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
            this.UlSecPersonGrid.DataSource = dt;
            this.UlSecPersonGrid.DataBind();
        }

        protected void UlSecPersonGrid_PageIndexChange(object sender, CheerUI.GridPageEventArgs e)
        {
            this.UlSecPersonGrid.PageIndex = e.NewPageIndex;
            LoadData(e.NewPageIndex);
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UlSecPersonGrid.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            LoadData(this.UlSecPersonGrid.PageIndex);
        }

        protected void MultiPersonSelect_Close(object sender, CheerUI.WindowCloseEventArgs e)
        {
            LoadData(0);
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            ArrayList delidlist = new ArrayList();
            int[] rows = UlSecPersonGrid.SelectedRowIndexArray;
            foreach (int row in rows)
            {
                string roleid = UlSecPersonGrid.Rows[row].Cells.FromKey(SECDimensionItemSchema.DIMITEMID).ToString().Trim();
                delidlist.Add(roleid);
            }
            if (delidlist.Count > 0)
            {
                SecurityDimensionItemLoader itemmanager = (SecurityDimensionItemLoader)eHRPageServer.GetPalauObject(typeof(SecurityDimensionItemLoader));
                itemmanager.DeleteDimensionListofPackage(delidlist, this.PackageID.Trim());
                this.LoadData(this.PageIndex);
                base.ShowAlert("删除成功!");
            }
        }
    }
}