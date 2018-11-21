using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using CheerUI;
using CHEER.BusinessLayer.ePersonnel.SystemConfig;
using CHEER.Common;
using CHEER.CommonLayer.ePersonnel.Schema;
using CHEER.PresentationLayer;
using Infragistics.Documents.Excel;

namespace CHEER.PresentationLayer.CommonUse
{
    public class ExcelExport
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ExcelExport()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //

        }

        /// <summary>
        /// 根据一组传入的SwapInfo信息构建SwapData对象集合，用于数据导出时公用代码的类信息的准备工作
        /// </summary>
        /// <param name="publicCodes">SwapInfo集合</param>
        /// <returns></returns>
        public ArrayList GetSwapsByPublicCode(ArrayList publicCodes)
        {
            ArrayList swapDatas = new ArrayList();
            if (!(publicCodes == null || publicCodes.Count == 0))
            {
                PersonnelPublicCodeManager _publicCodeManager = (PersonnelPublicCodeManager)(new CHEERBasePage().CreatePalauObject(typeof(PersonnelPublicCodeManager)));
                foreach (SwapInfo swapInfo in publicCodes)
                {
                    swapDatas.Add(GetSwapData(_publicCodeManager.GetCodeValues(((int)swapInfo.PublicCodeType).ToString()), swapInfo.ColumnName));
                }
            }
            return swapDatas;
        }
        private SwapData GetSwapData(DataSet data, string columnName)
        {
            NameValueCollection collection = new NameValueCollection();
            ArrayList keys = new ArrayList();
            SwapData swapData = new SwapData();
            foreach (DataRow publicCode in data.Tables[0].Rows)
            {
                if (keys.Contains(publicCode[PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMID_FULL].ToString()) == false)
                {
                    keys.Add(publicCode[PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMID_FULL].ToString());
                    collection.Add(publicCode[PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMID_FULL].ToString(),
                        publicCode[PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMVALUE_FULL].ToString());
                }
            }
            swapData.ColumnName = columnName;
            swapData.SwapValueCollection = collection;
            return swapData;
        }
        /// <summary>
        /// 用于获取Grid导出公用代码替换信息时传递构建SwapData需要的列名和公用代码的类型
        /// </summary>
        [Serializable]
        public class SwapInfo
        {
            /// <summary>
            /// 
            /// </summary>
            public SwapInfo()
            {
            }
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="columnName">列名</param>
            /// <param name="publicCodeType">公用代码类型</param>
            public SwapInfo(string columnName, PersonnelPublicCodeType publicCodeType)
            {
                ColumnName = columnName;
                PublicCodeType = publicCodeType;
            }
            /// <summary>
            /// 需要替换的列名
            /// </summary>
            public string ColumnName
            {
                get { return _columnName; }
                set { _columnName = value; }
            }
            private string _columnName;
            /// <summary>
            /// 对应的功用代码类型
            /// </summary>
            public PersonnelPublicCodeType PublicCodeType
            {
                get { return _publicCodeType; }
                set { _publicCodeType = value; }
            }
            private PersonnelPublicCodeType _publicCodeType;
        }

        #region 导出Excel

        /// <summary>
        /// 更据Grid显示样式，导出数据集数据到指定名称的文件中（存在列显示值的替换）
        /// </summary>
        ///<param name="strPath">导出路径</param>
        ///<param name="grid">当前显示Grid</param>
        ///<param name="ds">当前数据集</param>
        ///<param name="array">集合（构成元素：列名＋要替换值的键值对）</param>
        public static void ExportDataByGridWithXSL(string strPath, Grid grid, DataSet ds, ArrayList array)
        {
            //try
            //{
            DataSet _ds = GetFilterDataSet(grid, ds, array);
            BuildExcel(_ds, strPath);
            //}
            //catch(Exception _ex)
            //{
            //    throw _ex;
            //}
        }
        public static DataSet ExportDataByGridWithXSLhetong(string strPath, Grid grid, DataSet ds, ArrayList array)
        {
            //try
            //{
            DataSet _ds = GetFilterDataSet(grid, ds, array);
            return _ds;

            //}
            //catch(Exception _ex)
            //{
            //    throw _ex;
            //}
        }

        /// <summary>
        /// 更据Grid显示样式，导出数据集数据到指定名称的文件中
        /// </summary>
        ///<param name="strPath">导出路径</param>
        ///<param name="grid">当前显示Grid</param>
        ///<param name="ds">当前数据集</param>
        public static void ExportDataByGridWithXSL(string strPath, Grid grid, DataSet ds)
        {
            try
            {
                DataSet _ds = GetFilterDataSet(grid, ds);
                BuildExcel(_ds, strPath);
            }
            catch (Exception _ex)
            {
                throw _ex;
            }
        }
        /// <summary>
        /// 更据Grid显示样式，导出数据集数据到指定名称的文件中
        /// </summary>
        ///<param name="strPath">导出路径</param>
        ///<param name="grid">当前显示Grid</param>
        public static void ExportDataByGridWithXSL(string strPath, Grid grid)
        {
            try
            {
                DataSet _ds = GetGridData(grid);
                BuildExcel(_ds, strPath);
            }
            catch (Exception _ex)
            {
                throw _ex;
            }
        }
        /// <summary>
        /// 导出DataSet数据到指定的路径
        /// </summary>
        /// <param name="strPath">导出路径</param>
        /// <param name="ds">数据集</param>
        public static void ExportDataByGridWithXSL(string strPath, DataSet ds)
        {
            try
            {
                BuildExcel(ds, strPath);
            }
            catch (Exception _ex)
            {
                throw _ex;
            }
        }
        /// <summary>
        /// 创建转换格式文件(XSL)
        /// </summary>
        /// <param name="ds">要导出的数据集</param>
        /// <param name="XslPath">xsl文件存放路径</param>
        private static void GetXSLFile(DataSet ds, string XslPath)
        {
            string strColumn = "";
            string strRow = "";
            StringBuilder strbrow = new StringBuilder();
            StringBuilder strbcolumn = new StringBuilder();
            string dsName = ds.DataSetName;
            string tableName = ds.Tables[0].TableName;
            string header = dsName + "/" + tableName;
            foreach (DataColumn clm in ds.Tables[0].Columns)
            {
                #region 特殊字符 <,>,",*,%,(,),& 替换

                string strClmName = clm.ColumnName;
                string strRowName = clm.ColumnName;
                //strRowName = ExcelExport.CheckColumnAndReplace(strRowName);
                strRowName = XmlConvert.EncodeLocalName(strRowName);
                #region 列名替换(Excel表头)
                if (strClmName.IndexOf("&") != -1)
                    strClmName = strClmName.Replace("&", "&amp;");
                if (strClmName.IndexOf("<") != -1)
                    strClmName = strClmName.Replace("<", "&lt;");
                if (strClmName.IndexOf(">") != -1)
                    strClmName = strClmName.Replace(">", "&gt;");
                if (strClmName.IndexOf("\"") != -1)
                    strClmName = strClmName.Replace("\"", "&quot;");
                #endregion
                #endregion
                if (clm.DataType == typeof(System.String))
                {
                    strbrow.Append("<td >");
                }
                else
                {
                    strbrow.Append(@"<td x=""num"">");
                }
                strbrow.Append("<xsl:value-of select=" + "\"" + strRowName + "\"" + "/>" + "</td>" + "\r\n");
                strbcolumn.Append("<th>" + strClmName + "</th>" + "\r\n");
            }
            strColumn = strbcolumn.ToString();
            strRow = strbrow.ToString();
            StringBuilder strb = new StringBuilder();

            string str = @"<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"">
			<xsl:template match=""/"">
			<html xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:x=""urn:schemas-microsoft-com:office:excel"" xmlns=""http://www.w3.org/TR/REC-html40""> 
			<head> 
			<meta http-equiv=""Content-Type"" content=""text/html;charset=utf-8"" /> 
			<style>table{mso-displayed-decimal-separator:""\."";mso-displayed-thousand-separator:""\,"";}br {mso-data-placement:same-cell;}</style>
			<xml> 
			<x:ExcelWorkbook> 
			<x:ExcelWorksheets> 
			<x:ExcelWorksheet> 
			<x:Name>Sheet1</x:Name> 
			<x:WorksheetOptions> 
					<x:ProtectContents>False</x:ProtectContents> 
					<x:ProtectObjects>False</x:ProtectObjects> 
					<x:ProtectScenarios>False</x:ProtectScenarios> 
			</x:WorksheetOptions> 
			</x:ExcelWorksheet> 
			</x:ExcelWorksheets> 
			</x:ExcelWorkbook> 
			</xml> 
			</head>  
			<body> ";
            strb.Append(str);
            strb.Append("\r\n" + @"<table border=""1"" cellpadding=""0"" cellspacing=""0""> 
					<tr>" + "\r\n");
            strb.Append(strColumn);
            strb.Append(@" </tr> 
					<xsl:for-each select=""" + header + @""">
					<tr>");
            strb.Append("\r\n");
            strb.Append(strRow);

            strb.Append(@"</tr> 
					</xsl:for-each> 
					</table> 
					</body> 
					</html> 
					  
					 
					</xsl:template> 
					</xsl:stylesheet> ");
            string path = XslPath;
            if (File.Exists(path))
            {
                File.SetAttributes(path, FileAttributes.Normal);
                File.Delete(path);
            }
            FileStream fs = File.Create(path);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(strb.ToString());
            sw.Close();
            fs.Close();
        }
        /// <summary>
        /// 替换列名中的第一个数字字符
        /// </summary>
        /// <param name="columnname"></param>
        /// <returns></returns>
        private static string CheckColumnAndReplace(string columnname)
        {
            if (columnname != null && columnname.Trim() != "")
            {
                string firstchar = columnname.Substring(0, 1);
                if (ExcelExport.IsDateInt(firstchar))
                {
                    string replacechar = "_x003" + firstchar + "_";
                    string nextchars = columnname.Substring(1);
                    columnname = replacechar + nextchars;
                }
            }
            return columnname;
        }
        private static bool IsDateInt(string intdata)
        {
            try
            {
                Convert.ToInt32(intdata);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 根据数据集,生成替换后的xml文件
        /// </summary>
        /// <param name="ds">数据集合</param>
        /// <param name="XmlFilePath">xml文件路径</param>
        private static void GetXmlFile(DataSet ds, string XmlFilePath)
        {
            string strXml = ds.GetXml();
            if (File.Exists(XmlFilePath))
            {
                File.SetAttributes(XmlFilePath, FileAttributes.Normal);
                File.Delete(XmlFilePath);
            }
            FileStream fs1 = File.Create(XmlFilePath);
            StreamWriter writer = new StreamWriter(fs1);
            writer.Write(strXml);
            writer.Close();
            fs1.Close();
        }

        /// <summary>
        /// 生成Excel文件
        /// </summary>
        /// <param name="path">Excel导出全路径</param>
        /// <param name="ds">数据集</param>
        private static void BuildExcel(DataSet ds, string path)
        {

            try
            {
                Workbook Wb = new Workbook(WorkbookFormat.Excel2007);
                Worksheet Ws = Wb.Worksheets.Add("Sheet1");

                for (int column = 0; column < ds.Tables[0].Columns.Count; column++)
                {
                    Ws.Rows[0].Cells[column].Value = (ds.Tables[0].Columns[column].ColumnName);
                    Ws.Rows[0].Cells[column].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                }

                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    for (int column = 0; column < ds.Tables[0].Columns.Count; column++)
                    {
                        Ws.Rows[row + 1].Cells[column].Value = ds.Tables[0].Rows[row][column];
                    }
                }

                if (File.Exists(path))
                {
                    File.SetAttributes(path, FileAttributes.Normal);
                    File.Delete(path);
                }

                Wb.Save(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void BuildExcelhetong(DataSet ds, string path)
        {
            //			DataSet _ds = ds.Copy();
            if (File.Exists(path))
            {
                File.SetAttributes(path, FileAttributes.Normal);
                File.Delete(path);
            }
            string _path = path.Substring(0, path.Length - 4);
            string _fileXml = _path + ".xml";
            string _fileXsl = _path + ".xsl";
            string _fileXls = _path + ".xls";
            string _fileHtm = _path + ".htm";
            try
            {
                //在string类型的前加’
                //				_ds = AddCommaForStringType(_ds);
                GetXmlFile(ds, _fileXml);
                GetXSLFile(ds, _fileXsl);
                //Excel转换
                if (File.Exists(_fileHtm))
                {
                    File.SetAttributes(_fileHtm, FileAttributes.Normal);
                    File.Delete(_fileHtm);
                }
                XmlDocument doc = new XmlDocument();
                doc.Load(_fileXml);
                XslTransform xslt = new XslTransform();
                xslt.Load(_fileXsl);
                XmlElement root = doc.DocumentElement;
                XPathNavigator nav = root.CreateNavigator();
                XmlTextWriter writer = new XmlTextWriter(_fileHtm, null);
                xslt.Transform(nav, null, writer, null);
                writer.Close();
                FileStream stream = new FileStream(_fileHtm, FileMode.Open);
                StreamReader reader = new StreamReader(stream);
                string _str = reader.ReadToEnd();
                reader.Close();
                string rep = @"<table border=""1"" cellpadding=""0"" cellspacing=""0"">";
                string rep1 = @"<table x:str border=""1"" cellpadding=""0"" cellspacing=""0"">";
                if (_str.IndexOf(rep) != -1)
                {
                    _str = _str.Replace(rep, rep1);
                }
                rep = @"<td x=""num"">";
                rep1 = @"<td x:num>";
                _str = _str.Replace(rep, rep1);
                //替换换行符
                string newLine = "&lt;br&gt;";
                string newLine1 = "<br>";
                if (_str.IndexOf(newLine) != -1)
                {
                    _str = _str.Replace(newLine, newLine1);
                }

                if (File.Exists(_fileXls))
                {
                    File.SetAttributes(_fileXls, FileAttributes.Normal);
                    File.Delete(_fileXls);
                }
                stream = new FileStream(_fileXls, FileMode.Create);
                StreamWriter writer1 = new StreamWriter(stream);
                writer1.Write(_str);
                writer1.Close();
                stream.Close();
                File.Delete(_fileXml);
                File.Delete(_fileXsl);
                File.Delete(_fileHtm);

                //Workbook Wb = new Workbook(WorkbookFormat.Excel97To2003);

                //Worksheet WS = Wb.Worksheets.Add("Sheet1");

                //WS.Columns[0].CellFormat.FormatString = "yyyy/m/d";
                //Wb.Save(_fileXls);

                //Excel.Application  app= new Excel.Application();
                //app.Visible = true;
                //Excel.Workbook wb = app.Workbooks.Open(_fileXls);
                //Excel.Worksheet ws = (Excel.Worksheet)wb.Sheets[1];
                //Excel.Range range = ws.get_Range("H1", "H999");
                //range.NumberFormatLocal = @"yyyy-MM-dd";
                //wb.Save();
                //app.Quit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region 获取设置后的DataSet
        /// <summary>
        /// 更据Grid格式，设置数据集格式
        /// </summary>
        /// <param name="grid">显示数据的Grid</param>
        /// <param name="ds">存储数据的DataSet数据集</param>
        /// <returns>设置好的数据集DataSet</returns>
        private static DataSet GetFilterDataSet(Grid grid, DataSet ds, ArrayList array)
        {
            //获得显示数据的Gird中需要显示的所有列的集合信息
            DataColumnCollection col = ds.Tables[0].Columns;
            foreach (GridColumn clm in grid.Columns)
            {
                if (clm.Hidden)
                {
                    if (col.Contains(clm.ColumnID))
                        col.Remove(clm.ColumnID);
                }
            }
            //或得所有需要的替换的列的集合
            ArrayList replaceArray = new ArrayList(7);
            foreach (SwapData data in array)
            {
                if (!replaceArray.Contains(data.ColumnName))
                {
                    replaceArray.Add(data.ColumnName);
                }
            }
            //构建格式相同的数据集合【按照列排序】
            DataTable dtReturn = new DataTable(ds.Tables[0].TableName);
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                if (grid.Columns[i].Hidden == false)
                {
                    foreach (DataColumn clm_ex in col)
                    {
                        if (clm_ex.ColumnName == grid.Columns[i].ColumnID)
                        {
                            if (replaceArray.Contains(clm_ex.ColumnName) && (col[clm_ex.ColumnName].DataType == typeof(System.Int32) || col[clm_ex.ColumnName].DataType == typeof(System.Decimal)))
                            {
                                dtReturn.Columns.Add(grid.Columns[i].ColumnID, typeof(string));
                            }
                            else
                            {
                                dtReturn.Columns.Add(grid.Columns[i].ColumnID, clm_ex.DataType);
                            }

                        }
                    }
                }
            }
            DataSet dsReturn = new DataSet();
            dsReturn.Tables.Add(dtReturn);
            //对要进行值替换的列进行值替换
            if (null != array)
            {
                try
                {
                    //将数据列均设置为string格式，避免替换出错
                    ArrayList arrayInsert = new ArrayList();
                    #region 如果要替换的列是Int类型，添加该列的兄弟列
                    foreach (SwapData data in array)
                    {
                        if (col.Contains(data.ColumnName) && (col[data.ColumnName].DataType == typeof(System.Int32) || col[data.ColumnName].DataType == typeof(System.Decimal)))
                        {
                            string swapColName = data.ColumnName + "1$";
                            arrayInsert.Add(swapColName);
                        }
                    }
                    foreach (string str in arrayInsert)
                    {
                        col.Add(new DataColumn(str, typeof(string)));
                    }
                    #endregion
                    //按照行为单位遍历数据表，进行替换操作
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        //循环替换要替换的列的值
                        foreach (SwapData obj in array)
                        {
                            if (col.Contains(obj.ColumnName))
                            {
                                NameValueCollection nameCol = obj.SwapValueCollection;
                                if (nameCol != null)
                                {
                                    //整数类型数据替换
                                    if (col[obj.ColumnName].DataType == typeof(System.Int32) || col[obj.ColumnName].DataType == typeof(System.Decimal))
                                    {
                                        foreach (string str in nameCol.Keys)
                                        {
                                            if (dr[obj.ColumnName].ToString() == str)
                                            {
                                                string repClmN = obj.ColumnName + "1$";
                                                dr[repClmN] = nameCol[str];
                                            }
                                        }
                                    }
                                    //string类型替换
                                    else
                                    {
                                        foreach (string str in nameCol.Keys)
                                        {
                                            if (dr[obj.ColumnName].ToString() == str)
                                                dr[obj.ColumnName] = nameCol[str];
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            //删除替换的整型列
            ArrayList arrayDou = new ArrayList();
            foreach (DataColumn douClm in col)
            {
                string douClmN = douClm.ColumnName + "1$";
                if (douClm.DataType == typeof(System.Int32) || douClm.DataType == typeof(System.Decimal))
                {
                    if (col.Contains(douClmN) && col[douClmN].DataType == typeof(System.String))
                    {
                        arrayDou.Add(douClm.ColumnName);
                    }
                }
            }
            foreach (string strDou in arrayDou)
            {
                string strDou1 = strDou + "1$";
                col.Remove(strDou);
                col[strDou1].ColumnName = strDou;
            }
            //按照列原始格式填充数据
            dsReturn.Merge(ds);
            ds = GetFilterDataSet(grid, dsReturn);

            return ds;
        }


        /// <summary>
        /// 更据Grid格式，设置数据集格式
        /// </summary>
        /// <param name="grid">显示数据的Grid</param>
        /// <param name="ds">存储数据的DataSet数据集</param>
        /// <returns>设置好的数据集DataSet</returns>
        private static DataSet GetFilterDataSet(Grid grid, DataSet ds)
        {
            //构建格式相同的数据集合【按照列排序】
            DataTable dtReturn = new DataTable(ds.Tables[0].TableName);
            DataColumnCollection col_ex = ds.Tables[0].Columns;

            for (int i = 0; i < grid.Columns.Count; i++)
            {
                if (grid.Columns[i].ColumnID.ToString().ToUpper() != "DETIAL")
                {
                    foreach (DataColumn clm_ex in col_ex)
                    {
                        if (clm_ex.ColumnName == grid.Columns[i].ColumnID)
                        {
                            dtReturn.Columns.Add(grid.Columns[i].ColumnID, clm_ex.DataType);
                        }
                    }
                }
            }
            DataSet dsReturn = new DataSet();
            dsReturn.Tables.Add(dtReturn);
            //按照列原始格式填充数据
            dsReturn.Merge(ds);
            DataColumnCollection col = dsReturn.Tables[0].Columns;

            string _key = "";
            for (int i = col.Count - 1; i >= 0; i--)
            {
                _key = col[i].ColumnName;
                bool _exist = false;	//DataSet的列在DataGrid中是否存在或在DataGrid中是否隐藏
                int colid = -1;			//DataSet中需要移除的列号
                for (int j = 0; j < grid.Columns.Count; j++)
                {
                    if (_key.ToUpper() == grid.Columns[j].ColumnID.ToUpper() || _key.ToUpper() == grid.Columns[j].HeaderText.ToUpper())
                    {
                        if (!grid.Columns[j].Hidden)
                        {
                            _exist = true;
                            colid = j;
                            break;
                        }
                        //当grid表里面存在的行全部导出
                        //_exist = true;
                        
                        
                    }
                }
                if (!_exist)
                {
                    col.RemoveAt(i);
                }
                else
                {
                    if (col.Contains(grid.Columns[colid].ColumnID) || col.Contains(grid.Columns[colid].HeaderText))
                    {
                        if (col.Contains(grid.Columns[colid].HeaderText))
                        {
                            //col.RemoveAt(i);
                        }
                        else
                        {
                            if (grid.Columns[colid].HeaderText.IndexOf(';') != -1)
                            {
                                col[i].ColumnName = grid.Columns[colid].HeaderText.Substring(grid.Columns[colid].HeaderText.IndexOf(';') + 1);
                            }
                            else
                            {
                                col[i].ColumnName = grid.Columns[colid].HeaderText == "" ? GenerateBlank(i) : grid.Columns[colid].HeaderText;
                            }
                        }
                    }
                }
            }
            return dsReturn;
        }

        private static string GenerateBlank(int length)
        {
            string str = "";
            for (int i = 0; i < length; i++)
            {
                str += " ";
            }
            return str;
        }

        /// <summary>
        /// 更据Grid格式，设置数据集格式
        /// </summary>
        /// <param name="grid">显示数据的Grid</param>
        /// <returns>设置好的数据集DataSet</returns>
        private static DataSet GetFilterDataSet(Grid grid)
        {
            DataSet ds = new DataSet();
            DataTable tb = ds.Tables.Add();
            DataColumnCollection col = tb.Columns;
            foreach (GridColumn clm in grid.Columns)
            {
                if (clm.ColumnID.Trim() != "" && clm.ColumnID.ToString().ToUpper() != "DETIAL")
                {
                    col.Add(clm.ColumnID);
                    col[clm.ColumnID].ColumnName = clm.ColumnID;
                }

            }
            return ds;
        }

        /// <summary>
        /// 更据Grid格式，获取grid数据
        /// </summary>
        /// <param name="grid">显示数据的Grid</param>
        /// <returns>数据集DataSet</returns>
        private static DataSet GetGridData(Grid grid)
        {
            DataSet ds = GetFilterDataSet(grid);
            DataRow rw = null;
            foreach (GridRow row in grid.Rows)
            {
                rw = ds.Tables[0].NewRow();
                foreach (GridColumn clm in grid.Columns)
                {
                    if (clm.ColumnID.ToString().ToUpper() != "DETIAL")
                    {
                        rw[clm.ColumnID] = strValid(row.Cells.FromKey(clm.ColumnID).ToString());
                    }
                }
                ds.Tables[0].Rows.Add(rw);
            }
            ds = GetFilterDataSet(grid, ds);
            return ds;
        }
        #endregion
        #region 提供合法的字符
        /// <summary>
        /// 提供合法的字符
        /// </summary>
        /// <param name="obj">value</param>
        /// <returns></returns>
        private static string strValid(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return "";
                }
                else
                {
                    return obj.ToString().Trim();
                }
            }
            catch
            {
                return "";
            }
        }
        #endregion
    }

    /// <summary>
    /// 替换值包装类
    /// </summary>
    public class SwapData
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SwapData()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        private string _columnName;
        private NameValueCollection _col;
        /// <summary>
        /// 要进行替换的列名
        /// </summary>
        public string ColumnName
        {
            get
            {
                return _columnName;
            }
            set
            {
                _columnName = value;
            }
        }
        /// <summary>
        /// 替换值键值对（说明：该参数要保证键的唯一性）
        /// </summary>
        public NameValueCollection SwapValueCollection
        {
            get
            {
                return _col;
            }
            set
            {
                _col = value;
            }
        }
    }
}