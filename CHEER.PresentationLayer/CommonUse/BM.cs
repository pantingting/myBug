using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHEER.Platform.DAL;
using CheerUI;

namespace CHEER.PresentationLayer.CommonUse
{
    public class BM
    {
        public static void BindDropdownList(DropDownList ddlBMTable)
        {
            ddlBMTable.Items.Clear();

            ddlBMTable.Items.Add(new CheerUI.ListItem("请选择编码表", ""));
            ddlBMTable.Items.Add(new CheerUI.ListItem("乡镇街道", "bm_xzjd"));
            ddlBMTable.Items.Add(new CheerUI.ListItem("居委会", "bm_jwh"));
            ddlBMTable.Items.Add(new CheerUI.ListItem("企业大类", "bm_qydl"));
            ddlBMTable.Items.Add(new CheerUI.ListItem("企业小类", "bm_qylx"));
            ddlBMTable.Items.Add(new CheerUI.ListItem("管理网格", "bm_glwg"));

        }
    }
}