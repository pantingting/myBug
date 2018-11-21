using System;
using System.Data;
using CHEER.PresentationLayer;
using System.Collections;
using System.Web.UI.WebControls;
using System.Drawing;
using CHEER.Platform.DAL;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CHEER.PresentationLayer.CommonUse
{
    public class CommonMethod
    {
        /// <summary>
        /// 使用方式如下
        /// Common.Business.ExecuteSQL(_broker => {
        ///     DataTable dt = _broker.ExecuteSQLForDst(sql).Tables[0];
        /// }, ex => {
        ///     Console.WriteLine(ex.Message);
        /// });
        /// </summary>
        /// <param name="successDelegate">SuccessDelegate</param>
        /// <param name="failDelegate">failDelegate</param>
        public static void ExecuteFunc(Action<PersistBroker> successDelegate, Action<Exception> failDelegate)
        {
            PersistBroker _broker = PersistBroker.Instance();
            try
            {
                successDelegate(_broker);
            }
            catch(Exception ex)
            {
                failDelegate(ex);
            }
            finally
            {
                _broker.Close();
            }
        }

        public static CheerUI.LinkButtonField AddAddField(CheerUI.Grid grid, string headerText, string dataField, bool hidden, bool locked)
        {
            return AddLinkButtonField(grid, headerText, dataField, 45, hidden, locked, "", CheerUI.IconFont.Plus);
        }

        public static CheerUI.LinkButtonField AddDetailField(CheerUI.Grid grid, string headerText, string dataField, bool hidden, bool locked)
        {
            return AddLinkButtonField(grid, headerText, dataField, 45, hidden, locked, "", CheerUI.IconFont.Edit);
        }

        public static CheerUI.LinkButtonField AddDeleteField(CheerUI.Grid grid, string headerText, string dataField, bool hidden, bool locked)
        {
            return AddLinkButtonField(grid, headerText, dataField, 45, hidden, locked, "", CheerUI.IconFont.Remove);
        }

        public static CheerUI.LinkButtonField AddLinkButtonField(CheerUI.Grid grid, string headerText, string dataField, int width, bool hidden, bool locked, string toolTip, CheerUI.IconFont iconFont)
        {
            CheerUI.LinkButtonField lbf = new CheerUI.LinkButtonField();
            lbf.HeaderText = headerText;
            lbf.ColumnID = dataField;
            width = width < 50 ? 50 : width;
            lbf.Width = new Unit(width);
            lbf.Locked = locked;
            lbf.Hidden = hidden;
            lbf.IconFont = iconFont;
            lbf.TextAlign = CheerUI.TextAlign.Center;
            if (hidden)
            {
                lbf.EnableColumnHide = false;
            }
            if (toolTip != "")
            {
                lbf.ToolTip = toolTip;
            }
            grid.Columns.Add(lbf);
            return lbf;
        }

        public static CheerUI.BoundField AddField(CheerUI.Grid grid, string headerText, string dataField, int width, bool hidden)
        {
            return AddField(grid, headerText, dataField, width, hidden, false);
        }

        public static CheerUI.BoundField AddField(CheerUI.Grid grid, string headerText, string dataField, int width, bool hidden, bool locked)
        {
            return AddField(grid, headerText, dataField, width, hidden, locked, false);
        }

        public static CheerUI.BoundField AddField(CheerUI.Grid grid, string headerText, string dataField, int width, bool hidden, bool locked, bool showToolTip)
        {
            CheerUI.BoundField bf = new CheerUI.BoundField();
            bf.HeaderText = headerText;
            bf.DataField = dataField;
            bf.ColumnID = dataField;
            bf.SortField = dataField;
            bf.Width = new Unit(width);
            bf.Locked = locked;
            bf.Hidden = hidden;
            if (hidden)
            {
                bf.EnableColumnHide = false;
            }
            if (showToolTip)
            {
                bf.DataToolTipField = dataField;
            }
            grid.Columns.Add(bf);
            return bf;
        }

        public static CheerUI.BoundField AddFlexField(CheerUI.Grid grid, string headerText, string dataField, int flex, bool hidden)
        {
            return AddFlexField(grid, headerText, dataField, flex, hidden, false);
        }

        public static CheerUI.BoundField AddFlexField(CheerUI.Grid grid, string headerText, string dataField, int flex, bool hidden, bool locked)
        {
            return AddFlexField(grid, headerText, dataField, flex, hidden, locked, false);
        }

        public static CheerUI.BoundField AddFlexField(CheerUI.Grid grid, string headerText, string dataField, int flex, bool hidden, bool locked, bool showToolTip)
        {
            CheerUI.BoundField bf = new CheerUI.BoundField();
            bf.HeaderText = headerText;
            bf.DataField = dataField;
            bf.ColumnID = dataField;
            bf.SortField = dataField;
            bf.Hidden = hidden;
            bf.BoxFlex = flex;
            bf.Locked = locked;
            if (hidden)
            {
                bf.EnableColumnHide = false;
            }
            if (showToolTip)
            {
                bf.DataToolTipField = dataField;
            }
            grid.Columns.Add(bf);
            return bf;
        }

        public static CheerUI.RenderField AddRendererField(CheerUI.Grid grid, string headerText, string dataField, int width, string rendererFunction, bool hidden)
        {
            return AddRendererField(grid, headerText, dataField, width, rendererFunction, hidden, false);
        }

        public static CheerUI.RenderField AddRendererField(CheerUI.Grid grid, string headerText, string dataField, int width, string rendererFunction, bool hidden, bool locked)
        {
            CheerUI.RenderField rf = new CheerUI.RenderField();
            rf.HeaderText = headerText;
            rf.DataField = dataField;
            rf.ColumnID = dataField;
            rf.SortField = dataField;
            rf.Width = width;
            rf.Locked = locked;
            rf.Hidden = hidden;
            if (hidden)
            {
                rf.EnableColumnHide = false;
            }
            rf.RendererFunction = rendererFunction;
            grid.Columns.Add(rf);
            return rf;
        }

        public static CheerUI.RenderField AddFlexRendererField(CheerUI.Grid grid, string headerText, string dataField, int flex, string rendererFunction, bool hidden)
        {
            return AddFlexRendererField(grid, headerText, dataField, flex, rendererFunction, hidden, false);
        }

        public static CheerUI.RenderField AddFlexRendererField(CheerUI.Grid grid, string headerText, string dataField, int flex, string rendererFunction, bool hidden, bool locked)
        {
            CheerUI.RenderField rf = new CheerUI.RenderField();
            rf.HeaderText = headerText;
            rf.DataField = dataField;
            rf.ColumnID = dataField;
            rf.SortField = dataField;
            rf.BoxFlex = flex;
            rf.Hidden = hidden;
            rf.Locked = locked;
            if (hidden)
            {
                rf.EnableColumnHide = false;
            }
            rf.RendererFunction = rendererFunction;
            grid.Columns.Add(rf);
            return rf;
        }

        public static CheerUI.RenderCheckField AddFlexRenderCheckField(CheerUI.Grid grid, string headerText, string dataField, int flex, bool hidden, bool locked)
        {
            CheerUI.RenderCheckField rf = new CheerUI.RenderCheckField();
            rf.HeaderText = headerText;
            rf.DataField = dataField;
            rf.ColumnID = dataField;
            rf.SortField = dataField;
            rf.BoxFlex = flex;
            rf.Locked = locked;
            if (hidden)
            {
                rf.EnableColumnHide = false;
            }
            grid.Columns.Add(rf);
            return rf;
        }

        public static CheerUI.RenderCheckField AddRenderCheckField(CheerUI.Grid grid, string headerText, string dataField, int width, bool hidden, bool locked)
        {
            CheerUI.RenderCheckField rf = new CheerUI.RenderCheckField();
            rf.HeaderText = headerText;
            rf.DataField = dataField;
            rf.ColumnID = dataField;
            rf.SortField = dataField;
            rf.Width = width;
            rf.Locked = locked;
            if (hidden)
            {
                rf.EnableColumnHide = false;
            }
            grid.Columns.Add(rf);
            return rf;
        }

        public static CheerUI.RenderCheckField AddRenderCheckField(CheerUI.Grid grid, string headerText, string dataField, int width, bool hidden)
        {
            return AddRenderCheckField(grid, headerText, dataField, width, hidden, false);
        }

        public static CheerUI.RenderCheckField AddFlexRenderCheckField(CheerUI.Grid grid, string headerText, string dataField, int flex, bool hidden)
        {
            return AddFlexRenderCheckField(grid, headerText, dataField, flex, hidden, false);
        }

        public static CheerUI.WindowField AddWindowField(CheerUI.Grid grid, string headerText, string dataField, int width, string windowId, string urlFields, string url, string title)
        {
            CheerUI.WindowField wf = new CheerUI.WindowField();
            wf.HeaderText = headerText;
            wf.ColumnID = dataField;
            wf.Width = new Unit(width);
            wf.IconFont = CheerUI.IconFont.Edit;
            wf.WindowID = windowId;
            wf.Title = title;
            wf.DataIFrameUrlFields = urlFields;
            wf.DataIFrameUrlFormatString = url;
            wf.TextAlign = CheerUI.TextAlign.Center;
            grid.Columns.Add(wf);
            return wf;
        }

        public static string GenerateInfoLabel(int level, string parentId, PersistBroker _broker)
        {
            string sql = "SELECT IFNULL(MAX(label),'0') AS label FROM zsh_course_category WHERE level = " + level + " AND pid = '" + parentId + "';";
            string plsql = "SELECT IFNULL(label,'') AS label FROM zsh_course_category WHERE id = '" + parentId + "'";
            string Maxlabel = _broker.ExecuteSQLForDst(sql).Tables[0].Rows[0][0].ToString();
            DataTable dt = _broker.ExecuteSQLForDst(plsql).Tables[0];
            string Plabel = "";
            if (dt.Rows.Count > 0)
            {
                Plabel = dt.Rows[0][0].ToString();
            }

            if (Plabel == "")
            {
                if (Maxlabel == "0")
                {
                    return "0001";
                }
                else
                {
                    return (int.Parse(Maxlabel) + 1).ToString().PadLeft(4, '0');
                }
            }
            else
            {
                if (Maxlabel == "0")
                {
                    return Plabel + "0001";
                }
                else
                {
                    int lab = int.Parse(Maxlabel.Substring(Maxlabel.Length - 4)) + 1;
                    return Plabel + lab.ToString().PadLeft(4, '0');
                }
            }
        }

        /// <summary>
        /// 根据配置获取单号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetBillNO(string id)
        {
            var billNo = "";
            CommonMethod.ExecuteFunc(_broker => {
                var sql = $@"SELECT prefix,date_number,random_number FROM sup_settings WHERE id = '{id}'";
                var dt = _broker.ExecuteSQLForDst(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    var random_number = Convert.ToInt32(dt.Rows[0]["random_number"]);
                    var date_number = Convert.ToInt32(dt.Rows[0]["date_number"]);
                    var date = date_number == 0 ? "" : DateTime.Now.ToString("yyyyMMdd").Substring(0, date_number);
                    var randomNo = Common.GenerateCheckCodeNum(random_number);
                    billNo = $"{dt.Rows[0]["prefix"]}{date}{randomNo}";
                }
            }, ex => {

            });
            return billNo;
        }
    }
}


