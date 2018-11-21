using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.ePersonnel.SystemConfig;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.CommonLayer.eSecurity.Data;
using CHEER.CommonLayer.eSecurity.Schema;
using CHEER.PresentationLayer;
using CHEER.Platform.DAL;
using CHEER.PresentationLayer.CommonUse;
using CHEER.PresentationLayer.Controls;
using CHEER.Platform.Common;

namespace CHEER.PresentationLayer.Security
{
    public partial class AccountAllotPage : CHEERBasePage
    {
        private string accountpassword
        {
            get { return (string)ViewState["accountpassword"]; }
            set { ViewState["accountpassword"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitFace();
                CheckPageAccess();
                init_dropdownlist();
            }
        }
        private const string AllotFunID = "010020040";
        private void InitFace()
        {
            this.gp1.Title = "分配规则";
            this.Label10.Text = "(说明：以员工工号作为对应员工的登录帐号)";
            this.Label4.Text = "生效时间";
            this.Label11.Text = "(说明：默认选择系统日期，可以设定)";
            this.Label5.Text = "失效时间";
            this.Label12.Text = "(说明：默认为空，如果设定需大于生效时间)";
            this.Label6.Text = "初始密码";
            this.Label9.Text = "(说明：默认为帐号初始密码，可以设定)";
            this.Label7.Text = "初次登录修改密码";
            this.ckpwdchange.Text = "是";
            this.Label13.Text = "(说明：配置分配帐号初次登录密码修改状况)";
            this.Label8.Text = "员工在职状态";
            this.ckReg.Text = "正式";
            this.ckPro.Text = "试用";
            this.Label14.Text = "(说明：员工在职状态范围设定)";
            this.Label1.Text = "所属角色";
            this.Label3.Text = "(说明：员工帐号所担任的角色配置)";
            this.gp2.Title = "员工范围";
            this.Label15.Text = "(说明：默认情况下为用户具有数据权限的员工)";
            this.Label26.Text = "组织";
            this.Label23.Text = "工号从/到";
            this.Label18.Text = "入职日期从/到";
            this.Label20.Text = "员工类别";
            this.cmdAdd.Text = "分配";
            this.cmdLook.Text = "查看详细";
            this.txtStartDate.Text = DataProcessor.DateTimeToShortString(DateTime.Now);
            PersonnelParameterManager _manager = (PersonnelParameterManager)eHRPageServer.GetPalauObject(typeof(PersonnelParameterManager));
            this.accountpassword = _manager.GetParameterValue(PersonnelParameterName.DefaultPassword);
            txtPassWord.Text = accountpassword;
            this.cmdLook.Enabled = false;
            this.ckpwdchange.Checked = true;
            this.ckReg.Checked = true;
            this.ckPro.Checked = true;
        }
        private void CheckPageAccess()
        {
            if (!GetSecurityChecker().IsAllow(AllotFunID))
                base.ShowAlert("您没有此功能的权限！");
        }
        private void LoadPassword()
        {
            if (this.accountpassword != null && this.accountpassword.Trim() != "")
            {
                //this.RegisterStartupScript(Guid.NewGuid().ToString(), "<script>document.getElementById('txtPassWord').value='" + this.accountpassword + "';</script>");
                CheerUI.PageContext.RegisterStartupScript("F('" + txtPassWord.ClientID + "').setValue(" + this.accountpassword + ")");
            }
        }
        private void init_dropdownlist()
        {
            (StdBranchLoader as StdBranchLoader).IsHaveManageUnit = false;
            (StdBranchLoader as StdBranchLoader).LoadBranchDtsByID(base.GetSecurityChecker(), AllotFunID);
            ControlFiller.FillPersonnelPublicCodeReserve(this.drpType, PersonnelPublicCodeType.EmployeeType);
            //ControlFiller.FillDLIDL(this.drpDLIDL);
            string pathField = SECRoleSchema.SECROLE_TABLE + "." + SECRoleSchema.DEPTID;
            Role rolemanager = (Role)GetPalauObject(typeof(Role), AllotFunID, false, pathField, false, "");
            DataSet roleds = rolemanager.GetRoleDatasWithSecurity(null, null, false, null, base.getBusinessUnitID());
            DataView dv = roleds.Tables[0].DefaultView;
            dv.Sort = SECRoleSchema.ROLENAME;
            this.drpRoles.DataSource = dv;
            this.drpRoles.DataTextField = SECRoleSchema.ROLENAME;
            this.drpRoles.DataValueField = SECRoleSchema.ROLEID;
            this.drpRoles.DataBind();
            this.drpRoles.Items.Insert(0, new CheerUI.ListItem("", ""));

            //*********DLIDL 链接到公用代码************************
            PersistBroker _broker = PersistBroker.Instance();
            _broker.Open();
            try
            {
                drpDLIDL.Items.Clear();
                string sql = "SELECT ITEMID,ITEMVALUE FROM PSNPUBLICCODEITEM WHERE typeid='s3'";
                DataTable dt = _broker.ExecuteDataset(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    drpDLIDL.Items.Add(new CheerUI.ListItem("", ""));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        drpDLIDL.Items.Add(new CheerUI.ListItem(dt.Rows[i][1].ToString(), dt.Rows[i][0].ToString()));
                    }
                }
            }
            finally
            {
                _broker.Close();
            }

        }

