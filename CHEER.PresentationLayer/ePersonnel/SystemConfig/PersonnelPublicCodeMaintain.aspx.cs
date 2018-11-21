using CheerUI;
using CHEER.PresentationLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq; 
using CHEER.Common;
using System.Collections;
using System.Text;
using CHEER.BusinessLayer.Security;
using CHEER.BusinessLayer.ePersonnel.SystemConfig;
using CHEER.CommonLayer.ePersonnel.Schema;
using CHEER.CommonLayer.ePersonnel.Data;
using System.Configuration;
using CHEER.PresentationLayer.CommonUse;

namespace CHEER.PresentationLayer.ePersonnel.SystemConfig
{
    public partial class PersonnelPublicCodeMaintain : CHEERBasePage
    {
        public const string PERSONNELPUBLICCODEMAINTAIN_FUNCTIONID = "030100070";
        public const string PERSONNELPUBLICCODEMAINTAIN_FUNCTIONID_ADD = "030100070010";
        public const string PERSONNELPUBLICCODEMAINTAIN_FUNCTIONID_ADDSUB = "030100070020";
        PersonnelPublicCodeManager _PersonnelPublicCodeManager;
        PersonnelPublicCodeTypeManager _PersonnelPublicCodeTypeManager;

        private readonly bool _isCanUpdatePersonnelPublicItemCode = GetIsCanUpdatePersonnelPublicItemCode();
        private static bool GetIsCanUpdatePersonnelPublicItemCode()
        {
            string _value = "";
            {
                try
                {
                    _value = ConfigurationSettings.AppSettings["isCanUpdatePersonnelPublicItemCode"];
                }
                catch
                {
                }
            }
            if (_value == "Y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Init_Page();
              
            }
        }
        
        /// <summary>
        /// 权限检验
        /// </summary>
        private void CheckRight()
        {
            try
            {
                switch (base.CheckAccessibilityByID(PERSONNELPUBLICCODEMAINTAIN_FUNCTIONID))
                {
                    case PageAccessibility.Unaccessible:
                        ShowAlert(getAlert("ZGAIA00809"));
                        break;
                    case PageAccessibility.Unusable:
                        ShowAlert(getAlert("ZGAIA02583"));
                        break;
                    case PageAccessibility.Accessible:
                        SecurityChecker _checker = base.GetSecurityChecker();
                        this.btnAdd.Enabled = _checker.IsAllow(PERSONNELPUBLICCODEMAINTAIN_FUNCTIONID_ADD);
                        this.btnAddNewRow.Enabled = _checker.IsAllow(PERSONNELPUBLICCODEMAINTAIN_FUNCTIONID_ADDSUB);
                        break;
                    default:
                        ShowAlert(getAlert("ZGAIA00809"));
                        break;
                }
            }
            catch
            {
                base.ShowAlert(base.getAlert("ZGAIA02585"));
            }
        }

        private void Init_Page()
        {
            // 在第一行新增一条数据
            btnAddNewRow.OnClientClick = FGridPublicCode.GetAddNewRecordReference(GetdefaultObj(), true);
            btnAdd.Text = getString("ZGAIA00023");
            btnAddNewRow.Text = getString("ZGAIA00013");
            btnSave.Text = getString("ZGAIA00195");
            btnDelete.Text = getString("ZGAIA00194");

            txtTypeName.Label = getString("ZGAIA01446");
     
            txtTypeCode.Label = getString("ZGAIA01447");  
        }

        private JObject GetdefaultObj()
        {
            JObject defaultObj = new JObject();
            defaultObj.Add(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMID_FULL, "");
            defaultObj.Add(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMVALUE_FULL, "");
            defaultObj.Add(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_AVAILABLE_FULL, true);
            defaultObj.Add(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_CODE_FULL, "");
            defaultObj.Add(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_INDEXCODE_FULL, ""); 

            return defaultObj;
        }



