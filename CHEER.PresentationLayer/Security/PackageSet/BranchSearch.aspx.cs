using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.Organize;
using CHEER.Common.Schema;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;

namespace CHEER.PresentationLayer.Security.PackageSet
{
    public partial class BranchSearch : CHEERBasePage
    {
        private string SecurityID
        {
            get { return (string)ViewState["SecurityID"]; }
            set { ViewState["SecurityID"] = value; }
        }
        private string IsIncludeStopUnit
        {
            get { return (string)ViewState["IsIncludeStopUnit"]; }
            set { ViewState["IsIncludeStopUnit"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.InitFace();
                this.get_transdata();
                this.init_grid();
            }
        }
        private void InitFace()
        {
            this.txtDepName.Label = "组织名称";
            this.txtDepCode.Label = "组织代码";
            this.cmdSearch.Text = "查询";
        }

        private void init_grid()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {  new DataColumn(ORGStdStructSchema.ORGSTDSTRUCT_UNITID,typeof(string)),
													  new DataColumn(ORGStdStructSchema.ORGSTDSTRUCT_PUNITID,typeof(string)),
													  new DataColumn(ORGStdStructSchema.ORGSTDSTRUCT_LABEL,typeof(string)),
													 new DataColumn(ORGStdStructSchema.ORGSTDSTRUCT_LABELLENGTH,typeof(string)),
													 new DataColumn(ORGStdStructSchema.ORGSTDSTRUCT_LABELINDEX,typeof(string)),
													  new DataColumn(ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME,typeof(string)),
													  new DataColumn(ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE,typeof(string)),
													 new DataColumn("PUNITNAME",typeof(string)),
													 new DataColumn("PUNITCODE",typeof(string))
			});
            this.UltDepGrid.DataSource = dt;
            this.UltDepGrid.DataBind();
        }
        private void get_transdata()
        {
            if (Request.QueryString["SECURITYID"] != null)
                this.SecurityID = Request.QueryString["SECURITYID"].Trim();
            else
                this.SecurityID = "";
            if (Request.QueryString["INCLUDESTOPUNIT"] != null)
                this.IsIncludeStopUnit = Request.QueryString["INCLUDESTOPUNIT"].Trim();
            else
                this.IsIncludeStopUnit = "";
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            InitGrid();
        }

        protected void InitGrid()
        {
            CommonMethod.AddFlexField(this.UltDepGrid, "", ORGStdStructSchema.ORGSTDSTRUCT_UNITID, 0, true);
            CommonMethod.AddFlexField(this.UltDepGrid, "组织代码", ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE, 25, false);
            CommonMethod.AddFlexField(this.UltDepGrid, "组织名称", ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME, 25, false);
            CommonMethod.AddFlexField(this.UltDepGrid, "父组织代码", "PUNITCODE", 25, false);
            CommonMethod.AddFlexField(this.UltDepGrid, "父组织名称", "PUNITNAME", 25, false);
            CommonMethod.AddFlexField(this.UltDepGrid, "", ORGStdStructSchema.ORGSTDSTRUCT_PUNITID, 0, true);
            CommonMethod.AddFlexField(this.UltDepGrid, "", ORGStdStructSchema.ORGSTDSTRUCT_LABEL, 0, true);
            CommonMethod.AddFlexField(this.UltDepGrid, "", ORGStdStructSchema.ORGSTDSTRUCT_LABELLENGTH, 0, true);
            CommonMethod.AddFlexField(this.UltDepGrid, "", ORGStdStructSchema.ORGSTDSTRUCT_LABELINDEX, 0, true);
        }

        private void load_data(string depcode, string depname, string securityid)
        {
            bool isIncludeStopUnit = false;
            if (this.IsIncludeStopUnit == "True")
            {
                isIncludeStopUnit = true;
            }
            STDUnitManager unitmanager = (STDUnitManager)base.GetPalauObject(typeof(STDUnitManager), securityid, false, "", false, "", isIncludeStopUnit);
            DataSet datads = unitmanager.GetSecUnitDsForControlWithSecurity(depname, depcode);
            if (!this.ifdsisnotnull(datads))
            {
                this.init_grid();
                base.ShowAlert("没有查询到符合条件的数据！");
            }
            else
            {
                this.UltDepGrid.DataSource = datads.Tables[0].DefaultView;
                this.UltDepGrid.DataBind();
            }
        }
        private bool ifdsisnotnull(DataSet ds)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                    return true;
            }
            return false;
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            this.load_data(this.txtDepCode.Text.Trim(), this.txtDepName.Text.Trim(), this.SecurityID.Trim());
        }
    }
}