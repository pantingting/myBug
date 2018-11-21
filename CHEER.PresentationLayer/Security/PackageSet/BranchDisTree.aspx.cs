using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.Organize;
using CHEER.Common;
using CHEER.Common.Schema;
using CHEER.PresentationLayer;

namespace CHEER.PresentationLayer.Security.PackageSet
{
    public partial class BranchDisTree : CHEERBasePage
    {
        private string SecurityID
        {
            get { return (string)ViewState["SecurityID"]; }
            set { ViewState["SecurityID"] = value; }
        }
        private Hashtable AllDisLabelsHash
        {
            get { return (Hashtable)ViewState["AllDisLabels"]; }
            set { ViewState["AllDisLabels"] = value; }
        }
        private string IsIncludeStopUnit
        {
            get { return (string)ViewState["IsIncludeStopUnit"]; }
            set { ViewState["IsIncludeStopUnit"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.get_transdata();
                this.load_treedata();
            }
        }
        private void get_transdata()
        {
            if (Request.QueryString["SECURITYID"] != null)
                this.SecurityID = Request.QueryString["SECURITYID"].Trim();
            else
                this.SecurityID = "";
            if (Request.QueryString["INCLUDESTOPUNIT"] != null)
                this.IsIncludeStopUnit = Request.QueryString["INCLUDESTOPUNIT"].Trim();
            else
                this.IsIncludeStopUnit = "";
        }

        private void load_treedata()
        {
            STDOrganizeManager stdmanager = (STDOrganizeManager)eHRPageServer.GetPalauObject(typeof(STDOrganizeManager));
            DataSet unitds = this.getallunitds(stdmanager, "0");
            if (unitds.Tables.Count <= 0)
                return;
            Hashtable dislabelhash = null;
            if (this.SecurityID != "")
            {
                dislabelhash = this.getalldisunitlabel(stdmanager, this.SecurityID);
            }
            string filter = ORGStdStructSchema.ORGSTDSTRUCT_LABEL + "=" + "0000";
            DataRow[] rootUnit = unitds.Tables[0].Select(filter);
            if (rootUnit.Length > 0)
            {
                string labelstr = rootUnit[0][ORGStdStructSchema.ORGSTDSTRUCT_LABEL].ToString();
                if (labelstr == "0000")
                {
                    string rootid = rootUnit[0][ORGStdStructSchema.ORGSTDSTRUCT_UNITID].ToString();
                    string rootname = rootUnit[0][ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME].ToString();
                    string rootcode = rootUnit[0][ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE].ToString();
                    int rootlength = Convert.ToInt32(rootUnit[0][ORGStdStructSchema.ORGSTDSTRUCT_LABELLENGTH].ToString());
                    CheerUI.TreeNode rootnode = new CheerUI.TreeNode();
                    rootnode.Text = rootname;
                    rootnode.NodeID = rootid;
                    
                    if (dislabelhash != null && dislabelhash.ContainsKey(labelstr) && dislabelhash[labelstr].ToString() == "YES")
                    {
                        rootnode.Tag = rootcode + "*|*" + labelstr + "*|*" + rootlength.ToString() + "*|*" + rootid + "*|*YES";
                    }
                    else
                    {
                        rootnode.Tag = rootcode + "*|*" + labelstr + "*|*" + rootlength.ToString() + "*|*" + rootid + "*|*NO";
                    }
                    this.UltraDepTree.Nodes.Add(rootnode);
                }
            }
            else
            {
                return;
            }
            
            if (this.SecurityID != "")
            {
                this.AllDisLabelsHash = dislabelhash;
                if (dislabelhash == null || dislabelhash.Count == 0)
                    return;
                else
                    this.load_treenodes(unitds, dislabelhash, this.UltraDepTree.Nodes[0]);
            }
            else
            {
                this.load_treenodes(unitds, null, this.UltraDepTree.Nodes[0]);
            }
        }
        private Hashtable getalldisunitlabel(STDOrganizeManager stdmanager, string securityid)
        {
            Hashtable dislabelhash = new Hashtable();
            ArrayList seclabellist = this.getsecunitlabel(stdmanager, securityid);
            for (int i = 0; i < seclabellist.Count; i++)
            {
                string unitlabel = seclabellist[i].ToString();
                if (!dislabelhash.Contains(unitlabel))
                    dislabelhash.Add(unitlabel, "YES");
            }
            for (int i = 0; i < seclabellist.Count; i++)
            {
                string unitlabel = seclabellist[i].ToString();
                int labellength = unitlabel.Length;
                int segmentcount = labellength / 4;
                for (int j = 1; j < segmentcount; j++)
                {
                    string plabel = unitlabel.Substring(0, (labellength - j * 4));
                    if (plabel != "")
                    {
                        if (!dislabelhash.Contains(plabel))
                            dislabelhash.Add(plabel, "NO");
                    }
                }
            }
            return dislabelhash;
        }
        private ArrayList getsecunitlabel(STDOrganizeManager stdmanager, string securityid)
        {
            bool isIncludeStopUnit = false;
            if (this.IsIncludeStopUnit == "True")
            {
                isIncludeStopUnit = true;
            }
            STDOrganizeManager orgManager = (STDOrganizeManager)base.GetPalauObject(typeof(STDOrganizeManager), SecurityID, false, "", false, base.getBusinessUnitID(), isIncludeStopUnit);
            orgManager.IsIncludeStopUnit = isIncludeStopUnit;
            DataSet allbranchds = orgManager.GetUnitBySqlWithRootIDWithSecurity(true, "", false);
            if (!ifdsnotnull(allbranchds))
                return new ArrayList();
            ArrayList secunitlabel = new ArrayList();
            foreach (DataRow row in allbranchds.Tables[0].Rows)
            {
                string label = row[ORGStdStructSchema.ORGSTDSTRUCT_LABEL].ToString();
                secunitlabel.Add(label);
            }
            return secunitlabel;
        }
        private DataSet getallunitds(STDOrganizeManager stdmanager, string unitid)
        {
            if (IsIncludeStopUnit == "True")
            {
                stdmanager.IsIncludeStopUnit = true;
            }
            DataSet ds = stdmanager.GetUnitBySqlAndLevelMaxWithRootID(true, "", false, 2, unitid);
            return ds;
        }
        private void load_treenodes(DataSet unitds, Hashtable dislabelhash, CheerUI.TreeNode treenode)
        {
            string nodelen = this.getneedstr(treenode.Tag.ToString(), "length");
            int nodelength = Convert.ToInt32(nodelen);
            int labellength;
            string unitid;
            string unitcode;
            string unitname;
            string unitlabel;
            string unitsec;
            CheerUI.TreeNode newnode;
            
            for (int i = 0; i < unitds.Tables[0].Rows.Count; i++)
            {
                labellength = Convert.ToInt32(unitds.Tables[0].Rows[i][ORGStdStructSchema.ORGSTDSTRUCT_LABELLENGTH].ToString());
                unitid = unitds.Tables[0].Rows[i][ORGStdStructSchema.ORGSTDSTRUCT_UNITID].ToString();
                unitcode = unitds.Tables[0].Rows[i][ORGStdStructSchema.ORGSTDSTRUCT_UNITCODE].ToString();
                unitname = unitds.Tables[0].Rows[i][ORGStdStructSchema.ORGSTDSTRUCT_UNITNAME].ToString();
                unitlabel = unitds.Tables[0].Rows[i][ORGStdStructSchema.ORGSTDSTRUCT_LABEL].ToString();
                if (dislabelhash == null || (dislabelhash != null && dislabelhash.Count > 0 && dislabelhash.Contains(unitlabel)))
                {
                    unitsec = "YES";
                    if (dislabelhash != null && dislabelhash.Count > 0 && dislabelhash.Contains(unitlabel))
                    {
                        unitsec = dislabelhash[unitlabel].ToString();
                    }
                    bool noaddtotree = true;
                    if (labellength - nodelength == 4)
                    {
                        noaddtotree = this.ifnodeidhasexit(treenode.Nodes, unitid);
                    }
                    newnode = new CheerUI.TreeNode();
                    newnode.Text = unitname;
                    newnode.NodeID = unitid;
                    newnode.Tag = unitcode + "*|*" + unitlabel + "*|*" + labellength.ToString() + "*|*" + unitid + "*|*" + unitsec;
                    if (labellength - nodelength == 4)
                    {
                        if (!noaddtotree)
                        {
                            treenode.Expanded = true;
                            treenode.Nodes.Add(newnode);
                        }
                    }
                    if (labellength - nodelength == 8)
                    {
                        string punitid = unitds.Tables[0].Rows[i][ORGStdStructSchema.ORGSTDSTRUCT_PUNITID].ToString();
                        foreach (CheerUI.TreeNode node in treenode.Nodes)
                        {
                            string nodeunitid = node.NodeID.ToString();
                            if (punitid == nodeunitid)
                            {
                                if (!this.ifnodeidhasexit(node.Nodes, unitid))
                                {
                                    node.Expanded = false;
                                    node.Nodes.Add(newnode);
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
        private bool ifnodeidhasexit(CheerUI.TreeNodeCollection nodes, string newnodeid)
        {
            foreach (CheerUI.TreeNode node in nodes)
            {
                string nownodeid = node.NodeID.ToString();
                if (nownodeid == newnodeid)
                    return true;
            }
            return false;
        }
        private string getneedstr(string seccodelabelstr, string type)
        {
            string[] strlist = seccodelabelstr.Split(new char[] { '*', '|', '*' });
            ArrayList needlist = new ArrayList();
            foreach (string str in strlist)
            {
                if (str.Trim() != "")
                {
                    needlist.Add(str.Trim());
                }
            }
            try
            {
                if (type == "code")
                {
                    return needlist[0].ToString().Trim();
                }
                if (type == "label")
                {
                    return needlist[1].ToString().Trim();
                }
                if (type == "length")
                {
                    return needlist[2].ToString().Trim();
                }
                if (type == "sec")
                {
                    return needlist[4].ToString().Trim();
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        private bool ifdsnotnull(DataSet ds)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                    return true;
            }
            return false;
        }
    }
}