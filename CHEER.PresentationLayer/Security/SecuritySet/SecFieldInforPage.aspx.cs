using System;
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
    public partial class SecFieldInforPage : CHEERBasePage
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
            this.UlFieldSecGrid.DataSource = dt;
            this.UlFieldSecGrid.DataBind();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            InitGrid();
        }

        protected void InitGrid()
        {
            CommonMethod.AddFlexField(this.UlFieldSecGrid, "字段名称", SECFieldSchema.FIELDNAME, 25, false);
            CommonMethod.AddFlexField(this.UlFieldSecGrid, "字段编码", SECFieldSchema.FIELDID, 25, false);
            CommonMethod.AddFlexField(this.UlFieldSecGrid, "排序值", SECFieldSchema.FIELDORDER, 25, false);
            CommonMethod.AddFlexField(this.UlFieldSecGrid, "字段描述", SECFieldSchema.FIELDDESC, 25, false);
            CommonMethod.AddFlexField(this.UlFieldSecGrid, "", SECFieldSchema.FIELDDICTIONARYID, 0, true);
        }

        private void GetSecData()
        {
            int inttype = 0;
            if (this.ManType.Trim() != "")
                inttype = Convert.ToInt32(this.ManType.Trim());
            //
            FieldMapManager mapmanager = (FieldMapManager)eHRPageServer.GetPalauObject(typeof(FieldMapManager));
            DataSet secfieldds = mapmanager.GetFieldInforofMan(this.ManID.Trim(), inttype, this.FunPointID.Trim(), 1);
            if (secfieldds.Tables.Count > 0)
            {
                this.UlFieldSecGrid.DataSource = secfieldds.Tables[0].DefaultView;
                this.UlFieldSecGrid.DataBind();
            }
            else
                this.Init_Grid();
        }

        protected void cmdConfirm_Click(object sender, EventArgs e)
        {
            CheerUI.PageContext.RegisterStartupScript(CheerUI.ActiveWindow.GetHideRefreshReference());
        }
    }
}