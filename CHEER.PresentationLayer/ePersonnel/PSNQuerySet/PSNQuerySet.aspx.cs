using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.ePersonnel;
using CHEER.BusinessLayer.eQueryCenter.BaseConfig;
using CHEER.CommonLayer.ePersonnel.Data;
using CHEER.CommonLayer.ePersonnel.Schema;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;
using CHEER.Platform.DAL;

namespace CHEER.PresentationLayer.ePersonnel.PSNQuerySet
{
    public partial class PSNQuerySet : CHEERBasePage
    {

        public string language()
        {
            return InfomationPackage.LanguageCulture;
        }
        //绑定类别
        private void LoadRptType(CheerUI.DropDownList drp)
        {
            drp.Items.Clear();
            drp.Items.Add(new CheerUI.ListItem(getString("ZGAIA01157"), "PSN_BASE"));
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.Text = getString("ZGAIA00195");// "";保存

            //drpReportType.AutoPostBack = true;

            if (Request.QueryString["QSID"] != null && Request.QueryString["QSID"] != "")
            {
                txtReportID.Text = Request.QueryString["QSID"];
            }
            if (!IsPostBack)
            {
                InitddlfillField();
                InitddlfillField1();
                InitddlfillField2();
                InitddlfillField3();
                InitddlfillType();
                InitddlfillType2();
                InitddlfillType3();
                InitDropDowlistIsNo();
                AddRendererFunction();
                LoadRptType(drpReportType);
                Bind();
            }
        }

        


        protected void Page_Init(object sender, EventArgs e)
        {

            InitgrdMain();
            InitgrdMain1();
            InitgrdMain2();
            InitgrdMain3();
        }

        protected CheerUI.DropDownList ddlfillField = new CheerUI.DropDownList() { MaxPopHeight = 200 };
        protected CheerUI.DropDownList ddlfillField1 = new CheerUI.DropDownList() { MaxPopHeight = 200 };
        protected CheerUI.DropDownList ddlfillField2 = new CheerUI.DropDownList() { MaxPopHeight = 200 };
        protected CheerUI.DropDownList ddlfillField3 = new CheerUI.DropDownList() { MaxPopHeight = 200 };
        protected CheerUI.DropDownList ddlfillType = new CheerUI.DropDownList() { MaxPopHeight = 200 };
        protected CheerUI.DropDownList ddlfillType2 = new CheerUI.DropDownList() { MaxPopHeight = 200 };
        protected CheerUI.DropDownList ddlfillType3 = new CheerUI.DropDownList() { MaxPopHeight = 200 };
        protected CheerUI.DropDownList ddlfuzzyQuery = new CheerUI.DropDownList() { MaxPopHeight = 200 };
        protected CheerUI.DropDownList ddlisReadonly = new CheerUI.DropDownList() { MaxPopHeight = 200 };
        protected CheerUI.DropDownList ddlmveItems = new CheerUI.DropDownList() { MaxPopHeight = 200 };
        protected CheerUI.DropDownList ddlRequired = new CheerUI.DropDownList() { MaxPopHeight = 200 };

        protected void InitgrdMain()
        {
            CommonMethod.AddAddField(grdMain, "", "Add", false, false).EnablePostBack=false;
            CommonMethod.AddDeleteField(grdMain, "", "Delete", false, false).EnablePostBack=false;
            CommonMethod.AddFlexRendererField(grdMain, getString("ZGAIA00466"), PSNQUERYCONDITIONSETSchema.QCSFIELD, 10, "", false).Editor.Add(ddlfillField);//字段名称
            CommonMethod.AddFlexRendererField(grdMain, getString("ZGAIA05294"), PSNQUERYCONDITIONSETSchema.QCSTITLE, 10, "", false).Editor.Add(new CheerUI.TextBox());//参数标题
            CommonMethod.AddFlexRendererField(grdMain, getString("ZGAIA05294") + "(EN-US)", PSNQUERYCONDITIONSETSchema.QCS4, 10, "", true).Editor.Add(new CheerUI.TextBox());//参数标题
            CommonMethod.AddFlexRendererField(grdMain, getString("ZGAIA05294") + "(ZH-TW)", PSNQUERYCONDITIONSETSchema.QCS5, 10, "", true).Editor.Add(new CheerUI.TextBox());//参数标题
            CommonMethod.AddFlexRendererField(grdMain, getString("ZGAIA05295"), PSNQUERYCONDITIONSETSchema.QCSTYPE, 10, "", false).Editor.Add(ddlfillType);//参数类型
            CommonMethod.AddFlexRendererField(grdMain, getString("ZGAIA05296"), PSNQUERYCONDITIONSETSchema.QCSISDIM, 10, "", false).Editor.Add(ddlfuzzyQuery);//模糊查询
            CommonMethod.AddFlexRendererField(grdMain, getString("ZGAIA02886"), PSNQUERYCONDITIONSETSchema.QCSDEFAULTVALUE, 10, "", false).Editor.Add(new CheerUI.TextBox());//默认值
            CommonMethod.AddFlexRendererField(grdMain, getString("ZGAIA03448"), PSNQUERYCONDITIONSETSchema.QCSVALUELIST, 20, "", false).Editor.Add(new CheerUI.TextBox());//参数值
            CommonMethod.AddFlexRendererField(grdMain, getString("ZGAIA01293"), PSNQUERYCONDITIONSETSchema.QCSORDER, 10, "", false).Editor.Add(new CheerUI.TextBox());//排序值
            CommonMethod.AddFlexField(grdMain, "", PSNQUERYCONDITIONSETSchema.QCSID, 0, true);
        }
        protected void InitgrdMain1()
        {
            CommonMethod.AddAddField(grdMain1, "", "Add", false, false).EnablePostBack = false;
            CommonMethod.AddDeleteField(grdMain1, "", "Delete", false, false).EnablePostBack = false;
            CommonMethod.AddFlexRendererField(grdMain1, getString("ZGAIA00466"), PSNQUERYRESULTSETSchema.QRSDISPLAYFIELD, 15, "", false).Editor.Add(ddlfillField1);//字段名称
            CommonMethod.AddFlexRendererField(grdMain1, getString("ZGAIA05294"), PSNQUERYRESULTSETSchema.QRSTITLE, 25, "", false).Editor.Add(new CheerUI.TextBox());//参数标题
            CommonMethod.AddFlexRendererField(grdMain1, getString("ZGAIA05294") + "(EN-US)", PSNQUERYRESULTSETSchema.QRS4, 25, "", true).Editor.Add(new CheerUI.TextBox());//参数标题
            CommonMethod.AddFlexRendererField(grdMain1, getString("ZGAIA05294") + "(ZH-TW)", PSNQUERYRESULTSETSchema.QRS5, 25, "", true).Editor.Add(new CheerUI.TextBox());//参数标题
            CommonMethod.AddFlexRendererField(grdMain1, getString("ZGAIA01293"), PSNQUERYRESULTSETSchema.QRSORDER, 10, "", false).Editor.Add(new CheerUI.TextBox());//排序值
            CommonMethod.AddFlexField(grdMain1, "", PSNQUERYRESULTSETSchema.QRSID, 0, true);
        }
        protected void InitgrdMain2()
        {
            CommonMethod.AddAddField(grdMain2, "", "Add", false, false).EnablePostBack = false;
            CommonMethod.AddDeleteField(grdMain2, "", "Delete", false, false).EnablePostBack = false;
            CommonMethod.AddFlexRendererField(grdMain2, getString("ZGAIA00466"), PSNQUERYDETAILSETSchema.QDSFIELD, 10, "", false).Editor.Add(ddlfillField2);//字段名称
            CommonMethod.AddFlexRendererField(grdMain2, getString("ZGAIA05294"), PSNQUERYDETAILSETSchema.QDSNAME, 15, "", false).Editor.Add(new CheerUI.TextBox());//参数标题
            CommonMethod.AddFlexRendererField(grdMain2, getString("ZGAIA05294") + "(EN-US)", PSNQUERYDETAILSETSchema.QDS4, 15, "", true).Editor.Add(new CheerUI.TextBox());//参数标题
            CommonMethod.AddFlexRendererField(grdMain2, getString("ZGAIA05294") + "(ZH-TW)", PSNQUERYDETAILSETSchema.QDS5, 15, "", true).Editor.Add(new CheerUI.TextBox());//参数标题
            CommonMethod.AddFlexRendererField(grdMain2, getString("ZGAIA05295"), PSNQUERYDETAILSETSchema.QDSTYPE, 10, "", false).Editor.Add(ddlfillType2);//参数类型
            CommonMethod.AddFlexRendererField(grdMain2, getString("ZGAIA03448"), PSNQUERYDETAILSETSchema.QDSVALUELIST, 9, "", false).Editor.Add(new CheerUI.TextBox());//参数值
            CommonMethod.AddFlexRendererField(grdMain2, getString("ZGAIA05307"), PSNQUERYDETAILSETSchema.QDSISREAD, 6, "", false).Editor.Add(ddlisReadonly);//是否只读
            CommonMethod.AddFlexRendererField(grdMain2, "异动项", PSNQUERYDETAILSETSchema.QDSISCHANGE, 8, "", false).Editor.Add(ddlmveItems);//是否异动项
            CommonMethod.AddFlexRendererField(grdMain2, getString("ZGAIA01293"), PSNQUERYDETAILSETSchema.QDSORDER, 6, "", false).Editor.Add(new CheerUI.TextBox());//排序值
            CommonMethod.AddFlexField(grdMain2, "", PSNQUERYDETAILSETSchema.QDSID, 0, true);
        }
        protected void InitgrdMain3()
        {
            CommonMethod.AddAddField(grdMain3, "", "Add", false, false).EnablePostBack = false;
            CommonMethod.AddDeleteField(grdMain3, "", "Delete", false, false).EnablePostBack = false;
            CommonMethod.AddFlexRendererField(grdMain3, getString("ZGAIA00466"), PSNQUERYADDSETSchema.QASFIELD, 10, "", false).Editor.Add(ddlfillField3);//字段名称
            CommonMethod.AddFlexRendererField(grdMain3, getString("ZGAIA05294"), PSNQUERYADDSETSchema.QASTITLE, 10, "", false).Editor.Add(new CheerUI.TextBox());//参数标题
            CommonMethod.AddFlexRendererField(grdMain3, getString("ZGAIA05294") + "(EN-US)", PSNQUERYADDSETSchema.QAS4, 10, "", true).Editor.Add(new CheerUI.TextBox());//参数标题
            CommonMethod.AddFlexRendererField(grdMain3, getString("ZGAIA05294") + "(ZH-TW)", PSNQUERYADDSETSchema.QAS5, 10, "", true).Editor.Add(new CheerUI.TextBox());//参数标题
            CommonMethod.AddFlexRendererField(grdMain3, getString("ZGAIA05295"), PSNQUERYADDSETSchema.QASTYPE, 10, "", false).Editor.Add(ddlfillType3);//参数类型
            CommonMethod.AddFlexRendererField(grdMain3, getString("ZGAIA03448"), PSNQUERYADDSETSchema.QASVALUELIST, 10, "", false).Editor.Add(new CheerUI.TextBox());//参数值
            CommonMethod.AddFlexRendererField(grdMain3, getString("ZGAIA02886"), PSNQUERYADDSETSchema.QASDEFAULTVALUE, 10, "", false).Editor.Add(new CheerUI.TextBox());//默认值
            CommonMethod.AddFlexRendererField(grdMain3, getString("ZGAIA02882"), PSNQUERYADDSETSchema.QAISMUSTIN, 10, "", false).Editor.Add(ddlRequired);//是否必填
            CommonMethod.AddFlexRendererField(grdMain3, getString("ZGAIA01293"), PSNQUERYADDSETSchema.QASORDER, 10, "", false).Editor.Add(new CheerUI.TextBox());//排序值
            CommonMethod.AddFlexField(grdMain3, "", PSNQUERYADDSETSchema.QASID, 0, true);
        }

