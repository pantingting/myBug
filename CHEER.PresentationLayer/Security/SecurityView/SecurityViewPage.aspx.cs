using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.CommonLayer.ePersonnel;
using CHEER.CommonLayer.eSecurity.Schema;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;
using CHEER.BusinessLayer.Security.Portal;
using CHEER.PresentationLayer.CommonUse;


namespace CHEER.PresentationLayer.Security.SecurityView
{
    public partial class SecurityViewPage : CHEERBasePage
    {
        private const string SecurityViewPageID = "010020060";
        private const string SecurityViewExcelExportID = SecurityViewPageID + "010";
        private const string SecurityViewDetailID = SecurityViewPageID + "020";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckPageAccess();
                this.txtMenuIDList.Text = "";
                this.InitFace();
                InitializeTree();
                btnExport.OnClientClick = "return SelectCheck('" + "请先选择功能项！" + "')";
                btnView.OnClientClick = "return SelectCheck2('" + "请先选择功能项！" + "')";
            }
            
        }
        private void InitFace()
        {
         
            string noselect = "请先选择功能项！";
            //btnExport.Attributes.Add("onclick", "return SelectCheck('" + noselect + "')");
            btnExport.Text = "Excel导出";
         
            this.btnView.Text = "权限查看";

            //this.btnView.Attributes.Add("onclick", "return SelectCheck('" + noselect + "')");
        }

        private void CheckPageAccess()
        {
            if (!GetSecurityChecker().IsAllow(SecurityViewPageID))
                ShowAlert("您没有此功能的权限！");
            this.btnExport.Enabled = this.CheckFunAccess(SecurityViewExcelExportID);
            this.btnView.Enabled = this.CheckFunAccess(SecurityViewDetailID);
        }
        private bool CheckFunAccess(string funid)
        {
            return base.GetSecurityChecker().IsAllow(funid);
        }
    
        private void InitializeTree()
        {
            this.secviewTree.Nodes.Clear();
            Language LangInfo = base.GetLanguageInfo();

            CheerUI.TreeNode _rootNode = new CheerUI.TreeNode();
            
            _rootNode.Expanded = true;
            _rootNode.Text = "模块列表";
            _rootNode.Selectable = true;//TODO:GetSecurityChecker().IsAllow(MF_MenuID);
            _rootNode.NodeID = "000000";
            //_rootNode.OnClientClick = "F.wnd.updateIFrameNode(F('" + tabUnitManagerMaintainPage.ClientID + "'),'../StdOrg/UnitManagerMaintainPage.aspx?UnitID=" + _rootNode.NodeID + "');F.wnd.updateIFrameNode(F('" + tabOrgMulPage.ClientID + "'),'../StdOrg/OrgMulPage.aspx?UnitID=" + _rootNode.NodeID + "');";
            _rootNode.EnableClickEvent = false;
            
            FunctionModule modulemanager = (FunctionModule)eHRPageServer.GetPalauObject(typeof(FunctionModule));
            DataSet moduleds = modulemanager.GetAllModuleInfor();

            Hashtable htNodes = new Hashtable();
 
            if (moduleds.Tables.Count > 0)
            {
                if (moduleds.Tables[0].Rows.Count > 0)
                {
                    DataView dv = moduleds.Tables[0].DefaultView;
                    dv.Sort = SECFunModuleSchema.LABEL + " ASC";
                    for (int i = 0; i < dv.Count; i++)
                    {
                        string menuid = dv[i][SECFunModuleSchema.MENUID].ToString().Trim();

                        string modulename = "";
                        if (LangInfo == Language.US)
                        {
                            modulename = dv[i][SECFunModuleSchema.MENUENGLISHNAME].ToString();
                        }
                        else if (LangInfo == Language.TW)
                        {
                            modulename = dv[i][SECFunModuleSchema.MENUTWNAME].ToString();
                        }
                        else if (LangInfo == Language.CN)
                        {
                            modulename = dv[i][SECFunModuleSchema.MENUNAME].ToString();
                        }
                        else
                        {
                            modulename = dv[i][SECFunModuleSchema.MENUNAME].ToString();
                        }
                        string modulelabel = dv[i][SECFunModuleSchema.LABEL].ToString().Trim();
                       
                        CheerUI.TreeNode _leafNode = new CheerUI.TreeNode();
                        _leafNode.Tag = menuid;
                        _leafNode.NodeID = modulelabel;
                        _leafNode.Text = modulename;
                        _leafNode.Selectable = true;
                        _leafNode.EnableClickEvent = true;
                        //_leafNode.OnClientClick = "F.wnd.updateIFrameNode(F('" + tabUnitManagerMaintainPage.ClientID + "'),'../StdOrg/UnitManagerMaintainPage.aspx?UnitID=" + _leafNode.NodeID + "');F.wnd.updateIFrameNode(F('" + tabOrgMulPage.ClientID + "'),'../StdOrg/OrgMulPage.aspx?UnitID=" + _leafNode.NodeID + "');";
                    
                        _rootNode.Nodes.Add(_leafNode);
                      
                    }
                }
            }
            this.secviewTree.Nodes.Add(_rootNode);

        }

        protected void secviewTree_NodeCommand(object sender, CheerUI.TreeCommandEventArgs e)
        {
            txtMenuIDList.Text = "";
            loadFun(e.NodeID);
            table.Hidden = true;
        }

        private void loadFun(string moduleid)
        {
            this.detialTree.Nodes.Clear();

            FunctionLoader FL = new FunctionLoader();
            DataTable _DT = FL.GetAccessALLMenu(base.InfomationPackage.UserID.ToUpper(), moduleid);
            string LanCol = "MENUNAME";
            Hashtable HTFun = new Hashtable();
           
            for (int i = 0; i < _DT.Rows.Count; i++)
            {
                string MENUID = _DT.Rows[i]["MENUID"].ToString();
                //string RIGHTID = _DT.Rows[i]["RIGHTID"].ToString();
                string MENUNAME = _DT.Rows[i][LanCol].ToString();
                string LABEL = _DT.Rows[i]["LABEL"].ToString();
                string PMENUID = _DT.Rows[i]["PMENUID"].ToString();
                string URL = _DT.Rows[i]["URL"].ToString();
                string FUNCTIONTYPE = _DT.Rows[i]["FUNCTIONTYPE"].ToString();
                if (PMENUID == "000000")
                {
                    CheerUI.TreeNode modulenode = new CheerUI.TreeNode();
                    modulenode.Text = MENUNAME;
                    modulenode.EnableCheckBox = true;
                    modulenode.EnableCheckEvent = true;

                    modulenode.Tag = MENUID + "|" + MENUID + "|M";
                    modulenode.NodeID = MENUID + "|" + LABEL;
                    //modulenode.DataKey = MENUID + "|" + LABEL;
                    modulenode.Expanded = true;
                    modulenode.EnableClickEvent = true;

                    //if (_HTMenu.ContainsKey(MENUID))
                    //{ modulenode.Checked = true; }
                    detialTree.Nodes.Add(modulenode);
                    HTFun.Add(MENUID, modulenode);
                }
                else
                {
                    if (HTFun.ContainsKey(PMENUID))
                    {
                        CheerUI.TreeNode _parentitem = (CheerUI.TreeNode)HTFun[PMENUID];

                        CheerUI.TreeNode modulenode = new CheerUI.TreeNode();
                        modulenode.Text = MENUNAME;
                        modulenode.EnableCheckBox = true;
                        modulenode.EnableCheckEvent = true;
                        modulenode.EnableClickEvent = true;

                        modulenode.Tag = MENUID + "|" + MENUID;
                        if (FUNCTIONTYPE == "2")
                        {
                            modulenode.Tag += "|F";
                        }
                        else
                        {
                            modulenode.Tag += "|M";
                        }
                        //modulenode.DataKey = MENUID + "|" + FUNCTIONTYPE;
                        modulenode.NodeID = MENUID + "|" + FUNCTIONTYPE;
                        modulenode.Expanded = true;

                        //if (_HTMenu.ContainsKey(MENUID))
                        //{ modulenode.Checked = true; }

                        _parentitem.Nodes.Add(modulenode);
                        HTFun.Add(MENUID, modulenode);
                    }
                }
            }

        }

        protected void secviewTree_NodeCheck(object sender, CheerUI.TreeCheckEventArgs e)
        {
            txtMenuIDList.Text = "";
            if (!e.Node.Leaf)
            {
                CheckTreeNode(e.Node.Nodes, e.Checked);
            }
            CheckTreeNodeP(e.Node);

            CheerUI.TreeNode[] nodes = detialTree.GetCheckedNodes();
            if (nodes.Length > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (CheerUI.TreeNode node in nodes)
                {
                    sb.AppendFormat("{0} ", node.NodeID);
                }
                txtMenuIDList.Text =  sb.ToString();
            }
           
           
        }

        private void CheckTreeNode(CheerUI.TreeNodeCollection nodes, bool isChecked)
        {
            foreach (CheerUI.TreeNode node in nodes)
            {
                node.Checked = isChecked;
               
                if (!node.Leaf)
                {
                    CheckTreeNode(node.Nodes, isChecked);
                }
            }
        }

        private void CheckTreeNodeP(CheerUI.TreeNode node)
        {
            if (node.ParentNode != null)
            {
                CheckTreeNodeP(node.ParentNode);
                node.ParentNode.Checked = true;
              
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            string menuIDList = this.txtMenuIDList.Text;
            Session[AppConstants.SESSION_PAGE_MANAGER] = menuIDList;
           
            CheerUI.PageContext.RegisterStartupScript("showSecurity()");
            
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            SecurityViewBL securityView = (SecurityViewBL)eHRPageServer.GetPalauObject(typeof(SecurityViewBL));
            try
            {
                Hashtable menuidhash = new Hashtable();
                //----取出所有checked的menuid
                //string[] allmenuidlist = this.txtMenuIDList.Text.ToString().Split('|');
                //string menuidlist = this.AddMenuidHash(allmenuidlist, menuidhash);
                //-----
                string[] allmenuidlist = this.txtMenuIDList.Text.ToString().Split(' ');
                string menuidlist = "";
                foreach (string id in allmenuidlist)
                {
                    menuidlist += "'" + id.Split('|')[0] + "',";
                }
                if (menuidlist.Length > 0)
                    menuidlist = menuidlist.Substring(0, menuidlist.Length - 1);

                string moduleid = detialTree.Nodes[0].NodeID;
                DataSet ds = securityView.GetSecurityInfoForExcel(menuidlist, moduleid, base.getBusinessUnitID());
                ds = this.CheckData(ds);
                DataSet checkds = new DataSet();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    checkds.Merge(ds.Tables[0].Select("", "ROLENAME ASC,LABEL ASC"));
                }
                else
                {
                    checkds = ds;
                }
                GridDataExportor _exportor = new GridDataExportor(this.Request);

                string _scriptStr = _exportor.ExportExcelData(System.Web.HttpUtility.UrlEncode("功能权限查看") + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"), ds.Tables[0], GetGrid(GetDisparray(), checkds.Tables[0]));
                CheerUI.PageContext.RegisterStartupScript(_scriptStr);

                //loadFun(detialTree.Nodes[0].NodeID);
            }
            catch (Exception ex)
            {
                base.eHRThrow_Sys(ex);
            }

          
        }

        private DataSet CheckData(DataSet checkinfods)
        {
            foreach (DataRow dr in checkinfods.Tables[0].Rows)
            {
                string ifMenu = dr["IFMENU"].ToString();
                string functionMenuName;
                string menuName;
                if (ifMenu.CompareTo("1") == 0)
                {
                    functionMenuName = dr["FUNCTIONNAME"].ToString();
                    menuName = dr["MENUNAME"].ToString();
                    dr["FUNCTIONNAME"] = menuName;
                    dr["MENUNAME"] = "";
                }
            }
            return checkinfods;
        }

        private string AddMenuidHash(string[] allmenuidlist, Hashtable menuidhash)
        {
            foreach (string menuidfull in allmenuidlist)
            {
                if (menuidfull.Trim() != "")
                {
                    string menuflag = menuidfull.Trim().Substring(0, 1);
                    string addflag = menuidfull.Trim().Substring(1, 1);
                    string menuid = menuidfull.Substring(2);
                    if (addflag == "#")
                    {
                        if (!menuidhash.Contains(menuid))
                        {
                            if (menuflag.ToUpper().Trim() == "M")
                                menuidhash.Add(menuid, 1);
                            else
                                menuidhash.Add(menuid, 0);
                        }
                    }
                    if (addflag == "=")
                    {
                        if (menuidhash.Contains(menuid))
                            menuidhash.Remove(menuid);
                    }
                }
            }
            string menuidlist = "";
            if (menuidhash.Count > 0)
            {
                IDictionaryEnumerator IDE = menuidhash.GetEnumerator();
                IDE.Reset();
                while (IDE.MoveNext())
                {
                    menuidlist += "'" + IDE.Key + "',";
                }
                if (menuidlist.Length > 0)
                    menuidlist = menuidlist.Substring(0, menuidlist.Length - 1);
            }
            return menuidlist;
        }

        private CheerUI.Grid GetGrid(ArrayList listheader, DataTable dtsource)
        {
            CheerUI.Grid grid = new CheerUI.Grid();
            grid.Columns.Clear();
            CheerUI.BoundField bf;

            foreach (DataColumn clm in dtsource.Columns)
            {
                bf = new CheerUI.BoundField();
                bf.DataField = clm.ColumnName;
                bf.ID = clm.ColumnName;
                bf.DataFormatString = "{0}";
                bf.HeaderText = "";
                grid.Columns.Add(bf);
            }
          
            foreach (CheerUI.GridColumn uclm in grid.Columns)
            {
                bool flag = false;
                foreach (string str in listheader)
                {
                    string[] array = str.Split(new char[] { '|' });
                    if (uclm.ID.ToUpper() == array[0].ToUpper())
                    {
                        uclm.HeaderText = array[1];
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                    uclm.Hidden = true;
            }
            
            return grid;
        }

        private ArrayList GetDisparray()
        {
            ArrayList str_disparray = new ArrayList();
            str_disparray.Add("FUNCTIONNAME|" + "菜单名称");
            str_disparray.Add("MENUNAME|" + "功能点名称");
            str_disparray.Add("ROLENAME|" + "角色名称");
            str_disparray.Add("LOGINNAME|" + "登陆名");
            str_disparray.Add("TRUENAME|" + "姓名");
            str_disparray.Add("EMPLOYEEID|" + "工号");
            return str_disparray;
        }

        protected void detialTree_NodeCommand(object sender, CheerUI.TreeCommandEventArgs e)
        {
            table.Hidden = false;
            CheerUI.TreeNode node = e.Node;
            string [] menuinfor = node.Tag.Split('|');
            string menuname = node.Text;
            string menuid = menuinfor[0];
            string menuright = menuinfor[1];
            txtMenuName.Text = menuname;
            txtMenuNumber.Text = menuid;
            lblRightID.Text = menuright;
        }
    }
}