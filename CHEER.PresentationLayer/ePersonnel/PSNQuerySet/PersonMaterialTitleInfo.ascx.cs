using CHEER.BusinessLayer.ePersonnel.PersonnelBaseInfo;
using CHEER.Common;
using CHEER.CommonLayer.ePersonnel;
using CHEER.CommonLayer.ePersonnel.Data;
using CHEER.PresentationLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CHEER.PresentationLayer.ePersonnel.PSNQuerySet
{
    public partial class PersonMaterialTitleInfo : BaseControls
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            //设置页面控件的多语言信息
            txtTrueName.Text = base.getString("ZGAIA00208");
            txtAttendOnDate.Text = base.getString("ZGAIA00207");
            txtEmployeeID.Text = base.getString("ZGAIA00209");
            txtUnitName.Text = base.getString("ZGAIA00479");
       
            string personid = Session["psnPERSONID"].ToString();          
         
            LoadPersonInfo(personid);
          
        }

        public string GetBaseUrl()
        {
            return CHEERBasePage.GetBaseURL(Request);
        }
        public void LoadPersonInfo(string personID)
        {
            //			//获得公用信息管理类，目前只有人事主档表中的数据，先使用人事主档管理类
            //			AccessionInfoManager _accessManager = (AccessionInfoManager)eHRPageServer.GetPalauObject(typeof(AccessionInfoManager));
            //			AccessionInfoViewData _accessView = _accessManager.GetAccessionViewInfo(personID);
            //			AccessionInfoData _accessData = _accessView.AccessionViewInfo;
            //获得公用信息管理类，目前只有人事主档表中的数据，先使用人事主档管理类
            PersonAccountManager _personManager = (PersonAccountManager)eHRPageServer.GetPalauObject(typeof(PersonAccountManager));
            AccountInfoViewData _accuontView = _personManager.GetAccountViewInfo(personID);
            PersonAccountData _data = _accuontView.AccountInfoData;
            if (_data != null)
            {
                txtEmployeeID.Text = _data.EmployeeID.ToString();
                txtUnitName.Text = _data.BranchID.ToString();
            }
            txtTrueName.Text = _data.TrueName;
            txtAttendOnDate.Text = DataProcessor.DateTimeToShortString(_data.AttendOnDate);
            //如果没有照片，则不显示照片
            if (_data.PhotoID == null || _data.PhotoID.Length == 0)
            {
                imgPhoto.ImageUrl = GetBaseUrl() + "image/DefaultPersonPhoto.jpg";
            }
            else
            {
                //imgPhoto.Visible = true;
                imgPhoto.ImageUrl = GetBaseUrl() + "CommonPage/ShowPhoto.aspx?photoID=" + _data.PhotoID;
                //imgPhoto.ImageUrl = GetBaseUrl() + "image/DefaultPersonPhoto.jpg";
            }
            

            ViewState[AppConstants.QUERYSTRING_PERSONID] = personID;
        }

    }
}