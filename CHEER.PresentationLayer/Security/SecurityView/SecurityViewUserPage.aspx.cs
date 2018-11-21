using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.CommonLayer.ePersonnel;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;

namespace CHEER.PresentationLayer.Security.SecurityView
{
    public partial class SecurityViewUserPage : CHEERBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                get_transdata();
                Bind(0);
            }
        }

        private string ModuleID
        {
            get { return (string)ViewState["ModuleID"]; }
            set { ViewState["ModuleID"] = value; }
        }
     
        private void get_transdata()
        {
            if (Request.QueryString["MODULEID"] != null)
                this.ModuleID = Request.QueryString["MODULEID"].Trim();
            else
                this.ModuleID = "";
        }
        private void Bind(int index)
        {
            if(Session[AppConstants.SESSION_PAGE_MANAGER]==null)
				return ;
			Hashtable menuidhash = new Hashtable();
			string[] allmenuidlist = Session[AppConstants.SESSION_PAGE_MANAGER].ToString().Split(' ');;
            string menuidlist = "";
            foreach (string id in allmenuidlist)
            {
                menuidlist += "'" + id.Split('|')[0] + "',";
            }
            
            if (menuidlist.Length > 0)
            {
                menuidlist = menuidlist.Substring(0, menuidlist.Length - 1);
                SecurityViewBL viewBL = (SecurityViewBL)eHRPageServer.GetPalauObject(typeof(SecurityViewBL));
                DataSet ds = viewBL.GetSecurityInfoForUser(menuidlist, ModuleID, base.getBusinessUnitID());
                ds = this.CheckData(ds);
                mYGrid.DataSource = ds.Tables[0];
                //mYGrid.RecordCount = ds.Tables[0].Rows.Count;
                //mYGrid.DataSource = GetPagedDataTable(index, mYGrid.PageSize, ds.Tables[0]); 

            }
            else
            {
                mYGrid.DataSource = new DataTable();
            }
            mYGrid.DataBind();
        }

        private DataTable GetPagedDataTable(int pageIndex, int pageSize, DataTable dtSource)
        {
            DataTable source = dtSource;
            DataTable paged = source.Clone();
            int rowbegin = pageIndex * pageSize;
            int rowend = (pageIndex + 1) * pageSize;
            if (rowend > source.Rows.Count)
            {
                rowend = source.Rows.Count;
            }
            for (int i = rowbegin; i < rowend; i++)
            {
                paged.ImportRow(source.Rows[i]);
            }
            return paged;
        }


        private string AddMenuidHash(string[] allmenuidlist, Hashtable menuidhash)
        {
            foreach (string menuidfull in allmenuidlist)
            {
                if (menuidfull.Trim() != "")
                {
                    string menuflag = menuidfull.Trim().Substring(0, 1);
                    string addflag = menuidfull.Trim().Substring(1, 1);
                    string menuid = menuidfull.Substring(2);
                    if (addflag == "#")
                    {
                        if (!menuidhash.Contains(menuid))
                        {
                            if (menuflag.ToUpper().Trim() == "M")
                                menuidhash.Add(menuid, 1);
                            else
                                menuidhash.Add(menuid, 0);
                        }
                    }
                    if (addflag == "=")
                    {
                        if (menuidhash.Contains(menuid))
                            menuidhash.Remove(menuid);
                    }
                }
            }
            string menuidlist = "";
            if (menuidhash.Count > 0)
            {
                IDictionaryEnumerator IDE = menuidhash.GetEnumerator();
                IDE.Reset();
                while (IDE.MoveNext())
                {
                    menuidlist += "'" + IDE.Key + "',";
                }
                if (menuidlist.Length > 0)
                    menuidlist = menuidlist.Substring(0, menuidlist.Length - 1);
            }
            return menuidlist;
        }
        private DataSet CheckData(DataSet checkinfods)
        {
            foreach (DataRow dr in checkinfods.Tables[0].Rows)
            {
                string ifMenu = dr["IFMENU"].ToString();
                string functionMenuName;
                string menuName;
                if (ifMenu.CompareTo("1") == 0)
                {
                    functionMenuName = dr["FUNCTIONNAME"].ToString();
                    menuName = dr["MENUNAME"].ToString();
                    dr["FUNCTIONNAME"] = menuName;
                    dr["MENUNAME"] = "";
                }
            }
            return checkinfods;
        }
      

        protected void Page_Init()
        {

            if (base.GetLanguageInfo() == Language.US)
            {
                CommonMethod.AddFlexField(this.mYGrid, "菜单名称", "FUNCTIONENGNAME", 15, false);
                CommonMethod.AddFlexField(this.mYGrid, "功能点名称", "MENUENGLISHNAME", 15, false);
                CommonMethod.AddFlexField(this.mYGrid, "功能点名称", "MENUNAME", 15, true);
                CommonMethod.AddFlexField(this.mYGrid, "功能点名称", "FUNCTIONNAME", 15, true);
            }
            else
            {
                CommonMethod.AddFlexField(this.mYGrid, "菜单名称", "FUNCTIONNAME", 15, false);
                CommonMethod.AddFlexField(this.mYGrid, "功能点名称", "FUNCTIONENGNAME", 15, true);
                CommonMethod.AddFlexField(this.mYGrid, "功能点名称", "MENUNAME", 15, false);
                CommonMethod.AddFlexField(this.mYGrid, "功能点名称", "MENUENGLISHNAME", 15, true);
            }
            CommonMethod.AddFlexField(this.mYGrid, "登录名", "LOGINNAME", 15, false);
            CommonMethod.AddFlexField(this.mYGrid, "姓名", "TRUENAME", 15, false);
            CommonMethod.AddFlexField(this.mYGrid, "工号", "EMPLOYEEID", 15, false);
            CommonMethod.AddFlexField(mYGrid, "", "USERID", 0,true);
            CommonMethod.AddFlexField(mYGrid, "", "IFMENU", 0, true);
            CommonMethod.AddFlexField(mYGrid, "", "FUNCTIONLABEL", 0, true);
            CommonMethod.AddFlexField(mYGrid, "", "LABEL", 0, true);
        }

        protected void mYGrid_PageIndexChange(object sender, CheerUI.GridPageEventArgs e)
        {
            mYGrid.PageIndex = e.NewPageIndex;
            CheerUI.PageContext.RegisterStartupScript("mer()");
            //Bind(mYGrid.PageIndex);
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.mYGrid.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            CheerUI.PageContext.RegisterStartupScript("mer()");
        }
    }
}