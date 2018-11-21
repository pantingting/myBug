using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using CHEER.BusinessLayer.ePersonnel.SystemConfig;
using CHEER.Common;
using CHEER.CommonLayer.ePersonnel.Schema;
using CHEER.PresentationLayer;

namespace CHEER.PresentationLayer.CommonUse
{
    public class ControlFiller : CHEERBasePage
    {


        /// <summary>
        /// 装载系统保留的某类型人事公用代码(启用状态的值)至下拉框
        /// </summary>
        /// <param name="ddlist"></param>
        /// <param name="type"></param>
        public static void FillPersonnelPublicCodeReserve(CheerUI.DropDownList ddlist, PersonnelPublicCodeType type)
        {
            FillPersonnelPublicCode(ddlist, ((int)type).ToString(), 1);
        }

        /// <summary>
        /// 装载人事公用代码(全部或禁用的部分)至下拉框
        /// </summary>
        /// <param name="ddlist"></param>
        /// <param name="typeID"></param>
        /// <param name="flag">0:仅禁用部分 1:仅启用部分 2:全部</param>
        public static void FillPersonnelPublicCode(CheerUI.DropDownList ddlist, string typeID, int flag)
        {
            //得到管理对象
            PersonnelPublicCodeManager _itemManager =
                (PersonnelPublicCodeManager)eHRPageServer.getPalaulObject(typeof(CHEER.BusinessLayer.ePersonnel.SystemConfig.PersonnelPublicCodeManager), "CHEER.BusinessLayer.ePersonnel.SystemConfig");
            DataSet _ds = null;
            switch (flag)
            {
                case 0:
                    _ds = _itemManager.GetCodeValues(typeID, false);
                    break;
                case 1:
                    _ds = _itemManager.GetCodeValues(typeID, true);
                    break;
                case 2:
                    _ds = _itemManager.GetCodeValues(typeID);
                    break;
            }
            _ds.Tables[0].DefaultView.Sort = PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_INDEXCODE_FULL + " ASC ";
            ddlist.DataSource = _ds.Tables[0].DefaultView;
            ddlist.DataTextField = PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMVALUE_FULL;
            ddlist.DataValueField = PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMID_FULL;
            ddlist.DataBind();
            ddlist.Items.Insert(0, new CheerUI.ListItem("", ""));
        }
    }
}