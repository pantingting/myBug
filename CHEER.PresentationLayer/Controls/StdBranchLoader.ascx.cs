using CHEER.BusinessLayer.Organize;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.Common.Schema;
using CHEER.PresentationLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CHEER.PresentationLayer.Controls
{
    public partial class StdBranchLoader : BaseControls
    {
        public bool IsAutoPostBack
        {
            get
            {
                return this.ddlBranch.AutoPostBack;
            }
            set
            {
                this.ddlBranch.AutoPostBack = value;
            }
        }
        Hashtable dts = new Hashtable();
        public bool IsShowLabel
        {
            get
            {
                return this.ddlBranch.ShowLabel;
            }
            set
            {
                this.ddlBranch.ShowLabel = value;
            }
        }

        public CheerUI.Form FormBranch
        {
            get
            {
                return this.formBranch;
            }
        }

        public Double Width
        {
            get
            {
                return this.formBranch.Width.Value;
            }
            set
            {
                this.formBranch.Width = new Unit(value);
            }
        }

        public string Label
        {
            get
            {
                return (string)ViewState["Label"];

            }
            set
            {
                ViewState["Label"] = value;
                //this.lblBranch.Text = value;
            }
        }

        /// <summary>
        /// 包括子组织的复选框是否可见，默认可见
        /// </summary>
        public bool IsShowIncludeStopUnit
        {
            get
            {
                return !this.cbIncludeStopBranch.Hidden;
            }
            set
            {
                this.cbIncludeStopBranch.Hidden = !value;
            }
        }

        /// <summary>
        /// 功能点友好ID
        /// </summary>
        public string FunFriendID
        {
            get { return (string)ViewState["FunFriendID"]; }
            set { ViewState["FunFriendID"] = value; }
        }
        /// <summary>
        /// 功能点权限编号ID
        /// </summary>
        public string FunSecurityID
        {
            get { return (string)ViewState["FunSecurityID"]; }
            set { ViewState["FunSecurityID"] = value; }
        }

        /// <summary>
        /// 是否加入管理单元限制
        /// </summary>
        public bool IsHaveManageUnit
        {
            get
            {
                if (ViewState["IsHaveManageUnit"] != null)
                    return (bool)ViewState["IsHaveManageUnit"];
                else
                    return false;
            }
            set { ViewState["IsHaveManageUnit"] = value; }
        }
        /// <summary>
        /// 管理单元ID
        /// </summary>
        public string ManageUnitID
        {
            get
            {
                if (ViewState["ManageUnitID"] != null)
                {
                    return (string)ViewState["ManageUnitID"];
                }
                else
                {
                    return new CHEERBasePage().getBusinessUnitID();
                }
            }
            set { ViewState["ManageUnitID"] = value; }
        }

        public void LoadBranchDtsByID(SecurityChecker checker, string functionID)
        {
            this.ddlBranch.Items.Clear();
            if (checker == null || functionID == null || functionID.Length == 0)
            {
                this.ddlBranch.Items.Add(new CheerUI.ListItem("", ""));
            }
            else
            {
                this.FunSecurityID = functionID.Trim();
                DataSet branchs = checker.GetBranchsDatas(functionID, IsHaveManageUnit, ManageUnitID, this.cbIncludeStopBranch.Checked);
                if (branchs == null || branchs.Tables.Count == 0)
                {
                    this.ddlBranch.Items.Add(new CheerUI.ListItem("", ""));
                }
                else
                {
                    int firstLabelLength = 0;
                    if (branchs.Tables[0].Rows[0]["LABEL"].ToString() != "")
                    {
                        firstLabelLength = branchs.Tables[0].Rows[0]["LABEL"].ToString().Length;
                    }
                    string textField = "";
                    string valueField = "";
                    foreach (DataRow _row in branchs.Tables[0].Rows)
                    {
                        textField = returnedValue(_row["LABEL"].ToString(), firstLabelLength) + _row["UNITCODE"].ToString() + "　" + _row["UNITNAME"].ToString();
                        valueField = _row["UNITID"].ToString();
                        this.ddlBranch.Items.Add(new CheerUI.ListItem(textField, valueField));
                        dts.Add(_row["UNITID"].ToString(), _row["UNITID"].ToString());

                    }
                    this.ddlBranch.Items.Insert(0, new CheerUI.ListItem("", ""));
                    ViewState["units"] = dts;
                }
            }
        }

        private string returnedValue(string label, int firstLabel)
        {
            string value = "";
            switch (label.Length - firstLabel)
            {
                //此处用全角空格
                case 0: value = ""; break;
                case 4: value = "　"; break;
                case 8: value = "　　"; break;
                case 12: value = "　　　"; break;
                case 16: value = "　　　　"; break;
                case 20: value = "　　　　　"; break;
                case 24: value = "　　　　　　"; break;
                case 28: value = "　　　　　　　"; break;
                case 32: value = "　　　　　　　　"; break;
                case 36: value = "　　　　　　　　　"; break;
            }
            return value;
        }

        private string checkBoxLength = "100px";

        public string CheckBoxLength
        {
            get { return checkBoxLength; }
            set { checkBoxLength = value; }
        }

        /// <summary>
        /// 包括子组织的复选框是否可见，默认可见
        /// </summary>
        public bool IsShowIncludeSubBranch
        {
            get
            {
                return !this.cbIncludeSubBranch.Hidden;
            }
            set
            {
                this.cbIncludeSubBranch.Hidden = !value;
            }
        }

        /// <summary>
        /// 加载所有的组织节点
        /// </summary>
        public void LoadAllBranchData()
        {
            DataSet _units = ((STDOrganize)eHRPageServer.GetPalauObject(typeof(STDOrganize))).GetAllBranchDataSet();
            DataTable _dt = _units.Tables[0];
            string textField = "";
            string valueField = "";
            int firstLabelLength = 0;
            if (_dt.Rows.Count > 0)
            {
                if (_dt.Rows[0]["LABELINDEX"].ToString() != "")
                {
                    firstLabelLength = _dt.Rows[0]["LABELINDEX"].ToString().Length;
                }
            }
            foreach (DataRowView _row in _dt.DefaultView)
            {
                textField = returnedValue(_row["LABELINDEX"].ToString(), firstLabelLength) + _row["UNITCODE"].ToString() + "  " + _row["UNITNAME"].ToString();
                valueField = _row["UNITID"].ToString();
                this.ddlBranch.Items.Add(new CheerUI.ListItem(textField, valueField));
            }
            this.ddlBranch.Items.Insert(0, new CheerUI.ListItem("", ""));
        }

        /// <summary>
        /// 加载所有未停用的节点
        /// </summary>
        public void LoadnotstopBranchData()
        {
            DataSet _units = ((STDOrganize)eHRPageServer.GetPalauObject(typeof(STDOrganize))).GetnotstopBranchDataSet();
            DataTable _dt = _units.Tables[0];
            int firstLabelLength = 0;
            if (_dt.Rows.Count > 0)
            {
                if (_dt.Rows[0]["LABEL"].ToString() != "")
                {
                    firstLabelLength = _dt.Rows[0]["LABEL"].ToString().Length;
                }
            }
            this.ddlBranch.Items.Add(new CheerUI.ListItem("", ""));
            string textField = "";
            string valueField = "";
            foreach (DataRow _row in _dt.Rows)
            {
                textField = returnedValue(_row["LABEL"].ToString(), firstLabelLength) + _row["UNITCODE"].ToString() + "　" + _row["UNITNAME"].ToString();
                valueField = _row["UNITID"].ToString();
                this.ddlBranch.Items.Add(new CheerUI.ListItem(textField, valueField));
            }
        }
        /// <summary>
        /// 根据当前选择返回所选组织和其子节点的组织ID集合，返回结果的子节点不进行权限过滤。
        /// </summary>
        /// <param name="isDisposeEmpty">是否处理组织选空的情况，如为false，则选空时不做任何处理，返回所有有权限组织
        /// 如果为ture 则不选部们时，返回一个空的ArrayList</param>
        /// <returns>所选组织集合。如果选择为空，则根据isDisposeEmpty决定返回结果</returns>
        /// <remarks>
        /// 1。重载原来方法，加入参数决定是否特殊处理选空的情况，以处理数据权限和组织取交集后数据范围变小的情况
        /// 2。如果组织选择为空值，则不将空值加入到返回的结果中
        /// </remarks>
        public ArrayList GetSelectBranchIDList(bool isDisposeEmpty)
        {
            ArrayList branchIDList = new ArrayList();
            if (this.ddlBranch.SelectedValue.ToString().CompareTo("") != 0)
            {
                string branchID = this.ddlBranch.SelectedValue.ToString();

                //如果包含子组织，获得当前选择组织的所有子组织，并加入in条件中
                if (this.cbIncludeSubBranch.Checked)
                {
                    if (this.IsHaveManageUnit)
                        branchIDList = ((STDOrganizeManager)eHRPageServer.GetPalauObject(typeof(STDOrganizeManager))).GetAllSubBratchID(branchID, this.ManageUnitID.Trim());
                    else
                        branchIDList = ((STDOrganizeManager)eHRPageServer.GetPalauObject(typeof(STDOrganizeManager))).GetAllSubBratchID(branchID);
                }
                branchIDList.Add(branchID);
            }
            else
            {
                if (false == isDisposeEmpty)//正常情况，否则返回空的集合
                {
                    foreach (CheerUI.ListItem branchInfo in GetLoadBranchItem())
                    {
                        branchIDList.Add(branchInfo.Value);
                    }
                    //移除空选项
                    branchIDList.Remove("");
                }
            }
            return branchIDList;
        }
        /// <summary>
        /// 根据当前选择返回所选组织和其子节点的组织ID集合，返回结果的子节点不进行权限过滤。
        /// </summary>
        /// <returns>所选组织集合，如果选择为空，则返回所有已经加载的组织</returns>
        public ArrayList GetSelectBranchIDList()
        {
            return GetSelectBranchIDList(false);
        }
        /// <summary>
        /// 根据当前选择返回所选组织和其子节点的组织ID集合的字符串，返回结果的子节点不进行权限过滤。
        /// </summary>
        /// <returns>所选组织集合，长度为空表示未选择组织</returns>
        /// <param name="_splitStr">组织ID之间的分隔符，如果为null或长度为0，则适用默认分隔符","进行分隔</param>
        /// <param name="isDisposeEmpty">是否处理组织选空的情况，如为false，则选空时不做任何处理，返回所有有权限组织
        /// 如果为ture 则不选部们时，返回一个空的String</param>
        /// <remarks>
        /// 1。重载原来方法，加入参数决定是否特殊处理选空的情况，以处理数据权限和组织取交集后数据范围变小的情况
        /// 2。如果组织选择为空值，则不将空值加入到返回的结果中
        /// </remarks>
        public string GetSelectBranchIDStr(string _splitStr, bool isDisposeEmpty)
        {
            if (_splitStr == null || _splitStr.Length == 0)
            {
                _splitStr = ",";
            }
            StringBuilder _branchBuilder = new StringBuilder();
            ArrayList _branchIDList = GetSelectBranchIDList(isDisposeEmpty);
            foreach (string _branchID in _branchIDList)
            {
                if (_branchBuilder.Length == 0)
                {
                    _branchBuilder.Append(_branchID);
                }
                else
                {
                    _branchBuilder.Append(_splitStr);
                    _branchBuilder.Append(_branchID);
                }
            }
            return _branchBuilder.ToString();
        }
        /// <summary>
        /// 根据当前选择返回所选组织和其子节点的组织ID集合的字符串，返回结果的子节点不进行权限过滤。
        /// </summary>
        /// <returns>所选组织集合，长度为空表示未选择组织</returns>
        /// <param name="_splitStr">组织ID之间的分隔符，如果为null或长度为0，则适用默认分隔符","进行分隔</param>
        public string GetSelectBranchIDStr(string _splitStr)
        {
            return GetSelectBranchIDStr(_splitStr, false);
        }
        /// <summary>
        /// 设置当前选择的组织
        /// </summary>
        /// <param name="branchID">组织ID,如果为null且当前列表中存在空项，则选择空项</param>
        public void SetSelectBranch(string branchID)
        {
            if (branchID == null)
            {
                branchID = "";
            }
            if (this.ddlBranch.Items.FindByValue(branchID) != null)
            {
                this.ddlBranch.SelectedValue = branchID;
                CheerUI.PageContext.RegisterStartupScript("C('" + this.ddlBranch.ClientID + "').c_setValue('" + branchID + "');");
            }

        }
        /// <summary>
        /// 返回当前选择的组织信息，Text为组织显示名称，Vale为组织的ID
        /// </summary>
        public CheerUI.ListItem GetSelectBranchItem()
        {
            return this.ddlBranch.SelectedItem;
        }

        public CheerUI.ListItem[] GetLoadBranchItem()
        {
            CheerUI.ListItem[] _branchList = new CheerUI.ListItem[this.ddlBranch.Items.Count];
            this.ddlBranch.Items.CopyTo(_branchList, 0);
            return _branchList;
        }

        /// <summary>
        /// 组合控件是否可用
        /// </summary>
        public bool IsEnable
        {
            get
            {
                return this.ddlBranch.Enabled;
            }
            set
            {
                this.ddlBranch.Enabled = value;
                this.cbIncludeSubBranch.Enabled = value;
            }
        }

        public ArrayList GetSelectedBranchInfor()
        {
            if (this.ddlBranch.SelectedValue == null || this.ddlBranch.SelectedValue.Trim() == "")
                return new ArrayList();
            string selectedbranchid = ddlBranch.SelectedValue.Trim();
            string selectidbranchname = ddlBranch.SelectedItem.Text;
            string deptinforlist = this.txtDeptInfo.Text;
            string[] deptinforarray = deptinforlist.Split(new char[] { '*', '|', '*' });
            ArrayList needlist = new ArrayList();
            foreach (string str in deptinforarray)
            {
                if (str.Trim() != "")
                {
                    needlist.Add(str.Trim());
                }
            }
            ArrayList deptlist = new ArrayList();
            if (needlist.Count >= 4)
            {
                string windowdeptid = needlist[3].ToString().Trim();
                if (windowdeptid == selectedbranchid)
                {
                    deptlist.Add(needlist[3].ToString().Trim());
                    deptlist.Add(needlist[0].ToString().Trim());

                    //点击放大镜后selectidbranchname 的值没有\t来表示空格
                    //deptlist.Add((selectidbranchname.Split('\t'))[1].ToString());
                    deptlist.Add((selectidbranchname.Split('　'))[1].ToString());

                    deptlist.Add(needlist[1].ToString().Trim());
                    deptlist.Add(needlist[2].ToString().Trim());
                    return deptlist;
                }
            }
            //根据selectedbranchid获取相应信息
            STDUnit stdunit = (STDUnit)eHRPageServer.GetPalauObject(typeof(STDUnit));
            DataSet unitds = stdunit.GetUnitDSTByID(selectedbranchid);
            if (unitds.Tables.Count > 0)
            {
                deptlist.Add(selectedbranchid);
                deptlist.Add(unitds.Tables[0].Rows[0][ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE].ToString().Trim());
                deptlist.Add(unitds.Tables[0].Rows[0][ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME].ToString().Trim());
                string[] labellist = stdunit.GetCurrLabelInfoByID(selectedbranchid);
                deptlist.Add(labellist[0].Trim());
                deptlist.Add(labellist[1].Trim());
            }
            return deptlist;
        }

        /// <summary>
        /// 获取所选组织的查询sql语句
        /// </summary>
        /// <returns></returns>
        public string GetSelectedBranchSQLStr()
        {
            string selectsqlstr = "";
            ArrayList branchinfrolist = this.GetSelectedBranchInfor();
            if (branchinfrolist.Count <= 0)
            {
                //return "1=1";
                if (ViewState["units"] != null)
                {
                    Hashtable dt = (Hashtable)ViewState["units"];
                    string unitids = "";
                    foreach (var item in dt.Keys)
                    {
                        unitids += "'" + dt[item].ToString() + "',";
                    }
                    unitids = unitids.TrimEnd(',');
                    return " ORGSTDSTRUCT.UNITID in (" + unitids + ") ";
                }
                else
                {
                    return "1<>1";
                }
            }
            string label = branchinfrolist[3].ToString().Trim();
            bool issub = this.IsIncludeSubBranch;
            if (issub)
                selectsqlstr = ORGStdStructSchema.ORGSTDSTRUCT_TABLE + "." + ORGStdStructSchema.ORGSTDSTRUCT_LABEL + " like '" + label + "%'";
            else
                selectsqlstr = ORGStdStructSchema.ORGSTDSTRUCT_TABLE + "." + ORGStdStructSchema.ORGSTDSTRUCT_LABEL + " = '" + label + "'";
            if (IsHaveManageUnit == true)
            {
                selectsqlstr = selectsqlstr + " AND " + ORGStdStructSchema.ORGSTDSTRUCT_MANAGEUNIT + " = '" + ManageUnitID + "'";
            }
            return selectsqlstr;
        }

        /// <summary>
        /// 是否包含子组织选项，true表示包括，false表示不包括。
        /// </summary>
        public bool IsIncludeSubBranch
        {
            set
            {
                this.cbIncludeSubBranch.Checked = value;
            }
            get
            {
                return this.cbIncludeSubBranch.Checked;
            }
        }

        /// <summary>
        /// 下拉框选择的组织改变事件内容
        /// </summary>
        public class SelectBranchChangeEventArgs : EventArgs
        {
            private CheerUI.ListItem _newBranchInfo;
            /// <summary>
            /// 选择的组织改变的EventArgs类
            /// </summary>
            /// <param name="newBranchInfo"></param>
            public SelectBranchChangeEventArgs(CheerUI.ListItem newBranchInfo)
            {
                this._newBranchInfo = newBranchInfo;
            }
            /// <summary>
            /// 新选择的组织
            /// </summary>
            public CheerUI.ListItem NewBranchInfo
            {
                get { return _newBranchInfo; }
            }

        }
        /// <summary>
        /// 下拉框选择的组织改变的委托
        /// </summary>
        public delegate void SelectBranchChangeHandler(object sender, SelectBranchChangeEventArgs e);
        /// <summary>
        /// 停用选择事件内容
        /// </summary>
        public class SelectStopUnitEventArgs : EventArgs
        {
            private bool _isStop;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="isStop"></param>
            public SelectStopUnitEventArgs(bool isStop)
            {
                this._isStop = isStop;
            }
            /// <summary>
            /// 
            /// </summary>
            public bool IsStop
            {
                get { return _isStop; }
            }

        }
        /// <summary>
        /// 停用选择，组织改变的委托
        /// </summary>
        public delegate void SelectStopUnitHandler(object sender, SelectStopUnitEventArgs e);

        private void InitLau()
        {
            this.ddlBranch.Label = this.getString("ZGAIA00479");//"组织";
            if (!string.IsNullOrEmpty(Label))
            {
                this.ddlBranch.Label = Label;
            }
            this.cbIncludeSubBranch.Text = this.getString("ZGAIA02390");//"包含子组织"
            this.cbIncludeStopBranch.Text = this.getString("ZGAIA02391");//包含停用
        }

        /// <summary>
        /// 下拉值发生变化引发
        /// </summary>
        public event SelectBranchChangeHandler SelectBranchChange;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSelectBranchChange(SelectBranchChangeEventArgs e)
        {
            if (SelectBranchChange != null)
            {
                // Invokes the delegates. 
                SelectBranchChange(this, e);
            }
        }

        /// <summary>
        /// 选择部门停用引发事件
        /// </summary>
        public event SelectStopUnitHandler SelectStopUnit;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSelectStopUnit(SelectStopUnitEventArgs e)
        {
            if (SelectStopUnit != null)
            {
                // Invokes the delegates. 
                SelectStopUnit(this, e);
            }
        }

        void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectBranchChange(new SelectBranchChangeEventArgs(ddlBranch.SelectedItem));
        }

        void cbIncludeStopBranch_CheckedChanged(object sender, CheerUI.CheckedEventArgs e)
        {
            SelectStopUnitEventArgs args = new SelectStopUnitEventArgs(this.cbIncludeStopBranch.Checked);
            OnSelectStopUnit(args);
        }

        protected void Page_Init()
        {
            this.ddlBranch.SelectedIndexChanged += ddlBranch_SelectedIndexChanged;
            this.cbIncludeStopBranch.CheckedChanged += cbIncludeStopBranch_CheckedChanged;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitLau();
                string columnWidths = "100% ";
                if (this.IsShowIncludeSubBranch)
                {
                    columnWidths += CheckBoxLength + " ";
                }
                else
                {
                    columnWidths += "0px ";
                }
                if (this.IsShowIncludeStopUnit)
                {
                    columnWidths += "80px";
                }
                else
                {
                    columnWidths += "0px";
                }
                this.formBranchDetail.ColumnWidths = columnWidths;
            }
        }
    }
}