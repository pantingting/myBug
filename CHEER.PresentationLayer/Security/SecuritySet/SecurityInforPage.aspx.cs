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

namespace CHEER.PresentationLayer.Security.SecuritySet
{
    public partial class SecurityInforPage : CHEERBasePage
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.GetTransData();
                this.InitTab();
            }
        }

        private void InitTab()
        {
            SecurityPackageMap secmapmanager = (SecurityPackageMap)eHRPageServer.GetPalauObject(typeof(SecurityPackageMap));
            int inttype = 0;
            Language LangInfo = base.GetLanguageInfo();
            if (this.ManType.Trim() != "")
                inttype = Convert.ToInt32(this.ManType.Trim());
            DataSet secmoduleds = secmapmanager.GetSecModuleofMan(this.ManID.Trim(), inttype);
            if (secmoduleds.Tables.Count > 0)
            {
                if (secmoduleds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in secmoduleds.Tables[0].Rows)
                    {
                        
                        if (!row.IsNull(SECFunModuleSchema.MODULEID) && row[SECFunModuleSchema.MODULEID].ToString().Trim() != "")
                        {
                            string moduleid = row[SECFunModuleSchema.MODULEID].ToString().Trim();
                            string modulename = "";
                            if (LangInfo == Language.US)
                            {
                                modulename = row[SECFunModuleSchema.MENUENGLISHNAME].ToString().Trim();
                            }
                            else if (LangInfo == Language.TW)
                            {
                                modulename = row[SECFunModuleSchema.MENUNAME].ToString().Trim();
                            }
                            else
                            {
                                modulename = row[SECFunModuleSchema.MENUNAME].ToString().Trim();
                            }

                            CheerUI.Tab moduletab = new CheerUI.Tab();
                            moduletab.Title = modulename;
                            moduletab.ID = moduleid;
                            moduletab.IFrameUrl = "SecModuleInforPage.aspx?MODULEID=" + moduleid + "&MANID=" + this.ManID.Trim() + "&MANTYPE=" + this.ManType.Trim();
                            moduletab.EnableIFrame = true;
                            this.UlSecInforTab.Tabs.Add(moduletab);
                        }
                    }
                    return;
                }
            }
            string showmessage = "还未设置权限";
            if (ManType == "0")
                showmessage = "该角色" + showmessage;
            if (ManType == "1")
                showmessage = "该用户" + showmessage;
            base.ShowAlert(showmessage);
            //Response.Redirect("../SecControls/WaitPage.aspx?SHOWMESSAGE=" + showmessage);
            //TODO 等候页面 或消息页面
        }
    }
}