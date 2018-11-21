using CHEER.BusinessLayer.ePersonnel.SystemConfig;
using CHEER.Common;
using CHEER.PresentationLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.ePersonnel;
using CHEER.PresentationLayer.Controls;
using CHEER.Platform.DAL;
using System.Web.Hosting;
using System.IO;
using CHEER.CommonLayer.ePersonnel.Data;
using CHEER.CommonLayer.eAttendance;
using CHEER.BusinessLayer.eCompetence.JobCodeQuery;
using CHEER.CommonLayer.ePersonnel;
using System.Collections;
using CHEER.DataAccessLayer.eAttendance;
using CHEER.DataAccessLayer.ePersonnel.LaborAccount;
using CHEER.DataAccessLayer.ePersonnel;
using CHEER.BusinessLayer.ePayroll.PersonalPayrollInformationManage;
using CHEER.PresentationLayer.CommonUse;
using CHEER.BusinessLayer.Security;
using CHEER.BusinessLayer.ePersonnel.PersonnelBaseInfo;
using CHEER.Platform.Common;

namespace CHEER.PresentationLayer.ePersonnel.PSNQuerySet
{
    public partial class PSNAdd : CHEERBasePage
    {
        private const string PageFunctionID = "009000040001";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindddlSupplier();
                if (Request.QueryString["ONLYADD"] == "1")
                {
                    btnReturn.Visible = false;
                }