        private void LoadTree()
        {         

            try
            {
                _PersonnelPublicCodeTypeManager = (PersonnelPublicCodeTypeManager)eHRPageServer.GetPalauObject(typeof(PersonnelPublicCodeTypeManager));
            }
            catch
            {
                throw new Exception(base.getString("ZGAIA01967"));
            }
            try
            {
                DataTable _dtCode = _PersonnelPublicCodeTypeManager.GetCodeTypes().Tables[0];
                LoadCode(_dtCode);
            }
            catch (Exception e)
            {
                throw new Exception(getString("ZGAIA01451") + "\r\n" + e.Message);
            }
        }

        private void LoadCode(DataTable dtCode)
        {
            dtCode.DefaultView.Sort = PSNPublicCodeTypeSchema.PSNPUBLICCODETYPE_TYPEID_FULL + "  ASC";
            FTreePublicCode.Nodes.Clear();
            CheerUI.TreeNode rootItem = new CheerUI.TreeNode();
            rootItem.NodeID = "root";
            rootItem.Text = getString("ZGAIA01446");// "公用代码类别";
            rootItem.ToolTip = getString("ZGAIA01446");// "公用代码类别";
            rootItem.EnableClickEvent = true;
            CheerUI.TreeNode childItem = new CheerUI.TreeNode();
             
            if (dtCode != null && dtCode.Rows.Count > 0)
            {
                foreach (DataRow dr in dtCode.Rows)
                {
                    childItem = new CheerUI.TreeNode();
                    childItem.NodeID = dr[PSNPublicCodeTypeSchema.PSNPUBLICCODETYPE_TYPEID_FULL].ToString();
                    childItem.Text = dr[PSNPublicCodeTypeSchema.PSNPUBLICCODETYPE_TYPENAME_FULL].ToString();
                    childItem.ToolTip = dr[PSNPublicCodeTypeSchema.PSNPUBLICCODETYPE_TYPENAME_FULL].ToString();
                    childItem.EnableClickEvent = true;
                    rootItem.Nodes.Add(childItem);
                }
            }

            FTreePublicCode.Nodes.Add(rootItem);
            FTreePublicCode.ExpandAllNodes(FTreePublicCode.Nodes);
            FTreePublicCode.NodeCommand += Tree1_NodeCommand;             
          
        }

        protected void Page_Init(object sender, EventArgs e)
        {
             LoadTree();
             InitGridCol();
        }
        CheerUI.TextBox tbxEditorITEMVALUE=new CheerUI.TextBox ();

        CheerUI.TextBox tbxEditorINDEXCODE=new CheerUI.TextBox();
        CheerUI.TextBox tbxEditorCODE = new CheerUI.TextBox();
        protected void InitGridCol()
        {

            CommonMethod.AddField(this.FGridPublicCode, "", PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMID_FULL, 0, true);
             //"代码项名称"
            CommonMethod.AddFlexRendererField(this.FGridPublicCode,getString("ZGAIA01453"), PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMVALUE_FULL, 25, "", false).Editor.Add(tbxEditorITEMVALUE);
            //排序值
            CommonMethod.AddFlexRendererField(this.FGridPublicCode, getString("ZGAIA01450"), PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_INDEXCODE_FULL, 25, "", false).Editor.Add(tbxEditorINDEXCODE);
            // "代码值"
            CommonMethod.AddFlexRendererField(this.FGridPublicCode, getString("ZGAIA01455"), PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_CODE_FULL, 25, "", false).Editor.Add(tbxEditorCODE);
            //是否启用
            CommonMethod.AddFlexRenderCheckField(this.FGridPublicCode, getString("ZGAIA01458"), PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_AVAILABLE_FULL, 25, false);
           
          }

        void Tree1_NodeCommand(object sender, CheerUI.TreeCommandEventArgs e)
        {
            LoadGrid();
        }

