using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.PresentationLayer;
using Newtonsoft.Json.Linq;

namespace CHEER.PresentationLayer.Controls
{
    public partial class PersonSelect : BaseControls
    {
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enabled
        {
            get
            {
                return this.txtName.Enabled;
            }
            set
            {
                this.txtName.Enabled = value;
            }
        }

        /// <summary>
        /// 是否必填
        /// </summary>
        public bool Required
        {
            get
            {
                return this.txtName.Required;
            }
            set
            {
                this.txtName.Required = value;
            }
        }

        /// <summary>
        /// 是否显示必填提示星号
        /// </summary>
        public bool ShowRedStar
        {
            get
            {
                return this.txtName.ShowRedStar;
            }
            set
            {
                this.txtName.ShowRedStar = value;
            }
        }

        public CheerUI.Window detailWindow
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
            if (!IsPostBack)
            {
                if (Enabled)
                {
                    CheerUI.PageContext.RegisterStartupScript("C('" + img.ClientID + "').el.on('click',function(){" + detailWindow.GetSaveStateReference(txtName.ClientID, txtPersonId.ClientID, txtWNO.ClientID)
                        + detailWindow.GetShowReference(base.getBaseUrl() + "Organize/SecControls/PersonShowPage.aspx?RightID=" + RightID + "&FILTER=" + Filter + "&ORGID=" + OrgID, base.getString("ZGAIA00478"), Unit.Pixel(800), Unit.Pixel(500)) + "});");
                    CheerUI.PageContext.RegisterStartupScript("C('" + txtName.ClientID + "').on('focus',function(){C('" + img.ClientID + "').el.click();});");
                }
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

        /// <summary>
        /// 工号
        /// </summary>
        public string WNO
        {
            set
            {
                this.txtWNO.Text = value;
            }
            get
            {
                return this.txtWNO.Text;
            }
        }

        public CheerUI.TextBox TextPersonId
        {
            get
            {
                return this.txtPersonId;
            }
        }

        public CheerUI.TextBox TextName
        {
            get
            {
                return this.txtName;
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