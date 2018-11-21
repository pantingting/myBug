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
    public partial class SecFieldMaintainPage : CHEERBasePage
    {
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
                this.LoadTree();
                this.InitGrid();
            }
        }
        private void GetTransData()
        {
            if (Request.QueryString["MANID"] != null)
                this.ManID = Request.QueryString["MANID"].Trim();
            else
                this.ManID = "";
            if (Request.QueryString["MANTYPE"] != null)
                this.ManType = Request.QueryString["MANTYPE"].Trim();
            else
                this.ManType = "";
        }

        private void Init_Grid()
        {
            this.UlFieldSetGrid.DataSource = new DataTable();
            this.UlFieldSetGrid.DataBind();
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            InitGrid();
        }
        protected void InitGrid()
        {
            Language LangInfo = base.GetLanguageInfo();
            CommonMethod.AddFlexField(this.UlFieldSetGrid, "", "MENUID", 0, true);
            if (LangInfo == Language.US)
            {
                CommonMethod.AddFlexField(this.UlFieldSetGrid, "菜单名称", "MENUENGNAME", 15, false);
                CommonMethod.AddFlexField(this.UlFieldSetGrid, "菜单名称", "MENUNAME", 15, true);
            }
            else
            {
                CommonMethod.AddFlexField(this.UlFieldSetGrid, "菜单名称", "MENUNAME", 15, false);
                CommonMethod.AddFlexField(this.UlFieldSetGrid, "菜单名称", "MENUENGNAME", 15, true);
            }

            CommonMethod.AddFlexField(this.UlFieldSetGrid, "", "FUNCTIONID", 0, true);
            if (LangInfo == Language.US)
            {
                CommonMethod.AddFlexField(this.UlFieldSetGrid, "功能点名称", "FUNCTIONNAME", 15, true);
                CommonMethod.AddFlexField(this.UlFieldSetGrid, "功能点名称", "FUNCTIONENGNAME", 15, false);
            }
            else
            {
                CommonMethod.AddFlexField(this.UlFieldSetGrid, "功能点名称", "FUNCTIONNAME", 15, false);
                CommonMethod.AddFlexField(this.UlFieldSetGrid, "功能点名称", "FUNCTIONENGNAME", 15, true);
            }
            CommonMethod.AddFlexField(this.UlFieldSetGrid, "字段维护", "FIELD", 0, false);
            CommonMethod.AddFlexField(this.UlFieldSetGrid, "", "FUNCTIONORDER", 0, true);
            CommonMethod.AddFlexField(this.UlFieldSetGrid, "", "MENULABEL", 0, true);
        }

        private void LoadTree()
        {
            this.UlMenuTree.Nodes.Clear();
            CheerUI.TreeNode rootnode = new CheerUI.TreeNode();
            rootnode.Text = "模块列表";
            rootnode.Tag = "000000";
            rootnode.Expanded = true;
            Language LangInfo = base.GetLanguageInfo();
            FunctionModule modulemanager = (FunctionModule)eHRPageServer.GetPalauObject(typeof(FunctionModule));
            DataSet moduleds = modulemanager.GetAllModuleInfor();
            if (moduleds.Tables.Count > 0)
            {
                if (moduleds.Tables[0].Rows.Count > 0)
                {
                    DataView dv = moduleds.Tables[0].DefaultView;
                    dv.Sort = SECFunModuleSchema.LABEL + " ASC";
                    for (int i = 0; i < dv.Count; i++)
                    {
                        string menuid = dv[i][SECFunModuleSchema.MENUID].ToString().Trim();
                        int modulelevel = 1;
                        if (!dv[i].Row.IsNull(SECFunModuleSchema.ONLEVEL) && dv[i][SECFunModuleSchema.ONLEVEL].ToString().Trim() != "")
                            modulelevel = Convert.ToInt32(dv[i][SECFunModuleSchema.ONLEVEL].ToString().Trim());

                        string modulename = "";
                        if (LangInfo == Language.US)
                        {
                            modulename = dv[i][SECFunModuleSchema.MENUENGLISHNAME].ToString();
                        }
                        else if (LangInfo == Language.TW)
                        {
                            modulename = dv[i][SECFunModuleSchema.MENUNAME].ToString();
                        }
                        else
                        {
                            modulename = dv[i][SECFunModuleSchema.MENUNAME].ToString();
                        }
                        string modulelabel = dv[i][SECFunModuleSchema.LABEL].ToString().Trim();
                        CheerUI.TreeNode modulenode = new CheerUI.TreeNode();
                        modulenode.Text = modulename;
                        modulenode.Tag = menuid;
                        modulenode.EnableClickEvent=true;
                        modulenode.NodeID = modulelabel;
                        rootnode.Nodes.Add(modulenode);
                    }
                }
            }
            this.UlMenuTree.Nodes.Add(rootnode);
        }

        private void LoadGrid(string moduleid)
        {
            FieldManager mapmanager = (FieldManager)eHRPageServer.GetPalauObject(typeof(FieldManager));
            DataSet secds = mapmanager.GetModuleFieldFunInforOfMan(moduleid);
            if (secds.Tables.Count > 0)
            {
                this.UlFieldSetGrid.DataSource = secds.Tables[0].DefaultView;
                this.UlFieldSetGrid.DataBind();
            }
            else
                this.Init_Grid();
        }

        protected void UlMenuTree_NodeCommand(object sender, CheerUI.TreeCommandEventArgs e)
        {
            if (e.Node == null)
                return;
            string moduleid = e.Node.Tag.ToString().Trim();
            this.LoadGrid(moduleid);
           
        }
    }
}