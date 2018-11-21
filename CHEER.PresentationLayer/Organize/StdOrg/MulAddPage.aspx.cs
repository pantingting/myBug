using CheerUI;
using CHEER.BusinessLayer.Organize;
using CHEER.Common;
using CHEER.CommonLayer.Organize.Data.STDOrganize;
using CHEER.PresentationLayer;
using CHEER.Platform.DAL;
using CHEER.PresentationLayer.Organize.SecControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CHEER.PresentationLayer.Organize.StdOrg
{
    public partial class MulAddPage : CHEERBasePage
    {
        private const string OrgAddFunID = "020060030090";

        private string DeptID
        {
            get
            {
                return Request.QueryString["UNITID"];
            }
        }

        protected void InitLau()
        {
            btnAdd.Text = base.getString("ZGAIA00023");
            btnClose.Text = base.getString("ZGAIA00522");
            cbEmptyAfterSave.Text = base.getString("ZGAIA00437");
            ddlMulType.Label = base.getString("ZGAIA00825");
            ddlScope.Label = base.getString("ZGAIA00824");
            (PersonSelect as PersonSelect).TextName.Label = base.getString("ZGAIA00056");
            txtRemark.Label = base.getString("ZGAIA00171");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MF_Verify();
                InitDropDownLists();
                (PersonSelect as PersonSelect).RightID = OrgAddFunID;
                InitLau();
                if (DeptID == null)
                {
                    mainForm.Enabled = false;
                }
            }
        }

        private Hashtable MF_LoadFCData()
        {
            Hashtable hash = new Hashtable();
            return hash;
        }
        private string MF_MenuID = "020060030090";
        private void MF_Verify()
        {
            if (!GetSecurityChecker().IsAllow(MF_MenuID))
            {
                CheerUI.Alert.ShowInTop(getAlert("ZGAIA00809"));
                mainForm.Enabled = false;
            }
            IDictionaryEnumerator idenumerator = MF_LoadFCData().GetEnumerator();
            while (idenumerator.MoveNext())
            {
                if (idenumerator.Value is WebControl)
                {
                    if (!GetSecurityChecker().IsAllow(idenumerator.Key.ToString()))
                    {
                        ((WebControl)idenumerator.Value).Enabled = false;
                    }
                    else
                    {
                        ((WebControl)idenumerator.Value).Enabled = true;
                    }
                }
            }
        }

        private void InitDropDownLists()
        {
            ddlMulType.Items.Clear();
            ddlScope.Items.Clear();
            FillDropDownList(CHEER.Common.PersonnelPublicCodeType.BranchEmp, ddlMulType);
            FillDropDownList(CHEER.Common.PersonnelPublicCodeType.EmpDuty, ddlScope);
        }

        protected void FillDropDownList(CHEER.Common.PersonnelPublicCodeType code, CheerUI.DropDownList ddl)
        {
            PersistBroker _broker = PersistBroker.Instance();
            try
            {
                DataTable dt = _broker.ExecuteSQLForDst("SELECT ITEMID,ITEMVALUE FROM PSNPUBLICCODEITEM WHERE TYPEID='" + (int)code + "'").Tables[0];
                ddl.DataTextField = "ITEMVALUE";
                ddl.DataValueField = "ITEMID";
                ddl.DataSource = dt;
                ddl.DataBind();
            }
            catch (Exception ex)
            {
                CheerUI.Alert.ShowInTop(ex.Message);
            }
            finally
            {
                _broker.Close();
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            MulRecord rec = new MulRecord();
            if (ddlScope.Enabled)
                rec.EMPDUTY = ddlScope.SelectedValue;
            rec.EMPTYPE = ddlMulType.SelectedValue;
            rec.MEMO = txtRemark.Text.Trim().DBReplace();
            rec.OID = IDGenerator.GenIDString();
            rec.PERSONID = (PersonSelect as PersonSelect).TextPersonId.Text;
            rec.UNITID = DeptID;
            MulRecordLoader orgmanager = (MulRecordLoader)eHRPageServer.GetPalauObject(typeof(MulRecordLoader));
            if (!orgmanager.Create(rec))
            {
                CheerUI.Alert.ShowInTop(orgmanager.ErrorMsg);
                return;
            }
            if (cbEmptyAfterSave.Checked)
            {
                Clear();
            }
            CheerUI.Alert.ShowInTop(base.getString("ZGAIA01968"));
        }

        void Clear()
        {
            txtRemark.Text = "";
            ddlMulType.SelectedIndex = 0;
            ddlScope.SelectedIndex = 0;
            (PersonSelect as PersonSelect).TextName.Text = "";
            (PersonSelect as PersonSelect).TextPersonId.Text = "";
        }
    }
}