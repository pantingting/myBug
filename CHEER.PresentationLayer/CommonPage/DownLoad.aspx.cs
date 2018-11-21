using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.ePersonnel;
using CHEER.Common;
using CHEER.CommonLayer.ePersonnel.Data;
using CHEER.PresentationLayer;

namespace CHEER.PresentationLayer.CommonPage
{
    public partial class DownLoad : System.Web.UI.Page
    {
        private CHEERBasePage basePage = new CHEERBasePage();//因为弹出页面中Session容易丢失，故无法继承基类
        public bool Flag
        {
            get
            {
                if (!IsPostBack)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strfilename = Request.QueryString["FILENAME"];
                string templateValue = Request.QueryString["TEMPLATEVALUE"];
                ExcelDataExport(strfilename, templateValue);
            }
        }

        private void ExcelDataExport(string fileName, string tempValue)
        {
            string _strPath = Server.MapPath("") + "/DownLoadDir/";
            string _fileName = fileName;
            string _templateValue = tempValue;
            _strPath += _fileName;
            _strPath = _strPath.Replace('\\', '/');
            this.EnableViewState = false;
            if (_templateValue == "" || _templateValue == null)
            {
                this.TranFileToClient(_fileName);
            }
            else
            {
                BLOBFileData _data = new BLOBFileData();
                BLOBFileManager _manager = (BLOBFileManager)eHRPageServer.GetPalauObject(typeof(BLOBFileManager));
                _data = _manager.GetDataByID(_templateValue);
                int count = _data.Value.Length;
                byte[] _byte = new byte[count];
                _byte = Convert.FromBase64String(_data.Value);
                this.TranDataToClient(_fileName, _byte);
            }
        }
        private void TranFileToClient(string strfilename)
        {
            string path = strfilename;
            System.IO.FileInfo file = new System.IO.FileInfo(path);
            // 从缓冲区中清除当前输出内容
            Response.Clear();
            // 添加头信息，为"文件下载/另存为"对话框指定默认文件名
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            // 添加头信息，指定文件大小，让浏览器能够显示下载进度
            Response.AddHeader("Content-Length", file.Length.ToString());
            // 指定返回的是一个不能被客户端读取的流，必须被下载
            Response.ContentType = "application/octet-stream";
            // 把文件流发送到客户端
            Response.WriteFile(file.FullName);
            // 停止页面的执行
            Response.End();
        }
        private void TranDataToClient(string strfilename, byte[] templateValue)
        {
            string path = strfilename;
            int count = templateValue.Length;
            // 从缓冲区中清除当前输出内容
            Response.Clear();
            // 添加头信息，为"文件下载/另存为"对话框指定默认文件名
            Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(path));
            // 添加头信息，指定文件大小，让浏览器能够显示下载进度
            Response.AddHeader("Content-Length", count.ToString());
            // 指定返回的是一个不能被客户端读取的流，必须被下载
            Response.ContentType = "octet-stream";
            // 把文件流发送到客户端
            Response.OutputStream.Write(templateValue, 0, count);
            // 停止页面的执行
            Response.End();
        }
    }
}