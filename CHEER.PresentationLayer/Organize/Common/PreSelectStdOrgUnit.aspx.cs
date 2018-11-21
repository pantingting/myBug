using CHEER.BusinessLayer.Organize;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.Common.Schema;
using CHEER.PresentationLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CHEER.PresentationLayer.Organize.Common
{
    public partial class PreSelectStdOrgUnit : CHEERBasePage
    {
        private int orgLayer = 1;
        private Hashtable accessBranchHT = new Hashtable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MF_Verify();
                Initialize();
                InitializeTree();
            }
        }

        #region 页面权限检测公用方法
        private Hashtable MF_LoadFCData()
        {
            Hashtable hash = new Hashtable();
            return hash;
        }
        private string MF_MenuID = "020060030";
        private void MF_Verify()
        {
            if (!GetSecurityChecker().IsAllow(MF_MenuID))
                CheerUI.Alert.ShowInTop(getAlert("ZGAIA00809"));
            IDictionaryEnumerator idenumerator = MF_LoadFCData().GetEnumerator();
            while (idenumerator.MoveNext())
            {
                if (idenumerator.Value is WebControl)
                {
                    if (!GetSecurityChecker().IsAllow(idenumerator.Key.ToString()))
                    {
                        ((WebControl)idenumerator.Value).Enabled = false;
                    }
                    else
                    {
                        ((WebControl)idenumerator.Value).Enabled = true;
                    }
                }
            }
        }
        #endregion

        private bool IsHaveAccessBranch(string branchID)
        {
            if (Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERID].ToString() == "sa")
            {
                return true;
            }
            STDUnitManager stdManager =
                (STDUnitManager)eHRPageServer.GetPalauObject(typeof(STDUnitManager));
            STDUnit stdunit = stdManager.GetCurentUnitByID(branchID);
            if (stdunit.IsTempUnit)
                return true;
            SecurityChecker _checker = base.GetSecurityChecker();
            ArrayList branchArr = _checker.GetAccessBranches("020060030");
            for (int intx = 0; intx < branchArr.Count; intx++)
            {
                ArrayList tempArr = (ArrayList)branchArr[intx];
                if (tempArr[0].ToString().CompareTo(branchID) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        protected void Initialize()
        {
            treeOrg.Title = base.getString("ZGAIA00958");
            btnSearch.Text = base.getString("ZGAIA00644");
            btnArchive.Text = base.getString("ZGAIA00549");
            btnCopy.Text = base.getString("ZGAIA00253");
            tabUnitManagerMaintainPage.Title = base.getAlert("ZGAIA00498");
            tabOrgMulPage.Title = base.getAlert("ZGAIA00497");
        }

        private void InitializeTree()
        {
            treeOrg.Nodes.Clear();
            CHEER.BusinessLayer.ePersonnel.SystemConfig.PersonnelParameterManager parmManager =
                (CHEER.BusinessLayer.ePersonnel.SystemConfig.PersonnelParameterManager)eHRPageServer.GetPalauObject(typeof(CHEER.BusinessLayer.ePersonnel.SystemConfig.PersonnelParameterManager));
            int _extend_level = 4;
            string _ldlevel = parmManager.GetParameterValue(PersonnelParameterName.OrgLoadLevel);
            if (_ldlevel != "")
                _extend_level = int.Parse(_ldlevel);
            CheerUI.TreeNode _rootNode = new CheerUI.TreeNode();
            STDOrganizeManager stdM = (STDOrganizeManager)eHRPageServer.GetPalauObject(typeof(STDOrganizeManager));
            DataSet _allUnitDS = stdM.GetUnitBySqlAndLevelMax(true, "", true, _extend_level);
            DataTable _allUnitTable = _allUnitDS.Tables[0];
            if (Application[CHEER.Common.Constants.SecurityConstants.Organize_Layer] == null)
            {
                orgLayer = Int32.Parse(parmManager.GetParameterValue(PersonnelParameterName.OrganizeLayer));
            }
            else
            {
                orgLayer = Int32.Parse(Application[CHEER.Common.Constants.SecurityConstants.Organize_Layer].ToString());
            }
            DataView dv = _allUnitTable.DefaultView;
            dv.Sort = ORGStdStructSchema.ORGSTDSTRUCT_LABEL;
            _rootNode.Expanded = true;
            _rootNode.Text = CHEERBasePage.RemedyStringForShow(dv[0][ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE].ToString()) + "\t"
                + CHEERBasePage.RemedyStringForShow(dv[0][ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME].ToString());
            _rootNode.Selectable = GetSecurityChecker().IsAllow(MF_MenuID);
            _rootNode.NodeID = dv[0][ORGStdStructSchema.ORGSTDSTRUCT_UNITID].ToString();
            _rootNode.OnClientClick = "C.wnd.updateIFrameNode(C('" + tabUnitManagerMaintainPage.ClientID + "'),'../StdOrg/UnitManagerMaintainPage.aspx?UnitID=" + _rootNode.NodeID + "');C.wnd.updateIFrameNode(C('" + tabOrgMulPage.ClientID + "'),'../StdOrg/OrgMulPage.aspx?UnitID=" + _rootNode.NodeID + "');";
            _rootNode.EnableClickEvent = false;
            treeOrg.Nodes.Add(_rootNode);

            Hashtable htNodes = new Hashtable();
            htNodes.Add("0", _rootNode);
            if (InfomationPackage.UserID.ToUpper() != "SA")
            {
                Hashtable htAccUnit = new Hashtable();
                SecurityChecker sc = base.GetSecurityChecker();
                BranchStateList bsl = new BranchStateList();
                bsl.Add(BranchState.Efficient);
                bsl.Add(BranchState.Temp);
                string Filter = sc.GetInBranchSql("020060030", true, "0", bsl);
                string SQL = @"select UNITID,PUNITID,UNITCODE,UNITNAME,ISMANAGEUNIT,ISTEMPUNIT,Label,LabelLength,LabelIndex from ORGStdStruct " +
                    " where UNITID IN (" + Filter + ") AND UNITID <> '0' Order by LabelLength,LabelIndex ";
                CHEER.Platform.DAL.PersistBroker b = CHEER.Platform.DAL.PersistBroker.Instance();
                DataTable dt = b.ExecuteSQLForDst(SQL).Tables[0];
                b.Close();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CheerUI.TreeNode _leafNode = new CheerUI.TreeNode();
                    _leafNode.NodeID = dt.Rows[i]["UNITID"].ToString();
                    _leafNode.Text = CHEERBasePage.RemedyStringForShow(dt.Rows[i]["UNITCODE"].ToString()) + "\t"
                        + CHEERBasePage.RemedyStringForShow(dt.Rows[i]["UNITNAME"].ToString());
                    _leafNode.Selectable = IsHaveAccessBranch(_leafNode.NodeID);
                    _leafNode.OnClientClick = "C.wnd.updateIFrameNode(C('" + tabUnitManagerMaintainPage.ClientID + "'),'../StdOrg/UnitManagerMaintainPage.aspx?UnitID=" + _leafNode.NodeID + "');C.wnd.updateIFrameNode(C('" + tabOrgMulPage.ClientID + "'),'../StdOrg/OrgMulPage.aspx?UnitID=" + _leafNode.NodeID + "');";
                    _leafNode.EnableClickEvent = false;
                    if (htNodes.ContainsKey(dt.Rows[i]["PUNITID"].ToString()))
                    {
                        ((CheerUI.TreeNode)htNodes[dt.Rows[i]["PUNITID"].ToString()]).Nodes.Add(_leafNode);
                    }
                    else
                    {
                        ((CheerUI.TreeNode)htNodes["0"]).Nodes.Add(_leafNode);
                    }
                    htNodes.Add(dt.Rows[i]["UNITID"].ToString(), _leafNode);
                }
                for (int i = 0; i < _rootNode.Nodes.Count; i++)
                {
                    _rootNode.Nodes[i].Expanded = false;
                }
            }
            else
            {
                RecurDisplayNodes(_rootNode, 1, _allUnitTable);
            }
        }
        private void RecurDisplayNodes(CheerUI.TreeNode _parentNode, int expInt, DataTable _allUnitTable)
        {
            string filter = ORGStdStructSchema.ORGSTDSTRUCT_PUNITID + "='" + _parentNode.NodeID.ToString() + "'";
            foreach (DataRow dr in _allUnitTable.Select(filter))
            {
                CheerUI.TreeNode _leafNode = new CheerUI.TreeNode();
                if (expInt < orgLayer)
                {
                    _leafNode.Expanded = true;
                }
                _leafNode.NodeID = dr[ORGStdStructSchema.ORGSTDSTRUCT_UNITID].ToString();
                _leafNode.Text = CHEERBasePage.RemedyStringForShow(dr[ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE].ToString()) + "\t"
                    + CHEERBasePage.RemedyStringForShow(dr[ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME].ToString());
                _leafNode.OnClientClick = "C.wnd.updateIFrameNode(C('" + tabUnitManagerMaintainPage.ClientID + "'),'../StdOrg/UnitManagerMaintainPage.aspx?UnitID=" + _leafNode.NodeID + "');C.wnd.updateIFrameNode(C('" + tabOrgMulPage.ClientID + "'),'../StdOrg/OrgMulPage.aspx?UnitID=" + _leafNode.NodeID + "');";
                _leafNode.EnableClickEvent = false;
                _leafNode.Selectable = IsHaveAccessBranch(_leafNode.NodeID);
                _parentNode.Nodes.Add(_leafNode);
                RecurDisplayNodes(_leafNode, expInt + 1, _allUnitTable);
            }
        }

        protected void detailWindow_Close(object sender, CheerUI.WindowCloseEventArgs e)
        {
            InitializeTree();
        }

        protected void treeOrg_NodeLazyLoad(object sender, CheerUI.TreeNodeEventArgs e)
        {
            STDOrganizeManager stdM = (STDOrganizeManager)eHRPageServer.GetPalauObject(typeof(STDOrganizeManager));
            DataSet _allUnitDS = stdM.GetCurrentAllUnitsByUnitID(e.NodeID);
            DataTable _allUnitTable = null;
            if (_allUnitDS != null && _allUnitDS.Tables[0].Rows.Count != 0)
            {
                _allUnitTable = _allUnitDS.Tables[0];
            }
            string filter = ORGStdStructSchema.ORGSTDSTRUCT_PUNITID + "='" + e.NodeID + "'";
            e.Node.Expanded = true;
            var dataRows = _allUnitTable.Select(filter);
            if (dataRows.Length <= 0)
            {
                e.Node.Leaf = true;
            }
            foreach (DataRow dr in dataRows)
            {
                CheerUI.TreeNode _leafNode = new CheerUI.TreeNode();
                _leafNode.NodeID = dr[ORGStdStructSchema.ORGSTDSTRUCT_UNITID].ToString();
                _leafNode.Text = CHEERBasePage.RemedyStringForShow(dr[ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE].ToString()) + "\t"
                    + CHEERBasePage.RemedyStringForShow(dr[ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME].ToString());
                _leafNode.OnClientClick = "C.wnd.updateIFrameNode(C('" + tabUnitManagerMaintainPage.ClientID + "'),'../StdOrg/UnitManagerMaintainPage.aspx?UnitID=" + _leafNode.NodeID + "');C.wnd.updateIFrameNode(C('" + tabOrgMulPage.ClientID + "'),'../StdOrg/OrgMulPage.aspx?UnitID=" + _leafNode.NodeID + "');";
                _leafNode.EnableClickEvent = false;
                _leafNode.Selectable = IsHaveAccessBranch(_leafNode.NodeID);
                e.Node.Nodes.Add(_leafNode);
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            InitializeTree();
        }
    }
}