        private void LoadGrid()
        {
            string strTypeID = this.FTreePublicCode.SelectedNodeID;

            if (strTypeID == "root")
            {
                this.btnAdd.Enabled = true;
                this.btnDelete.Enabled = false;
                this.txtTypeName.Text = "";
                this.txtTypeCode.Text = "";
                this.txtTypeCode.Readonly = false;
                this.btnSave.Enabled = false;
                InitGrid();
            }
            else
            {
                this.btnAdd.Enabled = false;
                this.btnDelete.Enabled = true;
                this.btnSave.Enabled = true;
                this.txtTypeCode.Readonly = true;
                BindGrid(strTypeID);
                CheerUI.PageContext.RegisterStartupScript(this.FGridPublicCode.GetClearSelectionsReference());
                txtTypeCode.Text = this.FTreePublicCode.SelectedNode.NodeID;
                txtTypeName.Text = this.FTreePublicCode.SelectedNode.Text;
            }

        }
        private void BindGrid(string strTypeID)
        {
            // 1.设置总项数（特别注意：数据库分页一定要设置总记录数RecordCount）
            DataTable source = GetItemsByTypeID(strTypeID);

            // 3.绑定到Grid
            FGridPublicCode.DataSource = source;
            FGridPublicCode.DataBind();
        }

        private void InitGrid()
        {
            // 1.设置总项数（特别注意：数据库分页一定要设置总记录数RecordCount）
            DataTable source = new DataTable();
            source.Columns.Add(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMID_FULL);
            source.Columns.Add(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_TYPEID_FULL);
            source.Columns.Add(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMVALUE_FULL);
            source.Columns.Add(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_AVAILABLE_FULL);
            source.Columns.Add(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ISRESERVE_FULL);
            source.Columns.Add(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_CODE_FULL);
            source.Columns.Add(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_INDEXCODE_FULL); 

            // 3.绑定到Grid
            FGridPublicCode.DataSource = source;
            FGridPublicCode.DataBind();

        }

        private DataTable GetAllPublicType()
        {
            string strSQL = "select TYPEID,TYPENAME,ISRESERVE From PSNPUBLICCODETYPE";
            DataTable DT = new CHEERSQL().ExecuteSQLforDataTable(strSQL);
            return DT;
        }

        private DataTable GetItemsByTypeID(string strTypeID)
        {            
            try
            {
                _PersonnelPublicCodeManager = (PersonnelPublicCodeManager)eHRPageServer.GetPalauObject(typeof(PersonnelPublicCodeManager));
            }
            catch
            {
                throw new Exception(base.getString("ZGAIA01967"));
            }
           DataTable  dt = new DataTable ();
            try
            {
                dt = _PersonnelPublicCodeManager.GetCodeValues(strTypeID).Tables[0];
             
            }
            catch (Exception e)
            {
                throw new Exception(base.getString("ZGAIA01956") + "\r\n" + e.Message);
            }

            return dt;
        }   
 

        protected void btnSave_Click(object sender, EventArgs e)
        {            
            #region 
            if (!SaveCheckValidity())
                return;

            ArrayList _listItems = new ArrayList();
            PersonnelPublicCodeTypeData _typeData;
            try
            {

                _typeData = GetTypeDataObjectFromUI();
                _listItems = GetItemListFromUI();
            }
            catch (Exception _ex)
            {
                base.ShowAlert(_ex.Message);
                return;
            }
            try
            {
                _PersonnelPublicCodeManager = (PersonnelPublicCodeManager)eHRPageServer.GetPalauObject(typeof(PersonnelPublicCodeManager));
            }
            catch
            {
                base.ShowAlert(base.getString("ZGAIA01967"));
                return;
            }
            try
            {
                _PersonnelPublicCodeManager.UpdateCodeTypeAndItems(_typeData, _listItems);
                base.ShowAlert(base.getString("ZGAIA00749"));
            }
            catch (Exception _ex)
            {
                base.ShowAlert(base.getString("ZGAIA01726") + "\r\n" + _ex.Message);//"保存公用代码时失败,所有操作取消!");	
                return;
            }

            #endregion
 
            LoadTree();
            LoadGrid();
        }

