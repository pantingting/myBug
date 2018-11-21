using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.Common;
using CHEER.Common.AppConstants;
using CHEER.PresentationLayer;

namespace CHEER.PresentationLayer.Controls
{

    /// <summary>
    /// 
    /// </summary>
    public interface IUploadPage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_ds"></param>
        void BindDataForUpLoad(DataSet _ds);
    }

    public partial class ucUploadFile : CHEER.PresentationLayer.BaseControls
    {
        #region 定义
        /// <summary>
        /// 
        /// </summary>
        private const string UploadFileHelperURL = "CHEER.BusinessLayer.ePayroll.UploadFileHelper";
        /// <summary>
        /// 
        /// </summary>
        private const string PayUpLoadFielLogBEURL = "CHEER.BusinessLayer.PersonalPayrollDataManage.PayUpLoadFielLogBE";
        /// <summary>
        /// 
        /// </summary>
        private const string PayUpLoadFielLogBLURL = "CHEER.BusinessLayer.PersonalPayrollDataManage.PayUpLoadFielLogBL";
        //上传文件名
        string strPostFileName = null;
        //扩展名
        string strExtName = null;
        //文件存储路径
        string savepath = null;
        /// <summary>
        /// 
        /// </summary>
        DateTime dateNow;
        /// <summary>
        /// 
        /// </summary>
        public UploadModule Module
        {
            get
            {
                if (txtUploadModule.Text.Trim() == "")
                {
                    txtUploadModule.Text = UploadModule.other.GetHashCode().ToString();
                }
                return ((UploadModule)int.Parse(txtUploadModule.Text));
            }
            set
            {
                txtUploadModule.Text = ((int)value).ToString();
            }
        }

        public bool Hidden
        {
            set {
                this.formUplodFile.Hidden = value;
            }
        }
        public Unit Height
        {
            set {
                this.formUplodFile.Height = value;
            }
        }

        public Unit Width
        {
            set {
                this.formUplodFile.Width = value;
            }
        }

        public CheerUI.Form FormUploadFile
        {
            get {
                return this.formUplodFile;
            }
        }

        public string Padding
        {
            set {
                this.formUplodFile.BodyPadding = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UploadType Type
        {
            get
            {
                return ((UploadType)int.Parse(txtUploadType.Text));
            }
            set
            {
                txtUploadType.Text = ((int)value).ToString();
            }
        }
        /// <summary>
        /// 文件名缩写
        /// </summary>
        public string strShortName
        {
            get
            {
                return txtShortName.Text;
            }
            set
            {
                txtShortName.Text = value;
            }
        }
        /// <summary>
        /// 文件大小
        /// </summary>
        public double ContentLength
        {
            get
            {
                return double.Parse(txtContentLength.Text);
            }
            set
            {
                txtContentLength.Text = value.ToString();
            }
        }
        #endregion

        private void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面
            if (!IsPostBack)
                Face_Init();
        }
        /// <summary>
        /// 按钮图片设置
        /// </summary>
        private void Face_Init()
        {
            btnImput.Text = base.getString("ZGAIA01847");
            myFile.Label = base.getString("ZGAIA02236");
        }

        protected void btnImput_Click(object sender, EventArgs e)
        {
            try
            {
                //上传文件
                UploadFile();
                UploadCommon _UploadCommon = (UploadCommon)eHRPageServer.GetPalauObject(typeof(UploadCommon));
                DataSet ds = _UploadCommon.GetUploadData(savepath);
                if (Module.ToString() == null || Type.ToString() == null)
                {
                    throw new Exception(((CHEERBasePage)Page).getAlert("ZGAIA03626"));//程序参数设置不正确！
                }
                switch (Module)
                {
                    case UploadModule.ePayroll:
                        CHEER.BusinessLayer.ePayroll.UploadFileHelper _ePayrollUploadFileHelper = (
                            CHEER.BusinessLayer.ePayroll.UploadFileHelper)eHRPageServer.GetPalauObject
                            (typeof(CHEER.BusinessLayer.ePayroll.UploadFileHelper));
                        _ePayrollUploadFileHelper.ClientIP = Request.UserHostAddress;
                        _ePayrollUploadFileHelper.Operator = Page.Session[SystemAppConstants.SESSION_USERNAME].ToString();
                        ds = _ePayrollUploadFileHelper.CheckData(Type, ds);
                        break;
                    case UploadModule.eAttendance:
                        break;
                }
                if (ds == null || ds.Tables == null || ds.Tables[0] == null)
                {
                    CheerUI.Alert.ShowInTop(getAlert("ZGAIA03253"));
                }
                ((IUploadPage)this.Parent.Page).BindDataForUpLoad(ds);
                CheerUI.PageContext.RegisterStartupScript("Ext.get('" + myFile.ClientID + "-button-fileInputEl').dom.value='';Ext.get('" + myFile.ClientID + "-inputEl').dom.value='';F('" + formUplodFile.ClientID + "').setVisible(false);");
            }
            catch
            {
                CheerUI.Alert.ShowInTop(getAlert("ZGAIA02000"));
            }
        }


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        private bool UploadFile()
        {
            try
            {
                if (myFile.PostedFile != null && myFile.PostedFile.FileName != "")
                {
                    
                    //得到上传文件所需要的各种文件名称格式
                    GetFileName();
                    //检查文件
                    CheckFile();
                    //保存文件					
                    SaveUploadFile();
                    return true;
                }
                else
                {
                    throw new Exception(((CHEERBasePage)Page).getAlert("ZGAIA02008"));//文件信息不正确！
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message + ((CHEERBasePage)Page).getAlert("ZGAIA01995"));//上传不成功，请重试!
            }
        }

        #region 得到上传文件所需要的各种文件名称格式
        /// <summary>
        /// //得到上传文件所需要的各种文件名称格式
        /// </summary>
        private void GetFileName()
        {
            string[] shortFileNameS = null;
            strPostFileName = myFile.PostedFile.FileName;
            shortFileNameS = strPostFileName.Split(new char[] { '\\' });
            if (shortFileNameS.Length > 0)
                strShortName = shortFileNameS[shortFileNameS.Length - 1];
            else
                strShortName = strPostFileName;
            strExtName = strPostFileName.Substring(strPostFileName.LastIndexOf("."));
            //文件大小
            ContentLength = myFile.PostedFile.ContentLength;
        }
        #endregion

        #region 检查文件
        /// <summary>
        /// 检查文件
        /// </summary>
        /// <returns></returns>
        private void CheckFile()
        {
            try
            {
                //检查文件名称
                CheckFileName();
                //检查文件是否已经上传过
                //CheckFileExist();
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        /// <summary>
        /// 检查文件名
        /// </summary>
        /// <returns></returns>
        private void CheckFileName()
        {
            try
            {
                if (strExtName != ".xls" && strExtName != ".xlsx")
                {
                    throw new Exception(((CHEERBasePage)Page).getAlert("ZGAIA03627"));//文件类型不对，扩展名应为.xls！
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        /// <summary>
        /// 检查文件是否已经上传过
        /// </summary>
        /// <returns></returns>
        private void CheckFileExist()
        {
            //			try
            //			{
            //				PayUpLoadFielLogBL _bl=new PayUpLoadFielLogBL();
            //				if(_bl.CheckUpLoadFielLogExist(strShortName,myFile.PostedFile.ContentLength)==true)
            //					throw new Exception("同样的文件已经上传过了！");
            //			}
            //			catch
            //			{
            //				throw new Exception("同样的文件已经上传过了！");
            //			}
        }
        #endregion
        #region 保存文件
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <returns></returns>
        private bool SaveUploadFile()
        {
            string strNewFileName = null;
            string strExtName = null;
            try
            {
                strExtName = myFile.PostedFile.FileName.Substring(myFile.PostedFile.FileName.LastIndexOf("."));
                //自动根据日期和文件大小不同为文件命名,确保文件名不重复
                dateNow = DateTime.Now;
                strNewFileName = dateNow.DayOfYear.ToString() + myFile.PostedFile.ContentLength.ToString();
                //用Server.MapPath()取当前文件的绝对目录.
                savepath = Server.MapPath("") + "/../UpLoadDir/";
                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }
                savepath = savepath + strNewFileName + strExtName;
                savepath = savepath.Replace("\\", "/");
                myFile.PostedFile.SaveAs(savepath);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}