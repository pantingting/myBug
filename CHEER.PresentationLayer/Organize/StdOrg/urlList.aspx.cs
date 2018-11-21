using CHEER.BusinessLayer.Organize;
using CHEER.BusinessLayer.Organize.ManageUnit;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.CommonLayer.Organize.Data.STDOrganize;
using CHEER.Platform.DAL;
using CHEER.PresentationLayer.CommonUse;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CHEER.PresentationLayer.Organize.StdOrg
{
    public partial class urlList : CHEERBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DisplayInfo();
            }
        }
        protected void DisplayInfo()
        {
            shake.Text = getBaseUrl() + "eFore/Announcement/index.html";
            payremind.Text = getBaseUrl() + "/eFore/PayReminders/index.html";
            map.Text = getBaseUrl() + "eFore/Periphery/index.html";
            supplylist.Text = getBaseUrl() + "eFore/Suppliers/index.html";
            repair.Text = getBaseUrl() + "eFore/Warranty/index.html";
            bookHouse.Text = getBaseUrl() + "eFore/Lease/index.html ";
        }


































    }
}