        private bool SaveCheckValidity()
        {
            if (this.txtTypeName.Text == "")
            {
                base.ShowAlert(getString("ZGAIA01445"));
                return false;
            }
            if (this.txtTypeCode.Text.Trim().Length == 0)
            {
                base.ShowAlert(getString("ZGAIA01454"));
                return false;
            }
            CheerUI.TreeNode _rootNode = this.FTreePublicCode.Nodes[0];
            bool _isNameDuplicate = false;
            foreach (CheerUI.TreeNode _nodeItem in _rootNode.Nodes)
            {
                if (_nodeItem.NodeID.ToString().ToLower().Trim() != this.txtTypeCode.Text.ToLower().Trim())
                {
                    if (_nodeItem.Text.ToLower().Trim() == this.txtTypeName.Text.ToLower().Trim())
                    {
                        _isNameDuplicate = true;
                    }
                }
            }
            if (_isNameDuplicate)
            {
                base.ShowAlert(getString("ZGAIA01459"));
                return false;
            }
            if (!CheckGridValidity())
            {
                return false;
            }
            return true;
        }

        private bool CheckGridValidity()
        {
            Hashtable _valueHash = new Hashtable();
            Hashtable _idHash = new Hashtable();
            Hashtable _nameHash = new Hashtable();
            string _duplicateName = "";
            string _duplicateCode = "";
            bool _isNameDuplicate = false;
            bool _isIDDuplicate = false;

            List<Dictionary<string, object>> newAddedList = FGridPublicCode.GetNewAddedList();//新增行的数据

            string _value = "";
            string _code = "";
            string _indexCode = "";
            string ifcheck = "";

            for (int i = 0, count = FGridPublicCode.Rows.Count; i < count; i++)
            {
                  _value = FGridPublicCode.Rows[i].Cells.FromKey(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMVALUE_FULL).ToString().Trim();
                  _code =  FGridPublicCode.Rows[i].Cells.FromKey(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_CODE_FULL).ToString() ;
                  _indexCode = FGridPublicCode.Rows[i].Cells.FromKey(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_INDEXCODE_FULL).ToString();
                  ifcheck = FGridPublicCode.Rows[i].Cells.FromKey(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_AVAILABLE_FULL).ToString();
          
                bool _isSave;
                if (FGridPublicCode.Rows[i].Cells.FromKey(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_CODE_FULL).ToString() == null ||
                     FGridPublicCode.Rows[i].Cells.FromKey(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_CODE_FULL).ToString().Length == 0)
                {
                    _isSave = false;
                }
                else
                {
                    _isSave = true;
                }
                _value = _value.ToLower().Trim();
                _code = _code.ToLower().Trim();
                if ((_value == null || _value.Length == 0) && (_code == null || _code.Length == 0))
                {
                    if (_isSave == true)
                    {
                        base.ShowAlert(getString("ZGAIA01457") + (i + 1) + getString("ZGAIA01456"));
                        return false;
                    }
                }
                else
                {

                    if ((_value != null && _value.Length != 0) && (_code != null && _code.Length != 0))
                    {

                        if (_valueHash.Contains(_value))
                        {
                            _isNameDuplicate = true;
                            if (_duplicateName.Length == 0)
                            {
                                _duplicateName = _value;
                            }
                            else
                            {
                                _duplicateName += ";" + _value;
                            }
                        }
                        else
                        {
                            _valueHash.Add(_value, _value);
                        }

                        if (!_isCanUpdatePersonnelPublicItemCode)
                        {
                            if (_isSave)
                            {
                                _code = FGridPublicCode.Rows[i].Cells.FromKey(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_CODE_FULL).ToString().Trim();
                            }
                        }
                        if (_idHash.Contains(_code))
                        {
                            _isIDDuplicate = true;
                            if (_duplicateCode.Length == 0)
                            {
                                _duplicateCode = _code;
                            }
                            else
                            {
                                _duplicateCode += ";" + _code;
                            }
                        }
                        else
                        {
                            _idHash.Add(_code, _code);
                        }
                    }
                    else
                    {

                        base.ShowAlert(getString("ZGAIA01457") + (i + 1) + getString("ZGAIA01456"));
                        return false;
                    }
                }

            }

            for (int i = 0; i < newAddedList.Count; i++)
            {
                _value = newAddedList[i][PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMVALUE_FULL].ToString().Trim();
                _code = newAddedList[i][PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_CODE_FULL].ToString().Trim();
                if ((_value == null || _value.Length == 0) && (_code == null || _code.Length == 0))
                {
                    continue;
                }
                else
                {  
                    _value = _value.ToLower().Trim();
                    _code = _code.ToLower().Trim();
                    if ((_value != null && _value.Length != 0) && (_code != null && _code.Length != 0))
                    {

                        if (_valueHash.Contains(_value))
                        {
                            _isNameDuplicate = true;
                            if (_duplicateName.Length == 0)
                            {
                                _duplicateName = _value;
                            }
                            else
                            {
                                _duplicateName += ";" + _value;
                            }
                        }
                        else
                        {
                            _valueHash.Add(_value, _value);
                        }

                        //if (!_isCanUpdatePersonnelPublicItemCode)
                        //{
                        //    if (_isSave)
                        //    {
                        //        _code = FGridPublicCode.Rows[i].Cells.FromKey(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_CODE_FULL).ToString().Trim();
                        //    }
                        //}
                        if (_idHash.Contains(_code))
                        {
                            _isIDDuplicate = true;
                            if (_duplicateCode.Length == 0)
                            {
                                _duplicateCode = _code;
                            }
                            else
                            {
                                _duplicateCode += ";" + _code;
                            }
                        }
                        else
                        {
                            _idHash.Add(_code, _code);
                        }
                    }
                    else
                    {

                        base.ShowAlert(getString("ZGAIA01457") + (i + 1) + getString("ZGAIA01456"));
                        return false;
                    }
                }   
             
            }
            
            if (_isNameDuplicate)
            {
                base.ShowAlert(getString("ZGAIA01452") + " " + _duplicateName);
                return false;
            }
            if (_isIDDuplicate)
            {
                base.ShowAlert(getString("ZGAIA01449") + " " + _duplicateCode);
                return false;
            }
            return true;
        }

