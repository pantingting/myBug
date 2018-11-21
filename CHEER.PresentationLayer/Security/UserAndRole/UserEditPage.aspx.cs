using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.PresentationLayer;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.CommonLayer.eSecurity.Data;
using CHEER.PresentationLayer.Controls;
using CHEER.BusinessLayer.ePersonnel.PersonnelBaseInfo;
using CHEER.CommonLayer.ePersonnel.Data;
using System.Security.Cryptography;
using System.Text;
using CHEER.Common.AppConstants;

namespace CHEER.PresentationLayer.Security.UserAndRole
{
    public partial class UserEditPage : CHEERBasePage
    {
        private const string UserEditFunID = "0090000500010002";
        private string UserID
        {
            get { return (string)ViewState["UserID"]; }
            set { ViewState["UserID"] = value; }
        }
        private string PassWord
        {
            get { return (string)ViewState["PassWord"]; }
            set { ViewState["PassWord"] = value; }
        }
        public const string dispwd = "##########**********$$$$$$$$$$@@@@@@@@@@||||||||||";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.init_face();
                this.get_transdata();
                this.init_dropdownlist();
                this.load_data();
                (this.PersonSelect as PersonSelect).detailWindow = detailWindow;
            }
        }
        private void init_face()
        {
            this.btnAdd.Text = "保存";
            txtLoginName.Label = "登录名";
            txtPassWord.Label = "密码";
            txtConfirmPwd.Label = "确认密码";
            txtStartDate.Label = "生效时间";
            txtEndDate.Label = "失效时间";
            drpChangePwd.Label = "下次登录更改密码";
            drpIsLock.Label = "是否停用";
            txtDescription.Label = "用户描述";
            txtDescription.MaxLengthMessage = "(不得超过100字)";
            this.btnAdd.Enabled = this.CheckFunAccess(UserEditFunID);
        }
        private void get_transdata()
        {
            if (Request.QueryString["USERID"] != null)
                this.UserID = Request.QueryString["USERID"].Trim();
            else
                this.UserID = "";
        }
        private void init_dropdownlist()
        {
            this.drpChangePwd.Items.Clear();
            this.drpChangePwd.Items.Add(new CheerUI.ListItem("是", "1"));
            this.drpChangePwd.Items.Add(new CheerUI.ListItem("否", "0"));
            this.drpIsLock.Items.Clear();
            this.drpIsLock.Items.Add(new CheerUI.ListItem("否", "0"));
            this.drpIsLock.Items.Add(new CheerUI.ListItem("是", "1"));
        }
        private void load_data()
        {
            if (this.UserID.Trim() != "")
            {
                string userid = this.UserID.Trim();
                User usermanager = (User)eHRPageServer.GetPalauObject(typeof(User));
                SECUserData userdata = usermanager.GetSelfInfor(userid);
                this.PassWord = userdata.PASSWORD;
                this.txtDescription.Text = userdata.USERDESC;
                this.txtLoginName.Text = userdata.LOGINNAME;
                this.drpChangePwd.SelectedValue = userdata.NEVERUSED.ToString();
                this.drpIsLock.SelectedValue = userdata.ISLOCK.ToString();
                this.txtStartDate.Text = userdata.STARTDATE;
                this.txtEndDate.Text = userdata.ENDDATE;
                (this.PersonSelect as PersonSelect).TextPersonId.Text = userdata.PERSONID;
                PersonAccountManager accountmanager = new PersonAccountManager();
                PersonAccountData accountdata = accountmanager.Get(userdata.PERSONID.Trim());
                if (accountdata != null && accountdata.TrueName != null && accountdata.TrueName.Trim() != "")
                    (this.PersonSelect as PersonSelect).TextName.Text = accountdata.TrueName;
                //this.RegisterStartupScript(Guid.NewGuid().ToString(), "<script>var pwd=document.getElementById('txtPassWord');var compwd=document.getElementById('txtConfirmPwd');pwd.value='" + this.PassWord + "';compwd.value=pwd.value;</script>");
                CheerUI.PageContext.RegisterStartupScript("var pwd=C('" + txtPassWord.ClientID + "');var compwd=C('" + txtConfirmPwd.ClientID + "');pwd.setValue('" + PassWord + "'); compwd.setValue(pwd.getValue());");
            }
        }
        private bool CheckFunAccess(string funid)
        {
            return base.GetSecurityChecker().IsAllow(funid);
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtConfirmPwd.Text.Trim() != txtPassWord.Text.Trim())
            {
                ShowAlert("密码不正确！");
                return;
            }
            User usermanager = (User)eHRPageServer.GetPalauObject(typeof(User));
            string loginname = this.txtLoginName.Text.Trim().DBReplace();
            if (usermanager.JudgeIfLoginNameExist(loginname, this.UserID.Trim()))
            {
                string alex = "调整班别";
                base.ShowAlert(alex);
                return;
            }
            SECUserData userdata = usermanager.GetSelfInfor(this.UserID.Trim());
            userdata.BUILDDATE = DataProcessor.DateTimeToShortString(DateTime.Now);
            userdata.ENDDATE = this.txtEndDate.Text.Trim().DBReplace();
            userdata.ISLOCK = Convert.ToInt32(this.drpIsLock.SelectedValue.Trim());
            userdata.LASTCHANGEDATE = DataProcessor.DateTimeToShortString(DateTime.Now);
            userdata.LASTCHANGER = Session[SystemAppConstants.SESSION_USERNAME].ToString().Trim();
            userdata.LOGINNAME = this.txtLoginName.Text.Trim().DBReplace();
            userdata.NEVERUSED = Convert.ToInt32(this.drpChangePwd.SelectedValue.Trim());
            if (this.txtPassWord.Text.Trim() != PassWord)
            {
                userdata.PASSWORD = Hash(this.txtPassWord.Text.Trim());
            }
            userdata.PERSONID = (PersonSelect as PersonSelect).TextPersonId.Text.Trim().DBReplace();
            userdata.STARTDATE = this.txtStartDate.Text.Trim().DBReplace();
            userdata.USERDESC = this.txtDescription.Text.Trim().DBReplace();
            userdata.USERID = this.UserID.Trim();
            if (this.UserID.Trim() != "")
            {
                usermanager.UpdateUserData(userdata);
                base.ShowAlert("保存成功!");
                string userid = userdata.USERID.Trim();
                //this.RegisterStartupScript(Guid.NewGuid().ToString(), "<script>self.parent.document.getElementById('txtUserID').value='" + userid + "';</script>");
                this.load_data();
            }
        }

        public string Hash(string toHash)
        {
            MD5CryptoServiceProvider crypto = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(toHash);
            bytes = crypto.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            foreach (byte num in bytes)
            {
                sb.AppendFormat("{0:x2}", num);
            }
            return sb.ToString();        //32位
        }
    }
}