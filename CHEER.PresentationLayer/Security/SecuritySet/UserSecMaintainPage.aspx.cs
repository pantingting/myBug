using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.PresentationLayer;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.CommonLayer.eSecurity.Schema;
using System.Data;
using CHEER.CommonLayer.ePersonnel.Schema;
using CHEER.Common.Schema;

namespace CHEER.PresentationLayer.Security.SecuritySet
{
    public partial class UserSecMaintainPage : CHEERBasePage
    {
        private const string SecuritySetPageID = "0090000500020005";
        private const string SecurityQueryFunID = "0090000500020005";
        private const string SecurityFieldFunID = "0090000500020005";
        private const string SecurityEditFunID = "0090000500020005";
        private string UserID
        {
            get { return (string)ViewState["UserID"]; }
            set { ViewState["UserID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.CheckPageAccess();
                InitFace();
                this.GetTransData();
                this.InitDropDownList();
                this.LoadData();
                this.InitTab();
            }
        }
        private void CheckPageAccess()
        {
            if (!GetSecurityChecker().IsAllow(SecuritySetPageID))
                ShowAlert("您没有此功能的权限！");
        }
        private bool CheckFunAccess(string funid)
        {
            return base.GetSecurityChecker().IsAllow(funid);
        }
        private void InitFace()
        {
            this.cmdReturn.Text = "返回";
            this.txtLoginName.Label = "登录名";
            this.txtDept.Label = "归属组织";
            this.txtEmployeeID.Label = "工号";
            this.txtName.Label = "姓名";
            this.drpIsLock.Label = "是否停用";
        }
        private void GetTransData()
		{
			if(Request.QueryString["USERID"] != null)
				this.UserID = Request.QueryString["USERID"].Trim();
			else
				this.UserID = "";
			this.txtUserID.Text = this.UserID;
			if(Request.QueryString["FROMURL"] != null)
				this.txtFromUrl.Text = Request.QueryString["FROMURL"].Trim();
			else
				this.txtFromUrl.Text = "";
		}
        private void InitDropDownList()
        {
            this.drpIsLock.Items.Clear();
            this.drpIsLock.Items.Add(new CheerUI.ListItem("", "-1"));
            this.drpIsLock.Items.Add(new CheerUI.ListItem("否", "0"));
            this.drpIsLock.Items.Add(new CheerUI.ListItem("是", "1"));
        }
        private void LoadData()
        {
            if (this.UserID.Trim() != "")
            {
                User usermanager = (User)eHRPageServer.GetPalauObject(typeof(User));
                string wherestr = SECUserSchema.SECUSER_TABLE + "." + SECUserSchema.USERID + "='" + this.UserID.Trim() + "'";
                DataSet userds = usermanager.GetAllUserInforByQuery(wherestr, false);
                if (userds.Tables.Count > 0)
                {
                    if (userds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = userds.Tables[0].Rows[0];
                        if (!row.IsNull(SECUserSchema.LOGINNAME))
                            this.txtLoginName.Text = row[SECUserSchema.LOGINNAME].ToString().Trim();
                        if (!row.IsNull(PSNAccountSchema.PSNACCOUNT_EMPLOYEEID))
                            this.txtEmployeeID.Text = row[PSNAccountSchema.PSNACCOUNT_EMPLOYEEID].ToString().Trim();
                        if (!row.IsNull(PSNAccountSchema.PSNACCOUNT_TRUENAME))
                            this.txtName.Text = row[PSNAccountSchema.PSNACCOUNT_TRUENAME].ToString();
                        if (!row.IsNull(SECUserSchema.ISLOCK))
                        {
                            try
                            {
                                this.drpIsLock.SelectedValue = row[SECUserSchema.ISLOCK].ToString().Trim();
                            }
                            catch
                            {
                                this.InitDropDownList();
                            }
                        }
                        if (!row.IsNull(ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME))
                            this.txtDept.Text = row[ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME].ToString().Trim();
                    }
                }
            }
        }
        private void InitTab()
        {
            string userid = this.txtUserID.Text.Trim().DBReplace();
            mainTabStrip.Tabs.Clear();
            if (CheckFunAccess("0090000500010005"))
            {
                CheerUI.Tab secsettab = new CheerUI.Tab();
                secsettab.Title = "设置";
                secsettab.IFrameUrl = "SecurityMaintainPage.aspx?MANID=" + userid + "&MANTYPE=1";
                secsettab.EnableIFrame = true;
                mainTabStrip.Tabs.Add(secsettab);
            }
            //if (CheckFunAccess(SecurityQueryFunID))
            //{
            //    CheerUI.Tab secinfortab = new CheerUI.Tab();
            //    secinfortab.Title = base.getAlert("ZGAIA00190");
            //    secinfortab.IFrameUrl = "SecurityInforPage.aspx?MANID=" + userid + "&MANTYPE=1";
            //    secinfortab.EnableIFrame = true;
            //    mainTabStrip.Tabs.Add(secinfortab);
            //}
            //if (CheckFunAccess(SecurityFieldFunID))
            //{
            //    CheerUI.Tab secfieldtab = new CheerUI.Tab();
            //    secfieldtab.Title = "字段维护";
            //    secfieldtab.IFrameUrl = "SecFieldMaintainPage.aspx?MANID=" + userid + "&MANTYPE=1";
            //    secfieldtab.EnableIFrame = true;
            //    mainTabStrip.Tabs.Add(secfieldtab);
            //}
            if (userid == "")
                this.mainTabStrip.Enabled = false;
        }
    }
}