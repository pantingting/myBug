using CHEER.PresentationLayer;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CHEER.PresentationLayer.Organize.StdOrg
{
    public partial class QueryEmpIdName : BaseControls
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
            if (!IsPostBack)
            {
                CheerUI.PageContext.RegisterStartupScript("C('" + img.ClientID + "').el.on('click',function(){" + detailWindow.GetSaveStateReference(txtNo.ClientID, txtName.ClientID, txtPersonId.ClientID)
                    + detailWindow.GetShowReference(base.getString("ZGAIA00478"), Unit.Pixel(400), Unit.Pixel(400)) + "});");
                CheerUI.PageContext.RegisterStartupScript("C('" + txtNo.ClientID + "').on('focus',function(){C('" + img.ClientID + "').el.click();});");
            }
        }

        protected void LoadData()
        {
            txtNo.Label = LblNoText;
            txtName.Label = LblNameText;
            if (!String.IsNullOrEmpty(Properties))
            {
                userForm.RecoverPropertiesFromJObject(JObject.Parse(Properties));
            }
        }

        public CheerUI.Image Img
        {
            get
            {
                return img;
            }
        }

        public CheerUI.TextBox TextPersonId
        {
            get
            {
                return txtPersonId;
            }
        }

        public CheerUI.TextBox TextNo
        {
            get
            {
                return txtNo;
            }
        }

        public CheerUI.TextBox TextName
        {
            get
            {
                return txtName;
            }
        }

        public string Properties
        {
            get;
            set;
        }

        public string LblNoText
        {
            get;
            set;
        }

        public string LblNameText
        {
            get;
            set;
        }
    }

}