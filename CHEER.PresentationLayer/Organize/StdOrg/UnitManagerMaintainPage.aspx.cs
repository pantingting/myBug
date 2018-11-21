using CheerUI;
using CHEER.BusinessLayer.ePersonnel.JobFamily;
using CHEER.BusinessLayer.ePersonnel.PersonnelBaseInfo;
using CHEER.BusinessLayer.Organize;
using CHEER.BusinessLayer.Organize.ManageUnit;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.CommonLayer.ePersonnel.Data;
using CHEER.CommonLayer.Organize.Data.STDOrganize;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;
using CHEER.Platform.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CHEER.PresentationLayer.Organize.StdOrg
{
    public partial class UnitManagerMaintainPage : CHEERBasePage
    {
        private const string UnitManagerFrame_FUNCTIONID = "CHEER.PresentationLayer.Organize.Common.PreSelectStdOrgUnit";
        private bool IsNewUnit = false;
        private bool IsIndexChange
        {
            get
            {
                string originIndex = txtIndex.Text.Trim().DBReplace();
                originIndex = originIndex == string.Empty ? "0" : originIndex;
                string changeIndex = txtIndexHidden.Text.Trim().DBReplace();
                changeIndex = changeIndex == string.Empty ? "0" : changeIndex;
                bool isIndexChange = false;
                if (originIndex != changeIndex)
                    isIndexChange = true;
                return isIndexChange;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitDropDownList();
                CheckAccess();
                InitLau();
                ViewState["IsNewUnit"] = IsNewUnit;
                if (Request.QueryString["UnitID"] != null)
                {
                    if (IsHaveAccessBranch(Request.QueryString["UnitID"].ToString()) == true)
                    {
                        DisplayInfo();
						//bingCoordinate();
						//bingTotalamt();
						bindInfo(Request.QueryString["UnitID"]);
						if (Request.QueryString["UnitID"].ToString() == "0") {
							btnAdd.Hidden = true;
							btnDelete.Hidden = true;
							btnStop.Hidden = true;
							btnSave.Hidden = true;
							btnEffect.Hidden = true;
							btnEffectIncludeSubNodes.Hidden = true;
						}

					}
                    else
                    {
                        CheerUI.Alert.ShowInTop(getString("ZGAIA00659"));
                        panel.Enabled = false;
                    }
                }
                else
                {
                    btnAdd.Enabled = false;
                    btnDelete.Enabled = false;
                    btnStop.Enabled = false;
                    btnSave.Enabled = false;
                    btnEffect.Enabled = false;
                    btnEffectIncludeSubNodes.Enabled = false;
                }
            }
        }

        private void InitLau()
        {
            txtPUnitId.Label = base.getString("ZGAIA00721");
            txtPUnitName.Label = base.getString("ZGAIA00720");
            txtPUnitCode.Label = base.getString("ZGAIA00721");
            txtUnitCode.Label = base.getString("ZGAIA00683");
            txtUnitName.Label = base.getString("ZGAIA00425");
            txtIndex.Label = base.getString("ZGAIA00716");
            ddlOrgType.Label = base.getString("ZGAIA00678");
            txtWorks.Label = base.getString("ZGAIA00654");
            txtBelongWorks.Label = base.getString("ZGAIA00717");
            QueryEmpIdNameManager.LblNoText = base.getString("ZGAIA00677");
            QueryEmpIdNameHelper.LblNoText = base.getString("ZGAIA00687");
            QueryEmpIdNameManager.LblNameText = base.getString("ZGAIA00676");
            QueryEmpIdNameHelper.LblNameText = base.getString("ZGAIA00686");
            txtCostCenter.Label = base.getString("ZGAIA00652");
            txtAddress.Label = base.getString("ZGAIA00726");
            btnAdd.Text = base.getString("ZGAIA00023");
            btnDelete.Text = base.getString("ZGAIA00194");
            btnSave.Text = base.getString("ZGAIA00195");
            btnEffect.Text = base.getString("ZGAIA00731");
            btnEffectIncludeSubNodes.Text = base.getString("ZGAIA00730");
            btnStop.Text = base.getString("ZGAIA00729");
            groupOrg.Title = base.getString("ZGAIA00684");
            groupManagerAndHelper.Title = base.getString("ZGAIA00681");
            groupAttribute.Title = base.getString("ZGAIA00718");
        }

        private void CheckAccess()
        {
            MF_Verify();
            setviewinfo(ButtonType.Stop, btnStop.Enabled);
            setviewinfo(ButtonType.Add, btnAdd.Enabled);
            setviewinfo(ButtonType.Save, btnSave.Enabled);
            setviewinfo(ButtonType.Delete, btnDelete.Enabled);
            setviewinfo(ButtonType.Effect, btnEffect.Enabled);
            setviewinfo(ButtonType.EffectWithSub, btnEffectIncludeSubNodes.Enabled);
        }
        private void setviewinfo(ButtonType keytype, bool isenabled)
        {
            ViewState[keytype.ToString()] = isenabled;
        }
        void setenableofbutton(ButtonType keytype, CheerUI.Button btn)
        {
            btn.Enabled = getviewinfo(keytype);
        }
        bool getviewinfo(ButtonType keytype)
        {
            bool flag = false;
            if (ViewState[keytype.ToString()] != null)
                flag = (bool)ViewState[keytype.ToString()];
            return flag;
        }
        enum ButtonType
        {
            Add,
            Save,
            Delete,
            Effect,
            EffectWithSub,
            Stop
        }

        private Hashtable MF_LoadFCData()
        {
            Hashtable hash = new Hashtable();
            hash.Add("009000030002", btnDelete);
            hash.Add("009000030004", btnEffect);
            hash.Add("009000030001", btnAdd);
            hash.Add("009000030005", btnEffectIncludeSubNodes);
            hash.Add("009000030003", btnSave);
            //hash.Add("020060030120", btnStop);
            return hash;
        }

        private void MF_Verify()
        {
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

        private STDUnit GetOldUnit()
        {
            STDUnitManager stdManager =
                   (STDUnitManager)eHRPageServer.GetPalauObject(typeof(STDUnitManager));
            STDUnit unitOldData = stdManager.GetCurentUnitByID(txtUnitId.Text);
            return unitOldData;
        }

        private int get_number(string str)
        {
            int i = 0;
            if (str != null && str.Trim() != "")
                i = int.Parse(str.Trim());
            return i;
        }

        protected void DisplayInfo()
        {
            STDUnitManager stdManager =
                (STDUnitManager)eHRPageServer.GetPalauObject(typeof(STDUnitManager));
            STDUnit stdUnit = stdManager.GetCurentUnitByID(Request.QueryString["UnitID"].ToString());

            setenableofbutton(ButtonType.Delete, btnDelete);
            setenableofbutton(ButtonType.Effect, btnEffect);
            setenableofbutton(ButtonType.EffectWithSub, btnEffectIncludeSubNodes);
            setenableofbutton(ButtonType.Add, btnAdd);
            setenableofbutton(ButtonType.Save, btnSave);
            setenableofbutton(ButtonType.Stop, btnStop);

            txtUnitName.Text = stdUnit.UnitName;
            txtUnitCode.Text = stdUnit.UnitCode;
            txtUnitId.Text = stdUnit.UnitID;

            if (stdUnit.ParentUnitID == "")
            {
                txtPUnitId.Text = "";
                txtPSourceUnitId.Text = "";
                txtPUnitCode.Text = base.getString("ZGAIA00880") + base.getString("ZGAIA00479");
                txtPUnitName.Text = base.getString("ZGAIA00880") + base.getString("ZGAIA00479");
            }
            else
            {
                STDUnit pUnit = stdManager.GetCurentUnitByID(stdUnit.ParentUnitID);
                txtPUnitId.Text = stdUnit.ParentUnitID;
                txtPSourceUnitId.Text = stdUnit.ParentUnitID;
                txtPUnitCode.Text = pUnit.UnitCode;
                txtPUnitName.Text = pUnit.UnitName;
            }

            if ((BranchState)stdUnit.UnitState == BranchState.Temp)
            {
                setenableofbutton(ButtonType.Effect, btnEffect);
                btnStop.Enabled = false;

            }
            else if ((BranchState)stdUnit.UnitState == BranchState.Efficient)
            {
                btnEffect.Enabled = false;
            }

            txtIndex.Text = stdUnit.UnitIndex.ToString();
            txtIndexHidden.Text = stdUnit.UnitIndex.ToString();
            txtWorks.Text = stdUnit.Works.ToString();
            if (txtWorks.Text == "0")
                txtWorks.Text = "";
            txtBelongWorks.Text = stdUnit.BelongWorks.ToString();
            if (txtBelongWorks.Text == "0")
                txtBelongWorks.Text = "";

            if (stdUnit.Director != null && stdUnit.Director != "")
            {
                PersonAccountData trueNameData = ((PersonAccountManager)eHRPageServer.GetPalauObject(typeof(PersonAccountManager))).Get(stdUnit.Director);
                ResponsibilityManager resManager = (ResponsibilityManager)eHRPageServer.GetPalauObject(typeof(ResponsibilityManager));
                if (trueNameData != null)
                {
                    QueryEmpIdNameManager.TextNo.Text = trueNameData.EmployeeID;
                    QueryEmpIdNameManager.TextName.Text = trueNameData.TrueName;
                    QueryEmpIdNameManager.TextPersonId.Text = trueNameData.PersonID;
                }
            }
            if (stdUnit.Assistant != null && stdUnit.Assistant != "")
            {
                PersonAccountData trueNameData = ((PersonAccountManager)eHRPageServer.GetPalauObject(typeof(PersonAccountManager))).Get(stdUnit.Assistant);
                if (trueNameData != null)
                {
                    QueryEmpIdNameHelper.TextNo.Text = trueNameData.EmployeeID;
                    QueryEmpIdNameHelper.TextName.Text = trueNameData.TrueName;
                    QueryEmpIdNameHelper.TextPersonId.Text = trueNameData.PersonID;
                }
            }

            if (stdUnit.UnitCost != null)
            {
                txtCostCenter.Text = stdUnit.UnitCost;
            }
            if (stdUnit.Address != null)
            {
                txtAddress.Text = stdUnit.Address;
            }
            if (stdUnit.UnitType != null)
            {
                ddlOrgType.SelectedValue = stdUnit.UnitType;
            }

            FillAttr(stdUnit.GetUnitCurrentAttributeForPerformance());

            //shake.Text = getBaseUrl() + "eUserForm/RedEnvelope/?unitId=" + txtUnitId.Text;
            //moreRedPackage.Text = getBaseUrl() + "eUserForm/MoreRedEnvelope/?unitId=" + txtUnitId.Text + "#/shareOpen";
            //userAwards.Text = getBaseUrl() + "eUserForm/UserAwards/?unitId=" + txtUnitId.Text + "#/userAwards";
            //userCustomers.Text = getBaseUrl() + "eUserForm/CusreCommend/?unitId=" + txtUnitId.Text + "#/list";
            //userRecommend.Text = getBaseUrl() + "eUserForm/CusreCommend/?unitId=" + txtUnitId.Text + "#/page";
            //bookHouse.Text = getBaseUrl() + "eUserForm/OrderSubmit/?unitId=" + txtUnitId.Text;
        }

        private void FillAttr(ArrayList attrArry)
        {
            foreach (AttributeData attr in attrArry)
            {
                for (int i = 1; i < formAttribute.Rows.Count; i++)
                {
                    var formRow = formAttribute.Rows[i];
                    foreach (CheerUI.ControlBase control in formRow.Items)
                    {
                        if (control.ID == attr.AttrID)
                        {
                            if (control is CheerUI.TextBox)
                            {
                                ((CheerUI.TextBox)control).Text = attr.AttrValue;
                            }
                            else if (control is CheerUI.DropDownList)
                            {
                                ((CheerUI.DropDownList)control).SelectedValue = attr.AttrValue;
                            }
                        }
                    }
                }
            }
        }

        private void ClearAttr()
        {
            for (int i = 1; i < formAttribute.Rows.Count; i++)
            {
                var formRow = formAttribute.Rows[i];
                foreach (CheerUI.ControlBase control in formRow.Items)
                {
                    if (control is CheerUI.TextBox)
                    {
                        ((CheerUI.TextBox)control).Text = "";
                    }
                    else if (control is CheerUI.DropDownList)
                    {
                        ((CheerUI.DropDownList)control).SelectedValue = "";
                    }
                    else if (control is CheerUI.NumberBox)
                    {
                        ((CheerUI.NumberBox)control).Text = "";
                    }
                }
            }
        }

        protected void InitDropDownList()
        {
            PersistBroker _broker = PersistBroker.Instance();
            try
            {
                DataTable dt = _broker.ExecuteSQLForDst("SELECT ITEMID,ITEMVALUE FROM PSNPUBLICCODEITEM WHERE TYPEID='36'").Tables[0];
                ddlOrgType.DataTextField = "ITEMVALUE";
                ddlOrgType.DataValueField = "ITEMID";
                ddlOrgType.DataSource = dt;
                ddlOrgType.DataBind();
                ddlOrgType.Items.Insert(0, new CheerUI.ListItem("", ""));
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

        protected void Page_Init(object sender, EventArgs e)
        {
            InitAttribute();
        }

        protected void InitAttribute()
        {
            STDUnitAttributeManager attrManager = (STDUnitAttributeManager)eHRPageServer.GetPalauObject(typeof(STDUnitAttributeManager));
            ArrayList attrArr = attrManager.GetUnitAttribute();


            CheerUI.FormRow formRow = new CheerUI.FormRow();
            formRow.ColumnWidths = "33% 67%";

            for (var index = 0; index < attrArr.Count; index++)
            {
                if (index > 0 && index % 3 == 0)
                {
                    formRow.ColumnWidths = "33% 33% 34%";
                    formAttribute.Rows.Add(formRow);
                    formRow = new CheerUI.FormRow();
                }

                STDUnitAttribute attr = (STDUnitAttribute)attrArr[index];
                if (attr.AttributeInputType == InputType.TextBox)
                {
                    CheerUI.TextBox textBox = new CheerUI.TextBox();
                    textBox.Label = attr.AttributeName;
                    textBox.ID = attr.AttributeID;
                    textBox.Width = Unit.Pixel(240);
                    if (attr.IsMustInput == "1")
                    {
                        textBox.Required = true;
                        textBox.ShowRedStar = true;
                    }
                    formRow.Items.Add(textBox);
                }
                else if (attr.AttributeInputType == InputType.ListBox)
                {
                    CheerUI.DropDownList ddl = new CheerUI.DropDownList();
                    ddl.Label = attr.AttributeName;
                    ddl.ID = attr.AttributeID;
                    DataSet itemDS = attrManager.QueryExecuteSQL(attr.DataSourceSQL);
                    ddl.DataSource = itemDS.Tables[0];
                    ddl.DataTextField = itemDS.Tables[0].Columns[1].ColumnName;
                    ddl.DataValueField = itemDS.Tables[0].Columns[0].ColumnName;
                    ddl.Width = Unit.Pixel(240);
                    ddl.DataBind();
                    ddl.Items.Insert(0, new CheerUI.ListItem("", ""));
                    if (attr.IsMustInput == "0")
                    {

                    }
                    else
                    {
                        ddl.Required = true;
                        ddl.ShowRedStar = true;
                    }
                    formRow.Items.Add(ddl);
                }
                else
                {
                }
            }

            formAttribute.Rows.Add(formRow);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            STDUnitManager stdManager =
            (STDUnitManager)eHRPageServer.GetPalauObject(typeof(STDUnitManager));
            try
            {
                STDUnit unitData = GetEditUnit();
                if ((bool)ViewState["IsNewUnit"] == true)
                {
                    try
                    {
                        stdManager.CreateUnit(unitData);
                        SetBaseNumAndControls();
                    }
                    catch (Exception ex)
                    {
                        CheerUI.Alert.ShowInTop(ex.Message);
                        return;
                    }
                }
                else
                {
                    try
                    {
                        stdManager.IsRefreshLabelIndex = IsIndexChange;
                        stdManager.UpdateUnit(unitData);
                    }
                    catch (Exception ex)
                    {
                        CheerUI.Alert.ShowInTop(ex.Message);
                        return;
                    }
                }
				setInfo(unitData.UnitID);

				CheerUI.PageContext.RegisterStartupScript("parent.doTreeRefresh();");
                CheerUI.Alert.ShowInTop(base.getString("ZGAIA00749"));

                lblMessage.Hidden = true;

                setenableofbutton(ButtonType.EffectWithSub, btnEffectIncludeSubNodes);
                setenableofbutton(ButtonType.Add, btnAdd);
                setenableofbutton(ButtonType.Delete, btnDelete);
                //saveCoordinate(txtUnitId.Text);
                //saveTotalamt(txtUnitId.Text);
                txtPSourceUnitId.Text = unitData.ParentUnitID;
                CHEER.BusinessLayer.ePersonnel.LogInformManager loger = (CHEER.BusinessLayer.ePersonnel.LogInformManager)eHRPageServer.GetPalauObject(typeof(CHEER.BusinessLayer.ePersonnel.LogInformManager));
                CHEER.CommonLayer.ePersonnel.Data.LogInformData logdata = new CHEER.CommonLayer.ePersonnel.Data.LogInformData();
                ConstructLogInformation(ref logdata);
                logdata.ACTION = (int)LogAction.eOrganizeStdChangeSuccess;
                logdata.NOTES = Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERNAME].ToString() + base.getString("ZGAIA00711") + unitData.UnitName + base.getString("ZGAIA00576");
                logdata.ACTIONTYPE = (int)LogActionType.update;
                logdata.LOGTYPE = (int)LogPropertyType.information;
                loger.create(logdata);
            }
            catch (Exception ex)
            {
                CheerUI.Alert.ShowInTop(ex.Message);
                CHEER.BusinessLayer.ePersonnel.LogInformManager loger = (CHEER.BusinessLayer.ePersonnel.LogInformManager)eHRPageServer.GetPalauObject(typeof(CHEER.BusinessLayer.ePersonnel.LogInformManager));
                CHEER.CommonLayer.ePersonnel.Data.LogInformData logdata = new CHEER.CommonLayer.ePersonnel.Data.LogInformData();
                ConstructLogInformation(ref logdata);
                logdata.ACTION = (int)LogAction.eOrganizeStdChangeFail;
                logdata.NOTES = Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERNAME].ToString() + base.getString("ZGAIA00711") + txtUnitName.Text + base.getString("ZGAIA00575");
                logdata.ACTIONTYPE = (int)LogActionType.update;
                logdata.LOGTYPE = (int)LogPropertyType.information;
                loger.create(logdata);
            }
        }

        private STDUnit GetEditUnit()
        {
            STDUnitManager stdManager =
                (STDUnitManager)eHRPageServer.GetPalauObject(typeof(STDUnitManager));
            STDUnit unitData = stdManager.GetCurentUnitByID(txtUnitId.Text);
            if (unitData == null)
                unitData = stdManager.GetCurentUnitByID(txtPUnitId.Text);
            unitData.ParentUnitID = txtPUnitId.Text;
            unitData.SourceParentUnitID = txtPSourceUnitId.Text;
            unitData.UnitCode = txtUnitCode.Text;
            unitData.UnitID = txtUnitId.Text;
            unitData.UnitName = txtUnitName.Text;
            unitData.UnitIndex = get_number(txtIndex.Text);
            unitData.Works = get_number(txtWorks.Text);
            if (unitData.Works == 0)
                unitData.Works = -1;
            unitData.BelongWorks = get_number(txtBelongWorks.Text);
            if (unitData.BelongWorks == 0)
                unitData.BelongWorks = -1;
            unitData.Operator = Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERNAME].ToString();
            unitData.LastSaveDate = DateTime.Now;
            unitData.Director = "";
            unitData.Assistant = "";
            unitData.UnitLevel = "";
            unitData.UnitType = "";
            unitData.UnitGrade = "";
            unitData.UnitDuty = "";
            unitData.UnitCost = "";
            unitData.Address = "";
            unitData.Fax = "";
            unitData.PostCode = "";
            unitData.IsLegal = "";
            unitData.CpcCharacter = "";
            ManageUnitManager mum = (ManageUnitManager)eHRPageServer.GetPalauObject(typeof(ManageUnitManager));
            unitData.ManageUnit = mum.GetManageUnit(unitData.ParentUnitID);
            unitData.IsManageUnit = 0;
            if ((bool)ViewState["IsNewUnit"] == true)
            {
                unitData.IsTempUnit = true;
                unitData.UnitID = Guid.NewGuid().ToString();
                txtUnitId.Text = unitData.UnitID;
            }

            for (int i = 1; i < formAttribute.Rows.Count; i++)
            {
                var formRow = formAttribute.Rows[i];
                foreach (CheerUI.ControlBase control in formRow.Items)
                {
                    StdUnitHisAttr attr = (StdUnitHisAttr)eHRPageServer.GetPalauObject(typeof(StdUnitHisAttr));
                    attr.AttrID = "";
                    attr.AttrValue = "";
                    attr.UnitID = unitData.UnitID;
                    attr.EditionID = unitData.OrganizeID;

                    if (control is CheerUI.TextBox)
                    {
                        var textBox = ((CheerUI.TextBox)control);
                        if (textBox.Text != "")
                        {
                            attr.AttrID = textBox.ID;
                            attr.AttrValue = textBox.Text;
                            unitData.AttributeList.Add(attr);
                        }
                    }
                    else if (control is CheerUI.DropDownList)
                    {
                        var textBox = ((CheerUI.DropDownList)control);
                        if (textBox.SelectedItem.Text != "")
                        {
                            attr.AttrID = textBox.ID;
                            attr.AttrValue = textBox.SelectedItem.Text;
                            unitData.AttributeList.Add(attr);
                        }
                    }
                }
            }

            //if (QueryEmpIdNameManager.TextPersonId.Text != "")
            //{
            //    unitData.Director = QueryEmpIdNameManager.TextPersonId.Text;
            //}
            //if (QueryEmpIdNameHelper.TextPersonId.Text != "")
            //{
            //    unitData.Assistant = QueryEmpIdNameHelper.TextPersonId.Text;
            //}
            //if (ddlOrgType.SelectedValue != "")
            //{
            //    unitData.UnitType = ddlOrgType.SelectedValue;
            //}
            //if (txtCostCenter.Text.Trim() != "")
            //{
            //    unitData.UnitCost = txtCostCenter.Text.Trim().DBReplace();
            //}
            if (txtCompanyFullname.Text.Trim() != "")
            {
                unitData.Address = txtCompanyFullname.Text.Trim().DBReplace();
            }

		    

            return unitData;
        }

        private void ConstructLogInformation(ref CHEER.CommonLayer.ePersonnel.Data.LogInformData logdata)
        {
            try
            {
                logdata.CLIENTIP = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[0].ToString();
            }
            catch
            {
                logdata.CLIENTIP = Page.Request.UserHostAddress;
            }
            logdata.CREATETIME = DataProcessor.DateTimeToLongString(DateTime.Now);
            logdata.LOGID = IDGenerator.GenIDString();
            logdata.MODULE = (int)LogModule.eorganize;
            logdata.OPERATOR = Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERNAME].ToString();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            STDUnitManager stdManager =
            (STDUnitManager)eHRPageServer.GetPalauObject(typeof(STDUnitManager));
            STDOrganizeManager orgManager =
            (STDOrganizeManager)eHRPageServer.GetPalauObject(typeof(STDOrganizeManager));
            STDUnit punit = stdManager.GetCurentUnitByID(txtUnitId.Text);
            txtPUnitCode.Text = punit.UnitCode;
            txtPUnitId.Text = punit.UnitID;
            txtPUnitName.Text = punit.UnitName;
            txtPSourceUnitId.Text = punit.UnitID;
            txtUnitCode.Text = orgManager.GetSTDUnitDefaultCode(punit.UnitID) + GetUnitCodeID();
            txtUnitName.Text = "";
            QueryEmpIdNameManager.TextPersonId.Text = "";
            QueryEmpIdNameManager.TextName.Text = "";
            QueryEmpIdNameManager.TextNo.Text = "";
            QueryEmpIdNameHelper.TextPersonId.Text = "";
            QueryEmpIdNameHelper.TextName.Text = "";
            QueryEmpIdNameHelper.TextNo.Text = "";
            ddlOrgType.SelectedValue = "";
            txtCostCenter.Text = "";
            txtAddress.Text = "";
            //txtRedTotalAmt.Text = "";
            //txtCouponsTotalAmt.Text = "";

            //nbLongitude.Text = "";
            //nbLatitude.Text = "";

            ClearAttr();
			//ClearValue();
			txtCompanyFullname.Text = "";
			txtLegalPerson.Text = "";
			txtCreditCode.Text = "";
			txtCompAddress.Text = "";
			txtOpeningBank.Text = "";
			txtAccounts.Text = "";

			lblMessage.Hidden = false;
            lblMessage.Text = base.getString("ZGAIA00023") + base.getString("ZGAIA00479");

            txtWorks.Text = "";
            txtBelongWorks.Text = "";
            txtIndex.Text = "";
            txtIndexHidden.Text = "";
            btnEffectIncludeSubNodes.Enabled = false;
            btnDelete.Enabled = false;
            setenableofbutton(ButtonType.Effect, btnEffect);
            btnAdd.Enabled = false;
            //新增状态为true
            ViewState["IsNewUnit"] = true;
        }

        protected void ClearValue()
        {
            btnStop.Enabled = false;
            btnDelete.Enabled = false;
            btnEffect.Enabled = false;
            btnEffectIncludeSubNodes.Enabled = false;
            btnAdd.Enabled = false;
            btnSave.Enabled = false;
            ddlOrgType.SelectedValue = "";
            QueryEmpIdNameManager.TextPersonId.Text = "";
            QueryEmpIdNameManager.TextName.Text = "";
            QueryEmpIdNameManager.TextNo.Text = "";
            QueryEmpIdNameHelper.TextPersonId.Text = "";
            QueryEmpIdNameHelper.TextName.Text = "";
            QueryEmpIdNameHelper.TextNo.Text = "";
            txtPUnitCode.Text = "";
            txtPUnitId.Text = "";
            txtPUnitName.Text = "";
            txtPSourceUnitId.Text = "";
            txtUnitCode.Text = "";
            txtUnitId.Text = "";
            txtUnitName.Text = "";
            //nbLatitude.Text="";
            //nbLongitude.Text="";
            txtCostCenter.Text = "";
            txtAddress.Text = "";
            txtWorks.Text = "";
            txtBelongWorks.Text = "";
            txtIndex.Text = "";
            txtIndexHidden.Text = "";


			ClearAttr();
        }

        protected void btnEffect_Click(object sender, EventArgs e)
        {
            STDUnitManager stdManager =
                (STDUnitManager)eHRPageServer.GetPalauObject(typeof(STDUnitManager));
            try
            {
                //新值
                STDUnit unitData = GetEditUnit();

                stdManager.IsRefreshLabelIndex = IsIndexChange;
                try
                {
                    stdManager.EffectUnit(unitData, (bool)ViewState["IsNewUnit"]);
                }
                catch (Exception ex)
                {
                    CheerUI.Alert.ShowInTop(ex.Message);
                    return;
                }
                DataTable unitNewData = stdManager.GetData(unitData.UnitID);
                lblMessage.Hidden = true;
                //stdManager.UpdateCOMSUBHOSTSYN(unitNewData, true);
                UpdateCostCenter(unitData.UnitID, unitData.UnitCost);
				setInfo(unitData.UnitID);

				SetBaseNumAndControls();

                ViewState["IsNewUnit"] = false;

                CheerUI.PageContext.RegisterStartupScript("parent.doTreeRefresh();");
                CheerUI.Alert.ShowInTop(base.getString("ZGAIA00867"));

                setenableofbutton(ButtonType.Delete, btnDelete);
                setenableofbutton(ButtonType.EffectWithSub, btnEffectIncludeSubNodes);
                setenableofbutton(ButtonType.Add, btnAdd);

                //saveCoordinate(txtUnitId.Text);
                //saveTotalamt(txtUnitId.Text);
                btnEffect.Enabled = false;
                txtPSourceUnitId.Text = unitData.ParentUnitID;
                CHEER.BusinessLayer.ePersonnel.LogInformManager loger = (CHEER.BusinessLayer.ePersonnel.LogInformManager)eHRPageServer.GetPalauObject(typeof(CHEER.BusinessLayer.ePersonnel.LogInformManager));
                CHEER.CommonLayer.ePersonnel.Data.LogInformData logdata = new CHEER.CommonLayer.ePersonnel.Data.LogInformData();
                ConstructLogInformation(ref logdata);
                logdata.ACTION = (int)LogAction.eOrganizeStdEffectSuccess;
                logdata.NOTES = Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERNAME].ToString() + base.getString("ZGAIA00710") + unitData.UnitName + base.getString("ZGAIA00576");
                logdata.ACTIONTYPE = (int)LogActionType.other;
                logdata.LOGTYPE = (int)LogPropertyType.information;
                loger.create(logdata);
            }
            catch (Exception ex)
            {
                CheerUI.Alert.ShowInTop(base.getString("ZGAIA00868") + base.getString("ZGAIA03533") + ex.Message);
                CHEER.BusinessLayer.ePersonnel.LogInformManager loger = (CHEER.BusinessLayer.ePersonnel.LogInformManager)eHRPageServer.GetPalauObject(typeof(CHEER.BusinessLayer.ePersonnel.LogInformManager));
                CHEER.CommonLayer.ePersonnel.Data.LogInformData logdata = new CHEER.CommonLayer.ePersonnel.Data.LogInformData();
                ConstructLogInformation(ref logdata);
                logdata.ACTION = (int)LogAction.eOrganizeStdEffectFail;
                logdata.NOTES = Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERNAME].ToString() + base.getString("ZGAIA00710") + txtUnitName.Text + base.getString("ZGAIA00575");
                logdata.ACTIONTYPE = (int)LogActionType.other;
                logdata.LOGTYPE = (int)LogPropertyType.information;
                loger.create(logdata);

            }
        }

        private void UpdateCostCenter(string UnitID, string CostCenter)
        {
            if (CostCenter != "")
            {
                string SQL = @"update orgstdstruct set corecenterinfo = '" + CostCenter.Replace("'", "''") + "' where label like " +
                    " (select label from orgstdstruct where unitid = '" + UnitID + "') || '%' " +
                    " and istempunit = 0 ";
                CHEER.Platform.DAL.PersistBroker broker = CHEER.Platform.DAL.PersistBroker.Instance();
                if (broker.DBType == CHEER.Platform.DAL.SQLCenter.DatabaseType.SQLSERVER)
                {
                    SQL = @"update orgstdstruct set corecenterinfo = '" + CostCenter.Replace("'", "''") + "' where label like " +
                    " (select label from orgstdstruct where unitid = '" + UnitID + "') + '%' " +
                    " and istempunit = 0 ";
                }
                broker.ExecuteSQL(SQL);
                broker.Close();
            }
        }

        protected void btnEffectIncludeSubNodes_Click(object sender, EventArgs e)
        {
            STDUnitManager stdManager =
                (STDUnitManager)eHRPageServer.GetPalauObject(typeof(STDUnitManager));
            try
            {
                STDUnit unitData = GetEditUnit();
                //生效及其子组织
                stdManager.EffectIncludeChildrenUnits(unitData);
				setInfo(unitData.UnitID);
				ViewState["IsNewUnit"] = false;

                CheerUI.Alert.ShowInTop(base.getString("ZGAIA00867"));
                CheerUI.PageContext.RegisterStartupScript("parent.doTreeRefresh();");
                lblMessage.Hidden = true;
                setenableofbutton(ButtonType.Add, btnAdd);
                //saveCoordinate(txtUnitId.Text);
                //saveTotalamt(txtUnitId.Text);
                CHEER.BusinessLayer.ePersonnel.LogInformManager loger = (CHEER.BusinessLayer.ePersonnel.LogInformManager)eHRPageServer.GetPalauObject(typeof(CHEER.BusinessLayer.ePersonnel.LogInformManager));
                CHEER.CommonLayer.ePersonnel.Data.LogInformData logdata = new CHEER.CommonLayer.ePersonnel.Data.LogInformData();
                ConstructLogInformation(ref logdata);
                logdata.ACTION = (int)LogAction.eOrganizeStdIncludeSubEffectSuccess;
                logdata.NOTES = Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERNAME].ToString() + base.getString("ZGAIA00709") + unitData.UnitName + base.getString("ZGAIA00576");
                logdata.ACTIONTYPE = (int)LogActionType.other;
                logdata.LOGTYPE = (int)LogPropertyType.information;
                loger.create(logdata);
            }
            catch (Exception ex)
            {
                CheerUI.Alert.ShowInTop(base.getString("ZGAIA00868") + base.getString("ZGAIA03533") + ex.Message);
                CHEER.BusinessLayer.ePersonnel.LogInformManager loger = (CHEER.BusinessLayer.ePersonnel.LogInformManager)eHRPageServer.GetPalauObject(typeof(CHEER.BusinessLayer.ePersonnel.LogInformManager));
                CHEER.CommonLayer.ePersonnel.Data.LogInformData logdata = new CHEER.CommonLayer.ePersonnel.Data.LogInformData();
                ConstructLogInformation(ref logdata);
                logdata.ACTION = (int)LogAction.eOrganizeStdIncludeSubEffectFail;
                logdata.NOTES = Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERNAME].ToString() + base.getString("ZGAIA00709") + txtUnitName.Text + base.getString("ZGAIA00575");
                logdata.ACTIONTYPE = (int)LogActionType.other;
                logdata.LOGTYPE = (int)LogPropertyType.information;
                loger.create(logdata);

            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            STDUnitManager stdManager =
                (STDUnitManager)eHRPageServer.GetPalauObject(typeof(STDUnitManager));
            try
            {
                STDUnit unitData = GetEditUnit();
                #region 判断是否管理单元
                Cls mum = (Cls)eHRPageServer.GetPalauObject(typeof(Cls));
                if (mum.IsContainsMU(unitData.UnitID))
                {
                    CheerUI.Alert.ShowInTop(base.getAlert("ZGAIA00514"));
                    return;
                }
                #endregion
                stdManager.DeleteUnit(unitData);
                ClearValue();

                CheerUI.PageContext.RegisterStartupScript("parent.doTreeRefresh();");

                CHEER.BusinessLayer.ePersonnel.LogInformManager loger = (CHEER.BusinessLayer.ePersonnel.LogInformManager)eHRPageServer.GetPalauObject(typeof(CHEER.BusinessLayer.ePersonnel.LogInformManager));
                CHEER.CommonLayer.ePersonnel.Data.LogInformData logdata = new CHEER.CommonLayer.ePersonnel.Data.LogInformData();
                ConstructLogInformation(ref logdata);
                logdata.ACTION = (int)LogAction.eOrganizeStdDeleteSuccess;
                logdata.NOTES = Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERNAME].ToString() + base.getString("ZGAIA00708") + unitData.UnitName + base.getString("ZGAIA00576");
                logdata.ACTIONTYPE = (int)LogActionType.delete;
                logdata.LOGTYPE = (int)LogPropertyType.information;
                loger.create(logdata);

            }
            catch (Exception ex)
            {
                CheerUI.Alert.ShowInTop(base.getString("ZGAIA00021") + base.getString("ZGAIA03533") + ex.Message);
                CHEER.BusinessLayer.ePersonnel.LogInformManager loger = (CHEER.BusinessLayer.ePersonnel.LogInformManager)eHRPageServer.GetPalauObject(typeof(CHEER.BusinessLayer.ePersonnel.LogInformManager));
                CHEER.CommonLayer.ePersonnel.Data.LogInformData logdata = new CHEER.CommonLayer.ePersonnel.Data.LogInformData();
                ConstructLogInformation(ref logdata);
                logdata.ACTION = (int)LogAction.eOrganizeStdDeleteFail;
                logdata.NOTES = Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERNAME].ToString() + base.getString("ZGAIA00708") + txtUnitName.Text + base.getString("ZGAIA00575");
                logdata.ACTIONTYPE = (int)LogActionType.delete;
                logdata.LOGTYPE = (int)LogPropertyType.information;
                loger.create(logdata);
            }
        }

        private string GetUnitCodeID()
        {
            CodeNumberCreateManager codeManager = new CodeNumberCreateManager();
            ArrayList numJudgeResult = codeManager.JudgeIfJournalNumMax("UNITCODE");
            string typeNumMax = numJudgeResult[0].ToString().Trim();
            string sysNumMax = numJudgeResult[1].ToString().Trim();
            if (typeNumMax == "1")
            {
                CheerUI.Alert.ShowInTop(base.getAlert("ZGAIA00505"));
                return "";
            }
            if (sysNumMax == "1")
            {
                CheerUI.Alert.ShowInTop(base.getAlert("ZGAIA00504"));
                return "";
            }
            return codeManager.CreateCodeNumberWithAuto("UNITCODE", "");
        }

        private void SetBaseNumAndControls()
        {
            string unitCodeID = txtUnitCode.Text.Trim().DBReplace();
            string autoID = GetUnitCodeID().Trim();
            string parentUnitCodeID = txtPUnitCode.Text;
            if (unitCodeID == autoID || unitCodeID == parentUnitCodeID + autoID)
            {
                CodeNumberCreateManager codeManager = new CodeNumberCreateManager();
                codeManager.UpdateBaseNumOfJournulNum("UNITCODE");
            }
        }

        protected void btnStop_Click(object sender, EventArgs e)
        {
            STDUnitManager stdManager = (STDUnitManager)eHRPageServer.GetPalauObject(typeof(STDUnitManager));
            try
            {
                STDUnit unitData = GetEditUnit();

                #region 判断是否管理单元
                Cls mum = (Cls)eHRPageServer.GetPalauObject(typeof(Cls));
                if (mum.IsContainsMU(unitData.UnitID))
                {
                    base.ShowAlert(base.getAlert("ZGAIA00511"));
                    return;
                }
                #endregion
                //停用
                stdManager.StopUnit(unitData);

                ClearValue();

                CheerUI.PageContext.RegisterStartupScript("parent.doTreeRefresh();");
                #region Write Log
                CHEER.BusinessLayer.ePersonnel.LogInformManager loger = (CHEER.BusinessLayer.ePersonnel.LogInformManager)eHRPageServer.GetPalauObject(typeof(CHEER.BusinessLayer.ePersonnel.LogInformManager));
                CHEER.CommonLayer.ePersonnel.Data.LogInformData logdata = new CHEER.CommonLayer.ePersonnel.Data.LogInformData();
                ConstructLogInformation(ref logdata);
                logdata.ACTION = (int)LogAction.eOrganizeStdStopSuccess;
                logdata.NOTES = Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERNAME].ToString() + base.getAlert("ZGAIA00510") + unitData.UnitName + base.getAlert("ZGAIA00509");
                logdata.ACTIONTYPE = (int)LogActionType.other;
                logdata.LOGTYPE = (int)LogPropertyType.information;
                loger.create(logdata);
                #endregion
            }
            catch (Exception ex)
            {
                CheerUI.Alert.ShowInTop(base.getAlert("ZGAIA00508") + base.getString("ZGAIA03533") + ex.Message);
                CHEER.BusinessLayer.ePersonnel.LogInformManager loger = (CHEER.BusinessLayer.ePersonnel.LogInformManager)eHRPageServer.GetPalauObject(typeof(CHEER.BusinessLayer.ePersonnel.LogInformManager));
                CHEER.CommonLayer.ePersonnel.Data.LogInformData logdata = new CHEER.CommonLayer.ePersonnel.Data.LogInformData();
                ConstructLogInformation(ref logdata);
                logdata.ACTION = (int)LogAction.eOrganizeStdStopFail;
                logdata.NOTES = Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERNAME].ToString() + base.getAlert("ZGAIA00510") + txtUnitName.Text + base.getAlert("ZGAIA00507");
                logdata.ACTIONTYPE = (int)LogActionType.other;
                logdata.LOGTYPE = (int)LogPropertyType.information;
                loger.create(logdata);
            }
        }


		public void setInfo(string unitid) {
			PersistBroker _broker = PersistBroker.Instance();

			var sql = $@"update orgstdstruct set ITEM1='{txtCompanyFullname.Text.DBReplace()}',ITEM2='{txtLegalPerson.Text.DBReplace()}',ITEM3='{txtCreditCode.Text.DBReplace()}',
						ITEM4='{txtOpeningBank.Text.DBReplace()}',ITEM5='{txtAccounts.Text.DBReplace()}',ADDRESS='{txtCompAddress.Text.DBReplace()}' where UNITID='{unitid}'";
			_broker.ExecuteNonQuery(sql);
			_broker.Close();
		}

		public void bindInfo(string unitid) {
			PersistBroker _broker = PersistBroker.Instance();

			var sql = $@"select ITEM1,ITEM2,ITEM3,ITEM4,ITEM5,ADDRESS from orgstdstruct where UNITID='{unitid}'";
			var dt=_broker.ExecuteSQLForDst(sql).Tables[0];
			_broker.Close();
			if (dt.Rows.Count > 0) {
				txtCompanyFullname.Text = dt.Rows[0]["ITEM1"].ToString();
				txtLegalPerson.Text = dt.Rows[0]["ITEM2"].ToString();
				txtCreditCode.Text = dt.Rows[0]["ITEM3"].ToString();
				txtCompAddress.Text = dt.Rows[0]["ITEM4"].ToString();
				txtOpeningBank.Text = dt.Rows[0]["ITEM5"].ToString();
				txtAccounts.Text = dt.Rows[0]["ADDRESS"].ToString();
			}
		}

		//public void saveCoordinate(string unitId)
		//{
		//    //查询unitid 是否存在 
		//    string sql = "select 1 from unitcoordinate where unitId='" + unitId + "'";
		//    PersistBroker _broker = PersistBroker.Instance();
		//    try
		//    {
		//        DataTable dt = _broker.ExecuteSQLForDst(sql).Tables[0];
		//        if (dt.Rows.Count > 0)
		//        {
		//            //修改
		//            sql = "update unitcoordinate set longitude=" + nbLongitude.Text + ",latitude=" + nbLatitude.Text + " where unitId='" + unitId + "'";
		//        }
		//        else
		//        {
		//            //新增
		//            sql = "insert into unitcoordinate (unitCoordinateId,unitId,longitude,latitude) values (uuid(),'"+unitId+"','"+nbLongitude.Text+"','"+nbLatitude.Text+"')";
		//        }

		//        _broker.ExecuteNonQuery(sql);
		//    }
		//    catch (Exception)
		//    {
		//        ShowAlert("经纬度保存失败");
		//        return;
		//    }

		//}

		//public void saveTotalamt(string unitId)
		//{
		//    //查询unitid 是否存在 
		//    string sql = "select 1 from unittotalamt where unitId='" + unitId + "'";
		//    PersistBroker _broker = PersistBroker.Instance();
		//    try
		//    {
		//        DataTable dt = _broker.ExecuteSQLForDst(sql).Tables[0];
		//        DataTable dttot = getTotal(unitId);
		//        if (dt.Rows.Count > 0 && dttot.Rows.Count>0)
		//        {
		//            string RedTotalAmt = dttot.Rows[0]["RedTotalAmt"].ToString();
		//            string CouponsTotalAmt = dttot.Rows[0]["CouponsTotalAmt"].ToString();
		//            //修改
		//            sql = "update unittotalamt set RedTotalAmt=" + txtRedTotalAmt.Text + ",RedTotalAmtRem=RedTotalAmtRem+" + txtRedTotalAmt.Text + "-"+RedTotalAmt+",CouponsTotalAmt=" + txtCouponsTotalAmt.Text + ",CouponsTotalAmtRem=CouponsTotalAmtRem+" + txtCouponsTotalAmt.Text + "-"+CouponsTotalAmt+" where unitId='" + unitId + "'";
		//        }
		//        else
		//        {
		//            //新增
		//            sql = "insert into unittotalamt (unittotalamtId,unitId,RedTotalAmt,RedTotalAmtRem,CouponsTotalAmt,CouponsTotalAmtRem) values (uuid(),'" + unitId + "','" + txtRedTotalAmt.Text + "','" + txtRedTotalAmt.Text + "','" + txtCouponsTotalAmt.Text + "','" + txtCouponsTotalAmt.Text + "')";
		//        }

		//        _broker.ExecuteNonQuery(sql);
		//    }
		//    catch (Exception)
		//    {
		//        ShowAlert("额度保存失败");
		//        return;
		//    }
		//}
		//private void bingCoordinate()
		//{
		//    string unitId = Request.QueryString["UnitID"].ToString();
		//    string sql = "select longitude,latitude from unitcoordinate where unitId='" + unitId + "'";
		//    PersistBroker _broker= PersistBroker.Instance();
		//    try
		//    {
		//        DataTable dt = _broker.ExecuteSQLForDst(sql).Tables[0];
		//        if(dt.Rows.Count>0)
		//        {
		//            nbLatitude.Text = dt.Rows[0]["latitude"].ToString();
		//            nbLongitude.Text = dt.Rows[0]["longitude"].ToString();
		//        }
		//    }
		//    catch (Exception)
		//    {

		//        throw;
		//    }
		//}

		//private void bingTotalamt()
		//{
		//    string unitId = Request.QueryString["UnitID"].ToString();
		//    string sql = "select RedTotalAmt,CouponsTotalAmt from unittotalamt where unitId='" + unitId + "'";
		//    PersistBroker _broker = PersistBroker.Instance();
		//    try
		//    {
		//        DataTable dt = _broker.ExecuteSQLForDst(sql).Tables[0];
		//        if (dt.Rows.Count > 0)
		//        {
		//            txtRedTotalAmt.Text = dt.Rows[0]["RedTotalAmt"].ToString();
		//            txtCouponsTotalAmt.Text = dt.Rows[0]["CouponsTotalAmt"].ToString();
		//        }
		//    }
		//    catch (Exception)
		//    {

		//        throw;
		//    }
		//}


		//public DataTable getTotal(string unitId)
		//{
		//    PersistBroker _broker = PersistBroker.Instance();
		//    string sql = " select RedTotalAmt,CouponsTotalAmt from unittotalamt where unitId='" + unitId + "'";
		//    DataTable dt = new DataTable();
		//    try
		//    {
		//        dt=_broker.ExecuteDataset(sql).Tables[0];
		//    }
		//    catch (Exception)
		//    {

		//        throw;
		//    }
		//    finally
		//    {
		//        _broker.Close();
		//    }
		//    return dt;
		//}

	}
}