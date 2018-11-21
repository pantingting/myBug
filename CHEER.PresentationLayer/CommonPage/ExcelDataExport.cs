using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using CHEER.BusinessLayer.ePersonnel.Adjust;
using CHEER.BusinessLayer.ePersonnel.JobFamily;
using CHEER.BusinessLayer.ePersonnel.SystemConfig;
using CHEER.Common;
using CHEER.CommonLayer.ePersonnel.Schema;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;

namespace CHEER.PresentationLayer.CommonPage
{
    public class ExcelDataExport : CHEERBasePage
    {
        /// <summary>
        /// 根据枚举类型获取相应的替换值对象
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        private SwapData GetSwapDataByeEnum(int typeID, string _psnSchema)
        {
            DataSet _data = new DataSet();
            NameValueCollection _collection = new NameValueCollection();
            SwapData _swapData = new SwapData();
            switch ((ExcelExportTypeID)typeID)
            {
                //在职状态
                case ExcelExportTypeID.AccessionStatus:
                    {
                        PersonnelPublicCodeManager _itemManager =
                            (PersonnelPublicCodeManager)eHRPageServer.getPalaulObject(typeof(CHEER.BusinessLayer.ePersonnel.SystemConfig.PersonnelPublicCodeManager), "CHEER.BusinessLayer.ePersonnel.SystemConfig");
                        _collection = _itemManager.GetValueTextAccessionStatus();
                        _swapData.ColumnName = _psnSchema;
                        _swapData.SwapValueCollection = _collection;
                        break;
                    }
                //组织
                case ExcelExportTypeID.Branch:
                    {
                        Hashtable _branchhst = ((AdjustManager)eHRPageServer.GetPalauObject(typeof(CHEER.BusinessLayer.ePersonnel.Adjust.AdjustManager))).GetAllDeptNameHashtable();
                        foreach (string _branchID in _branchhst.Keys)
                        {
                            _collection.Add(_branchID, _branchhst[_branchID].ToString());
                        }
                        _swapData.ColumnName = _psnSchema;
                        _swapData.SwapValueCollection = _collection;
                        break;
                    }
                //性别
                case ExcelExportTypeID.Gender:
                    {
                        _collection.Add(((int)Gender.Female).ToString(), base.getString("ZGAIA00313"));
                        _collection.Add(((int)Gender.Male).ToString(), base.getString("ZGAIA00309"));
                        _collection.Add(((int)Gender.Unset).ToString(), "");
                        _swapData.ColumnName = _psnSchema;
                        _swapData.SwapValueCollection = _collection;
                        break;
                    }
                //职位
                case ExcelExportTypeID.Position:
                    {
                        Hashtable _positionhst = ((PositionManager)eHRPageServer.GetPalauObject(typeof(PositionManager))).GetAllWithHastable();
                        foreach (string _positionID in _positionhst.Keys)
                        {
                            _collection.Add(_positionID, _positionhst[_positionID].ToString());
                        }
                        _swapData.ColumnName = _psnSchema;
                        _swapData.SwapValueCollection = _collection;
                        break;
                    }
                //生效状态
                case ExcelExportTypeID.EffectStatus:
                    {
                        _collection.Add(((int)EffectStatus.Effect).ToString(), base.getString("ZGAIA00731"));
                        _collection.Add(((int)EffectStatus.UnEffect).ToString(), base.getString("ZGAIA00850"));
                        _swapData.ColumnName = _psnSchema;
                        _swapData.SwapValueCollection = _collection;
                        break;
                    }
                //工种
                case ExcelExportTypeID.JobType:
                    {
                        Hashtable _hstable = new Hashtable();
                        PersonnelPublicCodeManager _public = (PersonnelPublicCodeManager)eHRPageServer.GetPalauObject(typeof(PersonnelPublicCodeManager));
                        _data = _public.GetCodeValues(((int)PersonnelPublicCodeType.JobType).ToString());
                        foreach (DataRow _drow in _data.Tables[0].Rows)
                        {
                            if (!_hstable.ContainsKey(_drow[PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMID_FULL].ToString()))
                            {
                                _hstable.Add(_drow[PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMID_FULL].ToString(), _drow[PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMVALUE_FULL].ToString());
                            }
                        }
                        foreach (string jobtypeID in _hstable.Keys)
                        {
                            _collection.Add(jobtypeID, _hstable[jobtypeID].ToString());
                        }
                        _swapData.ColumnName = _psnSchema;
                        _swapData.SwapValueCollection = _collection;
                        break;
                    }
                //合同类型
                case ExcelExportTypeID.ContractType:
                    {
                        Hashtable _hstable = new Hashtable();
                        PersonnelPublicCodeManager _public = (PersonnelPublicCodeManager)eHRPageServer.GetPalauObject(typeof(PersonnelPublicCodeManager));
                        _data = _public.GetCodeValues(((int)PersonnelPublicCodeType.ContractType).ToString());
                        foreach (DataRow _drow in _data.Tables[0].Rows)
                        {
                            if (!_hstable.ContainsKey(_drow[PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMID_FULL].ToString()))
                            {
                                _hstable.Add(_drow[PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMID_FULL].ToString(), _drow[PSNPublicCodeItemSchema.PSNPUBLICCODEITEM_ITEMVALUE_FULL].ToString());
                            }
                        }
                        foreach (string jobtypeID in _hstable.Keys)
                        {
                            _collection.Add(jobtypeID, _hstable[jobtypeID].ToString());
                        }
                        _swapData.ColumnName = _psnSchema;
                        _swapData.SwapValueCollection = _collection;
                        break;
                    }
                //合同状态
                case ExcelExportTypeID.ContractStatus:
                    {
                        PersonnelPublicCodeManager _public = (PersonnelPublicCodeManager)eHRPageServer.GetPalauObject(typeof(PersonnelPublicCodeManager));
                        _collection = _public.GetValueTextContractStatus();
                        _swapData.ColumnName = _psnSchema;
                        _swapData.SwapValueCollection = _collection;
                        break;
                    }
                //合同是否作废
                case ExcelExportTypeID.ISRepeal:
                    {
                        _collection.Add("1", base.getString("ZGAIA00254"));
                        _collection.Add("0", base.getString("ZGAIA00785"));
                        _swapData.ColumnName = _psnSchema;
                        _swapData.SwapValueCollection = _collection;
                        break;
                    }
                //合同是否归档
                case ExcelExportTypeID.ISArchive:
                    {
                        _collection.Add("1", base.getString("ZGAIA00254"));
                        _collection.Add("0", base.getString("ZGAIA00785"));
                        _swapData.ColumnName = _psnSchema;
                        _swapData.SwapValueCollection = _collection;
                        break;
                    }

                default:
                    _swapData = null;
                    break;
            }
            return _swapData;
        }
        /// <summary>
        /// 根据枚举list获取arraylist参数
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        private ArrayList GetExcelReplaceArray(ArrayList _list, ArrayList _schemaList)
        {
            int count = _list.Count;
            ArrayList array = new ArrayList();
            SwapData _swapData = new SwapData();
            for (int i = 0; i < count; i++)
            {
                int ExcelExportTypeID = (int)_list[i];
                _swapData = this.GetSwapDataByeEnum((int)ExcelExportTypeID, _schemaList[i].ToString());
                array.Add(_swapData);
            }
            return array;
        }

