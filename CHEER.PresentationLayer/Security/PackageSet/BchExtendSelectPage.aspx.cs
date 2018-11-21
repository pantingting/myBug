using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CheerUI;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.CommonLayer.eSecurity;
using CHEER.CommonLayer.eSecurity.Data;
using CHEER.CommonLayer.eSecurity.Schema;
using CHEER.PresentationLayer;

namespace HRONE.PresentationLayer.Security.PackageSet
{
    public partial class BchExtendSelectPage : CHEERBasePage
    {
        private string PackageID
        {
            get { return (string)ViewState["PackageID"]; }
            set { ViewState["PackageID"] = value; }
        }
        private string DimensionID
        {
            get { return (string)ViewState["DimensionID"]; }
            set { ViewState["DimensionID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.CheckAccess();
            if (!IsPostBack)
            {
                this.InitFace();
                this.GetTransData();
                this.LoadData();
            }
        }
        private void CheckAccess()
        {
            base.CheckIsLoginOut();
        }
        private void InitFace()
        {
            this.cmdReturn.Text = "关闭";
            this.cmdConfirm.Text = "确定";
        }
        private void GetTransData()
        {
            if (Request.QueryString["DIMENSIONID"] != null)
                this.DimensionID = Request.QueryString["DIMENSIONID"].Trim();
            else
                this.DimensionID = "";
            if (Request.QueryString["PACKAGEID"] != null)
                this.PackageID = Request.QueryString["PACKAGEID"].Trim();
            else
                this.PackageID = "";
        }

        private void LoadData()
        {
            SecurityDimensionLoader dimmanager = (SecurityDimensionLoader)eHRPageServer.GetPalauObject(typeof(SecurityDimensionLoader));
            Hashtable selecthash = dimmanager.GetSelectItemInfor(this.DimensionID.Trim());
            SecurityDimensionItemLoader itemmanager = (SecurityDimensionItemLoader)eHRPageServer.GetPalauObject(typeof(SecurityDimensionItemLoader));
            DataSet itemds = itemmanager.GetDimItemValueInfor(this.DimensionID.Trim(), this.PackageID.Trim());
            DataTable dt = new DataTable();
            dt.Columns.Add("KEY");
            dt.Columns.Add("VALUE");
            var rowIndex = 0;
            List<int> selectedIndexs = new List<int>();
            foreach(string key in selecthash.Keys)
            {
                DataRow dr = dt.NewRow();
                dr["KEY"] = key;
                dr["VALUE"] = selecthash[key].ToString().Trim();
                dt.Rows.Add(dr);
                for (int i = 0; i < itemds.Tables[0].Rows.Count; i++)
                {
                    var val = itemds.Tables[0].Rows[i]["DIMITEMVALUE"].ToString();
                    if (val == dr["KEY"].ToString())
                    {
                        selectedIndexs.Add(rowIndex);
                        break;
                    }
                }
                rowIndex++;
            }
            this.grdMain.DataSource = dt;
            this.grdMain.DataBind();
            this.grdMain.SelectedRowIndexArray = selectedIndexs.ToArray();
        }

        protected void cmdConfirm_Click(object sender, EventArgs e)
        {
            ArrayList itemlist = new ArrayList();
            foreach (var index in this.grdMain.SelectedRowIndexArray)
            {
                itemlist.Add(this.grdMain.Rows[index].Cells.FromKey("KEY").ToString());
            }
            SecurityDimensionItemLoader itemmanager = (SecurityDimensionItemLoader)eHRPageServer.GetPalauObject(typeof(SecurityDimensionItemLoader));
            itemmanager.SaveDimItemList(itemlist, this.PackageID.Trim(), this.DimensionID.Trim(), DimensionItemType.Common, true);
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected void cmdReturn_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
    }
}