using CHEER.BusinessLayer.Security.Portal;
using CheerUI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace CHEER.PresentationLayer
{
    public partial class Index : CHEERBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMenu();
                userName.Text = "[" + InfomationPackage.UserName + "],你好";

                CHEER.Common.Business.ExecuteRedis(redis =>
                {
                    redis.GetDatabase().Publish("SYNCAPPLICATION", "");
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
                    var node = new CheerUI.TreeNode()
                    {
                        NodeID = row["MENUID"].ToString(),
                        Text = row["MENUNAME"].ToString(),
                        Attributes = new Newtonsoft.Json.Linq.JObject
                        {
                            { "url", row["URL"].ToString() }
                        }
                    };
                    CHEER.Common.Business.ExecuteStatement(() =>
                    {
                        if (row["ICONFONT"].ToString() != "")
                        {
                            node.IconFont = IconFontHelper.GetFromName(row["ICONFONT"].ToString());
                        }
                    });
                    treeMenu.Nodes.Add(node);
                    ResolveSubMenu(row, node);
                }
            }
        }

        private void ResolveSubMenu(DataRow dataRow, CheerUI.TreeNode treeNode)
        {
            DataRow[] rows = dataRow.GetChildRows("TreeRelation");
            if (rows.Length > 0)
            {
                foreach (DataRow row in rows)
                {
                    var subTreeNode = new CheerUI.TreeNode()
                    {
                        NodeID = row["MENUID"].ToString(),
                        Text = row["MENUNAME"].ToString(),
                        Attributes = new Newtonsoft.Json.Linq.JObject
                        {
                            { "url", row["URL"].ToString() }
                        }
                    };
                    treeNode.Nodes.Add(subTreeNode);
                    ResolveSubMenu(row, subTreeNode);
                }
            }
        }
    }
}
