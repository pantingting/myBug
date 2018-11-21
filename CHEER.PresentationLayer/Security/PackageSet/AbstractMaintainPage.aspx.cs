using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.CommonLayer.eSecurity;
using CHEER.CommonLayer.eSecurity.Schema;
using CHEER.PresentationLayer;

namespace CHEER.PresentationLayer.Security.PackageSet
{
    public partial class AbstractMaintainPage : CHEERBasePage
    {
        private string PackageID
        {
            get { return (string)ViewState["PackageID"]; }
            set { ViewState["PackageID"] = value; }
        }
        private string SelfDeptID
        {
            get { return (string)ViewState["SelfDeptID"]; }
            set { ViewState["SelfDeptID"] = value; }
        }
        private string ManagerDeptID
        {
            get { return (string)ViewState["ManagerDeptID"]; }
            set { ViewState["ManagerDeptID"] = value; }
        }
        private string HRDeptID
        {
            get { return (string)ViewState["HRDeptID"]; }
            set { ViewState["HRDeptID"] = value; }
        }
        private string EmpSelfID
        {
            get { return (string)ViewState["EmpSelfID"]; }
            set { ViewState["EmpSelfID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.CheckAccess();
            if (!IsPostBack)
            {
                this.SelfDeptID = "";
                this.ManagerDeptID = "";
                this.HRDeptID = "";
                this.EmpSelfID = "";
                this.InitFace();
                this.GetTransData();
                this.LoadData();
            }
        }
        private void CheckAccess()
        {
            base.CheckIsLoginOut();
        }
        private void InitFace()
        {
            chkSelf.Label = "所在组织";
            chkSelfSub.Label = "所在组织及子组织";
            chkManager.Label = "担任主管组织";
            chkManagerSub.Label = "担任主管组织及子组织";
            chkHR.Label = "担任管理组织";
            chkHRSub.Label = "担任管理组织及子组织";
            chkEmpSelf.Label = "员工本人";
            chkEmpSelfExcept.Label = "除员工本人";
            this.cmdSave.Text = "保存";
        }
        private void GetTransData()
        {
            if (Request.QueryString["PACKAGEID"] != null)
                this.PackageID = Request.QueryString["PACKAGEID"].Trim();
            else
                this.PackageID = "";
        }
        private void LoadData()
        {
            if (this.PackageID.Trim() != "")
            {
                SecurityDimensionItemLoader itemmanager = (SecurityDimensionItemLoader)eHRPageServer.GetPalauObject(typeof(SecurityDimensionItemLoader));
                DataSet absds = itemmanager.GetAbstractInfor(this.PackageID.Trim());
                if (absds.Tables.Count > 0)
                {
                    if (absds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in absds.Tables[0].Rows)
                        {
                            string absvalue = row[SECDimensionItemSchema.DIMITEMVALUE].ToString().Trim();
                            string absitemid = row[SECDimensionItemSchema.DIMITEMID].ToString().Trim();
                            if (absvalue.Trim() != "")
                                this.CheckTheBox(Convert.ToInt32(absvalue), absitemid);
                        }
                    }
                }
            }
        }

        private void CheckTheBox(int absvalue, string absitemid)
        {
            switch (absvalue)
            {
                case (int)AbstractDimensionType.OwnHROrg:
                    this.chkHR.Checked = true;
                    this.chkHRSub.Checked = false;
                    this.HRDeptID = absitemid;
                    break;
                case (int)AbstractDimensionType.OwnHROrgAndSubOrg:
                    this.chkHR.Checked = false;
                    this.chkHRSub.Checked = true;
                    this.HRDeptID = absitemid;
                    break;
                case (int)AbstractDimensionType.OwnManageOrg:
                    this.chkManager.Checked = true;
                    this.chkManagerSub.Checked = false;
                    this.ManagerDeptID = absitemid;
                    break;
                case (int)AbstractDimensionType.OwnManageOrgAndSubOrg:
                    this.chkManager.Checked = false;
                    this.chkManagerSub.Checked = true;
                    this.ManagerDeptID = absitemid;
                    break;
                case (int)AbstractDimensionType.OwnOrg:
                    this.chkSelf.Checked = true;
                    this.chkSelfSub.Checked = false;
                    this.SelfDeptID = absitemid;
                    break;
                case (int)AbstractDimensionType.OwnOrgAndSubOrg:
                    this.chkSelf.Checked = false;
                    this.chkSelfSub.Checked = true;
                    this.SelfDeptID = absitemid;
                    break;
                case (int)AbstractDimensionType.OwnSelf:
                    this.chkEmpSelf.Checked = true;
                    this.chkEmpSelfExcept.Checked = false;
                    this.EmpSelfID = absitemid;
                    break;
                case (int)AbstractDimensionType.ExceptSelf:
                    this.chkEmpSelfExcept.Checked = true;
                    this.chkEmpSelf.Checked = false;
                    this.EmpSelfID = absitemid;
                    break;
                default:
                    return;
            }
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            Hashtable abstracthash = new Hashtable();
            string selfvalue = "";
            if (this.chkSelf.Checked)
                selfvalue = ((int)AbstractDimensionType.OwnOrg).ToString().Trim();
            if (this.chkSelfSub.Checked)
                selfvalue = ((int)AbstractDimensionType.OwnOrgAndSubOrg).ToString().Trim();
            if (this.SelfDeptID.Trim() == "" && selfvalue.Trim() != "")
                this.SelfDeptID = Guid.NewGuid().ToString().Trim();
            if (this.SelfDeptID.Trim() != "")
                abstracthash.Add(this.SelfDeptID.Trim(), selfvalue.Trim());
            string managervalue = "";
            if (this.chkManager.Checked)
                managervalue = ((int)AbstractDimensionType.OwnManageOrg).ToString().Trim();
            if (this.chkManagerSub.Checked)
                managervalue = ((int)AbstractDimensionType.OwnManageOrgAndSubOrg).ToString().Trim();
            if (this.ManagerDeptID.Trim() == "" && managervalue.Trim() != "")
                this.ManagerDeptID = Guid.NewGuid().ToString().Trim();
            if (this.ManagerDeptID.Trim() != "")
                abstracthash.Add(this.ManagerDeptID.Trim(), managervalue.Trim());

            string hrvalue = "";
            if (this.chkHR.Checked)
                hrvalue = ((int)AbstractDimensionType.OwnHROrg).ToString().Trim();
            if (this.chkHRSub.Checked)
                hrvalue = ((int)AbstractDimensionType.OwnHROrgAndSubOrg).ToString().Trim();
            if (this.HRDeptID.Trim() == "" && hrvalue.Trim() != "")
                this.HRDeptID = Guid.NewGuid().ToString().Trim();
            if (this.HRDeptID.Trim() != "")
                abstracthash.Add(this.HRDeptID.Trim(), hrvalue.Trim());

            string empself = "";
            if (this.chkEmpSelf.Checked)
                empself = ((int)AbstractDimensionType.OwnSelf).ToString().Trim();
            if (this.chkEmpSelfExcept.Checked)
                empself = ((int)AbstractDimensionType.ExceptSelf).ToString().Trim();

            if (this.EmpSelfID.Trim() == "" && empself.Trim() != "")
                this.EmpSelfID = Guid.NewGuid().ToString().Trim();
            if (this.EmpSelfID.Trim() != "")
                abstracthash.Add(this.EmpSelfID.Trim(), empself.Trim());

            SecurityDimensionItemLoader itemmanager = (SecurityDimensionItemLoader)eHRPageServer.GetPalauObject(typeof(SecurityDimensionItemLoader));
            itemmanager.SaveAbstractData(abstracthash, this.PackageID.Trim());
            base.ShowAlert("保存" + "成功!");
        }

        protected void chkSelf_CheckedChanged(object sender, CheerUI.CheckedEventArgs e)
        {
            checkchange(chkSelf, chkSelfSub);
        }

        protected void chkSelfSub_CheckedChanged(object sender, CheerUI.CheckedEventArgs e)
        {
            checkchange(chkSelfSub, chkSelf);
        }

        protected void chkManager_CheckedChanged(object sender, CheerUI.CheckedEventArgs e)
        {
            checkchange(chkManager, chkManagerSub);
        }

        protected void chkManagerSub_CheckedChanged(object sender, CheerUI.CheckedEventArgs e)
        {
            checkchange(chkManagerSub, chkManager);
        }

        protected void chkHR_CheckedChanged(object sender, CheerUI.CheckedEventArgs e)
        {
            checkchange(chkHR, chkHRSub);
        }

        protected void chkHRSub_CheckedChanged(object sender, CheerUI.CheckedEventArgs e)
        {
            checkchange(chkHRSub, chkHR);
        }

        protected void chkEmpSelf_CheckedChanged(object sender, CheerUI.CheckedEventArgs e)
        {
            checkchange(chkEmpSelf, chkEmpSelfExcept);
        }

        protected void chkEmpSelfExcept_CheckedChanged(object sender, CheerUI.CheckedEventArgs e)
        {
            checkchange(chkEmpSelfExcept, chkEmpSelf);
        }

        private void checkchange(CheerUI.CheckBox checkedbox, CheerUI.CheckBox nocheckbox)
        {
            if (nocheckbox.Checked)
                nocheckbox.Checked = false;
        }

    }
}