        private string getModifValue(int i, string columnId,string tbxEditorID)
        {
            var _value = ((CheerUI.TextBox)FGridPublicCode.Rows[i].Grid.Columns.FromKey(columnId).FindControl(tbxEditorID)).Text;
            return _value.ToString();
        }     
    

        private void DeleteRowByID(string ItemID)
        {
            string strDel = @"DELETE PSNPUBLICCODEITEM WHERE ITEMID='{0}'";
            strDel = string.Format(strDel, ItemID);

            new CHEERSQL().ExecuteSQL(strDel);


        }
              

        protected void btnAdd_Click(object sender, System.EventArgs e)
        {
            if (!AddCheckValidity())
                return;
            ArrayList _listItems = new ArrayList();
            PersonnelPublicCodeTypeData _typeData = new PersonnelPublicCodeTypeData();
            _typeData.IsReserve = false;
            _typeData.ID = this.txtTypeCode.Text;
            _typeData.Name = this.txtTypeName.Text;

            try
            {
                _listItems = GetItemListFromUI();
            }
            catch (Exception _ex)
            {
                base.ShowAlert(_ex.Message);
                return;
            }
            try
            {
                _PersonnelPublicCodeManager = (PersonnelPublicCodeManager)eHRPageServer.GetPalauObject(typeof(PersonnelPublicCodeManager));
            }
            catch
            {
                base.ShowAlert(base.getString("ZGAIA01967"));
                return;
            }
            try
            {
                _PersonnelPublicCodeManager.InsertCodeTypeAndItems(_typeData, _listItems);
                base.ShowAlert(base.getString("ZGAIA00749"));
            }
            catch (Exception _ex)
            {
                base.ShowAlert(getString("ZGAIA01444") + "\r\n" + _ex.Message);//"保存公用代码时失败,所有操作取消!");	
                return;
            }
            try
            {
                LoadTree();               
                InitForNew();
                LoadGrid();
            }
            catch (Exception _ex)
            {
                base.ShowAlert(base.getString("ZGAIA01945") + "\r\n" + _ex.Message);
                return;
            }
        }
        public void InitForNew()
        {
            this.FTreePublicCode.SelectedNodeID = txtTypeCode.Text;
            this.FTreePublicCode.SelectedNode.Checked = true;

            this.FGridPublicCode.Rows.Clear();

            txtTypeCode.Text = "";
            txtTypeName.Text = "";


        }

