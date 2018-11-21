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
    public partial class MulEditPage : CHEERBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MF_Verify();
                InitDropDownLists();
                InitLau();
                LoadData();
            }
        }

        protected void LoadData()
        {
            MulRecord obj = GetData(OID);
            DeptID = obj.UNITID;
            ddlMulType.SelectedValue = obj.EMPTYPE;
            ddlScope.SelectedValue = obj.EMPDUTY;
            txtRemark.Text = obj.MEMO;
            (PersonSelect as PersonSelect).RightID = OrgAddFunID;
            (PersonSelect as PersonSelect).TextPersonId.Text = obj.PERSONID;
            (PersonSelect as PersonSelect).TextName.Text = GetNameByPersonId(obj.PERSONID);
        }

        protected string GetNameByPersonId(string personId)
        {
            PersistBroker _broker = PersistBroker.Instance();
            try
            {
                return _broker.ExecuteSQLForDst("SELECT TRUENAME FROM PSNACCOUNT WHERE PERSONID='" + personId + "'").Tables[0].Rows[0]["TRUENAME"].ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                _broker.Close();
            }
        }

        MulRecord GetData(string oid)
        {
            MulRecordLoader orgmanager = (MulRecordLoader)eHRPageServer.GetPalauObject(typeof(MulRecordLoader));
            MulRecord obj = orgmanager.GetRecordByOID(oid);
            return obj;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            MulRecord rec = new MulRecord();
            if (ddlScope.Enabled)
                rec.EMPDUTY = ddlScope.SelectedValue;
            rec.EMPTYPE = ddlMulType.SelectedValue;
            rec.MEMO = txtRemark.Text.Trim().DBReplace();
            rec.OID = OID;
            rec.PERSONID = (PersonSelect as PersonSelect).TextPersonId.Text;
            rec.UNITID = DeptID;
            MulRecordLoader orgmanager = (MulRecordLoader)eHRPageServer.GetPalauObject(typeof(MulRecordLoader));
            if (!orgmanager.Save(rec))
            {
                CheerUI.Alert.ShowInTop(orgmanager.ErrorMsg);
                return;
            }
            CheerUI.Alert.ShowInTop(base.getString("ZGAIA00749"));
        }

        private Hashtable MF_LoadFCData()
        {
            Hashtable hash = new Hashtable();
            return hash;
        }
        private const string OrgAddFunID = "020060030100";

        string DeptID
        {
            get
            {
                return ViewState["DeptID"].ToString();
            }
            set
            {
                ViewState["DeptID"] = value;
            }
        }

        private string OID
        {
            get
            {
                return Request.QueryString["OID"];
            }
        }

        protected void InitLau()
        {
            btnSave.Text = base.getString("ZGAIA00195");
            btnClose.Text = base.getString("ZGAIA00522");
            ddlMulType.Label = base.getString("ZGAIA00825");
            ddlScope.Label = base.getString("ZGAIA00824");
            (PersonSelect as PersonSelect).TextName.Label = base.getString("ZGAIA00056");
            txtRemark.Label = base.getString("ZGAIA00171");
        }


        private string MF_MenuID = "020060030100";
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
    }
}