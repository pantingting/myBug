using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.CommonLayer.eSecurity;
using CHEER.CommonLayer.eSecurity.Schema;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;

namespace CHEER.PresentationLayer.Security.SecuritySet
{
    public partial class SecFunInforPage : CHEERBasePage
    {
        private string FunPointID
        {
            get { return (string)ViewState["FunPointID"]; }
            set { ViewState["FunPointID"] = value; }
        }
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.InitFace();
                this.GetTransData();
                this.GetSecData();
            }
        }
        private void InitFace()
        {
            this.cmdConfirm.Text = "确定";
        }

        private void GetTransData()
        {
            if (Request.QueryString["FUNPOINTID"] != null)
                this.FunPointID = Request.QueryString["FUNPOINTID"].Trim();
            else
                this.FunPointID = "";
            if (Request.QueryString["MANID"] != null)
                this.ManID = Request.QueryString["MANID"].Trim();
            else
                this.ManID = "";
            if (Request.QueryString["MANTYPE"] != null)
                this.ManType = Request.QueryString["MANTYPE"].Trim();
            else
                this.ManType = "";
        }

        private void Init_Grid()
        {
            DataTable dt = new DataTable();
            
            this.UlFunSecGrid.DataSource = dt;
            this.UlFunSecGrid.DataBind();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            InitGrid();
        }

        protected void InitGrid()
        {
            CommonMethod.AddFlexField(this.UlFunSecGrid, "范围包名称", SECScopePackageSchema.PACKAGENAME, 25, false);
            CommonMethod.AddFlexField(this.UlFunSecGrid, "维度名称", SECDimensionSchema.DIMENSIONNAME, 25, false);
            CommonMethod.AddFlexField(this.UlFunSecGrid, "维度值", SECDimensionItemSchema.DIMITEMVALUE, 25, false);
            CommonMethod.AddFlexField(this.UlFunSecGrid, "", SECDimensionItemSchema.DIMITEMTYPE, 0, true);
        }

        private void GetSecData()
        {
            int inttype = 0;
            if (this.ManType.Trim() != "")
                inttype = Convert.ToInt32(this.ManType.Trim());
            //
            SecurityPackageMap mapmanager = (SecurityPackageMap)eHRPageServer.GetPalauObject(typeof(SecurityPackageMap));
            DataSet funsecds = mapmanager.GetFunSecInforofMan(this.ManID.Trim(), inttype, this.FunPointID.Trim());
            if (funsecds.Tables.Count > 0)
            {
                DataSet secds = this.ReplaceDsColumnData(funsecds);
                this.UlFunSecGrid.DataSource = secds.Tables[0].DefaultView;
                this.UlFunSecGrid.DataBind();
            }
            else
                this.Init_Grid();
        }

        private DataSet ReplaceDsColumnData(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                int includesubdept = (int)DimensionItemType.IncludeSubOrg;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (!row.IsNull(SECDimensionItemSchema.DIMITEMTYPE) && row[SECDimensionItemSchema.DIMITEMTYPE].ToString().Trim() != "")
                    {
                        int type = Convert.ToInt32(row[SECDimensionItemSchema.DIMITEMTYPE].ToString().Trim());
                        if (type == includesubdept)
                            row[SECDimensionItemSchema.DIMITEMVALUE] = row[SECDimensionItemSchema.DIMITEMVALUE].ToString().Trim() + base.getAlert("ZGAIA00213");
                    }
                }
            }
            return ds;
        }

        protected void cmdConfirm_Click(object sender, EventArgs e)
        {
            CheerUI.PageContext.RegisterStartupScript(CheerUI.ActiveWindow.GetHideRefreshReference());
        }
    }
}