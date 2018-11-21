<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleCopyTabPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.SuperWork.RoleCopyTabPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" AutoSizePanelID="Panel1" />
        <c:Panel BodyPadding="5px" runat="server" ID="Panel1" ShowBorder="false" ShowHeader="false" Layout="Fit">
            <Items>
                <c:TabStrip runat="server" ID="UlRoleTab">
                    <Toolbars>
                        <c:Toolbar runat="server">
                            <Items>
                                <c:Button runat="server" ID="cmdReturn" Text="返回" Icon="PageBack"></c:Button>
                            </Items>
                        </c:Toolbar>
                    </Toolbars>
                    <Tabs>
                    </Tabs>
                </c:TabStrip>
            </Items>
        </c:Panel>
    </form>
    <script>
        C.ready(function () {
            C('<%=cmdReturn.ClientID%>').on('click', function () {
                document.location.replace("../UserAndRole/RoleMaintainPage.aspx?BACK=Back");
            });
        });
    </script>
</body>
</html>
