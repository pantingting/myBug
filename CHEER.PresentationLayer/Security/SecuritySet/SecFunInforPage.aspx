<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SecFunInforPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.SecuritySet.SecFunInforPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="../../script/mergeCells.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <c:PageManager runat="server" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" AutoSizePanelID="Panel1" />
        <c:Panel runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit" ID="Panel1" >
            <Toolbars>
                <c:Toolbar runat="server">
                    <Items>
                        <c:Button runat="server" ID="cmdConfirm" Text="确定" Icon="PageBack" OnClick="cmdConfirm_Click"></c:Button>
                    </Items>
                </c:Toolbar>
            </Toolbars>
            <Items>
                <c:Grid runat="server" ID="UlFunSecGrid" ShowBorder="false" ShowHeader="false" EnableColumnLines="true"></c:Grid>
            </Items>
        </c:Panel>
    </form>
    <script>
        Ext.onReady(function () {
            mergeCells(F('<%=UlFunSecGrid.ClientID%>'), [1], [2]);
        });
        
    </script>
</body>
</html>
