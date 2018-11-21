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
using CHEER.CommonLayer.eSecurity.Schema;
using CHEER.PresentationLayer;
using CHEER.BusinessLayer.Security.Portal;

namespace CHEER.PresentationLayer.Security.SecuritySet
{
    public partial class SecuritySetPage : CHEERBasePage
    {
        private string PackageID
        {
            get { return (string)ViewState["PackageID"]; }
            set { ViewState["PackageID"] = value; }
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
                this.lbMenuIDList.Text = "";
                this.InitFace();
                this.GetTransData();
                this.LoadModuleTree();
            }
        }
        private void InitFace()
        {
            this.cmdSave.Text = "保存";
        }
        private void GetTransData()
        {
            if (Request.QueryString["PACKAGEID"] != null)
                this.PackageID = Request.QueryString["PACKAGEID"].Trim();
            else
                this.PackageID = "";
            this.txtPackageID.Text = this.PackageID.Trim();
            if (Request.QueryString["MANID"] != null)
                this.ManID = Request.QueryString["MANID"].Trim();
            else
                this.ManID = "";
            if (Request.QueryString["MANTYPE"] != null)
                this.ManType = Request.QueryString["MANTYPE"].Trim();
            else
                this.ManType = "";
            if (Request.QueryString["BACK"] != null)
                this.txtIsBack.Text = Request.QueryString["BACK"].Trim();
            else
                this.txtIsBack.Text = "";
        }
        private void LoadModuleTree()
        {
            this.UlModuleTree.Nodes.Clear();
            CheerUI.TreeNode rootnode = new CheerUI.TreeNode();
            rootnode.Text = "功能列表";
            rootnode.Tag = "000000";
            rootnode.NodeID = "000000";
            rootnode.Expanded = true;
            FunctionModule modulemanager = (FunctionModule)eHRPageServer.GetPalauObject(typeof(FunctionModule));
            DataSet moduleds = modulemanager.GetAllModuleInfor();
            string userid = base.InfomationPackage.UserID;
            if (userid != "" && userid.ToLower() != "sa")
            {
                moduleds = modulemanager.GetUserModuleInfor(userid);
            }
            if (moduleds.Tables.Count > 0)
            {
                if (moduleds.Tables[0].Rows.Count > 0)
                {
                    DataView dv = moduleds.Tables[0].DefaultView;
                    dv.Sort = SECFunModuleSchema.LABEL + " ASC";
                    for (int i = 0; i < dv.Count; i++)
                    {
                        //string moduleoid = dv[i][SECFunModuleSchema.OID].ToString().Trim();
                        string menuid = dv[i][SECFunModuleSchema.MENUID].ToString().Trim();
                        //string ifchecklic = dv[i][SECFunModuleSchema.MENUITEM1].ToString().Trim();
                        //ModuleLicense _ML = CommonFunction.CheckModuleValid(moduleoid,0) ;

                        string modulename;
                        if (base.GetLanguageInfo() == Language.US)
                        {
                            modulename = dv[i][SECFunModuleSchema.MENUENGLISHNAME].ToString();
                        }
                        else if (base.GetLanguageInfo() == Language.TW)
                        {
                            modulename = dv[i][SECFunModuleSchema.MENUTWNAME].ToString();
                        }
                        else if (base.GetLanguageInfo() == Language.CN)
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
                        modulenode.NodeID = modulelabel;
                        modulenode.EnableClickEvent = true;
                       
                        rootnode.Nodes.Add(modulenode);

                    }
                }
            }
            this.UlModuleTree.Nodes.Add(rootnode);
        }


        protected void UlModuleTree_NodeCommand(object sender, CheerUI.TreeCommandEventArgs e)
        {
            if (e.Node == null)
            {
                return;
            }
            lbMenuIDList.Text = "";
            string moduleid = e.Node.Tag.ToString();
            LoadMenu(moduleid);
        }