                //btnEffect.Text = getString("ZGAIA00731");//生效
                //btnAdd.Text = getString("ZGAIA00023");//新增
                //btnReturn.Text = getString("ZGAIA00252");//返回
            }
        }
        protected void BindddlSupplier()
        {
            CommonMethod.ExecuteFunc(_broker => {
                var sql = _broker.ExecuteSQLForDst($@" SELECT supplierId,supplierName FROM c_supplier ");
                ddlSupplier.DataTextField = "supplierName";
                ddlSupplier.DataValueField = "supplierId";
                ddlSupplier.DataSource = sql.Tables[0];
                ddlSupplier.DataBind();
                ddlSupplier.Items.Insert(0, new CheerUI.ListItem("", ""));
            }, ex => {
                ShowAlert("经销商绑定失败！");
            });
        }
        protected void isSupplierDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSupplier.Hidden = isSupplierDDL.SelectedValue == "0" ? true : false;
        }

        protected override void OnInitComplete(EventArgs e)
        {
            string QSID = "";
            CHEER.Platform.DAL.PersistBroker broker = CHEER.Platform.DAL.PersistBroker.Instance();
            string sql = "select * from PSNQUERYSET where QSTYPE='PSN_BASE'";
            DataTable dt = broker.ExecuteDataset(sql).Tables[0];
            broker.Close(); //添加 数据库关闭 
            foreach (DataRow dr in dt.Rows)
            {
                QSID = dr["QSID"].ToString();
            }
            ViewState["QSID"] = QSID;
            LoadCondition(QSID);

            base.OnInitComplete(e);
            if (ViewState["id"] != null)
            {
                try
                {
                    //TODO
                    // ((TextBox)Form.FindControl(ViewState["id"].ToString())).Text = GetEmployeeID();
                }
                catch
                {

                }
            }
            if (ViewState["ATTENDONDATE"] != null)
            {
                try
                {
                    //  ((ucDate)Form.FindControl(ViewState["ATTENDONDATE"].ToString())).Text = DataProcessor.DateTimeToShortString(DateTime.Now);
                }
                catch
                {

                }
            }
            if (ViewState["PROBATIONENDDATE"] != null)
            {
                try
                {
                    PersonnelParameterManager _manager = (PersonnelParameterManager)eHRPageServer.GetPalauObject(typeof(PersonnelParameterManager));
                    string months = _manager.GetParameterValue(PersonnelParameterName.ProbationAlterBeforMonths);
                    string days = _manager.GetParameterValue(PersonnelParameterName.ProbationAlterBeforDays);
                    DateTime dateNow = DateTime.Now;
                    dateNow = dateNow.AddMonths(Int32.Parse(months));
                    dateNow = dateNow.AddDays(Int32.Parse(days));
                }
                catch
                {

                }
            }
        }

        private void LoadCondition(string QSID)
        {
            string lang = InfomationPackage.LanguageCulture;
            string tit = "QASTITLE";
            if (lang == "EN-US")
                tit = "QAS4";
            if (lang == "ZH-TW")
                tit = "QAS5";
            DataTable dt = new PSNQUERYSETLoader().QueryAddSet(QSID);

            //调整组织位置,将其放在合理的位置上(第一个位置或者第二个位置)
            var tempItemArray = new object[] { };
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["QASTYPE"].ToString() == "DEPT")
                {
                    if ((i + 1) % 3 == 0)
                    {
                        tempItemArray = dt.Rows[i - 1].ItemArray;
                        dt.Rows[i - 1].ItemArray = dt.Rows[i].ItemArray;
                        dt.Rows[i].ItemArray = tempItemArray;
                    }
                    break;
                }
            }
            CheerUI.FormRow formRow = new CheerUI.FormRow();
            CheerUI.ContentPanel contentPanel = new CheerUI.ContentPanel();

            CheerUI.Form formTemp = new CheerUI.Form();
            formTemp.ShowBorder = false;
            formTemp.ShowHeader = false;

            CheerUI.FormRow formTempRow = new CheerUI.FormRow();
            formTempRow.ColumnWidths = "0.33 0.33 0.34 ";

            formTemp.Rows.Add(formTempRow);
            int m = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (m == 3)
                {

                    formRow.Items.Add(formTemp);
                    formCondition.Rows.Add(formRow);
                    formTemp = new CheerUI.Form();
                    formTemp.ShowBorder = false;
                    formTemp.ShowHeader = false;
                    formRow = new CheerUI.FormRow();
                    formTempRow = new CheerUI.FormRow();
                    formTempRow.ColumnWidths = "0.33 0.33 0.34 ";
                    formTemp.Rows.Add(formTempRow);
                    m = 0;
                }

                string QCSFIELD = dt.Rows[i]["QASFIELD"].ToString();
                string QCSORDER = dt.Rows[i]["QASORDER"].ToString();
                // string QCSNAME = dt.Rows[i]["QASNAME"].ToString();
                string QCSTITLE = dt.Rows[i][tit].ToString();
                string QCSTYPE = dt.Rows[i]["QASTYPE"].ToString();
                string QCSDEFAULTVALUE = dt.Rows[i]["QASDEFAULTVALUE"].ToString();
                string QCSVALUELIST = dt.Rows[i]["QASVALUELIST"].ToString();
                string QCSID = dt.Rows[i]["QASID"].ToString();
                string QAISMUSTIN = dt.Rows[i]["QAISMUSTIN"].ToString();

                switch (QCSTYPE)
                {
                    case "STATE"://在职状态型

                        CheerUI.DropDownList ddlState = new CheerUI.DropDownList();
                        ddlState.EnableMultiSelect = true;
                        //ddlState.EnableCheckMultiSelect = true;
                        if (QAISMUSTIN == "YES")
                        {
                            ddlState.Required = true;
                        }
                        ddlState.ID = QCSID;
                        ddlState.Label = QCSTITLE;

                        ddlState.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA01150"), "4"));

                        ddlState.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA01152"), "5"));

                        ddlState.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA00565"), "1"));

                        ddlState.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA01151"), "6"));

                        ddlState.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA00564"), "2"));

                        ddlState.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA01149"), "3"));

                        ddlState.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA01148"), "7"));

                        formTempRow.Items.Add(ddlState);
                        m++;
                        break;
                    case "DATE":          //日期型
                        CheerUI.DatePicker datePicker = new CheerUI.DatePicker();
                        if (QAISMUSTIN == "YES")
                        {
                            datePicker.Required = true;
                            datePicker.ShowRedStar = true;
                        }
                        datePicker.ID = QCSID;
                        datePicker.Label = QCSTITLE;
                        datePicker.DateFormatString = "yyyy-MM-dd";
                        formTempRow.Items.Add(datePicker);
                        m++;
                        break;
                    case "DEPT":          //组织列表
                        string str2 = "/Controls/StdBranchLoader.ascx";
                        str2 = Server.MapPath("~" + str2);
                        StdBranchLoader StdBranchLoader = (StdBranchLoader)Page.LoadControl(ConvertSpecifiedPathToRelativePath(Page, str2));
                        StdBranchLoader.IsHaveManageUnit = false;
                        StdBranchLoader.IsShowLabel = true;
                        StdBranchLoader.IsIncludeSubBranch = false;
                        StdBranchLoader.IsShowIncludeSubBranch = false;
                        StdBranchLoader.LoadBranchDtsByID(base.GetSecurityChecker(), PageFunctionID);
                        StdBranchLoader.ID = QCSID;

                        CheerUI.UserControlConnector userControl = new CheerUI.UserControlConnector();
                        userControl.Controls.Add(StdBranchLoader);
                        StdBranchLoader.Label = QCSTITLE;
                        formTempRow.Items.Add(userControl);
                        if (formTempRow.Items.Count == 1)
                        {
                            formTempRow.ColumnWidths = "0.66 0.34 ";
                        }
                        else if (formTempRow.Items.Count == 2)
                        {
                            formTempRow.ColumnWidths = "0.33 0.67 ";
                        }
                        m += 2;
                        break;
                    case "DEFAULTLIST":   //自定义列表型
                        CheerUI.DropDownList ddlDefaultList = new CheerUI.DropDownList();
                        if (QAISMUSTIN == "YES")
                        {
                            ddlDefaultList.Required = true;
                            ddlDefaultList.ShowRedStar = true;
                        }
                        ddlDefaultList.Label = QCSTITLE;
                        ddlDefaultList.ID = QCSID;
                        ddlDefaultList.Items.Add(new CheerUI.ListItem("", ""));
                        if (QCSVALUELIST != "" && QCSVALUELIST != null)
                        {
                            string[] valuelist = QCSVALUELIST.Split(',');
                            foreach (string value in valuelist)
                            {
                                string[] arr = value.Split('|');
                                CheerUI.ListItem item = new CheerUI.ListItem();
                                item.Text = arr[1];
                                item.Value = arr[0];
                                ddlDefaultList.Items.Add(item);
                            }
                        }
                        formTempRow.Items.Add(ddlDefaultList);
                        m++;
                        break;
                    case "SQLLIST":       //SQL列表型
                        CheerUI.DropDownList ddlSqlList = new CheerUI.DropDownList();
                        if (QAISMUSTIN == "YES")
                        {
                            ddlSqlList.Required = true;
                            ddlSqlList.ShowRedStar = true;
                        }
                        ddlSqlList.Label = QCSTITLE;
                        ddlSqlList.ID = QCSID;
                        DataTable dtTemp = new DataTable();
                        PersistBroker _broker = PersistBroker.Instance();
                        try
                        {
                            dtTemp = _broker.ExecuteSQLForDst(QCSVALUELIST).Tables[0];

                            if (QCSFIELD == "SHIFTGROUP")
                            {
                                FilterShiftGroup(dtTemp);
                            }
                        }
                        catch (Exception ee) { throw ee; }
                        finally { _broker.Close(); }

                        ddlSqlList.Items.Add(new CheerUI.ListItem("", ""));
                        if (dtTemp.Rows.Count > 0)
                        {
                            string strValue = "";
                            string strText = "";
                            CheerUI.ListItem item;
                            for (int j = 0; j < dtTemp.Rows.Count; j++)
                            {
                                item = new CheerUI.ListItem();
                                strValue = dtTemp.Rows[j][0].ToString();
                                strText = dtTemp.Rows[j][1].ToString();
                                item.Text = strText;
                                item.Value = strValue;
                                ddlSqlList.Items.Add(item);
                            }
                        }
                        formTempRow.Items.Add(ddlSqlList);
                        m++;
                        break;
                    case "BOOL":
                        CheerUI.CheckBox checkField = new CheerUI.CheckBox();

                        checkField.Label = QCSTITLE;
                        checkField.ID = QCSID;
                        formTempRow.Items.Add(checkField);
                        m++;
                        break;
                    case "INT":
                        CheerUI.NumberBox numberField = new CheerUI.NumberBox();
                        if (QAISMUSTIN == "YES")
                        {
                            numberField.Required = true;
                            numberField.ShowRedStar = true;
                        }
                        numberField.Label = QCSTITLE;
                        numberField.ID = QCSID;
                        formTempRow.Items.Add(numberField);
                        m++;
                        break;
                    default:
                        CheerUI.TextBox txtField = new CheerUI.TextBox();
                        if (QAISMUSTIN == "YES")
                        {
                            txtField.Required = true;
                            txtField.ShowRedStar = true;
                        }

                        txtField.Label = QCSTITLE;
                        txtField.ID = QCSID;

                        formTempRow.Items.Add(txtField);
                        m++;
                        break;
                }
            }
            for (; m < 3; m++)
            {
                contentPanel = new CheerUI.ContentPanel();
                contentPanel.ShowBorder = false;
                contentPanel.ShowHeader = false;
                contentPanel.Height = new Unit(1);
                formTempRow.Items.Add(contentPanel);

            }
            formRow.Items.Add(formTemp);
            formCondition.Rows.Add(formRow);


        }

        //过滤排班组
        private void FilterShiftGroup(DataTable dt)
        {
            string userid = Session["UserID"].ToString();

            if (userid.ToUpper() != "SA")
            {
                SecurityChecker sc = new SecurityChecker(userid);

                PersistBroker b = PersistBroker.Instance();
                string sql = @"select ATDSHIFTGROUP.* from ATDSHIFTGROUP 
                    left join SECUSER on secuser.PERSONID = ATDSHIFTGROUP.CREATOR 
                    left join psnaccount on psnaccount.PERSONID = secuser.PERSONID
                    left join orgstdstruct on branchid = unitid  where " + sc.GetFilterSql("091002") + " order by GROUPNAME";
                DataTable dtAccess = b.ExecuteSQLForDst(sql).Tables[0];
                b.Close();

                Hashtable ht = new Hashtable();
                for (int i = 0; i < dtAccess.Rows.Count; i++)
                {
                    if (!ht.ContainsKey(dtAccess.Rows[i]["GROUPID"].ToString()))
                    {
                        ht.Add(dtAccess.Rows[i]["GROUPID"].ToString(), "");
                    }
                }

                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    if (!ht.ContainsKey(dt.Rows[i]["GROUPID"].ToString()))
                    {
                        dt.Rows.RemoveAt(i);
                    }
                }
            }
        }


        /// <summary>
        /// 根据绝对路径获取相对路径
        /// </summary>
        /// <param name="page"></param>
        /// <param name="specifiedPath"></param>
        /// <returns></returns>
        public static string ConvertSpecifiedPathToRelativePath(Page page, string specifiedPath)
        {
            string virtualPath = page.Request.ApplicationPath;
            string pathRooted = HostingEnvironment.MapPath(virtualPath);
            if (!Path.IsPathRooted(specifiedPath) || specifiedPath.IndexOf(pathRooted) == -1)
            {
                return specifiedPath;
            }
            if (pathRooted.Substring(pathRooted.Length - 1, 1) == "\\")
            {
                specifiedPath = specifiedPath.Replace(pathRooted, "~/");
            }
            else
            {
                specifiedPath = specifiedPath.Replace(pathRooted, "~");
            }
            string relativePath = specifiedPath.Replace("\\", "/");
            return relativePath;
        }

        private string isnull(object obj)
        {
            if (obj == null)
            {
                return "";
            }
            else
            {
                return obj.ToString();
            }
        }
        private PSNACCOUNTET Save2()
        {
            DataTable dt = new PSNQUERYSETLoader().QueryAddSet(ViewState["QSID"].ToString());

            string personid = Guid.NewGuid().ToString();
            ViewState["PERSONID"] = personid;

            PSNACCOUNTET et2 = new PSNACCOUNTET();
            et2.PERSONID = personid;
            string lang = InfomationPackage.LanguageCulture;
            string tit = "QASTITLE";
            if (lang == "EN-US")
                tit = "QAS4";
            if (lang == "ZH-TW")
                tit = "QAS5";
            string ScontrolID = "";
            string value = "";
            string ATTENDONDATE = "";
            string DIMISSIONDATE = "";
            string PROBATIONENDDATE = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string QASFIELD = dt.Rows[i]["QASFIELD"].ToString();
                string QASTITLE = dt.Rows[i][tit].ToString();
                string QASTYPE = dt.Rows[i]["QASTYPE"].ToString();
                string QAISMUSTIN = dt.Rows[i]["QAISMUSTIN"].ToString();
                ScontrolID = dt.Rows[i]["QASID"].ToString();

                switch (QASTYPE)
                {
                    case "DATE":
                        CheerUI.DatePicker con1 = (CheerUI.DatePicker)CheerUI.ControlUtil.FindControl(ScontrolID);
                        value = con1.Text;
                        if (QAISMUSTIN == "YES" && (value == null || value == ""))
                        {
                            saveValue = false;
                            errorInfo = errorInfo + getString(QASTITLE + getString("ZGAIA00714"));
                            // base.ShowAlert(getString(QASTITLE + getString("ZGAIA00714")));//必填
                            return null;
                        }
                        if (QASFIELD == "ATTENDONDATE")
                        {
                            ATTENDONDATE = value;
                        }
                        if (QASFIELD == "DIMISSIONDATE")
                        {
                            DIMISSIONDATE = value;
                        }
                        if (QASFIELD == "PROBATIONENDDATE") //试满日
                        {
                            PROBATIONENDDATE = value;
                        }
                        //if (QASFIELD == "date1")
                        //{
                        //    date1 = value;
                        //}
                        //if (QASFIELD == "date2")
                        //{
                        //    date2 = value;
                        //}
                        //if (QASFIELD == "date3")
                        //{
                        //    date3 = value;
                        //}
                        //if (QASFIELD == "date4")
                        //{
                        //    date4 = value;
                        //}
                        //if (QASFIELD == "date5")
                        //{
                        //    date5 = value;
                        //}
                        if (ATTENDONDATE != "" && DIMISSIONDATE != "" && DateTime.Parse(ATTENDONDATE) > DateTime.Parse(DIMISSIONDATE)
                            || ATTENDONDATE != "" && PROBATIONENDDATE != "" && DateTime.Parse(ATTENDONDATE) > DateTime.Parse(PROBATIONENDDATE))//判断入职日是否为离职日之前
                        {
                            if (InfomationPackage.LanguageCulture == "EN-US")
                            {
                                errorInfo += "DIMISSIONDATE or PROBATIONENDDATE can't later then ATTENDONDATE!";
                            }
                            else
                            {
                                errorInfo += "保存失败，离职日或试满日早于入职日";
                            }
                            saveValue = false;
                            return null;
                        }
                        break;
                    case "DEPT":
                        StdBranchLoader bl = (StdBranchLoader)CheerUI.ControlUtil.FindControl(ScontrolID);
                        value = isnull(bl.GetSelectBranchItem().Value);
                        if (QAISMUSTIN == "YES" && (value == null || value == ""))
                        {
                            saveValue = false;
                            errorInfo = errorInfo + getString(QASTITLE + getString("ZGAIA00714"));
                            //base.ShowAlert(getString(QASTITLE + getString("ZGAIA00714")));//必填
                            return null;
                        }
                        break;
                    case "DEFAULTLIST":
                        CheerUI.DropDownList drp = (CheerUI.DropDownList)CheerUI.ControlUtil.FindControl(ScontrolID);
                        value = isnull(drp.SelectedValue);
                        if (QAISMUSTIN == "YES" && (value == null || value == ""))
                        {
                            saveValue = false;
                            errorInfo = errorInfo + getString(QASTITLE + getString("ZGAIA00714"));
                            // base.ShowAlert(getString(QASTITLE + getString("ZGAIA00714")));//必填
                            return null;
                        }
                        break;
                    case "SQLLIST":
                        CheerUI.DropDownList drp2 = (CheerUI.DropDownList)CheerUI.ControlUtil.FindControl(ScontrolID);
                        value = drp2.SelectedValue;
                        if (QAISMUSTIN == "YES" && (value == null || value == ""))
                        {
                            saveValue = false;
                            errorInfo = errorInfo + getString(QASTITLE + getString("ZGAIA00714"));
                            // base.ShowAlert(getString(QASTITLE + getString("ZGAIA00714")));//必填
                            return null;
                        }
                        break;
                    case "BOOL":
                        CheerUI.CheckBox ck = (CheerUI.CheckBox)CheerUI.ControlUtil.FindControl(ScontrolID);
                        if (ck.Checked)
                        { value = "1"; }
                        else
                        { value = "0"; }
                        break;
                    case "INT":
                        CheerUI.TextBox tb = (CheerUI.TextBox)CheerUI.ControlUtil.FindControl(ScontrolID);
                        value = isnull(tb.Text);
                        if (QAISMUSTIN == "YES" && (value == null || value == ""))
                        {
                            saveValue = false;
                            errorInfo = errorInfo + getString(QASTITLE + getString("ZGAIA00714"));
                            // base.ShowAlert(getString(QASTITLE + getString("ZGAIA00714")));//必填
                            return null;
                        }

                        break;
                    default:
                        CheerUI.TextBox tb2 = (CheerUI.TextBox)CheerUI.ControlUtil.FindControl(ScontrolID);
                        value = isnull(tb2.Text);
                        if (QAISMUSTIN == "YES" && (value == null || value == ""))
                        {
                            saveValue = false;
                            errorInfo = errorInfo + getString(QASTITLE + getString("ZGAIA00714"));
                            //base.ShowAlert(getString(QASTITLE + getString("ZGAIA00714")));//必填
                            return null;
                        }
                        if (QASFIELD == "EMPLOYEEID")  //工号
                        {
                            CHEER.Platform.DAL.PersistBroker broker1 = CHEER.Platform.DAL.PersistBroker.Instance();
                            string Esql = "select * from PSNACCOUNT where EMPLOYEEID= '" + value + "'";
                            DataTable dtE = broker1.ExecuteDataset(Esql).Tables[0];
                            if (dtE.Rows.Count > 0)
                            {
                                saveValue = false;
                                errorInfo = errorInfo + getString("ZGAIA05534");
                                //base.ShowAlert(getString("ZGAIA05418"));//工号已经存在

                                return null;
                            }
                        }
                        if (QASFIELD == "CARDNUM")  //卡号，需要判断是否合理
                        {
                            value = value.Trim();
                            tb2.Text = value;

                            if (value != "" && !value.All(char.IsDigit))
                            {
                                saveValue = false;
                                errorInfo = errorInfo + "卡号必须是数字！请重新输入！";

                                return null;
                            }
                            if (value != "" && !CheckCardNum(et2.PERSONID, value))  //检查卡号是否和已有在职人员重复
                            {
                                saveValue = false;
                                errorInfo = errorInfo + "卡号和已有在职人员重复！或离职员工卡号还未失效。";
                                return null;
                            }

                        }
                        if (QASFIELD == "CELLPHONE")  //手机号，需要判断是否合理
                        {
                            value = value.Trim();
                            tb2.Text = value;

                            if (!CommonUse.Common.IsHandset(value))
                            {
                                saveValue = false;
                                errorInfo = errorInfo + "手机号码格式不正确";
                                return null;
                            }

                            if (QueryExistsLoginName(value))
                            {
                                saveValue = false;
                                errorInfo = errorInfo + "手机号重复";
                                return null;
                            }
                        }
                        break;
                }
                SetValue(et2, QASFIELD, value);

            }


            if (saveValue)
            {
                PSNACCOUNTDA pda2 = new PSNACCOUNTDA();
                pda2.Insert(et2);
                string create = Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERNAME].ToString();
                //PersonalSynchronousInfo personSynInfo = new PersonalSynchronousInfo();
                //personSynInfo.AddPersonInfo(et2, create);
                return et2;
            }
            else
            {
                return null;
            }


        }
        private void SetValue(PSNACCOUNTET et2, string key, string value)
        {
            //性别，IDLIDL，在职状态
            switch (key)
            {
                case "BRANCHID":
                    et2.BRANCHID = value;
                    break;
                case "EMPLOYEEID":
                    et2.EMPLOYEEID = value;
                    break;
                case "TRUENAME":
                    et2.TRUENAME = value;
                    break;
                case "ACCESSIONSTATE":
                    et2.ACCESSIONSTATEPRE = int.Parse(value);
                    et2.ACCESSIONSTATE = et2.ACCESSIONSTATEPRE;
                    break;
                case "ATTENDONDATE":
                    et2.ATTENDONDATE = value;
                    break;
                case "CERTIFICATETYPEID":
                    et2.CERTIFICATETYPEID = value;
                    break;
                case "CERTIFICATENUMBER":
                    et2.CERTIFICATENUMBER = value;
                    break;
                case "GENDER":
                    if (value != "")
                    { et2.GENDER = int.Parse(value); }
                    else
                    { et2.GENDER = 0; }
                    break;
                case "CARDNUM":
                    et2.CARDNUM = value;
                    break;
                case "STARTSERVICEDATE":
                    et2.STARTSERVICEDATE = value;
                    break;
                case "DIMISSIONDATE":
                    et2.DIMISSIONDATE = value;
                    break;

                case "CELLPHONE":
                    et2.CELLPHONE = value;
                    break;
                case "EXT":
                    et2.EXT = value;
                    break;
                case "COMPANYEMAIL":
                    et2.COMPANYEMAIL = value;
                    break;
                case "CONTACTADDRESS":
                    et2.CONTACTADDRESS = value;
                    break;
                case "POSTCODE":
                    et2.POSTCODE = value;
                    break;
                case "EmergencyContact":
                    et2.EmergencyContact = value;
                    break;
                case "EmergencyContactNo":
                    et2.EmergencyContactNo = value;
                    break;

                case "JOBCODE":
                    et2.JOBCODE = value;
                    break;
                case "RANKID":
                    et2.RANKID = value;
                    break;
                case "TITLEID":
                    et2.TITLEID = value;
                    break;
                case "RESPONSIBILITYID":
                    et2.RESPONSIBILITYID = value;
                    break;
                case "NATIVEPLACEPROPERTYID":
                    et2.NATIVEPLACEPROPERTYID = value;
                    break;
                case "JOBTYPEID":
                    et2.JOBTYPEID = value;
                    break;
                case "ARRANGEMENTID":
                    et2.ARRANGEMENTID = value;
                    break;
                case "BELONGCORPID":
                    et2.BELONGCORPID = value;
                    break;
                case "EMPLOYEECHARID":
                    et2.EMPLOYEECHARID = value;
                    break;

                case "COMPANYBUS":
                    et2.COMPANYBUS = value;
                    break;
                case "PRODUCTLINE":
                    et2.PRODUCTLINE = value;
                    break;
                case "EDUCATIONALLEVELID":
                    et2.EDUCATIONALLEVELID = value;
                    break;
                case "PROBATIONENDDATE":
                    et2.PROBATIONENDDATE = value;
                    break;
                case "DATEOFBIRTH":
                    et2.DATEOFBIRTH = value;
                    break;
                case "NATIONID":
                    et2.NATIONID = value;
                    break;
                case "NATIONALITYID":
                    et2.NATIONALITYID = value;
                    break;
                case "FIRSTNAME":
                    et2.FIRSTNAME = value;
                    break;
                case "MIDDLENAME":
                    et2.MIDDLENAME = value;
                    break;
                case "LASTNAME":
                    et2.LASTNAME = value;
                    break;
                case "HOMEPLACE":
                    et2.HOMEPLACE = value;
                    break;
                case "POLICALFACEID":
                    et2.POLICALFACEID = value;
                    break;
                case "MARRIAGEID":
                    et2.MARRIAGEID = value;
                    break;
                case "GRADUATESCHOOLID":
                    et2.GRADUATESCHOOLID = value;
                    break;
                case "DLIDL":
                    et2.DLIDL = value;
                    break;
                case "EMPLOYEETYPEID":
                    et2.EMPLOYEETYPEID = value;
                    break;
                case "SOCIETYENSURENUM":
                    et2.SOCIETYENSURENUM = value;
                    break;
                case "EMPGROUP":
                    et2.EMPGROUP = value;
                    break;
                case "REMARK":
                    et2.REMARK = value;
                    break;
                case "SHIFTGROUP":
                    et2.SHIFTGROUP = value;
                    break;
                case "AnnualLeave":
                    if (value == "")
                    {

                        et2.AnnualLeave = -1;
                    }
                    else
                    {
                        et2.AnnualLeave = int.Parse(value);
                    }

                    break;
                case "VacationDays":
                    double days = 0;
                    if (value == "")
                    {
                        et2.OvertimeSettlementUpper = 0;
                    }
                    else
                    {
                        if (double.TryParse(value, out days))
                        {
                            if ((et2.AnnualLeave == 0 | et2.AnnualLeave == -1) && days != 0)
                            {
                                saveValue = false;
                                errorInfo = errorInfo + getString("ZGAIA05532");
                                et2.VacationDays = 0;
                            }
                            else
                            {
                                if (days < 0)
                                {
                                    saveValue = false;
                                    errorInfo = errorInfo + getString("ZGAIA05535");
                                    et2.VacationDays = 0;
                                }
                                else
                                {
                                    et2.VacationDays = days;
                                }


                            }

                        }
                        else
                        {
                            saveValue = false;
                            errorInfo = errorInfo + getString("ZGAIA05524") + getString("ZGAIA05531");
                            et2.VacationDays = 0;
                        }
                    }
                    break;
                case "overtimeSettlementUpper":
                    double upper = 0;
                    if (value == "")
                    { et2.OvertimeSettlementUpper = 0; }
                    else
                    {
                        if (double.TryParse(value, out upper))
                        {
                            if (upper < 0)
                            {
                                saveValue = false;
                                errorInfo = errorInfo + getString("ZGAIA05533");
                                et2.OvertimeSettlementUpper = 0;
                            }
                            else
                            {
                                et2.OvertimeSettlementUpper = upper;
                            }

                        }
                        else
                        {
                            saveValue = false;
                            errorInfo = errorInfo + getString("ZGAIA05526") + getString("ZGAIA05531");
                            et2.OvertimeSettlementUpper = 0;
                        }
                    }

                    break;
                case "ComAccountWorkingHours":
                    if (value != "")
                    {
                        et2.ComAccountWorkingHours = int.Parse(value);
                    }
                    else
                    {
                        et2.ComAccountWorkingHours = -1;
                    }

                    break;
                case "OvertimePayment":
                    et2.OvertimePayment = value;
                    break;
                case "AttendanceWay":
                    et2.AttendanceWay = value;
                    break;
                case "IsInterns":
                    if (value != "")
                    { et2.IsInterns = int.Parse(value); }
                    else
                    {
                        et2.IsInterns = -1;
                    }

                    break;
                case "IsAttendance":
                    if (value != "")
                    { et2.IsAttendance = int.Parse(value); }
                    else
                    {
                        et2.IsAttendance = -1;
                    }

                    break;
                case "ot1":
                    et2.ot1 = value;

                    break;
                case "ot2":
                    et2.ot2 = value;

                    break;
                case "ot3":
                    et2.ot3 = value;
                    break;
                case "string1":
                    et2.string1 = value;
                    break;
                case "string2":
                    et2.string2 = value;
                    break;
                case "string3":
                    et2.string3 = value;
                    break;
                case "string4": //增加string4的复制 add by kagen
                    et2.string4 = value;
                    break;
                case "string5":
                    et2.string5 = value;
                    break;
                case "date1":
                    et2.date1 = value;
                    break;
                case "date2":
                    et2.date2 = value;
                    break;
                case "date3":
                    et2.date3 = value;
                    break;
                case "Sql1":
                    et2.Sql1 = value;
                    break;
                case "Sql2":
                    et2.Sql2 = value;
                    break;
                case "Costcenter":
                    et2.Costcenter = value;
                    break;
                case "TransCard":
                    et2.TransCard = value;
                    break;
                case "age":
                    et2.age = value;
                    break;
                case "Entrychannels":
                    et2.Entrychannels = value;
                    break;
                case "Reportingto":
                    et2.Reportingto = value;
                    break;
                case "HealthExamination":
                    et2.HealthExamination = value;
                    break;
                case "firstworktime":
                    et2.firstworktime = value;
                    break;
                case "Accountplace":
                    et2.Accountplace = value;
                    break;
            }
        }
        private int GetDismissionCardNoCanUseDay(CHEERSQL hs)
        {
            int day = 15;
            try
            {
                string sql = "SELECT PARAMATERVALUE FROM ATDPARAMATER WHERE PARAMATERCODE='2016'";
                DataTable dt = hs.ExecuteSQLforDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    string value = dt.Rows[0]["PARAMATERVALUE"].ToString();
                    day = int.Parse(value);
                }
            }
            catch (Exception ex)
            {
                day = 15;
            }
            return day;
        }

        private bool CheckCardNum(string PersonID, string CardNum)
        {
            CHEERSQL hs = new CHEERSQL();
            int day = GetDismissionCardNoCanUseDay(hs);
            DataTable dt = hs.ExecuteSQLforDataTable("SELECT * FROM PSNACCOUNT WHERE PERSONID <>'" + PersonID +
                "' AND (ACCESSIONSTATE IN (1,2) OR DIMISSIONDATE>='" + DateTime.Today.AddDays(-day).ToString("yyyy-MM-dd") + "') AND CARDNUM ='" + CardNum + "' AND CARDNUM <>'' ");

            if (dt.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        bool saveValue = true;
        string errorInfo = "";
        private bool Effect2(PSNACCOUNTET et2)
        {
            PSNACCOUNTDA paccount2 = new PSNACCOUNTDA();
            PSNADJUSTGROUPDA2 padjustgroup2 = new PSNADJUSTGROUPDA2();
            PSNADJUSTDA2 padjust2 = new PSNADJUSTDA2();
            PSNLABORACCOUNTDA pda = new PSNLABORACCOUNTDA();
            ATDADJUSTTIMEORDERDA atoDA = new ATDADJUSTTIMEORDERDA();
            ImportPersonInforManager ipm = new ImportPersonInforManager();
            Hashtable htunit = ipm.getallbranchinfor();
            Hashtable htjobcode = ipm.getalljobcode();

            PersistBroker b = PersistBroker.Instance();
            try
            {
                et2.ACCESSIONSTATE = et2.ACCESSIONSTATEPRE;
                et2.NOPAYSTATUS = 10;  //默认的停薪留职属性
                paccount2.Update(et2, b);

                //---------异动组新增--------------------------------------------
                PSNADJUSTGROUPET2 gt2 = new PSNADJUSTGROUPET2();
                gt2.OID = Guid.NewGuid().ToString();
                gt2.ADJUSTNAME = "Hire";
                gt2.ADJUSTMAINTYPE = "58_0";
                gt2.PERSONID = et2.PERSONID;
                gt2.EFFECTDATE = et2.ATTENDONDATE;
                gt2.LASTUPDATETIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                gt2.TRANSACTOR = base.InfomationPackage.UserName;
                gt2.ISACCESSIONSTATE = 1;
                gt2.BATCHID = gt2.OID;
                gt2.STATUS = 1;
                gt2.MAINADJUSTID = gt2.OID;
                padjustgroup2.Insert(gt2, b);

                //---------异动明细新增--------------------------------------------
                PSNADJUSTET2 jt2 = new PSNADJUSTET2();
                //在职状态异动
                jt2.PERSONNELID = et2.PERSONID;
                jt2.ADJUSTID = Guid.NewGuid().ToString();
                jt2.EFFECTDATE = et2.ATTENDONDATE;
                jt2.ADJUSTTYPE = 33;
                jt2.STATUS = 1;
                jt2.GROUPMARK = 0;
                jt2.ADJUSTRESULT = et2.ACCESSIONSTATE.ToString();
                if (et2.ACCESSIONSTATE == 2)
                {
                    jt2.ADJUSTRESULTNAME = "正式";
                }
                else
                {
                    jt2.ADJUSTRESULTNAME = "试用";
                }

                jt2.STATEADJUSTNO = 0;
                jt2.ADJUSTACTION = 1;
                jt2.LASTUPDATETIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                jt2.TRANSACTOR = base.InfomationPackage.UserName;
                jt2.IFHASSEND = 0;
                jt2.MAINADJUSTID = gt2.OID;
                jt2.FORMERADJUSTRESULT = "";
                jt2.FORMERADJUSTRESULTNAME = "";
                jt2.ENDDATE = "9999-01-01";
                padjust2.Insert(jt2, b);

                //部门异动
                jt2.ADJUSTID = Guid.NewGuid().ToString();
                jt2.ADJUSTTYPE = 30;
                jt2.ADJUSTRESULT = et2.BRANCHID;
                jt2.ADJUSTRESULTNAME = new CHEER.BusinessLayer.Organize.STDUnitManager().GetCurentUnitByID(et2.BRANCHID).UnitName;
                padjust2.Insert(jt2, b);

                //其他异动 
                CPCJOBCODEBL cjob = new CPCJOBCODEBL();
                if (et2.JOBCODE != "")  //职位
                { AdjustInsert(padjust2, jt2, b, 47, et2.JOBCODE, cjob.getJobCodeName(et2.JOBCODE)); }
                if (et2.RANKID != "") //职等
                { AdjustInsert(padjust2, jt2, b, 50, et2.RANKID, cjob.getRankName(et2.RANKID)); }
                if (et2.RESPONSIBILITYID != "")  //职务
                { AdjustInsert(padjust2, jt2, b, 48, et2.RESPONSIBILITYID, cjob.getResponsibilityName(et2.RESPONSIBILITYID)); }
                if (et2.TITLEID != "")  //职称
                { AdjustInsert(padjust2, jt2, b, 49, et2.TITLEID, cjob.getTitleName(et2.TITLEID)); }

                PersonnelPublicCodeManager pc = new PersonnelPublicCodeManager();
                if (et2.NATIVEPLACEPROPERTYID != "")  //户籍性质
                { AdjustInsert(padjust2, jt2, b, 52, et2.NATIVEPLACEPROPERTYID, pc.GetCodeValueByID(et2.NATIVEPLACEPROPERTYID)); }
                if (et2.JOBTYPEID != "")  //工种
                { AdjustInsert(padjust2, jt2, b, 53, et2.JOBTYPEID, pc.GetCodeValueByID(et2.JOBTYPEID)); }
                if (et2.ARRANGEMENTID != "")   //编制
                { AdjustInsert(padjust2, jt2, b, 54, et2.ARRANGEMENTID, pc.GetCodeValueByID(et2.ARRANGEMENTID)); }
                if (et2.BELONGCORPID != "")   //地区
                { AdjustInsert(padjust2, jt2, b, 55, et2.BELONGCORPID, pc.GetCodeValueByID(et2.BELONGCORPID)); }
                if (et2.EMPLOYEECHARID != "")    //员工性质
                { AdjustInsert(padjust2, jt2, b, 56, et2.EMPLOYEECHARID, pc.GetCodeValueByID(et2.EMPLOYEECHARID)); }
                if (et2.EDUCATIONALLEVELID != "")    //学历
                { AdjustInsert(padjust2, jt2, b, 57, et2.EDUCATIONALLEVELID, pc.GetCodeValueByID(et2.EDUCATIONALLEVELID)); }
                if (et2.AttendanceWay != "")    //
                { AdjustInsert(padjust2, jt2, b, 85, et2.AttendanceWay, pc.GetCodeValueByID(et2.AttendanceWay)); }
                if (et2.OvertimePayment != "")    //
                { AdjustInsert(padjust2, jt2, b, 89, et2.OvertimePayment, pc.GetCodeValueByID(et2.OvertimePayment)); }


                //---------劳动力帐号新增--------------------------------------------
                PSNLABORACCOUNTET le = new PSNLABORACCOUNTET();
                le.LABORDISABLEDATE = "9999-01-01";
                le.LABOREFFECTDATE = et2.ATTENDONDATE;
                le.LABORID = "-/-/-/-/-/-/-";
                le.LABORNAME = "-/-/-/-/-/-/-";
                le.PERSONID = et2.PERSONID;
                le.PSNLABORID = Guid.NewGuid().ToString();
                pda.Insert(le, b);

                //---------默认班次新增--------------------------------------------
                ATDADJUSTTIMEORDERET atoet = new ATDADJUSTTIMEORDERET();
                atoet.BEGINDATE = "1997-01-01";
                atoet.ENDDATE = "9999-01-01";
                atoet.BUSINESSUNITID = base.InfomationPackage.ManageUnitID;
                atoet.OPDATE = DateTime.Now.ToString("yyyy-MM-dd");
                atoet.OPPERSON = base.InfomationPackage.UserName;
                atoet.PERSONID = et2.PERSONID;
                atoet.TIMEORDERID = "1";
                atoDA.Insert(atoet, b);

                //自动关联计薪模式
                CHEER.Platform.DAL.PersistBroker _broker = CHEER.Platform.DAL.PersistBroker.Instance();
                string Sql = "select configvalue from PAYROLLCONFIG where configid='AutoPayPattern' ";
                DataTable Dt = _broker.ExecuteSQLForDst(Sql).Tables[0];
                if (Dt.Rows.Count > 0)
                {
                    if (Dt.Rows[0][0].ToString().ToLower() == "yes")
                    {
                        Sql = "select accountpatternid from PAYACCOUNTPATTERN where isdeleted=0 and isdefault=1";
                        Dt = _broker.ExecuteSQLForDst(Sql).Tables[0];
                        if (Dt.Rows.Count > 0)
                        {
                            PayPersonalPayrollModeLoader _ModeLoader = (PayPersonalPayrollModeLoader)eHRPageServer.GetPalauObject(typeof(PayPersonalPayrollModeLoader));
                            PayPersonalPayrollMode _ModeET = (PayPersonalPayrollMode)eHRPageServer.GetPalauObject(typeof(PayPersonalPayrollMode));
                            _ModeET.BusinessUnitID = base.getBusinessUnitID();
                            _ModeET.USERID = et2.PERSONID;
                            _ModeET.OID = Guid.NewGuid().ToString();
                            _ModeET.PAYROLLMODEID = Dt.Rows[0][0].ToString();
                            _ModeLoader.Add(_ModeET, _broker);
                        }
                    }
                }

                //自动关联所得税群组
                Sql = "select configvalue from PAYROLLCONFIG where configid='AutoPayIncomeTax' ";
                Dt = _broker.ExecuteSQLForDst(Sql).Tables[0];
                if (Dt.Rows.Count > 0)
                {
                    if (Dt.Rows[0][0].ToString().ToLower() == "yes")
                    {
                        Sql = "select incometaxid from PAYINCOMETAX where isdeleted=0 and isdefault=1";
                        Dt = _broker.ExecuteSQLForDst(Sql).Tables[0];
                        if (Dt.Rows.Count > 0)
                        {
                            PayPersonalIncomeTaxLoader _TaxLoader = (PayPersonalIncomeTaxLoader)eHRPageServer.GetPalauObject(typeof(PayPersonalIncomeTaxLoader));
                            PayPersonalIncomeTax _TaxET = (PayPersonalIncomeTax)eHRPageServer.GetPalauObject(typeof(PayPersonalIncomeTax));
                            _TaxET.BusinessUnitID = base.getBusinessUnitID();
                            _TaxET.USERID = et2.PERSONID;
                            _TaxET.TAXPAYGROUPID = Dt.Rows[0][0].ToString();
                            _TaxET.OID = Guid.NewGuid().ToString();
                            _TaxLoader.Add(_TaxET, _broker);
                        }
                    }
                }
                _broker.Close();

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                b.Close();
            }
        }

        private void AdjustInsert(PSNADJUSTDA2 padjust2, PSNADJUSTET2 jt2, PersistBroker b, int adjusttype, string result, string resultname)
        {
            jt2.ADJUSTID = Guid.NewGuid().ToString();
            jt2.ADJUSTTYPE = adjusttype;
            jt2.ADJUSTRESULT = result;
            jt2.ADJUSTRESULTNAME = resultname;
            padjust2.Insert(jt2, b);
        }


        protected void btnEffect_Click(object sender, EventArgs e)
        {
            PersonManager pm = new PersonManager();//验证身份证方法

            CHEER.BusinessLayer.ePersonnel.LogInformManager loger = (CHEER.BusinessLayer.ePersonnel.LogInformManager)eHRPageServer.GetPalauObject(typeof(CHEER.BusinessLayer.ePersonnel.LogInformManager));
            CHEER.CommonLayer.ePersonnel.Data.LogInformData logdata = new CHEER.CommonLayer.ePersonnel.Data.LogInformData();
            construct_log_information(ref logdata);
            PersistBroker _broker = PersistBroker.Instance();
            PSNACCOUNTET et2 = Save2();
            if (saveValue == false)
            {
                base.ShowAlert(errorInfo);

            }
            else
            {
                if (et2 != null)
                {
                    Effect2(et2);
                    btnEffect.Enabled = false;
                    PsnCardChangeBS pcc = new PsnCardChangeBS();
                    pcc.InsertPCC(et2.PERSONID, et2.TRUENAME, null, et2.CARDNUM, 1);
                    #region write log
                    logdata.NOTES = base.getAlert("ZGAIA01102") + et2.EMPLOYEEID + base.getAlert("ZGAIA01095") + base.getAlert("ZGAIA00251");
                    logdata.ACTIONTYPE = (int)LogActionType.insert;
                    logdata.LOGTYPE = (int)LogPropertyType.information;
                    loger.create(logdata);
                    #endregion

                    //自动计算年休假
                    AutoCalculateVacation(et2.EMPLOYEEID);
                    //供应商
                    var personId = et2.PERSONID;
                    var sql = $@"update psnaccount set isSupplier='{isSupplierDDL.SelectedValue}',supplierId='{ddlSupplier.SelectedValue}' where PERSONID='{personId}'";
                    _broker.ExecuteNonQuery(sql);

                    //var sql = @"UPDATE psnaccount set headImageUrl='" + imgLogoUrl.ImageUrl.Replace("'", "''") + "',foreIDUrl='" + imgforeIDUrl.ImageUrl.Replace("'", "''") + "',backIDUrl='" + imgbackIDUrl.ImageUrl.Replace("'", "''") + "',loginName='" + et2.CELLPHONE + "',`password`=md5('" + txtpassword.Text.Replace("'", "''") + "'),`status`='" + ddlstatus1.SelectedValue + "' where PERSONID='" + et2.PERSONID + "';";
                    //_broker.ExecuteNonQuery(sql);
                    base.ShowAlert("保存成功！");
                }
                else
                {
                    #region write log
                    logdata.NOTES = base.getAlert("ZGAIA01102") + et2.EMPLOYEEID + base.getAlert("ZGAIA01095") + base.getAlert("ZGAIA00249");
                    logdata.ACTIONTYPE = (int)LogActionType.insert;
                    logdata.LOGTYPE = (int)LogPropertyType.error;
                    loger.create(logdata);
                    #endregion
                }

            }

        }

        /// <summary>
        /// 判断重复登录名，手机号
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        private bool QueryExistsLoginName(string loginName)
        {
            bool flag = false;
            PersistBroker _broker = PersistBroker.Instance();
            try
            {
                //判断重复登录名，手机号
                var queryExistsLoginName = @"select loginName from psnaccount where loginName = '" + loginName + @"'";
                DataTable dtLoginName = _broker.ExecuteSQLForDst(queryExistsLoginName).Tables[0];
                if (dtLoginName.Rows.Count > 0)
                {
                    flag = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _broker.Close();
            }
            return flag;
        }

        private void AutoCalculateVacation(string EmployeeID)
        {
            try
            {
                //自动计算年休假
                CHEER.BusinessLayer.eAttendance.SpecialVacation.VacationCalculateEngine vce = new CHEER.BusinessLayer.eAttendance.SpecialVacation.VacationCalculateEngine();
                CHEER.BusinessLayer.eAttendance.SpecialVacation.VacationCalcuCondition vcc = new CHEER.BusinessLayer.eAttendance.SpecialVacation.VacationCalcuCondition();
                vcc.BusinessUnitID = "0";
                vcc.CalcuDate = DateTime.Now;

                vcc.InfoPg = new CHEER.Common.InfoPackage();
                vcc.InfoPg.FilterSql = "1=1";
                vcc.InfoPg.IsAboutPerson = true;
                vcc.InfoPg.UserID = "sa";
                vce.InformationPackage = vcc.InfoPg;

                vcc.BusinessUnitID = "0";
                vcc.VacationTypeList = new ArrayList();
                vcc.User = "SA";

                vcc.Label = "0000";
                vcc.DeptList = new ArrayList();
                vcc.DeptList.Add("0");
                vcc.IsConSub = true;
                CHEER.Platform.DAL.PersistBroker b = CHEER.Platform.DAL.PersistBroker.Instance();
                DataTable dtCalType = b.ExecuteSQLForDst("SELECT ID FROM ATDATTENDANCECLASS where CLASSTYPE = 'J' ").Tables[0];
                b.Close();
                if (dtCalType.Rows.Count > 0)
                {
                    for (int m = 0; m < dtCalType.Rows.Count; m++)
                    {
                        vcc.VacationTypeList.Add(dtCalType.Rows[m]["ID"].ToString());
                    }
                }
                vcc.WnoFrom = EmployeeID;
                vcc.WnoTo = EmployeeID;
                vce.Calculate(vcc);
            }
            catch (Exception ex)
            {
                //
            }
        }
        private void construct_log_information(ref CHEER.CommonLayer.ePersonnel.Data.LogInformData logdata)
        {
            logdata.ACTION = (int)LogAction.personinput;
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
            logdata.MODULE = (int)LogModule.epersonnel;
            logdata.OPERATOR = Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERNAME].ToString();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            CheerUI.PageContext.Redirect(getBaseUrl() + "ePersonnel/PSNQuerySet/PSNAdd.aspx?ONLYADD=" + Request.QueryString["ONLYADD"]);
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            CheerUI.PageContext.Redirect(getBaseUrl() + "ePersonnel/PSNQuerySet/PSNGrid.aspx");
        }

        //protected void imgFileLogoUrl_FileSelected(object sender, EventArgs e)
        //{
        //    if (imgFileLogoUrl.HasFile)
        //    {
        //        string fileName = imgFileLogoUrl.ShortFileName;
        //        if (!ValidateFileType(fileName))
        //        {
        //            base.ShowAlert("无效的文件类型！");
        //            return;
        //        }
        //        fileName = fileName.Replace(":", "_").Replace(" ", "_").Replace("\\", "_").Replace("/", "_");
        //        fileName = DateTime.Now.Ticks.ToString() + "_" + fileName;

        //        imgFileLogoUrl.SaveAs(Server.MapPath("~/upload/" + fileName));

        //        imgLogoUrl.ImageUrl = getBaseUrl() + "upload/" + fileName;

        //        // 清空文件上传组件
        //        imgFileLogoUrl.Reset();
        //    }
        //}

        //protected void imgfileforeIDUrl_FileSelected(object sender, EventArgs e)
        //{
        //    if (imgfileforeIDUrl.HasFile)
        //    {
        //        string fileName = imgfileforeIDUrl.ShortFileName;
        //        if (!ValidateFileType(fileName))
        //        {
        //            base.ShowAlert("无效的文件类型！");
        //            return;
        //        }
        //        fileName = fileName.Replace(":", "_").Replace(" ", "_").Replace("\\", "_").Replace("/", "_");
        //        fileName = DateTime.Now.Ticks.ToString() + "_" + fileName;

        //        imgfileforeIDUrl.SaveAs(Server.MapPath("~/upload/" + fileName));

        //        imgforeIDUrl.ImageUrl = getBaseUrl() + "upload/" + fileName;

        //        // 清空文件上传组件
        //        imgfileforeIDUrl.Reset();
        //    }

        //}

        //protected void imgfilebackIDUrl_FileSelected(object sender, EventArgs e)
        //{
        //    if (imgfilebackIDUrl.HasFile)
        //    {
        //        string fileName = imgfilebackIDUrl.ShortFileName;
        //        if (!ValidateFileType(fileName))
        //        {
        //            base.ShowAlert("无效的文件类型！");
        //            return;
        //        }
        //        fileName = fileName.Replace(":", "_").Replace(" ", "_").Replace("\\", "_").Replace("/", "_");
        //        fileName = DateTime.Now.Ticks.ToString() + "_" + fileName;

        //        imgfilebackIDUrl.SaveAs(Server.MapPath("~/upload/" + fileName));

        //        imgbackIDUrl.ImageUrl = getBaseUrl() + "upload/" + fileName;

        //        // 清空文件上传组件
        //        imgfilebackIDUrl.Reset();
        //    }
        //}
    }
}