        void constructdata(out SecAllotData obj, out string ermsg)
        {
            AccountAllot allot = new AccountAllot();
            obj = new SecAllotData();
            obj.IsIncludeProbation = this.ckPro.Checked;
            obj.IsIncludeRegular = ckReg.Checked;
            obj.IsPWDfristChange = this.ckpwdchange.Checked;
            obj.Operator = base.GetSecurityChecker().UserID;
            obj.StartTime = DataProcessor.StringToDateTime(this.txtStartDate.Text);
            obj.ToTime = DataProcessor.StringToDateTime(this.txtEndDate.Text);
            //这是原有的密码方式
            //obj.PassWord = this.txtPassWord.Text.Trim().DBReplace();
            //这是目前系统中统一的密码方式
            obj.PassWord = allot.Hash(this.txtPassWord.Text.Trim());
            //obj.PassWord = Hash(this.txtPassWord.Text.Trim());


            ermsg = "";
            if (obj.IsPwdNull())
            {
                ermsg += "密码不能为空！";
                return;
            }
            this.accountpassword = obj.PassWord;
            if (obj.IsStartDateNull())
            {
                ermsg += "生效时间不能为空！";
                return;
            }
            if (!obj.IsTimeLogical())
            {
                ermsg += "失效时间必须大于生效时间！";
                return;
            }
        }
        string Operator
        {
            get { return ViewState["Operator"].ToString(); }
            set { ViewState["Operator"] = value; }
        }
        DateTime OperTime
        {
            get { return (DateTime)ViewState["OperTime"]; }
            set { ViewState["OperTime"] = value; }
        }

        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            SecAllotData obj; string ermsg;
            constructdata(out obj, out ermsg);
            if (ermsg != "")
            {
                LoadPassword();
                base.ShowAlert(ermsg);
                return;
            }
            AccountAllot allotmgr = (AccountAllot)base.GetPalauObject(typeof(AccountAllot), AllotFunID, true, "", false, "");
            string stroper; DateTime opertime;

            LicenseChecker LC = new LicenseChecker();
            int Managers = LC.GetMangerLicense(this);
            int Users = LC.GetUserLicense(this);
            int UsedManagers = LC.GetUsedMangerLicense();
            int UsedUsers = LC.GetUsedUserLicense();
            int iEffectCount = iEffectCount = allotmgr.GetEffectCount((this.StdBranchLoader as StdBranchLoader).GetSelectBranchItem().Value, this.txtEmpFrom.Text.Trim(), this.txtEmpTo.Text.Trim(),
                    UcDate1.Text, UcDate2.Text, this.drpDLIDL.SelectedValue, this.drpType.SelectedValue);

            //2013.8.16 Add by David 增加License判断
            if (this.drpRoles.SelectedValue != null && this.drpRoles.SelectedValue != "")  //选中角色，则需要判断License
            {
                if (LC.IsRoleManager(drpRoles.SelectedValue))
                {
                    int Remains = Managers - UsedManagers;
                    if (iEffectCount > Remains)
                    {
                        string format100 = "[{2}数]超出License限制！" + "\\r\\n授权数：{0}" + "\\r\\n" + "当前数据：{1} \\r\\n";
                        string MSG = String.Format(format100, new object[] { Managers, UsedManagers, "主管自助" });
                        base.ShowAlert(MSG);
                        return;
                    }
                }
                else
                {
                    int Remains = Users - UsedUsers;
                    if (iEffectCount > Remains)
                    {
                        string format100 = "[{2}数]超出License限制！" + "\\r\\n授权数：{0}" + "\\r\\n" + "当前数据：{1} \\r\\n";
                        string MSG = String.Format(format100, new object[] { Users, UsedUsers, "员工自助" });
                        base.ShowAlert(MSG);
                        return;
                    }
                }
            }

            if (UsedUsers + UsedManagers + iEffectCount >= Users + Managers)
            {
                string format100 = "[{2}数]超出License限制！" + "\\r\\n授权数：{0}" + "\\r\\n" + "当前数据：{1} \\r\\n";
                string MSG = String.Format(format100, new object[] { Users + Managers, UsedUsers + UsedManagers, "用户" });
                base.ShowAlert(MSG);
                return;
            }

            int _count = allotmgr.Allot(obj, (this.StdBranchLoader as StdBranchLoader).GetSelectBranchItem().Value, this.txtEmpFrom.Text.Trim(), this.txtEmpTo.Text.Trim(),
                UcDate1.Text, UcDate2.Text, this.drpDLIDL.SelectedValue, this.drpType.SelectedValue, out stroper, out opertime, this.drpRoles.SelectedValue);
            string msg = "帐号分配信息：已经为" + _count + "个员工成功分配帐号！";
            if (_count > 0)
            {
                msg += "您可以单击查看详细按钮，转入帐号分配信息查看面查看！";
                this.cmdLook.Enabled = true;
            }
            else
            {
                this.cmdLook.Enabled = false;
            }
            this.lblmsg.Text = msg;
            LoadPassword();
            setkeydata(stroper, opertime);
            base.ShowAlert("分配成功！");
        }

        protected void cmdLook_Click(object sender, EventArgs e)
        {
            if (ViewState["Operator"] != null && ViewState["OperTime"] != null)
                CheerUI.PageContext.Redirect("AllotLookPage.aspx?stroper=" + Operator + "&timeoper=" + DataProcessor.DateTimeToStandardString(OperTime) + "&FromURL=AccountAllotPage.aspx");
        }
        void setkeydata(string oper, DateTime time)
        {
            Operator = oper;
            OperTime = time;
        }

    }
}