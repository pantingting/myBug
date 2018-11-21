using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.ePersonnel.SystemConfig;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.CommonLayer.ePersonnel;
using CHEER.CommonLayer.eSecurity.Data;
using CHEER.CommonLayer.eSecurity.Schema;
using CHEER.PresentationLayer;

namespace CHEER.PresentationLayer.Security
{
    public partial class SeniorAccountAllotPage : CHEERBasePage
    {
        private string accountpassword
        {
            get { return (string)ViewState["accountpassword"]; }
            set { ViewState["accountpassword"] = value; }
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
        public string erFileID
        {
            get { return ViewState["erFileID"].ToString(); }
            set { ViewState["erFileID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitFace();
                init_dropdownlist();
            }
        }
        private void InitFace()
        {
            this.gp1.Title = "分配规则";
            this.drpLogin.Label = "帐号名";
            this.Label1.Text = "(说明：可以以工号或者英文名作为帐号名)";
            this.txtBegin.Label = "帐号前缀";
            this.Label10.Text = "(说明：一般为不超过5位的字母或者数字组合)";
            this.txtEnd.Label = "帐号后缀";
            this.Label15.Text = "(说明：一般为不超过5位的字母或者数字组合)";
            this.txtStartDate.Label = "生效时间";
            this.Label11.Text = "(说明：默认选择系统日期，可以设定)";
            this.txtEndDate.Label = "失效时间";
            this.Label12.Text = "(说明：默认为空，如果设定需大于生效时间)";
            this.txtPassWord.Label = "初始密码";
            this.Label9.Text = "(说明：默认为帐号初始密码，可以设定)";
            this.ckpwdchange.Label = "初次登录修改密码";
            this.ckpwdchange.Text = "是";
            this.Label13.Text = "(说明：配置分配帐号初次登录密码修改状况)";
            this.drpRoles.Label = "所属角色";
            this.Label14.Text = "(说明：员工帐号所担任的角色配置)";
            this.cmdAdd.Text = "分配";
            this.cmdLook.Text = "查看详细";
            this.cmdErDown.Text = "错误信息下载";
            this.cmdReturn.Text = "返回";
            this.txtStartDate.Text = DataProcessor.DateTimeToShortString(DateTime.Now);
            PersonnelParameterManager _manager = (PersonnelParameterManager)eHRPageServer.GetPalauObject(typeof(PersonnelParameterManager));
            this.accountpassword = _manager.GetParameterValue(PersonnelParameterName.DefaultPassword);
            this.ckpwdchange.Checked = true;
            this.cmdErDown.Enabled = false;
            this.cmdLook.Enabled = false;
            txtPassWord.Text = accountpassword;
            this.drpLogin.Items.Add(new CheerUI.ListItem("工号", "1"));
            this.drpLogin.Items.Add(new CheerUI.ListItem("英文名", "2"));
        }
        private const string AllotFunID = "010020040";
        private void init_dropdownlist()
        {
            string pathField = SECRoleSchema.SECROLE_TABLE + "." + SECRoleSchema.DEPTID;
            Role rolemanager = (Role)GetPalauObject(typeof(Role), AllotFunID, false, pathField, false, "");
            DataSet roleds = rolemanager.GetRoleDatasWithSecurity(null, null, false, null);
            DataView dv = roleds.Tables[0].DefaultView;
            dv.Sort = SECRoleSchema.ROLENAME;
            this.drpRoles.DataSource = dv;
            this.drpRoles.DataTextField = SECRoleSchema.ROLENAME;
            this.drpRoles.DataValueField = SECRoleSchema.ROLEID;
            this.drpRoles.DataBind();
            this.drpRoles.Items.Insert(0, new CheerUI.ListItem("", ""));
        }
        void constructdata(out SecAllotData obj, out string ermsg)
        {
            AccountAllot allot = new AccountAllot();

            obj = new SecAllotData();
            obj.IsIncludeProbation = true;
            obj.IsIncludeRegular = true;
            obj.IsPWDfristChange = this.ckpwdchange.Checked;
            obj.Operator = base.GetSecurityChecker().UserID;
            obj.StartTime = DataProcessor.StringToDateTime(this.txtStartDate.Text);
            obj.ToTime = DataProcessor.StringToDateTime(this.txtEndDate.Text);
            //这是原有的密码方式
            //obj.PassWord = this.txtPassWord.Text.Trim().DBReplace();
            //这是目前系统中统一的密码方式
            //obj.PassWord = Hash(this.txtPassWord.Text.Trim());
            obj.PassWord = allot.Hash(this.txtPassWord.Text.Trim());

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
            if (this.txtBegin.Text.Trim() != "")
            {
                string strb = txtBegin.Text.Trim().DBReplace();
                if (strb.IndexOf("#") != -1 || strb.IndexOf("'") != -1
                    || strb.IndexOf("[") != -1 || strb.IndexOf("]") != -1)
                {
                    ermsg += "帐号前缀包含非法字符#,',[,]等！";
                    return;
                }
            }
            if (this.txtEnd.Text.Trim() != "")
            {
                string strb = txtEnd.Text.Trim().DBReplace();
                if (strb.IndexOf("#") != -1 || strb.IndexOf("'") != -1
                    || strb.IndexOf("[") != -1 || strb.IndexOf("]") != -1)
                {
                    ermsg += "帐号后缀包含非法字符#,'[,]等！";
                }
            }
        }

        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            SecAllotData obj; string ermsg;
            constructdata(out obj, out ermsg);
            if (ermsg != "")
            {
                txtPassWord.Text = this.accountpassword;
                base.ShowAlert(ermsg);
                return;
            }
            AccountAllot allotmgr = (AccountAllot)eHRPageServer.GetPalauObject(typeof(AccountAllot));
            if (Session["allotpersons"] != null)
            {
                Hashtable hash = (Hashtable)Session["allotpersons"];
                string stroper; DateTime opertime;
                bool emporeng = true;
                if (this.drpLogin.SelectedValue == "2")
                    emporeng = false;
                bool cser;

                DateTime _now = DateTime.Now;
                string _time = _now.Year.ToString() + _now.Month.ToString() + _now.Day.ToString() + _now.Hour.ToString() + _now.Minute.ToString();

                string filename = Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERNAME].ToString() + _time + IDGenerator.GenIDString() + ".txt";
                string filepath = Server.MapPath("") + "/AllotInfo/" + filename;
                int _count = allotmgr.Allot(obj, hash, out stroper, out opertime, this.drpRoles.SelectedValue, this.txtBegin.Text.Trim(), this.txtEnd.Text.Trim(), emporeng, out cser, filepath);
                if (cser)
                {
                    this.cmdErDown.Enabled = true;
                    this.lblmsg.Text = "分配失败信息：分配帐号过程中出现错误，您可以单击下载错误按钮查看出现错误的员工及原因！";
                }
                this.lblmsg.Text = "帐号分配信息：已经为" + _count + "个员工成功分配帐号！您可以单击查看详细按钮，转入帐号分配信息查看面查看！";
                if (_count > 0)
                {
                    this.cmdLook.Enabled = true;
                }
                else
                {
                    this.cmdLook.Enabled = false;
                }
                setkeydata(stroper, opertime, cser, filename);
                txtPassWord.Text = this.accountpassword;
                if (!cser)
                {
                    base.ShowAlert("分配成功！");
                }
                this.cmdAdd.Enabled = false;
            }
            Session["allotpersons"] = null;
        }
        void setkeydata(string oper, DateTime time, bool containser, string filename)
        {
            Operator = oper;
            OperTime = time;
            if (containser)
                erFileID = filename;
        }

