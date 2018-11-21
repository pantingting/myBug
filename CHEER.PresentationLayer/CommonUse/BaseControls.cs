using System;
using CHEER.Platform.GlobalRM;
using CHEER.Platform.AppDomainManagement;
using System.Web.UI;
using System.Web;
namespace CHEER.PresentationLayer
{
	/// <summary>
	/// BaseControls 的摘要说明。
	/// </summary>
	public class BaseControls: System.Web.UI.UserControl
	{
		public BaseControls()
		{
			
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

        public string getBaseUrl()
        {
            return CHEERBasePage.GetBaseURL(this.Request);

        }
        public string getBaseUrl(System.Web.UI.Page _page)
        {
            return CHEERBasePage.GetBaseURL(_page.Request);
        }
        public static string GetBaseURL(HttpRequest request)
        {
            if (request.IsSecureConnection)
            {
                return @"https://" + request.Url.Authority + request.ApplicationPath + "/";
            }
            else
            {
                return @"http://" + request.Url.Authority + request.ApplicationPath + "/";
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		override protected void OnInit(EventArgs e)
		{
			if(Session["CultureInfo"] == null)
			{
				System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ZH-CN");
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;
			}
			else
			{
				System.Threading.Thread.CurrentThread.CurrentCulture =(System.Globalization.CultureInfo)Session["CultureInfo"];
				System.Threading.Thread.CurrentThread.CurrentUICulture = (System.Globalization.CultureInfo)Session["CultureInfo"];
			}
			base.OnInit(e);
		}
		/// <summary>
		/// 资源提取器
		/// </summary>
		public CHEER.Platform.GlobalRM.GlobalResourceManager getResourceManager() 
		{
			GlobalResourceManager _ResourceManager=(GlobalResourceManager)LSAManager.GetData("GlobalResourceManager") ;
			if (System.Object.ReferenceEquals(null, _ResourceManager))
				_ResourceManager= (GlobalResourceManager)LSAManager.GetData("ResourceManager");   
			return _ResourceManager;
		}
		private CHEER.Platform.GlobalRM.GlobalResourceManager _rm = null;
		private CHEER.Platform.GlobalRM.GlobalResourceManager getRM()
		{
			if( _rm == null)
			{
				_rm = getResourceManager();
			}
			return _rm ;
		}
		/// <summary>
		/// 取得多语言的提醒内容
		/// </summary>
		/// <param name="_classtype">类的类型</param>
		/// <param name="KeyName">关键字</param>
		/// <returns></returns>
		public string getAlert(System.Type _classtype,string KeyName)
		{
            return getRM().GetLanString(KeyName, "BU");
		}

        public string getAlert(string KeyName)
        {
            return getRM().GetLanString(KeyName, "BU");
        }
        public void ScriptStartup(string _scriptContent)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _scriptContent, true);
        }
        /// <summary>
        /// 写客户端脚本(RegisterStartupScript)
        /// </summary>
        /// <param name="_scriptContent"></param>
        public void ScriptBlock(string _scriptContent)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), _scriptContent, true);
        }
        /// <summary>
        /// 显示Alert提示信息
        /// </summary>
        /// <param name="_strMessage"></param>
        /// <remarks>modify by * 2004/04/22</remarks>
        public void ShowAlert(string _strMessage)
        {
            //ScriptStartup("alert('" + _strMessage.Replace("\r", @"\r").Replace("\n", @"\n").Replace("'", "\"") + "');");
            CheerUI.Alert.ShowInTop(_strMessage.Replace("\r", @"\r").Replace("\n", @"\n").Replace("'", "\""));
        }
		/// <summary>
		/// 取得多语言的界面控件内容
		/// </summary>
		/// <param name="_classtype">类的类型</param>
		/// <param name="KeyName">关键字</param>
		/// <returns></returns>
		public string getString(System.Type _classtype, string KeyName)
		{
            return getRM().GetLanString(KeyName, "UI");
		}

        public string getString(string _ID)
        {
            string str = getRM().GetLanString(_ID, "UI");
            return str;
        }
	}
}


