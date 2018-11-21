using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.PresentationLayer;

namespace CHEER.PresentationLayer.Security.Services
{
    public partial class MailMsgSetting : CHEERBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnClose.Text = "关闭窗口";
                this.get_transdata();
                this.init_tab();
            }
        }
        private void get_transdata()
		{
            ViewState["id"] = Request.QueryString["id"];
		}
        private void init_tab()
        {
            UltDepTab.Tabs.Clear();
            string id = "";
            if (ViewState["id"] != null) { id = ViewState["id"].ToString(); }
            CheerUI.Tab treetab = new CheerUI.Tab();
            treetab.Title = "邮件模板设定";
            treetab.IFrameUrl = "../../eMailCenter/Solution/SetTempleteVariable.aspx?TempleteID=" + id;
            treetab.EnableIFrame = true;
            UltDepTab.Tabs.Add(treetab);

            CheerUI.Tab searchtab = new CheerUI.Tab();
            searchtab.Title = "邮件接收人设定";
            searchtab.IFrameUrl = "../../eMailCenter/Solution/SetRecipients.aspx?TempleteID=" + id;
            searchtab.EnableIFrame = true;
            UltDepTab.Tabs.Add(searchtab);
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            CheerUI.PageContext.RegisterStartupScript(CheerUI.ActiveWindow.GetHidePostBackReference());
        }
    }
}