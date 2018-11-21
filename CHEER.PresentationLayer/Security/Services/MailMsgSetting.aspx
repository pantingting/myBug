<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MailMsgSetting.aspx.cs" Inherits="CHEER.PresentationLayer.Security.Services.MailMsgSetting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <c:PageManager runat="server" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" AutoSizePanelID="Panel1" />
        <c:Panel runat="server" ID="Panel1" ShowBorder="false" ShowHeader="false" Layout="Fit">
            <Toolbars>
                <c:Toolbar runat="server">
                    <Items>
                        <c:Button runat="server" ID="btnClose" Text="关闭窗口" Icon="SystemClose" OnClick="btnClose_Click"></c:Button>
                    </Items>
                </c:Toolbar>
            </Toolbars>
            <Items>
                <c:TabStrip runat="server" ID="UltDepTab" ShowBorder="false" ></c:TabStrip>
            </Items>
        </c:Panel>
    </form>
</body>
</html>
