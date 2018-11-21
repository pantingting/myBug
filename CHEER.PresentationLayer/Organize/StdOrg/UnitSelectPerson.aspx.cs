using CheerUI;
using CHEER.BusinessLayer.Organize;
using CHEER.BusinessLayer.Security;
using CHEER.CommonLayer.ePersonnel.Schema;
using CHEER.PresentationLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CHEER.PresentationLayer.Organize.StdOrg
{
    public partial class UnitSelectPerson : CHEERBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitLau();
                InitDropDownList();
            }
        }

        protected void InitDropDownList()
        {
            ddlOrg.Items.Add(new CheerUI.ListItem("", ""));
            SecurityChecker ck = base.GetSecurityChecker();
            DataRow[] drcoll = ck.GetBranchsDatas("020060030", true, base.getBusinessUnitID()).Tables[0].Select(null, null);
            foreach (DataRow dr in drcoll)
            {
                ddlOrg.Items.Add(new CheerUI.ListItem(dr["UNITCODE"].ToString() + "\t" + dr["UNITNAME"].ToString()
                    , dr["UNITID"].ToString()));
            }
        }

        protected void InitLau()
        {
            txtNo.Label = base.getString("ZGAIA00209");
            txtName.Label = base.getString("ZGAIA00208");
            ddlOrg.Label = base.getString("ZGAIA00479");
            btnConfirm.Text = base.getString("ZGAIA00228");
            btnCancel.Text = base.getString("ZGAIA00031");
            grdMain.Columns.FromKey("EMPLOYEEID").HeaderText = getString("ZGAIA00209");
            grdMain.Columns.FromKey("TRUENAME").HeaderText = getString("ZGAIA00208");
            btnQuery.Text = base.getString("ZGAIA00196");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHideReference());
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            var selectedIndex = grdMain.SelectedRowIndex;
            if (selectedIndex < 0)
            {
                CheerUI.Alert.ShowInTop(getAlert("ZGAIA00735"));
                return;
            }
            var row = grdMain.Rows[selectedIndex];
            PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(row.Cells.FromKey("EMPLOYEEID").ToString(), row.Cells.FromKey("TRUENAME").ToString(), row.Cells.FromKey("PERSONID").ToString()) + ActiveWindow.GetHideReference());
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (txtNo.Text.Trim() == "" && txtName.Text.Trim() == "" && ddlOrg.SelectedValue == "")
            {
                CheerUI.Alert.ShowInTop(base.getAlert("ZGAIA00499"));
                return;
            }

            STDUnitManager stdUnitManager = (STDUnitManager)base.GetPalauObject(typeof(STDUnitManager), "020060030", true, "", false, "");
            DataSet ds = stdUnitManager.GetPersonByEmproeeAndNameAndBranchID(txtNo.Text, txtName.Text, ddlOrg.SelectedValue);

            grdMain.DataSource = ds.Tables[0];
            grdMain.DataBind();
        }

        protected void grdMain_RowDoubleClick(object sender, GridRowClickEventArgs e)
        {
            var selectedIndex = grdMain.SelectedRowIndex;
            var row = grdMain.Rows[selectedIndex];
            PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(row.Cells.FromKey("EMPLOYEEID").ToString(), row.Cells.FromKey("TRUENAME").ToString(), row.Cells.FromKey("PERSONID").ToString()) + ActiveWindow.GetHideReference());
        }
    }
}