using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using CheerUI;
using CHEER.PresentationLayer;

namespace CHEER.PresentationLayer.CommonUse
{
    /// <summary>
    /// 执行Excel导出的类
    /// </summary>
    public class GridDataExportor : System.Web.UI.Page
    {
        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="pageRequest"></param>
        public GridDataExportor(HttpRequest pageRequest)
        {
            request = pageRequest;
        }
        #region 执行Excel导出
        private HttpRequest request = null;
        /// <summary>
        /// 
        /// </summary>
        public const string Session_DownloadFileName = "DownloadFileName";
        /// <summary>
        /// 获得应用程序的根路径
        /// </summary>
        /// <returns></returns>
        public string GetBaseUrl()
        {
            return CHEERBasePage.GetBaseURL(request);
        }

        public string ExportExcelData(string exportFileName, DataSet _data)
        {
            return ExportExcelData(exportFileName, null, _data, null);
        }

        /// <summary>
        ///  执行Excel导出
        /// </summary>
        /// <param name="exportFileName">导出的文件名</param>
        /// <param name="_grid">grid</param>
        /// <param name="_data">DataSet</param>
        /// <param name="_swapdata"></param>
        /// <returns>现在数据的script</returns>
        public string ExportExcelData(string exportFileName, Grid _grid, DataSet _data, ArrayList _swapdata)
        {
            string serverPath = this.GetBaseUrl();
            string _strPath = Server.MapPath(Context.Request.ApplicationPath) + "/DownLoadDir/";
            if (!Directory.Exists(_strPath))
            {
                Directory.CreateDirectory(_strPath);
            }
            if (exportFileName == null || exportFileName.Trim() == "")
            {
                exportFileName = DateTime.Now.ToString("s", DateTimeFormatInfo.InvariantInfo).Replace("-", "").Replace(":", "").Replace("T", "");
            }
            //给导出文件添加用户标识
            else
            {
                string UID = Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERID] == null ? Guid.NewGuid().ToString() : Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERID].ToString().Replace("-", "_");
                exportFileName += "_" + UID;
            }
            string _fileName = exportFileName + ".xlsx";
            _strPath += _fileName;
            _strPath = _strPath.Replace('\\', '/');
            //新UI中一般不使用CheckBox 
            //第一个CheckBox列的索引号
            //int chkBoxColumnIndex = -1;
            //第一个CheckBox列是否是可见的
            //bool chkBoxColumnIsHidden = false;


            if (_grid == null)
            {
                ExcelExport.ExportDataByGridWithXSL(_strPath, _data);
            }
            else
            {
                for (int iCol = 0; iCol < _grid.Columns.Count; iCol++)
                {
                    if (!_grid.Columns[iCol].Hidden)
                    {
                        break;
                    }
                }
                if (_data == null)
                {
                    ExcelExport.ExportDataByGridWithXSL(_strPath, _grid);
                }
                else
                {
                    if (_swapdata == null)
                    {
                        DataSet _dsCopy = _data.Copy();
                        _dsCopy.Tables[0].PrimaryKey = null;
                        ExcelExport.ExportDataByGridWithXSL(_strPath, _grid, _dsCopy);
                    }
                    else
                    {
                        DataSet _dsCopy = _data.Copy();
                        _dsCopy.Tables[0].PrimaryKey = null;
                        ExcelExport.ExportDataByGridWithXSL(_strPath, _grid, _dsCopy, _swapdata);
                    }
                }
            }
            string downLoadUrl = serverPath + @"CommonPage/downLoad.aspx?filename=" + Server.UrlEncode(_strPath);
            string strFeature = "height=300,width=300,top=200,left=200,toolbar=no,menubar=no,scrollbars=no,resizable=no,location=no, status=no";
            string strScript = "window.open('" + downLoadUrl + "' ,\"downloadwindow\",'" + strFeature + "');";

