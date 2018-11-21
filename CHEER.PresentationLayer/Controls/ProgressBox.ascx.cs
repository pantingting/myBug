using CHEER.PresentationLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CHEER.PresentationLayer.Controls
{
    public class ResponseFilter : Stream
    {
        private Stream _responseStream;
        private MemoryStream _memoryStream;

        internal ResponseFilter(Stream responseStream)
        {
            _responseStream = responseStream;
            _memoryStream = new MemoryStream();
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            _memoryStream.Flush();
        }

        public override long Length
        {
            get { return 0; }
        }

        public override long Position
        {
            get { return 0; }
            set { }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return 0;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return 0;
        }

        public override void SetLength(long value)
        {
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _memoryStream.Write(buffer, offset, count);
        }

        public override void Close()
        {
            StringBuilder sb = new StringBuilder();
            var allHtml = GetCompleteHtml();
            allHtml = allHtml.Substring(0, allHtml.IndexOf("</CheerUI.Response>"));
            allHtml = allHtml.Substring(allHtml.IndexOf("<CheerUI.Response>") + "<CheerUI.Response>".Length);

            sb.Append(allHtml);

            string responseText = sb.ToString();

            // 从输出流创建TextWriter
            TextWriter writer = new StreamWriter(_responseStream, Encoding.UTF8);

            writer.Write(responseText);

            // 输出
            writer.Flush();
            writer.Dispose();
            base.Close();
            _responseStream.Close();
        }

        public string GetCompleteHtml()
        {
            string _completeHtml;

            _memoryStream.Position = 0;
            using (TextReader reader = new StreamReader(_memoryStream))
            {
                _completeHtml = reader.ReadToEnd();
            }

            return _completeHtml;
        }

    }

    public partial class ProgressBox : BaseControls
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["GETPROGRESSVALUE"] != null)
            {
                ResponseValue();
            }
            else
            {
                Session.Remove(ID + "_ProcessBox_Value");
                Session.Remove(ID + "_ProcessBox_Max");
                Session.Remove(ID + "_ProcessBox_Alert");
            }
        }

        /// <summary>
        /// 线程结束后需要执行的JS函数
        /// </summary>
        public string postBackFunction
        {
            get;
            set;
        }

        /// <summary>
        /// Ajax请求的Max值,通常为数组长度
        /// </summary>
        public int Max
        {
            get
            {
                return Session[ID + "_ProcessBox_Max"] == null ? 0 : int.Parse(Session[ID + "_ProcessBox_Max"].ToString());
            }
            set
            {
                Session[ID + "_ProcessBox_Max"] = value;
            }
        }

        /// <summary>
        /// Ajax请求的Value值,通常为遍历数组时的索引值
        /// </summary>
        public int Value
        {
            get
            {
                return Session[ID + "_ProcessBox_Value"] == null ? 0 : int.Parse(Session[ID + "_ProcessBox_Value"].ToString());
            }
            set
            {
                Session[ID + "_ProcessBox_Value"] = value;
            }
        }

        /// <summary>
        /// 前台Ajax请求的JS脚本
        /// </summary>
        /// <returns></returns>
        public string JSGoFunction()
        {
            return "goRequestValueMax();";
        }

        /// <summary>
        /// 结束请求显示Alert
        /// </summary>
        /// <returns></returns>
        public string ShowAlertScript
        {
            get
            {
                return Session[ID + "_ProcessBox_Alert"] == null ? "" : Session[ID + "_ProcessBox_Alert"].ToString();
            }
            set
            {
                Session[ID + "_ProcessBox_Alert"] = value;
            }
        }

        /// <summary>
        /// 向客户端发回截取之后的Value,Max值
        /// </summary>
        private void ResponseValue()
        {
            string response = "<CheerUI.Response>{'value':'" + Value + "','max':'" + Max + "'";
            if (Value == Max && Value > 0)
            {
                response += ",'alert':'" + ShowAlertScript + "'";
            }
            response += "}</CheerUI.Response>";
            HttpContext.Current.Trace.IsEnabled = false;
            HttpContext.Current.Response.ContentType = "text/html";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            ResponseFilter filter = new ResponseFilter(HttpContext.Current.Response.Filter);
            HttpContext.Current.Response.Filter = filter;
            HttpContext.Current.Response.Write(response);
            HttpContext.Current.Response.Flush();

            if (Value == Max)
            {
                Session.Remove(ID + "_ProcessBox_Value");
                Session.Remove(ID + "_ProcessBox_Max");
                Session.Remove(ID + "_ProcessBox_Alert");
            }
        }
    }
}