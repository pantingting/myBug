<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleUserPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.UserAndRole.RoleUserPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AutoSizePanelID="Panel" />
        <c:Panel BodyPadding="5px" runat="server" ID="Panel" Layout="Fit" ShowBorder="false" ShowHeader="false">
            <Items>
                <c:Grid runat="server" OnPageIndexChange="UlRoleGrid_PageIndexChange" IsDatabasePaging="true" AllowPaging="true" EnableCheckBoxSelect="true" EnableColumnLines="true" ID="UlRoleGrid" ShowHeader="false">
                    <Toolbars>
                        <c:Toolbar runat="server">
                            <Items>
                                <c:Button runat="server" ID="btnAdd" IconFont="Plus" Text="新增"></c:Button>
                                <c:Button runat="server" ID="btnDelete" IconFont="Remove" Text="删除" OnClientClick="setValue();" OnClick="btnDelete_Click"></c:Button>
                            </Items>
                        </c:Toolbar>
                    </Toolbars>
                </c:Grid>
            </Items>
        </c:Panel>
        <asp:HiddenField runat="server" ID="selectedRowIndexArray" />
        <c:HiddenField runat="server" ID="txtRoleID"></c:HiddenField>
    </form>
    <c:Window runat="server" ID="RoleSelectPage" WindowPosition="Center" Target="Top" EnableIFrame="true" Hidden="true" Height="420px" Width="600px" CloseAction="HidePostBack"></c:Window>
    <script>
        C.ready(function () {
            C('<%=btnAdd.ClientID%>').on('click', function () {
                return openaddpage();
            });
        });

        function setValue() {
            var a = C('<%=UlRoleGrid.ClientID%>');
            var indexs = [];
            $.each(a.alldata, (i,val) => {
                $.each(a.getSelectedRows(), (j,r) =>{
                    if (val.id == r) {
                        indexs.push(i);
                    }
                })
            });
            $('#<%=selectedRowIndexArray.ClientID%>')[0].value = indexs.join(',');
        }

        function openaddpage() {
            var userid = C('<%=txtRoleID.ClientID%>').getValue();
            C('<%=RoleSelectPage.ClientID%>').c_show('<%=getBaseUrl()%>Security/UserAndRole/UserSelectPage.aspx?ROLEID=' + userid, '<%="用户选取"%>');
            return false;
        }
    </script>
</body>
</html>
