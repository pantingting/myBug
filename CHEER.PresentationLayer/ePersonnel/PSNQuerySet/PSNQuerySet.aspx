<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PSNQuerySet.aspx.cs" Inherits="CHEER.PresentationLayer.ePersonnel.PSNQuerySet.PSNQuerySet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" OnCustomEvent="PageManager1_CustomEvent" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" />
        <c:Panel runat="server" ID="panelMain" ShowBorder="false" ShowHeader="false" BodyPadding="5px">
            <Items>
                <c:Panel runat="server" ID="Panel1" ShowHeader="false" ShowBorder="true" BodyPadding="5px">
                    <Toolbars>
                        <c:Toolbar runat="server">
                            <Items>
                                <c:Button runat="server" ID="btnSave" Text="保存" IconFont="Save" OnClick="btnSave_Click"></c:Button>
                            </Items>
                        </c:Toolbar>
                    </Toolbars>
                    <Items>
                        <c:DropDownList runat="server" ID="drpReportType" Label="类型名称"></c:DropDownList>
                        <c:Grid EnableCollapse="true" EnableMultiSelect="false" runat="server" DataKeyNames="QCSFIELD" Margin="5px 0 0 0" ID="grdMain" Title="查询条件字段设置" EnableColumnLines="true" ClicksToEdit="2" AllowCellEditing="true">
                        </c:Grid>
                        <c:Grid EnableCollapse="true" runat="server" DataKeyNames="QRSDISPLAYFIELD" ID="grdMain1" Margin="5px 0 0 0" Title="表格字段设置" EnableColumnLines="true" ClicksToEdit="2" AllowCellEditing="true">
                        </c:Grid>
                        <c:Grid EnableCollapse="true" runat="server" DataKeyNames="QDSFIELD" ID="grdMain2" Margin="5px 0 0 0" Title="详细页面字段设置" EnableColumnLines="true" ClicksToEdit="2" AllowCellEditing="true">
                        </c:Grid>
                        <c:Grid EnableCollapse="true" runat="server" DataKeyNames="QASFIELD" ID="grdMain3" Margin="5px 0 0 0" Title="新增字段设置" EnableColumnLines="true" ClicksToEdit="2" AllowCellEditing="true">
                        </c:Grid>
                    </Items>
                </c:Panel>
            </Items>
        </c:Panel>
        <c:Label runat="server" ID="txtReportID" Hidden="true"></c:Label>
        <c:Label runat="server" ID="txtName" Hidden="true"></c:Label>
    </form>
    <script>

        var Data = "[{'FieldID':'BRANCHID','ZH-CN':'部门' ,'EN-US':'Organization','ZH-TW':'部門'},";
        Data += "{'FieldID':'EMPLOYEEID','ZH-CN':'工号' ,'EN-US':'EmployeeID','ZH-TW':'工號'},";
        Data += "{'FieldID':'TRUENAME','ZH-CN':'姓名' ,'EN-US':'Name','ZH-TW':'姓名'},";
        Data += "{'FieldID':'ACCESSIONSTATE','ZH-CN':'在职状态' ,'EN-US':'In-service Status','ZH-TW':'在職狀態'},";
        Data += "{'FieldID':'ATTENDONDATE','ZH-CN':'入职日' ,'EN-US':'On Hire Date','ZH-TW':'入職日'},";
        Data += "{'FieldID':'CERTIFICATETYPEID','ZH-CN':'证件类型' ,'EN-US':'ID Type','ZH-TW':'證件類型'},";
        Data += "{'FieldID':'CERTIFICATENUMBER','ZH-CN':'证件号码' ,'EN-US':'Certificate ID','ZH-TW':'證件號碼'},";
        Data += "{'FieldID':'GENDER','ZH-CN':'性别' ,'EN-US':'Gender','ZH-TW':'性別'},";
        Data += "{'FieldID':'CARDNUM','ZH-CN':'卡号' ,'EN-US':'Card No.','ZH-TW':'卡號'},";
        Data += "{'FieldID':'STARTSERVICEDATE','ZH-CN':'工龄起算日' ,'EN-US':'Seniority Calculation Begin Date','ZH-TW':'工齡起算日'},";
        Data += "{'FieldID':'EmergencyContact','ZH-CN':'紧急联系人' ,'EN-US':'Emergency contact','ZH-TW':'緊急聯系人'},";
        Data += "{'FieldID':'EmergencyContactNo','ZH-CN':'紧急联系人电话' ,'EN-US':'Emergency contact telephone','ZH-TW':'緊急聯系人電話'},";
        Data += "{'FieldID':'CELLPHONE','ZH-CN':'手机号码' ,'EN-US':'Mobile','ZH-TW':'手機號碼'},";
        Data += "{'FieldID':'COMPANYEMAIL','ZH-CN':'内部信箱' ,'EN-US':'Internal Email','ZH-TW':'內部信箱'},";
        Data += "{'FieldID':'CONTACTADDRESS','ZH-CN':'联系地址' ,'EN-US':'Contact Address','ZH-TW':'聯系地址'},";
        Data += "{'FieldID':'POSTCODE','ZH-CN':'邮政编码' ,'EN-US':'Postcode','ZH-TW':'郵政編碼'},";
        Data += "{'FieldID':'JOBCODE','ZH-CN':'职位' ,'EN-US':'Job','ZH-TW':'職位'},";
        Data += "{'FieldID':'RESPONSIBILITYID','ZH-CN':'职务' ,'EN-US':'Title','ZH-TW':'職務'},";
        Data += "{'FieldID':'TITLEID','ZH-CN':'职称' ,'EN-US':'Level','ZH-TW':'職稱'},";
        Data += "{'FieldID':'RANKID','ZH-CN':'职等' ,'EN-US':'Position Level','ZH-TW':'職等'},";
        Data += "{'FieldID':'PAYROLLRANKID','ZH-CN':'薪资级别' ,'EN-US':'Salary level','ZH-TW':'薪資級別'},";
        Data += "{'FieldID':'NATIVEPLACEPROPERTYID','ZH-CN':'户籍性质' ,'EN-US':'Family Identification Type','ZH-TW':'戶籍性質'},";
        Data += "{'FieldID':'JOBTYPEID','ZH-CN':'工种' ,'EN-US':'Job Specialty','ZH-TW':'工種'},";
        Data += "{'FieldID':'ARRANGEMENTID','ZH-CN':'编制' ,'EN-US':'Organizational Structure','ZH-TW':'編制'},";
        Data += "{'FieldID':'BELONGCORPID','ZH-CN':'地区' ,'EN-US':'Area','ZH-TW':'地區'},";
        Data += "{'FieldID':'EMPLOYEECHARID','ZH-CN':'员工性质' ,'EN-US':'Employee Character','ZH-TW':'員工性質'},";
        Data += "{'FieldID':'DIMISSIONDATE','ZH-CN':'离职日期' ,'EN-US':'Dimission Date','ZH-TW':'離職日期'},";
        Data += "{'FieldID':'COMPANYBUS','ZH-CN':'厂车' ,'EN-US':'Factory Car','ZH-TW':'廠車'},";
        Data += "{'FieldID':'DATEOFBIRTH','ZH-CN':'出生日期' ,'EN-US':'Birthday','ZH-TW':'出生日期'},";
        Data += "{'FieldID':'NATIONID','ZH-CN':'民族' ,'EN-US':'Nationality','ZH-TW':'民族'},";
        Data += "{'FieldID':'NATIONALITYID','ZH-CN':'国籍' ,'EN-US':'Nationality','ZH-TW':'國籍'},";
        Data += "{'FieldID':'FIRSTNAME','ZH-CN':'FirstName' ,'EN-US':'FirstName','ZH-TW':'FirstName'},";
        Data += "{'FieldID':'MIDDLENAME','ZH-CN':'MiddleName' ,'EN-US':'MiddleName','ZH-TW':'MiddleName'},";
        Data += "{'FieldID':'LASTNAME','ZH-CN':'LastName' ,'EN-US':'LastName','ZH-TW':'LastName'},";
        Data += "{'FieldID':'HOMEPLACE','ZH-CN':'籍贯' ,'EN-US':'Native Place','ZH-TW':'籍貫'},";
        Data += "{'FieldID':'POLICALFACEID','ZH-CN':'政治面貌' ,'EN-US':'Political Status','ZH-TW':'政治面貌'},";
        Data += "{'FieldID':'MARRIAGEID','ZH-CN':'婚姻状况' ,'EN-US':'Marital Status','ZH-TW':'婚姻狀況'},";
        Data += "{'FieldID':'GRADUATESCHOOLID','ZH-CN':'毕业院校' ,'EN-US':'Graduated Universities','ZH-TW':'畢業院校'},";
        Data += "{'FieldID':'EMPLOYEETYPEID','ZH-CN':'员工类别' ,'EN-US':'Employee Type','ZH-TW':'員工類別'},";
        Data += "{'FieldID':'SOCIETYENSURENUM','ZH-CN':'社保号码' ,'EN-US':'Social Insurance ID','ZH-TW':'社保號碼'},";
        Data += "{'FieldID':'EMPGROUP','ZH-CN':'员工组' ,'EN-US':'Employee Group','ZH-TW':'員工組'},";
        Data += "{'FieldID':'REMARK','ZH-CN':'备注' ,'EN-US':'Remark','ZH-TW':'備註'},";
        Data += "{'FieldID':'SHIFTGROUP','ZH-CN':'班组' ,'EN-US':'ShiftGroup','ZH-TW':'班組'},";
        Data += "{'FieldID':'ActivityArrayID','ZH-CN':'活动总览' ,'EN-US':'Activities overview','ZH-TW':'活動總覽'},";
        Data += "{'FieldID':'CurrentTeamID','ZH-CN':'当前团队' ,'EN-US':'The current team','ZH-TW':'當前團隊'},";
        Data += "{'FieldID':'DOCUMENTCODE','ZH-CN':'档案号' ,'EN-US':'File No.','ZH-TW':'檔案號'},";
        Data += "{'FieldID':'INDATE','ZH-CN':'进档案时间' ,'EN-US':'Filing Date','ZH-TW':'進檔案時間'},";
        Data += "{'FieldID':'OUTDATE','ZH-CN':'出档案时间' ,'EN-US':'File Transfer Out Date','ZH-TW':'出檔案時間'},";
        Data += "{'FieldID':'AnnualLeave','ZH-CN':'年假' ,'EN-US':'Annual leave ','ZH-TW':'年假'},";
        Data += "{'FieldID':'VacationDays','ZH-CN':'年假时数' ,'EN-US':'Vacation hours ','ZH-TW':'年假时数'},";
        Data += "{'FieldID':'AttendanceWay','ZH-CN':'出勤方式' ,'EN-US':'Attendance way','ZH-TW':'出勤方式'},";
        Data += "{'FieldID':'overtimeSettlementUpper','ZH-CN':'标准工时加班费结算上限' ,'EN-US':'Standard working hours overtime settlement upper limit','ZH-TW':'標準工時加班費結算上限'},";
        Data += "{'FieldID':'ComAccountWorkingHours','ZH-CN':'综合工时账户' ,'EN-US':'Comprehensive account of working hours','ZH-TW':'綜合工時賬戶'},";
        Data += "{'FieldID':'IsAttendance','ZH-CN':'考勤' ,'EN-US':'Attendance','ZH-TW':'考勤'},";
        Data += "{'FieldID':'OvertimePayment','ZH-CN':'加班结算方式' ,'EN-US':'Overtime payment','ZH-TW':'加班結算方式'},";
        Data += "{'FieldID':'ot1','ZH-CN':'Ot1' ,'EN-US':'Ot1','ZH-TW':'Ot1'},";
        Data += "{'FieldID':'ot2','ZH-CN':'Ot2' ,'EN-US':'Ot2','ZH-TW':'Ot2'},";
        Data += "{'FieldID':'ot3','ZH-CN':'Ot3' ,'EN-US':'Ot3','ZH-TW':'Ot3'},";
        Data += "{'FieldID':'Costcenter','ZH-CN':'Costcenter' ,'EN-US':'Costcenter','ZH-TW':'Costcenter'},";
        Data += "{'FieldID':'age','ZH-CN':'年龄' ,'EN-US':'Age','ZH-TW':'年齡'},";
        Data += "{'FieldID':'Entrychannels','ZH-CN':'入职渠道' ,'EN-US':'The Entry Channel','ZH-TW':'入職渠道'},";
        Data += "{'FieldID':'Reportingto','ZH-CN':'汇报对象' ,'EN-US':'Report object','ZH-TW':'匯報對象'},";
        Data += "{'FieldID':'HealthExamination','ZH-CN':'健康体检' ,'EN-US':'Health examination','ZH-TW':'健康體檢'},";
        Data += "{'FieldID':'firstworktime','ZH-CN':'第一次职位开始日期' ,'EN-US':'First position the start date','ZH-TW':'第壹次職位開始日期'},";
        Data += "{'FieldID':'EXT','ZH-CN':'分机' ,'EN-US':'Ext','ZH-TW':'分機'},";
        Data += "{'FieldID':'EDUCATIONALLEVELID','ZH-CN':'学历' ,'EN-US':'Education Qualification','ZH-TW':'學歷'},";
        Data += "{'FieldID':'PRODUCTLINE','ZH-CN':'线别' ,'EN-US':'Line stop','ZH-TW':'線別'},";
        Data += "{'FieldID':'PROBATIONENDDATE','ZH-CN':'试满日' ,'EN-US':'Probation Finish Date','ZH-TW':'試滿日'},";
        Data += "{'FieldID':'DLIDL','ZH-CN':'IDL' ,'EN-US':'IDL','ZH-TW':'IDL'},";
        Data += "{'FieldID':'IsInterns','ZH-CN':'实习生' ,'EN-US':'Interns','ZH-TW':'實習生'},";

        Data += "{'FieldID':'TransCard','ZH-CN':'实习生' ,'EN-US':'WaitTransferCardNumber','ZH-TW':'待轉換卡號'},";

        Data += "{'FieldID':'Accountplace','ZH-CN':'户口所在地' ,'EN-US':'Permanent Residence','ZH-TW':'戶口所在地'}]";


        var US = "EN-US";
        var CN = "ZH-CN";
        var TW = "ZH-TW";
        var STRING = "STRING";
        var INT = "INT";
        var DATE = "DATE";
        var DEPT = "DEPT";
        var DEFAULTLIST = "DEFAULTLIST";
        var SQLLIST = "SQLLIST";
        var BOOL = "BOOL";
        var STATE = "STATE";

        //在职状态
        var Probation = "<%=getString("ZGAIA00565") %>";
        var Regular = "<%=getString("ZGAIA00564") %>";
        var Retired = "<%=getString("ZGAIA01149") %>";
        var Dimission = "<%=getString("ZGAIA01150") %>";
        var Export = "<%=getString("ZGAIA01152") %>";
        var PromotingProbation = "<%=getString("ZGAIA01151") %>";
        var Unchecked = "<%=getString("ZGAIA01975") %>";
        var Unknown = "<%=getString("ZGAIA01848") %>";

        //性别
        var Male = "<%=getString("ZGAIA00309") %>";
        var Female = "<%=getString("ZGAIA00313") %>";
        //年假
        var YesValue = "<%=getString("ZGAIA00254") %>";
        var NOValue = "<%=getString("ZGAIA00785") %>";

        //在职值
        var ACCESSIONSTATEValue = "1|" + Probation + ",2|" + Regular + ",3|" + Retired + ",4|" + Dimission + ",5|" + Export + ",6|" + PromotingProbation + ",7|" + Unchecked;
        //性别值
        var GENDERValue = "2|" + Male + ",1|" + Female;
        //年假
        var AnnualLeaveValue = "0|" + NOValue + ",1|" + YesValue;
        //综合工时帐户 
        var ComAccountWorkingHoursValue = "0|" + NOValue + ",1|" + YesValue;
        //考勤
        var IsAttendanceValue = "0|" + NOValue + ",1|" + YesValue;
        //实习生    
        var IsInternsValue = "0|" + NOValue + ",1|" + YesValue;
        var N0 = "NO";
        var YES = "YES";

        function renderFellField(value, metaData, record, rowIndex, colIndex, store, view) {
            var newValue = C('<%= ddlfillField.ClientID%>').getTextByValue(value);
            return newValue;
        }

        function renderFellField1(value, metaData, record, rowIndex, colIndex, store, view) {
            var newValue = C('<%= ddlfillField1.ClientID%>').getTextByValue(value);
            return newValue;
        }

        function renderFellField2(value, metaData, record, rowIndex, colIndex, store, view) {
            var newValue = C('<%= ddlfillField2.ClientID%>').getTextByValue(value);
            return newValue;
        }

        function renderFellField3(value, metaData, record, rowIndex, colIndex, store, view) {
            var newValue = C('<%= ddlfillField3.ClientID%>').getTextByValue(value);
            return newValue;
        }

        function renderFellType(value, metaData, record, rowIndex, colIndex, store, view) {
            var newValue = C('<%= ddlfillType.ClientID%>').getTextByValue(value);
            return newValue;
        }

        function renderFellType2(value, metaData, record, rowIndex, colIndex, store, view) {
            var newValue = C('<%= ddlfillType2.ClientID%>').getTextByValue(value);
            return newValue;
        }

        function renderFellType3(value, metaData, record, rowIndex, colIndex, store, view) {
            var newValue = C('<%= ddlfillType3.ClientID%>').getTextByValue(value);
            return newValue;
        }

        function renderFuzzyQuery(value, metaData, record, rowIndex, colIndex, store, view) {
            var newValue = C('<%= ddlfuzzyQuery.ClientID%>').getTextByValue(value);
            return newValue;
        }

        function renderIsReadonly(value, metaData, record, rowIndex, colIndex, store, view) {
            var newValue = C('<%= ddlisReadonly.ClientID%>').getTextByValue(value);
            return newValue;
        }

        function renderMveItems(value, metaData, record, rowIndex, colIndex, store, view) {
            var newValue = C('<%= ddlmveItems.ClientID%>').getTextByValue(value);
            return newValue;
        }

        function renderRequired(value, metaData, record, rowIndex, colIndex, store, view) {
            var newValue = C('<%= ddlRequired.ClientID%>').getTextByValue(value);
            return newValue;
        }

        C.ready(function () {
            C('<%=grdMain.ClientID%>').on('rowclick', function (event, index) {
                var dataObj =
                        {
                            Add: this.getCellValue(0, 'Add'),
                            Delete: this.getCellValue(0, 'Delete'),
                            QCSFIELD: "",
                            QCSTITLE: "",
                            QCS4: "",
                            QCS5: "",
                            QCSTYPE: "",
                            QCSISDIM: "",
                            QCSDEFAULTVALUE: "",
                            QCSVALUELIST: "",
                            QCSORDER: this.data.length + 1
                        }
                if (this.getSelectedCellColumnId() === 'Add') {
                    this.addNewRecord(dataObj, true);
                    this.selectRows([this.data.length - 1 ]);
                }
                if (this.getSelectedCellColumnId() === 'Delete') {
                    this.deleteSelectedRows();
                }
            });

            C('<%=grdMain1.ClientID%>').on('rowclick', function (event, index) {
                var dataObj =
                        {
                            Add: this.getCellValue(0, 'Add'),
                            Delete: this.getCellValue(0, 'Delete'),
                            QRSDISPLAYFIELD: "",
                            QRSTITLE: "",
                            QRS4: "",
                            QRS5: "",
                            QRSORDER: this.data.length + 1
                        }
                if (this.getSelectedCellColumnId() === 'Add') {
                    this.addNewRecord(dataObj, true);
                    this.selectRows([this.data.length - 1 ]);
                }
                if (this.getSelectedCellColumnId() === 'Delete') {
                    this.deleteSelectedRows();
                }
            });

            C('<%=grdMain2.ClientID%>').on('rowclick', function (event, index) {
                var dataObj =
                        {
                            Add: this.getCellValue(0, 'Add'),
                            Delete: this.getCellValue(0, 'Delete'),
                            QDSFIELD: "",
                            QDSNAME: "",
                            QDS4: "",
                            QDS5: "",
                            QDSTYPE: "",
                            QDSVALUELIST: "",
                            QDSISREAD: "",
                            QDSISCHANGE: "",
                            QDSORDER: this.data.length + 1
                        }
                if (this.getSelectedCellColumnId() === 'Add') {
                    this.addNewRecord(dataObj, true);
                    this.selectRows([this.data.length - 1]);
                }
                if (this.getSelectedCellColumnId() === 'Delete') {
                    this.deleteSelectedRows();
                }
            });

            C('<%=grdMain3.ClientID%>').on('rowclick', function (event, index) {
                var dataObj =
                        {
                            Add: this.getCellValue(0, 'Add'),
                            Delete: this.getCellValue(0, 'Delete'),
                            QASFIELD: "",
                            QASTITLE: "",
                            QAS4: "",
                            QAS5: "",
                            QASTYPE: "",
                            QASVALUELIST: "",
                            QASDEFAULTVALUE: "",
                            QAISMUSTIN: "",
                            QASORDER: this.data.length + 1
                        }
                if (this.getSelectedCellColumnId() === 'Add') {
                    this.addNewRecord(dataObj, true);
                    this.selectRows([this.data.length - 1 ]);
                }
                if (this.getSelectedCellColumnId() === 'Delete') {
                    this.deleteSelectedRows();
                }
            });

            var JsonData = eval('(' + Data + ')');

            C('<%=ddlfillField.ClientID%>').on('select', function (event) {
                var grid = this.parent;
                var gram1 = this.getValue();
                var selectedRecord = grid.getRowValue(grid.getSelectedRows()[0]);
                selectedRecord.set = function (id, value) {
                    grid.updateCellValue(grid.getSelectedRows()[0], id, value);
                }
                selectedRecord.set('QCSFIELD', gram1);
                for (var i in JsonData) {
                    if (JsonData[i]["FieldID"] == gram1) {
                        selectedRecord.set('QCSTITLE', JsonData[i]["ZH-CN"]);
                        selectedRecord.set('QCS4', JsonData[i]["EN-US"]);
                        selectedRecord.set('QCS5', JsonData[i]["ZH-TW"]);
                    }
                }
                if (gram1 == "ACCESSIONSTATE" || gram1 == "GENDER" || gram1 == "AnnualLeave" || gram1 == "ComAccountWorkingHours" || gram1 == "IsAttendance" || gram1 == "IsInterns") { //自定义列表类型
                    selectedRecord.set('QCSTYPE', DEFAULTLIST);
                    selectedRecord.set('QCSISDIM', N0);
                    if (gram1 == "ACCESSIONSTATE") {
                        selectedRecord.set('QCSVALUELIST', ACCESSIONSTATEValue);
                    }
                    if (gram1 == "GENDER") //性别
                    {
                        selectedRecord.set("QCSVALUELIST", GENDERValue);
                    }
                    if (gram1 == "AnnualLeave")//年假
                    {
                        selectedRecord.set("QCSVALUELIST", AnnualLeaveValue);
                    }

                    if (gram1 == "ComAccountWorkingHours")//综合工时帐户
                    {
                        selectedRecord.set("QCSVALUELIST", ComAccountWorkingHoursValue);
                    }
                    if (gram1 == "IsAttendance")//考勤
                    {
                        selectedRecord.set("QCSVALUELIST", IsAttendanceValue);
                    }
                    if (gram1 == "IsInterns")//实习生
                    {
                        selectedRecord.set("QCSVALUELIST", IsInternsValue);
                    }
                } else if (gram1 == "NATIONID" || gram1 == "DLIDL" || gram1 == "NATIONALITYID" || gram1 == "CERTIFICATETYPEID" || gram1 == "Costcenter" || gram1 == "POLICALFACEID" || gram1 == "JOBTYPEID" ||
                    gram1 == "EMPLOYEETYPEID" || gram1 == "MARRIAGEID" || gram1 == "EMPLOYEECHARID" || gram1 == "ARRANGEMENTID" || gram1 == "Sql1" || gram1 == "Sql2" ||
                    gram1 == "BELONGCORPID" || gram1 == "NATIVEPLACEPROPERTYID" || gram1 == "EMPGROUP" || gram1 == "COMPANYBUS" || gram1 == "PRODUCTLINE" || gram1 == "EDUCATIONALLEVELID" ||
                    gram1 == "JOBCODE" || gram1 == "RESPONSIBILITYID" || gram1 == "TITLEID" || gram1 == "RANKID" || gram1 == "PAYROLLRANKID" ||
	                gram1 == "AttendanceWay" || gram1 == "OvertimePayment" || gram1 == "SHIFTGROUP") {//SQL列表型
                    selectedRecord.set('QCSTYPE', SQLLIST);
                    selectedRecord.set('QCSISDIM', N0);
                    if (gram1 == "NATIONID")
                    { selectedRecord.set("QCSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='32'"); }
                    if (gram1 == "DLIDL")
                    { selectedRecord.set("QCSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='S3'"); }
                    if (gram1 == "NATIONALITYID")
                    { selectedRecord.set("QCSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='1'"); }
                    if (gram1 == "CERTIFICATETYPEID")
                    { selectedRecord.set("QCSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='0'"); }
                    if (gram1 == "Costcenter")
                    { selectedRecord.set("QCSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='122'"); }
                    if (gram1 == "Sql1")
                    { selectedRecord.set("QCSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='123'"); }
                    if (gram1 == "Sql2")
                    { selectedRecord.set("QCSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='124'"); }
                    if (gram1 == "POLICALFACEID")
                    { selectedRecord.set("QCSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='33'"); }
                    if (gram1 == "JOBTYPEID")
                    { selectedRecord.set("QCSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='5'"); }

                    if (gram1 == "EMPLOYEETYPEID")
                    { selectedRecord.set("QCSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='16'"); }
                    if (gram1 == "MARRIAGEID")
                    { selectedRecord.set("QCSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='17'"); }
                    if (gram1 == "EMPLOYEECHARID")
                    { selectedRecord.set("QCSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='29'"); }
                    if (gram1 == "ARRANGEMENTID")
                    { selectedRecord.set("QCSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='6'"); }
                    if (gram1 == "BELONGCORPID")
                    { selectedRecord.set("QCSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='30'"); }

                    if (gram1 == "NATIVEPLACEPROPERTYID")
                    { selectedRecord.set("QCSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='2'"); }
                    if (gram1 == "EMPGROUP")
                    { selectedRecord.set("QCSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='S'"); }
                    if (gram1 == "COMPANYBUS")
                    { selectedRecord.set("QCSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='S0'"); }
                    if (gram1 == "PRODUCTLINE")
                    { selectedRecord.set("QCSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='S1'"); }
                    if (gram1 == "EDUCATIONALLEVELID")
                    { selectedRecord.set("QCSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='3'"); }

                    if (gram1 == "JOBCODE")
                    { selectedRecord.set("QCSVALUELIST", "select JOBCODEID,NAMES From CPCJOBCODE WHERE ISEFFECT=1 AND ISDELETED=0"); }

                    if (gram1 == "RESPONSIBILITYID")
                    { selectedRecord.set("QCSVALUELIST", "select POSIITEMID,NAMES from CPCPOSIITEM where POSIGROUPID='272e2357-3a4a-4bb3-8ae4-ff0d015c0683'  and ISDELETED='0'"); }
                    if (gram1 == "TITLEID")
                    { selectedRecord.set("QCSVALUELIST", "select POSIITEMID,NAMES from CPCPOSIITEM where POSIGROUPID='0ec53b9d-2b70-4d31-8eea-ad6fdb33febf'  and ISDELETED='0'"); }
                    if (gram1 == "RANKID")
                    { selectedRecord.set("QCSVALUELIST", "select POSITYPEID,NAMES from CPCLIBPOSITYPE where ISDELETED='0'"); }

                    if (gram1 == "PAYROLLRANKID")
                    { selectedRecord.set("QCSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where typeid='S2'"); }
                    if (gram1 == "AttendanceWay")
                    { selectedRecord.set("QCSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where typeid='59'"); }
                    if (gram1 == "OvertimePayment")
                    { selectedRecord.set("QCSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where typeid='60'"); }
                    if (gram1 == "SHIFTGROUP")
                    { selectedRecord.set("QCSVALUELIST", " select GROUPID,GROUPNAME from ATDSHIFTGROUP "); }
                } else if (gram1 == "DATEOFBIRTH" || gram1 == "DIMISSIONDATE" || gram1 == "date1" || gram1 == "date2" || gram1 == "date3"
                 || gram1 == "ATTENDONDATE" || gram1 == "STARTSERVICEDATE" || gram1 == "PROBATIONENDDATE" ||
                    gram1 == "ENTERSOCIETYDATE" || gram1 == "INDATE" || gram1 == "OUTDATE")  //日期型
                {
                    selectedRecord.set('QCSTYPE', DATE);
                    selectedRecord.set('QCSISDIM', N0);
                } else if (gram1 == "BRANCHID")  //组织列表
                {
                    selectedRecord.set('QCSTYPE', DEPT);
                    selectedRecord.set('QCSISDIM', N0);
                } else if (gram1 == "ACCOUNTITME1")   //BOOL型
                {
                    selectedRecord.set('QCSTYPE', BOOL);
                    selectedRecord.set('QCSISDIM', N0);
                } else if (gram1 == "VacationDays" || gram1 == "overtimeSettlementUpper")//数值类型
                {
                    selectedRecord.set('QCSTYPE', INT);
                    selectedRecord.set('QCSISDIM', N0);
                } else  //其他类型
                {
                    selectedRecord.set('QCSTYPE', STRING);
                    selectedRecord.set('QCSISDIM', YES);
                }
            });

            C('<%=ddlfillField1.ClientID%>').on('select', function (event) {
                var grid = this.parent;
                var gram1 = this.getValue();
                var selectedRecord = grid.getRowValue(grid.getSelectedRows()[0]);
                selectedRecord.set = function (id, value) {
                    grid.updateCellValue(grid.getSelectedRows()[0], id, value);
                }
                selectedRecord.set('QRSDISPLAYFIELD', gram1);
                for (var i in JsonData) {
                    if (JsonData[i]["FieldID"] == gram1) {
                        selectedRecord.set('QRSTITLE', JsonData[i]["ZH-CN"]);
                        selectedRecord.set('QRS4', JsonData[i]["EN-US"]);
                        selectedRecord.set('QRS5', JsonData[i]["ZH-TW"]);
                    }
                }
            });


            C('<%=ddlfillField2.ClientID%>').on('select', function (event) {
                var grid = this.parent;
                var gram1 = this.getValue();
                var selectedRecord = grid.getRowValue(grid.getSelectedRows()[0]);
                selectedRecord.set = function (id, value) {
                    grid.updateCellValue(grid.getSelectedRows()[0], id, value);
                }
                selectedRecord.set('QDSFIELD', gram1);
                for (var i in JsonData) {
                    if (JsonData[i]["FieldID"] == gram1) {
                        selectedRecord.set('QDSTITLE', JsonData[i]["ZH-CN"]);
                        selectedRecord.set('QDS4', JsonData[i]["EN-US"]);
                        selectedRecord.set('QDS5', JsonData[i]["ZH-TW"]);
                    }
                }
                if (gram1 == "ACCESSIONSTATE" || gram1 == "GENDER" || gram1 == "AnnualLeave" || gram1 == "ComAccountWorkingHours" || gram1 == "IsAttendance" || gram1 == "IsInterns") { //自定义列表类型
                    selectedRecord.set('QDSTYPE', DEFAULTLIST);
                    selectedRecord.set('QDSISDIM', N0);
                    if (gram1 == "ACCESSIONSTATE") {
                        selectedRecord.set('QDSVALUELIST', ACCESSIONSTATEValue);
                    }
                    if (gram1 == "GENDER") //性别
                    {
                        selectedRecord.set("QDSVALUELIST", GENDERValue);
                    }
                    if (gram1 == "AnnualLeave")//年假
                    {
                        selectedRecord.set("QDSVALUELIST", AnnualLeaveValue);
                    }

                    if (gram1 == "ComAccountWorkingHours")//综合工时帐户
                    {
                        selectedRecord.set("QDSVALUELIST", ComAccountWorkingHoursValue);
                    }
                    if (gram1 == "IsAttendance")//考勤
                    {
                        selectedRecord.set("QDSVALUELIST", IsAttendanceValue);
                    }
                    if (gram1 == "IsInterns")//实习生
                    {
                        selectedRecord.set("QDSVALUELIST", IsInternsValue);
                    }
                } else if (gram1 == "NATIONID" || gram1 == "DLIDL" || gram1 == "NATIONALITYID" || gram1 == "CERTIFICATETYPEID" || gram1 == "Costcenter" || gram1 == "POLICALFACEID" || gram1 == "JOBTYPEID" ||
                    gram1 == "EMPLOYEETYPEID" || gram1 == "MARRIAGEID" || gram1 == "EMPLOYEECHARID" || gram1 == "ARRANGEMENTID" || gram1 == "Sql1" || gram1 == "Sql2" ||
                    gram1 == "BELONGCORPID" || gram1 == "NATIVEPLACEPROPERTYID" || gram1 == "EMPGROUP" || gram1 == "COMPANYBUS" || gram1 == "PRODUCTLINE" || gram1 == "EDUCATIONALLEVELID" ||
                    gram1 == "JOBCODE" || gram1 == "RESPONSIBILITYID" || gram1 == "TITLEID" || gram1 == "RANKID" || gram1 == "PAYROLLRANKID" ||
	    gram1 == "AttendanceWay" || gram1 == "OvertimePayment" || gram1 == "SHIFTGROUP") {//SQL列表型
                    selectedRecord.set('QDSTYPE', SQLLIST);
                    selectedRecord.set('QDSISDIM', N0);
                    if (gram1 == "NATIONID")
                    { selectedRecord.set("QDSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='32'"); }
                    if (gram1 == "DLIDL")
                    { selectedRecord.set("QDSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='S3'"); }
                    if (gram1 == "NATIONALITYID")
                    { selectedRecord.set("QDSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='1'"); }
                    if (gram1 == "CERTIFICATETYPEID")
                    { selectedRecord.set("QDSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='0'"); }
                    if (gram1 == "Costcenter")
                    { selectedRecord.set("QDSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='122'"); }
                    if (gram1 == "Sql1")
                    { selectedRecord.set("QDSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='123'"); }
                    if (gram1 == "Sql2")
                    { selectedRecord.set("QDSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='124'"); }
                    if (gram1 == "POLICALFACEID")
                    { selectedRecord.set("QDSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='33'"); }
                    if (gram1 == "JOBTYPEID")
                    { selectedRecord.set("QDSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='5'"); }

                    if (gram1 == "EMPLOYEETYPEID")
                    { selectedRecord.set("QDSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='16'"); }
                    if (gram1 == "MARRIAGEID")
                    { selectedRecord.set("QDSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='17'"); }
                    if (gram1 == "EMPLOYEECHARID")
                    { selectedRecord.set("QDSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='29'"); }
                    if (gram1 == "ARRANGEMENTID")
                    { selectedRecord.set("QDSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='6'"); }
                    if (gram1 == "BELONGCORPID")
                    { selectedRecord.set("QDSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='30'"); }

                    if (gram1 == "NATIVEPLACEPROPERTYID")
                    { selectedRecord.set("QDSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='2'"); }
                    if (gram1 == "EMPGROUP")
                    { selectedRecord.set("QDSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='S'"); }
                    if (gram1 == "COMPANYBUS")
                    { selectedRecord.set("QDSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='S0'"); }
                    if (gram1 == "PRODUCTLINE")
                    { selectedRecord.set("QDSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='S1'"); }
                    if (gram1 == "EDUCATIONALLEVELID")
                    { selectedRecord.set("QDSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='3'"); }

                    if (gram1 == "JOBCODE")
                    { selectedRecord.set("QDSVALUELIST", "select JOBCODEID,NAMES From CPCJOBCODE WHERE ISEFFECT=1 AND ISDELETED=0"); }

                    if (gram1 == "RESPONSIBILITYID")
                    { selectedRecord.set("QDSVALUELIST", "select POSIITEMID,NAMES from CPCPOSIITEM where POSIGROUPID='272e2357-3a4a-4bb3-8ae4-ff0d015c0683'  and ISDELETED='0'"); }
                    if (gram1 == "TITLEID")
                    { selectedRecord.set("QDSVALUELIST", "select POSIITEMID,NAMES from CPCPOSIITEM where POSIGROUPID='0ec53b9d-2b70-4d31-8eea-ad6fdb33febf'  and ISDELETED='0'"); }
                    if (gram1 == "RANKID")
                    { selectedRecord.set("QDSVALUELIST", "select POSITYPEID,NAMES from CPCLIBPOSITYPE where ISDELETED='0'"); }

                    if (gram1 == "PAYROLLRANKID")
                    { selectedRecord.set("QDSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where typeid='S2'"); }
                    if (gram1 == "AttendanceWay")
                    { selectedRecord.set("QDSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where typeid='59'"); }
                    if (gram1 == "OvertimePayment")
                    { selectedRecord.set("QDSVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where typeid='60'"); }
                    if (gram1 == "SHIFTGROUP")
                    { selectedRecord.set("QDSVALUELIST", " select GROUPID,GROUPNAME from ATDSHIFTGROUP "); }
                } else if (gram1 == "DATEOFBIRTH" || gram1 == "DIMISSIONDATE" || gram1 == "date1" || gram1 == "date2" || gram1 == "date3"
                 || gram1 == "ATTENDONDATE" || gram1 == "STARTSERVICEDATE" || gram1 == "PROBATIONENDDATE" ||
                    gram1 == "ENTERSOCIETYDATE" || gram1 == "INDATE" || gram1 == "OUTDATE")  //日期型
                {
                    selectedRecord.set('QDSTYPE', DATE);
                    selectedRecord.set('QDSISDIM', N0);
                } else if (gram1 == "BRANCHID")  //组织列表
                {
                    selectedRecord.set('QDSTYPE', DEPT);
                    selectedRecord.set('QDSISDIM', N0);
                } else if (gram1 == "ACCOUNTITME1")   //BOOL型
                {
                    selectedRecord.set('QDSTYPE', BOOL);
                    selectedRecord.set('QDSISDIM', N0);
                } else if (gram1 == "VacationDays" || gram1 == "overtimeSettlementUpper")//数值类型
                {
                    selectedRecord.set('QDSTYPE', INT);
                    selectedRecord.set('QDSISDIM', N0);
                } else  //其他类型
                {
                    selectedRecord.set('QDSTYPE', STRING);
                    selectedRecord.set('QDSISDIM', YES);
                }

                if (gram1 == "ACCESSIONSTATE" || gram1 == "DIMISSIONDATE" || gram1 == "BRANCHID" ||
                   gram1 == "JOBSERIESID" || gram1 == "JOBCODE" || gram1 == "RESPONSIBILITYID" || gram1 == "TITLEID" ||
                   gram1 == "GRADEID" || gram1 == "ARRANGEMENTID" || gram1 == "JOBTYPEID" || gram1 == "NATIVEPLACEPROPERTYID" ||
                   gram1 == "EMPLOYEECHARID" || gram1 == "BELONGCORPID" || gram1 == "INDATE" || gram1 == "OUTDATE" ||
                   gram1 == "CurrentTeamID" || gram1 == "ATTENDONDATE" || gram1 == "EDUCATIONALLEVELID") {
                    selectedRecord.set("QDSISREAD", YES);
                }
                else {
                    selectedRecord.set("QDSISREAD", N0);
                }
            });

            C('<%=ddlfillField3.ClientID%>').on('select', function (event) {
                var grid = this.parent;
                var gram1 = this.getValue();
                var selectedRecord = grid.getRowValue(grid.getSelectedRows()[0]);
                selectedRecord.set = function (id, value) {
                    grid.updateCellValue(grid.getSelectedRows()[0], id, value);
                }
                selectedRecord.set('QASFIELD', gram1);
                for (var i in JsonData) {
                    if (JsonData[i]["FieldID"] == gram1) {
                        selectedRecord.set('QASTITLE', JsonData[i]["ZH-CN"]);
                        selectedRecord.set('QAS4', JsonData[i]["EN-US"]);
                        selectedRecord.set('QAS5', JsonData[i]["ZH-TW"]);
                    }
                }
                if (gram1 == "ACCESSIONSTATE" || gram1 == "GENDER" || gram1 == "AnnualLeave" || gram1 == "ComAccountWorkingHours" || gram1 == "IsAttendance" || gram1 == "IsInterns") { //自定义列表类型
                    selectedRecord.set('QASTYPE', DEFAULTLIST);
                    selectedRecord.set('QASISDIM', N0);
                    if (gram1 == "ACCESSIONSTATE") {
                        selectedRecord.set('QASVALUELIST', ACCESSIONSTATEValue);
                    }
                    if (gram1 == "GENDER") //性别
                    {
                        selectedRecord.set("QASVALUELIST", GENDERValue);
                    }
                    if (gram1 == "AnnualLeave")//年假
                    {
                        selectedRecord.set("QASVALUELIST", AnnualLeaveValue);
                    }

                    if (gram1 == "ComAccountWorkingHours")//综合工时帐户
                    {
                        selectedRecord.set("QASVALUELIST", ComAccountWorkingHoursValue);
                    }
                    if (gram1 == "IsAttendance")//考勤
                    {
                        selectedRecord.set("QASVALUELIST", IsAttendanceValue);
                    }
                    if (gram1 == "IsInterns")//实习生
                    {
                        selectedRecord.set("QASVALUELIST", IsInternsValue);
                    }
                } else if (gram1 == "NATIONID" || gram1 == "DLIDL" || gram1 == "NATIONALITYID" || gram1 == "CERTIFICATETYPEID" || gram1 == "Costcenter" || gram1 == "POLICALFACEID" || gram1 == "JOBTYPEID" ||
                    gram1 == "EMPLOYEETYPEID" || gram1 == "MARRIAGEID" || gram1 == "EMPLOYEECHARID" || gram1 == "ARRANGEMENTID" || gram1 == "Sql1" || gram1 == "Sql2" ||
                    gram1 == "BELONGCORPID" || gram1 == "NATIVEPLACEPROPERTYID" || gram1 == "EMPGROUP" || gram1 == "COMPANYBUS" || gram1 == "PRODUCTLINE" || gram1 == "EDUCATIONALLEVELID" ||
                    gram1 == "JOBCODE" || gram1 == "RESPONSIBILITYID" || gram1 == "TITLEID" || gram1 == "RANKID" || gram1 == "PAYROLLRANKID" ||
	    gram1 == "AttendanceWay" || gram1 == "OvertimePayment" || gram1 == "SHIFTGROUP") {//SQL列表型
                    selectedRecord.set('QASTYPE', SQLLIST);
                    selectedRecord.set('QASISDIM', N0);
                    if (gram1 == "NATIONID")
                    { selectedRecord.set("QASVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='32'"); }
                    if (gram1 == "DLIDL")
                    { selectedRecord.set("QASVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='S3'"); }
                    if (gram1 == "NATIONALITYID")
                    { selectedRecord.set("QASVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='1'"); }
                    if (gram1 == "CERTIFICATETYPEID")
                    { selectedRecord.set("QASVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='0'"); }
                    if (gram1 == "Costcenter")
                    { selectedRecord.set("QASVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='122'"); }
                    if (gram1 == "Sql1")
                    { selectedRecord.set("QASVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='123'"); }
                    if (gram1 == "Sql2")
                    { selectedRecord.set("QASVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='124'"); }
                    if (gram1 == "POLICALFACEID")
                    { selectedRecord.set("QASVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='33'"); }
                    if (gram1 == "JOBTYPEID")
                    { selectedRecord.set("QASVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='5'"); }

                    if (gram1 == "EMPLOYEETYPEID")
                    { selectedRecord.set("QASVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='16'"); }
                    if (gram1 == "MARRIAGEID")
                    { selectedRecord.set("QASVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='17'"); }
                    if (gram1 == "EMPLOYEECHARID")
                    { selectedRecord.set("QASVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='29'"); }
                    if (gram1 == "ARRANGEMENTID")
                    { selectedRecord.set("QASVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='6'"); }
                    if (gram1 == "BELONGCORPID")
                    { selectedRecord.set("QASVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='30'"); }

                    if (gram1 == "NATIVEPLACEPROPERTYID")
                    { selectedRecord.set("QASVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='2'"); }
                    if (gram1 == "EMPGROUP")
                    { selectedRecord.set("QASVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='S'"); }
                    if (gram1 == "COMPANYBUS")
                    { selectedRecord.set("QASVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='S0'"); }
                    if (gram1 == "PRODUCTLINE")
                    { selectedRecord.set("QASVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='S1'"); }
                    if (gram1 == "EDUCATIONALLEVELID")
                    { selectedRecord.set("QASVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='3'"); }

                    if (gram1 == "JOBCODE")
                    { selectedRecord.set("QASVALUELIST", "select JOBCODEID,NAMES From CPCJOBCODE WHERE ISEFFECT=1 AND ISDELETED=0"); }

                    if (gram1 == "RESPONSIBILITYID")
                    { selectedRecord.set("QASVALUELIST", "select POSIITEMID,NAMES from CPCPOSIITEM where POSIGROUPID='272e2357-3a4a-4bb3-8ae4-ff0d015c0683'  and ISDELETED='0'"); }
                    if (gram1 == "TITLEID")
                    { selectedRecord.set("QASVALUELIST", "select POSIITEMID,NAMES from CPCPOSIITEM where POSIGROUPID='0ec53b9d-2b70-4d31-8eea-ad6fdb33febf'  and ISDELETED='0'"); }
                    if (gram1 == "RANKID")
                    { selectedRecord.set("QASVALUELIST", "select POSITYPEID,NAMES from CPCLIBPOSITYPE where ISDELETED='0'"); }

                    if (gram1 == "PAYROLLRANKID")
                    { selectedRecord.set("QASVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where typeid='S2'"); }
                    if (gram1 == "AttendanceWay")
                    { selectedRecord.set("QASVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where typeid='59'"); }
                    if (gram1 == "OvertimePayment")
                    { selectedRecord.set("QASVALUELIST", "select itemid,itemvalue from PSNPUBLICCODEITEM where typeid='60'"); }
                    if (gram1 == "SHIFTGROUP")
                    { selectedRecord.set("QASVALUELIST", " select GROUPID,GROUPNAME from ATDSHIFTGROUP "); }
                } else if (gram1 == "DATEOFBIRTH" || gram1 == "DIMISSIONDATE" || gram1 == "date1" || gram1 == "date2" || gram1 == "date3"
                 || gram1 == "ATTENDONDATE" || gram1 == "STARTSERVICEDATE" || gram1 == "PROBATIONENDDATE" ||
                    gram1 == "ENTERSOCIETYDATE" || gram1 == "INDATE" || gram1 == "OUTDATE")  //日期型
                {
                    selectedRecord.set('QASTYPE', DATE);
                    selectedRecord.set('QASISDIM', N0);
                } else if (gram1 == "BRANCHID")  //组织列表
                {
                    selectedRecord.set('QASTYPE', DEPT);
                    selectedRecord.set('QASISDIM', N0);
                } else if (gram1 == "ACCOUNTITME1")   //BOOL型
                {
                    selectedRecord.set('QASTYPE', BOOL);
                    selectedRecord.set('QASISDIM', N0);
                } else if (gram1 == "VacationDays" || gram1 == "overtimeSettlementUpper")//数值类型
                {
                    selectedRecord.set('QASTYPE', INT);
                    selectedRecord.set('QASISDIM', N0);
                } else  //其他类型
                {
                    selectedRecord.set('QASTYPE', STRING);
                    selectedRecord.set('QASISDIM', YES);
                }

                if (gram1 == "ACCESSIONSTATE" || gram1 == "TRUENAME" || gram1 == "EMPLOYEEID" || gram1 == "ATTENDONDATE"
                  || gram1 == "BRANCHID" || gram1 == "CERTIFICATETYPEID" || gram1 == "CERTIFICATENUMBER" || gram1 == "GENDER"
                  || gram1 == "STARTSERVICEDATE" || gram1 == "CARDNUM" || gram1 == "EmergencyContact" || gram1 == "EmergencyContactNo")
                { selectedRecord.set("QAISMUSTIN", YES); }
                else
                { selectedRecord.set("QAISMUSTIN", N0); }
            });

        });

    </script>
</body>
</html>
