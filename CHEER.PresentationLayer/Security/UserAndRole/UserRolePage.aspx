<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserRolePage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.UserAndRole.UserRolePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AutoSizePanelID="Panel" FormMessageTarget="Qtip" AjaxLoadingType="Mask" />
        <c:Panel runat="server" BodyPadding="5px" ID="Panel" Layout="Fit" ShowBorder="false" ShowHeader="false">
            <Items>
                <c:Grid runat="server" AllowPaging="true" EnableColumnLines="true" ID="UlRoleGrid" ShowHeader="false">
                    <Toolbars>
                        <c:Toolbar runat="server">
                            <Items>
                                <c:Button runat="server" ID="btnAdd" IconFont="Plus" Text="新增"></c:Button>
                                <c:Button runat="server" ID="btnDelete" IconFont="Remove" Text="删除" OnClick="btnDelete_Click"></c:Button>
                            </Items>
                        </c:Toolbar>
                    </Toolbars>
                </c:Grid>
            </Items>
        </c:Panel>
    </form>
    <c:HiddenField runat="server" ID="txtUserID"></c:HiddenField>
    <c:Window runat="server" ID="RoleSelectPage" WindowPosition="Center" Target="Top" EnableIFrame="true" Hidden="true" Height="420px" Width="600px" CloseAction="HidePostBack" OnClose="RoleSelectPage_Close"></c:Window>
    <script>
        C.ready(function () {
            C('<%=btnAdd.ClientID%>').on('click', function () {
                return openaddpage();
            });
        });

        function openaddpage() {
            var userid = C('<%=txtUserID.ClientID%>').getValue();
            C('<%=RoleSelectPage.ClientID%>').c_show('<%=getBaseUrl()%>Security/UserAndRole/RoleSelectPage.aspx?USERID=' + userid, '<%="角色选取"%>');
            return false;
        }

    </script>
</body>
</html>
