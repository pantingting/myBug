using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.eCompetence.Common.Interfaces;
using CHEER.BusinessLayer.ePersonnel.Adjust;
using CHEER.BusinessLayer.ePersonnel.JobFamily;
using CHEER.BusinessLayer.ePersonnel.PersonnelBaseInfo;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.Common.Schema;
using CHEER.CommonLayer.ePersonnel;
using CHEER.CommonLayer.ePersonnel.Schema;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;
using CHEER.Platform.DAL.SQLCenter;
using CHEER.PresentationLayer.Controls;
using CHEER.Platform.Common;

namespace CHEER.PresentationLayer.Security
{
    public partial class AlotSeletlPage : CHEERBasePage
    {
        private Hashtable BranchName
        {
            get
            {
                return (Hashtable)ViewState["branchname"];
            }
            set
            {
                ViewState["branchname"] = value;
            }
        }
        private Hashtable PositionName
        {
            get
            {
                return (Hashtable)ViewState["positionname"];
            }
            set
            {
                ViewState["positionname"] = value;
            }
        }
        private ArrayList EnglishName = new ArrayList();
        private bool Isvisible
        {
            get { return (bool)ViewState["Isvisible"]; }
            set { ViewState["Isvisible"] = value; }
        }
        private bool IsEdit
        {
            get { return (bool)ViewState["IsEdit"]; }
            set { ViewState["IsEdit"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.CheckAccess();
                GetButtonText();
                (this.UserQuery as UserQuery).FunFriendlyID = "010020040";
                (this.UserQuery as UserQuery).Security_Checker = base.GetSecurityChecker();
                (this.UserQuery as UserQuery).IsHaveManageUnit = false;
            }
            (this.UserQuery as UserQuery).AcessionStatus_Unchecked = false;
            (this.UserQuery as UserQuery).AcessionStatus_Dimission = false;
            (this.UserQuery as UserQuery).AcessionStatus_Export = false;
            (this.UserQuery as UserQuery).AcessionStatus_PromotingProbation = false;
            (this.UserQuery as UserQuery).AcessionStatus_Retired = false;
        }
        private void GetButtonText()
        {
            this.cmdAdd.Text = "下一步";//"下一步";fff
        }
        private void CheckAccess()
        {
            if (!GetSecurityChecker().IsAllow("010020040"))
                ShowAlert("您没有此功能的权限！");
        }
        protected void Page_Init()
        {
            InitGrid();
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            (this.UserQuery as UserQuery).btnSearchEvent += btnQuery_Click;
        }
        private void InitGrid()
        {
            CommonMethod.AddFlexField(UltraBaseInfo, "组织代码", ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE, 15, false);
            CommonMethod.AddFlexField(UltraBaseInfo, "组织", PSNAccountSchema.PSNACCOUNT_BRANCHID_FULL, 15, false);
            CommonMethod.AddFlexField(UltraBaseInfo, "工号", PSNAccountSchema.PSNACCOUNT_EMPLOYEEID_FULL, 15, false);
            CommonMethod.AddFlexField(UltraBaseInfo, "姓名", PSNAccountSchema.PSNACCOUNT_TRUENAME_FULL, 15, false);
            CommonMethod.AddFlexField(UltraBaseInfo, "", PSNAccountSchema.PSNACCOUNT_PERSONID_FULL, 15, true);
            CommonMethod.AddFlexField(UltraBaseInfo, "英文名", PSNAccountSchema.PSNACCOUNT_LASTNAME_FULL, 15, false);
            CommonMethod.AddFlexField(UltraBaseInfo, "性别", PSNAccountSchema.PSNACCOUNT_GENDER_FULL, 15, false);
            CommonMethod.AddFlexField(UltraBaseInfo, "", PSNAccountSchema.PSNACCOUNT_MIDDLENAME_FULL, 15, true);
            CommonMethod.AddFlexField(UltraBaseInfo, "", PSNAccountSchema.PSNACCOUNT_FIRSTNAME_FULL, 15, true);
            CommonMethod.AddFlexField(UltraBaseInfo, "在职状态", PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE_FULL, 15, false);
            CommonMethod.AddFlexField(UltraBaseInfo, "集团职位", PSNAccountSchema.PSNACCOUNT_POSITIONID_FULL, 15, true);
            CommonMethod.AddFlexField(UltraBaseInfo, "职位", PSNAccountSchema.PSNACCOUNT_JOBCODE_FULL, 15, false);

        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            LoadData(0);
            if (UltraBaseInfo.Rows.Count == 0)
            {
                base.ShowAlert("没有查询到符合条件的数据！");
            }
        }

        private void LoadData(int index)
        {
            AccountAllot accountAllot = (AccountAllot)eHRPageServer.GetPalauObject(typeof(AccountAllot));
            SQLSelectEntity sqlInfo = accountAllot.AddCondition(InitSelectEntity());
            DataSet _dt = null;
            PersonManager _person = (PersonManager)base.GetPalauObject(typeof(PersonManager), "010020040", true, "", (this.UserQuery as UserQuery).IsHaveManageUnit, (this.UserQuery as UserQuery).ManageUnitID);
            string str = new SQLStringGener().GetSQL(base.InfomationPackage.DBType, sqlInfo);
            str += " order by " + ORGStdStructSchema.ORGSTDSTRUCT_LABELINDEX + " asc , " +
                PSNAccountSchema.PSNACCOUNT_EMPLOYEEID + " asc";
            _dt = _person.GetPersonsDstWithSecurity(str);
            UltraBaseInfo.RecordCount = _dt.Tables[0].Rows.Count;
            LoadData(GetPagedDataTable(index, UltraBaseInfo.PageSize, _dt.Tables[0]));
        }

        protected void LoadData(DataTable dt)
        {
            this.UltraBaseInfo.DataSource = dt;
            this.UltraBaseInfo.DataBind();
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
        private SQLSelectEntity InitSelectEntity()
        {
            SQLSelectEntity _retunrObj = (this.UserQuery as UserQuery).getSelectEntityWithNoSelectItems();

            #region 加入SelectItem列
            ArrayList _itemList = new ArrayList(13);

            _itemList.Add(new TableField(PSNAccountSchema.PSNACCOUNT_TABLENAME, PSNAccountSchema.PSNACCOUNT_PERSONID, PSNAccountSchema.PSNACCOUNT_PERSONID_FULL, SQLDataType.String));
            _itemList.Add(new TableField(PSNAccountSchema.PSNACCOUNT_TABLENAME, PSNAccountSchema.PSNACCOUNT_LASTNAME, PSNAccountSchema.PSNACCOUNT_LASTNAME_FULL, SQLDataType.String));
            _itemList.Add(new TableField(PSNAccountSchema.PSNACCOUNT_TABLENAME, PSNAccountSchema.PSNACCOUNT_MIDDLENAME, PSNAccountSchema.PSNACCOUNT_MIDDLENAME_FULL, SQLDataType.String));
            _itemList.Add(new TableField(PSNAccountSchema.PSNACCOUNT_TABLENAME, PSNAccountSchema.PSNACCOUNT_FIRSTNAME, PSNAccountSchema.PSNACCOUNT_FIRSTNAME_FULL, SQLDataType.String));
            _itemList.Add(new TableField(PSNAccountSchema.PSNACCOUNT_TABLENAME, PSNAccountSchema.PSNACCOUNT_TRUENAME, PSNAccountSchema.PSNACCOUNT_TRUENAME_FULL, SQLDataType.String));

            _itemList.Add(new TableField(PSNAccountSchema.PSNACCOUNT_TABLENAME, PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE, PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE_FULL, SQLDataType.Number));
            _itemList.Add(new TableField(PSNAccountSchema.PSNACCOUNT_TABLENAME, PSNAccountSchema.PSNACCOUNT_GENDER, PSNAccountSchema.PSNACCOUNT_GENDER_FULL, SQLDataType.Number));

            _itemList.Add(new TableField(PSNAccountSchema.PSNACCOUNT_TABLENAME, PSNAccountSchema.PSNACCOUNT_EMPLOYEEID, PSNAccountSchema.PSNACCOUNT_EMPLOYEEID_FULL, SQLDataType.String));
            _itemList.Add(new TableField(PSNAccountSchema.PSNACCOUNT_TABLENAME, PSNAccountSchema.PSNACCOUNT_BRANCHID, PSNAccountSchema.PSNACCOUNT_BRANCHID_FULL, SQLDataType.String));
            _itemList.Add(new TableField(PSNAccountSchema.PSNACCOUNT_TABLENAME, PSNAccountSchema.PSNACCOUNT_POSITIONID, PSNAccountSchema.PSNACCOUNT_POSITIONID_FULL, SQLDataType.String));
            _itemList.Add(new TableField(PSNAccountSchema.PSNACCOUNT_TABLENAME, PSNAccountSchema.PSNACCOUNT_JOBCODE, PSNAccountSchema.PSNACCOUNT_JOBCODE_FULL, SQLDataType.String));
            _itemList.Add(new TableField(ORGStdStructSchema.ORGSTDSTRUCT_TABLE, ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE, SQLDataType.String));
            #endregion
            _retunrObj.SelectItems.AddRange(_itemList);
            return _retunrObj;
        }

        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            if (UltraBaseInfo.SelectedRowIndexArray.Length <= 0)
            {
                ShowAlert("请先选择员工！");
                return;
            }
            Hashtable hashpersonIDs = new Hashtable();
            int[] rows = UltraBaseInfo.SelectedRowIndexArray;
            foreach (int row in rows)
            {
                hashpersonIDs.Add( UltraBaseInfo.Rows[row].Cells.FromKey(PSNAccountSchema.PSNACCOUNT_PERSONID_FULL).ToString(),
						UltraBaseInfo.Rows[row].Cells.FromKey(PSNAccountSchema.PSNACCOUNT_EMPLOYEEID_FULL).ToString() + "#" +
						UltraBaseInfo.Rows[row].Cells.FromKey(PSNAccountSchema.PSNACCOUNT_LASTNAME_FULL).ToString().Trim() );
            }
            if (hashpersonIDs.Count > 0)
            {
                Session["allotpersons"] = hashpersonIDs;
                CheerUI.PageContext.Redirect("SeniorAccountAllotPage.aspx"); 
            }
        }

        protected void UltraBaseInfo_PageIndexChange(object sender, CheerUI.GridPageEventArgs e)
        {
            this.UltraBaseInfo.PageIndex = e.NewPageIndex;
            LoadData(e.NewPageIndex);
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UltraBaseInfo.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            LoadData(this.UltraBaseInfo.PageIndex);
        }

        protected void UltraBaseInfo_RowDataBound(object sender, CheerUI.GridRowEventArgs e)
        {
            Hashtable _positionhst = ((PositionManager)eHRPageServer.GetPalauObject(typeof(PositionManager))).GetAllWithHastable();
            this.PositionName = _positionhst;
            JobcodeInterface obj = (JobcodeInterface)eHRPageServer.GetPalauObject(typeof(JobcodeInterface));

            Hashtable _jobcodehst = obj.GetAllJobcode(null);
            ArrayList _personBrachList = new ArrayList();
            for (int i = 0; i < UltraBaseInfo.Rows.Count; i++)
            {
                _personBrachList.Add(UltraBaseInfo.Rows[i].Cells.FromKey(PSNAccountSchema.PSNACCOUNT_BRANCHID_FULL).ToString());
            }
            Hashtable _branchhst = ((AdjustManager)eHRPageServer.GetPalauObject(typeof(AdjustManager))).GetDeptNameHashtableByID(_personBrachList);	
            //设置性别
            int _genderValue = Int32.Parse(UltraBaseInfo.Rows[e.RowIndex].Cells.FromKey(PSNAccountSchema.PSNACCOUNT_GENDER_FULL).ToString());
            switch ((Gender)_genderValue)
            {
                case Gender.Male:
                    UltraBaseInfo.Rows[e.RowIndex].Cells.SetValue(PSNAccountSchema.PSNACCOUNT_GENDER_FULL, "男");
                    break;
                case Gender.Female:
                    UltraBaseInfo.Rows[e.RowIndex].Cells.SetValue(PSNAccountSchema.PSNACCOUNT_GENDER_FULL, "女");
                    break;
                default:
                    UltraBaseInfo.Rows[e.RowIndex].Cells.SetValue(PSNAccountSchema.PSNACCOUNT_GENDER_FULL, "");
                    break;
            }
            #region	设置英文名
            string _middleName = UltraBaseInfo.Rows[e.RowIndex].Cells.FromKey(PSNAccountSchema.PSNACCOUNT_MIDDLENAME_FULL).ToString();
            string _englishName = "";
            if (_middleName == null || _middleName.Length == 0)
            {
                _englishName = UltraBaseInfo.Rows[e.RowIndex].Cells.FromKey(PSNAccountSchema.PSNACCOUNT_FIRSTNAME_FULL).ToString()
                    + " " + UltraBaseInfo.Rows[e.RowIndex].Cells.FromKey(PSNAccountSchema.PSNACCOUNT_LASTNAME_FULL).ToString();
            }
            else
            {
                _englishName = UltraBaseInfo.Rows[e.RowIndex].Cells.FromKey(PSNAccountSchema.PSNACCOUNT_FIRSTNAME_FULL).ToString()
                    + " " + _middleName + " " + UltraBaseInfo.Rows[e.RowIndex].Cells.FromKey(PSNAccountSchema.PSNACCOUNT_LASTNAME_FULL).ToString();
            }
            UltraBaseInfo.Rows[e.RowIndex].Cells.SetValue(PSNAccountSchema.PSNACCOUNT_LASTNAME_FULL,_englishName);
            #endregion

            #region 根据集团职位信息和组织信息替换退休人员grid中的组织名称和集团职位名称
            string _accessionStateCell = UltraBaseInfo.Rows[e.RowIndex].Cells.FromKey(PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE_FULL).ToString();
            string _gridBarnch = UltraBaseInfo.Rows[e.RowIndex].Cells.FromKey(PSNAccountSchema.PSNACCOUNT_BRANCHID_FULL).ToString();
            if (_branchhst[_gridBarnch] == null)
            {
                UltraBaseInfo.Rows[e.RowIndex].Cells.SetValue(PSNAccountSchema.PSNACCOUNT_BRANCHID_FULL, "");
            }
            else
            {
                UltraBaseInfo.Rows[e.RowIndex].Cells.SetValue(PSNAccountSchema.PSNACCOUNT_BRANCHID_FULL, _branchhst[_gridBarnch].ToString());
            }
            string _gridPosition = UltraBaseInfo.Rows[e.RowIndex].Cells.FromKey(PSNAccountSchema.PSNACCOUNT_POSITIONID_FULL).ToString();
            if ( _positionhst[_gridPosition] == null)
            {
                UltraBaseInfo.Rows[e.RowIndex].Cells.SetValue(PSNAccountSchema.PSNACCOUNT_POSITIONID_FULL, "");
            }
            else
            {
                UltraBaseInfo.Rows[e.RowIndex].Cells.SetValue(PSNAccountSchema.PSNACCOUNT_POSITIONID_FULL, _positionhst[_gridPosition].ToString());
            }

            string _gridJobcode = UltraBaseInfo.Rows[e.RowIndex].Cells.FromKey(PSNAccountSchema.PSNACCOUNT_JOBCODE_FULL).ToString();
            if ( _jobcodehst[_gridJobcode] == null)
            {
                UltraBaseInfo.Rows[e.RowIndex].Cells.SetValue(PSNAccountSchema.PSNACCOUNT_JOBCODE_FULL, "");
            }
            else
            {
                UltraBaseInfo.Rows[e.RowIndex].Cells.SetValue(PSNAccountSchema.PSNACCOUNT_JOBCODE_FULL, _jobcodehst[_gridJobcode].ToString());
            }
            #endregion
            #region 
            int _accessionStateValue;
            if (_accessionStateCell == "")
                _accessionStateValue = -1;
            else
                _accessionStateValue = Convert.ToInt32(_accessionStateCell);
            switch ((AccessionStatus)_accessionStateValue)
            {
                case AccessionStatus.Dimission:

                    UltraBaseInfo.Rows[e.RowIndex].Cells.SetValue(PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE_FULL, "离职");
                    break;
                case AccessionStatus.Export:
                    UltraBaseInfo.Rows[e.RowIndex].Cells.SetValue(PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE_FULL, "调出");
                    break;
                case AccessionStatus.Probation:
                    UltraBaseInfo.Rows[e.RowIndex].Cells.SetValue(PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE_FULL, "试用");
                    break;
                case AccessionStatus.PromotingProbation:
                    UltraBaseInfo.Rows[e.RowIndex].Cells.SetValue(PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE_FULL, "晋级试用");
                    break;
                case AccessionStatus.Regular:
                    UltraBaseInfo.Rows[e.RowIndex].Cells.SetValue(PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE_FULL, "正式");
                    break;
                case AccessionStatus.Retired:
                    UltraBaseInfo.Rows[e.RowIndex].Cells.SetValue(PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE_FULL, "退休");
                    break;
                case AccessionStatus.Unchecked:
                    UltraBaseInfo.Rows[e.RowIndex].Cells.SetValue(PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE_FULL, "未接收");
                    break;
                default:
                    UltraBaseInfo.Rows[e.RowIndex].Cells.SetValue(PSNAccountSchema.PSNACCOUNT_ACCESSIONSTATE_FULL, "未知");
                    break;
            }
            #endregion
        }

        
    }
}