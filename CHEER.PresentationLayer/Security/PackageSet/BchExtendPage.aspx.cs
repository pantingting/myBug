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

namespace CHEER.PresentationLayer.Security.PackageSet
{
    public partial class BchExtendPage : CHEERBasePage
    {
        private string PackageID
        {
            get { return (string)ViewState["PackageID"]; }
            set { ViewState["PackageID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.CheckAccess();
            if (!IsPostBack)
            {
                this.GetTransData();
                this.Init_Grid();
                this.LoadPSNAbstractData();
            }
        }
        private void CheckAccess()
        {
            base.CheckIsLoginOut();
        }
        private void GetTransData()
        {
            if (Request.QueryString["PACKAGEID"] != null)
                this.PackageID = Request.QueryString["PACKAGEID"].Trim();
            else
                this.PackageID = "";
            this.txtPackageID.Text = this.PackageID.Trim();
        }
        private void Init_Grid()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {  new DataColumn(SECDimensionSchema.DIMENSIONID,typeof(string)),
													 new DataColumn(SECDimensionSchema.DIMENSIONNAME,typeof(string)),
													 new DataColumn(SECDimensionSchema.DIMENSIONITEM1,typeof(int)),
													 new DataColumn(SECDimensionSchema.SELECTSQLSTR,typeof(string)),
													 new DataColumn(SECDimensionSchema.SELECTID,typeof(string)),
													 new DataColumn(SECDimensionSchema.SELECTNAME,typeof(string)),
													 new DataColumn("DIMITEMVALUES",typeof(string)),
			});
            this.UlAbstractGrid.DataSource = dt;
            this.UlAbstractGrid.DataBind();
        }
        private void LoadPSNAbstractData()
        {
            SecurityDimensionLoader dimmanager = (SecurityDimensionLoader)eHRPageServer.GetPalauObject(typeof(SecurityDimensionLoader));
            DataSet psnextds = dimmanager.GetAllExtendInfor(true);
            if (psnextds.Tables.Count > 0)
            {
                if (psnextds.Tables[0].Rows.Count > 0)
                {
                    ArrayList dimidlist = new ArrayList();
                    foreach (DataRow row in psnextds.Tables[0].Rows)
                    {

                        string dimensionid = row[SECDimensionSchema.DIMENSIONID].ToString().Trim();
                        dimidlist.Add(dimensionid);
                    }
                    SecurityDimensionItemLoader itemmanager = (SecurityDimensionItemLoader)eHRPageServer.GetPalauObject(typeof(SecurityDimensionItemLoader));
                    Hashtable itemnameshash = itemmanager.GetDimItemValueNames(dimidlist, this.PackageID.Trim());
                    psnextds.Tables[0].Columns.Add("DIMITEMVALUES");
                    foreach (DataRow row in psnextds.Tables[0].Rows)
                    {
                        string dimensionid = row[SECDimensionSchema.DIMENSIONID].ToString().Trim();
                        if (itemnameshash.Contains(dimensionid))
                            row["DIMITEMVALUES"] = itemnameshash[dimensionid].ToString().Trim();
                        else
                            row["DIMITEMVALUES"] = "";
                    }
                    if (psnextds.Tables[0].Rows.Count >= 8)
                        this.UlAbstractGrid.PageSize = psnextds.Tables[0].Rows.Count + 2;
                    else
                        this.UlAbstractGrid.PageSize = 10;
                    this.UlAbstractGrid.DataSource = psnextds.Tables[0].DefaultView;
                    this.UlAbstractGrid.DataBind();
                    return;
                }
            }
            this.Init_Grid();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            InitGrid();
        }

        protected void InitGrid()
        {
            CommonMethod.AddFlexField(this.UlAbstractGrid, "", SECDimensionSchema.DIMENSIONID, 0, true);
            CommonMethod.AddFlexField(this.UlAbstractGrid, "维度名称", SECDimensionSchema.DIMENSIONNAME, 50, false);
            CommonMethod.AddFlexField(this.UlAbstractGrid, "维度值", "DIMITEMVALUES", 44, false);
            CommonMethod.AddFlexField(this.UlAbstractGrid, "", SECDimensionSchema.DIMENSIONITEM1, 0, true);
            CommonMethod.AddFlexField(this.UlAbstractGrid, "", SECDimensionSchema.SELECTSQLSTR, 0, true);
            CommonMethod.AddFlexField(this.UlAbstractGrid, "", SECDimensionSchema.SELECTID, 0, true);
            CommonMethod.AddFlexField(this.UlAbstractGrid, "", SECDimensionSchema.SELECTNAME, 0, true);
            CommonMethod.AddLinkButtonField(this.UlAbstractGrid, "详细", "EDIT", 40, false, false, "", CheerUI.IconFont.Edit).CommandName = "edit";
        }

        protected void UlAbstractGrid_RowCommand(object sender, CheerUI.GridCommandEventArgs e)
        {
            string url = UlAbstractGrid.Rows[e.RowIndex].Cells.FromKey(SECDimensionSchema.DIMENSIONITEM1).ToString().Trim();
            string dimid = UlAbstractGrid.Rows[e.RowIndex].Cells.FromKey(SECDimensionSchema.DIMENSIONID).ToString().Trim();
            if (e.CommandName == "edit")
            {
                CheerUI.PageContext.RegisterStartupScript("showDetil('" + url + "','" + dimid + "')");
            }
        }

        protected void windowMain_Close(object sender, CheerUI.WindowCloseEventArgs e)
        {
            LoadPSNAbstractData();
        }
    }
}