using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;
using CHEER.PresentationLayer.CommonPage;

namespace CHEER.PresentationLayer.Security
{
    public partial class AllotLookPage : CHEERBasePage
    {
        string stroper
        {
            get { return ViewState["stroper"].ToString(); }
            set { ViewState["stroper"] = value; }
        }
        DateTime timeoper
        {
            get { return (DateTime)ViewState["timeoper"]; }
            set { ViewState["timeoper"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.cmdAdd.Text = "导出";
                this.cmdDelete.Text = "返回";
                setrequestvalue();
                this.LoadDada(0);
                ViewState["From"] = Request.QueryString["FromURL"];
            }
        }
        void setrequestvalue()
        {
            this.stroper = Request.QueryString["stroper"];
            this.timeoper = DataProcessor.StringToDateTime(Request.QueryString["timeoper"]);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            InitGrid();
        }

        private void InitGrid()
        {
            CommonMethod.AddFlexField(this.UlRoleGrid, "组织名称", "UNITNAME", 15, false);
            CommonMethod.AddFlexField(this.UlRoleGrid, "工号", "EMPLOYEEID", 15, false);
            CommonMethod.AddFlexField(this.UlRoleGrid, "登录帐号", "LOGINNAME", 15, false);
            CommonMethod.AddFlexField(this.UlRoleGrid, "生效时间", "STARTDATE", 15, false);
            CommonMethod.AddFlexField(this.UlRoleGrid, "失效时间", "ENDDATE", 15, false);
            CommonMethod.AddFlexField(this.UlRoleGrid, "创建日期", "BUILDDATE", 15, false);
        }

        private void LoadDada(int index)
        {
            AccountAllot allotmgr = (AccountAllot)eHRPageServer.GetPalauObject(typeof(AccountAllot));
            DataSet ds = allotmgr.getAllotData(stroper, timeoper);
            UlRoleGrid.RecordCount = ds.Tables[0].Rows.Count;
            LoadData(GetPagedDataTable(index, UlRoleGrid.PageSize, ds.Tables[0]));
        }
        private void LoadData(DataTable dt)
        {
            this.UlRoleGrid.DataSource = dt;
            this.UlRoleGrid.DataBind();
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

        protected void UlRoleGrid_PageIndexChange(object sender, CheerUI.GridPageEventArgs e)
        {
            UlRoleGrid.PageIndex = e.NewPageIndex;
            LoadDada(e.NewPageIndex);
        }

        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            AccountAllot allotmgr = (AccountAllot)eHRPageServer.GetPalauObject(typeof(AccountAllot));
            DataSet ds = allotmgr.getAllotData(stroper, timeoper);
            ExcelDataExport _excelData = new ExcelDataExport();
            string _scriptStr = _excelData.ExportExcelData("AllotInformation", this.UlRoleGrid, ds);
            base.Script(_scriptStr);
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            CheerUI.PageContext.Redirect(ViewState["From"].ToString());   
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            UlRoleGrid.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            LoadDada(this.UlRoleGrid.PageIndex);
        }
    }
}