        protected void AddRendererFunction()
        {
            var FellField = this.grdMain.Columns.FromKey(PSNQUERYCONDITIONSETSchema.QCSFIELD) as CheerUI.RenderField;
            FellField.RendererFunction = "renderFellField";

            var FellField1 = this.grdMain1.Columns.FromKey(PSNQUERYRESULTSETSchema.QRSDISPLAYFIELD) as CheerUI.RenderField;
            FellField1.RendererFunction = "renderFellField1";

            var FellField2 = this.grdMain2.Columns.FromKey(PSNQUERYDETAILSETSchema.QDSFIELD) as CheerUI.RenderField;
            FellField2.RendererFunction = "renderFellField2";

            var FellField3 = this.grdMain3.Columns.FromKey(PSNQUERYADDSETSchema.QASFIELD) as CheerUI.RenderField;
            FellField3.RendererFunction = "renderFellField3";


            var FellType = this.grdMain.Columns.FromKey(PSNQUERYCONDITIONSETSchema.QCSTYPE) as CheerUI.RenderField;
            FellType.RendererFunction = "renderFellType";

            var FellType2 = this.grdMain2.Columns.FromKey(PSNQUERYDETAILSETSchema.QDSTYPE) as CheerUI.RenderField;
            FellType2.RendererFunction = "renderFellType2";

            var FellType3 = this.grdMain3.Columns.FromKey(PSNQUERYADDSETSchema.QASTYPE) as CheerUI.RenderField;
            FellType3.RendererFunction = "renderFellType3";

            var FuzzyQuery = this.grdMain.Columns.FromKey(PSNQUERYCONDITIONSETSchema.QCSISDIM) as CheerUI.RenderField;
            FuzzyQuery.RendererFunction = "renderFuzzyQuery";

            var IsReadonly = this.grdMain2.Columns.FromKey(PSNQUERYDETAILSETSchema.QDSISREAD) as CheerUI.RenderField;
            IsReadonly.RendererFunction = "renderIsReadonly";

            var MveItems = this.grdMain2.Columns.FromKey(PSNQUERYDETAILSETSchema.QDSISCHANGE) as CheerUI.RenderField;
            MveItems.RendererFunction = "renderMveItems";

            var Required = this.grdMain3.Columns.FromKey(PSNQUERYADDSETSchema.QAISMUSTIN) as CheerUI.RenderField;
            Required.RendererFunction = "renderRequired";
        }

        //绑定
        private void Bind()
        {
            if (txtReportID.Text != null && txtReportID.Text != "")
            {
                #region 公共部分
                PSNQUERYSETDA qd = new PSNQUERYSETDA();
                PSNQUERYSETET et = qd.GetPSNQUERYSETET(txtReportID.Text);
                txtName.Text = et.QSNAME;
                drpReportType.SelectedIndex = -1;
                drpReportType.Items.FindByValue(et.QSTYPE).Selected = true;
                //txtReportID.Value = Request.QueryString["QSID"];

                PSNQUERYSETLoader ql = new PSNQUERYSETLoader();
                #endregion

                #region 查询条件设置部分
                DataTable dt = ql.QueryConditionSet(et.QSID);
                grdMain.DataSource = dt;
                grdMain.DataBind();
                #endregion

                #region 列表字段设置部分
                DataTable dt1 = ql.QueryResultSet(et.QSID);
                grdMain1.DataSource = dt1;
                grdMain1.DataBind();
                #endregion

                #region 详细字段设置部分
                DataTable dt2 = ql.QueryDetailSet(et.QSID);
                grdMain2.DataSource = dt2;
                grdMain2.DataBind();
                #endregion

                #region 新增页面字段设置部分
                DataTable dt3 = ql.QueryAddSet(et.QSID);
                grdMain3.DataSource = dt3;
                grdMain3.DataBind();
                #endregion


            }
            else
            {
                #region 查询条件部分
                grdMain.DataSource = new DataTable();
                grdMain.DataBind();
                #endregion

                #region 列表字段设置部分
                grdMain1.DataSource = new DataTable();
                grdMain1.DataBind();
                #endregion

                #region 详细字段设置部分
                grdMain2.DataSource = new DataTable();
                grdMain2.DataBind();
                #endregion

                #region 新增页面字段设置部分
                grdMain3.DataSource = new DataTable();
                grdMain3.DataBind();
                #endregion
            }
        }

