using CHEER.BusinessLayer.eAttendance;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.Common.AppConstants;
using CHEER.CommonLayer.eAttendance.Schema;
using CHEER.CommonLayer.eSecurity.Data;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CHEER.PresentationLayer.Security.UserAndRole
{
    public partial class RoleEditPage : CHEERBasePage
    {
        private const string RoleEditFunID = "0090000500020005";
        private string RoleID
        {
            get { return (string)ViewState["RoleID"]; }
            set { ViewState["RoleID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckPageAccess();
                Init_Lau();
                init_dropdownlist();
                get_transdata();
                load_data();
            }
        }

        private void init_dropdownlist()
        {
            (this.StdBranchLoader as StdBranchLoader).IsHaveManageUnit = true;
            (this.StdBranchLoader as StdBranchLoader).LoadBranchDtsByID(base.GetSecurityChecker(), RoleEditFunID);
            CHEER.Platform.DAL.PersistBroker b = CHEER.Platform.DAL.PersistBroker.Instance();
            DataTable dt = new DataTable();
            try
            {
                chkFormAccess.Items.Clear();
                dt = b.ExecuteSQLForDst("select PRS_ID,PRS_NAME from edfprocess ORDER BY PRS_CREATEDATETIME").Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                { 
                    chkFormAccess.Items.Add(new CheerUI.CheckItem(dt.Rows[i]["PRS_NAME"].ToString(), dt.Rows[i]["PRS_ID"].ToString()));
                }
            }
            catch
            {
            }
            finally
            {
                b.Close();
            }
        }

        private void load_data()
        {
            if (this.RoleID.Trim() != "")
            {
                string roleid = this.RoleID.Trim();
                Role rolemanager = (Role)eHRPageServer.GetPalauObject(typeof(Role));
                SECRoleData roledata = rolemanager.GetSelfInfor(roleid);
                this.txtRoleName.Text = roledata.ROLENAME.Trim();
                this.txtDescription.Text = roledata.ROLEDESC.Trim();
                string deptid = roledata.DEPTID.Trim();
                (this.StdBranchLoader as StdBranchLoader).SetSelectBranch(deptid);
                Role R = new Role();
                DataTable dt = R.GetRoleForms(roleid);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string PID = dt.Rows[i]["PROCESSID"].ToString();
                    if (chkFormAccess.Items.FindByValue(PID) != null)
                    {
                        chkFormAccess.Items.FindByValue(PID).Selected = true;
                    }
                }
            }
        }

        private void CheckPageAccess()
        {
            this.btnSave.Enabled = this.CheckFunAccess(RoleEditFunID);
        }
        private bool CheckFunAccess(string funid)
        {
            return base.GetSecurityChecker().IsAllow(funid);
        }

        private void get_transdata()
        {
            if (Request.QueryString["ROLEID"] != null)
                this.RoleID = Request.QueryString["ROLEID"].Trim();
            else
                this.RoleID = "";
        }

        protected void Init_Lau()
        {
            this.txtRoleName.Label = "角色名称";
            (this.StdBranchLoader as StdBranchLoader).Label = "归属组织";
            this.txtDescription.Label = "角色描述";
            this.btnSave.Text = "保存";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if ((this.StdBranchLoader as StdBranchLoader).GetSelectBranchItem().Value == "")
            {
                base.ShowAlert("请选择组织");
                return;
            }
            Role rolemanager = (Role)eHRPageServer.GetPalauObject(typeof(Role));
            string rolename = this.txtRoleName.Text.Trim().DBReplace();
            if (rolemanager.JudgeIfRoleNameExist(rolename, this.RoleID.Trim(), base.getBusinessUnitID()))
            {
                string alex = "角色名已被使用,请重新填写!";
                base.ShowAlert(alex);
                return;
            }
            SECRoleData roledata = rolemanager.GetSelfInfor(this.RoleID.Trim());
            CheerUI.ListItem deptitem = (this.StdBranchLoader as StdBranchLoader).GetSelectBranchItem();
            roledata.DEPTID = deptitem.Value.Trim();
            roledata.LASTCHANGEDATE = DataProcessor.DateTimeToShortString(DateTime.Now);
            roledata.LASTCHANGER = Session[SystemAppConstants.SESSION_USERNAME].ToString().Trim(); ;
            roledata.ROLEDESC = this.txtDescription.Text;
            roledata.ROLEID = this.RoleID.Trim();
            roledata.ROLENAME = this.txtRoleName.Text.Trim().DBReplace();

            if (this.RoleID.Trim() != "")
            {
                rolemanager.UpdateRoleData(roledata);
                base.ShowAlert("保存成功!");
                string userid = roledata.ROLEID.Trim();
                CheerUI.PageContext.RegisterStartupScript("parent.sendBack('" + userid + "');");
                this.btnSave.Enabled = false;
            }
            CHEER.DataAccessLayer.eSecurity.SECROLEFORMDA sDA = new CHEER.DataAccessLayer.eSecurity.SECROLEFORMDA();
            Role R = new Role();
            R.DeleteRoleForm(RoleID);

            for (int i = 0; i < chkFormAccess.Items.Count; i++)
            {
                if (chkFormAccess.Items[i].Selected)
                {
                    SECROLEFORMET set = new SECROLEFORMET();
                    set.RFID = Guid.NewGuid().ToString();
                    set.ROLEID = RoleID;
                    set.PROCESSID = chkFormAccess.Items[i].Value;
                    sDA.Insert(set);
                }
            }
        }
    }
}