        private DataSet ReplaceRowData(DataSet _dataSet)
        {
            DataSet _data = new DataSet();
            _data = _dataSet;
            foreach (DataRow _drow in _data.Tables[0].Rows)
            {
                //工号
                if (_data.Tables[0].Columns.Contains(PSNAccountSchema.PSNACCOUNT_EMPLOYEEID_FULL))
                {
                    string employeeID = _drow[PSNAccountSchema.PSNACCOUNT_EMPLOYEEID_FULL].ToString();
                    //					employeeID = "'" + employeeID;
                    _drow[PSNAccountSchema.PSNACCOUNT_EMPLOYEEID_FULL] = employeeID;
                }
                //替换英文名
                if (_data.Tables[0].Columns.Contains(PSNAccountSchema.PSNACCOUNT_MIDDLENAME_FULL))
                {
                    string _middleName = _drow[PSNAccountSchema.PSNACCOUNT_MIDDLENAME_FULL].ToString();
                    string _englishName = "";
                    if (_middleName == null || _middleName.Length == 0)
                    {
                        _englishName = _drow[PSNAccountSchema.PSNACCOUNT_FIRSTNAME_FULL].ToString()
                            + " " + _drow[PSNAccountSchema.PSNACCOUNT_LASTNAME_FULL].ToString();
                    }
                    else
                    {
                        _englishName = _drow[PSNAccountSchema.PSNACCOUNT_FIRSTNAME_FULL].ToString()
                            + " " + _middleName + " " + _drow[PSNAccountSchema.PSNACCOUNT_LASTNAME_FULL].ToString();
                    }
                    _drow[PSNAccountSchema.PSNACCOUNT_LASTNAME_FULL] = _englishName;
                }
                //替换生效时间
                if (_data.Tables[0].Columns.Contains(PSNAdjustSchema.PSNADJUST_EFFECTDATE_FULL))
                {
                    string datatime = _drow[PSNAdjustSchema.PSNADJUST_EFFECTDATE_FULL].ToString();
                    datatime = DataProcessor.DateTimeToShortString(DataProcessor.StringToDateTime(datatime));
                    _drow[PSNAdjustSchema.PSNADJUST_EFFECTDATE_FULL] = datatime;
                }
                //替换生效日期
                if (_data.Tables[0].Columns.Contains(PSNContractSchema.PSNCONTRACT_ENABLEDATE_FULL))
                {
                    string datatime = _drow[PSNContractSchema.PSNCONTRACT_ENABLEDATE_FULL].ToString();
                    datatime = DataProcessor.DateTimeToShortString(DataProcessor.StringToDateTime(datatime));
                    _drow[PSNContractSchema.PSNCONTRACT_ENABLEDATE_FULL] = datatime;
                }
                //替换签订日期
                if (_data.Tables[0].Columns.Contains(PSNContractSchema.PSNCONTRACT_SUBSCRIBEDATE_FULL))
                {
                    string datatime = _drow[PSNContractSchema.PSNCONTRACT_SUBSCRIBEDATE_FULL].ToString();
                    datatime = DataProcessor.DateTimeToShortString(DataProcessor.StringToDateTime(datatime));
                    _drow[PSNContractSchema.PSNCONTRACT_SUBSCRIBEDATE_FULL] = datatime;
                }
                //替换失效日期
                if (_data.Tables[0].Columns.Contains(PSNContractSchema.PSNCONTRACT_DISABLEDATE_FULL))
                {
                    string datatime = _drow[PSNContractSchema.PSNCONTRACT_DISABLEDATE_FULL].ToString();
                    datatime = DataProcessor.DateTimeToShortString(DataProcessor.StringToDateTime(datatime));
                    _drow[PSNContractSchema.PSNCONTRACT_DISABLEDATE_FULL] = datatime;
                }
                //替换入职日期
                if (_data.Tables[0].Columns.Contains(PSNAccountSchema.PSNACCOUNT_ATTENDONDATE_FULL))
                {
                    string datatime = _drow[PSNAccountSchema.PSNACCOUNT_ATTENDONDATE_FULL].ToString();
                    datatime = DataProcessor.DateTimeToShortString(DataProcessor.StringToDateTime(datatime));
                    _drow[PSNAccountSchema.PSNACCOUNT_ATTENDONDATE_FULL] = datatime;
                }
                //替换试满日期
                if (_data.Tables[0].Columns.Contains(PSNAccountSchema.PSNACCOUNT_PROBATIONENDDATE_FULL))
                {
                    string datatime = _drow[PSNAccountSchema.PSNACCOUNT_PROBATIONENDDATE_FULL].ToString();
                    datatime = DataProcessor.DateTimeToShortString(DataProcessor.StringToDateTime(datatime));
                    _drow[PSNAccountSchema.PSNACCOUNT_PROBATIONENDDATE_FULL] = datatime;
                }
            }
            //替换异动内容
            if (_data.Tables[0].Columns.Contains(PSNAdjustSchema.PSNADJUST_ADJUSTID_FULL))
            {
                ArrayList arrayID = new ArrayList();
                ArrayList arrayName = new ArrayList();
                foreach (DataRow _drow in _data.Tables[0].Rows)
                {
                    arrayID.Add(_drow[PSNAdjustSchema.PSNADJUST_ADJUSTID_FULL].ToString());
                }
                AdjustManager _adjust = (AdjustManager)eHRPageServer.GetPalauObject(typeof(AdjustManager));
                arrayName = _adjust.GetBatchAdjustByAdjustID(arrayID);
                int count = arrayID.Count;
                string adjustName;
                for (int i = 0; i < count; i++)
                {
                    adjustName = ((CHEER.BusinessLayer.ePersonnel.Adjust.Adjust)arrayName[i]).AdjustActionName + "：" + ((CHEER.BusinessLayer.ePersonnel.Adjust.Adjust)arrayName[i]).AdjustSourceName + " -> " + ((CHEER.BusinessLayer.ePersonnel.Adjust.Adjust)arrayName[i]).AdjustResultName;
                    _data.Tables[0].Rows[i][PSNAdjustSchema.PSNADJUST_ADJUSTRESULT_FULL] = adjustName;
                }
            }
            return _data;
        }
        /// <summary>
        /// 执行Excel导出
        /// </summary>
        /// <param name="_pageID"></param>
        /// <param name="_grid"></param>
        /// <param name="_data"></param>
        /// <param name="_list"></param>
        public string ExportExcelData(string _pageID, CheerUI.Grid _grid, DataSet _data, ArrayList _list, ArrayList _schemaList)
        {
            ArrayList array = new ArrayList();
            array = this.GetExcelReplaceArray(_list, _schemaList);
            _data = this.ReplaceRowData(_data);
            string strPath;
            string strScript = InitExprotFileInfo(out strPath, _pageID);
            ExcelExport.ExportDataByGridWithXSL(strPath, _grid, _data, array);
            return strScript;
        }
        public DataSet ExportExcelDatahetong(string _pageID, CheerUI.Grid _grid, DataSet _data, ArrayList _list, ArrayList _schemaList)
        {
            ArrayList array = new ArrayList();
            array = this.GetExcelReplaceArray(_list, _schemaList);
            _data = this.ReplaceRowData(_data);
            string strPath;
            string strScript = InitExprotFileInfo(out strPath, _pageID);
            DataSet ds = ExcelExport.ExportDataByGridWithXSLhetong(strPath, _grid, _data, array);
            return ds;
        }
        /// <summary>
        /// 执行Excel导出
        /// </summary>
        /// <param name="_pageID"></param>
        /// <param name="_grid"></param>
        /// <param name="_data"></param>
        public string ExportExcelData(string _pageID, CheerUI.Grid _grid, DataSet _data)
        {
            _data = this.ReplaceRowData(_data);
            string strPath;
            string strScript = InitExprotFileInfo(out strPath, _pageID);
            ExcelExport.ExportDataByGridWithXSL(strPath, _grid, _data);
            return strScript;
        }