        protected void InitddlfillField()
        {
            ddlfillField.MatchFieldWidth = false;
            
            ddlfillField.Items.Add(new CheerUI.ListItem("", ""));
            //必选项
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA04709"), "BRANCHID"));//部门
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA00209"), "EMPLOYEEID"));//工号
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA00208"), "TRUENAME"));//姓名
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA00241"), "ACCESSIONSTATE"));//在职状态
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA00207"), "ATTENDONDATE"));//入职日
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01019"), "CERTIFICATETYPEID"));//证件类型
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01021"), "CERTIFICATENUMBER"));//证件号码
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA00307"), "GENDER"));//性别
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA00312"), "CARDNUM"));//卡号
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01140"), "STARTSERVICEDATE"));//工龄起算日
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA05357"), "EmergencyContact"));//紧急联系人
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA05358"), "EmergencyContactNo"));//紧急联系人电话

            //联系信息
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01070"), "CELLPHONE"));//手机号码
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA00873"), "EXT"));//分机
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01644"), "COMPANYEMAIL"));//内部信箱
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01069"), "CONTACTADDRESS"));//联系地址
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01066"), "POSTCODE"));//邮政编码

            //异动项
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA00592"), "JOBCODE"));//职位
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA00841"), "RESPONSIBILITYID"));//职务
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01137"), "TITLEID"));//职称
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01136"), "RANKID"));//职等
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA05476"), "PAYROLLRANKID"));//薪资级别
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01143"), "NATIVEPLACEPROPERTYID"));//户籍性质
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01144"), "JOBTYPEID"));//工种
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01146"), "ARRANGEMENTID"));//编制
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01842"), "BELONGCORPID"));//地区
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01139"), "EMPLOYEECHARID"));//员工性质
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01138"), "EDUCATIONALLEVELID"));//学历

            //其他项目
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA02121"), "DIMISSIONDATE"));//离职日期
            ddlfillField.Items.Add(new CheerUI.ListItem("厂车", "COMPANYBUS"));//厂车
            ddlfillField.Items.Add(new CheerUI.ListItem("线别", "PRODUCTLINE"));//线别
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01090"), "PROBATIONENDDATE"));//试满日
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01593"), "DATEOFBIRTH"));//出生日期
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA02221"), "NATIONID"));//民族
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01022"), "NATIONALITYID"));//国籍
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01027"), "FIRSTNAME"));//FirstName
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01023"), "MIDDLENAME"));//MiddleName
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01025"), "LASTNAME"));//LastName
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01026"), "HOMEPLACE"));//籍贯
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01608"), "POLICALFACEID"));//政治面貌
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01020"), "MARRIAGEID"));//婚姻状况
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01028"), "GRADUATESCHOOLID"));//毕业院校
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA01142"), "DLIDL"));//DL/IDL
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA00559"), "EMPLOYEETYPEID"));//员工类别
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA02306"), "SOCIETYENSURENUM"));//社保号码
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA00054"), "EMPGROUP"));//员工组
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA00171"), "REMARK"));//备注
            ddlfillField.Items.Add(new CheerUI.ListItem("班组", "SHIFTGROUP"));//班组

            //生产工时信息
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA05308"), "ActivityArrayID"));//活动总览
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA05313"), "CurrentTeamID"));//当前团队

            //档案信息
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA02255"), "DOCUMENTCODE"));//档案号
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA02253"), "INDATE"));//进档案时间
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA02251"), "OUTDATE"));//出档案时间

            //详细设置
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA05523"), "AnnualLeave"));//约定年假
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA05524"), "VacationDays"));//年假天数
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA05525"), "AttendanceWay"));//出勤方式
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA05526"), "overtimeSettlementUpper"));//标准工时加班费结算上限
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA05527"), "ComAccountWorkingHours"));//综合工时账户
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA05528"), "IsAttendance"));//考勤
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA05529"), "OvertimePayment"));//加班结算方式
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA05530"), "IsInterns"));//实习生
            ddlfillField.Items.Add(new CheerUI.ListItem("OT1", "ot1"));
            ddlfillField.Items.Add(new CheerUI.ListItem("OT2", "ot2"));
            ddlfillField.Items.Add(new CheerUI.ListItem("OT3", "ot3"));
            ddlfillField.Items.Add(new CheerUI.ListItem("string1", "string1"));
            ddlfillField.Items.Add(new CheerUI.ListItem("string2", "string2"));
            ddlfillField.Items.Add(new CheerUI.ListItem("string3", "string3"));
            ddlfillField.Items.Add(new CheerUI.ListItem("string4", "string4"));
            ddlfillField.Items.Add(new CheerUI.ListItem("string5", "string5"));
            ddlfillField.Items.Add(new CheerUI.ListItem("date1", "date1"));
            ddlfillField.Items.Add(new CheerUI.ListItem("date2", "date2"));
            ddlfillField.Items.Add(new CheerUI.ListItem("date3", "date3"));
            ddlfillField.Items.Add(new CheerUI.ListItem("Sql1", "Sql1"));
            ddlfillField.Items.Add(new CheerUI.ListItem("Sql2", "Sql2"));
            ddlfillField.Items.Add(new CheerUI.ListItem("Costcenter", "Costcenter"));
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA05700"), "TransCard"));//待转换卡号
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA05670"), "age"));//年龄
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA05671"), "Entrychannels"));//入职渠道
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA05672"), "Reportingto"));//汇报对象
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA05673"), "HealthExamination"));//健康体检
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA05674"), "firstworktime"));//第一次职位开始日期
            ddlfillField.Items.Add(new CheerUI.ListItem(getString("ZGAIA05675"), "Accountplace"));//户口所在地
        }

        protected void InitddlfillField1() {
            ddlfillField1.MatchFieldWidth = false;

            ddlfillField1.Items.Add(new CheerUI.ListItem("", ""));
            //必选项
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA04709"), "BRANCHID"));//部门
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA00209"), "EMPLOYEEID"));//工号
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA00208"), "TRUENAME"));//姓名
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA00241"), "ACCESSIONSTATE"));//在职状态
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA00207"), "ATTENDONDATE"));//入职日
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01019"), "CERTIFICATETYPEID"));//证件类型
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01021"), "CERTIFICATENUMBER"));//证件号码
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA00307"), "GENDER"));//性别
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA00312"), "CARDNUM"));//卡号
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01140"), "STARTSERVICEDATE"));//工龄起算日
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA05357"), "EmergencyContact"));//紧急联系人
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA05358"), "EmergencyContactNo"));//紧急联系人电话

            //联系信息
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01070"), "CELLPHONE"));//手机号码
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA00873"), "EXT"));//分机
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01644"), "COMPANYEMAIL"));//内部信箱
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01069"), "CONTACTADDRESS"));//联系地址
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01066"), "POSTCODE"));//邮政编码

            //异动项
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA00592"), "JOBCODE"));//职位
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA00841"), "RESPONSIBILITYID"));//职务
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01137"), "TITLEID"));//职称
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01136"), "RANKID"));//职等
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA05476"), "PAYROLLRANKID"));//薪资级别
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01143"), "NATIVEPLACEPROPERTYID"));//户籍性质
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01144"), "JOBTYPEID"));//工种
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01146"), "ARRANGEMENTID"));//编制
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01842"), "BELONGCORPID"));//地区
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01139"), "EMPLOYEECHARID"));//员工性质
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01138"), "EDUCATIONALLEVELID"));//学历

            //其他项目
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA02121"), "DIMISSIONDATE"));//离职日期
            ddlfillField1.Items.Add(new CheerUI.ListItem("厂车", "COMPANYBUS"));//厂车
            ddlfillField1.Items.Add(new CheerUI.ListItem("线别", "PRODUCTLINE"));//线别
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01090"), "PROBATIONENDDATE"));//试满日
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01593"), "DATEOFBIRTH"));//出生日期
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA02221"), "NATIONID"));//民族
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01022"), "NATIONALITYID"));//国籍
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01027"), "FIRSTNAME"));//FirstName
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01023"), "MIDDLENAME"));//MiddleName
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01025"), "LASTNAME"));//LastName
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01026"), "HOMEPLACE"));//籍贯
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01608"), "POLICALFACEID"));//政治面貌
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01020"), "MARRIAGEID"));//婚姻状况
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01028"), "GRADUATESCHOOLID"));//毕业院校
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA01142"), "DLIDL"));//DL/IDL
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA00559"), "EMPLOYEETYPEID"));//员工类别
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA02306"), "SOCIETYENSURENUM"));//社保号码
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA00054"), "EMPGROUP"));//员工组
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA00171"), "REMARK"));//备注
            ddlfillField1.Items.Add(new CheerUI.ListItem("班组", "SHIFTGROUP"));//班组

            //生产工时信息
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA05308"), "ActivityArrayID"));//活动总览
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA05313"), "CurrentTeamID"));//当前团队

            //档案信息
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA02255"), "DOCUMENTCODE"));//档案号
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA02253"), "INDATE"));//进档案时间
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA02251"), "OUTDATE"));//出档案时间

            //详细设置
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA05523"), "AnnualLeave"));//约定年假
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA05524"), "VacationDays"));//年假天数
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA05525"), "AttendanceWay"));//出勤方式
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA05526"), "overtimeSettlementUpper"));//标准工时加班费结算上限
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA05527"), "ComAccountWorkingHours"));//综合工时账户
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA05528"), "IsAttendance"));//考勤
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA05529"), "OvertimePayment"));//加班结算方式
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA05530"), "IsInterns"));//实习生
            ddlfillField1.Items.Add(new CheerUI.ListItem("OT1", "ot1"));
            ddlfillField1.Items.Add(new CheerUI.ListItem("OT2", "ot2"));
            ddlfillField1.Items.Add(new CheerUI.ListItem("OT3", "ot3"));
            ddlfillField1.Items.Add(new CheerUI.ListItem("string1", "string1"));
            ddlfillField1.Items.Add(new CheerUI.ListItem("string2", "string2"));
            ddlfillField1.Items.Add(new CheerUI.ListItem("string3", "string3"));
            ddlfillField1.Items.Add(new CheerUI.ListItem("string4", "string4"));
            ddlfillField1.Items.Add(new CheerUI.ListItem("string5", "string5"));
            ddlfillField1.Items.Add(new CheerUI.ListItem("date1", "date1"));
            ddlfillField1.Items.Add(new CheerUI.ListItem("date2", "date2"));
            ddlfillField1.Items.Add(new CheerUI.ListItem("date3", "date3"));
            ddlfillField1.Items.Add(new CheerUI.ListItem("Sql1", "Sql1"));
            ddlfillField1.Items.Add(new CheerUI.ListItem("Sql2", "Sql2"));
            ddlfillField1.Items.Add(new CheerUI.ListItem("Costcenter", "Costcenter"));
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA05700"), "TransCard"));//待转换卡号
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA05670"), "age"));//年龄
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA05671"), "Entrychannels"));//入职渠道
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA05672"), "Reportingto"));//汇报对象
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA05673"), "HealthExamination"));//健康体检
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA05674"), "firstworktime"));//第一次职位开始日期
            ddlfillField1.Items.Add(new CheerUI.ListItem(getString("ZGAIA05675"), "Accountplace"));//户口所在地

        }

