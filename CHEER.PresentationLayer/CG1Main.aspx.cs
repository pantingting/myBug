using CHEER.BusinessLayer.Security.Portal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace CHEER.PresentationLayer
{
    public partial class CG1Main : CHEERBasePage
    {
        public class NavMenu
        {
            public string id { get; set; }
            public string text { get; set; }
            public string url { get; set; }
            public List<NavMenu> subMenus { get; set; } = new List<NavMenu>();
        }

        protected string USERNAME = "";
        protected string BRANCHNAME = "?";
        protected string remainMessage = "";
        protected List<NavMenu> menus = new List<NavMenu>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string language = "ZH-CN";
                HttpCookie clan = Request.Cookies["language"];
                if (clan != null && clan.Value != "")
                {
                    language = clan.Value;
                }

                HttpCookie mm = new HttpCookie("language", language);
                mm.Expires = DateTime.Now.AddYears(10);
                Response.Cookies.Add(mm);

                LoadMenu();
                USERNAME = "[" + InfomationPackage.UserName + "],你好";

                CHEER.Common.Business.ExecuteSQL(broker =>
                {
                    broker.ExecuteSQLForDst(
                        $@"SELECT UNITNAME FROM ORGSTDSTRUCT WHERE UNITID = '{ InfomationPackage.BranchId }' ").Tables[0].AsEnumerable().ToList().ForEach(row =>
                        {
                            BRANCHNAME = row["UNITNAME"].ToString();
                        });
                });
            }
        }

        private void LoadMenu()
        {
            FunctionLoader FL = new FunctionLoader();
            DataTable _DT = FL.GetAccessForeMenu(InfomationPackage.UserID);
            DataSet ds = _DT.DataSet;
            ds.Relations.Add("TreeRelation", ds.Tables[0].Columns["MENUID"], ds.Tables[0].Columns["PMENUID"], false);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (row["PMENUID"].ToString() == "000000")
                {
                    var navMenu = new NavMenu()
                    {
                        id = row["MENUID"].ToString(),
                        text = row["MENUNAME"].ToString(),
                        url = row["URL"].ToString()
                    };
                    menus.Add(navMenu);
                    ResolveSubMenu(row, navMenu);
                }
            }
        }

        private void ResolveSubMenu(DataRow dataRow, NavMenu navMenu)
        {
            DataRow[] rows = dataRow.GetChildRows("TreeRelation");
            if (rows.Length > 0)
            {
                foreach (DataRow row in rows)
                {
                    var subNavMenu = new NavMenu()
                    {
                        id = row["MENUID"].ToString(),
                        text = row["MENUNAME"].ToString(),
                        url = row["URL"].ToString()
                    };
                    navMenu.subMenus.Add(subNavMenu);
                    //ResolveSubMenu(row, subNavMenu);
                }
            }
        }
    }
}