<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BchEditPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.PackageSet.BchEditPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AutoSizePanelID="Panel1" FormMessageTarget="Qtip" AjaxLoadingType="Mask" />
        <c:Panel runat="server" ID="Panel1" ShowBorder="false" ShowHeader="false" BodyPadding="5px" Layout="Fit">
            <Items>
                <c:Grid runat="server" ID="UlSecPersonGrid" IsDatabasePaging="true" EnableColumnLines="true" AllowPaging="true" ShowHeader="false">
                    <Toolbars>
                        <c:Toolbar runat="server">
                            <Items>
                                <c:Button runat="server" ID="cmdAdd" Text="新增" IconFont="Plus"></c:Button>
                                <c:Button runat="server" ID="cmdDelete" Text="删除" IconFont="Remove" OnClick="cmdDelete_Click"></c:Button>
                            </Items>
                        </c:Toolbar>
                    </Toolbars>
                </c:Grid>
            </Items>
        </c:Panel>
    </form>
    <c:HiddenField runat="server" ID="txtPackageID"></c:HiddenField>
    <c:HiddenField runat="server" ID="txtRightID"></c:HiddenField>
    <c:Window runat="server" ID="BranchDisTab" WindowPosition="Center" Target="Top" EnableIFrame="true" Hidden="true" Height="460px" Width="680px" CloseAction="HidePostBack" OnClose="BranchDisTab_Close"></c:Window>
    <script>
        C.ready(function () {
            C('<%=cmdAdd.ClientID%>').on('click', function () {
                var packageid = C('<%=txtPackageID.ClientID%>').getValue();
                var rightid = C('<%=txtRightID.ClientID%>').getValue();
                C('<%=BranchDisTab.ClientID%>').c_show('<%=getBaseUrl()%>Security/PackageSet/BranchDisTab.aspx?PACKAGEID=' + packageid + '&RIGHTID=' + rightid, '<%=base.getString("ZGAIA00524")%>');

            });
        });

    </script>
</body>
</html>