        protected void InitddlfillField2() {
            ddlfillField2.MatchFieldWidth = false;

            ddlfillField2.Items.Add(new CheerUI.ListItem("", ""));
            //必选项
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA04709"), "BRANCHID"));//部门
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA00209"), "EMPLOYEEID"));//工号
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA00208"), "TRUENAME"));//姓名
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA00241"), "ACCESSIONSTATE"));//在职状态
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA00207"), "ATTENDONDATE"));//入职日
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01019"), "CERTIFICATETYPEID"));//证件类型
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01021"), "CERTIFICATENUMBER"));//证件号码
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA00307"), "GENDER"));//性别
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA00312"), "CARDNUM"));//卡号
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01140"), "STARTSERVICEDATE"));//工龄起算日
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05357"), "EmergencyContact"));//紧急联系人
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05358"), "EmergencyContactNo"));//紧急联系人电话

            //联系信息
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01070"), "CELLPHONE"));//手机号码
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA00873"), "EXT"));//分机
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01644"), "COMPANYEMAIL"));//内部信箱
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01069"), "CONTACTADDRESS"));//联系地址
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01066"), "POSTCODE"));//邮政编码

            //异动项
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA00592"), "JOBCODE"));//职位
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA00841"), "RESPONSIBILITYID"));//职务
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01137"), "TITLEID"));//职称
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01136"), "RANKID"));//职等
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05476"), "PAYROLLRANKID"));//薪资级别
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01143"), "NATIVEPLACEPROPERTYID"));//户籍性质
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01144"), "JOBTYPEID"));//工种
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01146"), "ARRANGEMENTID"));//编制
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01842"), "BELONGCORPID"));//地区
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01139"), "EMPLOYEECHARID"));//员工性质
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01138"), "EDUCATIONALLEVELID"));//学历

            //其他项目
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA02121"), "DIMISSIONDATE"));//离职日期
            ddlfillField2.Items.Add(new CheerUI.ListItem("厂车", "COMPANYBUS"));//厂车
            ddlfillField2.Items.Add(new CheerUI.ListItem("线别", "PRODUCTLINE"));//线别
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01090"), "PROBATIONENDDATE"));//试满日
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01593"), "DATEOFBIRTH"));//出生日期
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA02221"), "NATIONID"));//民族
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01022"), "NATIONALITYID"));//国籍
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01027"), "FIRSTNAME"));//FirstName
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01023"), "MIDDLENAME"));//MiddleName
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01025"), "LASTNAME"));//LastName
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01026"), "HOMEPLACE"));//籍贯
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01608"), "POLICALFACEID"));//政治面貌
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01020"), "MARRIAGEID"));//婚姻状况
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01028"), "GRADUATESCHOOLID"));//毕业院校
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01142"), "DLIDL"));//DL/IDL
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA00559"), "EMPLOYEETYPEID"));//员工类别
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA02306"), "SOCIETYENSURENUM"));//社保号码
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA00054"), "EMPGROUP"));//员工组
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA00171"), "REMARK"));//备注
            ddlfillField2.Items.Add(new CheerUI.ListItem("班组", "SHIFTGROUP"));//班组

            //生产工时信息
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05308"), "ActivityArrayID"));//活动总览
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05313"), "CurrentTeamID"));//当前团队

            //档案信息
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA02255"), "DOCUMENTCODE"));//档案号
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA02253"), "INDATE"));//进档案时间
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA02251"), "OUTDATE"));//出档案时间

            //详细设置
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05523"), "AnnualLeave"));//约定年假
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05524"), "VacationDays"));//年假天数
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05525"), "AttendanceWay"));//出勤方式
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05526"), "overtimeSettlementUpper"));//标准工时加班费结算上限
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05527"), "ComAccountWorkingHours"));//综合工时账户
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05528"), "IsAttendance"));//考勤
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05529"), "OvertimePayment"));//加班结算方式
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05530"), "IsInterns"));//实习生
            ddlfillField2.Items.Add(new CheerUI.ListItem("OT1", "ot1"));
            ddlfillField2.Items.Add(new CheerUI.ListItem("OT2", "ot2"));
            ddlfillField2.Items.Add(new CheerUI.ListItem("OT3", "ot3"));
            ddlfillField2.Items.Add(new CheerUI.ListItem("string1", "string1"));
            ddlfillField2.Items.Add(new CheerUI.ListItem("string2", "string2"));
            ddlfillField2.Items.Add(new CheerUI.ListItem("string3", "string3"));
            ddlfillField2.Items.Add(new CheerUI.ListItem("string4", "string4"));
            ddlfillField2.Items.Add(new CheerUI.ListItem("string5", "string5"));
            ddlfillField2.Items.Add(new CheerUI.ListItem("date1", "date1"));
            ddlfillField2.Items.Add(new CheerUI.ListItem("date2", "date2"));
            ddlfillField2.Items.Add(new CheerUI.ListItem("date3", "date3"));
            ddlfillField2.Items.Add(new CheerUI.ListItem("Sql1", "Sql1"));
            ddlfillField2.Items.Add(new CheerUI.ListItem("Sql2", "Sql2"));
            ddlfillField2.Items.Add(new CheerUI.ListItem("Costcenter", "Costcenter"));
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05700"), "TransCard"));//待转换卡号
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05670"), "age"));//年龄
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05671"), "Entrychannels"));//入职渠道
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05672"), "Reportingto"));//汇报对象
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05673"), "HealthExamination"));//健康体检
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05674"), "firstworktime"));//第一次职位开始日期
            ddlfillField2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05675"), "Accountplace"));//户口所在地


        }