        protected void cmdLook_Click(object sender, EventArgs e)
        {
            if (ViewState["Operator"] != null && ViewState["OperTime"] != null)
                CheerUI.PageContext.Redirect("AllotLookPage.aspx?stroper=" + Operator + "&timeoper=" + DataProcessor.DateTimeToStandardString(OperTime) + "&FromURL=AlotSeletlPage.aspx");
        }

        protected void cmdReturn_Click(object sender, EventArgs e)
        {
            CheerUI.PageContext.Redirect("AlotSeletlPage.aspx?" + AppConstants.IS_BACK + "=" + AppConstants.IS_BACK);
        }

        protected void cmdErDown_Click(object sender, EventArgs e)
        {
            string _fileID = Server.MapPath("") + "/AllotInfo/" + erFileID;
            if (_fileID != null && _fileID.Trim() != "")
            {
                FileStream filereader = new FileStream(_fileID, FileMode.OpenOrCreate, FileAccess.Read);
                System.IO.BinaryReader reader = new BinaryReader(filereader);
                byte[] _bytes = reader.ReadBytes((int)filereader.Length);
                reader.Close();
                string _strFileName = "alloterrormessage" + ".txt";

                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + _strFileName);
                Response.AddHeader("Content-Length", _bytes.Length.ToString());
                Response.AddHeader("Content-Length", _bytes.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.BinaryWrite(_bytes);
                Response.Charset = "UTF-8";
                Response.End();
            }
        }


    }
}