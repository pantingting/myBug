using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.ePersonnel;
using CHEER.BusinessLayer.ePersonnel.PersonnelBaseInfo;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.DataAccessLayer.ePersonnel;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;
using CHEER.Platform.DAL;
using CHEER.PresentationLayer.Controls;

namespace CHEER.PresentationLayer.ePersonnel.PSNQuerySet
{
    public partial class PSNBasicQuery : CHEERBasePage
    {
        private const string PageFunctionID = "009000040001";
        private const string PageFunctionIDManage = "009000040001";
        private const string PageFunctionIDView = "009000040001";

        /**
         * Added By Kyo 2013-03-06 13:59
         * 将离职15卡号可以重新使用做成参数
         * 参数代码2016
         */
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
        protected void BindSupplier()
        {
            string personId = Request.QueryString["PERSONID"];
            CommonMethod.ExecuteFunc(_broker => {
                var sql = $@"select isSupplier,supplierId from psnaccount where PERSONID='{personId}'";
                var dt = _broker.ExecuteSQLForDst(sql).Tables[0];
                isSupplierDDL.SelectedValue = dt.Rows[0]["isSupplier"].ToString();
                ddlSupplier.SelectedValue = dt.Rows[0]["supplierId"].ToString();
            }, ex => {
                ShowAlert("经销商绑定失败！");
            });
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
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                BindddlSupplier();
                BindSupplier();
                isSupplierDDL_SelectedIndexChanged(null, null);
                //**************** 如果是 合同页面转接过来查看基本信息资料，则屏蔽保存按钮*********
                if (Request.QueryString["changepage"] != null)
                {
                    string changepage = Request.QueryString["changepage"].ToString();
                    if (changepage == "true")
                    {
                        btnUpdate.Hidden = true;
                    }
                }
                //*********************************************************************************
                SecurityChecker sc = base.GetSecurityChecker();
                if (!sc.IsAllow(PageFunctionID))
                {
                    base.ShowAlert(base.getAlert("ZGAIA00809"));
                    return;
                }
                else
                {
                    btnUpdate.Enabled = sc.IsAllow(PageFunctionIDManage);
                }
                btnUpdate.Text = getString("ZGAIA00195");//保存

                var sql = "";
      //          Common.Business.ExecuteSQL(_broker =>
      //          {
      //              sql = "SELECT  headImageUrl,foreIDUrl,backIDUrl,loginName,`password`,status from psnaccount where PERSONID='" + Request.QueryString["PERSONID"] + "';";
      //              var dt = _broker.ExecuteSQLForDst(sql).Tables[0];
      //              if (dt.Rows.Count > 0)
      //              {
      //                  imgLogoUrl.ImageUrl = dt.Rows[0]["headImageUrl"].ToString();
      //                  imgforeIDUrl.ImageUrl = dt.Rows[0]["foreIDUrl"].ToString();
      //                  imgbackIDUrl.ImageUrl = dt.Rows[0]["backIDUrl"].ToString();
						//txtloginName.Text = dt.Rows[0]["loginName"].ToString();
						//txtpassword.Text = dt.Rows[0]["password"].ToString();
						//ddlstatus1.SelectedValue = dt.Rows[0]["status"].ToString();
						//hfimglogo.Text = dt.Rows[0]["headImageUrl"].ToString();
      //                  hfimgforeIDUrl.Text = dt.Rows[0]["foreIDUrl"].ToString();
      //                  hfimgbackIDUrl.Text = dt.Rows[0]["backIDUrl"].ToString();
      //                  hfpassword.Text = dt.Rows[0]["password"].ToString();
      //              }
      //          }, ex =>
      //          {
      //              CheerUI.Alert.ShowInTop(ex.Message);
      //          });

            }
            btnReturn.OnClientClick = CheerUI.ActiveWindow.GetHidePostBackReference();
        }
        protected void isSupplierDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSupplier.Hidden = isSupplierDDL.SelectedValue == "0" ? true : false;
        }
        protected override void OnInitComplete(EventArgs e)
        {
            string PERSONID = Request.QueryString["PERSONID"];
            //personid.Value = PERSONID;
            string QSID = "";

            CHEER.Platform.DAL.PersistBroker broker = CHEER.Platform.DAL.PersistBroker.Instance();
            string sql = "select * from PSNQUERYSET where QSTYPE='PSN_BASE'";
            DataTable dt = broker.ExecuteDataset(sql).Tables[0];
            broker.Close();
            foreach (DataRow dr in dt.Rows)
            {
                QSID = dr["QSID"].ToString();
            }
            ViewState["QSIS"] = QSID;

            //查询employeeid的值 add by kagen at 20120621
            DataTable dt_epid = broker.ExecuteDataset("select employeeid from psnaccount where personid='" + PERSONID + "'").Tables[0];
            broker.Close();
            if (dt_epid.Rows.Count > 0)
            {
                ViewState["OldEmployeeid"] = "";
                ViewState["OldEmployeeid"] = dt_epid.Rows[0][0].ToString();
            }

            //***************************************

            LoadCondition(PERSONID, QSID);
            LoadControlValue(PERSONID, QSID);
            base.OnInitComplete(e);
        }

        private void LoadCondition(string PERSONID, string QSID)
        {
            DataTable dt = new PSNQUERYSETLoader().QueryDetailSet(QSID);
            string lang = InfomationPackage.LanguageCulture;
            string tit = "QDSNAME";
            if (lang == "EN-US")
                tit = "QDS4";
            if (lang == "ZH-TW")
                tit = "QDS5";


            //调整组织位置,将其放在合理的位置上(第一个位置或者第二个位置)
            var tempItemArray = new object[] { };
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["QDSTYPE"].ToString() == "DEPT")
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

                string QDSFIELD = dt.Rows[i]["QDSFIELD"].ToString();
                string QDSORDER = dt.Rows[i]["QDSORDER"].ToString();
                string QDSNAME = dt.Rows[i][tit].ToString();
                string QDSTYPE = dt.Rows[i]["QDSTYPE"].ToString();
                string QDSID = dt.Rows[i]["QDSID"].ToString();
                string QDSISREAD = dt.Rows[i]["QDSISREAD"].ToString();

                switch (QDSTYPE)
                {
                    case "STATE"://在职状态型

                        CheerUI.DropDownList ddlState = new CheerUI.DropDownList();
                        ddlState.EnableCheckBoxSelect = true;
                        ddlState.EnableMultiSelect = true;
                        if (QDSISREAD == "YES")
                        {
                            ddlState.Enabled = true;
                        }
                        ddlState.ID = QDSID;
                        ddlState.Label = QDSNAME;

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
                        if (QDSISREAD == "YES")
                        {
                            datePicker.Enabled = false;
                        }
                        datePicker.ID = QDSID;
                        datePicker.Label = QDSNAME;
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
                        StdBranchLoader.ID = QDSID;

                        CheerUI.UserControlConnector userControl = new CheerUI.UserControlConnector();
                        userControl.Controls.Add(StdBranchLoader);
                        StdBranchLoader.Label = QDSNAME;
                        formTempRow.Items.Add(userControl);
                        if (formTempRow.Items.Count == 1)
                        {
                            formTempRow.ColumnWidths = "0.66 0.34 ";
                        }
                        else if (formTempRow.Items.Count == 2)
                        {
                            formTempRow.ColumnWidths = "0.33 0.67 ";
                        }
                        if (QDSISREAD == "YES")
                        {
                            StdBranchLoader.IsEnable = false;
                        }
                        m += 2;
                        break;
                    case "DEFAULTLIST":   //自定义列表型
                        CheerUI.DropDownList ddlDefaultList = new CheerUI.DropDownList();
                        if (QDSISREAD == "YES")
                        {
                            ddlDefaultList.Enabled = false;
                        }
                        ddlDefaultList.Label = QDSNAME;
                        ddlDefaultList.ID = QDSID;
                        formTempRow.Items.Add(ddlDefaultList);
                        m++;
                        break;
                    case "SQLLIST":       //SQL列表型
                        CheerUI.DropDownList ddlSqlList = new CheerUI.DropDownList();
                        if (QDSISREAD == "YES")
                        {
                            ddlSqlList.Enabled = false;
                        }
                        ddlSqlList.Label = QDSNAME;
                        ddlSqlList.ID = QDSID;

                        formTempRow.Items.Add(ddlSqlList);
                        m++;
                        break;
                    case "BOOL":
                        CheerUI.CheckBox checkField = new CheerUI.CheckBox();

                        checkField.Label = QDSNAME;
                        checkField.ID = QDSID;
                        formTempRow.Items.Add(checkField);
                        m++;
                        break;
                    case "INT":
                        CheerUI.NumberBox numberField = new CheerUI.NumberBox();
                        if (QDSISREAD == "YES")
                        {
                            numberField.Enabled = false;
                        }
                        numberField.Label = QDSNAME;
                        numberField.ID = QDSID;
                        formTempRow.Items.Add(numberField);
                        m++;
                        break;
                    default:
                        CheerUI.TextBox txtField = new CheerUI.TextBox();
                        if (QDSISREAD == "YES")
                        {
                            txtField.Enabled = false;
                        }

                        txtField.Label = QDSNAME;
                        txtField.ID = QDSID;
                        if (QDSFIELD == "TRUENAME")
                        {
                            ViewState["TRUENAME"] = txtField.ID;
                        }
                        if (QDSFIELD == "CARDNUM")
                        {
                            ViewState["CARDNUM"] = txtField.ID;
                        }
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
        public void LoadControlValue(string PERSONID, string QSID)
        {

            string QDSVALUELIST = "";
            //加载控件的值
            string oldfields = "";
            DataTable dt1 = new PSNQUERYSETLoader().QueryDetailSet(QSID);
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                string QDSFIELD = dt1.Rows[i]["QDSFIELD"].ToString();
                QDSVALUELIST = dt1.Rows[i]["QDSVALUELIST"].ToString();
                oldfields = oldfields + QDSFIELD + ",";
            }
            string fields = oldfields.Substring(0, oldfields.Length - 1);
            string sql = "select " + fields + " from PSNACCOUNT where PERSONID='" + PERSONID + "'";

            CHEER.Platform.DAL.PersistBroker broker = CHEER.Platform.DAL.PersistBroker.Instance();
            DataTable dt2 = broker.ExecuteSQLForDst(sql).Tables[0];      //查询出记录
            broker.Close();

            DataTable dt3 = new PSNQUERYSETLoader().QueryDetailSet(QSID);//取出控件类型

            if (dt2.Rows.Count == 0)
            {
                return;
            }
            for (int j = 0; j < dt2.Rows.Count; j++)
            {
                for (int k = 0; k < dt2.Columns.Count; k++)
                {
                    string value = dt2.Rows[j][k].ToString();
                    string QDSTYPE = dt3.Rows[k]["QDSTYPE"].ToString();
                    string QDSID = dt3.Rows[k]["QDSID"].ToString();

                    switch (QDSTYPE)
                    {
                        case "STATE":
                            CheerUI.DropDownList ddlState = (CheerUI.DropDownList)CheerUI.ControlUtil.FindControl(QDSID);
                            ddlState.SelectedValueArray = value.Split(',');
                            break;
                        case "DATE":
                            CheerUI.DatePicker datePicker = (CheerUI.DatePicker)CheerUI.ControlUtil.FindControl(QDSID);
                            datePicker.Text = value;
                            break;
                        case "DEPT":
                            StdBranchLoader StdBranchLoader = (StdBranchLoader)CheerUI.ControlUtil.FindControl(QDSID);
                            StdBranchLoader.SetSelectBranch(value);
                            break;
                        case "DEFAULTLIST":
                            CheerUI.DropDownList ddlDefaultList = (CheerUI.DropDownList)CheerUI.ControlUtil.FindControl(QDSID);
                            ddlDefaultList.Items.Add("", "");
                            DataTable dt4 = new PSNQUERYSETLoader().QueryDetailSet(QSID);

                            QDSVALUELIST = dt1.Rows[k]["QDSVALUELIST"].ToString();


                            if (QDSVALUELIST != " " && QDSVALUELIST != null)
                            {
                                string[] valuelist = QDSVALUELIST.Split(',');
                                foreach (string value1 in valuelist)
                                {
                                    string[] arr = value1.Split('|');
                                    CheerUI.ListItem item = new CheerUI.ListItem();
                                    item.Text = arr[1];
                                    item.Value = arr[0];
                                    ddlDefaultList.Items.Add(item);
                                }
                            }
                            ddlDefaultList.SelectedValue = value;
                            break;
                        case "SQLLIST":
                            CheerUI.DropDownList ddlSqlList = (CheerUI.DropDownList)CheerUI.ControlUtil.FindControl(QDSID);

                            QDSVALUELIST = dt1.Rows[k]["QDSVALUELIST"].ToString();
                            DataTable dt = new DataTable();
                            PersistBroker _broker = PersistBroker.Instance();
                            try
                            {
                                dt = _broker.ExecuteSQLForDst(QDSVALUELIST).Tables[0];
                            }
                            catch (Exception ee) { throw ee; }
                            finally { _broker.Close(); }
                            ddlSqlList.Items.Clear();
                            ddlSqlList.Items.Add(new CheerUI.ListItem("", ""));
                            if (dt.Rows.Count > 0)
                            {
                                string strValue = "";
                                string strText = "";
                                CheerUI.ListItem item;
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    item = new CheerUI.ListItem();
                                    strValue = dt.Rows[i][0].ToString();
                                    strText = dt.Rows[i][1].ToString();
                                    item.Text = strText;
                                    item.Value = strValue;

                                    ddlSqlList.Items.Add(item);
                                }
                            }
                            ddlSqlList.SelectedValue = value;
                            break;
                        case "BOOL":
                            CheerUI.CheckBox checkField = (CheerUI.CheckBox)CheerUI.ControlUtil.FindControl(QDSID);

                            if (value == "1")
                            {
                                checkField.Checked = true;
                            }
                            else
                            {
                                checkField.Checked = false;
                            }
                            break;
                        case "INT":
                            CheerUI.NumberBox numberField = (CheerUI.NumberBox)CheerUI.ControlUtil.FindControl(QDSID);
                            numberField.Text = value;
                            break;
                        default:
                            CheerUI.TextBox txtField = (CheerUI.TextBox)CheerUI.ControlUtil.FindControl(QDSID);
                            txtField.Text = value;
                            break;
                    }
                }
            }
            if (ViewState["TRUENAME"] != null)
            {
                CheerUI.TextBox tx = (CheerUI.TextBox)CheerUI.ControlUtil.FindControl(ViewState["TRUENAME"].ToString());
                truename.Text = tx.Text;
            }
            if (ViewState["CARDNUM"] != null)
            {
                CheerUI.TextBox tx = (CheerUI.TextBox)CheerUI.ControlUtil.FindControl(ViewState["CARDNUM"].ToString());
                cardnum.Text = tx.Text;
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            bool isAnnualLeave = true;//是否约定年假
            bool isVacationDays = false;//年假天数

            string showString = "";//输出异常信息
            string value = "";
            PSNACCOUNTDA pda2 = new PSNACCOUNTDA();

            string sql = "update PSNACCOUNT set ";
            DataTable dt1 = new PSNQUERYSETLoader().QueryDetailSet(ViewState["QSIS"].ToString());
            string ATTENDONDATE = "";
            string DIMISSIONDATE = "";
            string PROBATIONENDDATE = "";
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                string QDSFIELD = dt1.Rows[i]["QDSFIELD"].ToString();
                string QDSTYPE = dt1.Rows[i]["QDSTYPE"].ToString();
                string QDSNAME = dt1.Rows[i]["QDSNAME"].ToString();
                string QCSID = dt1.Rows[i]["QDSID"].ToString();
                sql = sql + QDSFIELD;
                switch (QDSTYPE)
                {
                    case "STATE":
                        CheerUI.DropDownList ddlState = (CheerUI.DropDownList)CheerUI.ControlUtil.FindControl(QCSID);
                        for (int l = 0; l < ddlState.SelectedValueArray.Length; l++)
                        {
                            if (value.Length > 0)
                            {
                                value += "," + ddlState.SelectedValueArray[l].ToString();
                            }
                            else
                            {
                                value += ddlState.SelectedValueArray[l].ToString();
                            }
                        }
                        sql = sql + "='" + value + "'" + ",";
                        break;
                    case "DATE":
                        CheerUI.DatePicker datePicker = (CheerUI.DatePicker)CheerUI.ControlUtil.FindControl(QCSID);
                        value = datePicker.Text;

                        if (QDSFIELD == "ATTENDONDATE")
                        {
                            ATTENDONDATE = value;
                        }
                        if (QDSFIELD == "DIMISSIONDATE")
                        {
                            DIMISSIONDATE = value;
                        }
                        if (QDSFIELD == "PROBATIONENDDATE")
                        {
                            PROBATIONENDDATE = value;
                        }
                        if (ATTENDONDATE != "" && DIMISSIONDATE != "" && DateTime.Parse(ATTENDONDATE) > DateTime.Parse(DIMISSIONDATE)
                             || ATTENDONDATE != "" && PROBATIONENDDATE != "" && DateTime.Parse(ATTENDONDATE) > DateTime.Parse(PROBATIONENDDATE))//判断入职日是否为离职日之前
                        {
                            if (InfomationPackage.LanguageCulture == "EN-US")
                            {
                                CheerUI.Alert.ShowInTop("DIMISSIONDATE can't later then ATTENDONDATE or PROBATIONENDDATE!");
                            }
                            else
                            {
                                CheerUI.Alert.ShowInTop("保存失败，入职日晚于离职日或试满日!");
                            }
                            return;
                        }
                        sql = sql + "='" + value + "'" + ",";
                        break;
                    case "DEPT":
                        StdBranchLoader StdBranchLoader = (StdBranchLoader)CheerUI.ControlUtil.FindControl(QCSID);

                        value = StdBranchLoader.GetSelectBranchItem().Value;
                        sql = sql + "='" + value + "'" + ",";
                        break;
                    case "DEFAULTLIST":
                        CheerUI.DropDownList ddlDefaultList = (CheerUI.DropDownList)CheerUI.ControlUtil.FindControl(QCSID);

                        value = ddlDefaultList.SelectedValue;

                        if (QDSFIELD == "ACCESSIONSTATE" || QDSFIELD == "GENDER" || QDSFIELD == "DLIDL")
                        {
                            sql = sql + "='" + value + "',";

                        }
                        else
                        {
                            if (QDSFIELD == "AnnualLeave" && (value == "0" || value == "-1" || value == ""))
                            {
                                isAnnualLeave = false;
                            }
                            if (value == "")
                            {
                                value = "-1";
                            }
                            sql = sql + "='" + value + "'" + ",";
                        }
                        break;
                    case "SQLLIST":
                        CheerUI.DropDownList ddlSqlList = (CheerUI.DropDownList)CheerUI.ControlUtil.FindControl(QCSID);
                        value = ddlSqlList.SelectedValue;

                        sql = sql + "='" + value + "'" + ",";

                        //加入证件类型
                        if (QDSFIELD == "CERTIFICATETYPEID")
                        {
                            ViewState["IDTYPE"] = ddlSqlList.ID;
                        }

                        break;
                    case "BOOL":
                        CheerUI.CheckBox checkField = (CheerUI.CheckBox)CheerUI.ControlUtil.FindControl(QCSID);

                        if (checkField.Checked)
                        {
                            value = "1";
                        }
                        else
                        {
                            value = "0";
                        }

                        sql = sql + "='" + value + "'" + ",";

                        break;
                    case "INT":
                        CheerUI.NumberBox numberField = (CheerUI.NumberBox)CheerUI.ControlUtil.FindControl(QCSID);
                        value = numberField.Text;
                        int intValue = 0;
                        double douValue = 0;
                        if (QDSFIELD == "overtimeSettlementUpper" || QDSFIELD == "VacationDays")
                        {
                            if (double.TryParse(numberField.Text, out douValue))
                            {
                                sql = sql = sql + "=" + value + ",";
                            }
                            else
                            {
                                sql = sql.Substring(0, sql.LastIndexOf(',') + 1);
                                //isVacationValue = true;
                                showString = showString + QDSNAME + getString("ZGAIA05531") + "\r\n";

                            }
                        }
                        else if (int.TryParse(numberField.Text, out intValue))
                        {
                            sql = sql + "=" + value + ",";

                        }

                        else
                        {
                            sql = sql.Substring(0, sql.LastIndexOf(',') + 1);
                            //isVacationValue = true;
                            showString = showString + QDSNAME + getString("ZGAIA05531") + "\r\n";

                        }
                        if (QDSFIELD == "overtimeSettlementUpper" && douValue < 0)
                        {
                            showString = showString + getString("ZGAIA05533") + "\r\n";
                        }
                        if (QDSFIELD == "VacationDays" && douValue < 0)
                        {
                            showString = showString + getString("ZGAIA05535") + "\r\n";
                        }
                        if (QDSFIELD == "VacationDays" && (value != "" && value != null && value != "0" && value != "0.00"))
                        {
                            isVacationDays = true;
                        }


                        break;
                    default:

                        CheerUI.TextBox txtField = (CheerUI.TextBox)CheerUI.ControlUtil.FindControl(QCSID);
                        value = txtField.Text.Trim().DBReplace();
                        txtField.Text = value;

                        sql = sql + "='" + value + "'" + ",";
                        //判定是否为工号，如果为工号，则判定是否重复！ add by kagen at 20120621
                        if (QDSFIELD == "EMPLOYEEID")
                        {
                            if (ViewState["OldEmployeeid"].ToString() != txtField.Text)
                            {
                                string sql_sel = "Select count(*) from psnaccount where employeeid='" + txtField.Text + "'";
                                PersistBroker _broker = PersistBroker.Instance();
                                DataTable dt_sum = _broker.ExecuteDataset(sql_sel).Tables[0];
                                _broker.Close();
                                if (Convert.ToInt32(dt_sum.Rows[0][0]) > 0)
                                {
                                    CheerUI.Alert.ShowInTop("工号重复！请重新输入！");
                                    return;
                                }

                            }
                        }

                        if (QDSFIELD == "CARDNUM")  //卡号，需要判断是否合理
                        {
                            value = value.Trim();
                            if (value != "" && !value.All(char.IsDigit))
                            {
                                CheerUI.Alert.ShowInTop("卡号必须是数字！请重新输入！");
                                return;
                            }

                            if (value != "" && !CheckCardNum(Request.QueryString["PERSONID"], value))  //检查卡号是否和已有在职人员重复
                            {
                                CheerUI.Alert.ShowInTop("卡号和已有在职人员重复！");
                                return;
                            }
                        }
                        if (QDSFIELD == "CELLPHONE")  //手机号，需要判断是否合理
                        {
                            value = value.Trim();

                            if (!CommonUse.Common.IsHandset(value))
                            {
                                CheerUI.Alert.ShowInTop("手机号码格式不正确！");
                                return;
                            }
                        }
                        break;
                }
            }
            CommonMethod.ExecuteFunc(_broker => {
                //供应商
                string personId = Request.QueryString["PERSONID"];
                var supplierSql = $@"update psnaccount set isSupplier='{isSupplierDDL.SelectedValue}',supplierId='{ddlSupplier.SelectedValue}' where PERSONID='{personId}'";
                _broker.ExecuteNonQuery(supplierSql);
            }, ex => {

            });
            if (isAnnualLeave == false && isVacationDays == true)
            {

                showString = showString + getString("ZGAIA05532") + "\r\n";
            }
            if (showString != "")
            {
                base.ShowAlert(showString);
            }
            else
            {

                string sql1 = sql.Substring(0, sql.Length - 1);
                string person = Request.QueryString["PERSONID"];
                sql1 = sql1 + " where PERSONID='" + person + "'";

                CHEER.Platform.DAL.PersistBroker broker = CHEER.Platform.DAL.PersistBroker.Instance();
                if (broker.ExecuteSQL(sql1))
                {
                    PsnCardChangeBS pccBS = new PsnCardChangeBS();
                    pccBS.InsertPCC(personid.Text, "", "", "", 4);
                    bool name = false;
                    bool card = false;
                    if (ViewState["TRUENAME"] != null)
                    {
                        CheerUI.TextBox txtField = (CheerUI.TextBox)CheerUI.ControlUtil.FindControl(ViewState["TRUENAME"].ToString());
                        if (truename.Text != txtField.Text)
                            name = true;
                    }
                    if (ViewState["CARDNUM"] != null)
                    {
                        CheerUI.TextBox txtField = (CheerUI.TextBox)CheerUI.ControlUtil.FindControl(ViewState["CARDNUM"].ToString());
                        if (cardnum.Text != txtField.Text)
                            card = true;
                    }
                    if (name && card)
                    {
                        CheerUI.TextBox tbname = (CheerUI.TextBox)CheerUI.ControlUtil.FindControl(ViewState["TRUENAME"].ToString());
                        CheerUI.TextBox tbcard = (CheerUI.TextBox)CheerUI.ControlUtil.FindControl(ViewState["CARDNUM"].ToString());
                        PsnCardChangeBS pcc = new PsnCardChangeBS();
                        if (cardnum.Text == "")
                        {
                            pcc.InsertPCC(personid.Text, tbname.Text, cardnum.Text, tbcard.Text, 1);
                        }
                        else
                        {
                            pcc.InsertPCC(personid.Text, tbname.Text, cardnum.Text, tbcard.Text, 3);
                        }
                        truename.Text = tbname.Text;
                        cardnum.Text = tbcard.Text;
                    }
                    else if (name)
                    {
                        CheerUI.TextBox tbname = (CheerUI.TextBox)CheerUI.ControlUtil.FindControl(ViewState["TRUENAME"].ToString());
                        if (ViewState["CARDNUM"] != null)
                        {
                            PsnCardChangeBS pcc = new PsnCardChangeBS();
                            pcc.InsertPCC(personid.Text, tbname.Text, cardnum.Text, cardnum.Text, 3);
                        }
                        else
                        {
                            PsnCardChangeBS pcc = new PsnCardChangeBS();
                            pcc.insertByID(personid.Text, tbname.Text);
                        }
                        truename.Text = tbname.Text;
                    }
                    else if (card)
                    {
                        CheerUI.TextBox tbcard = (CheerUI.TextBox)CheerUI.ControlUtil.FindControl(ViewState["CARDNUM"].ToString());
                        if (ViewState["TRUENAME"] != null)
                        {
                            PsnCardChangeBS pcc = new PsnCardChangeBS();
                            if (cardnum.Text == "")
                            {
                                pcc.InsertPCC(personid.Text, truename.Text, cardnum.Text, tbcard.Text, 1);
                            }
                            else
                                pcc.InsertPCC(personid.Text, truename.Text, cardnum.Text, tbcard.Text, 3);
                        }
                        else
                        {
                            PsnCardChangeBS pcc = new PsnCardChangeBS();
                            pcc.insertByCard(personid.Text, cardnum.Text, tbcard.Text);
                        }
                        cardnum.Text = tbcard.Text;
                    }

                    OnInitComplete(null);
                    base.ShowAlert(getString("ZGAIA01916"));
                }

            }


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


        //        hfimglogo.Text = imgLogoUrl.ImageUrl;

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
        //        hfimgforeIDUrl.Text = imgforeIDUrl.ImageUrl;
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
        //        hfimgbackIDUrl.Text = imgbackIDUrl.ImageUrl;

        //    }
        //}

    }
}