        public string ExportExcelData(string _pageID, CheerUI.Grid _grid, DataSet _data, ArrayList _list)
        {
            string strPath;
            string strScript = InitExprotFileInfo(out strPath, _pageID);
            if (_list != null && _list.Count > 0)
            {
                ExcelExport.ExportDataByGridWithXSL(strPath, _grid, _data, _list);
            }
            else
            {
                ExcelExport.ExportDataByGridWithXSL(strPath, _grid, _data);
            }
            return strScript;
        }
        /// <summary>
        /// 数据导出临时文件目录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GetExprotDownloadPath(HttpRequest request)
        {
            return Server.MapPath(Context.Request.ApplicationPath) + ExprotDownloadPath;
        }
        public string ExprotDownloadPath = "/DownLoadDir/";
        private string InitExprotFileInfo(out string fileName, string pageID)
        {
            string _strPath = GetExprotDownloadPath(Context.Request);
            if (!Directory.Exists(_strPath))
            {
                Directory.CreateDirectory(_strPath);
            }
            _strPath = _strPath + pageID + Guid.NewGuid().ToString() + ".xlsx";
            _strPath = _strPath.Replace('\\', '/');
            fileName = _strPath;
            string downLoadUrl = GetBaseURL(Context.Request) + @"/CommonPage/downLoad.aspx?filename=" + Server.UrlEncode(_strPath);
            //string strFeature = "height=300,width=300,top=200,left=200,toolbar=no,menubar=no,scrollbars=no,resizable=no,location=no, status=no";
            //return "window.open('" +downLoadUrl+ "' ,\"downloadwindow\",'" +strFeature+ "');";
            return @";window.attachEvent('onload',function(){
                        var myIframe=document.getElementById('myDownIframe');
                        if(myIframe==null){
                            var iframe = document.createElement('IFRAME'); 
                            iframe.setAttribute('src', '" + downLoadUrl + @"'); 
                            iframe.id = 'myDownIframe';
                            iframe.style.display = 'none';
                            document.forms[0].appendChild(iframe);
                        }else{
                            myIframe.setAttribute('src', '" + downLoadUrl + @"');
                        }
                    });";
        }
    }
}