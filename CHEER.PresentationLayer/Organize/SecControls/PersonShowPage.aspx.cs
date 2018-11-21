using CheerUI;
using CHEER.BusinessLayer.ePersonnel.PersonnelBaseInfo;
using CHEER.Common;
using CHEER.Common.Schema;
using CHEER.CommonLayer.ePersonnel.Schema;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;
using CHEER.PresentationLayer.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CHEER.PresentationLayer.Organize.SecControls
{
    public partial class PersonShowPage : CHEERBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                ViewState["menuID"] = "123a";
                if (Request.QueryString["RightID"] != null && Request.QueryString["RightID"].ToString() != "")
                {

                    ViewState["menuID"] = Request.QueryString["RightID"].ToString();
                }

                InitData();

                InitLau();

                (StdBranchLoader as StdBranchLoader).IsHaveManageUnit = true;
                (StdBranchLoader as StdBranchLoader).LoadBranchDtsByID(base.GetSecurityChecker(), ViewState["menuID"].ToString());

                AddRenderderFunction();
            }
        }

        protected void AddRenderderFunction() 
        {
            var empState = grdMain.Columns.FromKey(PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE) as CheerUI.RenderField;
            empState.RendererFunction = @"
                                    function(value){
                                        return C('" + ddlStateInfo.ClientID + @"').getTextByValue(value);
                                    }
                                ";
        }

        protected void InitLau()
        {
            txtEmpNo.Label = base.getString("ZGAIA00209");
            txtEmpName.Label = base.getString("ZGAIA00208");
            ddlSex.Label = base.getString("ZGAIA00307");
            (StdBranchLoader as StdBranchLoader).Label = base.getString("ZGAIA00479");
            ddlState.Label = base.getString("ZGAIA00241");
            btnQuery.Text = base.getString("ZGAIA00196");
        }

        protected void InitData()
        {
            ddlState.Items.Add(new CheerUI.ListItem("", ""));
            CheerUI.ListItem lt = new CheerUI.ListItem(base.getString("ZGAIA00565"), ((int)AccessionStatus.Probation).ToString());//"试用"
            ddlState.Items.Add(lt);
            lt = new CheerUI.ListItem(base.getString("ZGAIA01151"), ((int)AccessionStatus.PromotingProbation).ToString());//"晋级试用"
            ddlState.Items.Add(lt);
            lt = new CheerUI.ListItem(base.getString("ZGAIA00564"), ((int)AccessionStatus.Regular).ToString());//"正式"
            ddlState.Items.Add(lt);

            ddlSex.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA00313"),//"女"
                ((int)Gender.Female).ToString()));
            ddlSex.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA00309"),//"男"
                ((int)Gender.Male).ToString()));
            ddlSex.Items.Insert(0, new CheerUI.ListItem("", ((int)Gender.Unset).ToString()));

            ddlStateInfo.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA01150"), ((int)AccessionStatus.Dimission).ToString()));
            ddlStateInfo.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA01152"), ((int)AccessionStatus.Export).ToString()));
            ddlStateInfo.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA00565"), ((int)AccessionStatus.Probation).ToString()));
            ddlStateInfo.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA01151"), ((int)AccessionStatus.PromotingProbation).ToString()));
            ddlStateInfo.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA00564"), ((int)AccessionStatus.Regular).ToString()));
            ddlStateInfo.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA01149"), ((int)AccessionStatus.Retired).ToString()));
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            InitGrid();
        }

        protected void InitGrid()
        {
            CommonMethod.AddField(grdMain, "ID", PSNAccountSchema.PSNACCOUNT_PERSONID, 0, true);
            CommonMethod.AddFlexRendererField(grdMain, base.getString("ZGAIA00261"), ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE, 30, "", false);
            CommonMethod.AddFlexRendererField(grdMain, base.getString("ZGAIA00479"), ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME, 30, "", false);
            CommonMethod.AddFlexRendererField(grdMain, base.getString("ZGAIA00209"), PSNAccountSchema.PSNACCOUNT_EMPLOYEEID, 15, "", false);
            CommonMethod.AddFlexRendererField(grdMain, base.getString("ZGAIA00208"), PSNAccountSchema.PSNACCOUNT_TRUENAME, 15, "", false);
            CommonMethod.AddFlexRendererField(grdMain, base.getString("ZGAIA00241"), PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE, 15, "", false).Editor.Add(ddlStateInfo);
        }

        private CheerUI.DropDownList ddlStateInfo = new CheerUI.DropDownList();

        void PersonShowPage_SelectBranchChange(object sender, StdBranchLoader.SelectBranchChangeEventArgs e)
        {
            CheerUI.Alert.ShowInTop(e.NewBranchInfo.Value);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            LoadData(0);
        }

        private SelectedData getSessionSQL()
        {
            SelectedData data = new SelectedData();
            data.DeptID = (StdBranchLoader as StdBranchLoader).GetSelectedBranchSQLStr();
            if (!string.IsNullOrEmpty(ddlSex.SelectedValue))
            {
                data.intSex = int.Parse(ddlSex.SelectedValue);
            }
            if (!string.IsNullOrEmpty(ddlState.SelectedValue))
            {
                data.intStatus = int.Parse(ddlState.SelectedValue);
            }
            if (!string.IsNullOrEmpty(txtEmpNo.Text))
            {
                data.Wno = txtEmpNo.Text;
            }
            if (!string.IsNullOrEmpty(txtEmpName.Text))
            {
                data.Name = txtEmpName.Text;
            }
            return data;
        }

        private DataTable GetPagedDataTable(int pageIndex, int pageSize,DataTable dtSource)
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

        private void LoadData(int pageindex)
        {
            string Wno = "";
            string Name = "";
            int intSex = 0;
            string DeptID = "";
            int intStatus = 0;
            SelectedData data = getSessionSQL();
            Wno = data.Wno;
            Name = data.Name;
            intSex = data.intSex;
            DeptID = data.DeptID;
            intStatus = data.intStatus;
            string accounttb = PSNAccountSchema.PSNACCOUNT_TABLENAME;
            string stdtb = ORGStdStructSchema.ORGSTDSTRUCT_TABLE;
            string selectSQL = "select distinct " + accounttb + "." + PSNAccountSchema.PSNACCOUNT_PERSONID + "," +
                PSNAccountSchema.PSNACCOUNT_TRUENAME + "," + PSNAccountSchema.PSNACCOUNT_EMPLOYEEID + "," +
                PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE + "," + ORGStdStructSchema.ORGSTDSTRUCT_TABLE + "." + ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME + "," +
                ORGStdStructSchema.ORGSTDSTRUCT_TABLE + "." + ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE + "," +
                ORGStdStructSchema.ORGSTDSTRUCT_TABLE + "." + ORGStdStructSchema.ORGSTDSTRUCT_LABELINDEX +
                " from " + accounttb +
                " left join " + stdtb + " on " + accounttb + "." + PSNAccountSchema.PSNACCOUNT_BRANCHID +
                "=" + stdtb + "." + ORGStdStructSchema.ORGSTDSTRUCT_UNITID +
                " where 1=1 ";

            if (Wno != "")
                selectSQL += " and " + PSNAccountSchema.PSNACCOUNT_EMPLOYEEID + " like '%" + CHEER.Common.DataProcessor.FormatString(Wno) + "%'";
            if (Name != "")
                selectSQL += " and " + PSNAccountSchema.PSNACCOUNT_TRUENAME + " like '%" + CHEER.Common.DataProcessor.FormatString(Name) + "%'";
            if (intSex != 0)
                selectSQL += " and " + PSNAccountSchema.PSNACCOUNT_GENDER + "=" + intSex.ToString().Trim();
            if (DeptID != "")
                selectSQL += " and " + DeptID;
            if (intStatus != 0)
                selectSQL += " and " + PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE + "=" + intStatus.ToString().Trim();
            else
                selectSQL += " and " + PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE + " in (1,2,6)";
            selectSQL += " order by " + ORGStdStructSchema.ORGSTDSTRUCT_TABLE + "." + ORGStdStructSchema.ORGSTDSTRUCT_LABELINDEX + " asc , " + PSNAccountSchema.PSNACCOUNT_TABLENAME + "." + PSNAccountSchema.PSNACCOUNT_EMPLOYEEID + " asc";
            PersonManager personmanager = (PersonManager)base.GetPalauObject(typeof(PersonManager), ViewState["menuID"].ToString(), true, "", true, base.getBusinessUnitID());
            DataTable _dt = personmanager.GetPersonsDstWithSecurity(selectSQL).Tables[0];
            if (_dt.Rows.Count > 0)
            {
                grdMain.RecordCount = _dt.Rows.Count;
                LoadData(GetPagedDataTable(pageindex, grdMain.PageSize, _dt));
                return;
            }
            CheerUI.Alert.ShowInTop(base.getString("ZGAIA03512"));

            LoadData(new DataTable());
        }

        protected void LoadData(DataTable dt)
        {
            grdMain.DataSource = dt;
            grdMain.DataBind();
        }

        protected void grdMain_PageIndexChange(object sender, CheerUI.GridPageEventArgs e)
        {
            grdMain.PageIndex = e.NewPageIndex;
            LoadData(e.NewPageIndex);
        }

        [Serializable]
        private class SelectedData
        {
            private string oWno = "";
            private string oName = "";
            private int ointSex = 0;
            private string oDeptID = "";
            private int ointStatus = 0;
            public string Wno
            {
                get { return oWno; }
                set { oWno = value; }
            }
            public string Name
            {
                get { return oName; }
                set { oName = value; }
            }
            public int intSex
            {
                get { return ointSex; }
                set { ointSex = value; }
            }
            public string DeptID
            {
                get { return oDeptID; }
                set { oDeptID = value; }
            }
            public int intStatus
            {
                get { return ointStatus; }
                set { ointStatus = value; }
            }
        }

        protected void grdMain_RowDoubleClick(object sender, CheerUI.GridRowClickEventArgs e)
        {
            var selectedIndex = grdMain.SelectedRowIndex;
            var row = grdMain.Rows[selectedIndex];
            PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(row.Cells.FromKey(PSNAccountSchema.PSNACCOUNT_TRUENAME).ToString(), row.Cells.FromKey(PSNAccountSchema.PSNACCOUNT_PERSONID).ToString()) + ActiveWindow.GetHideReference());
        }
    }
}