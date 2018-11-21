using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.CommonLayer.eSecurity.Schema;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;

namespace CHEER.PresentationLayer.Security.SecuritySet
{
    public partial class SecModuleInforPage : CHEERBasePage
    {
        private string ModuleID
        {
            get { return (string)ViewState["ModuleID"]; }
            set { ViewState["ModuleID"] = value; }
        }
        private string ManID
        {
            get { return (string)ViewState["ManID"]; }
            set { ViewState["ManID"] = value; }
        }
        private string ManType
        {
            get { return (string)ViewState["ManType"]; }
            set { ViewState["ManType"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.GetTransData();
                this.LoadGrid();
            }
        }
        private void GetTransData()
        {
            if (Request.QueryString["MODULEID"] != null)
                this.ModuleID = Request.QueryString["MODULEID"].Trim();
            else
                this.ModuleID = "";
            if (Request.QueryString["MANID"] != null)
                this.ManID = Request.QueryString["MANID"].Trim();
            else
                this.ManID = "";
            this.txtManID.Text = this.ManID.Trim();
            if (Request.QueryString["MANTYPE"] != null)
                this.ManType = Request.QueryString["MANTYPE"].Trim();
            else
                this.ManType = "";
            this.txtManType.Text = this.ManType.Trim();
        }

        private void Init_Grid()
        {
            this.UlFieldSetGrid.DataSource = new DataTable();
            this.UlFieldSetGrid.DataBind();
        }

        private void LoadGrid()
        {
            int inttype = 0;
            if (this.ManType.Trim() != "")
                inttype = Convert.ToInt32(this.ManType.Trim());
            SecurityPackageMap mapmanager = (SecurityPackageMap)eHRPageServer.GetPalauObject(typeof(SecurityPackageMap));
            DataSet secds = mapmanager.GetSecFunctionofMan(this.ManID.Trim(), inttype, this.ModuleID.Trim());
            if (secds.Tables.Count > 0)
            {
                this.UlFieldSetGrid.DataSource = secds.Tables[0].DefaultView;
                this.UlFieldSetGrid.DataBind();
            }
            else
                this.Init_Grid();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            InitGrid();
        }

        protected void InitGrid()
        {
            Language LangInfo = base.GetLanguageInfo();
            CommonMethod.AddFlexField(this.UlFieldSetGrid, "", SECFunModuleSchema.MENUID, 0, true);
            if (LangInfo == Language.US)
            {
                CommonMethod.AddFlexField(this.UlFieldSetGrid, "菜单名称", SECFunModuleSchema.MENUENGLISHNAME, 15, false);
                CommonMethod.AddFlexField(this.UlFieldSetGrid, "菜单名称", SECFunModuleSchema.MENUNAME, 0, true);
            }
            else
            {
                CommonMethod.AddFlexField(this.UlFieldSetGrid, "菜单名称", SECFunModuleSchema.MENUNAME, 15, false);
                CommonMethod.AddFlexField(this.UlFieldSetGrid, "菜单名称", SECFunModuleSchema.MENUENGLISHNAME, 0, true);
            }
            CommonMethod.AddFlexField(this.UlFieldSetGrid, "", "FUNCTIONID", 0, true);
            if (LangInfo == Language.US)
            {
                CommonMethod.AddFlexField(this.UlFieldSetGrid, "功能点名称", "FUNCTIONENGNAME", 15, false);
                CommonMethod.AddFlexField(this.UlFieldSetGrid, "功能点名称", "FUNCTIONNAME", 0, true);
            }
            else
            {
                CommonMethod.AddFlexField(this.UlFieldSetGrid, "功能点名称", "FUNCTIONNAME", 15, false);
                CommonMethod.AddFlexField(this.UlFieldSetGrid, "功能点名称", "FUNCTIONENGNAME", 0, true);
            }

            //CommonMethod.AddWindowField(this.UlFieldSetGrid, "字段信息", "FIELD", 60, "", "", "", "");
            //CommonMethod.AddWindowField(this.UlFieldSetGrid, "权限信息", "SECURITY", 60, "", "", "", "");
            CommonMethod.AddLinkButtonField(this.UlFieldSetGrid, "字段信息", "", 60, false, false, "", CheerUI.IconFont.Edit).CommandName = "FIELD";
            CommonMethod.AddLinkButtonField(this.UlFieldSetGrid, "权限信息", "", 60, false, false, "", CheerUI.IconFont.Edit).CommandName = "SECURITY";
            CommonMethod.AddFlexField(this.UlFieldSetGrid, "", SECFunModuleSchema.LABEL, 0, true);
            CommonMethod.AddFlexField(this.UlFieldSetGrid, "", "FUNCTIONORDER", 0, true);
        }

        protected void UlFieldSetGrid_RowCommand(object sender, CheerUI.GridCommandEventArgs e)
        {
            string funid = UlFieldSetGrid.Rows[e.RowIndex].Cells.FromKey("FUNCTIONID").ToString();
            string menuid = "";
            if (UlFieldSetGrid.Rows[e.RowIndex].Cells.FromKey("MENUID") != null)
                menuid = UlFieldSetGrid.Rows[e.RowIndex].Cells.FromKey("MENUID").ToString();

            if (e.CommandName == "FIELD")
            {
                CheerUI.PageContext.RegisterStartupScript("showField('" + funid + "','" + menuid + "')");
            }
            if (e.CommandName == "SECURITY")
            {
                CheerUI.PageContext.RegisterStartupScript("showSecurity('" + funid + "','" + menuid + "')");
            }
        }

    }
}