        protected void InitddlfillField3() {
            ddlfillField3.MatchFieldWidth = false;

            ddlfillField3.Items.Add(new CheerUI.ListItem("", ""));
            //必选项
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA04709"), "BRANCHID"));//部门
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA00209"), "EMPLOYEEID"));//工号
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA00208"), "TRUENAME"));//姓名
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA00241"), "ACCESSIONSTATE"));//在职状态
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA00207"), "ATTENDONDATE"));//入职日
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01019"), "CERTIFICATETYPEID"));//证件类型
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01021"), "CERTIFICATENUMBER"));//证件号码
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA00307"), "GENDER"));//性别
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA00312"), "CARDNUM"));//卡号
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01140"), "STARTSERVICEDATE"));//工龄起算日
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05357"), "EmergencyContact"));//紧急联系人
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05358"), "EmergencyContactNo"));//紧急联系人电话

            //联系信息
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01070"), "CELLPHONE"));//手机号码
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA00873"), "EXT"));//分机
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01644"), "COMPANYEMAIL"));//内部信箱
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01069"), "CONTACTADDRESS"));//联系地址
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01066"), "POSTCODE"));//邮政编码

            //异动项
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA00592"), "JOBCODE"));//职位
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA00841"), "RESPONSIBILITYID"));//职务
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01137"), "TITLEID"));//职称
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01136"), "RANKID"));//职等
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05476"), "PAYROLLRANKID"));//薪资级别
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01143"), "NATIVEPLACEPROPERTYID"));//户籍性质
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01144"), "JOBTYPEID"));//工种
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01146"), "ARRANGEMENTID"));//编制
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01842"), "BELONGCORPID"));//地区
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01139"), "EMPLOYEECHARID"));//员工性质
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01138"), "EDUCATIONALLEVELID"));//学历

            //其他项目
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA02121"), "DIMISSIONDATE"));//离职日期
            ddlfillField3.Items.Add(new CheerUI.ListItem("厂车", "COMPANYBUS"));//厂车
            ddlfillField3.Items.Add(new CheerUI.ListItem("线别", "PRODUCTLINE"));//线别
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01090"), "PROBATIONENDDATE"));//试满日
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01593"), "DATEOFBIRTH"));//出生日期
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA02221"), "NATIONID"));//民族
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01022"), "NATIONALITYID"));//国籍
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01027"), "FIRSTNAME"));//FirstName
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01023"), "MIDDLENAME"));//MiddleName
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01025"), "LASTNAME"));//LastName
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01026"), "HOMEPLACE"));//籍贯
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01608"), "POLICALFACEID"));//政治面貌
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01020"), "MARRIAGEID"));//婚姻状况
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01028"), "GRADUATESCHOOLID"));//毕业院校
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01142"), "DLIDL"));//DL/IDL
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA00559"), "EMPLOYEETYPEID"));//员工类别
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA02306"), "SOCIETYENSURENUM"));//社保号码
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA00054"), "EMPGROUP"));//员工组
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA00171"), "REMARK"));//备注
            ddlfillField3.Items.Add(new CheerUI.ListItem("班组", "SHIFTGROUP"));//班组

            //生产工时信息
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05308"), "ActivityArrayID"));//活动总览
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05313"), "CurrentTeamID"));//当前团队

            //档案信息
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA02255"), "DOCUMENTCODE"));//档案号
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA02253"), "INDATE"));//进档案时间
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA02251"), "OUTDATE"));//出档案时间

            //详细设置
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05523"), "AnnualLeave"));//约定年假
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05524"), "VacationDays"));//年假天数
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05525"), "AttendanceWay"));//出勤方式
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05526"), "overtimeSettlementUpper"));//标准工时加班费结算上限
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05527"), "ComAccountWorkingHours"));//综合工时账户
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05528"), "IsAttendance"));//考勤
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05529"), "OvertimePayment"));//加班结算方式
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05530"), "IsInterns"));//实习生
            ddlfillField3.Items.Add(new CheerUI.ListItem("OT1", "ot1"));
            ddlfillField3.Items.Add(new CheerUI.ListItem("OT2", "ot2"));
            ddlfillField3.Items.Add(new CheerUI.ListItem("OT3", "ot3"));
            ddlfillField3.Items.Add(new CheerUI.ListItem("string1", "string1"));
            ddlfillField3.Items.Add(new CheerUI.ListItem("string2", "string2"));
            ddlfillField3.Items.Add(new CheerUI.ListItem("string3", "string3"));
            ddlfillField3.Items.Add(new CheerUI.ListItem("string4", "string4"));
            ddlfillField3.Items.Add(new CheerUI.ListItem("string5", "string5"));
            ddlfillField3.Items.Add(new CheerUI.ListItem("date1", "date1"));
            ddlfillField3.Items.Add(new CheerUI.ListItem("date2", "date2"));
            ddlfillField3.Items.Add(new CheerUI.ListItem("date3", "date3"));
            ddlfillField3.Items.Add(new CheerUI.ListItem("Sql1", "Sql1"));
            ddlfillField3.Items.Add(new CheerUI.ListItem("Sql2", "Sql2"));
            ddlfillField3.Items.Add(new CheerUI.ListItem("Costcenter", "Costcenter"));
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05700"), "TransCard"));//待转换卡号
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05670"), "age"));//年龄
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05671"), "Entrychannels"));//入职渠道
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05672"), "Reportingto"));//汇报对象
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05673"), "HealthExamination"));//健康体检
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05674"), "firstworktime"));//第一次职位开始日期
            ddlfillField3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05675"), "Accountplace"));//户口所在地
        }

        protected void InitddlfillType()
        {
            ddlfillType.MatchFieldWidth = false;
            ddlfillType.Items.Add(new CheerUI.ListItem("", ""));
            ddlfillType.Items.Add(new CheerUI.ListItem(getString("ZGAIA05297"), "STRING"));
            ddlfillType.Items.Add(new CheerUI.ListItem(getString("ZGAIA05298"), "INT"));
            ddlfillType.Items.Add(new CheerUI.ListItem(getString("ZGAIA01520"), "DATE"));
            ddlfillType.Items.Add(new CheerUI.ListItem(getString("ZGAIA05299"), "DEPT"));
            ddlfillType.Items.Add(new CheerUI.ListItem(getString("ZGAIA05300"), "DEFAULTLIST"));
            ddlfillType.Items.Add(new CheerUI.ListItem(getString("ZGAIA05301"), "SQLLIST"));
            ddlfillType.Items.Add(new CheerUI.ListItem(getString("ZGAIA05302"), "BOOL"));
            ddlfillType.Items.Add(new CheerUI.ListItem(getString("ZGAIA00241"), "STATE"));
        }

        protected void InitddlfillType2()
        {
            ddlfillType2.MatchFieldWidth = false;
            ddlfillType2.Items.Add(new CheerUI.ListItem("", ""));
            ddlfillType2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05297"), "STRING"));
            ddlfillType2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05298"), "INT"));
            ddlfillType2.Items.Add(new CheerUI.ListItem(getString("ZGAIA01520"), "DATE"));
            ddlfillType2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05299"), "DEPT"));
            ddlfillType2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05300"), "DEFAULTLIST"));
            ddlfillType2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05301"), "SQLLIST"));
            ddlfillType2.Items.Add(new CheerUI.ListItem(getString("ZGAIA05302"), "BOOL"));
            ddlfillType2.Items.Add(new CheerUI.ListItem(getString("ZGAIA00241"), "STATE"));
        }

        protected void InitddlfillType3()
        {
            ddlfillType3.MatchFieldWidth = false;
            ddlfillType3.Items.Add(new CheerUI.ListItem("", ""));
            ddlfillType3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05297"), "STRING"));
            ddlfillType3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05298"), "INT"));
            ddlfillType3.Items.Add(new CheerUI.ListItem(getString("ZGAIA01520"), "DATE"));
            ddlfillType3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05299"), "DEPT"));
            ddlfillType3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05300"), "DEFAULTLIST"));
            ddlfillType3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05301"), "SQLLIST"));
            ddlfillType3.Items.Add(new CheerUI.ListItem(getString("ZGAIA05302"), "BOOL"));
            ddlfillType3.Items.Add(new CheerUI.ListItem(getString("ZGAIA00241"), "STATE"));
        }