            //复原第一个CheckBox列的设置
            //if (chkBoxColumnIndex > -1)
            //{
            //    _grid.Columns[chkBoxColumnIndex].Hidden = chkBoxColumnIsHidden;
            //}
            return "if($('#iframe_DownLoad').length>0){ $('#iframe_DownLoad')[0].src='" + downLoadUrl + "'; }else{ $('<iframe id=\"iframe_DownLoad\" style=\"display:none;\" src=\"" + downLoadUrl + "\"></iframe>').appendTo(document.body); }";
        }
        /// <summary>
        ///  执行Excel导出
        /// </summary>
        /// <param name="exportFileName">导出的文件名</param>
        /// <param name="_grid">grid</param>
        /// <param name="_data">DataSet</param>
        /// <returns>现在数据的script</returns>
        public string ExportExcelData(string exportFileName, Grid _grid, DataSet _data)
        {
            return ExportExcelData(exportFileName, _grid, _data, null);
        }
        /// <summary>
        ///  执行Excel导出
        /// </summary>
        /// <param name="exportFileName">导出的文件名</param>
        /// <param name="_grid">grid</param>
        /// <returns>现在数据的script</returns>
        public string ExportExcelData(string exportFileName, Grid _grid)
        {
            return ExportExcelData(exportFileName, _grid, null);
        }
        /// <summary>
        ///  执行Excel导出
        /// </summary>
        /// <param name="exportFileName">导出的文件名</param>
        /// <param name="_grid">grid</param>
        /// <param name="_data">DataSet</param>
        /// <returns>现在数据的script</returns>
        public string ExportExcelData(string exportFileName, DataTable _data, Grid _grid)
        {
            DataSet _ds = createDatasetByDatatable(_data);
            return ExportExcelData(exportFileName, _grid, _ds);
        }
        /// <summary>
        ///  执行Excel导出
        /// </summary>
        /// <param name="exportFileName">导出的文件名</param>
        /// <param name="_grid">grid</param>
        /// <param name="_data">DataSet</param>
        /// <param name="_swapdata"></param>
        /// <returns>现在数据的script</returns>
        public string ExportExcelData(string exportFileName, Grid _grid, DataTable _data, ArrayList _swapdata)
        {
            DataSet _ds = createDatasetByDatatable(_data);
            return ExportExcelData(exportFileName, _grid, _ds, _swapdata);
        }
        /// <summary>
        ///  执行Excel导出
        /// </summary>
        /// <param name="exportFileName">导出的文件名</param>
        /// <param name="_grid">grid</param>
        /// <param name="_data">DataSet</param>
        /// <param name="_swapdata"></param>
        /// <returns>现在数据的script</returns>
        public string ExportExcelData(string exportFileName, Grid _grid, DataView _data, ArrayList _swapdata)
        {
            DataSet _ds = createDatasetByDataview(_data);
            return ExportExcelData(exportFileName, _grid, _ds, _swapdata);
        }
        ///// <summary>
        ///// 纯数据导出
        ///// </summary>
        ///// <param name="exportFileName"></param>
        ///// <param name="_data"></param>
        ///// <returns></returns>
        //public string ExportExcelData(string exportFileName,DataSet _data)
        //{
        //    string serverPath = @"http://" +Context.Request.Url.Host + Context.Request.ApplicationPath;
        //    string _strPath = Server.MapPath(Context.Request.ApplicationPath) + "/DownLoadDir/";
        //    if(!Directory.Exists(_strPath))
        //    {
        //        Directory.CreateDirectory(_strPath);
        //    }
        //    if(exportFileName==null || exportFileName.Trim()=="")
        //    {
        //        exportFileName=DateTime.Now.ToString("s",DateTimeFormatInfo.InvariantInfo).Replace("-","").Replace(":","").Replace("T","");
        //    }
        //        //给导出文件添加用户标识
        //    else
        //    {
        //        string UID = Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERID] == null ? Guid.NewGuid().ToString() : Session[CHEER.Common.AppConstants.SystemAppConstants.SESSION_USERID].ToString().Replace("-","_");
        //        exportFileName +="_"+UID;
        //    }
        //    string _fileName = exportFileName+".xls";
        //    _strPath += _fileName;
        //    _strPath = _strPath.Replace('\\','/');
        //    DataSet _dsCopy=_data.Copy();
        //    _dsCopy.Tables[0].PrimaryKey=null;
        //    CHEER.PresentationLayer.ePerformance.Common.ExcelExport.BuildExcel(_dsCopy,_strPath);
        //    string downLoadUrl = serverPath + @"/CommonPage/downLoad.aspx?filename=" + Server.UrlEncode(_strPath);
        //    string strFeature="height=300,width=300,top=200,left=200,toolbar=no,menubar=no,scrollbars=no,resizable=no,location=no, status=no";
        //    string strScript="window.open('" +downLoadUrl+ "' ,\"downloadwindow\",'" +strFeature+ "');";
        //    return strScript;
        //}
        /// <summary>
        /// 通过datatable创建dataset 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private DataSet createDatasetByDatatable(DataTable table)
        {
            if (table == null)
            {
                return null;
            }
            DataSet _ds = new DataSet();
            DataTable _tbl = table.Copy();
            _ds.Tables.Add(_tbl);
            return _ds;
        }
        /// <summary>
        /// 通过datatable创建dataset 
        /// </summary>
        /// <param name="dv"></param>
        /// <returns></returns>
        private DataSet createDatasetByDataview(DataView dv)
        {
            if (dv == null)
            {
                return null;
            }
            DataSet _ds = new DataSet();
            DataTable _tbl = dv.Table.Clone();
            DataRow[] dws = dv.Table.Select(dv.RowFilter);
            DataRow row = null;
            foreach (DataRow dw in dws)
            {
                row = _tbl.NewRow();
                row = copyRowData(row, dw);
                _tbl.Rows.Add(row);
            }
            _ds.Tables.Add(_tbl);
            return _ds;
        }
        /// <summary>
        /// 把_dwSource 的数据copy 到_dwSub
        /// </summary>
        /// <param name="_dwSub"></param>
        /// <param name="_dwSource"></param>
        /// <returns></returns>
        private DataRow copyRowData(DataRow _dwSub, DataRow _dwSource)
        {
            for (int i = 0; i < _dwSource.ItemArray.Length; i++)
            {
                _dwSub[i] = _dwSource[i];
            }
            return _dwSub;
        }
        /// <summary>
        /// 获得script中的路径  
        /// </summary>
        /// <param name="strScript"></param>
        /// <returns></returns>
        public string getFilePathInScript(string strScript)
        {
            string strPath = "";
            if (strScript.IndexOf("filename=") > 0)
            {
                strPath = strScript.Substring(strScript.IndexOf("filename=") + 9);
                if (strPath.IndexOf("' ,") > 0)
                {
                    strPath = strPath.Substring(0, strPath.IndexOf("' ,"));
                }
            }
            return strPath;
        }
        #endregion
    }
}