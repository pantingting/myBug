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
using CHEER.Common.Schema;
using CHEER.CommonLayer.eSecurity.Schema;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;

namespace CHEER.PresentationLayer.Security.UserAndRole
{
    public partial class UserRolePage : CHEERBasePage
    {
        private string UserID
        {
            get { return (string)ViewState["UserID"]; }
            set { ViewState["UserID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitFace();
                get_transdata();
                load_data();
            }
        }


        private void InitFace()
        {
            btnAdd.Text = "新增";
            btnDelete.Text = "删除";
        }

        private void get_transdata()
        {
            if (Request.QueryString["USERID"] != null)
                this.UserID = Request.QueryString["USERID"].Trim();
            else
                this.UserID = "";
            this.txtUserID.Text = this.UserID.Trim();
        }

        private void init_grid()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {  new DataColumn(SECRoleSchema.ROLEID,typeof(string)),
													new DataColumn(SECRoleSchema.ROLENAME,typeof(string)),
													new DataColumn(SECRoleSchema.DEPTID,typeof(string)),
													new DataColumn(SECRoleSchema.ROLEDESC,typeof(string)),
													 new DataColumn(ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE,typeof(string)),
													new DataColumn(ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME,typeof(string)),
			});
            this.UlRoleGrid.DataSource = dt;
            this.UlRoleGrid.DataBind();
        }

        private void load_data()
        {
            if (this.UserID.Trim() != "")
            {
                string userid = this.UserID.Trim();
                UserRoleMapManager mapmanager = (UserRoleMapManager)eHRPageServer.GetPalauObject(typeof(UserRoleMapManager));
                DataSet mapds = mapmanager.GetRoleInforByUserID(userid);
                if (mapds.Tables[0].Rows.Count > 0)
                {
                    mapds.Tables[0].DefaultView.Sort = SECRoleSchema.ROLENAME + " ASC";
                    this.UlRoleGrid.DataSource = mapds.Tables[0].DefaultView;
                    this.UlRoleGrid.DataBind();
                }
                else
                    this.init_grid();
            }
            else
                this.init_grid();
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            InitGrid();
        }

        protected void InitGrid()
        {
            CommonMethod.AddFlexField(this.UlRoleGrid, "角色名", SECRoleSchema.ROLENAME, 15, false);
            CommonMethod.AddFlexField(this.UlRoleGrid, "", SECRoleSchema.DEPTID, 0, true);
            CommonMethod.AddFlexField(this.UlRoleGrid, "", SECRoleSchema.ROLEID, 0, true);
            CommonMethod.AddFlexField(this.UlRoleGrid, "组织代码", ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE, 15, false);
            CommonMethod.AddFlexField(this.UlRoleGrid, "归属组织", ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME, 15, false);
            CommonMethod.AddFlexField(this.UlRoleGrid, "角色描述", SECRoleSchema.ROLEDESC, 15, false);
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            this.load_data();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ArrayList delidlist = new ArrayList();
            int[] rows = this.UlRoleGrid.SelectedRowIndexArray;
            foreach (int row in rows)
            {
                string roleid = this.UlRoleGrid.Rows[row].Cells.FromKey(SECRoleSchema.ROLEID).ToString().Trim();
                delidlist.Add(roleid);
            }
            if (delidlist.Count > 0)
            {
                UserRoleMapManager mapmanager = (UserRoleMapManager)eHRPageServer.GetPalauObject(typeof(UserRoleMapManager));
                mapmanager.DeleteRelationList(delidlist, this.UserID.Trim(), 1);
                this.load_data();
                base.ShowAlert("删除成功!");
            }
            else {
                base.ShowAlert("请选择需要删除的数据");
            }
        }

        protected void RoleSelectPage_Close(object sender, CheerUI.WindowCloseEventArgs e)
        {
           // CheerUI.PageContext.RegisterStartupScript("Refresh()");
            this.load_data();
        }
    }
}