        protected void InitDropDowlistIsNo()
        {
            ddlfuzzyQuery.MatchFieldWidth = false;
            ddlfuzzyQuery.Items.Add(new CheerUI.ListItem("",""));
            ddlfuzzyQuery.Items.Add(new CheerUI.ListItem(getString("ZGAIA00254"), "YES"));
            ddlfuzzyQuery.Items.Add(new CheerUI.ListItem(getString("ZGAIA00785"), "NO"));

            ddlisReadonly.MatchFieldWidth = false;
            ddlisReadonly.Items.Add(new CheerUI.ListItem("", ""));
            ddlisReadonly.Items.Add(new CheerUI.ListItem(getString("ZGAIA00254"), "YES"));
            ddlisReadonly.Items.Add(new CheerUI.ListItem(getString("ZGAIA00785"), "NO"));

            ddlmveItems.MatchFieldWidth = false;
            ddlmveItems.Items.Add(new CheerUI.ListItem("", ""));
            ddlmveItems.Items.Add(new CheerUI.ListItem(getString("ZGAIA00254"), "YES"));
            ddlmveItems.Items.Add(new CheerUI.ListItem(getString("ZGAIA00785"), "NO"));

            ddlRequired.MatchFieldWidth = false;
            ddlRequired.Items.Add(new CheerUI.ListItem("", ""));
            ddlRequired.Items.Add(new CheerUI.ListItem(getString("ZGAIA00254"), "YES"));
            ddlRequired.Items.Add(new CheerUI.ListItem(getString("ZGAIA00785"), "NO"));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string a = txtReportID.Text;
            PSNQUERYSETET et = new PSNQUERYSETET();
            PSNQUERYSETDA qd = new PSNQUERYSETDA();
            PSNQUERYCONDITIONSETDA parada = new PSNQUERYCONDITIONSETDA();
            PSNQUERYRESULTSETDA parada1 = new PSNQUERYRESULTSETDA();
            PSNQUERYDETAILSETDA parada2 = new PSNQUERYDETAILSETDA();
            PSNQUERYADDSETDA parada3 = new PSNQUERYADDSETDA();
            bool isQueryCheck = HsCheckQuery();
            bool isFieldCheck = HsCheckFieldSet();
            bool isDeatilCheck = HsCheckDeatil();
            string flag = "";
            bool isNewFieldCheck = HsCheckNewField(ref flag);

            if (txtReportID.Text == null || txtReportID.Text == "")
            {
                txtReportID.Text = Guid.NewGuid().ToString();
                et.QSID = txtReportID.Text;
                et.QSTYPE = drpReportType.SelectedValue;
                et.QSNAME = txtName.Text;
                qd.Insert(et);
            }
            else
            {
                et.QSID = txtReportID.Text;
                et.QSNAME = txtName.Text;
                et.QSTYPE = drpReportType.SelectedValue;

                qd.Update(et);
            }
            string error = "";

            #region 查询条件部分
            if (isQueryCheck) 
            {
                //add
                List<Dictionary<string, object>> addedList = grdMain.GetNewAddedList();
                var enumerator = addedList.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var val = enumerator.Current;
                    string QCSID = Guid.NewGuid().ToString();
                    string QCSFIELD = val["QCSFIELD"].ToString();
                    if (QCSFIELD == "")
                    {
                        continue;
                    }
                    string QCSORDER = isnull(val["QCSORDER"]);
                    string QCSTITLE = isnull(val["QCSTITLE"]);
                    string QCS4 = "";
                    string QCS5 = "";
                    string QCSTYPE = isnull(val["QCSTYPE"]);
                    string QCSDEFAULTVALUE = val["QCSDEFAULTVALUE"].ToString();
                    string QCSVALUELIST = val["QCSVALUELIST"].ToString();
                    string QCSISDIM = val["QCSISDIM"].ToString();

                    PSNQUERYCONDITIONSETET para = new PSNQUERYCONDITIONSETET();
                    para.QCSID = QCSID;
                    para.QCSFIELD = QCSFIELD;
                    para.QCSORDER = int.Parse(QCSORDER);//需要修改
                    para.QCSTITLE = QCSTITLE;
                    para.QCSTYPE = QCSTYPE;
                    para.QCSDEFAULTVALUE = QCSDEFAULTVALUE;
                    para.QCSVALUELIST = QCSVALUELIST;
                    para.QCSISDIM = QCSISDIM;
                    para.QSID = txtReportID.Text;
                    para.QCS4 = QCS4;
                    para.QCS5 = QCS5;

                    parada.Insert(para);
                }

                //update
                Dictionary<int, Dictionary<string, object>> modifiedDict = grdMain.GetModifiedDict();
                foreach (int rowIndex in modifiedDict.Keys)
                {
                    var row = this.grdMain.Rows[rowIndex];
                    if (row.Cells.FromKey("QCSID") == null || row.Cells.FromKey("QCSID").ToString() == "")
                        continue;
                    string QCSID = row.Cells.FromKey("QCSID").ToString();
                    string QCSFIELD = row.Cells.FromKey("QCSFIELD").ToString(); 
                    if (QCSFIELD == "")
                    {
                        continue;
                    }
                    string QCSORDER = row.Cells.FromKey("QCSORDER").ToString();
                    string QCSTITLE = row.Cells.FromKey("QCSTITLE").ToString(); 
                    string QCS4 = row.Cells.FromKey("QCS4").ToString(); 
                    string QCS5 = row.Cells.FromKey("QCS5").ToString(); 
                    string QCSTYPE = row.Cells.FromKey("QCSTYPE").ToString(); 
                    string QCSDEFAULTVALUE = row.Cells.FromKey("QCSDEFAULTVALUE").ToString(); 
                    string QCSVALUELIST = row.Cells.FromKey("QCSVALUELIST").ToString();
                    string QCSISDIM = row.Cells.FromKey("QCSISDIM").ToString();

                    PSNQUERYCONDITIONSETET para = new PSNQUERYCONDITIONSETET();
                    para.QCSID = QCSID;
                    para.QCSFIELD = QCSFIELD;
                    para.QCSORDER = int.Parse(QCSORDER);//需要修改
                    para.QCSTITLE = QCSTITLE;
                    para.QCSTYPE = QCSTYPE;
                    para.QCSDEFAULTVALUE = QCSDEFAULTVALUE;
                    para.QCSVALUELIST = QCSVALUELIST;
                    para.QCSISDIM = QCSISDIM;
                    para.QSID = txtReportID.Text;
                    para.QCS4 = QCS4;
                    para.QCS5 = QCS5;
                    parada.Update(para);
                }


                //delete
                List<int> delList = grdMain.GetDeletedList();
                foreach (int rowIndex in delList)
                {
                     var row = this.grdMain.Rows[rowIndex];
                     if (row.Cells.FromKey("QCSID") == null || row.Cells.FromKey("QCSID").ToString() == "")
                         continue;
                    string QCSID = row.Cells.FromKey("QCSID").ToString();
                   
                    parada.Delete(QCSID);
                }

            }
            else
            {
                error += base.getString("ZGAIA05798") + ",";
            }
            #endregion

            #region 列表字段设置部分
            if (isFieldCheck) 
            {
                //add
                List<Dictionary<string, object>> addedList = grdMain1.GetNewAddedList();
                var enumerator = addedList.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var val = enumerator.Current;
                    string QRSID = Guid.NewGuid().ToString();
                    string QRSDISPLAYFIELD = val["QRSDISPLAYFIELD"].ToString();
                    if (QRSDISPLAYFIELD == "")
                    {
                        continue;
                    }
                    string QRSORDER = isnull(val["QRSORDER"]);
                    string Title = isnull(val["QRSTITLE"]);
                    string QRS4 = "";
                    string QRS5 = "";
                    PSNQUERYRESULTSETET para = new PSNQUERYRESULTSETET();
                    para.QRSID = QRSID;
                    para.QRSDISPLAYFIELD = QRSDISPLAYFIELD;
                    para.QRSORDER = int.Parse(QRSORDER);
                    para.QSID = txtReportID.Text;
                    para.QRSTITLE = Title;
                    para.QRS4 = QRS4;
                    para.QRS5 = QRS5;
                    parada1.Insert(para);
                }

                //update
                Dictionary<int, Dictionary<string, object>> modifiedDict = grdMain1.GetModifiedDict();
                foreach (int rowIndex in modifiedDict.Keys)
                {
                    var row = this.grdMain1.Rows[rowIndex];
                    if (row.Cells.FromKey("QRSID") == null || row.Cells.FromKey("QRSID").ToString() == "")
                        continue;
                    string QRSID = row.Cells.FromKey("QRSID").ToString();
                    string QRSDISPLAYFIELD = row.Cells.FromKey("QRSDISPLAYFIELD").ToString();
                    if (QRSDISPLAYFIELD == "")
                    {
                        continue;
                    }
                    string QRSORDER = row.Cells.FromKey("QRSORDER").ToString();
                    string QSID = row.Cells.FromKey("QSID").ToString();
                    string QRS4 = row.Cells.FromKey("QRS4").ToString();
                    string QRS5 = row.Cells.FromKey("QRS5").ToString();
                    string Title = row.Cells.FromKey("QRSTITLE").ToString();

                    PSNQUERYRESULTSETET para = new PSNQUERYRESULTSETET();
                    para.QRSID = QRSID;
                    para.QRSDISPLAYFIELD = QRSDISPLAYFIELD;
                    para.QRSORDER = int.Parse(QRSORDER);
                    para.QSID = txtReportID.Text;
                    para.QRSTITLE = Title;
                    para.QRS4 = QRS4;
                    para.QRS5 = QRS5;
                    parada1.Update(para);
                }


