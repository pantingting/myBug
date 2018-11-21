using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.PresentationLayer;
using System.Data;
using System.Text;
using Infragistics.Documents.Excel;
using System.IO;

namespace CHEER.PresentationLayer.CommonPage
{
    public partial class EXPORT : System.Web.UI.Page
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
        private void Page_Load(object sender, System.EventArgs e)
        {
            //this.LblDownLoad.Text = basePage.getString("ZGAIA00072");// 
            //this.lnkDownLoad.Text = basePage.getString("ZGAIA00541");// 
            if (!IsPostBack)
            {
                //this.Image.ImageUrl = "../Image/download 16.gif";
                ExcelDataExport();
            }
        }
        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }
        private void InitializeComponent()
        {
            //this.lnkDownLoad.Click += new System.EventHandler(this.lnkDownLoad_Click);
            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion
        private void lnkDownLoad_Click(object sender, System.EventArgs e)
        {
            this.ExcelDataExport();
        }

        /// <summary>
        /// 支持DataTable,DataSet
        /// </summary>
        private void ExcelDataExport()
        {
            string sessionName = Request.QueryString["SESSIONNAME"];
            if (sessionName != null)
            {
                if (Session[sessionName] != null)
                {
                    DataTable dt = new DataTable();
                    if (Session[sessionName] is DataSet)
                    {
                        dt = ((DataSet)Session[sessionName]).Tables[0];
                    }
                    else
                    {
                        dt = (DataTable)Session[sessionName];
                    }
                    string fileName = Request.QueryString["TYPE"].ToString()
                            + DateTime.Now.ToString("yyyyMMddHHmmss")
                            + "_" + Request.QueryString["UserID"].ToString();
                    ExportExcel(fileName, dt);
                }
            }
            else
            {
                basePage.ShowAlert("获取文件失败，请重试");
            }
        }
        public void ExportExcel(string fileName, DataTable dataSource)
        {
            try
            {
                Workbook Wb = new Workbook(WorkbookFormat.Excel2007);
                Worksheet Ws = Wb.Worksheets.Add("Sheet1");

                for (int column = 0; column < dataSource.Columns.Count; column++)
                {
                    Ws.Rows[0].Cells[column].Value = (dataSource.Columns[column].ColumnName);
                    Ws.Rows[0].Cells[column].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                }

                for (int row = 0; row < dataSource.Rows.Count; row++)
                {
                    for (int column = 0; column < dataSource.Columns.Count; column++)
                    {
                        Ws.Rows[row + 1].Cells[column].Value = dataSource.Rows[row][column];
                    }
                }

                string Path = Server.MapPath("~") + "\\DownLoadDir\\" + Guid.NewGuid().ToString() + ".xlsx";
                //string filename = Guid.NewGuid().ToString() + ".xls";

                if (File.Exists(Path))
                {
                    File.SetAttributes(Path, FileAttributes.Normal);
                    File.Delete(Path);
                }
                Wb.Save(Path);

                FileInfo fileInfo = new FileInfo(Path);
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xlsx");
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(fileInfo.FullName);
                Response.End();
                Response.Flush();
                Response.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //System.Web.UI.WebControls.GridView navPage = null;
            //Response.Clear();
            //Response.Buffer = false;
            //Response.Charset = "utf-8";
            //Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName + ".xls");
            //Response.ContentEncoding = System.Text.Encoding.UTF8;
            //Response.ContentType = "application/ms-excel";
            //Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            //System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            //HtmlTextWriter oHtmlTextWriter = new HtmlTextWriter(oStringWriter);
            //DataTable dt = dataSource;
            //navPage = new System.Web.UI.WebControls.GridView();
            //navPage.DataSource = dataSource;
            //navPage.AllowPaging = false;
            //navPage.DataBind();
            //navPage.RenderControl(oHtmlTextWriter);// 数字字符导Excel后转Text格式，否则前面的 0 字符会自动舍弃
            //Response.Write(AddExcelHead());
            //Response.Write(oStringWriter.ToString());
            //Response.Write(AddExcelbottom());
            //Response.Flush();
            //Response.End();
        }
        public static string AddExcelHead()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
            sb.Append(" <head>");
            sb.Append(" <!--[if gte mso 9]><xml>");
            sb.Append("<x:ExcelWorkbook>");
            sb.Append("<x:ExcelWorksheets>");
            sb.Append("<x:ExcelWorksheet>");
            sb.Append("<x:Name></x:Name>");
            sb.Append("<x:WorksheetOptions>");
            sb.Append("<x:Print>");
            sb.Append("<x:ValidPrinterInfo />");
            sb.Append(" </x:Print>");
            sb.Append("</x:WorksheetOptions>");
            sb.Append("</x:ExcelWorksheet>");
            sb.Append("</x:ExcelWorksheets>");
            sb.Append("</x:ExcelWorkbook>");
            sb.Append("</xml>");
            sb.Append("<![endif]-->");
            sb.Append(" </head>");
            sb.Append("<body>");
            return sb.ToString();
        }
        public static string AddExcelbottom()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("</body>");
            sb.Append("</html>");
            return sb.ToString();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
    }
}


