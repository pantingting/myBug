using CHEER.BusinessLayer.ePersonnel;
using CHEER.BusinessLayer.Organize;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.Common.Schema;
using CHEER.CommonLayer.ePersonnel.Schema;
using CHEER.PresentationLayer;
using CHEER.Platform.DAL;
using CHEER.Platform.DAL.SQLCenter;
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
using CHEER.CommonLayer.ePersonnel.Data;

namespace CHEER.PresentationLayer.Controls
{
    public partial class UserQuery : BaseControls
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            OnInitComplete();
        }

        /// <summary>
        /// 在职状态属性
        /// </summary>
        private bool _AcessionStatus_Dimission = true;
        private bool _AcessionStatus_Export = true;
        private bool _AcessionStatus_Probation = true;
        private bool _AcessionStatus_PromotingProbation = true;
        private bool _AcessionStatus_Regular = true;
        private bool _AcessionStatus_Retired = true;
        private bool _AcessionStatus_Unchecked = false;

        /// <summary>
        /// 离职
        /// </summary>
        public bool AcessionStatus_Dimission
        {
            set
            {
                _AcessionStatus_Dimission = value;
            }
            get
            {
                return _AcessionStatus_Dimission;
            }
        }
        /// <summary>
        /// 调出
        /// </summary>
        public bool AcessionStatus_Export
        {
            set
            {
                _AcessionStatus_Export = value;
            }
            get
            {
                return _AcessionStatus_Export;
            }
        }
        /// <summary>
        /// 试用
        /// </summary>
        public bool AcessionStatus_Probation
        {
            set
            {
                _AcessionStatus_Probation = value;
            }
            get
            {
                return _AcessionStatus_Probation;
            }
        }
        /// <summary>
        /// 晋级试用
        /// </summary>
        public bool AcessionStatus_PromotingProbation
        {
            set
            {
                _AcessionStatus_PromotingProbation = value;
            }
            get
            {
                return _AcessionStatus_PromotingProbation;
            }
        }
        /// <summary>
        /// 正式
        /// </summary>
        public bool AcessionStatus_Regular
        {
            set
            {
                _AcessionStatus_Regular = value;
            }
            get
            {
                return _AcessionStatus_Regular;
            }
        }
        /// <summary>
        /// 退休
        /// </summary>
        public bool AcessionStatus_Retired
        {
            set
            {
                _AcessionStatus_Retired = value;
            }
            get
            {
                return _AcessionStatus_Retired;
            }
        }
        /// <summary>
        /// 未接受
        /// </summary>
        public bool AcessionStatus_Unchecked
        {
            set
            {
                _AcessionStatus_Unchecked = value;
            }
            get
            {
                return _AcessionStatus_Unchecked;
            }
        }

        /// <summary>
        /// 用于构建组织下拉框的Security_Checker
        /// </summary>
        public SecurityChecker Security_Checker
        {
            set
            {
                ViewState["Security_Checker"] = value;
            }
            get
            {
                return (SecurityChecker)ViewState["Security_Checker"];
            }
        }
        /// <summary>
        /// 用于构建组织下拉框的FunFriendlyID （查询功能权限编号）
        /// </summary>
        ///[Obsolete("权限接口修改，实际赋值时请使用功能权限编号而不要使用友好ID",false)]
        public string FunFriendlyID
        {
            set
            {
                ViewState["FunFriendlyID"] = value;
            }
            get
            {
                return ViewState["FunFriendlyID"].ToString();
            }
        }

        /// <summary>
        /// 是否加入管理单元限制
        /// </summary>
        private bool _IsHaveManageUnit = false;
        public bool IsHaveManageUnit
        {
            get
            {
                if (ViewState["IsHaveManageUnit"] != null)
                {
                    return (bool)ViewState["IsHaveManageUnit"];
                }
                else
                {
                    return _IsHaveManageUnit;
                }
            }
            set
            {
                _IsHaveManageUnit = value;
                ViewState["IsHaveManageUnit"] = value;
            }
        }
        /// <summary>
        /// 是否加入管理单元限制
        /// </summary>
        private string _ManageUnitID;
        public string ManageUnitID
        {
            get
            {
                if (ViewState["ManageUnitID"] != null)
                {
                    return ViewState["ManageUnitID"].ToString();
                }
                else
                {
                    return new CHEERBasePage().getBusinessUnitID();
                }
            }
            set
            {
                _ManageUnitID = value;
                ViewState["ManageUnitID"] = value;
            }
        }
        /// <summary>
        /// 是否包含子组织选项可以被编辑 ，true表示可以，false表示不可以
        /// </summary>
        private bool _IsIncludeSubBranchCanEdit = true;
        public bool IsIncludeSubBranchCanEdit
        {
            get
            {
                if (ViewState["IsIncludeSubBranchCanEdit"] != null)
                {
                    return (bool)ViewState["IsIncludeSubBranchCanEdit"];
                }
                else
                {
                    return _IsIncludeSubBranchCanEdit;
                }
            }
            set
            {
                _IsIncludeSubBranchCanEdit = value;
                ViewState["IsIncludeSubBranchCanEdit"] = value;
            }
        }
        /// <summary>
        /// 是否包含子组织选项，true表示包括，false表示不包括。
        /// </summary>
        private bool _IsIncludeSubBranch = true;
        public bool IsIncludeSubBranch
        {
            set
            {
                _IsIncludeSubBranch = value;
                ViewState["IsIncludeSubBranch"] = value;
            }
            get
            {
                if (ViewState["IsIncludeSubBranch"] != null)
                {
                    return (bool)ViewState["IsIncludeSubBranch"];
                }
                else
                {
                    return _IsIncludeSubBranch;
                }
            }
        }
        /// <summary>
        /// 包括停用组织的复选框是否可见，默认不可见
        /// </summary>
        private bool _IsShowIncludeStopUnit = false;
        public bool IsShowIncludeStopUnit
        {
            get
            {
                if (ViewState["IsShowIncludeStopUnit"] != null)
                {
                    return (bool)ViewState["IsShowIncludeStopUnit"];
                }
                else
                {
                    return _IsShowIncludeStopUnit;
                }
            }
            set
            {
                _IsShowIncludeStopUnit = value;
                ViewState["IsShowIncludeStopUnit"] = value;
            }
        }
        /// <summary>
        /// 是否包含停用组织，默认不包含
        /// </summary>
        private bool _IsIncludeStopUnit = false;
        public bool IsIncludeStopUnit
        {
            get
            {
                if (ViewState["IsIncludeStopUnit"] != null)
                {
                    return (bool)ViewState["IsIncludeStopUnit"];
                }
                else
                {
                    return _IsIncludeStopUnit;
                }
            }
            set
            {
                _IsIncludeStopUnit = value;
                ViewState["IsIncludeStopUnit"] = value;
            }
        }

        protected void OnInitComplete()
        {
            LoadCondition();
        }

        void StdBranchLoader_SelectStopUnit(object sender, CHEER.PresentationLayer.Controls.StdBranchLoader.SelectStopUnitEventArgs e)
        {
            IsIncludeStopUnit = e.IsStop;
        }

        /// <summary>
        /// 返回SQLSelectEntity
        /// </summary>
        /// <returns></returns>
        public SQLSelectEntity getSelectEntityWithNoSelectItems2()
        {
            SQLSelectEntity _sqlInfo = new SQLSelectEntity();
            //配置FROM子句
            _sqlInfo.FromTableList.Add("0", "PSNACCOUNT");
            _sqlInfo.FromTableList.Add("1", "ORGSTDSTRUCT");
            //配置WHERE子句
            TableField _fieldIDSel1 = new TableField("PSNACCOUNT", "BRANCHID");
            TableField _fieldIDSel2 = new TableField("ORGSTDSTRUCT", "UNITID");
            SQLRelation _sqlRelation = SQLRelation.EQUAL;
            _sqlInfo.SQLCondition = new SQLConditionItem(_fieldIDSel1, _sqlRelation, _fieldIDSel2);
            ArrayList getItem = getSelectItemArray();
            if (getItem.Count > 0)
            {
                for (int intx = 0; intx < getItem.Count; intx++)
                {
                    _sqlInfo.SQLCondition = new SQLConditionUnit(_sqlInfo.SQLCondition, SQLLogicOperator.AND, (SQLConditionItem)getItem[intx]);
                }
            }
            return _sqlInfo;
        }
        public SQLSelectEntity getSelectEntityWithNoSelectItems()
        {
            SQLSelectEntity _sqlInfo = new SQLSelectEntity();
            //配置FROM子句
            _sqlInfo.FromTableList.Add("0", "PSNACCOUNT");
            FieldConst fieldConst = new FieldConst("1");
            ArrayList arr = new ArrayList();
            arr.Add("1");
            SQLRelation _sqlRelation = SQLRelation.EQUAL;
            SQLConditionItem _sqlItem = new SQLConditionItem(fieldConst, _sqlRelation, arr);
            _sqlInfo.SQLCondition = new SQLConditionUnit(_sqlItem, SQLLogicOperator.AND, _sqlItem);
            ArrayList getItem = getSelectItemArray();
            if (getItem.Count > 0)
            {
                for (int intx = 0; intx < getItem.Count; intx++)
                {
                    _sqlInfo.SQLCondition = new SQLConditionUnit(_sqlInfo.SQLCondition, SQLLogicOperator.AND, (SQLConditionItem)getItem[intx]);
                }
            }
            return _sqlInfo;
        }
        /// <summary>
        /// 返回带字段的SQLSelectEntity
        /// </summary>
        /// <returns></returns>
        public SQLSelectEntity getSelectEntity()
        {
            SQLSelectEntity sse = getSelectEntityWithNoSelectItems2();
            sse = GetSelectItemsEntity(sse, getSelectItem());
            return sse;
        }
        private SQLSelectEntity GetSelectItemsEntity(SQLSelectEntity _sqlInfo, ArrayList schemaTable)
        {
            foreach (object o in schemaTable)
            {
                TableField _field = new TableField(((SchemaKey)o).TableName, ((SchemaKey)o).ColName, ((SchemaKey)o).ColFullName, ((SchemaKey)o).ColType);
                _sqlInfo.SelectItems.Add(_field);
            }
            return _sqlInfo;
        }
        private ArrayList getSelectItem()
        {
            ArrayList _al = new ArrayList();
            _al.Add(new SchemaKey(PSNAccountSchema.PSNACCOUNT_TABLENAME, PSNAccountSchema.PSNACCOUNT_PERSONID, "", SQLDataType.String));
            _al.Add(new SchemaKey(PSNAccountSchema.PSNACCOUNT_TABLENAME, PSNAccountSchema.PSNACCOUNT_EMPLOYEEID, "", SQLDataType.String));
            _al.Add(new SchemaKey(PSNAccountSchema.PSNACCOUNT_TABLENAME, PSNAccountSchema.PSNACCOUNT_LASTNAME, "", SQLDataType.String));
            _al.Add(new SchemaKey(PSNAccountSchema.PSNACCOUNT_TABLENAME, PSNAccountSchema.PSNACCOUNT_MIDDLENAME, "", SQLDataType.String));
            _al.Add(new SchemaKey(PSNAccountSchema.PSNACCOUNT_TABLENAME, PSNAccountSchema.PSNACCOUNT_FIRSTNAME, "", SQLDataType.String));
            _al.Add(new SchemaKey(PSNAccountSchema.PSNACCOUNT_TABLENAME, PSNAccountSchema.PSNACCOUNT_TRUENAME, "", SQLDataType.String));
            _al.Add(new SchemaKey(PSNAccountSchema.PSNACCOUNT_TABLENAME, PSNAccountSchema.PSNACCOUNT_BRANCHID, "", SQLDataType.String));
            _al.Add(new SchemaKey(PSNAccountSchema.PSNACCOUNT_TABLENAME, PSNAccountSchema.PSNACCOUNT_POSITIONID, "", SQLDataType.String));
            return _al;
        }

        public PersonQueryData getQueryData()
        {
            PersonQueryData pqd = new PersonQueryData();
            StdBranchLoader StdBranchLoader = (StdBranchLoader)this.FindControl("StdBranchLoader");
            if (StdBranchLoader != null)
            {
                pqd.Dept = StdBranchLoader.GetSelectBranchItem().Value;
            }
            return pqd;
        }

        public ArrayList getSelectItemArray()
        {
            ArrayList arr = new ArrayList();
            DataTable dt2 = new PSNQUERYSETLoader().QueryConditionSet(txtQSID.Text);
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                string QCSFIELD = dt2.Rows[i]["QCSFIELD"].ToString();
                string QCSTYPE = dt2.Rows[i]["QCSTYPE"].ToString();
                string QCSID = dt2.Rows[i]["QCSID"].ToString();
                string QCSISDIM = dt2.Rows[i]["QCSISDIM"].ToString();
                switch (QCSTYPE)
                {
                    case "STATE":
                        CheerUI.DropDownList ddlState = (CheerUI.DropDownList)CheerUI.ControlUtil.FindControl(QCSID);
                        if (ddlState.SelectedValue.Length > 0)
                        {
                            string[] str = ddlState.SelectedValueArray;
                            ArrayList _arrayWhrSel = new ArrayList();
                            for (int b = 0; b < str.Length; b++)
                            {
                                _arrayWhrSel.Add(str[b]);
                            }
                            TableField _fieldIDSel = new TableField(QCSFIELD);
                            SQLRelation _sqlRelation = SQLRelation.In;
                            arr.Add(new SQLConditionItem(_fieldIDSel, _sqlRelation, _arrayWhrSel));
                        }
                        break;
                    case "DATE":
                        CheerUI.DatePicker datePicker = (CheerUI.DatePicker)CheerUI.ControlUtil.FindControl(QCSID);
                        if (!string.IsNullOrEmpty(datePicker.Text))
                        {
                            if (QCSISDIM == "NO")
                            {
                                ArrayList _arrayWhrSel = new ArrayList();
                                _arrayWhrSel.Add(datePicker.Text);
                                TableField _fieldIDSel = new TableField(QCSFIELD);
                                SQLRelation _sqlRelation = SQLRelation.EQUAL;
                                arr.Add(new SQLConditionItem(_fieldIDSel, _sqlRelation, _arrayWhrSel));
                            }
                            else
                            {
                                ArrayList _arrayWhrSel = new ArrayList();
                                _arrayWhrSel.Add(datePicker.Text);
                                TableField _fieldIDSel = new TableField(QCSFIELD);
                                SQLRelation _sqlRelation = SQLRelation.Like;
                                arr.Add(new SQLConditionItem(_fieldIDSel, _sqlRelation, _arrayWhrSel));
                            }
                        }
                        break;
                    case "DEPT":
                        StdBranchLoader StdBranchLoader = (StdBranchLoader)CheerUI.ControlUtil.FindControl(QCSID);
                        if (!string.IsNullOrEmpty(StdBranchLoader.GetSelectBranchItem().Value))
                        {
                            string deptID = StdBranchLoader.GetSelectBranchItem().Value.Trim();
                            if (deptID.CompareTo("") != 0)
                            {
                                ArrayList _arrayWhrSel = new ArrayList();
                                TableField _fieldIDSel;
                                SQLRelation _sqlRelation;
                                if (StdBranchLoader.IsIncludeSubBranch == true)
                                {
                                    _fieldIDSel = new TableField(ORGStdStructSchema.ORGSTDSTRUCT_TABLE, ORGStdStructSchema.ORGSTDSTRUCT_LABEL);
                                    _sqlRelation = SQLRelation.BeginLike;
                                    STDUnitManager unitManager = (STDUnitManager)(new CHEERBasePage().CreatePalauObject(typeof(STDUnitManager)));
                                    _arrayWhrSel.Add(unitManager.GetCurentUnitByID(deptID).UnitLabel);
                                }
                                else
                                {
                                    _fieldIDSel = new TableField(ORGStdStructSchema.ORGSTDSTRUCT_TABLE, ORGStdStructSchema.ORGSTDSTRUCT_UNITID);
                                    _sqlRelation = SQLRelation.EQUAL;
                                    _arrayWhrSel.Add(deptID);
                                }
                                arr.Add(new SQLConditionItem(_fieldIDSel, _sqlRelation, _arrayWhrSel));
                            }
                        }
                        break;
                    case "DEFAULTLIST":
                        CheerUI.DropDownList ddlDefaultList = (CheerUI.DropDownList)CheerUI.ControlUtil.FindControl(QCSID);
                        if (!string.IsNullOrEmpty(ddlDefaultList.SelectedValue))
                        {
                            if (QCSISDIM == "NO")
                            {
                                ArrayList _arrayWhrSel = new ArrayList();
                                _arrayWhrSel.Add(ddlDefaultList.SelectedValue);
                                TableField _fieldIDSel = new TableField(QCSFIELD);
                                SQLRelation _sqlRelation = SQLRelation.EQUAL;
                                arr.Add(new SQLConditionItem(_fieldIDSel, _sqlRelation, _arrayWhrSel));
                            }
                            else
                            {
                                ArrayList _arrayWhrSel = new ArrayList();
                                _arrayWhrSel.Add(ddlDefaultList.SelectedValue);
                                TableField _fieldIDSel = new TableField(QCSFIELD);
                                SQLRelation _sqlRelation = SQLRelation.Like;
                                arr.Add(new SQLConditionItem(_fieldIDSel, _sqlRelation, _arrayWhrSel));
                            }
                        }
                        break;
                    case "SQLLIST":
                        CheerUI.DropDownList ddlSqlList = (CheerUI.DropDownList)CheerUI.ControlUtil.FindControl(QCSID);
                        if (!string.IsNullOrEmpty(ddlSqlList.SelectedValue))
                        {
                            if (QCSISDIM == "NO")
                            {
                                ArrayList _arrayWhrSel = new ArrayList();
                                _arrayWhrSel.Add(ddlSqlList.SelectedValue);
                                TableField _fieldIDSel = new TableField(QCSFIELD);
                                SQLRelation _sqlRelation = SQLRelation.EQUAL;
                                arr.Add(new SQLConditionItem(_fieldIDSel, _sqlRelation, _arrayWhrSel));
                            }
                            else
                            {
                                ArrayList _arrayWhrSel = new ArrayList();
                                _arrayWhrSel.Add(ddlSqlList.SelectedValue);
                                TableField _fieldIDSel = new TableField(QCSFIELD);
                                SQLRelation _sqlRelation = SQLRelation.Like;
                                arr.Add(new SQLConditionItem(_fieldIDSel, _sqlRelation, _arrayWhrSel));
                            }
                        }
                        break;
                    case "BOOL":
                        CheerUI.CheckBox checkField = (CheerUI.CheckBox)CheerUI.ControlUtil.FindControl(QCSID);
                        if (checkField.Checked)
                        {
                            ArrayList _arrayWhrSel = new ArrayList();
                            _arrayWhrSel.Add(1);
                            TableField _fieldIDSel = new TableField(QCSFIELD);
                            SQLRelation _sqlRelation = SQLRelation.EQUAL;
                            arr.Add(new SQLConditionItem(_fieldIDSel, _sqlRelation, _arrayWhrSel));
                        }
                        else
                        {
                            ArrayList _arrayWhrSel = new ArrayList();
                            _arrayWhrSel.Add(0);
                            TableField _fieldIDSel = new TableField(QCSFIELD);
                            SQLRelation _sqlRelation = SQLRelation.EQUAL;
                            arr.Add(new SQLConditionItem(_fieldIDSel, _sqlRelation, _arrayWhrSel));
                        }
                        break;
                    case "INT":
                        CheerUI.NumberBox numberField = (CheerUI.NumberBox)CheerUI.ControlUtil.FindControl(QCSID);
                        if (!string.IsNullOrEmpty(numberField.Text))
                        {
                            if (QCSISDIM == "NO")
                            {
                                ArrayList _arrayWhrSel = new ArrayList();
                                _arrayWhrSel.Add(numberField.Text.Trim());
                                TableField _fieldIDSel = new TableField(QCSFIELD);
                                SQLRelation _sqlRelation = SQLRelation.EQUAL;
                                arr.Add(new SQLConditionItem(_fieldIDSel, _sqlRelation, _arrayWhrSel));
                            }
                            else
                            {
                                ArrayList _arrayWhrSel = new ArrayList();
                                _arrayWhrSel.Add(numberField.Text.Trim());
                                TableField _fieldIDSel = new TableField(QCSFIELD);
                                SQLRelation _sqlRelation = SQLRelation.Like;
                                arr.Add(new SQLConditionItem(_fieldIDSel, _sqlRelation, _arrayWhrSel));
                            }
                        }
                        break;
                    default:
                        CheerUI.TextBox txtField = (CheerUI.TextBox)CheerUI.ControlUtil.FindControl(QCSID);
                        if (!string.IsNullOrEmpty(txtField.Text))
                        {
                            if (QCSISDIM == "NO")
                            {
                                ArrayList _arrayWhrSel = new ArrayList();
                                _arrayWhrSel.Add(txtField.Text.Trim());
                                TableField _fieldIDSel = new TableField(QCSFIELD);
                                SQLRelation _sqlRelation = SQLRelation.EQUAL;
                                arr.Add(new SQLConditionItem(_fieldIDSel, _sqlRelation, _arrayWhrSel));
                            }
                            else
                            {
                                ArrayList _arrayWhrSel = new ArrayList();
                                _arrayWhrSel.Add(txtField.Text.Trim());
                                TableField _fieldIDSel = new TableField(QCSFIELD);
                                SQLRelation _sqlRelation = SQLRelation.Like;
                                arr.Add(new SQLConditionItem(_fieldIDSel, _sqlRelation, _arrayWhrSel));
                            }
                        }
                        break;
                }
            }
            return arr;
        }

        public string[] ValidateForms;

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

        protected void LoadCondition()
        {
            CHEER.Platform.DAL.PersistBroker broker = CHEER.Platform.DAL.PersistBroker.Instance();
            string sql = "SELECT * FROM PSNQUERYSET WHERE QSTYPE='PSN_BASE'";
            DataTable dt = broker.ExecuteDataset(sql).Tables[0];
            broker.Close();
            string QSID = "";
            foreach (DataRow dr in dt.Rows)
            {
                QSID = dr["QSID"].ToString();
            }
            txtQSID.Text = QSID;
            PSNQUERYSETLoader psnloader = new PSNQUERYSETLoader();
            dt = psnloader.QueryConditionSet(QSID);
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();

            string tit = "QCSTITLE";

            //调整组织位置,将其放在合理的位置上(第一个位置或者第二个位置)
            var tempItemArray = new object[] { };
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["QCSTYPE"].ToString() == "DEPT")
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
            formTempRow.ColumnWidths = "0.33 0.33 0.34 68";

            formTemp.Rows.Add(formTempRow);
            int m = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (m == 3)
                {
                    contentPanel = new CheerUI.ContentPanel();
                    contentPanel.ShowBorder = false;
                    contentPanel.ShowHeader = false;
                    contentPanel.Height = new Unit(1);
                    formTempRow.Items.Add(contentPanel);
                    formRow.Items.Add(formTemp);
                    this.formUserQuery.Rows.Add(formRow);
                    formTemp = new CheerUI.Form();
                    formTemp.ShowBorder = false;
                    formTemp.ShowHeader = false;
                    formRow = new CheerUI.FormRow();
                    formTempRow = new CheerUI.FormRow();
                    formTempRow.ColumnWidths = "0.33 0.33 0.34 68";
                    formTemp.Rows.Add(formTempRow);
                    m = 0;
                }

                string QCSFIELD = dt.Rows[i]["QCSFIELD"].ToString();
                string QCSORDER = dt.Rows[i]["QCSORDER"].ToString();
                string QCSNAME = dt.Rows[i]["QCSNAME"].ToString();
                string QCSTITLE = dt.Rows[i][tit].ToString();
                string QCSTYPE = dt.Rows[i]["QCSTYPE"].ToString();
                string QCSDEFAULTVALUE = dt.Rows[i]["QCSDEFAULTVALUE"].ToString();
                string QCSVALUELIST = dt.Rows[i]["QCSVALUELIST"].ToString();
                string QCSID = dt.Rows[i]["QCSID"].ToString();

                switch (QCSTYPE)
                {
                    case "STATE"://在职状态型
                        CheerUI.DropDownList ddlState = new CheerUI.DropDownList();
                        ddlState.ID = QCSID;
                        ddlState.Label = QCSTITLE;
                        ddlState.EnableMultiSelect = true;
                        //ddlState.EnableCheckMultiSelect = true;
                        if (AcessionStatus_Dimission)
                        {
                            ddlState.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA01150"), "4"));
                        }
                        if (AcessionStatus_Export)
                        {
                            ddlState.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA01152"), "5"));
                        }
                        if (AcessionStatus_Probation)
                        {
                            ddlState.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA00565"), "1"));
                        }
                        if (AcessionStatus_PromotingProbation)
                        {
                            ddlState.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA01151"), "6"));
                        }
                        if (AcessionStatus_Regular)
                        {
                            ddlState.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA00564"), "2"));
                        }
                        if (AcessionStatus_Retired)
                        {
                            ddlState.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA01149"), "3"));
                        }
                        if (AcessionStatus_Unchecked)
                        {
                            ddlState.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA01148"), "7"));
                        }
                        formTempRow.Items.Add(ddlState);
                        m++;
                        break;
                    case "DATE":          //日期型
                        CheerUI.DatePicker datePicker = new CheerUI.DatePicker();
                        datePicker.ID = QCSID;
                        datePicker.Label = QCSTITLE;
                        datePicker.DateFormatString = "yyyy-MM-dd";
                        formTempRow.Items.Add(datePicker);
                        m++;
                        break;
                    case "DEPT":          //组织列表
                        string str2 = "/Controls/StdBranchLoader.ascx";
                        str2 = this.Server.MapPath("~" + str2);
                        StdBranchLoader StdBranchLoader = (StdBranchLoader)Page.LoadControl(ConvertSpecifiedPathToRelativePath(this.Page, str2));
                        StdBranchLoader.IsHaveManageUnit = true;
                        StdBranchLoader.IsShowLabel = true;
                        StdBranchLoader.IsShowIncludeStopUnit = IsShowIncludeStopUnit;
                        StdBranchLoader.SelectStopUnit += StdBranchLoader_SelectStopUnit;
                        StdBranchLoader.ID = QCSID;
                        if (this.Security_Checker != null && this.FunFriendlyID != null)
                        {
                            StdBranchLoader.LoadBranchDtsByID(Security_Checker, this.FunFriendlyID);
                        }
                        else
                        {
                            StdBranchLoader.LoadAllBranchData();
                        }
                        CheerUI.UserControlConnector userControl = new CheerUI.UserControlConnector();
                        userControl.Controls.Add(StdBranchLoader);
                        StdBranchLoader.Label = QCSTITLE;
                        formTempRow.Items.Add(userControl);
                        if (formTempRow.Items.Count == 1)
                        {
                            formTempRow.ColumnWidths = "0.66 0.34 68";
                        }
                        else if (formTempRow.Items.Count == 2)
                        {
                            formTempRow.ColumnWidths = "0.33 0.67 68";
                        }
                        m += 2;
                        break;
                    case "DEFAULTLIST":   //自定义列表型
                        CheerUI.DropDownList ddlDefaultList = new CheerUI.DropDownList();
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
                        ddlSqlList.Label = QCSTITLE;
                        ddlSqlList.ID = QCSID;
                        formTempRow.Items.Add(ddlSqlList);
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
                        numberField.Label = QCSTITLE;
                        numberField.ID = QCSID;
                        formTempRow.Items.Add(numberField);
                        m++;
                        break;
                    default:
                        CheerUI.TextBox txtField = new CheerUI.TextBox();
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
            CheerUI.Button btnQuery = new CheerUI.Button();
            btnQuery.Text = base.getString("ZGAIA00196");
            btnQuery.IconFont = CheerUI.IconFont.Search;
            btnQuery.Click += btnQuery_Click;
            btnQuery.ValidateForms = ValidateForms;
            formTempRow.Items.Add(btnQuery);
            formRow.Items.Add(formTemp);
            this.formUserQuery.Rows.Add(formRow);
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            if (btnSearchEvent != null)
            {
                btnSearchEvent(sender, e);
            }
        }

        public EventHandler btnSearchEvent;

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
    }
}