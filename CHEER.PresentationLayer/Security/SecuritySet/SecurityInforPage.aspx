<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SecurityInforPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.SecuritySet.SecurityInforPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" AutoSizePanelID="Panel1"/>
        <c:Panel runat="server" Layout="Fit" ID="Panel1" ShowBorder="false" ShowHeader="false">
            <Items>
                <c:TabStrip runat="server" ID="UlSecInforTab" ShowBorder="false">
                    <Tabs></Tabs>
                </c:TabStrip>
            </Items>
        </c:Panel>

    </form>
</body>
</html>
