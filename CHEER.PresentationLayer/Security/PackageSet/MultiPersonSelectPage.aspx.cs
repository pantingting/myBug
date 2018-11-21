using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.ePersonnel.PersonnelBaseInfo;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.Common.Schema;
using CHEER.CommonLayer.ePersonnel.Schema;
using CHEER.CommonLayer.eSecurity;
using CHEER.CommonLayer.eSecurity.Schema;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;
using CHEER.PresentationLayer.Controls;

namespace CHEER.PresentationLayer.Security.PackageSet
{
    public partial class MultiPersonSelectPage : CHEERBasePage
    {
        private string PackageID
        {
            get { return (string)ViewState["PackageID"]; }
            set { ViewState["PackageID"] = value; }
        }
        private string RightID
        {
            get { return (string)ViewState["RightID"]; }
            set { ViewState["RightID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.GetTransData();
                InitFace();
                this.LoadDropDownList();
                InitGrid();
            }
        }

        private void InitFace()
        {
            txtWno.Label = "工号";
            txtName.Label = "姓名";
            dltSex.Label = "性别";
            (StdBranchLoader as StdBranchLoader).Label = "组织";
            ddlState.Label = "在职状态";
            cmdSeach.Text = "查询";
            cmdOK.Text = "确定";
            cmdCancel.Text = "取消";
            (StdBranchLoader as StdBranchLoader).FunSecurityID = this.RightID.Trim();
        }
        private void GetTransData()
        {
            if (Request.QueryString["PACKAGEID"] != null)
                this.PackageID = Request.QueryString["PACKAGEID"].Trim();
            else
                this.PackageID = "";
            if (Request.QueryString["RIGHTID"] != null)
                this.RightID = Request.QueryString["RIGHTID"].Trim();
            else
                this.RightID = "";
        }
        private void LoadDropDownList()
        {
            (StdBranchLoader as StdBranchLoader).IsHaveManageUnit = false;
            (StdBranchLoader as StdBranchLoader).IsShowIncludeStopUnit = false;
            (StdBranchLoader as StdBranchLoader).LoadBranchDtsByID(base.GetSecurityChecker(), this.RightID.Trim());//"030070050020");
            this.ddlState.Items.Add(new CheerUI.ListItem("", ""));
            CheerUI.ListItem lt = new CheerUI.ListItem("离职", ((int)AccessionStatus.Dimission).ToString());//"离职"
            this.ddlState.Items.Add(lt);
            lt = new CheerUI.ListItem("调出", ((int)AccessionStatus.Export).ToString());//"调出"
            this.ddlState.Items.Add(lt);
            lt = new CheerUI.ListItem("试用", ((int)AccessionStatus.Probation).ToString());//"试用"
            this.ddlState.Items.Add(lt);
            lt = new CheerUI.ListItem("晋级试用", ((int)AccessionStatus.PromotingProbation).ToString());//"晋级试用"
            this.ddlState.Items.Add(lt);
            lt = new CheerUI.ListItem("正式", ((int)AccessionStatus.Regular).ToString());//"正式"
            this.ddlState.Items.Add(lt);
            lt = new CheerUI.ListItem("退休", ((int)AccessionStatus.Retired).ToString());//"退休"
            this.ddlState.Items.Add(lt);
            dltSex.Items.Add(new CheerUI.ListItem("女",//"女"
                ((int)Gender.Female).ToString()));
            dltSex.Items.Add(new CheerUI.ListItem("男",//"男"
                ((int)Gender.Male).ToString()));
            dltSex.Items.Insert(0, new CheerUI.ListItem("", ((int)Gender.Unset).ToString()));
        }
        private void InitGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {  new DataColumn(PSNAccountSchema.PSNACCOUNT_PERSONID,typeof(string)),
													 new DataColumn(PSNAccountSchema.PSNACCOUNT_EMPLOYEEID,typeof(string)),
													 new DataColumn(PSNAccountSchema.PSNACCOUNT_TRUENAME,typeof(int)),
													 new DataColumn(ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME,typeof(string)),
													 new DataColumn(ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE,typeof(string)),
													 new DataColumn(PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE,typeof(string)),
			});
            this.Grd.DataSource = dt;
            this.Grd.DataBind();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            init_Grid();
        }
        protected void init_Grid()
        {
            CommonMethod.AddFlexField(this.Grd, "", PSNAccountSchema.PSNACCOUNT_PERSONID, 0, true);
            CommonMethod.AddFlexField(this.Grd, "组织代码", ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE, 20, false);
            CommonMethod.AddFlexField(this.Grd, "组织", ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME, 20, false);
            CommonMethod.AddFlexField(this.Grd, "工号", PSNAccountSchema.PSNACCOUNT_EMPLOYEEID, 20, false);
            CommonMethod.AddFlexField(this.Grd, "姓名", PSNAccountSchema.PSNACCOUNT_TRUENAME, 20, false);
            CommonMethod.AddFlexField(this.Grd, "在职状态", PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE, 20, false);
        }
        private void getSessionSQL()
        {
            SelectedData data = new SelectedData();
            if (txtWno.Text.Trim() != "")
            {
                data.Wno = txtWno.Text;
            }
            if (txtName.Text.Trim() != "")
            {
                data.Name = txtName.Text;
            }
            if (dltSex.SelectedItem.Text.Trim() != "")
            {
                data.intSex = int.Parse(dltSex.SelectedValue);
            }
            data.DeptID = (StdBranchLoader as StdBranchLoader).GetSelectedBranchSQLStr();
            if (ddlState.SelectedItem.Text.Trim() != "")
            {
                data.intStatus = int.Parse(ddlState.SelectedValue);
            }
            Session["SELECTDATA"] = data;
        }
        private void LoadData(int pageindex, bool isAlert)
        {
            string Wno = "";
            string Name = "";
            int intSex = 0;
            string DeptID = "";
            int intStatus = 0;
            SelectedData data = new SelectedData();
            if (Session["SELECTDATA"] != null)
                data = (SelectedData)Session["SELECTDATA"];
            Wno = data.Wno;
            Name = data.Name;
            intSex = data.intSex;
            DeptID = data.DeptID;
            intStatus = data.intStatus;
            string accounttb = PSNAccountSchema.PSNACCOUNT_TABLENAME;
            string stdtb = ORGStdStructSchema.ORGSTDSTRUCT_TABLE;
            string selectSQL = "select " + accounttb + "." + PSNAccountSchema.PSNACCOUNT_PERSONID + "," +
                PSNAccountSchema.PSNACCOUNT_TRUENAME + "," + PSNAccountSchema.PSNACCOUNT_EMPLOYEEID + "," +
                PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE + "," + ORGStdStructSchema.ORGSTDSTRUCT_TABLE + "." + ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME + "," +
ORGStdStructSchema.ORGSTDSTRUCT_TABLE + "." + ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE +
                " from " + accounttb +
                " left join " + stdtb + " on " + accounttb + "." + PSNAccountSchema.PSNACCOUNT_BRANCHID +
                "=" + stdtb + "." + ORGStdStructSchema.ORGSTDSTRUCT_UNITID +
                //" left join " + stdtb + " on " + stdtb + "." + ORGStdStructSchema.ORGSTDSTRUCT_UNITID + "=" + orgstdtb + "." + ORGStdStructUnitSchema.ORGSTDSTRUCTUNIT_UNITID +
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
            {
                selectSQL += " and " + PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE + "=" + intStatus.ToString().Trim();
            }
            else
            {
                selectSQL += " and " + PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE + " in (" +
                    (int)AccessionStatus.Dimission + "," +
                    (int)AccessionStatus.Export + "," +
                    (int)AccessionStatus.Probation + "," +
                    (int)AccessionStatus.PromotingProbation + "," +
                    (int)AccessionStatus.Regular + "," +
                    (int)AccessionStatus.Retired + ")";
            }
            selectSQL += " and " + accounttb + "." + PSNAccountSchema.PSNACCOUNT_PERSONID + " not in ( select " +
                SECDimensionItemSchema.DIMITEMVALUE + " from " + SECDimensionItemSchema.SECDIMENSIONITEM_TABLE +
                " where " + SECDimensionItemSchema.PACKAGEID + "='" + this.PackageID.Trim() + "'" +
                " and " + SECDimensionItemSchema.DIMENSIONID + "='PERSON')";
            selectSQL += " order by " + ORGStdStructSchema.ORGSTDSTRUCT_LABELINDEX + " asc , " +
                PSNAccountSchema.PSNACCOUNT_EMPLOYEEID + " asc";
            PersonManager personmanager = (PersonManager)base.GetPalauObject(typeof(PersonManager), this.RightID.Trim(), true, "", false, "", false);
            DataTable _dt = personmanager.GetPersonsDstWithSecurity(selectSQL).Tables[0];
            if (_dt.Rows.Count > 0)
            {
                this.Grd.RecordCount= _dt.Rows.Count;
                LoadData(GetPagedDataTable(pageindex, Grd.PageSize, _dt));
                return;
            }
            this.InitGrid();
            if (this.Grd.Rows.Count == 0 && isAlert)
                base.ShowAlert("没有查询到符合条件的数据！");

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
        protected void LoadData(DataTable dt)
        {
            this.Grd.DataSource = dt;
            this.Grd.DataBind();
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

        protected void cmdSeach_Click(object sender, EventArgs e)
        {
            getSessionSQL();
            LoadData(0, true);
        }

        protected void cmdOK_Click(object sender, EventArgs e)
        {
            int pageindex = this.Grd.PageIndex;
            ArrayList addidlist = new ArrayList();
            int[] rows = Grd.SelectedRowIndexArray;
            foreach (int index in rows)
            {
                string psnid = Grd.Rows[index].Cells.FromKey(PSNAccountSchema.PSNACCOUNT_PERSONID).ToString().Trim();
                addidlist.Add(psnid);
            }
            if (addidlist.Count > 0)
            {
                SecurityDimensionItemLoader itemmanager = (SecurityDimensionItemLoader)eHRPageServer.GetPalauObject(typeof(SecurityDimensionItemLoader));
                itemmanager.SaveDimItemList(addidlist, this.PackageID.Trim(), "PERSON", DimensionItemType.Common, false);
                base.ShowAlert("保存成功!");
                this.LoadData(pageindex, false);
            }
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            CheerUI.PageContext.RegisterStartupScript(CheerUI.ActiveWindow.GetHidePostBackReference());
        }

        protected void Grd_PageIndexChange(object sender, CheerUI.GridPageEventArgs e)
        {
            this.Grd.PageIndex = e.NewPageIndex;
            LoadData(e.NewPageIndex, true);
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Grd.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            LoadData(this.Grd.PageIndex, true);
        }
    }
}