        private bool AddCheckValidity()
        {
            if (this.txtTypeName.Text == "")
            {
                base.ShowAlert(getString("ZGAIA01445"));
                return false;
            }
            if (this.txtTypeCode.Text.Trim().Length == 0)
            {
                base.ShowAlert(getString("ZGAIA01454"));
                return false;
            }
            CheerUI.TreeNode _rootNode = this.FTreePublicCode.Nodes[0];
            bool _isNameDuplicate = false;
            bool _isIDDuplicate = false;
            foreach (CheerUI.TreeNode _nodeItem in _rootNode.Nodes)
            {
                if (_nodeItem.Text.ToLower().Trim() == this.txtTypeCode.Text.ToLower().Trim())
                {
                    _isNameDuplicate = true;
                }
                if (_nodeItem.NodeID.ToString().ToLower().Trim() == this.txtTypeCode.Text.ToLower().Trim())
                {
                    _isIDDuplicate = true;
                }
            }
            if (_isNameDuplicate)
            {
                base.ShowAlert(getString("ZGAIA01459"));
                return false;
            }
            if (_isIDDuplicate)
            {
                base.ShowAlert(getString("ZGAIA01443"));
                return false;
            }
            if (!CheckGridValidity())
            {
                return false;
            }
            return true;
        }