        private void LoadMenu(string moduleid)
        {
            this.UlFunTree.Nodes.Clear();
            
            Hashtable _HTMenu = GetAccessMenuHash(moduleid);
            FunctionLoader FL = new FunctionLoader();
            DataTable _DT = FL.GetAllMenu(moduleid);

            string userid = base.InfomationPackage.UserID;
            if (userid != "" && userid.ToLower() != "sa")
            {
                _DT = FL.GetUserMenu(moduleid,userid);
            }
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
                string CANUSE = _DT.Rows[i]["CANUSE"].ToString();
                if (CANUSE == "0") { continue; }
                if (PMENUID == "000000")
                {
                    CheerUI.TreeNode modulenode = new CheerUI.TreeNode();
                    modulenode.Text = MENUNAME;
                    modulenode.Tag = MENUID + "|" + MENUID + "|M";
                    modulenode.NodeID = MENUID + "_" + LABEL;
                    modulenode.Expanded = true;
                    modulenode.EnableCheckBox = true;
                    modulenode.EnableCheckEvent = true; 
                    if (_HTMenu.ContainsKey(MENUID))
                    { modulenode.Checked = true; }
                    UlFunTree.Nodes.Add(modulenode);
                    HTFun.Add(MENUID, modulenode);
                }
                else
                {
                    if (HTFun.ContainsKey(PMENUID))
                    {
                        CheerUI.TreeNode _parentitem = (CheerUI.TreeNode)HTFun[PMENUID];

                        CheerUI.TreeNode modulenode = new CheerUI.TreeNode();
                        modulenode.Text = MENUNAME;
                        modulenode.Tag = MENUID + "|" + MENUID;
                        if (FUNCTIONTYPE == "2")
                        {
                            modulenode.Tag += "|F";
                        }
                        else
                        {
                            modulenode.Tag += "|M";
                        }
                        modulenode.NodeID = MENUID + "_" + FUNCTIONTYPE;
                        modulenode.Expanded = true;
                        modulenode.EnableCheckBox = true;
                        modulenode.EnableCheckEvent = true; 
                        if (_HTMenu.ContainsKey(MENUID))
                        { modulenode.Checked = true; }

                        _parentitem.Nodes.Add(modulenode);
                        HTFun.Add(MENUID, modulenode);
                    }
                }
            }
        }

        private Hashtable GetAccessMenuHash(string moduleid)
        {
            int inttype = 0;
            if (this.ManType.Trim() != "")
            {
                inttype = Convert.ToInt32(this.ManType.Trim());
            }

            SecurityPackageMap secmapmanager = (SecurityPackageMap)eHRPageServer.GetPalauObject(typeof(SecurityPackageMap));
            DataSet secds = secmapmanager.GetPackageSecFunDataofMan(this.ManID.Trim(), inttype, moduleid, this.PackageID.Trim());
            Hashtable sechash = new Hashtable();

            foreach (DataRow row in secds.Tables[0].Rows)
            {
                string funid = row[SECPackageMapSchema.MENUID].ToString().Trim();
                string funtype = row[SECPackageMapSchema.IFMENU].ToString().Trim();
                string funflag = "F";
                if (funtype.Trim() != "0")
                {
                    funflag = "M";
                }
                if (!sechash.Contains(funid))
                {
                    lbMenuIDList.Text = this.lbMenuIDList.Text + "|" + funflag + "#" + funid;
                    sechash.Add(funid, "");
                }
            }

            return sechash;
        }
        bool globalAreaFlag = true;
        protected void UlFunTree_NodeCheck(object sender, CheerUI.TreeCheckEventArgs e)
        {
            if(!globalAreaFlag)
				return;
			else
				globalAreaFlag=false;
            if (e.Checked)
            {
                UlFunTree.CheckAllNodes(e.Node.Nodes);
            }
            else
            {
                UlFunTree.UncheckAllNodes(e.Node.Nodes);
            }
            //var menuidlist = document.getElementById('txtMenuIDList');	
			
			string nfunid=e.Node.Tag.Split('|')[0];//node.getTag().split('|')[0];
			string nfunflag=e.Node.Tag.Split('|')[2];//node.getTag().split('|')[2];
			if(e.Checked)
                lbMenuIDList.Text = lbMenuIDList.Text + "|" + nfunflag + "#" + nfunid;
			else
                lbMenuIDList.Text = lbMenuIDList.Text + "|" + nfunflag + "=" + nfunid;


            getNodesTag(e.Node);
            //foreach (CheerUI.TreeNode node in e.Node.Nodes)
            //{
            //    nfunid = node.Tag.Split('|')[0];
            //    nfunflag = node.Tag.Split('|')[2];
            //    if (node.Checked)
            //    {

            //        lbMenuIDList.Text = lbMenuIDList.Text + "|" + nfunflag + "#" + nfunid;
            //    }
            //    else
            //    {
            //        lbMenuIDList.Text = lbMenuIDList.Text + "|" + nfunflag + "=" + nfunid;
            //    }
            //}

			if(e.Checked)
				changepnodecheck(e.Node,e.Checked);
			globalAreaFlag = true;
			return;

        }
        private void getNodesTag(CheerUI.TreeNode PNode)
        {
            if (PNode.Nodes.Count > 0)
            {
                string nfunid;
                string nfunflag;
                foreach (CheerUI.TreeNode node in PNode.Nodes)
                {
                    nfunid = node.Tag.Split('|')[0];
                    nfunflag = node.Tag.Split('|')[2];
                    if (node.Checked)
                    {

                        lbMenuIDList.Text = lbMenuIDList.Text + "|" + nfunflag + "#" + nfunid;
                    }
                    else
                    {
                        lbMenuIDList.Text = lbMenuIDList.Text + "|" + nfunflag + "=" + nfunid;
                    }
                    getNodesTag(node);
                }
            }
        }

        public void changepnodecheck(CheerUI.TreeNode node,bool eChecked)
        {
            CheerUI.TreeNode Pnode = node.ParentNode;
            if (Pnode == null)
                return;
            string nfunid=Pnode.Tag.Split('|')[0];
			string nfunflag=Pnode.Tag.Split('|')[2];
            lbMenuIDList.Text = lbMenuIDList.Text + "|" + nfunflag + "#" + nfunid;
			Pnode.Checked=eChecked;
            changepnodecheck(Pnode, eChecked);
        }
       
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            if (this.UlFunTree.Nodes.Count <= 0)
                return;
            string moduleid = this.UlFunTree.Nodes[0].NodeID.ToString().Split('_')[0].Trim();
            string modulelabel = this.UlFunTree.Nodes[0].NodeID.ToString().Split('_')[1].Trim();
            if (lbMenuIDList.Text.Trim() == "")
                return;
            Hashtable menuidhash = new Hashtable();
            string[] allmenuidlist = this.lbMenuIDList.Text.Trim().Split('|');
            menuidhash = this.AddMenuidHash(allmenuidlist, menuidhash);
            int inttype = 0;
            if (this.ManType.Trim() != "")
                inttype = Convert.ToInt32(this.ManType.Trim());
            SecurityPackageMap secmanager = (SecurityPackageMap)eHRPageServer.GetPalauObject(typeof(SecurityPackageMap));
            secmanager.SavePackageMapInforList(menuidhash, moduleid, this.PackageID.Trim(), this.ManID.Trim(), inttype);
            base.ShowAlert("保存成功!");
            this.lbMenuIDList.Text = "";
            //this.LoadFunctionTree(moduleid,modulelabel);
            LoadMenu(moduleid);
        }
        private Hashtable AddMenuidHash(string[] allmenuidlist, Hashtable menuidhash)
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
            return menuidhash;
        }
    }
}