using CHEER.BusinessLayer.ePersonnel;
using CHEER.BusinessLayer.ePersonnel.Adjust;
using CHEER.BusinessLayer.ePersonnel.LaborAccount;
using CHEER.BusinessLayer.Security;
using CHEER.Common;
using CHEER.PresentationLayer;
using CHEER.PresentationLayer.CommonUse;
using CHEER.Platform.DAL;
using CHEER.Platform.DAL.SQLCenter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CheerUI;
using System.Text;
namespace CHEER.PresentationLayer.ePersonnel.PSNQuerySet
{
    public partial class PSNGrid : CHEERBasePage
    {
        private const string PageFunctionID = "00900004";
        private string QSID = "e00497f3-0660-4851-ada9-9b4d4e88493e";
        protected CHEER.PresentationLayer.Controls.UserQuery UserQuery;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SecurityChecker sc = base.GetSecurityChecker();
                if (!sc.IsAllow(PageFunctionID))
                {
                    base.ShowAlert(base.getAlert("ZGAIA00809"));
                    return;
                }
                AddRendererFunction();
                UserQuery.Security_Checker = base.GetSecurityChecker();
                UserQuery.FunFriendlyID = PageFunctionID;

                LoadAccessState();

                btnAdd.Hidden = !sc.IsAllow("009000040001");
                btnExport.Hidden = !sc.IsAllow("009000040002");
                LoadData(0);


            }

        }
        protected void Page_Init()
        {
            InitGrid();
        }
        private void AddRendererFunction()
        {


        }
        protected void InitGrid()
        {
            SecurityChecker sc = base.GetSecurityChecker();

            //公用代码
            DataTable dt = new PSNQUERYSETLoader().QueryResultSet(QSID);
            CommonMethod.AddFlexField(grdMain, "", "PERSONID", 0, true);
            string lang = InfomationPackage.LanguageCulture;
            string tit = "QRSTITLE";
            if (lang == "EN-US")
                tit = "QRS4";
            if (lang == "ZH-TW")
                tit = "QRS5";
            Hashtable FieldValueHT = GetFieldValueHT();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string QRSDISPLAYFIELD = dt.Rows[i]["QRSDISPLAYFIELD"].ToString();

                if (QRSDISPLAYFIELD == "BRANCHID") //部门
                {
                    CommonMethod.AddFlexRendererField(grdMain, "部门", QRSDISPLAYFIELD, 10, @"", false);
                    continue;
                }
                if (FieldValueHT.ContainsKey(QRSDISPLAYFIELD))
                {

                    object obj = FieldValueHT[QRSDISPLAYFIELD];
                    if (obj is DataTable)
                    {
                        DataTable dtSL = (DataTable)obj;

                    }
                    else if (obj is ArrayList)
                    {
                        ArrayList arrL = (ArrayList)obj;

                    }

                    CommonMethod.AddFlexRendererField(grdMain, dt.Rows[i][tit].ToString(), QRSDISPLAYFIELD, 10, "", false);

                    continue;
                }

                CommonMethod.AddFlexField(grdMain, dt.Rows[i][tit].ToString(), QRSDISPLAYFIELD, 10, false);


                //设置字段格式

            }
            if (sc.IsAllow("009000040001"))
            {
                CommonMethod.AddDetailField(grdMain, getString("ZGAIA00089"), "DETAIL", false, false).CommandName = "DETAIL";
            }

        }

        //设置网格字段的ValueList
        private Hashtable GetFieldValueHT()
        {
            Hashtable ht = new Hashtable();
            if (ViewState["FIELDINX"] == null)
            {
                CHEERSQL hs = new CHEERSQL();
                DataTable dt = hs.ExecuteSQLforDataTable(@"select QDSFIELD,QDSTYPE,QDSVALUELIST from PSNQUERYDETAILSET 
                where QDSTYPE in ('DEFAULTLIST','SQLLIST')  and QDSFIELD in (select QRSDISPLAYFIELD from PSNQUERYRESULTSET) ");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!ht.ContainsKey(dt.Rows[i]["QDSFIELD"].ToString()))
                    {
                        string strvl = dt.Rows[i]["QDSVALUELIST"].ToString();
                        if (dt.Rows[i]["QDSTYPE"].ToString() == "DEFAULTLIST")  //【DEFAULTLIST】 2|M - 男,1|F - 女
                        {
                            try
                            {
                                ArrayList arrL = new ArrayList();
                                string[] items = strvl.Split(',');
                                for (int m = 0; m < items.Length; m++)
                                {
                                    string[] arrs = items[m].Split('|');
                                    if (arrs.Length == 2)
                                    {
                                        arrL.Add(arrs);
                                    }
                                }
                                ht.Add(dt.Rows[i]["QDSFIELD"].ToString(), arrL);
                            }
                            catch
                            {
                            }
                        }
                        else  //【SQLLIST】 select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='30'
                        {
                            try
                            {
                                DataTable dtSL = hs.ExecuteSQLforDataTable(strvl);
                                ht.Add(dt.Rows[i]["QDSFIELD"].ToString(), dtSL);

                            }
                            catch
                            { }
                        }

                    }
                }
                if (!ht.Contains("BRANCHID"))
                {
                    string strSQL = "select UNITID,UNITNAME From ORGSTDSTRUCT where ISTEMPUNIT=0 ";
                    DataTable _dt = new CHEERSQL().ExecuteSQLforDataTable(strSQL);
                    ht.Add("BRANCHID", _dt);
                }

                ViewState["FIELDINX"] = ht;
            }
            else
            {
                ht = (Hashtable)ViewState["FIELDINX"];
            }
            return ht;


        }
        protected void LoadAccessState()
        {
            UserQuery.AcessionStatus_Dimission = true;
            UserQuery.AcessionStatus_Export = false;
            UserQuery.AcessionStatus_Probation = true;
            UserQuery.AcessionStatus_PromotingProbation = true;
            UserQuery.AcessionStatus_Regular = true;
            UserQuery.AcessionStatus_Retired = false;
            UserQuery.AcessionStatus_Unchecked = false;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            UserQuery.btnSearchEvent += btnQuery_Click;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            LoadData(0);
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

        protected void LoadData(int pageIndex)
        {
            DataTable _DT = getData();
            ViewState["Edateset"] = _DT.DataSet;
            ResetBranch(_DT);
            ResetDatatable(_DT);
            grdMain.RecordCount = _DT.Rows.Count;
            LoadData(GetPagedDataTable(pageIndex, grdMain.PageSize, _DT));
        }

        private DataTable getData()
        {

            PsnLaborAccountLoader pll = new PsnLaborAccountLoader();
            pll.InformationPackage = base.GetSecurityInfo(PageFunctionID, true, "", true, getBusinessUnitID(), false);
            CHEER.Platform.DAL.SQLCenter.SQLSelectEntity _sSelectEntity = UserQuery.getSelectEntityWithNoSelectItems();
            // CHEER.Platform.DAL.SQLCenter.SQLSelectEntity _sSelectEntity = UserQuery1.getSelectEntityWithNoSelectItems();

            _sSelectEntity.SelectItems.Add(new TableField("PSNACCOUNT", "PERSONID"));

            DataTable dtCols = new PSNQUERYSETLoader().QueryResultSet(QSID);
            for (int i = 0; i < dtCols.Rows.Count; i++)
            {
                string QRSDISPLAYFIELD = dtCols.Rows[i]["QRSDISPLAYFIELD"].ToString();
                _sSelectEntity.SelectItems.Add(new TableField("PSNACCOUNT", QRSDISPLAYFIELD));
            }

            string PsnFilter = new SecurityHelp().GetSecuritySql(base.GetSecurityInfo(PageFunctionID, true, "", true, getBusinessUnitID(), false), _sSelectEntity);

            string sqljoin = @"FROM PSNACCOUNT left join ORGSTDSTRUCT on ORGSTDSTRUCT.UNITID = PSNACCOUNT.BRANCHID " +
                " left join PSNPUBLICCODEITEM on ITEMID= PSNACCOUNT.EMPGROUP";

            sqljoin += " WHERE";
            string sqlperson = PsnFilter.Replace("FROM PSNACCOUNT  ,ORGSTDSTRUCT WHERE", sqljoin);
            //if (sqlperson.IndexOf("and") < 0 || sqlperson.IndexOf("AND") < 0)
            //{
            //    sqlperson += "  PSNACCOUNT.status <= 2";
            //}
            //else
            //{
            //    sqlperson += " and PSNACCOUNT.status <= 2";
            //}
            sqlperson += "  ORDER BY PSNACCOUNT.EMPLOYEEID ";


            PersistBroker _broker = PersistBroker.Instance();
            DataTable _DTPerson = _broker.ExecuteSQLForDst(sqlperson).Tables[0];
            _broker.Close(); //添加 数据库关闭操作 20120607


            return _DTPerson;

        }

        protected void LoadData(DataTable dt)
        {
            grdMain.DataSource = dt;
            grdMain.DataBind();
        }

        protected void grdMain_PageIndexChange(object sender, CheerUI.GridPageEventArgs e)
        {
            grdMain.PageIndex = e.NewPageIndex;
            LoadData(e.NewPageIndex);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            CheerUI.PageContext.Redirect(getBaseUrl() + "ePersonnel/PSNQuerySet/PSNAdd.aspx");
        }

        protected void grdMain_RowDataBound(object sender, CheerUI.GridRowEventArgs e)
        {
            if (e.DataItem != null)
            {
                DataRowView row = e.DataItem as DataRowView;
                Hashtable FieldValueHT = null;
                if (ViewState["FIELDINX"] != null)
                {
                    FieldValueHT = (Hashtable)ViewState["FIELDINX"];

                    foreach (DictionaryEntry de in FieldValueHT)
                    {
                        if (row.Row.Table.Columns.Contains(de.Key.ToString()))
                        {
                            object obj = de.Value;
                            if (obj is DataTable)
                            {
                                DataTable dtSL = (DataTable)obj;
                                foreach (DataRow dr in dtSL.Rows)
                                {
                                    if (dr[0].ToString() == row[de.Key.ToString()].ToString())
                                    {
                                        grdMain.Rows[e.RowIndex].Cells.SetValue(de.Key.ToString(), dr[1].ToString());
                                    }

                                }
                            }
                            else if (obj is ArrayList)
                            {
                                foreach (string[] str in ((ArrayList)obj).ToArray())
                                {
                                    if (str[0] == row[de.Key.ToString()].ToString())
                                    {
                                        //   row[de.Key.ToString()] = str[1].ToString();
                                        grdMain.Rows[e.RowIndex].Cells.SetValue(de.Key.ToString(), str[1].ToString());
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (grdMain.Rows.Count > 0)
            {
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=" + System.Web.HttpUtility.UrlEncode(getString("ZGAIA05305")) + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls");
                Response.ContentType = "Application/excel";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(GetGridTableHtml(grdMain));
                Response.End();
            }
        }
        private string GetGridTableHtml(Grid grid)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<meta http-equiv=\"content-type\" content=\"Application/excel; charset=utf-8\"/>");

            sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");

            sb.Append("<tr>");
            foreach (GridColumn column in grid.Columns)
            {
                if (!string.IsNullOrEmpty(column.HeaderText) && column.HeaderText != "详细")
                {
                    sb.AppendFormat("<td>{0}</td>", column.HeaderText);

                }
            }
            sb.Append("</tr>");


            foreach (GridRow row in grid.Rows)
            {
                sb.Append("<tr>");
                int columnIndex = 0;
                foreach (object value in row.Values)
                {
                    string columnId = grid.Columns[columnIndex].ColumnID;
                    if (columnId != "PERSONID" && columnId != "DETAIL")
                    {
                        string html = value.ToString();
                        sb.AppendFormat("<td>{0}</td>", html);
                    }

                    columnIndex++;
                }

                sb.Append("</tr>");
            }

            sb.Append("</table>");

            return sb.ToString();
        }

        //根据字段类型，更新表数据，主要处理自定义列表或者SQL列表型数据
        private void ResetDatatable(DataTable dtSource)
        {
            CHEERSQL hs = new CHEERSQL();
            DataTable dt = hs.ExecuteSQLforDataTable(@"select QDSFIELD,QDSTYPE,QDSVALUELIST from PSNQUERYDETAILSET 
            where QDSTYPE in ('DEFAULTLIST','SQLLIST')  and QDSFIELD in (select QRSDISPLAYFIELD from PSNQUERYRESULTSET) ");

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                string strvl = dt.Rows[i]["QDSVALUELIST"].ToString();
                string fieldid = dt.Rows[i]["QDSFIELD"].ToString();

                if (dt.Rows[i]["QDSTYPE"].ToString() == "DEFAULTLIST")  //【DEFAULTLIST】 2|M - 男,1|F - 女
                {
                    try
                    {
                        Hashtable htDL = new Hashtable();

                        ArrayList arrL = new ArrayList();
                        string[] items = strvl.Split(',');
                        for (int m = 0; m < items.Length; m++)
                        {
                            string[] arrs = items[m].Split('|');
                            if (arrs.Length == 2)
                            {
                                if (!htDL.ContainsKey(arrs[0]))
                                {
                                    htDL.Add(arrs[0], arrs[1]);
                                }
                            }
                        }

                        if (dtSource.Columns.Contains(fieldid))    //更新字段Value到Text
                        {
                            string newfield = fieldid + "_bak";
                            dtSource.Columns.Add(newfield);

                            for (int m = 0; m < dtSource.Rows.Count; m++)
                            {
                                string key = dtSource.Rows[m][fieldid].ToString();
                                if (htDL.ContainsKey(key))
                                {
                                    dtSource.Rows[m][newfield] = htDL[key].ToString();
                                }
                            }

                            dtSource.Columns.Remove(fieldid);
                            dtSource.Columns[newfield].ColumnName = fieldid;
                        }

                    }
                    catch
                    {
                    }
                }
                else  //【SQLLIST】 select itemid,itemvalue from PSNPUBLICCODEITEM where TYPEID='30'
                {
                    try
                    {
                        DataTable dtSL = hs.ExecuteSQLforDataTable(strvl);
                        Hashtable htSL = new Hashtable();
                        for (int n = 0; n < dtSL.Rows.Count; n++)
                        {
                            if (!htSL.ContainsKey(dtSL.Rows[n][0].ToString()))
                            {
                                htSL.Add(dtSL.Rows[n][0].ToString(), dtSL.Rows[n][1].ToString());
                            }
                        }

                        if (dtSource.Columns.Contains(fieldid))    //更新字段Value到Text
                        {
                            string newfield = fieldid + "_bak";
                            dtSource.Columns.Add(newfield);

                            for (int m = 0; m < dtSource.Rows.Count; m++)
                            {
                                string key = dtSource.Rows[m][fieldid].ToString();
                                if (htSL.ContainsKey(key))
                                {
                                    dtSource.Rows[m][newfield] = htSL[key].ToString();
                                }
                            }

                            dtSource.Columns.Remove(fieldid);
                            dtSource.Columns[newfield].ColumnName = fieldid;
                        }
                    }
                    catch
                    { }
                }
            }

        }

        private void ResetBranch(DataTable dtSource)
        {
            Hashtable _branchhst = ((AdjustManager)eHRPageServer.GetPalauObject(typeof(CHEER.BusinessLayer.ePersonnel.Adjust.AdjustManager))).GetAllDeptNameHashtable();
            if (dtSource.Columns.Contains("BRANCHID"))
            {//有组织列的时候更新组织数据
                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    foreach (string str in _branchhst.Keys)
                    {
                        if (dtSource.Rows[i]["BRANCHID"].ToString() == str)
                            dtSource.Rows[i]["BRANCHID"] = _branchhst[str];
                    }
                }

            }
        }

        protected void grdMain_RowCommand(object sender, CheerUI.GridCommandEventArgs e)
        {
            if (e != null)
            {
                string personid = grdMain.Rows[e.RowIndex].Cells.FromKey("PERSONID").ToString();
                if (e.CommandName == "DETAIL")
                {
                    var iframeUrl = "PSNBasicQuery.aspx?PERSONID=" + personid;
                    CheerUI.PageContext.RegisterStartupScript(this.detailWindow.GetShowReference(iframeUrl, "详细"));
                    //CheerUI.PageContext.Redirect("PSNBasicQuery.aspx?PERSONID=" + personid);
                }
            }
        }

        

        protected void btnPass_Click(object sender, EventArgs e)
        {
            if (grdMain.SelectedRowIndexArray.Length == 0)
            {
                CheerUI.Alert.ShowInTop("请选择要生效的配送员！");
                return;
            }
            Prompt prompt = new Prompt();
            prompt.Message = "生效备注：";
            prompt.Title = "请输入";
            prompt.DefaultValue = "";
            prompt.MultiLine = true;
            prompt.MultiLineHeight = 200;
            prompt.Width = 300;
            prompt.MaxWidth = 600;
            prompt.MinWidth = 300;
            prompt.OkScript = PManager.GetCustomEventReference("'Prompt$'+arguments[0]", false, true);
            prompt.Show();
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
           
            if (grdMain.SelectedRowIndexArray.Length == 0)
            {
                CheerUI.Alert.ShowInTop("请选择要驳回的配送员！");
                return;
            }
            Prompt prompt = new Prompt();
            prompt.Message = "驳回备注：";
            prompt.Title = "请输入";
            prompt.DefaultValue = "";
            prompt.MultiLine = true;
            prompt.MultiLineHeight = 200;
            prompt.Width = 300;
            prompt.MaxWidth = 600;
            prompt.MinWidth = 300;
            prompt.OkScript = PManager.GetCustomEventReference("'Prompts$'+arguments[0]", false, true);
            prompt.Show();
        }

        protected void detailWindow_Close(object sender, WindowCloseEventArgs e)
        {
            LoadData(0);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
         
            if (grdMain.SelectedRowIndexArray.Length == 0)
            {
                CheerUI.Alert.ShowInTop("请选择要开除的人员！");
                return;
            }
            Prompt prompt = new Prompt();
            prompt.Message = "开除备注：";
            prompt.Title = "请输入";
            prompt.DefaultValue = "";
            prompt.MultiLine = true;
            prompt.MultiLineHeight = 200;
            prompt.Width = 300;
            prompt.MaxWidth = 600;
            prompt.MinWidth = 300;
            prompt.OkScript = PManager.GetCustomEventReference("'Promptes$'+arguments[0]", false, true);
            prompt.Show();

        }
        protected void Deletebtn_Click(object sender, EventArgs e)
        {
            var selectedRows = grdMain.SelectedRowIndexArray;
            var selectedIds = new List<string>();
            foreach (var selectedRow in selectedRows)
            {
                var id = grdMain.Rows[selectedRow].Cells.FromKey("PERSONID").ToString();
                selectedIds.Add(id);
            }

            if (selectedIds.Count == 0)
            {
                Alert.ShowInTop("请至少选择一项!");
                return;
            }

           CommonMethod.ExecuteFunc(_broker =>
            {
                _broker.ExecuteNonQuery($"delete from psnaccount   WHERE PERSONID IN ('{ string.Join("', '", selectedIds) }')");
                Alert.ShowInTop("删除成功!");
                LoadData(0);
            }, ex =>
            {
                Alert.ShowInTop("删除失败!");
            });
        }
    }
}