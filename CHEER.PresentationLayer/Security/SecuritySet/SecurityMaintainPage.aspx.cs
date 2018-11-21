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
using CHEER.CommonLayer.eSecurity.Schema;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;

namespace CHEER.PresentationLayer.Security.SecuritySet
{
    public partial class SecurityMaintainPage : CHEERBasePage
    {
        private string ManID
        {
            get { return (string)ViewState["ManID"]; }
            set { ViewState["ManID"] = value; }
        }
        private string ManType
        {
            get { return (string)ViewState["ManType"]; }
            set { ViewState["ManType"] = value; }
        }
        private string secpack = "0090000500020005";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                setenabled();
                this.InitFace();
                this.GetTransData();
                this.LoadData(0);
            }
        }
        void setenabled()
        {
            if (base.GetSecurityChecker().IsAllow(secpack))
            {
                this.cmdAdd.Enabled = true;
            }
            else
            {
                this.cmdAdd.Enabled = false;
            }
            if (base.GetSecurityChecker().IsAllow("0090000500020005"))
            {
                this.cmdDelete.Enabled = true;
            }
            else
            {
                this.cmdDelete.Enabled = false;
            }
        }

        private void InitFace()
        {
            this.cmdAdd.Text = "新增";
            this.cmdDelete.Text = "删除";
        }
        private void GetTransData()
        {
            if (Request.QueryString["MANID"] != null)
                this.ManID = Request.QueryString["MANID"].Trim();
            else
                this.ManID = "";
            this.txtManID.Text = this.ManID.Trim();
            if (Request.QueryString["MANTYPE"] != null)
                this.ManType = Request.QueryString["MANTYPE"].Trim();
            else
                this.ManType = "";
            this.txtManType.Text = this.ManType.Trim();
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            InitGrid();
        }
        protected void InitGrid()
        {
            CommonMethod.AddFlexField(this.UlPackageGrid, "", SECPackageMapSchema.PACKAGEID, 0, true);
            CommonMethod.AddFlexField(this.UlPackageGrid, "范围包名称", SECScopePackageSchema.PACKAGENAME, 15, false);
            CommonMethod.AddFlexField(this.UlPackageGrid, "范围包描述", SECScopePackageSchema.PACKAGEDESC, 20, false);
            CommonMethod.AddLinkButtonField(this.UlPackageGrid, "权限设定", "", 80, false, false, "", CheerUI.IconFont.Wrench).CommandName = "SECURITY";
            CommonMethod.AddLinkButtonField(this.UlPackageGrid, "详细", "", 45, false, false, "", CheerUI.IconFont.Edit).CommandName = "EDIT";
        }

        private void LoadData(int pageindex)
        {
            int inttype = 0;
            if (this.ManType.Trim() != "")
                inttype = Convert.ToInt32(this.ManType.Trim());
            SecurityScopePackageLoader packagemanager = (SecurityScopePackageLoader)eHRPageServer.GetPalauObject(typeof(SecurityScopePackageLoader));
            DataSet fieldds = packagemanager.GetPackageInforofMan(this.ManID.Trim(), inttype);
            this.UlPackageGrid.RecordCount = 0;
            if (fieldds.Tables.Count > 0)
            {
                this.UlPackageGrid.RecordCount = fieldds.Tables[0].Rows.Count;
                LoadData(GetPagedDataTable(pageindex, this.UlPackageGrid.PageSize, fieldds.Tables[0]));
            }
            else
                LoadData(new DataTable());
        }

        protected void LoadData(DataTable dt)
        {
            this.UlPackageGrid.DataSource = dt;
            this.UlPackageGrid.DataBind();
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

        protected void UlPackageGrid_PageIndexChange(object sender, CheerUI.GridPageEventArgs e)
        {
            this.UlPackageGrid.PageIndex = e.NewPageIndex;
            LoadData(e.NewPageIndex);
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UlPackageGrid.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            LoadData(UlPackageGrid.PageIndex);
        }

        protected void UlPackageGrid_RowCommand(object sender, CheerUI.GridCommandEventArgs e)
        {
            string packageid = UlPackageGrid.Rows[e.RowIndex].Cells.FromKey(SECPackageMapSchema.PACKAGEID).ToString().Trim();
            if (e.CommandName == "EDIT")
            {
                CheerUI.PageContext.RegisterStartupScript("showDetil('" + packageid + "')");
            }
            if (e.CommandName == "SECURITY")
            {
                CheerUI.PageContext.RegisterStartupScript("showSecurity('" + packageid + "')");
            }
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            ArrayList delidlist = new ArrayList();
            int[] rows = UlPackageGrid.SelectedRowIndexArray;
            foreach (int row in rows)
            {
                string roleid = UlPackageGrid.Rows[row].Cells.FromKey(SECPackageMapSchema.PACKAGEID).ToString().Trim();
                delidlist.Add(roleid);
            }
            if (delidlist.Count > 0)
            {
                SecurityScopePackageLoader packagemanager = (SecurityScopePackageLoader)eHRPageServer.GetPalauObject(typeof(SecurityScopePackageLoader));
                packagemanager.DeleteByIDList(delidlist);
                this.LoadData(0);
                base.ShowAlert("删除成功!");
            }
        }

        protected void SetSecuityMainPage_Close(object sender, CheerUI.WindowCloseEventArgs e)
        {
            this.LoadData(0);
        }
    }
}