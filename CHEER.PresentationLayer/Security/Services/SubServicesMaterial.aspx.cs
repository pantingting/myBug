using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.Security.Services;
using CHEER.Common;
using CHEER.Common.eHRService;
using CHEER.CommonLayer.eSecurity.Data;
using CHEER.CommonLayer.eSecurity.Schema;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;

namespace CHEER.PresentationLayer.Security.Services
{
    public partial class SubServicesMaterial : CHEERBasePage
    {
        private int CURAPPNO
        {
            get { return int.Parse(this.Request["CURAPPNO"]); }
        }
        private bool IsRegister
        {
            get
            {
                if (this.Request["ISREGISTER"] == null || this.Request["ISREGISTER"] != "0")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitFace();
                LoadData();
            }
        }

        CheerUI.Button btnRunstate = new CheerUI.Button();

        private void InitFace()
        {
            
            this.txtAppNO.Label = "应用程序号";
            this.txtLastTime.Label = "最后运行时间";
            this.txtRMachineName.Label = "机器名";
            this.cmdRefresh.Text = "刷 新";
            this.lblAlertMsg.Text = "主服务最后运行时间间隔过长，主服务可能没有启动，请确保主服务的已启动！";
            btnRunstate.Text = "开始";
            //this.TextBox1.Text = base.getString("ZGAIA00338");
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            InitGrid();
        }
        private void LoadData()
        {
            MainServicesManager mainServicesManager = (MainServicesManager)eHRPageServer.GetPalauObject(typeof(MainServicesManager));
            HRSMainSRunData mainServicesInfo = mainServicesManager.GetMainServices(this.CURAPPNO);
            this.txtAppNO.Text = this.CURAPPNO.ToString();
            this.txtRMachineName.Text = mainServicesInfo.MACHINENAME;
            if (mainServicesInfo.LASTTIME == null || mainServicesInfo.LASTTIME.Length == 0)
            {
                this.txtLastTime.Text = mainServicesInfo.LASTTIME;
            }
            else
            {
                this.txtLastTime.Text = DataProcessor.DateTimeToLongString(DateTime.Parse(mainServicesInfo.LASTTIME));
            }
            if (this.txtLastTime.Text == "")
            {
                this.lblAlertMsg.Visible = true;
            }
            else
            {
                if (DateTime.Parse(mainServicesInfo.NOWTIME).Subtract(DateTime.Parse(mainServicesInfo.LASTTIME)).TotalSeconds > (ServicesList.ChkLastUpTimeSp + 20))
                {
                    this.lblAlertMsg.Visible = true;
                }
                else
                {
                    this.lblAlertMsg.Visible = false;
                }
            }

            DataTable dt = new CHEERSQL().ExecuteSQLforDataTable(@"SELECT MODULENO,NAME,FILENAME,LOGFOLDER,CUSTOMPARAMETER,DESCRIPTION,STARTTYPE,CURAPPNO,LASTTIME
                ,LASTOPERATOR,COMPONENTNAME,RUNSTATE,ISMULTI,ISENABLE,OID,SetUrl,SetUrlWinPara
                ,(case when RUNTYPE='0' then '实时' when RUNTYPE='1' then '每天' when RUNTYPE='2' then '每周' when RUNTYPE='3' then '每月' end) as RUNTYPE,
                (case when RUNTYPE='1' then '' else RUNVALE end) as RUNVALE,
                (case when RUNTYPE='0' then '' else RUNTIME end) as RUNTIME
                FROM HRSSUBSREGISTER WHERE CURAPPNO = " + CURAPPNO + "  order by NAME ");

            UlUserGrid.DataSource = dt;

            UlUserGrid.DataBind();
        }
        protected void InitGrid()
        {
            CommonMethod.AddFlexField(this.UlUserGrid, "", HRSSubSRegisterSchema.OID, 0, true);
            CommonMethod.AddFlexField(this.UlUserGrid, "", HRSSubSRegisterSchema.COMPONENTNAME, 0, true);
            CommonMethod.AddFlexField(this.UlUserGrid, "服务名", HRSSubSRegisterSchema.NAME, 25, false);
            CommonMethod.AddFlexField(this.UlUserGrid, "描述", HRSSubSRegisterSchema.DESCRIPTION, 25, false);
            CommonMethod.AddFlexField(this.UlUserGrid, "运行状态", HRSSubSRegisterSchema.RUNTYPE, 5, false);
            CommonMethod.AddFlexField(this.UlUserGrid, "运行值", HRSSubSRegisterSchema.RUNVALE, 5, false);
            CommonMethod.AddFlexField(this.UlUserGrid, "运行时间", HRSSubSRegisterSchema.RUNTIME, 5, false);
            CommonMethod.AddFlexField(this.UlUserGrid, "启动类型", HRSSubSRegisterSchema.STARTTYPE, 5, false);
            CommonMethod.AddFlexRendererField(this.UlUserGrid, "当前状态", HRSSubSRegisterSchema.RUNSTATE, 5, "", false).RendererFunction = "renderMe";
            CommonMethod.AddFlexRendererField(this.UlUserGrid, "使用状态", HRSSubSRegisterSchema.ISENABLE, 5, "", false).RendererFunction = "isEnable";
            CommonMethod.AddWindowField(this.UlUserGrid, "编辑", "EDIT", 40, "EditPage", HRSSubSRegisterSchema.OID, "SubServicesEditPage.aspx?OID={0}", "服务详细信息");
           
        }

        protected void UlUserGrid_RowDataBound(object sender, CheerUI.GridRowEventArgs e)
        {
            string startype= UlUserGrid.Rows[e.RowIndex].Cells.FromKey(HRSSubSRegisterSchema.STARTTYPE).ToString();
            if (startype == "1")
                UlUserGrid.Rows[e.RowIndex].Cells.SetValue(HRSSubSRegisterSchema.STARTTYPE, "自动");
            else {
                UlUserGrid.Rows[e.RowIndex].Cells.SetValue(HRSSubSRegisterSchema.STARTTYPE, "手动");
            }

            
        }

        protected void PageManager1_CustomEvent(object sender, CheerUI.CustomEventArgs e)
        {
            SubServicesManager subServicesManager = (SubServicesManager)eHRPageServer.GetPalauObject(typeof(SubServicesManager));
            string OID = Oids.Value;

            if (e.EventArgument == "RUNSTATE_Click")
            {
                switch ((ServicesRunStateType)Convert.ToInt32(cellValues.Value))
                {
                    case ServicesRunStateType.STOP:
                        subServicesManager.Start(OID);
                        break;
                    case ServicesRunStateType.STOPING:
                        base.ShowAlert("当前服务正在停止中，不允许启动！");
                        break;
                    case ServicesRunStateType.STARTING:
                        base.ShowAlert("当前服务正在启动中，不允许停止！");
                        break;
                    case ServicesRunStateType.RUN:
                    case ServicesRunStateType.LISTEN:
                        subServicesManager.Stop(OID);
                        break;	
                }
            }
            else if (e.EventArgument == "ISENABLE_Click")
            {
                if (Convert.ToInt32(cellValues.Value) == 0)
                {
                    subServicesManager.Enable(OID);
                }
                else
                {
                    subServicesManager.Disable(OID);
                }
            }
            LoadData();
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        
        
    }
}