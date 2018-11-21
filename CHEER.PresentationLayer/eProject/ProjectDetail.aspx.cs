using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CHEER.PresentationLayer.CommonUse;

namespace CHEER.PresentationLayer.eProject
{
    public partial class ProjectDetail : CHEERBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    CommonMethod.ExecuteFunc(_broker => {
                        var dt = _broker.ExecuteSQLForDst($"select * from bug_project where id={Request.QueryString["id"]}").Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            name.Text = dt.Rows[0]["name"]?.ToString();
                            startTime.Text = dt.Rows[0]["startTime"]?.ToString();
                        }
                    }, ex => {
                        ShowAlert(ex.Message);
                    });
                }
            }
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            CommonMethod.ExecuteFunc(_broker => {
                var sql = "";
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    sql = $"update bug_project set name='{name.Text.Trim()}',startTime='{startTime.Text}' where id={Request.QueryString["id"]}";
                }
                else
                {
                    sql = $"insert  into bug_project (name,startTime,createTime) values('{name.Text.Trim()}','{startTime.Text}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')";
                }
                _broker.ExecuteNonQuery(sql);
                CheerUI.Alert.ShowInTop("保存成功！", "提示", CheerUI.MessageBoxIcon.Success, CheerUI.ActiveWindow.GetHidePostBackReference());
            }, ex => {
                ShowAlert(ex.Message);
            });
        }
    }
}