                //delete
                List<int> delList = grdMain1.GetDeletedList();
                foreach (int rowIndex in delList)
                {
                    var row = this.grdMain1.Rows[rowIndex];
                    if (row.Cells.FromKey("QRSID") == null || row.Cells.FromKey("QRSID").ToString() == "")
                        continue;
                    string QRSID = row.Cells.FromKey("QRSID").ToString();

                    parada1.Delete(QRSID);
                }

            }
            else
            {
                error += base.getString("ZGAIA05799") + ",";
            }
            #endregion

            #region 详细字段设置部分
            if (isDeatilCheck)  
            {
                //add
                 List<Dictionary<string, object>> addedList = grdMain2.GetNewAddedList();
                var enumerator = addedList.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var val = enumerator.Current;
                    string QDSID = Guid.NewGuid().ToString();
                    string QDSFIELD = val["QDSFIELD"].ToString();
                    if (QDSFIELD == "")
                    {
                        continue;
                    }
                    string QDSVALUELIST = isnull(val["QDSVALUELIST"]);
                    string QDSORDER = isnull(val["QDSORDER"]);
                    string QDSNAME = isnull(val["QDSNAME"]);
                    string QDS4 = "";
                    string QDS5 = "";
                    string QDSTYPE = isnull(val["QDSTYPE"]);
                    string QDSISREAD = isnull(val["QDSISREAD"]);
                    string QDSISCHANGE = isnull(val["QDSISCHANGE"]);
                    PSNQUERYDETAILSETET para = new PSNQUERYDETAILSETET();
                    para.QDSID = QDSID;
                    para.QDSFIELD = QDSFIELD;
                    para.QDSVALUELIST = QDSVALUELIST;
                    para.QDSORDER = int.Parse(QDSORDER);//需要修改
                    para.QDSNAME = QDSNAME;
                    para.QDSTYPE = QDSTYPE;
                    para.QDSISREAD = QDSISREAD;
                    para.QSID = txtReportID.Text;
                    para.QDS4 = QDS4;
                    para.QDS5 = QDS5;
                    para.QDSISCHANGE = QDSISCHANGE;
                    para.IsConfigurable = "NO";
                    parada2.Insert(para);

                }

                //update

                Dictionary<int, Dictionary<string, object>> modifiedDict = grdMain2.GetModifiedDict();
                foreach (int rowIndex in modifiedDict.Keys)
                {
                    var row = this.grdMain2.Rows[rowIndex];
                    if (row.Cells.FromKey("QDSID") == null || row.Cells.FromKey("QDSID").ToString() == "")
                        continue;
                    string QDSID = row.Cells.FromKey("QDSID").ToString();
                    string QDSFIELD = row.Cells.FromKey("QDSFIELD").ToString(); 
                    if (QDSFIELD == "")
                    {
                        continue;
                    }
                    string QDSVALUELIST = row.Cells.FromKey("QDSVALUELIST").ToString(); 
                    string QDSORDER = row.Cells.FromKey("QDSORDER").ToString(); 
                    string QDSNAME = row.Cells.FromKey("QDSNAME").ToString(); 
                    string QDS4 = row.Cells.FromKey("QDS4").ToString(); 
                    string QDS5 = row.Cells.FromKey("QDS5").ToString(); 
                    string QDSTYPE = row.Cells.FromKey("QDSTYPE").ToString(); 
                    string QDSISREAD = row.Cells.FromKey("QDSISREAD").ToString(); 
                    string QDSISCHANGE = row.Cells.FromKey("QDSISCHANGE").ToString();
                    string IsConfigurable = row.Cells.FromKey("IsConfigurable").ToString();
                    PSNQUERYDETAILSETET para = new PSNQUERYDETAILSETET();
                    para.QDSID = QDSID;
                    para.QDSFIELD = QDSFIELD;
                    para.QDSVALUELIST = QDSVALUELIST;
                    para.QDSORDER = int.Parse(QDSORDER);//需要修改
                    para.QDSNAME = QDSNAME;
                    para.QDSTYPE = QDSTYPE;
                    para.QDSISREAD = QDSISREAD;
                    para.QSID = txtReportID.Text;
                    para.QDS4 = QDS4;
                    para.QDS5 = QDS5;
                    para.QDSISCHANGE = QDSISCHANGE;
                    para.IsConfigurable = IsConfigurable;
                    parada2.Update(para);
                }

