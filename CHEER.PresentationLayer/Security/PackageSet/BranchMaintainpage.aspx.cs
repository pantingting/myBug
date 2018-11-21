using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.PresentationLayer;

namespace CHEER.PresentationLayer.Security.PackageSet
{
    public partial class BranchMaintainpage : CHEERBasePage
    {
        private string PackageID
        {
            get { return (string)ViewState["PackageID"]; }
            set { ViewState["PackageID"] = value; }
        }
        private string RightID
        {
            get { return (string)ViewState["RightID"]; }
            set { ViewState["RightID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.CheckAccess();
            if (!IsPostBack)
            {
                this.GetTransData();
                this.InitTab();
 
            }
        }
        private void CheckAccess()
        {
            base.CheckIsLoginOut();
        }
        private void GetTransData()
        {
            if (Request.QueryString["PACKAGEID"] != null)
                this.PackageID = Request.QueryString["PACKAGEID"].Trim();
            else
                this.PackageID = "";
            if (Request.QueryString["RIGHTID"] != null)
                this.RightID = Request.QueryString["RIGHTID"].Trim();
            else
                this.RightID = "";
        }
        private void InitTab()
        {
            this.UlPsnRelateTab.Tabs.Clear();
            CheerUI.Tab branchtab = new CheerUI.Tab();
            branchtab.Title = "组织维度";
            branchtab.IFrameUrl = "BchEditPage.aspx?PACKAGEID=" + this.PackageID.Trim() + "&RIGHTID=" + this.RightID.Trim();
            branchtab.EnableIFrame = true;
            UlPsnRelateTab.Tabs.Add(branchtab);
            CheerUI.Tab abstracttab = new CheerUI.Tab();
            abstracttab.Title = "抽象维度";
            //abstracttab.Hidden = true;
            abstracttab.IFrameUrl = "AbstractMaintainPage.aspx?PACKAGEID=" + this.PackageID.Trim();
            abstracttab.EnableIFrame = true;
            UlPsnRelateTab.Tabs.Add(abstracttab);
            CheerUI.Tab psnrelatetab = new CheerUI.Tab();
            psnrelatetab.Title = "人员相关扩展维度";
            psnrelatetab.IFrameUrl = "BchExtendPage.aspx?PACKAGEID=" + this.PackageID.Trim();
            psnrelatetab.EnableIFrame = true;
            UlPsnRelateTab.Tabs.Add(psnrelatetab);
        }
    }
}