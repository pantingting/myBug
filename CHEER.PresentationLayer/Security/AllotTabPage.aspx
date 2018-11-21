<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllotTabPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.AllotTabPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <c:PageManager runat="server" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" AutoSizePanelID="UlUserTab" />
        <c:TabStrip runat="server" ID="UlUserTab" ShowBorder="false">
            <Tabs></Tabs>
        </c:TabStrip>
    </form>
</body>
</html>
