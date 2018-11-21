using System;
using System.Data;
using System.Collections;
using CHEER.BusinessLayer.ePersonnel.PersonnelBaseInfo;
using CHEER.BusinessLayer.Organize;
using CHEER.BusinessLayer.ePersonnel.SystemConfig;
using CHEER.CommonLayer.Organize.Data.STDOrganize;
using CHEER.CommonLayer.ePersonnel.Schema;
using CHEER.CommonLayer.ePersonnel.Data;
using CHEER.CommonLayer.ePersonnel;
using CHEER.Common;
using CHEER.PresentationLayer.CommonUse;
using CHEER.BusinessLayer.Controls;

namespace CHEER.PresentationLayer.CommonUse
{
    /// <summary>
    /// 编号引擎
    /// </summary>
    public class CodeNumberCreateManager : CHEERBasePage
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CodeNumberCreateManager()
        {
        }
        /*生成代码规则
         * 1.根据代码类型获取该代码的所有配置集合
         * 2.获取各配置集合结果值
         * 2.1 固定字符串--直接获取
         * 2.2 日期--根据日期类型和日期格式获取
         * 2.3 流水号--基准号+增量/使用后同步更新基准值,判断基准号是否最大
         * 2.4 个性化取值--根据取值类型及规则获取
         * 2.5 非当前日期和部分个性化取值需要参数传入
         */
        #region 公共方法 主体部分包括手动和自动生成规则编号,根据需要调用
        /// <summary>
        /// 手动获取某类型编码规则的当前编号(手动生成,除固定字符串,自动日期,归属管理单元和流水号外其他全部参数传入,流水号在页面判断是否最大)
        /// </summary>
        /// <param name="numberType">编号类型(工号--EMPLOYEEID,合同编号--CONTRACTNUM,协议编号--PROTOCOLNUM,变更编号--ALTERATIONNUM)</param>
        /// <param name="patameterCodeValueHash">需要传入的规则code和value--key:code;text:value(目前包括员工性质--empcharname)</param>
        /// <returns></returns>
        public string CreateCodeNumberWithHands(string numberType, Hashtable patameterCodeValueHash)
        {
            string codeNumber = "";
            //获取该编码规则的所有规则数据并排序asc
            DataSet ruleDS = this.GetRuleDataByNumberType(numberType);
            if (!PSNCommonFiller.JudgeIfDSNotNull(ruleDS))
                return codeNumber;
            DataView ruleDV = ruleDS.Tables[0].DefaultView;
            ruleDV.Sort = PSNEmpIDRoleSchema.ROLEINDEX + " ASC";
            string ruleTypeStr = "";
            string ruleValue = "";
            string returnStr = "";
            for (int i = 0; i < ruleDV.Count; i++)
            {
                ruleTypeStr = ruleDV[i][PSNEmpIDRoleSchema.ROLETYPE].ToString().Trim();
                ruleValue = ruleDV[i][PSNEmpIDRoleSchema.ROLEVALUE].ToString().Trim();
                EmpIDRoleType ruleType = this.ConvertStrToTypeEnum(ruleTypeStr);
                if (ruleType == EmpIDRoleType.ruleNull)
                    continue;
                switch (ruleType)
                {
                    //固定字符
                    case EmpIDRoleType.appointstr:
                        codeNumber += ruleValue;
                        break;
                    //日期
                    case EmpIDRoleType.datestr:
                        string dateFormat = ruleDV[i][PSNEmpIDRoleSchema.ITEM1].ToString().Trim();
                        returnStr = this.GetDateStrByHands(dateFormat, ruleValue, patameterCodeValueHash);
                        codeNumber += returnStr;
                        break;
                    //个性化取值
                    case EmpIDRoleType.psninforvalue:
                        returnStr = this.GetPsnInforStrByHands(ruleValue, ruleDV, i, patameterCodeValueHash);
                        codeNumber += returnStr;
                        break;
                    //流水号
                    case EmpIDRoleType.journalnum:
                        returnStr = this.GetJournalNum(ruleValue, ruleDV, i);
                        codeNumber += returnStr;
                        break;
                    default:
                        break;
                }
            }
            return codeNumber;
        }
        /// <summary>
        /// 自动获取某类型编码规则的当前编号(所有参数值都自动获取,不存在则为空"",流水号在页面判断是否最大)
        /// </summary>
        /// <param name="numberType">编号类型(工号--EMPLOYEEID,合同编号--CONTRACTNUM,协议编号--PROTOCOLNUM,变更编号--ALTERATIONNUM)</param>
        /// <returns></returns>
        public string CreateCodeNumberWithAuto(string numberType, string personID)
        {
            string codeNumber = "";
            //获取该编码规则的所有规则数据并排序asc
            DataSet ruleDS = this.GetRuleDataByNumberType(numberType);
            if (!PSNCommonFiller.JudgeIfDSNotNull(ruleDS))
                return "";
            DataView ruleDV = ruleDS.Tables[0].DefaultView;
            ruleDV.Sort = PSNEmpIDRoleSchema.ROLEINDEX + " ASC";
            string ruleTypeStr = "";
            string ruleValue = "";
            string returnStr = "";
            for (int i = 0; i < ruleDV.Count; i++)
            {
                ruleTypeStr = ruleDV[i][PSNEmpIDRoleSchema.ROLETYPE].ToString().Trim();
                ruleValue = ruleDV[i][PSNEmpIDRoleSchema.ROLEVALUE].ToString().Trim();
                EmpIDRoleType ruleType = this.ConvertStrToTypeEnum(ruleTypeStr);
                if (ruleType == EmpIDRoleType.ruleNull)
                    continue;
                switch (ruleType)
                {
                    //固定字符
                    case EmpIDRoleType.appointstr:
                        codeNumber += ruleValue;
                        break;
                    //日期
                    case EmpIDRoleType.datestr:
                        string dateFormat = ruleDV[i][PSNEmpIDRoleSchema.ITEM1].ToString().Trim();
                        returnStr = this.GetDateStrAuto(personID, dateFormat, ruleValue);
                        codeNumber += returnStr;
                        break;
                    //个性化取值
                    case EmpIDRoleType.psninforvalue:
                        returnStr = this.GetPsnInforStrAuto(personID, ruleValue, ruleDV, i);
                        codeNumber += returnStr;
                        break;
                    //流水号
                    case EmpIDRoleType.journalnum:
                        returnStr = this.GetJournalNum(ruleValue, ruleDV, i);
                        codeNumber += returnStr;
                        break;
                    default:
                        break;
                }
            }
            return codeNumber;
        }
        public string CreateCodeNumberWithAuto(string numberType, string personID, string basenumber)
        {
            string codeNumber = "";
            //获取该编码规则的所有规则数据并排序asc
            DataSet ruleDS = this.GetRuleDataByNumberType(numberType);
            if (!PSNCommonFiller.JudgeIfDSNotNull(ruleDS))
                return "";
            DataView ruleDV = ruleDS.Tables[0].DefaultView;
            ruleDV.Sort = PSNEmpIDRoleSchema.ROLEINDEX + " ASC";
            string ruleTypeStr = "";
            string ruleValue = "";
            string returnStr = "";
            for (int i = 0; i < ruleDV.Count; i++)
            {
                ruleTypeStr = ruleDV[i][PSNEmpIDRoleSchema.ROLETYPE].ToString().Trim();
                ruleValue = ruleDV[i][PSNEmpIDRoleSchema.ROLEVALUE].ToString().Trim();
                EmpIDRoleType ruleType = this.ConvertStrToTypeEnum(ruleTypeStr);
                if (ruleType == EmpIDRoleType.ruleNull)
                    continue;
                switch (ruleType)
                {
                    //固定字符
                    case EmpIDRoleType.appointstr:
                        codeNumber += basenumber;
                        break;
                    //日期
                    case EmpIDRoleType.datestr:
                        string dateFormat = ruleDV[i][PSNEmpIDRoleSchema.ITEM1].ToString().Trim();
                        returnStr = this.GetDateStrAuto(personID, dateFormat, basenumber);
                        codeNumber += returnStr;
                        break;
                    //个性化取值
                    case EmpIDRoleType.psninforvalue:
                        returnStr = this.GetPsnInforStrAuto(personID, basenumber, ruleDV, i);
                        codeNumber += returnStr;
                        break;
                    //流水号
                    case EmpIDRoleType.journalnum:
                        returnStr = this.GetJournalNum(basenumber, ruleDV, i);
                        codeNumber += returnStr;
                        break;
                    default:
                        break;
                }
            }
            return codeNumber;
        }
        /// <summary>
        /// 判断某类型编码规则是否包含该细项类型,主要用来对不存在的细项进行提示,及判断是否使用手动编号生成方法
        /// </summary>
        /// <param name="detailTypeCode">细项类型(非自动日期,员工性质等)</param>
        /// <param name="numberType"></param>
        /// <returns>true--存在,false--不存在</returns>
        public bool JudgeIfRuleDetailExist(string detailTypeCode, string numberType)
        {
            //根据编码规则类型及明细类型获取值--item2 rolevalue
            Hashtable columnHash = new Hashtable();
            columnHash.Add(PSNEmpIDRoleSchema.ITEM2, numberType);
            columnHash.Add(PSNEmpIDRoleSchema.ROLEVALUE, detailTypeCode);
            EmpIDRoleManager rulemanager = (EmpIDRoleManager)eHRPageServer.GetPalauObject(typeof(EmpIDRoleManager));
            DataSet detailDS = rulemanager.GetRuleDataByColumnsValue(columnHash, null);
            if (PSNCommonFiller.JudgeIfDSNotNull(detailDS))
                return true;
            return false;
        }
        /// <summary>
        /// 判断流水号当前基准号是否到最大,1--到最大,0--未到最大或不存在流水号，ArrayList[0]代表管理单元中的流水号，ArrayList[1]代表系统流水号
        /// </summary>
        /// <param name="numberType">编码规则类型</param>
        /// <returns></returns>
        public ArrayList JudgeIfJournalNumMax(string numberType)
        {
            ArrayList numJudgeResult = new ArrayList();
            //先判断该管理单元下的流水号
            //再判断系统级流水号
            //根据编码规则枚举获取流水号的信息--item2
            EmpIDRoleManager rulemanager = (EmpIDRoleManager)eHRPageServer.GetPalauObject(typeof(EmpIDRoleManager));
            #region 管理单元流水号
            EmpIDRoleData ruleData = rulemanager.GetJournalNumDataByNumberType(numberType, base.getBusinessUnitID().Trim());
            if (ruleData == null)
                numJudgeResult.Add("0");
            else
            {
                int baseNum = Convert.ToInt32(ruleData.ROLEVALUE.Trim());
                int numLen = Convert.ToInt32(ruleData.ROLELENGTH.Trim());
                int addNum = ruleData.ADDVALUE;
                string newNumber = this.GetNewNumber(numLen, baseNum, addNum);
                if (newNumber.Length <= numLen)
                    numJudgeResult.Add("0");
                else
                    numJudgeResult.Add("1");
            }
            #endregion
            #region 系统级流水号
            EmpIDRoleData ruleDataSys = rulemanager.GetJournalNumDataByNumberType(numberType, null);
            if (ruleDataSys == null)
                numJudgeResult.Add("0");
            else
            {
                int baseNumSys = Convert.ToInt32(ruleDataSys.ROLEVALUE.Trim());
                int numLenSys = Convert.ToInt32(ruleDataSys.ROLELENGTH.Trim());
                int addNumSys = ruleDataSys.ADDVALUE;
                string newNumberSys = this.GetNewNumber(numLenSys, baseNumSys, addNumSys);
                if (newNumberSys.Length <= numLenSys)
                    numJudgeResult.Add("0");
                else
                    numJudgeResult.Add("1");
            }
            #endregion
            return numJudgeResult;
        }
        /// <summary>
        /// 根据需要更新某编码规则的流水基准号(基准号+增量),一般是在保存编号相应的数据时调用(存在则更新,不存在不做任何操作)
        /// </summary>
        /// <param name="numberType">编码规则类型</param>
        public void UpdateBaseNumOfJournulNum(string numberType)
        {
            //先更新该管理单元下的流水号
            //再更新系统级流水号
            //根据编码规则枚举获取流水号的信息--item2
            EmpIDRoleManager rulemanager = (EmpIDRoleManager)eHRPageServer.GetPalauObject(typeof(EmpIDRoleManager));
            #region 更新管理单元的
            EmpIDRoleData ruleData = rulemanager.GetJournalNumDataByNumberType(numberType, base.getBusinessUnitID().Trim());
            if (ruleData != null)
            {
                int baseNum = Convert.ToInt32(ruleData.ROLEVALUE.Trim());
                int numLen = Convert.ToInt32(ruleData.ROLELENGTH.Trim());
                int addNum = ruleData.ADDVALUE;
                string newNumber = this.GetNewNumber(numLen, baseNum, addNum);
                if (newNumber.Length > numLen)
                    return;
                ruleData.ROLEVALUE = newNumber;
            }
            #endregion
            #region 更新系统级的
            EmpIDRoleData ruleDataSys = rulemanager.GetJournalNumDataByNumberType(numberType, null);
            if (ruleDataSys != null)
            {
                int baseNumSys = Convert.ToInt32(ruleDataSys.ROLEVALUE.Trim());
                int numLenSys = Convert.ToInt32(ruleDataSys.ROLELENGTH.Trim());
                int addNumSys = ruleDataSys.ADDVALUE;
                string newNumberSys = this.GetNewNumber(numLenSys, baseNumSys, addNumSys);
                if (newNumberSys.Length > numLenSys)
                    return;
                ruleDataSys.ROLEVALUE = newNumberSys;
            }
            #endregion
            if (ruleData != null)
                rulemanager.update(ruleData);
            if (ruleDataSys != null)
                rulemanager.update(ruleDataSys);
        }
        /// <summary>
        /// 根据需要更新某编码规则的流水基准号,置为0000，为太保客制化
        /// </summary>
        /// <param name="numberType">编码规则类型</param>
        public void UpdateBaseNumOfJournulNumToOrig(string numberType)
        {
            //先更新该管理单元下的流水号
            //再更新系统级流水号
            //根据编码规则枚举获取流水号的信息--item2
            EmpIDRoleManager rulemanager = (EmpIDRoleManager)eHRPageServer.GetPalauObject(typeof(EmpIDRoleManager));
            #region 更新管理单元的
            EmpIDRoleData ruleData = rulemanager.GetJournalNumDataByNumberType(numberType, base.getBusinessUnitID().Trim());
            if (ruleData != null)
                ruleData.ROLEVALUE = "0000";
            #endregion
            if (ruleData != null)
                rulemanager.update(ruleData);
        }
        /// <summary>
        /// 获取某个编码规则中第index个规则的取值,目前用于太保客制化构建编码的前半部分以便判断是否需要更新为0000
        /// </summary>
        /// <param name="numberType">编码类型</param>
        /// <param name="index">在编码规则中的位置-1</param>
        public string GetTypeValue(string numberType, int index)
        {
            DataSet ruleDS = this.GetRuleDataByNumberType(numberType);
            if (!PSNCommonFiller.JudgeIfDSNotNull(ruleDS))
                return "";
            DataView ruleDV = ruleDS.Tables[0].DefaultView;
            ruleDV.Sort = PSNEmpIDRoleSchema.ROLEINDEX + " ASC";
            if (ruleDV.Count <= index)
                return "";
            string ruleType = ruleDV[index][PSNEmpIDRoleSchema.ROLETYPE].ToString().Trim();
            string ruleValue = ruleDV[index][PSNEmpIDRoleSchema.ROLEVALUE].ToString().Trim();
            //固定字符
            if (ruleType == ((int)EmpIDRoleType.appointstr).ToString().Trim())
                return ruleValue;
            //日期
            if (ruleType == ((int)EmpIDRoleType.datestr).ToString().Trim())
            {
                string dateFormat = ruleDV[index][PSNEmpIDRoleSchema.ITEM1].ToString().Trim();
                if (ruleValue.Trim() == "autodate")
                    return this.GetDateStrByDateFormat(DateTime.Now, dateFormat);
                else
                {
                    //通过附加方法获取需要的日期，再进行如下处理
                    String dateValue = "";
                    if (dateValue != "")
                        return this.GetDateStrByDateFormat(DataProcessor.StringToDateTime(dateValue), dateFormat);
                    else
                        return "";
                }
            }
            //个性化取值
            if (ruleType == ((int)EmpIDRoleType.psninforvalue).ToString().Trim())
            {
                string psnSort = ruleDV[index][PSNEmpIDRoleSchema.ITEM1].ToString().Trim();
                string psnLength = ruleDV[index][PSNEmpIDRoleSchema.ROLELENGTH].ToString().Trim();
                string psnInforValue = "";
                //员工公司名称
                if (ruleValue.Trim() == "empcompanyname")
                {
                    string companyID = base.getBusinessUnitID();
                    STDUnitManager unitManager = (STDUnitManager)eHRPageServer.GetPalauObject(typeof(STDUnitManager));
                    UnitData unitData = unitManager.GetCurentUnitDataByID(companyID.Trim());
                    psnInforValue = unitData.UnitName;
                }
                //
                if (ruleValue.Trim() == "empcompanycode")
                {
                    string companyID = base.getBusinessUnitID();
                    STDUnitManager unitManager = (STDUnitManager)eHRPageServer.GetPalauObject(typeof(STDUnitManager));
                    UnitData unitData = unitManager.GetCurentUnitDataByID(companyID.Trim());
                    psnInforValue = unitData.UnitCode;
                }
                return this.GetPsnInforOfEmp(psnSort, Convert.ToInt32(psnLength), psnInforValue);
            }
            return "";
        }
        /// <summary>
        /// 判断某类型编码是否包含某种编码规则
        /// </summary>
        /// <param name="ruleType">规则类型</param>
        /// <param name="numberType">编码类型</param>
        /// <returns>true--存在,false--不存在</returns>
        public bool JudgeIfRuleDetailExist(int ruleType, string numberType)
        {
            //根据编码规则类型及明细类型获取值--item2 rolevalue
            Hashtable columnHash = new Hashtable();
            columnHash.Add(PSNEmpIDRoleSchema.ITEM2, numberType);
            columnHash.Add(PSNEmpIDRoleSchema.ROLETYPE, ruleType);
            EmpIDRoleManager rulemanager = (EmpIDRoleManager)eHRPageServer.GetPalauObject(typeof(EmpIDRoleManager));
            DataSet detailDS = rulemanager.GetRuleDataByColumnsValue(columnHash, null);
            if (PSNCommonFiller.JudgeIfDSNotNull(detailDS))
                return true;
            return false;
        }
        /// <summary>
        /// 判断某类型编码是否包含某种编码规则
        /// </summary>
        /// <param name="ruleType">规则类型</param>
        /// <param name="numberType">编码类型</param>
        /// <returns>true--存在,false--不存在</returns>
        public bool JudgeIfRuleDetailExist(EmpIDRoleType ruleType, string numberType)
        {
            //根据编码规则类型及明细类型获取值--item2 rolevalue
            Hashtable columnHash = new Hashtable();
            columnHash.Add(PSNEmpIDRoleSchema.ITEM2, numberType);
            columnHash.Add(PSNEmpIDRoleSchema.ROLETYPE, ((int)ruleType).ToString().Trim());
            EmpIDRoleManager rulemanager = (EmpIDRoleManager)eHRPageServer.GetPalauObject(typeof(EmpIDRoleManager));
            DataSet detailDS = rulemanager.GetRuleDataByColumnsValue(columnHash, null);
            if (PSNCommonFiller.JudgeIfDSNotNull(detailDS))
                return true;
            return false;
        }
        #endregion
        #region 私有方法
        /// <summary>
        /// 转换编码类型值为枚举型
        /// </summary>
        /// <param name="ruleTypeStr">编码类型值</param>
        /// <returns></returns>
        private EmpIDRoleType ConvertStrToTypeEnum(string ruleTypeStr)
        {
            EmpIDRoleType ruleType = EmpIDRoleType.ruleNull;
            //固定字符
            if (ruleTypeStr.Trim() == ((int)EmpIDRoleType.appointstr).ToString().Trim())
                ruleType = EmpIDRoleType.appointstr;
            //日期
            if (ruleTypeStr.Trim() == ((int)EmpIDRoleType.datestr).ToString().Trim())
                ruleType = EmpIDRoleType.datestr;
            //个性化取值
            if (ruleTypeStr.Trim() == ((int)EmpIDRoleType.psninforvalue).ToString().Trim())
                ruleType = EmpIDRoleType.psninforvalue;
            //流水号
            if (ruleTypeStr.Trim() == ((int)EmpIDRoleType.journalnum).ToString().Trim())
                ruleType = EmpIDRoleType.journalnum;
            return ruleType;
        }
        /// <summary>
        /// 手工获取日期
        /// </summary>
        /// <param name="dateFormat">日期格式</param>
        /// <param name="dateType">日期类型</param>
        /// <param name="parameterCodeValueHash">页面传入的手工获取值</param>
        /// <returns></returns>
        private string GetDateStrByHands(string dateFormat, string dateType, Hashtable parameterCodeValueHash)
        {
            string returnStr = "";
            switch (dateType)
            {
                case "autodate":
                    returnStr = this.GetDateStrByDateFormat(DateTime.Now, dateFormat);
                    break;
                default:
                    if (parameterCodeValueHash.Contains(dateType))
                    {
                        string dateValue = parameterCodeValueHash[dateType].ToString().Trim();
                        if (dateValue != "")
                            returnStr = this.GetDateStrByDateFormat(DataProcessor.StringToDateTime(dateValue), dateFormat);
                    }
                    break;
            }
            return returnStr;
        }
        /// <summary>
        /// 手工获取个性化取值
        /// </summary>
        /// <param name="psnInforType">个性化类型</param>
        /// <param name="ruleDV">编号规则的DataView</param>
        /// <param name="index">DataView中的当前行数</param>
        /// <param name="parameterCodeValueHash">页面传入的手工获取值</param>
        /// <returns></returns>
        private string GetPsnInforStrByHands(string psnInforType, DataView ruleDV, int index, Hashtable parameterCodeValueHash)
        {
            string returnStr = "";
            string psnSort = ruleDV[index][PSNEmpIDRoleSchema.ITEM1].ToString().Trim();
            string psnLength = ruleDV[index][PSNEmpIDRoleSchema.ROLELENGTH].ToString().Trim();
            switch (psnInforType)
            {
                //员工公司名称 如有其他扩充可添加case代码
                case "empcompanyname":
                    try
                    {
                        string companyID = base.getBusinessUnitID();
                        STDUnitManager unitManager = (STDUnitManager)eHRPageServer.GetPalauObject(typeof(STDUnitManager));
                        UnitData unitData = unitManager.GetCurentUnitDataByID(companyID.Trim());
                        if (unitData != null)
                            returnStr = unitData.UnitName;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    break;
                case "empcompanycode":
                    try
                    {
                        string companyID = base.getBusinessUnitID();
                        STDUnitManager unitManager = (STDUnitManager)eHRPageServer.GetPalauObject(typeof(STDUnitManager));
                        UnitData unitData = unitManager.GetCurentUnitDataByID(companyID.Trim());
                        if (unitData != null)
                            returnStr = unitData.UnitCode;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    break;
                default:
                    if (parameterCodeValueHash.Contains(psnInforType))
                        returnStr = parameterCodeValueHash[psnInforType].ToString().Trim();
                    break;
            }
            if (returnStr != "")
                returnStr = this.GetPsnInforOfEmp(psnSort, Convert.ToInt32(psnLength), returnStr);
            return returnStr;
        }
        /// <summary>
        /// 获取流水号
        /// </summary>
        /// <param name="baseValue">基准号</param>
        /// <param name="ruleDV">编号规则的DataView</param>
        /// <param name="index">DataView中的当前行数</param>
        /// <returns></returns>
        private string GetJournalNum(string baseValue, DataView ruleDV, int index)
        {
            string returnStr = "";
            int baseNum = Convert.ToInt32(baseValue);
            int length = Convert.ToInt32(ruleDV[index][PSNEmpIDRoleSchema.ROLELENGTH].ToString().Trim());
            int addNum = Convert.ToInt32(ruleDV[index][PSNEmpIDRoleSchema.ADDVALUE].ToString().Trim());
            returnStr = this.GetNewNumber(length, baseNum, addNum);
            return returnStr;
        }
        /// <summary>
        /// 自动获取日期
        /// </summary>
        /// <param name="personID">当前被操作的人员ID</param>
        /// <param name="dateFormat">日期格式</param>
        /// <param name="dateType">日期类型</param>
        /// <returns></returns>
        private string GetDateStrAuto(string personID, string dateFormat, string dateType)
        {
            string returnStr = "";
            switch (dateType)
            {
                //当前日期
                case "autodate":
                    returnStr = this.GetDateStrByDateFormat(DateTime.Now, dateFormat);
                    break;
                default:
                    //通过附加方法获取需要的日期，再进行如下处理，可扩充case代码
                    string dateValue = "";
                    if (dateValue != "")
                        returnStr = this.GetDateStrByDateFormat(DataProcessor.StringToDateTime(dateValue), dateFormat);
                    break;
            }
            return returnStr;
        }
        /// <summary>
        /// 自动获取个性化取值
        /// </summary>
        /// <param name="personID">当前被操作的人员ID</param>
        /// <param name="psnInforType">个性化类型</param>
        /// <param name="ruleDV">编号规则的DataView</param>
        /// <param name="index">DataView中的当前行数</param>
        /// <returns></returns>
        private string GetPsnInforStrAuto(string personID, string psnInforType, DataView ruleDV, int index)
        {
            try
            {
                string returnStr = "";
                string psnSort = ruleDV[index][PSNEmpIDRoleSchema.ITEM1].ToString().Trim();
                string psnLength = ruleDV[index][PSNEmpIDRoleSchema.ROLELENGTH].ToString().Trim();
                switch (psnInforType)
                {
                    //员工性质 如有其他扩充可添加case代码
                    case "empcharname":
                        if (personID != null && personID != "")
                        {
                            PersonAccountManager personManager = (PersonAccountManager)eHRPageServer.GetPalauObject(typeof(PersonAccountManager));
                            AccountInfoViewData accountData = personManager.GetAccountViewInfo(personID);
                            if (accountData != null)
                                returnStr = accountData.AccountInfoData.EmployeeCharID.Trim();
                        }
                        break;
                    //员工公司名称
                    case "empcompanyname":
                        string companyID = base.getBusinessUnitID();
                        STDUnitManager unitManager = (STDUnitManager)eHRPageServer.GetPalauObject(typeof(STDUnitManager));
                        UnitData unitData = unitManager.GetCurentUnitDataByID(companyID.Trim());
                        if (unitData != null)
                            returnStr = unitData.UnitName;
                        break;
                    //员工公司代号
                    case "empcompanycode":
                        companyID = base.getBusinessUnitID();
                        unitManager = (STDUnitManager)eHRPageServer.GetPalauObject(typeof(STDUnitManager));
                        unitData = unitManager.GetCurentUnitDataByID(companyID.Trim());
                        if (unitData != null)
                            returnStr = unitData.UnitCode;
                        break;
                    default:
                        break;
                }
                if (returnStr != "")
                    returnStr = this.GetPsnInforOfEmp(psnSort, Convert.ToInt32(psnLength), returnStr);
                return returnStr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 如果原流水号小于max-add
        /// </summary>
        /// <param name="length">流水号长度</param>
        /// <param name="oldnum">流水号当前基准号</param>
        /// <param name="addvalue">流水号增量</param>
        /// <returns></returns>
        private string GetNewNumber(int length, int baseNum, int addNum)
        {
            int newNum = baseNum + addNum;
            string newNumber = newNum.ToString();
            int nowlen = newNumber.Length;
            if (nowlen < length)
            {
                for (int i = 0; i < length - nowlen; i++)
                {
                    newNumber = "0" + newNumber;
                }
            }
            return newNumber;
        }
        /// <summary>
        /// 根据规则类型获取该规则的所有规则构成(包括管理单元级和系统级)
        /// </summary>
        /// <param name="numberType">规则类型(工号--EMPLOYEEID,合同编号--CONTRACTNUM,协议编号--PROTOCOLNUM,变更编号--ALTERATIONNUM)</param>
        /// <returns></returns>
        private DataSet GetRuleDataByNumberType(string numberType)
        {
            //需要把该管理单元下的规则和系统级规则全部获取
            try
            {
                EmpIDRoleManager rulemanager = (EmpIDRoleManager)eHRPageServer.GetPalauObject(typeof(EmpIDRoleManager));
                DataSet ruleDS = rulemanager.GetAllRuleDataOfManageUnit(numberType, base.getBusinessUnitID().Trim());
                return ruleDS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取相应格式的日期值字符串
        /// </summary>
        /// <param name="dateValue">日期值</param>
        /// <param name="dateFormat">日期格式</param>
        /// <returns></returns>
        public string GetDateStrByDateFormat(DateTime dateValue, string dateFormat)
        {
            string dateStr = "";
            switch (dateFormat)
            {
                case "YYYYMMDD":
                    dateStr = this.getYYYY(dateValue) + this.getMM(dateValue) + this.getDD(dateValue);
                    break;
                case "YYYYMM":
                    dateStr = this.getYYYY(dateValue) + this.getMM(dateValue);
                    break;
                case "YYYY":
                    dateStr = this.getYYYY(dateValue);
                    break;
                case "YYMMDD":
                    dateStr = this.getYY(dateValue) + this.getMM(dateValue) + this.getDD(dateValue);
                    break;
                case "YYMM":
                    dateStr = this.getYY(dateValue) + this.getMM(dateValue);
                    break;
                case "YY":
                    dateStr = this.getYY(dateValue);
                    break;
                case "MMDD":
                    dateStr = this.getMM(dateValue) + this.getDD(dateValue);
                    break;
                case "MM":
                    dateStr = this.getMM(dateValue);
                    break;
                case "DD":
                    dateStr = this.getDD(dateValue);
                    break;
                default:
                    dateStr = "";
                    break;
            }
            return dateStr;
        }
        private string getYYYY(DateTime date)
        {
            string yearYYYY = date.Year.ToString();
            return yearYYYY;
        }
        private string getYY(DateTime date)
        {
            string yearYY = date.Year.ToString();
            yearYY = yearYY.Substring(2, 2);
            return yearYY;
        }
        private string getMM(DateTime date)
        {
            string monthMM = date.Month.ToString();
            if (monthMM.Length < 2)
                monthMM = "0" + monthMM;
            return monthMM;
        }
        private string getDD(DateTime date)
        {
            string dayDD = date.Day.ToString();
            if (dayDD.Length < 2)
                dayDD = "0" + dayDD;
            return dayDD;
        }
        /// <summary>
        /// 根据个性化取值顺序和长度获取个性化取值字符串
        /// </summary>
        /// <param name="psnSort">取值顺序</param>
        /// <param name="psnLen">取值长度</param>
        /// <param name="psnInforValue">个性化值</param>
        /// <returns></returns>
        private string GetPsnInforOfEmp(string psnSort, int psnLen, string psnInforValue)
        {
            string psnInforStr = "";
            int fullLength = psnInforValue.Length;
            if (fullLength <= psnLen)
                psnInforStr = psnInforValue;
            else
            {
                //从首部开始
                if (psnSort == "fromstart")
                {
                    psnInforStr = psnInforValue.Substring(0, psnLen);
                }
                //从尾部开始
                if (psnSort == "fromend")
                {
                    psnInforStr = psnInforValue.Substring(fullLength - psnLen, psnLen);
                }
            }
            return psnInforStr;
        }
        #endregion
    }
}