                //del
                List<int> delList = grdMain2.GetDeletedList();
                foreach (int rowIndex in delList)
                {
                    var row = this.grdMain2.Rows[rowIndex];
                    if (row.Cells.FromKey("QDSID") == null || row.Cells.FromKey("QDSID").ToString() == "")
                        continue;
                    string QDSID = row.Cells.FromKey("QDSID").ToString();

                    parada2.Delete(QDSID);
                }

            }
            else
            {
                error += base.getString("ZGAIA05800") + ",";
            }
            #endregion

            #region 新增页面字段部分
            if (isNewFieldCheck)
            {
                //add
                 List<Dictionary<string, object>> addedList = grdMain3.GetNewAddedList();
                var enumerator = addedList.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var val = enumerator.Current;
                    string QASID = Guid.NewGuid().ToString();
                    string QASFIELD = isnull(val["QASFIELD"]);
                    if (QASFIELD == "")
                    {
                        continue;
                    }
                    string QASTITLE = isnull(val["QASTITLE"]);
                    string QAS4 = "";
                    string QAS5 = "";
                    string QASTYPE = isnull(val["QASTYPE"]);
                    string QASVALUELIST = isnull(val["QASVALUELIST"]);
                    string QASDEFAULTVALUE = isnull(val["QASDEFAULTVALUE"]);
                    string QAISMUSTIN = isnull(val["QAISMUSTIN"]);
                    string QASORDER = isnull(val["QASORDER"]);

                    PSNQUERYADDSETET para = new PSNQUERYADDSETET();
                    para.QASID = QASID;
                    para.QASFIELD = QASFIELD;
                    para.QASTITLE = QASTITLE;
                    para.QASTYPE = QASTYPE;
                    para.QASVALUELIST = QASVALUELIST;
                    para.QASDEFAULTVALUE = QASDEFAULTVALUE;
                    para.QAISMUSTIN = QAISMUSTIN;
                    para.QASORDER = int.Parse(QASORDER);
                    para.QSID = txtReportID.Text;
                    para.QAS4 = QAS4;
                    para.QAS5 = QAS5;
                    parada3.Insert(para);
                }

                //update
                 Dictionary<int, Dictionary<string, object>> modifiedDict = grdMain3.GetModifiedDict();
                 foreach (int rowIndex in modifiedDict.Keys)
                 {
                     var row = this.grdMain3.Rows[rowIndex];
                     if (row.Cells.FromKey("QASID") == null || row.Cells.FromKey("QASID").ToString() == "")
                         continue;
                     string QASID = row.Cells.FromKey("QASID").ToString();
                     string QASFIELD = row.Cells.FromKey("QASFIELD").ToString();
                     if (QASFIELD == "")
                     {
                         continue;
                     }
                     string QASTITLE = row.Cells.FromKey("QASTITLE").ToString(); 
                     string QAS4 = row.Cells.FromKey("QAS4").ToString(); 
                     string QAS5 = row.Cells.FromKey("QAS5").ToString(); 
                     string QASTYPE = row.Cells.FromKey("QASTYPE").ToString();
                     string QASVALUELIST = row.Cells.FromKey("QASVALUELIST").ToString(); 
                     string QASDEFAULTVALUE = row.Cells.FromKey("QASDEFAULTVALUE").ToString();
                     string QAISMUSTIN = row.Cells.FromKey("QAISMUSTIN").ToString(); 
                     string QASORDER = row.Cells.FromKey("QASORDER").ToString();
                     PSNQUERYADDSETET para = new PSNQUERYADDSETET();
                     para.QASID = QASID;
                     para.QASFIELD = QASFIELD;
                     para.QASTITLE = QASTITLE;
                     para.QASTYPE = QASTYPE;
                     para.QASVALUELIST = QASVALUELIST;
                     para.QASDEFAULTVALUE = QASDEFAULTVALUE;
                     para.QAISMUSTIN = QAISMUSTIN;
                     para.QASORDER = int.Parse(QASORDER);
                     para.QSID = txtReportID.Text;
                     para.QAS4 = QAS4;
                     para.QAS5 = QAS5;
                     parada3.Update(para);
                 }

                //del
                 List<int> delList = grdMain3.GetDeletedList();
                 foreach (int rowIndex in delList)
                 {
                     var row = this.grdMain3.Rows[rowIndex];
                     if (row.Cells.FromKey("QASID") == null || row.Cells.FromKey("QASID").ToString() == "")
                         continue;
                     string QASID = row.Cells.FromKey("QASID").ToString();

                     parada3.Delete(QASID);
                 }
            }
            else
            {

                if (flag == "1")
                {
                    error += base.getString("ZGAIA05804");
                }
                else
                {
                    error += base.getString("ZGAIA05801");
                }


            }
            #endregion
            if (error != "")
            {
                base.ShowAlert(error + "  " + base.getString("ZGAIA05802"));
                Bind();
                return;
            }
            //保存新增页面字段后确定必选字段是否全部选择
            if (!AddCheck())
            {
                base.ShowAlert(getString("姓名、工号、入职日、在职状态、部门为必选项，请检查"));
                Bind();
                return;
            }


            Bind();


            base.ShowAlert(getString("ZGAIA01384"));

        }

        

        /// <summary>
        /// 查询条件判定
        /// </summary>
        /// <returns></returns>
        private bool HsCheckQuery()
        {
            Hashtable hsCheckQuery = new Hashtable();
            bool IsCheck = true;
            for (int i = 0; i < grdMain.Rows.Count; i++)
            {
                if (grdMain.GetDeletedList().Contains(i))
                {
                    continue;
                }
                if (grdMain.Rows[i].Cells.FromKey("QCSID") == null)
                {
                    grdMain.Rows[i].Cells.SetValue("QCSID", Guid.NewGuid().ToString());
                }
                string QCSID = isnull(grdMain.Rows[i].Cells.FromKey("QCSID"));
                string QCSFIELD = isnull(grdMain.Rows[i].Cells.FromKey("QCSFIELD"));
                string QCSTITLE = isnull(grdMain.Rows[i].Cells.FromKey("QCSTITLE"));

                if (!hsCheckQuery.Contains(QCSTITLE))
                {
                    hsCheckQuery.Add(QCSTITLE, QCSFIELD);
                }
                else
                {
                    IsCheck = false;
                    return IsCheck;
                }
            }
            return IsCheck;
        }

        /// <summary>
        /// 字段设置判断
        /// </summary>
        /// <returns></returns>
        private bool HsCheckFieldSet()
        {
            Hashtable hsCheckFieldSet = new Hashtable();
            bool IsCheck = true;
            for (int i = 0; i < grdMain1.Rows.Count; i++)
            {
                if (grdMain1.GetDeletedList().Contains(i))
                {
                    continue;
                }
                if (grdMain1.Rows[i].Cells.FromKey("QRSID") == null)
                {
                    grdMain1.Rows[i].Cells.SetValue("QRSID",Guid.NewGuid().ToString());
                }
                string QRSID = isnull(grdMain1.Rows[i].Cells.FromKey("QRSID"));
                string QRSDISPLAYFIELD = isnull(grdMain1.Rows[i].Cells.FromKey("QRSDISPLAYFIELD"));

                string Title = isnull(grdMain1.Rows[i].Cells.FromKey("QRSTITLE"));
                if (!hsCheckFieldSet.Contains(Title))
                {
                    hsCheckFieldSet.Add(Title, QRSDISPLAYFIELD);
                }
                else
                {
                    IsCheck = false;
                    return IsCheck;
                }
            }
            return IsCheck;
        }
        /// <summary>
        /// 详细字段判断
        /// </summary>
        /// <returns></returns>
        private bool HsCheckDeatil()
        {
            Hashtable hsCheckDeatil = new Hashtable();
            bool IsCheck = true;
            for (int i = 0; i < grdMain2.Rows.Count; i++)
            {
                if (grdMain2.GetDeletedList().Contains(i))
                {
                    continue;
                }

                

                if (grdMain2.Rows[i].Cells.FromKey("QDSID") == null)
                {
                    grdMain2.Rows[i].Cells.SetValue("QDSID", Guid.NewGuid().ToString());
                }
                string QDSID = isnull(grdMain2.Rows[i].Cells.FromKey("QDSID"));
                string QDSFIELD = isnull(grdMain2.Rows[i].Cells.FromKey("QDSFIELD"));
                string QDSNAME = isnull(grdMain2.Rows[i].Cells.FromKey("QDSNAME"));
                if (!hsCheckDeatil.Contains(QDSNAME))
                {
                    hsCheckDeatil.Add(QDSNAME, QDSFIELD);
                }
                else
                {
                    IsCheck = false;
                    return IsCheck;
                }
            }
            return IsCheck;
        }
        /// <summary>
        /// 新增字段判断
        /// </summary>
        /// <returns></returns>
        private bool HsCheckNewField(ref string flag)
        {
            Hashtable hsCheckNewField = new Hashtable();
            bool IsCheck = true;

            for (int i = 0; i < grdMain3.Rows.Count; i++)
            {
                if (grdMain3.GetDeletedList().Contains(i))
                {
                    continue;
                }
                if (grdMain3.Rows[i].Cells.FromKey("QASID") == null)
                {
                    grdMain3.Rows[i].Cells.SetValue("QASID",Guid.NewGuid().ToString());
                }
                string QASID = isnull(grdMain3.Rows[i].Cells.FromKey("QASID"));
                string QASFIELD = isnull(grdMain3.Rows[i].Cells.FromKey("QASFIELD"));
                string QASTITLE = isnull(grdMain3.Rows[i].Cells.FromKey("QASTITLE"));

                if (!hsCheckNewField.Contains(QASTITLE))
                {
                    hsCheckNewField.Add(QASTITLE, QASFIELD);
                }
                else
                {

                    IsCheck = false;
                    return IsCheck;
                }
            }
            DataTable dt = new PSNQUERYSETDA().QueryCustomeField();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string name = dt.Rows[i]["COLUMNNAME"].ToString();
                if (!hsCheckNewField.Contains(name))
                {
                    hsCheckNewField.Add(name, name);
                }
                else
                {
                    flag = "1";
                    IsCheck = false;
                    return IsCheck;
                }

            }
            return IsCheck;
        }

        //保存之前检查新增必选字段是否选择了
        private bool AddCheck()
        {
            bool flag = false;
            string allField = string.Empty;
            int num = 0;
            for (int i = 0; i < grdMain3.Rows.Count; i++)
            {
                string QASFIELD = isnull(grdMain3.Rows[i].Cells.FromKey("QASFIELD"));
                string[] DBField = { "TRUENAME", "EMPLOYEEID", "ATTENDONDATE", "ACCESSIONSTATE", "BRANCHID" };//姓名、工号、入职日、在职状态、部门为必填项
                for (int j = 0; j < DBField.Length; j++)
                {
                    if (QASFIELD == DBField[j])
                    {
                        num++;
                    }
                }
            }
            if (num == 5)
            {
                flag = true;
            }
            return flag;
        }
        //判断是否为空
        private string isnull(object obj)
        {
            if (obj == null)
            {
                return "";
            }
            else
            {
                return obj.ToString();
            }
        }

        private void SycPsnQuerySetDetail(PersistBroker _broker, string QSID)
        {
            string tableId = "";
            DataTable dtGetTableId = _broker.ExecuteSQLForDst("SELECT * FROM QCTABLEINFO WHERE PHYSICALNAME='QC_PSN_MAIN'").Tables[0];
            if (dtGetTableId.Rows.Count > 0)
            {
                tableId = dtGetTableId.Rows[0]["TABLEID"].ToString();
            }
            if (tableId == "")
            {
                return;
            }
            TableInfoLoader tableInfoLoader = new TableInfoLoader();
            tableInfoLoader.SyncPsnQuerySetDetail(_broker, tableId);
        }

        protected void PageManager1_CustomEvent(object sender, CheerUI.CustomEventArgs e)
        {
            Bind();
        }
    }
}