using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.PresentationLayer.CommonUse;
using CheerUI;

namespace CHEER.PresentationLayer.eProject
{
    public partial class ProjectList : CHEERBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            CommonMethod.AddField(mainGrid,"","id",0,true);
            CommonMethod.AddFlexField(mainGrid,"项目名称","name",1,false);
            CommonMethod.AddFlexField(mainGrid, "开始日期", "startTime", 1, false);
            CommonMethod.AddFlexField(mainGrid, "创建时间", "createTime", 1, false);
        }
        protected void BindData()
        {
            CommonMethod.ExecuteFunc(_broker=> {
                var sql = "SELECT * from bug_project order by createTime desc";
                BindPagedDataTable(sql,mainGrid,_broker);
            },ex=> {
                ShowAlert(ex.Message);
            });
        }

        protected void addBtn_Click(object sender, EventArgs e)
        {
            detailWindow.Show("ProjectDetail.aspx", "新增",IconFont.None);
        }

        protected void delBtn_Click(object sender, EventArgs e)
        {
            var selectedRows = mainGrid.SelectedRowIndexArray;
            var selectedIds = new List<string>();
            foreach (var selectedRow in selectedRows)
            {
                var id = mainGrid.Rows[selectedRow].Cells.FromKey("id").ToString();
                selectedIds.Add(id);
            }

            if (selectedIds.Count == 0)
            {
                ShowAlert("请至少选择一项!");
                return;
            }

            CommonMethod.ExecuteFunc(_broker =>
            {
                _broker.ExecuteNonQuery($"delete from bug_project  WHERE id IN ('{ string.Join("', '", selectedIds) }')");
                Alert.ShowInTop("删除成功!");
                BindData();
            }, ex =>
            {
                Alert.ShowInTop("删除失败!");
            });
        }

        protected void detailWindow_Close(object sender, WindowCloseEventArgs e)
        {
            BindData();
        }
    }
}