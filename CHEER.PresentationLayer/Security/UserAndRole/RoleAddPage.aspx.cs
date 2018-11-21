using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.Common.AppConstants;
using CHEER.CommonLayer.eSecurity.Data;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CHEER.PresentationLayer.Security.UserAndRole
{
    public partial class RoleAddPage : CHEERBasePage
    {
        private const string RoleAddFunID = "0090000500020001";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckPageAccess();
                Init_Lau();
                Init_DropDownList();
            }
        }

        private void CheckPageAccess()
        {
            if (!GetSecurityChecker().IsAllow(RoleAddFunID))
                ShowAlert(getAlert("ZGAIA00809"));
            this.btnSave.Enabled = this.CheckFunAccess(RoleAddFunID);
        }
        private bool CheckFunAccess(string funid)
        {
            return base.GetSecurityChecker().IsAllow(funid);
        }

        private void Init_DropDownList()
        {
            (this.StdBranchLoader as StdBranchLoader).IsHaveManageUnit = true;
            (this.StdBranchLoader as StdBranchLoader).LoadBranchDtsByID(base.GetSecurityChecker(), RoleAddFunID);
        }

        void Init_Lau()
        {
            this.txtRoleName.Label = base.getString("ZGAIA00237");
            (this.StdBranchLoader as StdBranchLoader).Label = base.getString("ZGAIA00240");
            this.txtDescription.Label = base.getString("ZGAIA00282");
            this.btnSave.Text = base.getString("ZGAIA00195");
        }

        private void LoadData()
        {
            if (this.txtRoleID.Text.Trim() != "")
            {
                string roleid = this.txtRoleID.Text.Trim().DBReplace();
                Role rolemanager = (Role)eHRPageServer.GetPalauObject(typeof(Role));
                SECRoleData roledata = rolemanager.GetSelfInfor(roleid);
                this.txtRoleName.Text = roledata.ROLENAME.Trim();
                this.txtDescription.Text = roledata.ROLEDESC.Trim();
                string deptid = roledata.DEPTID.Trim();
                (this.StdBranchLoader as StdBranchLoader).SetSelectBranch(deptid);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if ((this.StdBranchLoader as StdBranchLoader).GetSelectBranchItem().Value == "")
            {
                base.ShowAlert(base.getAlert("ZGAIA00104"));
                return;
            }
            Role rolemanager = (Role)eHRPageServer.GetPalauObject(typeof(Role));
            string rolename = this.txtRoleName.Text.Trim().DBReplace();
            if (rolemanager.JudgeIfRoleNameExist(rolename, null, base.getBusinessUnitID()))
            {
                string alex = base.getAlert("ZGAIA00102");
                base.ShowAlert(alex);
                return;
            }
            SECRoleData roledata = new SECRoleData();
            CheerUI.ListItem deptitem = (this.StdBranchLoader as StdBranchLoader).GetSelectBranchItem();
            roledata.DEPTID = deptitem.Value.Trim();
            roledata.LASTCHANGEDATE = DataProcessor.DateTimeToShortString(DateTime.Now);
            roledata.LASTCHANGER = Session[SystemAppConstants.SESSION_USERNAME].ToString().Trim(); ;
            roledata.ROLEDESC = this.txtDescription.Text;
            roledata.ROLEID = Guid.NewGuid().ToString().Trim();
            roledata.ROLENAME = this.txtRoleName.Text.Trim().DBReplace();
            rolemanager.InsertRoleData(roledata);
            base.ShowAlert(base.getString("ZGAIA00749"));
            string userid = roledata.ROLEID.Trim();
            CheerUI.PageContext.RegisterStartupScript("parent.sendBack('" + userid + "');");
            this.btnSave.Enabled = false;
        }
    }
}