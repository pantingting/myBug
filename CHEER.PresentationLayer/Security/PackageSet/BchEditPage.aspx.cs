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
using CHEER.CommonLayer.eSecurity;
using CHEER.CommonLayer.eSecurity.Schema;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;

namespace CHEER.PresentationLayer.Security.PackageSet
{
    public partial class BchEditPage : CHEERBasePage
    {
        private string PackageID
        {
            get { return (string)ViewState["PackageID"]; }
            set { ViewState["PackageID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.InitFace();
                this.GetTransData();
                this.LoadData(0);
            }
        }

        private void InitFace()
        {

            this.cmdAdd.Text = "新 增";
            this.cmdDelete.Text = "删 除";
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
                this.txtRightID.Text = Request.QueryString["RIGHTID"].Trim();
            else
                this.txtRightID.Text = "";
        }
        private void Init_Grid()
        {
            UlSecPersonGrid.DataSource = new DataTable();
            UlSecPersonGrid.DataBind();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            InitGrid();
        }
        protected void InitGrid()
        {
            CommonMethod.AddFlexField(this.UlSecPersonGrid, "", SECDimensionItemSchema.DIMITEMID, 0, true);
            CommonMethod.AddFlexField(this.UlSecPersonGrid, "组织代码", ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE, 15, false);
            CommonMethod.AddFlexField(this.UlSecPersonGrid, "组织名称", ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME, 15, false);
            CommonMethod.AddFlexField(this.UlSecPersonGrid, "是否包含子组织", "INCLUDESUB", 15, false);
            CommonMethod.AddFlexField(this.UlSecPersonGrid, "", SECDimensionItemSchema.DIMITEMTYPE, 0, true);
        
        }

        private void LoadData(int pageindex)
        {
            if (this.PackageID.Trim() != "")
            {
                SecurityDimensionItemLoader itemmanager = (SecurityDimensionItemLoader)eHRPageServer.GetPalauObject(typeof(SecurityDimensionItemLoader));
                DataSet branchdimds = itemmanager.GetBranchDimensionInfor(this.PackageID.Trim());
                if (branchdimds.Tables.Count > 0)
                {
                    if (branchdimds.Tables[0].Rows.Count > 0)
                    {
                        DataSet repds = this.ReplaceDsData(branchdimds);
                        this.UlSecPersonGrid.RecordCount = repds.Tables[0].Rows.Count;
                        LoadData(GetPagedDataTable(pageindex, UlSecPersonGrid.PageSize, repds.Tables[0]));
                    }
                    else
                    {
                        this.UlSecPersonGrid.RecordCount = 0;
                        LoadData(new DataTable());
                    }
                }
            }
            else
            {
                Init_Grid();
            }
        }

        protected void LoadData(DataTable dt)
        {
            this.UlSecPersonGrid.DataSource = dt;
            this.UlSecPersonGrid.DataBind();
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
        private DataSet ReplaceDsData(DataSet ds)
        {
            ds.Tables[0].Columns.Add("INCLUDESUB");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (!row.IsNull(SECDimensionItemSchema.DIMITEMTYPE) && row[SECDimensionItemSchema.DIMITEMTYPE].ToString().Trim() != "")
                {
                    int itemtype = Convert.ToInt32(row[SECDimensionItemSchema.DIMITEMTYPE].ToString().Trim());
                    if (itemtype == (int)DimensionItemType.IncludeSubOrg)
                        row["INCLUDESUB"] = "是";
                    if (itemtype == (int)DimensionItemType.IncludeSubOrgInManageUnit)
                        row["INCLUDESUB"] = "是(当前管理单元)";
                    if (itemtype == (int)DimensionItemType.Common)
                        row["INCLUDESUB"] = "否";
                }
                else
                    row["INCLUDESUB"] = "";
            }
            return ds;
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
                this.LoadData(0);
                base.ShowAlert("删除成功!");
            }
        }

        protected void BranchDisTab_Close(object sender, CheerUI.WindowCloseEventArgs e)
        {
            this.LoadData(0);
        }
    }
}