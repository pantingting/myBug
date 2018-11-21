using CHEER.PresentationLayer;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CHEER.PresentationLayer.Organize.SecControls
{
    public partial class PersonSelect : BaseControls
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
            if (!IsPostBack)
            {
                CheerUI.PageContext.RegisterStartupScript("C('" + img.ClientID + "').el.on('click',function(){" + detailWindow.GetSaveStateReference(txtName.ClientID, txtPersonId.ClientID)
                    + detailWindow.GetShowReference(base.getBaseUrl() + "Organize/SecControls/PersonShowPage.aspx?RightID=" + RightID + "&FILTER=" + Filter + "&ORGID=" + OrgID, base.getString("ZGAIA00478"), Unit.Pixel(800), Unit.Pixel(500)) + "});");
                CheerUI.PageContext.RegisterStartupScript("C('" + txtName.ClientID + "').on('focus',function(){C('" + img.ClientID + "').el.click();});");
            }
        }

        string _filter = "NO";
        public string Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }
        string _orgID = "";
        public string OrgID
        {
            get
            {
                return _orgID;
            }
            set
            {
                _orgID = value;
            }
        }

        string _txtRightId = "";
        public string RightID
        {
            set
            {
                _txtRightId = value;
            }
            get
            {
                return _txtRightId;
            }
        }

        public CheerUI.TextBox TextPersonId
        {
            get
            {
                return txtPersonId;
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

        protected void LoadData()
        {
            if (!String.IsNullOrEmpty(Properties))
            {
                userForm.RecoverPropertiesFromJObject(JObject.Parse(Properties));
            }
        }
    }
}