
using CHEER.BusinessLayer.ePersonnel;
using CHEER.Common;
using CHEER.CommonLayer.ePersonnel.Data;
using System;

namespace CHEER.PresentationLayer.CommonPage
{
	public partial class ShowPhoto : System.Web.UI.Page
    {
        private void Page_Load(object sender, System.EventArgs e)
        {
            //使用photoiD从文件中获得二进制文件，判断其类型，并专程流输出。
            string _photoID = Request["photoID"];
            if (_photoID != null || _photoID.Length == 0)
            {
                try
                {
                    //得到大文件数据
                    PersonPhotoManager _manager = (PersonPhotoManager)eHRPageServer.GetPalauObject(typeof(PersonPhotoManager));
                    PersonPhotoData _data = _manager.Get(_photoID);
                    if (_data == null || _data.PHOTOVALUE == null || _data.PHOTOVALUE.Length == 0)
                    {
                        Response.Clear();
                        Response.End();
                    }
                    else
                    {
                        int _count = _data.PHOTOVALUE.Length;
                        byte[] _bytes = new byte[_count];
                        _bytes = _data.PHOTOVALUE;
                        //根据图片类型输出图片内容
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Expires = 0;
                        Response.Charset = "utf-8";
                        string _strFileName = Request.QueryString["FileName"];
                        switch ((BLOBFileType)(Convert.ToInt32(_data.PHOTOTYPE)))
                        {
                            case BLOBFileType.Jpg:
                                Response.ContentType = "image/JPEG";
                                break;
                            case BLOBFileType.Bmp:
                                Response.ContentType = "image/BMP";
                                break;
                            default:
                                return;
                        }
                        Response.OutputStream.Write(_bytes, 0, _count);
                        Response.End();
                    }
                }
                catch
                {
                }
            }
        }
    }
}