using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.Common;
using CHEER.PresentationLayer;
using CHEER.BusinessLayer.Security;
using CHEER.CommonLayer.eSecurity.Data;
using CHEER.PresentationLayer.Controls;
using CHEER.BusinessLayer.ePersonnel.PersonnelBaseInfo;
using CHEER.CommonLayer.ePersonnel.Data;
using CheerUI;
using System.Security.Cryptography;
using System.Text;
using CHEER.Common.AppConstants;
using CHEER.Platform.Common;

namespace CHEER.PresentationLayer.Security.UserAndRole
{
    public partial class UserAddPage : CHEERBasePage
    {
        private const string UserAddFunID = "0090000500010001";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitFace();
                init_dropdownlist();
                (this.PersonSelect as PersonSelect).detailWindow = detailWindow;
            }
        }

        private void InitFace()
        {
            this.btnAdd.Text = base.getString("ZGAIA00195");
            txtLoginName.Label = base.getString("ZGAIA00238");
            txtPassWord.Label = base.getString("ZGAIA00301");
            txtConfirmPwd.Label = base.getString("ZGAIA00300");
            txtStartDate.Label = base.getString("ZGAIA00284");
            txtEndDate.Label = base.getString("ZGAIA00285");
            drpChangePwd.Label = base.getString("ZGAIA00304");
            drpIsLock.Label = base.getString("ZGAIA00239");
            txtDescription.Label = base.getString("ZGAIA00265");
            txtDescription.MaxLengthMessage = base.getString("ZGAIA00302");
            this.btnAdd.Enabled = this.CheckFunAccess(UserAddFunID);
        }
        private void init_dropdownlist()
        {
            this.drpChangePwd.Items.Clear();
            this.drpChangePwd.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA00785"), "0"));
            this.drpChangePwd.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA00254"), "1"));
            this.drpIsLock.Items.Clear();
            this.drpIsLock.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA00785"), "0"));
            this.drpIsLock.Items.Add(new CheerUI.ListItem(base.getString("ZGAIA00254"), "1"));
        }

        private bool CheckFunAccess(string funid)
        {
            return base.GetSecurityChecker().IsAllow(funid);
        }

        private void LoadData()
        {
            if (this.txtUserID.Text.Trim() != "")
            {
                string userid = this.txtUserID.Text.Trim().DBReplace();
                User usermanager = (User)eHRPageServer.GetPalauObject(typeof(User));
                SECUserData userdata = usermanager.GetSelfInfor(userid);
                this.txtDescription.Text = userdata.USERDESC;
                this.txtLoginName.Text = userdata.LOGINNAME;
                this.drpChangePwd.SelectedValue = userdata.NEVERUSED.ToString();
                this.drpIsLock.SelectedValue = userdata.ISLOCK.ToString();
                this.txtStartDate.Text = userdata.STARTDATE;
                this.txtEndDate.Text = userdata.ENDDATE;
                (this.PersonSelect as PersonSelect).TextPersonId.Text = userdata.PERSONID;
                PersonAccountManager accountmanager = new PersonAccountManager();
                PersonAccountData accountdata = accountmanager.Get(userdata.PERSONID.Trim());
                (this.PersonSelect as PersonSelect).TextName.Text = accountdata.TrueName;
                ViewState["pwd"] = userdata.PASSWORD;
                ViewState["cadd"] = true;
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


        private void clearpage()
        {
            this.txtLoginName.Text = "";
            this.txtStartDate.Text = "";
            this.txtEndDate.Text = "";
            this.txtPassWord.Text = "";
            this.txtConfirmPwd.Text = "";
            this.txtDescription.Text = "";
            this.init_dropdownlist();
            (this.PersonSelect as PersonSelect).TextPersonId.Text = "";
            (this.PersonSelect as PersonSelect).TextName.Text = "";
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            User usermanager = (User)eHRPageServer.GetPalauObject(typeof(User));
            string loginname = this.txtLoginName.Text.Trim().DBReplace();
            if (usermanager.JudgeIfLoginNameExist(loginname, null))
            {
                string alex = base.getAlert("ZGAIA00071");
                base.ShowAlert(alex);
                return;
            }
            SECUserData userdata = new SECUserData();
            userdata.BUILDDATE = DataProcessor.DateTimeToShortString(DateTime.Now);
            userdata.ENDDATE = this.txtEndDate.Text.Trim().DBReplace();
            userdata.ISLOCK = Convert.ToInt32(this.drpIsLock.SelectedValue.Trim());
            userdata.LASTCHANGEDATE = DataProcessor.DateTimeToShortString(DateTime.Now);
            userdata.LASTCHANGER = Session[SystemAppConstants.SESSION_USERNAME].ToString().Trim(); ;
            userdata.LOGINNAME = this.txtLoginName.Text.Trim().DBReplace();
            userdata.NEVERUSED = Convert.ToInt32(this.drpChangePwd.SelectedValue.Trim());
            if (ViewState["cadd"] != null && (bool)ViewState["cadd"] && ViewState["pwd"].ToString().Trim() == this.txtPassWord.Text.Trim())
                userdata.PASSWORD = Hash(this.txtPassWord.Text.Trim());
            else
                userdata.PASSWORD = Hash(this.txtPassWord.Text.Trim());
            userdata.PERSONID = (this.PersonSelect as PersonSelect).TextPersonId.Text.Trim().DBReplace();
            userdata.STARTDATE = this.txtStartDate.Text.Trim().DBReplace();
            userdata.USERDESC = this.txtDescription.Text.Trim().DBReplace();
            userdata.USERID = Guid.NewGuid().ToString().Trim();
            userdata.USERTYPE = 0;
            usermanager.InsertUserData(userdata);
            base.ShowAlert(base.getString("ZGAIA00749"));
            string userid = userdata.USERID.Trim();
            //PageContext.RegisterStartupScript("<script>F('"+txtUserID.ClientID+"').setValue(" + userid + ");</script>");
            
            txtUserID.Text = userid;
            this.btnAdd.Enabled = false;

            CheerUI.PageContext.RegisterStartupScript("if(parent.sendBack){parent.sendBack('" + userid + "');}");
        }
    }
}