        #region
        private ArrayList GetItemListFromUI()
        {

            ArrayList _listItems = new ArrayList();
            PersonnelPublicCodeItemData _data;          

            string _value = "";
            string _code = "";
            string _indexCode = "";
            string ifcheck = "";

            for (int i = 0, count = FGridPublicCode.Rows.Count; i < count; i++)
            {
                _value = FGridPublicCode.Rows[i].Cells.FromKey(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMVALUE_FULL).ToString().Trim();
                _code = FGridPublicCode.Rows[i].Cells.FromKey(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_CODE_FULL).ToString();
                _indexCode = FGridPublicCode.Rows[i].Cells.FromKey(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_INDEXCODE_FULL).ToString();
                ifcheck = FGridPublicCode.Rows[i].Cells.FromKey(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_AVAILABLE_FULL).ToString();
                if (ifcheck == "False")
                {
                    ifcheck = "0";
                }
                if (ifcheck == "True")
                {
                    ifcheck = "1";
                }
                bool _isSave;                 
                _isSave = true;
                if ((_code == null || _code.Length == 0) && (_value == null || _value.Length == 0))
                {
                    continue;
                }
                else
                {
                    _data = new PersonnelPublicCodeItemData();
                    if (ifcheck != null)
                    {                        
                        if (ifdataisint(ifcheck))
                        {
                            int check = Convert.ToInt32(ifcheck);
                            _data.Available = Convert.ToBoolean(check);
                        }
                        else
                            _data.Available = Convert.ToBoolean(ifcheck);
                    }
                    else
                        _data.Available = false;


                    //if (_selectRow.Cells.FromKey(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ISRESERVE_FULL).Value == null)
                    //{
                    //    _selectRow.Cells.FromKey(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ISRESERVE_FULL).Value = 0;
                    //}
                    //_data.IsReserve = Convert.ToBoolean(_selectRow.Cells.FromKey(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ISRESERVE_FULL).Value);
                    _data.IsReserve = true;
                    
                    _data.ItemID = FGridPublicCode.Rows[i].Cells.FromKey(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMID_FULL).ToString().Trim();
                   
                    _data.ItemValue = _value;
                    _data.TypeID = this.txtTypeCode.Text;
                    _data.CODE = _code;
                    _data.IndexCode = _indexCode;

                    _listItems.Add(_data);
                }
            }
            List<Dictionary<string, object>> newAddedList = FGridPublicCode.GetNewAddedList();//新增行的数据
            for (int i = 0; i < newAddedList.Count; i++)
            {

                _value =newAddedList[i][PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMVALUE_FULL].ToString();
                _code = newAddedList[i][PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_CODE_FULL].ToString();
                _indexCode = newAddedList[i][PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_INDEXCODE_FULL].ToString();
                ifcheck = newAddedList[i][PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_AVAILABLE_FULL].ToString();
                if ((_code == null || _code.Length == 0) && (_value == null || _value.Length == 0))
                {
                    continue;
                }
                else
                {
                    if (ifcheck == "False")
                    {
                        ifcheck = "0";
                    }
                    if (ifcheck == "True")
                    {
                        ifcheck = "1";
                    }
                    _data = new PersonnelPublicCodeItemData();
                    if (ifcheck != null)
                    {
                        if (ifdataisint(ifcheck))
                        {
                            int check = Convert.ToInt32(ifcheck);
                            _data.Available = Convert.ToBoolean(check);
                        }
                        else
                            _data.Available = Convert.ToBoolean(ifcheck);
                    }
                    else
                        _data.Available = false;

                    _data.IsReserve = false; ;

                    _data.ItemID = this.txtTypeCode.Text + "_" + _code;

                    _data.ItemValue = _value;
                    _data.TypeID = this.txtTypeCode.Text;
                    _data.CODE = _code;
                    _data.IndexCode = _indexCode;

                    _listItems.Add(_data);
                }
            }

            return _listItems;
        }
        private bool ifdataisint(string dataint)
        {
            try
            {
                Convert.ToInt32(dataint);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private PersonnelPublicCodeTypeData GetTypeDataObjectFromUI()
        {
            try
            {
                _PersonnelPublicCodeTypeManager = (PersonnelPublicCodeTypeManager)eHRPageServer.GetPalauObject(typeof(PersonnelPublicCodeTypeManager));
            }
            catch
            {
                throw new Exception(base.getString("ZGAIA01967"));

            }
            PersonnelPublicCodeTypeData _typeData = null;
            try
            {
                _typeData = _PersonnelPublicCodeTypeManager.GetData(this.txtTypeCode.Text.Trim());
            }
            catch (Exception _ex)
            {
                throw new Exception(getString("ZGAIA01451") + "\r\n" + _ex.Message);

            }
            _typeData.Name = this.txtTypeName.Text.Trim().DBReplace();
            return _typeData;
        }
        #endregion

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //var rows = this.FGridPublicCode.SelectedRowIndexArray;
            var rows = this.FGridPublicCode.GetDeletedList();
            string itemId = "'"; int count = 0;
            try
            {
                foreach (int i in rows)
                {
                    if (FGridPublicCode.Rows[i].Cells.FromKey(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_AVAILABLE_FULL) != null &&
                        FGridPublicCode.Rows[i].Cells.FromKey(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_AVAILABLE_FULL).ToString().ToLower() == "false")
                    {
                        count++;
                        itemId += FGridPublicCode.Rows[i].Cells.FromKey(PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMID_FULL).ToString() + "','";
                    }

                }
            }
            catch
            {}

            itemId += "'";
            if (count > 0)
            {
                CHEER.Platform.DAL.PersistBroker _broker = CHEER.Platform.DAL.PersistBroker.Instance();
                try
                {
                    _broker.ExecuteNonQuery("DELETE FROM PSNPUBLICCODEITEM WHERE ITEMID IN (" + itemId + ")");
                    LoadGrid();
                    if (itemId == "''")
                    {
                        base.ShowAlert(base.getString("ZGAIA04177"));
                    }
                    else
                    {
                        base.ShowAlert(base.getString("ZGAIA00940"));
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    _broker.Close();
                }
            }
            else
            {
                base.ShowAlert(base.getString("ZGAIA00022"));
            }
        }

       
    }
}