using CHEER.BusinessLayer.Organize;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.CommonLayer.Organize;
using CHEER.CommonLayer.Organize.Schema;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;
using CHEER.Platform.DAL;
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
    public partial class OrgMulPage : CHEERBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MF_Verify();
                InitLau();
                LoadData();
                AddRendererFunction();
                InitFace();
            }
        }

        protected void AddRendererFunction()
        {
            var empType = grdMain.Columns.FromKey(MulRecordSchema.EMPTYPE) as CheerUI.RenderField;
            empType.RendererFunction = @"
                                    function(value){
                                        return C('" + ddlEmpType.ClientID + @"').getTextByValue(value);
                                    }
                                ";

            var empDuty = grdMain.Columns.FromKey(MulRecordSchema.EMPDUTY) as CheerUI.RenderField;
            empDuty.RendererFunction = @"
                                    function(value){
                                        return C('" + ddlEmpDuty.ClientID + @"').getTextByValue(value);
                                    }
                                ";
        }

        protected string UNITID
        {
            get
            {
                return Request.QueryString["UNITID"];
            }
        }

        protected void InitFace()
        {
            if (!string.IsNullOrEmpty(UNITID))
            {
                STDUnitManager stdManager =
                    (STDUnitManager)eHRPageServer.GetPalauObject(typeof(STDUnitManager));
                STDUnit stdunit = stdManager.GetCurentUnitByID(UNITID);
                if (stdunit.IsTempUnit)
                {
                    CheerUI.Alert.ShowInTop(base.getAlert("ZGAIA00554"));
                    grdMain.Enabled = false;
                }
                if (IsHaveAccessBranch(UNITID))
                {

                }
                else
                {
                    CheerUI.Alert.ShowInTop(getString("ZGAIA00659"));
                    grdMain.Enabled = false;
                }
            }
            else
            {
                grdMain.Enabled = false;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            InitGrid();
        }

        protected void InitLau()
        {
            btnAdd.Text = base.getString("ZGAIA00280");
            btnDelete.Text = base.getString("ZGAIA00279");
        }

        CheerUI.DropDownList ddlEmpType = new CheerUI.DropDownList();
        CheerUI.DropDownList ddlEmpDuty = new CheerUI.DropDownList();

        protected void InitGrid()
        {
            CommonMethod.AddField(grdMain, "", MulRecordSchema.OID, 0, true);
            CommonMethod.AddFlexField(grdMain, "", MulRecordSchema.PERSONID, 0, true);
            CommonMethod.AddFlexField(grdMain, base.getAlert("ZGAIA00010"), "UNITCODE",15, false);
            CommonMethod.AddFlexField(grdMain, base.getAlert("ZGAIA00333"), "UNITNAME",15, false);
            CommonMethod.AddFlexField(grdMain, base.getAlert("ZGAIA00036"), "EMPLOYEEID", 15, false);
            CommonMethod.AddFlexField(grdMain, base.getAlert("ZGAIA00035"), "TRUENAME", 13, false);
            CommonMethod.AddFlexRendererField(grdMain, base.getAlert("ZGAIA00553"), MulRecordSchema.EMPTYPE, 15, "", false).Editor.Add(ddlEmpType);
            CommonMethod.AddFlexRendererField(grdMain, base.getAlert("ZGAIA00552"), MulRecordSchema.EMPDUTY, 20, "", false).Editor.Add(ddlEmpDuty);
            CommonMethod.AddFlexField(grdMain, base.getAlert("ZGAIA00256"), MulRecordSchema.MEMO, 40, false);
            if (base.GetSecurityChecker().IsAllow("020060030100"))
                CommonMethod.AddWindowField(grdMain, base.getString("ZGAIA00089"), "EDIT", 50, "detailWindow", MulRecordSchema.OID, "MulEditPage.aspx?OID={0}", base.getString("ZGAIA00823"));
        }

        protected void LoadData()
        {
            FillDropDownList(CHEER.Common.PersonnelPublicCodeType.BranchEmp, ddlEmpType);
            FillDropDownList(CHEER.Common.PersonnelPublicCodeType.EmpDuty, ddlEmpDuty);
            MulRecordLoader loader = (MulRecordLoader)eHRPageServer.GetPalauObject(typeof(MulRecordLoader));
            DataView dv = loader.GetMultiInfo(UNITID).Tables[0].DefaultView;
            grdMain.DataSource = dv;
            grdMain.DataBind();
        }

        protected void FillDropDownList(CHEER.Common.PersonnelPublicCodeType code,CheerUI.DropDownList ddl)
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

        private bool IsHaveAccessBranch(string branchID)
        {
            if (Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERID].ToString() == "sa")
            {
                return true;
            }
            SecurityChecker _checker = base.GetSecurityChecker();
            ArrayList branchArr = _checker.GetAccessBranches("020060030");
            for (int intx = 0; intx < branchArr.Count; intx++)
            {
                ArrayList tempArr = (ArrayList)branchArr[intx];
                if (tempArr[0].ToString().CompareTo(branchID) == 0)
                {
                    return true;
                }
            }
            STDUnitManager stdManager =
                (STDUnitManager)eHRPageServer.GetPalauObject(typeof(STDUnitManager));
            STDUnit stdunit = stdManager.GetCurentUnitByID(branchID);
            if (stdunit.IsTempUnit == true)
            {
                return true;
            }
            return false;
        }

        private Hashtable MF_LoadFCData()
        {
            Hashtable hash = new Hashtable();
            hash.Add("020060030090", btnAdd);
            hash.Add("020060030110", btnDelete);
            return hash;
        }
        private string MF_MenuID = "020060030";
        private void MF_Verify()
        {
            if (!GetSecurityChecker().IsAllow(MF_MenuID))
            {
                CheerUI.Alert.ShowInTop(getAlert("ZGAIA00809"));
                grdMain.Enabled = false;
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

        protected void grdMain_PageIndexChange(object sender, CheerUI.GridPageEventArgs e)
        {
            grdMain.PageIndex = e.NewPageIndex;
        }

        protected void detailWindow_Close(object sender, CheerUI.WindowCloseEventArgs e)
        {
            LoadData();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ArrayList delidlist = new ArrayList();
            Hashtable hashperson = new Hashtable();
            int[] rows = grdMain.SelectedRowIndexArray;
            foreach (int row in rows)
            {
                string personid = grdMain.Rows[row].Cells.FromKey(MulRecordSchema.PERSONID).ToString().Trim();
                delidlist.Add(grdMain.Rows[row].Cells.FromKey(MulRecordSchema.OID).ToString().Trim());
                if (!hashperson.ContainsKey(personid))
                {
                    hashperson.Add(personid, grdMain.Rows[row].Cells.FromKey("EMPLOYEEID").ToString().Trim());
                }
            }
            if (delidlist.Count > 0)
            {
                Hashtable hashNoRight;
                if (!base.GetSecurityChecker().IsPermitInPersons("020060030110", hashperson, out hashNoRight))
                {
                    string _emp = "";
                    foreach (string id in hashNoRight.Keys)
                    {
                        _emp += hashNoRight[id].ToString() + CM.Comma;
                    }
                    if (_emp.Length == 0)
                        _emp += CM.Comma;
                    string _strshow = base.getAlert("ZGAIA00551") + _emp.Substring(0, _emp.Length - 1) + base.getAlert("ZGAIA00550");
                    CheerUI.Alert.ShowInTop(_strshow);
                    return;
                }
                MulRecordLoader orgmanager = (MulRecordLoader)eHRPageServer.GetPalauObject(typeof(MulRecordLoader));
                if (!orgmanager.Delete(delidlist))
                {
                    base.ShowAlert(orgmanager.ErrorMsg);
                    return;
                }
                LoadData();
                CheerUI.Alert.ShowInTop(base.getString("ZGAIA00940"));
            }
        }
    }
}