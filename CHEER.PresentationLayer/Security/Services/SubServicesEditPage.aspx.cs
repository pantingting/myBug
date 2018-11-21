using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.Security.Services;
using CHEER.Common;
using CHEER.CommonLayer.eSecurity.Data;
using CHEER.PresentationLayer;

namespace CHEER.PresentationLayer.Security.Services
{
    public partial class SubServicesEditPage : CHEERBasePage
    {
        private string SubServicesOID
        {
            get { return this.Request["OID"]; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                InitFace();
                CheckPageAccess();
                LoadData();
            }
        }
        private void InitFace()
        {
            this.txtAppNO.Label = "应用程序名";
            this.txtComName.Label = "注册名称";
            this.txtModuleNO.Label = "模块编号";
            this.txtFileName.Label = "所在文件";
            this.txtRunState.Label = "当前状态";
            this.txtShowName.Label = "服务名";
            this.txtLogFileDir.Label = "日志路径";
            this.txtCustomParameter.Label = "自定义参数";
            this.txtCustomParameter.MaxLengthMessage = "(不得超过1000字)";
            this.txtDescription.Label = "描 述";
            this.txtDescription.MaxLengthMessage = "(不得超过200字)";
            this.drpRunType.Label = "运行状态";
            this.drpStartType.Label = "启动类型";
            this.txtRunValue.Label = "运行值";
            this.tpTimeRunTime.Label = "运行时间";
            //this.cmdRefresh.Text = "刷 新";
            this.cmdAdd.Text = "保 存";
            //this.cmdStart.Text = base.getString("ZGAIA00362");
            //this.cmdStop.Text = base.getString("ZGAIA00361");
            //this.btnRun.Text = base.getString("ZGAIA00364");
            this.drpRunType.Items.Add(new CheerUI.ListItem("实时", ((int)ServicesRunType.REAL).ToString()));
            this.drpRunType.Items.Add(new CheerUI.ListItem("每天", ((int)ServicesRunType.DAY).ToString()));
            this.drpRunType.Items.Add(new CheerUI.ListItem("每周", ((int)ServicesRunType.WEEK).ToString()));
            this.drpRunType.Items.Add(new CheerUI.ListItem("每月", ((int)ServicesRunType.MONTH).ToString()));

            this.drpStartType.Items.Add(new CheerUI.ListItem("手动", ((int)ServiesStartType.Manual).ToString()));
            this.drpStartType.Items.Add(new CheerUI.ListItem("自动", ((int)ServiesStartType.Auto).ToString()));	
            
        }

        private void CheckPageAccess()
        {
            if (SecurityHelp.isSA(GetSecurityChecker().UserID) == false)
            {
                //ShowErrorInfo("该功能只对sa开放，请用sa登录后再试!");
                ShowAlert("该功能只对sa开放，请用sa登录后再试!");
            }
        }

        private void SetRunTypeInfo(ServicesRunType runtype)
        {
            switch (runtype)
            {
                case ServicesRunType.REAL:
                    this.tpTimeRunTime.Readonly = true;

                    this.txtRunValue.Readonly = false;
                    break;
                case ServicesRunType.DAY:
                    this.tpTimeRunTime.Readonly = false;
                    this.txtRunValue.Readonly = true;

                    break;
                case ServicesRunType.WEEK:
                case ServicesRunType.MONTH:
                    this.tpTimeRunTime.Readonly = false;
                    this.txtRunValue.Readonly = false;
                    break;
            }
        }

        private void LoadData()
        {
            SubServicesManager subServicesManager = (SubServicesManager)eHRPageServer.GetPalauObject(typeof(SubServicesManager));
            HRSSubSRegisterData data = subServicesManager.GetSubServices(SubServicesOID);
            this.txtAppNO.Text = data.CURAPPNO.ToString();
            this.txtModuleNO.Text = data.MODULENO;
            this.txtComName.Text = data.COMPONENTNAME;
            this.txtFileName.Text = data.FILENAME;
            this.txtRunState.Text = GetRunStateStr((ServicesRunStateType)data.RUNSTATE);
            this.txtShowName.Text = data.NAME;
            this.txtLogFileDir.Text = data.LOGFOLDER;
            this.txtCustomParameter.Text = data.CUSTOMPARAMETER;
            this.txtDescription.Text = data.DESCRIPTION;
            this.drpRunType.SelectedValue = data.RUNTYPE.ToString();
            this.txtRunValue.Text = data.RUNVALE;
            this.tpTimeRunTime.Text = data.RUNTIME;
            this.drpStartType.SelectedValue = data.STARTTYPE.ToString();
            if (data.SetUrl.Length > 0)
            {
                this.btnOtherSet.Visible = true;
                this.btnOtherSet.OnClientClick = "return showother('" + data.SetUrl.Substring(1) + "')";
            }
            else
            {
                this.btnOtherSet.Visible = false;
            }
            
            SetRunTypeInfo((ServicesRunType)data.RUNTYPE);
        }
        //private void SetRunStateInfo(ServicesRunStateType runState)
        //{
        //    switch (runState)
        //    {
        //        case ServicesRunStateType.STOP:
        //        case ServicesRunStateType.STOPING:
        //            this.cmdStart.Enabled = true;
        //            this.cmdStop.Enabled = false;
        //            break;
        //        case ServicesRunStateType.STARTING:
        //        case ServicesRunStateType.RUN:
        //        case ServicesRunStateType.LISTEN:
        //            this.cmdStart.Enabled = false;
        //            this.cmdStop.Enabled = true;
        //            break;
        //    }
        //}
        private string GetRunStateStr(ServicesRunStateType runState)
        {
            string runStateStr = "";
            switch (runState)
            {
                case ServicesRunStateType.STOP:
                    runStateStr = "停止";
                    break;
                case ServicesRunStateType.STARTING:
                    runStateStr = "启动中";
                    break;
                case ServicesRunStateType.RUN:
                    runStateStr = "运行中";
                    break;
                case ServicesRunStateType.LISTEN:
                    runStateStr = "监听中";
                    break;
                case ServicesRunStateType.STOPING:
                    runStateStr = "停止中";
                    break;
            }
            return runStateStr;
        }
        private HRSSubSRegisterData CreateData()
        {
            HRSSubSRegisterData data = new HRSSubSRegisterData();
            data.OID = this.SubServicesOID;
            data.NAME = this.txtShowName.Text;
            data.LOGFOLDER = this.txtLogFileDir.Text;
            data.CUSTOMPARAMETER = this.txtCustomParameter.Text;
            data.DESCRIPTION = this.txtDescription.Text;
            data.RUNTYPE = Convert.ToInt32(this.drpRunType.SelectedValue);
            data.RUNVALE = this.txtRunValue.Text;
            data.RUNTIME = this.tpTimeRunTime.Text;
            data.STARTTYPE = Convert.ToInt32(this.drpStartType.SelectedValue);
            return data;
        }

        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            SubServicesManager subServicesManager = (SubServicesManager)eHRPageServer.GetPalauObject(typeof(SubServicesManager));
            HRSSubSRegisterData data = CreateData();
            try
            {
                subServicesManager.Update(data);
                LoadData();
                ShowAlert("保存成功！服务重新启动后修改内容才会正式使用！");
            }
            catch (Exception ex)
            {
                base.ShowAlert(ex.Message);
            }
            LoadData();
        }

    }
}