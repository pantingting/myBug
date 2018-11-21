using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.CommonLayer.eSecurity.Data;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.Controls;

namespace CHEER.PresentationLayer.Security.SuperWork
{
    public partial class RoleCopyPage : CHEERBasePage
    {
        private const string AllotFunID = "0090000500020005";
        string RoleID
        {
            get { return ViewState["RoleID"].ToString(); }
            set { ViewState["RoleID"] = value; }
        }
        string RoleName
        {
            get { return ViewState["RoleName"].ToString(); }
            set { ViewState["RoleName"] = value; }
        }
        string DeptID
        {
            get { return ViewState["DeptID"].ToString(); }
            set { ViewState["DeptID"] = value; }
        }
        string DeptName
        {
            get { return ViewState["DeptName"].ToString(); }
            set { ViewState["DeptName"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitFace();
                CheckPageAccess();
                RoleID = Request.QueryString["ROLEID"];
                if (!GetData(RoleID))
                {
                    base.ShowAlert("获取角色数据出错，原因：该角色所属组织不存在！");
                    return;
                }
                init_dropdownlist();
            }
        }
        private void InitFace()
        {
            this.gp1.Title = "源角色信息";
            this.txtName.Label = "源角色名称";
            this.txtDeptName.Label = "源角色组织";
            this.gp2.Title = "命名规则";
            this.Label6.Text = "字符替换";
            this.Label11.Text = "(说明：要替换的字符)";
            this.Label12.Text = "(说明：替换成的字符)";
            this.Label7.Text = "字符插入";
            this.Label13.Text = "起始位置：";
            this.Label14.Text = "(说明：插入字符的开始位置)";
            this.Label15.Text = "(说明：要插入的字符)";
            this.Label4.Text = "名称前缀";
            this.Label17.Text = "(说明：角色名称前缀)";
            this.Label5.Text = "名称后缀";
            this.Label18.Text = "(说明：角色名称后缀)";
            this.gp3.Title = "复制目标";
            (StdBranchLoader as StdBranchLoader).Label = "目的组织";
            this.ckPack.Label = "是否复制功能权限";
            this.ckPack.Text = "是";
            this.ckSec.Label = "是否复制数据/字段权限";
            this.ckSec.Text = "是";
            this.cmdAdd.Text = "复制";
        }
        bool GetData(string roleID)
        {
            RoleCopy loader = (RoleCopy)eHRPageServer.GetPalauObject(typeof(RoleCopy));
            DataTable dt = loader.LoadRole(roleID);
            if (dt.Rows.Count > 0)
            {
                this.RoleName = dt.Rows[0]["ROLENAME"].ToString();
                this.DeptID = dt.Rows[0]["UNITID"].ToString();
                this.DeptName = dt.Rows[0]["UNITNAME"].ToString();
                this.txtName.Text = this.RoleName;
                this.txtDeptName.Text = this.DeptName;
                return true;
            }
            else
            {
                return false;
            }
        }
        private void CheckPageAccess()
        {
            if (!GetSecurityChecker().IsAllow(AllotFunID))
                ShowAlert("您没有此功能的权限！");
        }
        private void init_dropdownlist()
        {
            (this.StdBranchLoader as StdBranchLoader).IsHaveManageUnit = true;
            (this.StdBranchLoader as StdBranchLoader).LoadBranchDtsByID(base.GetSecurityChecker(), AllotFunID);
        }

        bool constructnewname(out string newname, out string ermsg)
        {
            string rolename = txtName.Text.Trim().DBReplace();
            string repOLD = this.txtRepOID.Text.Trim().DBReplace();
            string repNew = this.txtRepNew.Text.Trim().DBReplace();
            if (repOLD != "")
                rolename = rolename.Replace(repOLD, repNew);
            if (this.numIndex.Text.Trim() != "" && this.txtInsertText.Text != "")
            {
                try
                {
                    int insertPosi = int.Parse(numIndex.Text.Trim());
                    if (insertPosi < rolename.Length)
                        rolename = rolename.Insert(insertPosi, this.txtInsertText.Text);
                    else
                        rolename += this.txtInsertText.Text.Trim().DBReplace();
                }
                catch
                {
                    ermsg = "起始位置必须是数字，请修改！";
                    newname = "";
                    return false;
                }
            }
            rolename = this.txtBegin.Text.Trim() + rolename + this.txtEnd.Text.Trim().DBReplace();
            if (rolename.Length > 50)
            {
                ermsg = "角色名称过长(超过50个字符)，请修改！";
                newname = "";
                return false;
            }
            else
            {
                ermsg = "";
                newname = rolename;
                return true;
            }
        }
        SECRoleData getroledata(string rolename)
        {
            SECRoleData obj = new SECRoleData();
            obj.DEPTID = (this.StdBranchLoader as StdBranchLoader).GetSelectBranchItem().Value;
            obj.ROLEID = IDGenerator.GenIDString();
            obj.ROLENAME = rolename;
            obj.LASTCHANGEDATE = DataProcessor.DateTimeToShortString(DateTime.Now);
            obj.LASTCHANGER = Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERNAME].ToString();
            return obj;
        }

        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            string strdept = (this.StdBranchLoader as StdBranchLoader).GetSelectBranchItem().Value;
            if (strdept == "")
            {
                base.ShowAlert("目的组织不能为空！");
                return;
            }
            string newname, ermsg;
            if (!constructnewname(out newname, out ermsg))
            {
                base.ShowAlert(ermsg);
                return;
            }
            CHEER.BusinessLayer.Organize.ManageUnit.ManageUnitManager MUMger = (CHEER.BusinessLayer.Organize.ManageUnit.ManageUnitManager)eHRPageServer.GetPalauObject(typeof(CHEER.BusinessLayer.Organize.ManageUnit.ManageUnitManager));
            string manageUnitID = MUMger.GetManageUnit(strdept);
            RoleCopy loader = (RoleCopy)eHRPageServer.GetPalauObject(typeof(RoleCopy));
            if (!loader.RoleCopyExecute(this.RoleID, getroledata(newname), manageUnitID, this.ckPack.Checked, this.ckSec.Checked))
            {
                base.ShowAlert(loader.BLError);
            }
            else
            {
                base.ShowAlert("复制成功！");
                this.lblmsg.Text = "复制角色：[" + this.RoleName + "]到组织：[" + (this.StdBranchLoader as StdBranchLoader).GetSelectBranchItem().Text +
                    "]下成功，新角色名称为：[" + newname + "]";
            }
        }

        protected void ckPack_CheckedChanged(object sender, CheerUI.CheckedEventArgs e)
        {
            if (ckSec.Checked)
                ckSec.Checked = false;
        }

        protected void ckSec_CheckedChanged(object sender, CheerUI.CheckedEventArgs e)
        {
            if (ckPack.Checked)
                ckPack.Checked = false;
        }
    }
}